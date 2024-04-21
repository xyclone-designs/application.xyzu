#nullable enable

using Android.Content;
using Android.Content.Res;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.SwipeRefreshLayout.Widget;

using Com.Sothree.Slidinguppanel;

using Google.Android.Material.AppBar;
using Google.Android.Material.FloatingActionButton;

using System;
using System.ComponentModel;

using Xyzu.Droid;
using Xyzu.Menus;
using Xyzu.Views.NowPlaying;
using Xyzu.Views.Toolbar;

namespace Xyzu.Activities
{
	public partial class LibraryActivity : BaseActivity, SwipeRefreshLayout.IOnRefreshListener
	{
		public static class IntentKeys
		{
			public const string IsFromNotification = "IntentKey_IsFromNotification";
		}

		protected AppBarLayout? _Appbarlayout;
		protected ToolbarSearchView? _ToolbarSearch;
		protected FloatingActionButton? _Floatingactionbutton;

		protected virtual int AppbarlayoutResourceId { get; }
		protected virtual int ToolbarSearchResourceId { get; }
		protected virtual int FloatingactionbuttonResourceId { get; }

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
		protected override void OnUiModeChanged(Configuration newConfig)
		{
			base.OnUiModeChanged(newConfig);
		}
		protected override void XyzuPlayerOnServiceConnectionChanged(object sender, ServiceConnectionChangedEventArgs args)
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

					SlidingUpPanelLayout.PanelState disconnectedpanelstate = ViewNowPlaying.Settings.ForceShowNowPlaying
						? SlidingUpPanelLayout.PanelState.Collapsed
						: SlidingUpPanelLayout.PanelState.Hidden;

					SlidingUpPanel?.SetPanelState(disconnectedpanelstate);

					ViewNowPlaying.ViewReset();

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

		public virtual void OnRefresh()
		{ }

		protected virtual void OnDialogRequested(DialogFragment? dialogfragment)
		{
			dialogfragment?.SetStyle(DialogFragment.StyleNoFrame, 0);
			dialogfragment?.Show(SupportFragmentManager, string.Empty);
		}
		protected virtual void OnFloatingactionbuttonClick(object sender, EventArgs args)
		{ }
	}
}