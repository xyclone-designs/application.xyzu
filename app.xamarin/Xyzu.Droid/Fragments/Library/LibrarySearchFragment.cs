#nullable enable

using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;

using Google.Android.Material.AppBar;
using Google.Android.Material.Tabs;

using System;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Views.Library;
using Xyzu.Views.Option;
using Xyzu.Views.Toolbar;

namespace Xyzu.Fragments.Library
{
	public class LibrarySearchFragment : LibraryFragment
	{
		public new LibrarySearchView? View { get; set; }
		public Action<LibrarySearchView>? OnCreateViewAction { get; set; }

		public override LibraryView? LibraryView
		{
			get => View;
		}
		public ToolbarSearchView? LibrarySearch { get;set; }

		public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
		{
			if (Context is null)
				return base.OnCreateView(inflater, container, savedInstanceState);

			View = new LibrarySearchView(Context)
			{
				LibraryFragment = this,
			};

			OnCreateViewAction?.Invoke(View);

			View.SearchResults.LibraryItemsAdapter.PropertyChanged += LibraryItemsAdapter_PropertyChanged;

			return View;
		}
		public override void OnDestroyView()
		{
			base.OnDestroyView();

			if (View != null)
				View.SearchResults.LibraryItemsAdapter.PropertyChanged += LibraryItemsAdapter_PropertyChanged;
		}
		public override bool OnBackPressed()
		{
			if (View != null && View.SearchResults.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
			{
				View.SearchResults.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

				return true;
			}

			return base.OnBackPressed();
		}
		public override bool OnMenuItemClick(IMenuItem? item)
		{
			return (item is null || View is null || View.OnMenuItemClick(item)) || base.OnMenuItemClick(item);
		}
		public override void ConfigureTab(TabLayout.Tab tab, Context? context)
		{
			base.ConfigureTab(tab, context);

			tab
				.SetId(Resource.String.library_search)
				.SetText(Resource.String.library_search)
				.SetIcon(Resource.Drawable.icon_library_search);
		}
		public override void ConfigureToolbar(Toolbar? toolbar, Context? context)
		{
			base.ConfigureToolbar(toolbar, context);

			if (toolbar is null || Activity?.GetType() == typeof(Activities.LibraryActivityTabLayout))
				return;

			toolbar.SetTitle(Resource.String.library_search);
		}
		public override void ConfigureMenuItem(IMenuItem? menuitem, Context? context)
		{
			base.ConfigureMenuItem(menuitem, context);

			menuitem?.SetTitle(Resource.String.library_search);
			menuitem?.SetIcon(Resource.Drawable.icon_library_search);
		}
		public override void ConfigureListOptions(OptionsLibraryListView listoptions)
		{
			base.ConfigureListOptions(listoptions);

			listoptions.OptionsIsReversed.Visibility = ViewStates.Gone;
			listoptions.OptionsSortKey.Visibility = ViewStates.Gone;
			listoptions.LayoutTypes = ISearchSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered();
			listoptions.LayoutTypeSelected = View?.Settings.LayoutType ?? ISongsSettings.Defaults.LayoutType;
			listoptions.OnOptionsLayoutTypeItemSelected = layouttype =>
			{
				if (View is null)
					return;

				View.Settings.LayoutType = layouttype;
			};
		}
	}
}