using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.DrawerLayout.Widget;

using Com.Sothree.Slidinguppanel;

using Google.Android.Material.Navigation;			

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Fragments.Library;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Views.Insets;
using Xyzu.Views.Toolbar;

using JavaRunnable = Java.Lang.Runnable;
using FrameLayout = Android.Widget.FrameLayout;

namespace Xyzu.Activities
{
	[Android.App.Activity(
		Theme = "@style/LibraryTheme",
		WindowSoftInputMode = SoftInput.StateVisible | SoftInput.AdjustResize,
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
	public partial class LibraryActivityDrawerLayout : LibraryActivity, NavigationView.IOnNavigationItemSelectedListener
	{
		protected IDrawerLayoutable[]? _DrawerLayoutables;
		protected IDrawerLayoutable? _CurrentDrawerLayoutable;
		protected LibraryTypes? _CurrentLibraryType;
		protected CoordinatorLayout? _Coordinatorlayout;
		protected DrawerLayout? _Drawerlayout;
		protected NavigationView? _Navigationview;
		protected FrameLayout? _Contentframelayout;
		protected ActionBarDrawerToggle? _Actionbardrawertoggle;
		protected ToolbarDrawerView? _ToolbarDrawer;

		protected IDrawerLayoutable[] DrawerLayoutables
		{
			set => _DrawerLayoutables = value;
			get => _DrawerLayoutables ??= Settings.PagesOrdered
				.Select<LibraryPages, IDrawerLayoutable>(pageordered => pageordered switch
				{
					ILibrarySettingsDroid.Options.Pages.Albums => FragmentLibraryAlbums,
					ILibrarySettingsDroid.Options.Pages.Artists => FragmentLibraryArtists,
					ILibrarySettingsDroid.Options.Pages.Genres => FragmentLibraryGenres,
					ILibrarySettingsDroid.Options.Pages.Playlists => FragmentLibraryPlaylists,
					ILibrarySettingsDroid.Options.Pages.Queue => FragmentLibraryQueue,
					ILibrarySettingsDroid.Options.Pages.Songs => FragmentLibrarySongs,

					_ => throw new ArgumentException(string.Format("Invalid LibraryPages '{0}' in LibraryActivityDrawerLayout.DrawerLayoutables.Get", pageordered))

				}).ToArray();
		}
		protected IDrawerLayoutable? CurrentDrawerLayoutable
		{
			get => _CurrentDrawerLayoutable;
			set
			{
				if (_CurrentDrawerLayoutable != null)
					_CurrentDrawerLayoutable.OnReconfigure -= OnReconfigure;

				PreviousDrawerLayoutable = _CurrentDrawerLayoutable;
				_CurrentDrawerLayoutable = value;
				CurrentLibraryFragment = _CurrentDrawerLayoutable as LibraryFragment;

				if (_CurrentDrawerLayoutable != null)
					_CurrentDrawerLayoutable.OnReconfigure += OnReconfigure;

				OnPropertyChanged();
				OnPropertyChanged(nameof(PreviousDrawerLayoutable));
			}
		}
		protected IDrawerLayoutable? PreviousDrawerLayoutable
		{
			get; set;
		}

		protected override int AppbarlayoutResourceId
		{
			get => Resource.Id.xyzu_layout_library_drawerlayout_appbarlayout;
		}
		protected override int ToolbarSearchResourceId
		{ 
			get => Resource.Id.xyzu_layout_library_drawerlayout_toolbarsearchview;
		}				
		protected override int FloatingactionbuttonResourceId
		{ 
			get => Resource.Id.xyzu_layout_library_drawerlayout_floatingactionbutton;
		}

		public CoordinatorLayout Coordinatorlayout 
		{ 
			get => _Coordinatorlayout ??= 
				FindViewById<CoordinatorLayout>(Resource.Id.xyzu_layout_library_drawerlayout_root_coordinatorlayout) ?? 
				throw new InflateException("Could not find view 'xyzu_layout_library_drawerlayout_root_coordinatorlayout' in 'layout_library_drawerlayout'");
		}
		public DrawerLayout Drawerlayout 
		{ 
			get => _Drawerlayout ??=
				FindViewById<DrawerLayout>(Resource.Id.xyzu_layout_library_drawerlayout_drawerlayout) ??
				throw new InflateException("Could not find view 'xyzu_layout_library_drawerlayout_drawerlayout' in 'layout_library_drawerlayout'");
		}
		public NavigationView Navigationview 
		{ 
			get => _Navigationview ??=
				FindViewById<NavigationView>(Resource.Id.xyzu_layout_library_drawerlayout_navigationview) ??
				throw new InflateException("Could not find view 'xyzu_layout_library_drawerlayout_navigationview' in 'layout_library_drawerlayout'");
		}
		public FrameLayout Contentframelayout 
		{ 
			get => _Contentframelayout ??=
				FindViewById<FrameLayout>(Resource.Id.xyzu_layout_library_drawerlayout_framelayout) ??
				throw new InflateException("Could not find view 'xyzu_layout_library_drawerlayout_framelayout' in 'layout_library_drawerlayout'");
		}
		public ActionBarDrawerToggle Actionbardrawertoggle 
		{ 
			get => _Actionbardrawertoggle ??= 
				new ActionBarDrawerToggle(this, Drawerlayout, ActivityToolbar, Resource.String.open, Resource.String.close);
		}
		public ToolbarDrawerView ToolbarDrawer
		{
			get => _ToolbarDrawer ??=
				FindViewById<ToolbarDrawerView>(Resource.Id.xyzu_layout_library_drawerlayout_toolbardrawerview) ??
				throw new InflateException("Could not find view 'xyzu_layout_library_drawerlayout_toolbardrawerview' in 'layout_library_drawerlayout'");
		}

		void ToDrawerLayoutable(IDrawerLayoutable? drawerlayoutable)
		{
			drawerlayoutable ??= CurrentDrawerLayoutable ?? Settings.PageDefault switch
			{
				ILibrarySettingsDroid.Options.Pages.Albums => FragmentLibraryAlbums,
				ILibrarySettingsDroid.Options.Pages.Artists => FragmentLibraryArtists,
				ILibrarySettingsDroid.Options.Pages.Genres => FragmentLibraryGenres,
				ILibrarySettingsDroid.Options.Pages.Playlists => FragmentLibraryPlaylists,
				ILibrarySettingsDroid.Options.Pages.Queue => FragmentLibraryQueue,
				ILibrarySettingsDroid.Options.Pages.Songs => FragmentLibrarySongs,

				_ => null
			};

			if (drawerlayoutable != null)
			{
				IMenuItem? menuitem = DrawerLayoutables?.Index(drawerlayoutable) is int index
					? Navigationview?.Menu.GetItem(index)
					: null;

				OnDrawerLayoutableSelected(drawerlayoutable, menuitem);
			}
		}

		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.xyzu_layout_library_drawerlayout);

			_CurrentLibraryType = 
				savedInstanceState?.GetString(nameof(_CurrentLibraryType)) is string currentlibrarytype &&
				Enum.TryParse(currentlibrarytype, out LibraryTypes librarytype) 
					? librarytype
					: new LibraryTypes?();

			SetSupportActionBar(ActivityToolbar = ToolbarDrawer.Toolbar);

			SupportActionBar?.SetHomeButtonEnabled(true);
			SupportActionBar?.SetDisplayHomeAsUpEnabled(false);
			SupportActionBar?.SetDisplayShowHomeEnabled(false);
			SupportActionBar?.SetDisplayShowTitleEnabled(false); 
			SetStatusBars(
				Resource.Id.xyzu_layout_library_drawerlayout_statusbarsurface_statusbarinsetview,
				Resource.Id.xyzu_layout_library_drawerlayout_statusbarprimary_statusbarinsetview);

			InitSlidingPanelLayout(
				FindViewById(Resource.Id.xyzu_layout_library_drawerlayout_root_slidinguppanellayout),
				FindViewById(Resource.Id.xyzu_layout_library_drawerlayout_nowplayingview),
				FindViewById(Resource.Id.xyzu_layout_library_drawerlayout_nowplayingview_cardview));

			for (int index = 0; index < DrawerLayoutables.Length; index++)
				DrawerLayoutables[index].ConfigureMenuItem(
					context: this,
					menuitem: Navigationview.Menu
						.Add(0, index, 0, string.Empty)?
						.SetCheckable(true));

			if (Navigationview.LayoutParameters != null)
				Navigationview.LayoutParameters!.Width = Resources?.Configuration?.Orientation == Orientation.Landscape
					? Resources.GetDimensionPixelSize(Resource.Dimension.dp360)
					: DrawerLayout.LayoutParams.WrapContent;
		}
		protected override void OnStart()
		{
			base.OnStart();

			ToDrawerLayoutable(_CurrentLibraryType switch
			{
				LibraryTypes.LibraryAlbums => FragmentLibraryAlbums,
				LibraryTypes.LibraryArtists => FragmentLibraryArtists,
				LibraryTypes.LibraryGenres => FragmentLibraryGenres,
				LibraryTypes.LibraryPlaylists => FragmentLibraryPlaylists,
				LibraryTypes.LibraryQueue => FragmentLibraryQueue,
				LibraryTypes.LibrarySongs => FragmentLibrarySongs,
				LibraryTypes.LibrarySearch => FragmentLibrarySearch,

				_ => null
			});
		}
		protected override void OnResume()
		{
			base.OnResume();

			Navigationview?.SetNavigationItemSelectedListener(this);
			if (Actionbardrawertoggle != null) Drawerlayout?.AddDrawerListener(Actionbardrawertoggle);
		}
		protected override void OnPause()
		{
			base.OnPause();

			Navigationview?.SetNavigationItemSelectedListener(null);
			if (Actionbardrawertoggle != null) Drawerlayout?.RemoveDrawerListener(Actionbardrawertoggle);
		}
		protected override void OnSaveInstanceState(Bundle outstate)
		{
			base.OnSaveInstanceState(outstate);

			outstate.PutString(nameof(_CurrentLibraryType), CurrentDrawerLayoutable?.LibraryType.ToString() ?? string.Empty);
		}

		public override void OnBackPressed()
		{
			if (PreviousDrawerLayoutable is null)
				base.OnBackPressed();
			else
			{
				ToDrawerLayoutable(CurrentDrawerLayoutable = PreviousDrawerLayoutable);
				PreviousDrawerLayoutable = null;
			}

			OnReconfigure(this, IConfigurable.ReconfigureType_All);
		}
		public override bool OnCreateOptionsMenu(IMenu? menu)
		{
			OnReconfigure(this, IConfigurable.ReconfigureType_Menu);

			return base.OnCreateOptionsMenu(menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem menuitem)
		{
			bool result = false;

			switch ((menuitem.ItemId, SupportFragmentManager.BackStackEntryCount))
			{
				case (Android.Resource.Id.Home, 0) when Drawerlayout != null:
					if (Drawerlayout.IsOpen)
						Drawerlayout.Close();
					else Drawerlayout.Open();
					result = true;
					break;

				default: break;
			}

			if (result is false)
				result = base.OnOptionsItemSelected(menuitem);

			if (result is false)
				result = CurrentDrawerLayoutable?.OnMenuItemClick(menuitem) ?? false;
			
			return result;
		}
		public override bool OnMenuItemClick(IMenuItem? menuitem)
		{
			bool result = false;
			
			if (menuitem != null)
				result = OnNavigationItemSelected(menuitem);
			
			return result ? result : base.OnMenuItemClick(menuitem);
		}

		public bool OnNavigationItemSelected(IMenuItem menuitem)
		{
			IDrawerLayoutable? drawerlayoutable = true switch
			{
				true when menuitem.ItemId == DrawerLayoutables?.Index(FragmentLibraryQueue) 
					=> FragmentLibraryQueue,

				true when menuitem.ItemId == DrawerLayoutables?.Index(FragmentLibrarySongs) 
					=> FragmentLibrarySongs,

				true when menuitem.ItemId == DrawerLayoutables?.Index(FragmentLibraryAlbums) 
					=> FragmentLibraryAlbums,

				true when menuitem.ItemId == DrawerLayoutables?.Index(FragmentLibraryArtists) 
					=> FragmentLibraryArtists,

				true when menuitem.ItemId == DrawerLayoutables?.Index(FragmentLibraryGenres) 
					=> FragmentLibraryGenres,

				true when menuitem.ItemId == DrawerLayoutables?.Index(FragmentLibraryPlaylists) 
					=> FragmentLibraryPlaylists,

				_ => null
			};

			if (drawerlayoutable is null)
				return false;

			ActivityCancellationTokenSourceCancel();

			OnDrawerLayoutableSelected(drawerlayoutable, menuitem);

			return true;
		}
		public void OnDrawerLayoutableSelected(IDrawerLayoutable drawerlayoutable, IMenuItem? menuitem)
		{
			SupportFragmentManager
				.BeginTransaction()
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_framelayout, drawerlayoutable.Fragment)
				.RunOnCommit(new JavaRunnable(() =>
				{
					Drawerlayout?.CloseDrawers();

					if (Actionbardrawertoggle != null)
						Actionbardrawertoggle.DrawerIndicatorEnabled = true;

					OnReconfigure(this, CurrentDrawerLayoutable = drawerlayoutable, IConfigurable.ReconfigureType_All);

					menuitem?.SetChecked(true);

					RefreshDrawerLayout(CurrentDrawerLayoutable);

				})).Commit();
		}
		protected async void RefreshDrawerLayout(IDrawerLayoutable? drawerlayoutable, bool force = false)
		{
			drawerlayoutable ??= CurrentDrawerLayoutable;

			if (drawerlayoutable is null)
				return;

			await drawerlayoutable.Refresh(force);
		}

		protected override void OnFloatingactionbuttonClick(object? sender, EventArgs args)
		{
			base.OnFloatingactionbuttonClick(sender, args);

			if (CurrentDrawerLayoutable == FragmentLibraryPlaylists)
				FragmentLibraryPlaylists.View?.DisplayCreateDialog();
		}
		protected override void OnPanelStateChangedAction(View view, SlidingUpPanelLayout.PanelState state1, SlidingUpPanelLayout.PanelState state2)
		{
			base.OnPanelStateChangedAction(view, state1, state2);

			if (SlidingUpPanel is null)
				return;

			switch (true)
			{
				case true when
				state1 == SlidingUpPanelLayout.PanelState.Hidden &&
				state2 != SlidingUpPanelLayout.PanelState.Hidden:
					Navigationview.SetMarginBottom(SlidingUpPanel.PanelHeight + Resources?.GetDimensionPixelSize(Resource.Dimension.dp16) ?? 0, true);
					break;

				case true when
				state1 != SlidingUpPanelLayout.PanelState.Hidden &&
				state2 == SlidingUpPanelLayout.PanelState.Hidden:
					Navigationview.SetMarginBottom(-SlidingUpPanel.PanelHeight - Resources?.GetDimensionPixelSize(Resource.Dimension.dp16) ?? 0, true);
					break;									  

				default: break;
			}
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(CurrentDrawerLayoutable):
					Floatingactionbutton.Visibility = CurrentDrawerLayoutable == FragmentLibraryPlaylists
						? ViewStates.Visible
						: ViewStates.Invisible;
					break;

				default: break;
			}
		}
		protected override void XyzuLibraryOnServiceConnectionChanged(object? sender, ServiceConnectionChangedEventArgs args)
		{
			base.XyzuLibraryOnServiceConnectionChanged(sender, args);

			switch (XyzuLibrary.Instance.ScannerServiceConnectionState)
			{
				case ServiceConnectionChangedEventArgs.Events.Disconnected:
					RefreshDrawerLayout(null, true);
					break;

				default: break;
			}
		}

		public interface IDrawerLayoutable : IConfigurable, Toolbar.IOnMenuItemClickListener { }
	}
}