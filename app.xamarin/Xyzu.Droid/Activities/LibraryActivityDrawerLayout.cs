#nullable enable

using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.DrawerLayout.Widget;

using Com.Sothree.Slidinguppanel;

using Google.Android.Material.AppBar;			
using Google.Android.Material.Navigation;			

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Fragments.Library;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Views.Toolbar;

using JavaRunnable = Java.Lang.Runnable;

namespace Xyzu.Activities
{
	[Android.App.Activity(
		Theme = "@style/LibraryTheme",
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
	public partial class LibraryActivityDrawerLayout : LibraryActivity, NavigationView.IOnNavigationItemSelectedListener
	{
		protected IDrawerLayoutable[]? _DrawerLayoutables;
		protected IDrawerLayoutable? _CurrentDrawerLayoutable;
		protected CoordinatorLayout? _Coordinatorlayout;
		protected DrawerLayout? _Drawerlayout;
		protected NavigationView? _Navigationview;
		protected ContentFrameLayout? _Contentframelayout;
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
		public ContentFrameLayout Contentframelayout 
		{ 
			get => _Contentframelayout ??=
				FindViewById<ContentFrameLayout>(Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout) ??
				throw new InflateException("Could not find view 'xyzu_layout_library_drawerlayout_contentframelayout' in 'layout_library_drawerlayout'");
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

			SetSupportActionBar(ActivityToolbar = ToolbarDrawer.Toolbar);

			SupportActionBar?.SetHomeButtonEnabled(true);
			SupportActionBar?.SetDisplayHomeAsUpEnabled(false);
			SupportActionBar?.SetDisplayShowHomeEnabled(false);
			SupportActionBar?.SetDisplayShowTitleEnabled(false);

			InitSlidingPanelLayout(
				RootView = FindViewById(Resource.Id.xyzu_layout_library_drawerlayout_root_slidinguppanellayout),
				FindViewById(Resource.Id.xyzu_layout_library_drawerlayout_nowplayingview),
				FindViewById(Resource.Id.xyzu_layout_library_drawerlayout_nowplayingview_cardview));

			for (int index = 0; index < DrawerLayoutables.Length; index++)
				DrawerLayoutables[index].ConfigureMenuItem(
					context: this,
					menuitem: Navigationview.Menu
						.Add(0, index, 0, string.Empty)?
						.SetCheckable(true));
		}		  
		protected override void OnResume()
		{
			base.OnResume();

			Navigationview?.SetNavigationItemSelectedListener(this);
			if (Actionbardrawertoggle != null) Drawerlayout?.AddDrawerListener(Actionbardrawertoggle);

			ToDrawerLayoutable(null);
		}
		protected override void OnPause()
		{
			base.OnPause();

			Navigationview?.SetNavigationItemSelectedListener(null);
			if (Actionbardrawertoggle != null) Drawerlayout?.RemoveDrawerListener(Actionbardrawertoggle);
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

		public override void NavigateAlbum(IAlbum? album)
		{
			base.NavigateAlbum(album);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibraryAlbum, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, GenerateFragmentLibraryAlbum(album))
				.Commit();
		}
		public override void NavigateArtist(IArtist? artist)
		{
			base.NavigateArtist(artist);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibraryArtist, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, GenerateFragmentLibraryArtist(artist))
				.Commit();
		}
		public override void NavigateGenre(IGenre? genre)
		{
			base.NavigateGenre(genre);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibraryGenre, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, GenerateFragmentLibraryGenre(genre))
				.Commit();
		}
		public override void NavigatePlaylist(IPlaylist? playlist)
		{
			base.NavigatePlaylist(playlist);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibraryPlaylist, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, GenerateFragmentLibraryPlaylist(playlist))
				.Commit();
		}
		public override void NavigateQueue()
		{
			base.NavigateQueue();

			ToDrawerLayoutable(FragmentLibraryQueue);
		}
		public override void NavigateSearch()
		{
			base.NavigateSearch();

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentDrawerLayoutable = FragmentLibrarySearch, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, FragmentLibrarySearch)
				.Commit();
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
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_drawerlayout_contentframelayout, drawerlayoutable.Fragment)
				.Commit();

			Drawerlayout?.CloseDrawers();

			CurrentDrawerLayoutable = drawerlayoutable;

			if (Actionbardrawertoggle != null)
				Actionbardrawertoggle.DrawerIndicatorEnabled = true;

			menuitem?.SetChecked(true);

			OnReconfigure(this, IConfigurable.ReconfigureType_All);

			RefreshDrawerLayout(CurrentDrawerLayoutable);
		}
		protected async void RefreshDrawerLayout(IDrawerLayoutable? drawerlayoutable, bool force = false)
		{
			drawerlayoutable ??= CurrentDrawerLayoutable;

			if (drawerlayoutable is null)
				return;

			await drawerlayoutable.Refresh(force);
		}

		protected override void OnFloatingactionbuttonClick(object sender, EventArgs args)
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
		protected override void OnReconfigure(object sender, IConfigurable? toolbarconfigurable, params string[] reconfiguretypes)
		{
			ReconfigureLayout();

			foreach (string reconfiguretype in reconfiguretypes)
				switch (reconfiguretype)
				{
					case IConfigurable.ReconfigureType_All:
						ReconfigureAppBar();
						ReconfigureMenu();
						ReconfigureMenuItem();
						ReconfigureToolbar();
						break;

					case IConfigurable.ReconfigureType_AppBar:
						ReconfigureAppBar();
						break;							

					case IConfigurable.ReconfigureType_Menu:
						ReconfigureMenu();
						break;

					case IConfigurable.ReconfigureType_MenuItem:
						ReconfigureMenuItem();
						break;

					case IConfigurable.ReconfigureType_Toolbar:
						ReconfigureToolbar();
						break;

					default: break;
				}
		}
		protected override void ReconfigureAppBar()
		{
			base.ReconfigureAppBar();

			CurrentDrawerLayoutable?.ConfigureAppBar(Appbarlayout, this);
		}
		protected override void ReconfigureLayout()
		{
			switch (true)
			{
				case true when CurrentDrawerLayoutable == FragmentLibrarySearch:

					ToolbarSearch.Visibility = ViewStates.Visible;
					ToolbarDrawer.Visibility = ViewStates.Gone;
					Contentframelayout.Visibility = ViewStates.Visible;

					break;

				case true when CurrentDrawerLayoutable == FragmentLibraryAlbum:
				case true when CurrentDrawerLayoutable == FragmentLibraryArtist:
				case true when CurrentDrawerLayoutable == FragmentLibraryGenre:
				case true when CurrentDrawerLayoutable == FragmentLibraryPlaylist:

					ToolbarSearch.Visibility = ViewStates.Gone;
					ToolbarDrawer.Visibility = ViewStates.Gone;
					Contentframelayout.Visibility = ViewStates.Visible;

					break;

				case true when CurrentDrawerLayoutable == FragmentLibraryAlbums:
				case true when CurrentDrawerLayoutable == FragmentLibraryArtists:
				case true when CurrentDrawerLayoutable == FragmentLibraryGenres:
				case true when CurrentDrawerLayoutable == FragmentLibraryPlaylists:
				case true when CurrentDrawerLayoutable == FragmentLibraryQueue:
				case true when CurrentDrawerLayoutable == FragmentLibrarySongs:
				case true when CurrentDrawerLayoutable == FragmentLibrarySearch:
				default:

					ToolbarSearch.Visibility = ViewStates.Gone;
					ToolbarDrawer.Visibility = ViewStates.Visible;
					Contentframelayout.Visibility = ViewStates.Visible;

					break;
			}

			base.ReconfigureLayout();
		}
		protected override void ReconfigureMenu()
		{
			CurrentDrawerLayoutable?.ConfigureMenu(ActivityToolbar?.Menu, this);

			base.ReconfigureMenu();
		}
		protected override void ReconfigureMenuItem()
		{
			if
			(
				ActivityToolbar?.Menu != null &&
				DrawerLayoutables?.Index(CurrentDrawerLayoutable) is int index &&
				index < ActivityToolbar.Menu.Size() &&
				ActivityToolbar.Menu.GetItem(index) is IMenuItem menuitem

			) CurrentDrawerLayoutable?.ConfigureMenuItem(menuitem, this);

			base.ReconfigureMenuItem();
		}
		protected override void ReconfigureToolbar()
		{
			Actionbardrawertoggle?.SyncState();
			CurrentDrawerLayoutable?.ConfigureToolbar(ActivityToolbar, this);

			base.ReconfigureToolbar();
		}
		protected override void XyzuLibraryOnServiceConnectionChanged(object sender, ServiceConnectionChangedEventArgs args)
		{
			base.XyzuLibraryOnServiceConnectionChanged(sender, args);

			switch (XyzuLibrary.Instance.ServiceConnectionState)
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