#nullable enable

using Android.Views;

using System.Linq;

namespace Xyzu.Activities
{
	public partial class LibraryActivityDrawerLayout 
	{
		protected override void OnReconfigure(object sender, IConfigurable? toolbarconfigurable, params string[] reconfiguretypes)
		{
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

			ReconfigureLayout();
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
	}
}