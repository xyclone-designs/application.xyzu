#nullable enable

using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;

using Google.Android.Material.Tabs;

using System;

using Xyzu.Droid;
using Xyzu.Views.Library;

namespace Xyzu.Fragments.Library
{
	public class LibraryAlbumFragment : LibraryFragment
	{
		public new LibraryAlbumView? View { get; set; }
		public Action<LibraryAlbumView>? OnCreateViewAction { get; set; }

		public override LibraryView? LibraryView
		{
			get => View;
		}

		public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
		{
			if (Context is null)
				return base.OnCreateView(inflater, container, savedInstanceState);

			View ??= new LibraryAlbumView(Context)
			{
				LibraryFragment = this,
			};

			OnCreateViewAction?.Invoke(View);

			View.AlbumSongs.LibraryItemsAdapter.PropertyChanged += LibraryItemsAdapter_PropertyChanged;

			return View;
		}
		public override void OnDestroyView()
		{
			base.OnDestroyView();

			if (View != null)
				View.AlbumSongs.LibraryItemsAdapter.PropertyChanged += LibraryItemsAdapter_PropertyChanged;
		}
		public override bool OnBackPressed()
		{
			if (View != null && View.AlbumSongs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
			{
				View.AlbumSongs.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

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
				.SetId(Resource.String.library_album)
				.SetText(Resource.String.library_album)
				.SetIcon(Resource.Drawable.icon_library_album);
		}
		public override void ConfigureToolbar(Toolbar? toolbar, Context? context)
		{
			base.ConfigureToolbar(toolbar, context);

			if (toolbar is null || Activity?.GetType() == typeof(Activities.LibraryActivityTabLayout))
				return;

			toolbar.SetTitle(Resource.String.library_album);
		}
		public override void ConfigureMenuItem(IMenuItem? menuitem, Context? context)
		{
			base.ConfigureMenuItem(menuitem, context);

			menuitem?
				.SetTitle(Resource.String.library_album)?
				.SetIcon(Resource.Drawable.icon_library_album);
		}
	}
}