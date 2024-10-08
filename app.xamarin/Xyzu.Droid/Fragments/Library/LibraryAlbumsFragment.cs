﻿#nullable enable

using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;

using Google.Android.Material.Tabs;

using System;
using System.Linq;
using Xyzu.Activities;
using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Views.Library;
using Xyzu.Views.Option;

namespace Xyzu.Fragments.Library
{
	public class LibraryAlbumsFragment : LibraryFragment
	{
		public new LibraryAlbumsView? View { get; set; }
		public Action<LibraryAlbumsView>? OnCreateViewAction { get; set; }

		public override LibraryView? LibraryView
		{
			get => View;
		}
		public override LibraryActivity.LibraryTypes LibraryType
		{
			get => LibraryActivity.LibraryTypes.LibraryAlbums;
		}

		public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
		{
			if (Context is null)
				return base.OnCreateView(inflater, container, savedInstanceState);

			View = new LibraryAlbumsView(Context)
			{
				LibraryFragment = this,
			};

			OnCreateViewAction?.Invoke(View);

			View.Albums.LibraryItemsAdapter.PropertyChanged += LibraryItemsAdapter_PropertyChanged;

			return View;
		}
		public override void OnDestroyView()
		{
			base.OnDestroyView();

			if (View != null)
				View.Albums.LibraryItemsAdapter.PropertyChanged += LibraryItemsAdapter_PropertyChanged;
		}
		public override bool OnBackPressed()
		{
			if (View != null && View.Albums.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
			{
				View.Albums.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

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
				.SetId(Resource.String.library_albums)
				.SetText(Resource.String.library_albums)
				.SetIcon(Resource.Drawable.icon_library_albums);
		}
		public override void ConfigureToolbar(Toolbar? toolbar, Context? context)
		{
			base.ConfigureToolbar(toolbar, context);

			if (toolbar is null || Activity?.GetType() == typeof(LibraryActivityTabLayout))
				return;

			toolbar.SetTitle(Resource.String.library_albums);
		}
		public override void ConfigureMenuItem(IMenuItem? menuitem, Context? context)
		{
			base.ConfigureMenuItem(menuitem, context);

			menuitem?
				.SetTitle(Resource.String.library_albums)?
				.SetIcon(Resource.Drawable.icon_library_albums);
		}
		public override void ConfigureListOptions(OptionsLibraryListView listoptions)
		{
			base.ConfigureListOptions(listoptions);

			listoptions.IsReversed = View?.Settings.IsReversed ?? IAlbumsSettings.Defaults.IsReversed;
			listoptions.LayoutTypes = IAlbumsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered();
			listoptions.LayoutTypeSelected = View?.Settings.LayoutType ?? IAlbumsSettings.Defaults.LayoutType;
			listoptions.SortKeys = IAlbumsSettings.Options.SortKeys.AsEnumerable().OrderBy(_ => _);
			listoptions.SortKeySelected = View?.Settings.SortKey ?? IAlbumsSettings.Defaults.SortKey;
			listoptions.OnOptionsIsReversedClicked = isreversed =>
			{
				if (View is null)
					return;

				View.Settings.IsReversed = isreversed;
			};
			listoptions.OnOptionsLayoutTypeItemSelected = layouttype =>
			{
				if (View is null)
					return;

				View.Settings.LayoutType = layouttype;
			};
			listoptions.OnOptionsSortKeyItemSelected = sortkey =>
			{
				if (View is null)
					return;

				View.Settings.SortKey = sortkey;
			};
		}
	}
}