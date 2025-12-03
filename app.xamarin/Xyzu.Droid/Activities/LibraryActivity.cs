using Android.Animation;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.SwipeRefreshLayout.Widget;

using Com.Sothree.Slidinguppanel;

using Google.Android.Material.AppBar;
using Google.Android.Material.FloatingActionButton;

using System;
using System.ComponentModel;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library;
using Xyzu.Menus;
using Xyzu.Settings.UserInterface;
using Xyzu.Views.Insets;
using Xyzu.Views.NowPlaying;
using Xyzu.Views.Toolbar;

namespace Xyzu.Activities
{
	public partial class LibraryActivity : BaseActivity, SwipeRefreshLayout.IOnRefreshListener
	{
		public static class BundleKeys
		{
			public const string SlidingUpPanel_State = "BundleKeys_SlidingUpPanel_State";
		}
		public static class IntentKeys
		{
			public const string IsFromNotification = "IntentKey_IsFromNotification";
			public const string IsFromWidget = "IntentKey_IsFromWidget";
		}

		protected StatusBarInsetView? _StatusBarPrimary;
		protected StatusBarInsetView? _StatusBarSurface;
		protected AppBarLayout? _Appbarlayout;
		protected ToolbarSearchView? _ToolbarSearch;
		protected FloatingActionButton? _Floatingactionbutton;
		protected IUserInterfaceSettingsDroid? _UserInterfaceSettings;

		protected virtual int AppbarlayoutResourceId { get; }
		protected virtual int ToolbarSearchResourceId { get; }
		protected virtual int FloatingactionbuttonResourceId { get; }
		public IUserInterfaceSettingsDroid UserInterfaceSettings
		{
			get => _UserInterfaceSettings ??= XyzuSettings.Instance.GetUserInterfaceDroid();
		}

		public StatusBarInsetView? StatusBarPrimary
		{
			get => _StatusBarPrimary;
			set
			{
				_StatusBarPrimary = value;

				StatusBarPrimaryAnimator ??= ValueAnimator.OfFloat(0F, 0.2F, 0.5F, 0.2F, 0F);

				if (StatusBarPrimaryAnimator is not null)
				{
					StatusBarPrimaryAnimator.SetDuration(2_000);
					StatusBarPrimaryAnimator.AddListener(new AnimatorListener
					{
						OnAnimationEndAction = animator => 
						{
							if (XyzuLibrary.Instance.ScannerServiceConnectionState == ServiceConnectionChangedEventArgs.Events.Connected)
								StatusBarPrimaryAnimator?.Start();
						}
					});
					StatusBarPrimaryAnimator.AddUpdateListener(new AnimatorUpdateListener
					{
						OnAnimationUpdateAction = valueanimator =>
						{
							if (StatusBarPrimary is not null && valueanimator?.AnimatedValue is Java.Lang.Float alpha)
								StatusBarPrimary.Alpha = alpha.FloatValue();
						}
					});
				}
			}
		}
		public StatusBarInsetView? StatusBarSurface
		{
			get => _StatusBarSurface;
			set => _StatusBarSurface = value;
		}
		public ValueAnimator? StatusBarPrimaryAnimator { get; set; }

		protected AppBarLayout Appbarlayout
		{
			get => _Appbarlayout ??=
				FindViewById<AppBarLayout>(AppbarlayoutResourceId) ??
				throw new InflateException();
		}
		protected ToolbarSearchView ToolbarSearch
		{
			get => _ToolbarSearch ??=
				FindViewById<ToolbarSearchView>(ToolbarSearchResourceId) ??
				throw new InflateException();
		}  
		protected FloatingActionButton Floatingactionbutton
		{
			get => _Floatingactionbutton ??=
				FindViewById<FloatingActionButton>(FloatingactionbuttonResourceId) ??
				throw new InflateException();
		}

		protected override void OnResume()
		{
			base.OnResume();

			Floatingactionbutton.Click += OnFloatingactionbuttonClick;

			if (Intent != null && Intent.GetBooleanExtra(IntentKeys.IsFromNotification, false))
			{
				SlidingUpPanel?.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
				ViewNowPlaying?.SetTransition(NowPlayingView.Ids.MotionScene.Transitions.Expand);
				ViewNowPlaying?.TransitionToEnd();
				ViewNowPlaying?.PlayerQueuePropertyChanged(this, new PropertyChangedEventArgs(nameof(Player.IQueue.CurrentIndex)));
			}
		}
		protected override void OnPause()
		{
			base.OnPause();

			Floatingactionbutton.Click -= OnFloatingactionbuttonClick;
		}
		protected override void OnSaveInstanceState(Bundle outstate)
		{
			base.OnSaveInstanceState(outstate);

			outstate.PutString(BundleKeys.SlidingUpPanel_State, true switch
			{
				true when SlidingUpPanel?.GetPanelState() == SlidingUpPanelLayout.PanelState.Hidden => "Hidden",
				true when SlidingUpPanel?.GetPanelState() == SlidingUpPanelLayout.PanelState.Dragging => "Dragging",
				true when SlidingUpPanel?.GetPanelState() == SlidingUpPanelLayout.PanelState.Expanded => "Expanded",
				true when SlidingUpPanel?.GetPanelState() == SlidingUpPanelLayout.PanelState.Collapsed => "Collapsed",

				_ => null
			});
		}
		protected override void OnRestoreInstanceState(Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState(savedInstanceState);

			if (XyzuPlayer.Instance.ServiceConnectionState == ServiceConnectionChangedEventArgs.Events.Connected)
				switch (savedInstanceState.GetString(BundleKeys.SlidingUpPanel_State))
				{
					case "Expanded":
						SlidingUpPanel?.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
						ConfigureNowPlayingState(SlidingUpPanelLayout.PanelState.Dragging, SlidingUpPanelLayout.PanelState.Expanded);
						break;
					case "Collapsed":
						SlidingUpPanel?.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);
						ConfigureNowPlayingState(SlidingUpPanelLayout.PanelState.Dragging, SlidingUpPanelLayout.PanelState.Collapsed);
						break;

					case "Hidden": break;
					case "Dragging": break;
					default: break;
				}				
		}
		protected override void OnUiModeChanged(Configuration newConfig)
		{
			base.OnUiModeChanged(newConfig);
		}
		protected override void XyzuLibraryScanServiceUpdate(object? sender, IScanner.ServiceNotification.UpdateEventArgs args)
		{
			if (SupportActionBar is null || XyzuLibrary.Instance.ScanServiceBinder is null)
				return;

			(SupportActionBar.Title, SupportActionBar.Subtitle) = args.SubText is null
				? (GetString(Resource.String.application_label_short) , null)
				: (GetString(Resource.String.scanning),
					XyzuLibrary.Instance.ScanServiceBinder.Service.ScanType switch
					{
						IScanner.ScanTypes.Album => string.Format("{0} [{1}]", GetString(Resource.String.library_albums), args.SubText),
						IScanner.ScanTypes.Artist => string.Format("{0} [{1}]", GetString(Resource.String.library_artists), args.SubText),
						IScanner.ScanTypes.Genre => string.Format("{0} [{1}]", GetString(Resource.String.library_genres), args.SubText),
						IScanner.ScanTypes.Playlist => string.Format("{0} [{1}]", GetString(Resource.String.library_playlists), args.SubText),
						IScanner.ScanTypes.Song => string.Format("{0} [../{1}]", GetString(Resource.String.library_songs), args.SubText.Split('/').Last()),

						_ => string.Format("{0} [{1}]", args.ContentText, args.SubText),
					});
		}
		protected override void XyzuLibraryOnServiceConnectionChanged(object? sender, ServiceConnectionChangedEventArgs args)
		{
			base.XyzuLibraryOnServiceConnectionChanged(sender, args);

			if (XyzuLibrary.Instance.ScanServiceBinder is not null)
				switch (args.Event)
				{
					case ServiceConnectionChangedEventArgs.Events.Connected:
						StatusBarPrimaryAnimator?.Start();
						if (XyzuLibrary.Instance.ScanServiceBinder.Service.Notification is not null)
							XyzuLibrary.Instance.ScanServiceBinder.Service.Notification.OnUpdate += XyzuLibraryScanServiceUpdate;
						break;

					case ServiceConnectionChangedEventArgs.Events.Disconnected:
						StatusBarPrimaryAnimator?.End();
						if (XyzuLibrary.Instance.ScanServiceBinder.Service.Notification is not null)
							XyzuLibrary.Instance.ScanServiceBinder.Service.Notification.OnUpdate -= XyzuLibraryScanServiceUpdate;
						XyzuLibraryScanServiceUpdate(this, new IScanner.ServiceNotification.UpdateEventArgs { });
						break;

					default: break;
				}
		}
		protected override void XyzuPlayerOnServiceConnectionChanged(object? sender, ServiceConnectionChangedEventArgs args)
		{
			base.XyzuPlayerOnServiceConnectionChanged(sender, args);

			if (ViewNowPlaying is null)
				return;

			switch (XyzuPlayer.Instance.ServiceConnectionState)
			{
				case ServiceConnectionChangedEventArgs.Events.Connected:

					ViewNowPlaying.Player = XyzuPlayer.Instance.Player;

					if (SlidingUpPanel?.GetPanelState() is SlidingUpPanelLayout.PanelState panelstate)
						switch (true)
						{
							case true when panelstate == SlidingUpPanelLayout.PanelState.Anchored:
							case true when panelstate == SlidingUpPanelLayout.PanelState.Hidden:
								SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);
								break;

							case true when panelstate == SlidingUpPanelLayout.PanelState.Collapsed:
							case true when panelstate == SlidingUpPanelLayout.PanelState.Expanded:
							case true when panelstate == SlidingUpPanelLayout.PanelState.Dragging:
							default: break;
						}

					ViewNowPlaying.ViewRefresh();

					break;

				case ServiceConnectionChangedEventArgs.Events.Disconnected:
					SlidingUpPanel?.SetPanelState(
						p0: UserInterfaceSettings.NowPlayingForceShow
							? SlidingUpPanelLayout.PanelState.Collapsed
							: SlidingUpPanelLayout.PanelState.Hidden);

					ViewNowPlaying.Player = null;
					ViewNowPlaying.ViewRefresh();

					break;

				default: break;
			}
		}

		public override void OnBackPressed()
		{
			if (CurrentLibraryFragment?.OnBackPressed() ?? false)
				return;

			base.OnBackPressed();
		}
		public override bool OnOptionsItemSelected(IMenuItem menuitem)
		{
			switch ((MenuOptions?)menuitem.ItemId)
			{
				case MenuOptions.Back:
					return true;					 
				case MenuOptions.GoToNowPlaying:
					SlidingUpPanel?.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
					return true;
				case MenuOptions.Rescan:
					ActivityCancellationTokenSourceCancel();
					XyzuLibrary.Instance.ScannerServiceScan(true);
					return true;
				case MenuOptions.Search:
					NavigateSearch();
					return true;
				case MenuOptions.Settings:
					ActivityCancellationTokenSourceCancel();
					StartActivity(typeof(SettingsActivity));
					return true;

				default: return base.OnOptionsItemSelected(menuitem);
			};
		}

		protected virtual void SetStatusBars(int? surface, int? primary)
		{
			if (primary.HasValue)
				StatusBarPrimary =
					FindViewById<StatusBarInsetView>(primary.Value) ??
					throw new InflateException(string.Format("Could not find view '{0}'", primary.Value));

			if (surface.HasValue)
				StatusBarSurface =
					FindViewById<StatusBarInsetView>(surface.Value) ??
					throw new InflateException(string.Format("Could not find view '{0}'", surface.Value));

			XyzuLibraryOnServiceConnectionChanged(this, new ServiceConnectionChangedEventArgs(XyzuLibrary.Instance.ScannerServiceConnectionState));
		}
		protected virtual void OnDialogRequested(DialogFragment? dialogfragment)
		{
			dialogfragment?.SetStyle(DialogFragment.StyleNoFrame, 0);
			dialogfragment?.Show(SupportFragmentManager, string.Empty);
		}
		protected virtual void OnFloatingactionbuttonClick(object? sender, EventArgs args)
		{ }

		public virtual void OnRefresh()
		{ }
	}
}