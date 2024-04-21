#nullable enable

using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.CoordinatorLayout.Widget;	
using AndroidX.ViewPager2.Adapter;
using AndroidX.ViewPager2.Widget;

using Google.Android.Material.Tabs;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

using Xyzu.Droid;
using Xyzu.Fragments.Library;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Views.Library;
using Xyzu.Views.Toolbar;

using JavaRunnable = Java.Lang.Runnable;

namespace Xyzu.Activities
{
	[Android.App.Activity(
		Theme = "@style/LibraryTheme",
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
	public partial class LibraryActivityTabLayout : LibraryActivity
	{
		private ITabLayoutable? _CurrentTabLayoutable;
		private CoordinatorLayout? _Coordinatorlayout;
		private ViewPager2? _Viewpager2;
		private ContentFrameLayout? _Contentframelayout;
		private ToolbarTabLayoutView? _ToolbarTabLayout;

		protected ITabLayoutable? CurrentTabLayoutable 
		{
			get
			{
				if (_CurrentTabLayoutable != null)
					return _CurrentTabLayoutable;

				if (ToolbarTabLayout.Tablayout != null)
				{
					_CurrentTabLayoutable = Tablayoutadapter?.TabLayoutables.ElementAtOrDefault(ToolbarTabLayout.Tablayout.SelectedTabPosition);

					if (_CurrentTabLayoutable != null)
						_CurrentTabLayoutable.OnReconfigure += OnReconfigure;
				}

				return _CurrentTabLayoutable;
			}
			set
			{
				if (_CurrentTabLayoutable != null)
					_CurrentTabLayoutable.OnReconfigure -= OnReconfigure;

				PreviousTabLayoutable = _CurrentTabLayoutable;
				_CurrentTabLayoutable = value;
				CurrentLibraryFragment = _CurrentTabLayoutable as LibraryFragment;

				if (_CurrentTabLayoutable != null)
					_CurrentTabLayoutable.OnReconfigure += OnReconfigure;

				OnPropertyChanged();
				OnPropertyChanged(nameof(PreviousTabLayoutable));
			}
		}
		protected ITabLayoutable? PreviousTabLayoutable
		{ 
			get; set;
		}

		protected CoordinatorLayout Coordinatorlayout
		{
			get => _Coordinatorlayout ??=
				FindViewById<CoordinatorLayout>(Resource.Id.xyzu_layout_library_tablayout_coordinatorlayout) ??
				throw new InflateException("Could not find view 'xyzu_layout_library_tablayout_coordinatorlayout' in 'layout_library_tablayout'");
		}
		protected ViewPager2 Viewpager2 
		{
			get => _Viewpager2 ??=
				FindViewById<ViewPager2>(Resource.Id.xyzu_layout_library_tablayout_viewpager2) ??
				throw new InflateException("Could not find view 'layout_library_tablayout_viewpager2' in 'layout_library_tablayout'");
		}
		protected TabLayoutFragmentAdapter? Tablayoutadapter 
		{
			get; set;
		}
		protected TabLayoutMediator? Tablayoutmediator 
		{
			get; set;
		}
		protected ContentFrameLayout Contentframelayout
		{
			get => _Contentframelayout ??=
				FindViewById<ContentFrameLayout>(Resource.Id.xyzu_layout_library_tablayout_contentframelayout) ??
				throw new InflateException("Could not find view 'xyzu_layout_library_tablayout_contentframelayout' in 'layout_library_tablayout'");
		}								 
		protected ToolbarTabLayoutView ToolbarTabLayout
		{
			get => _ToolbarTabLayout ??=
				FindViewById<ToolbarTabLayoutView>(Resource.Id.xyzu_layout_library_tablayout_toolbartablayoutview) ??
				throw new InflateException("Could not find view 'xyzu_layout_library_tablayout_toolbartablayoutview' in 'layout_library_tablayout'");
		}

		protected override int AppbarlayoutResourceId
		{
			get => Resource.Id.xyzu_layout_library_tablayout_appbarlayout;
		}
		protected override int ToolbarSearchResourceId 
		{
			get => Resource.Id.xyzu_layout_library_tablayout_toolbarsearchview;
		}
		protected override int FloatingactionbuttonResourceId
		{
			get => Resource.Id.xyzu_layout_library_tablayout_floatingactionbutton;
		}
		
		public override void NavigateAlbum(IAlbum? album)
		{
			base.NavigateAlbum(album);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentTabLayoutable = FragmentLibraryAlbum, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_tablayout_contentframelayout, GenerateFragmentLibraryAlbum(album))
				.Commit();
		}
		public override void NavigateArtist(IArtist? artist)
		{
			base.NavigateArtist(artist);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentTabLayoutable = FragmentLibraryArtist, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_tablayout_contentframelayout, GenerateFragmentLibraryArtist(artist))
				.Commit();
		}
		public override void NavigateGenre(IGenre? genre)
		{
			base.NavigateGenre(genre);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentTabLayoutable = FragmentLibraryGenre, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_tablayout_contentframelayout, GenerateFragmentLibraryGenre(genre))
				.Commit();
		}
		public override void NavigatePlaylist(IPlaylist? playlist)
		{
			base.NavigatePlaylist(playlist);

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentTabLayoutable = FragmentLibraryPlaylist, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_tablayout_contentframelayout, GenerateFragmentLibraryPlaylist(playlist))
				.Commit();
		}
		public override void NavigateQueue()
		{
			base.NavigateQueue();

			if (Tablayoutadapter?.TabLayoutables.Index(FragmentLibraryQueue) is int index && ToolbarTabLayout.Tablayout.GetTabAt(index) is TabLayout.Tab tab)
				ToolbarTabLayout.Tablayout.SelectTab(tab, true);																																																										
		}
		public override void NavigateSearch()
		{
			base.NavigateSearch();

			SupportFragmentManager
				.BeginTransaction()
				.RunOnCommit(new JavaRunnable(() => OnReconfigure(this, CurrentTabLayoutable = FragmentLibrarySearch, IConfigurable.ReconfigureType_All)))
				.Replace(Contentframelayout?.Id ?? Resource.Id.xyzu_layout_library_tablayout_contentframelayout, FragmentLibrarySearch)
				.Commit();
		}

		public override void OnBackPressed()
		{
			if (_CurrentTabLayoutable != null)
				OnReconfigure(this, _CurrentTabLayoutable = null, IConfigurable.ReconfigureType_All);
			else base.OnBackPressed();
		}
		public override bool OnCreateOptionsMenu(IMenu? menu)
		{
			OnReconfigure(this, IConfigurable.ReconfigureType_Menu);

			return base.OnCreateOptionsMenu(menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem menuitem)
		{
			bool result = CurrentTabLayoutable?.OnMenuItemClick(menuitem) ?? false;

			if (result is false)
				result = base.OnOptionsItemSelected(menuitem);

			return result;
		}

		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.xyzu_layout_library_tablayout);

			ActivityToolbar = ToolbarTabLayout.Toolbar;
			ActivityToolbar.SetOnMenuItemClickListener(this);
			ActivityToolbar.SetOnCreateContextMenuListener(this);
			SetSupportActionBar(ActivityToolbar);
			SupportActionBar?.SetHomeButtonEnabled(true);

			InitSlidingPanelLayout(
				RootView = FindViewById(Resource.Id.xyzu_layout_library_tablayout_root_slidinguppanellayout),
				FindViewById(Resource.Id.xyzu_layout_library_tablayout_nowplayingview),
				FindViewById(Resource.Id.xyzu_layout_library_tablayout_nowplayingview_cardview));

			Viewpager2.OffscreenPageLimit = 1;
			Viewpager2.UserInputEnabled = false;
			Viewpager2.Adapter = Tablayoutadapter ??= new TabLayoutFragmentAdapter(this, SupportFragmentManager, Lifecycle)
			{
				TabLayoutables = Settings.PagesOrdered
					.Select<LibraryPages, ITabLayoutable>(pageordered => pageordered switch
					{
						ILibrarySettingsDroid.Options.Pages.Albums => FragmentLibraryAlbums,
						ILibrarySettingsDroid.Options.Pages.Artists => FragmentLibraryArtists,
						ILibrarySettingsDroid.Options.Pages.Genres => FragmentLibraryGenres,
						ILibrarySettingsDroid.Options.Pages.Playlists => FragmentLibraryPlaylists,
						ILibrarySettingsDroid.Options.Pages.Queue => FragmentLibraryQueue,
						ILibrarySettingsDroid.Options.Pages.Songs => FragmentLibrarySongs,

						_ => throw new ArgumentException(string.Format("Invalid LibraryPages '{0}' in LibraryActivityTabLayout.DrawerLayoutables.Get", pageordered))

					}).ToList()
			};

			Tablayoutmediator ??= new TabLayoutMediator(ToolbarTabLayout.Tablayout, Viewpager2, true, true, Tablayoutadapter);
			Tablayoutmediator.Attach();
		}
		protected override void OnStart()
		{
			base.OnStart();

			ToolbarTabLayout.Tablayout.TabSelected += TabSelected;
			ToolbarTabLayout.Tablayout.TabReselected += TabReselected;
			ToolbarTabLayout.Tablayout.TabUnselected += TabUnselected;

			if (Settings.PageDefault switch
			{
				ILibrarySettingsDroid.Options.Pages.Albums => FragmentLibraryAlbums,
				ILibrarySettingsDroid.Options.Pages.Artists => FragmentLibraryArtists,
				ILibrarySettingsDroid.Options.Pages.Genres => FragmentLibraryGenres,
				ILibrarySettingsDroid.Options.Pages.Playlists => FragmentLibraryPlaylists,
				ILibrarySettingsDroid.Options.Pages.Queue => FragmentLibraryQueue,
				ILibrarySettingsDroid.Options.Pages.Songs => FragmentLibrarySongs,

				_ => null as ITabLayoutable
			
			} is ITabLayoutable tablayoutable && Tablayoutadapter?.TabLayoutables.Index(tablayoutable) is int index && ToolbarTabLayout.Tablayout.GetTabAt(index) is TabLayout.Tab tab)
				ToolbarTabLayout.Tablayout.SelectTab(tab, true);
		}
		protected override void OnStop()
		{
			base.OnStop();

			ToolbarTabLayout.Tablayout.TabSelected -= TabSelected;
			ToolbarTabLayout.Tablayout.TabReselected -= TabReselected;
			ToolbarTabLayout.Tablayout.TabUnselected -= TabUnselected;
		}

		protected override void OnFloatingactionbuttonClick(object sender, EventArgs args)
		{
			base.OnFloatingactionbuttonClick(sender, args);

			if (CurrentTabLayoutable == FragmentLibraryPlaylists)
				FragmentLibraryPlaylists.View?.DisplayCreateDialog();
		}
		protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(CurrentTabLayoutable):
					Floatingactionbutton.Visibility = CurrentTabLayoutable == FragmentLibraryPlaylists
						? ViewStates.Visible
						: ViewStates.Invisible;
					break;

				default: break;
			}
		}
		protected override void ReconfigureAppBar()
		{
			base.ReconfigureAppBar();

			CurrentTabLayoutable?.ConfigureAppBar(Appbarlayout, this);
		}
		protected override void ReconfigureMenu()
		{
			CurrentTabLayoutable?.ConfigureMenu(ActivityToolbar?.Menu, this);

			base.ReconfigureMenu();
		}
		protected override void ReconfigureMenuItem()
		{
			if 
			(
				ActivityToolbar?.Menu != null &&
				ToolbarTabLayout.Tablayout.SelectedTabPosition < ActivityToolbar.Menu.Size() &&
				ActivityToolbar.Menu.GetItem(ToolbarTabLayout.Tablayout.SelectedTabPosition) is IMenuItem menuitem
			
			) CurrentTabLayoutable?.ConfigureMenuItem(menuitem, this);

			base.ReconfigureMenuItem();
		}
		protected override void ReconfigureLayout()
		{
			switch (true)
			{
				case true when CurrentTabLayoutable == FragmentLibrarySearch:

					ToolbarSearch.Visibility = ViewStates.Visible;
					ToolbarTabLayout.Visibility = ViewStates.Gone;
					Viewpager2.Visibility = ViewStates.Gone;
					Contentframelayout.Visibility = ViewStates.Visible;

					break;

				case true when CurrentTabLayoutable == FragmentLibraryAlbum:
				case true when CurrentTabLayoutable == FragmentLibraryArtist:
				case true when CurrentTabLayoutable == FragmentLibraryGenre:
				case true when CurrentTabLayoutable == FragmentLibraryPlaylist:

					ToolbarSearch.Visibility = ViewStates.Gone;
					ToolbarTabLayout.Visibility = ViewStates.Gone;
					Viewpager2.Visibility = ViewStates.Gone;
					Contentframelayout.Visibility = ViewStates.Visible;

					break;

				case true when CurrentTabLayoutable == FragmentLibraryAlbums:
				case true when CurrentTabLayoutable == FragmentLibraryArtists:
				case true when CurrentTabLayoutable == FragmentLibraryGenres:
				case true when CurrentTabLayoutable == FragmentLibraryPlaylists:
				case true when CurrentTabLayoutable == FragmentLibraryQueue:
				case true when CurrentTabLayoutable == FragmentLibrarySongs:
				default:

					ToolbarSearch.Visibility = ViewStates.Gone;
					ToolbarTabLayout.Visibility = ViewStates.Visible;
					Viewpager2.Visibility = ViewStates.Visible;
					Contentframelayout.Visibility = ViewStates.Gone;

					break;
			}

			base.ReconfigureLayout();
		}
		protected override void ReconfigureToolbar()
		{
			CurrentTabLayoutable?.ConfigureToolbar(ActivityToolbar, this);

			base.ReconfigureToolbar();
		}
		protected override void XyzuLibraryOnServiceConnectionChanged(object sender, ServiceConnectionChangedEventArgs args)
		{
			base.XyzuLibraryOnServiceConnectionChanged(sender, args);

			switch (XyzuLibrary.Instance.ServiceConnectionState)
			{
				case ServiceConnectionChangedEventArgs.Events.Disconnected:
					RefreshTabLayout(null, true);
					break;

				default: break;
			}
		}

		protected void TabSelected(object sender, TabLayout.TabSelectedEventArgs args)
		{
			ActivityCancellationTokenSourceCancel();

			OnReconfigure(this, IConfigurable.ReconfigureType_All);

			if (args.Tab != null)
				CurrentTabLayoutable = Tablayoutadapter?.TabLayoutables.ElementAtOrDefault(args.Tab.Position);
			
			RefreshTabLayout(CurrentTabLayoutable);
		}
		protected void TabUnselected(object sender, TabLayout.TabUnselectedEventArgs args) { }
		protected void TabReselected(object sender, TabLayout.TabReselectedEventArgs args) { }

		protected async void RefreshTabLayout(ITabLayoutable? tablayoutable, bool force = false)
		{
			tablayoutable ??= CurrentTabLayoutable;

			if (tablayoutable is null)
				return;

			await tablayoutable.Refresh(force);															   
		}

		public interface ITabLayoutable : IConfigurable, Toolbar.IOnMenuItemClickListener
		{
			public const string ReconfigureType_Tab = "Tab";

			void ConfigureTab(TabLayout.Tab tab, Context? context);
		}
		public class TabLayoutFragmentAdapter : FragmentStateAdapter, TabLayoutMediator.ITabConfigurationStrategy
		{
			public TabLayoutFragmentAdapter(Context context, Fragment fragment) : base(fragment)
			{
				Context = context;
				TabLayoutables = Enumerable.Empty<ITabLayoutable>();
			}
			public TabLayoutFragmentAdapter(Context context, FragmentActivity fragmentActivity) : base(fragmentActivity)
			{
				Context = context;
				TabLayoutables = Enumerable.Empty<ITabLayoutable>();
			}
			public TabLayoutFragmentAdapter(Context context, FragmentManager fragmentManager, Lifecycle lifecycle) : base(fragmentManager, lifecycle)
			{
				Context = context;
				TabLayoutables = Enumerable.Empty<ITabLayoutable>();
			}

			public Context? Context { get; }
			public IEnumerable<ITabLayoutable> TabLayoutables { get; set; }
			public Action<Fragment>? OnCreateFragment { get; set; }

			public override int ItemCount
			{
				get => TabLayoutables.Count();
			}
			public override Fragment CreateFragment(int p0)
			{
				Fragment fragment = TabLayoutables.ElementAt(p0).Fragment;

				OnCreateFragment?.Invoke(fragment);

				return fragment;
			}

			public void OnConfigureTab(TabLayout.Tab tab, int position)
			{
				TabLayoutables.ElementAtOrDefault(position).ConfigureTab(tab, Context);
			}
		}
	}
}