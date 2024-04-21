#nullable enable

using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;

using Google.Android.Material.Tabs;

using System;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Views.Library;
using Xyzu.Views.Option;
using Xyzu.Views.Misc;

namespace Xyzu.Fragments.Library
{
	public class LibraryArtistFragment : LibraryFragment
	{
		public new LibraryArtistView? View { get; set; }
		public Action<LibraryArtistView>? OnCreateViewAction { get; set; }

		public override LibraryView? LibraryView
		{
			get => View;
		}

		public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
		{
			if (Context is null)
				return base.OnCreateView(inflater, container, savedInstanceState);

			View = new LibraryArtistView(Context)
			{
				LibraryFragment = this,
			};

			OnCreateViewAction?.Invoke(View);

			View.ArtistItems.LibraryItemsAdapter.PropertyChanged += LibraryItemsAdapter_PropertyChanged;

			return View;
		}
		public override void OnDestroyView()
		{
			base.OnDestroyView();

			if (View != null)
			{
				View.ArtistItems.LibraryItemsAdapter.PropertyChanged += LibraryItemsAdapter_PropertyChanged;
			}
		}
		public override bool OnBackPressed()
		{
			if (View is null || View.ArtistItems.LibraryItemsAdapter.RecyclerViewAdapterState != RecyclerViewAdapterStates.Select)
				return base.OnBackPressed();

			View.ArtistItems.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			return true;
		}
		public override bool OnMenuItemClick(IMenuItem? item)
		{
			return (item is null || View is null || View.OnMenuItemClick(item)) || base.OnMenuItemClick(item);
		}
		public override void ConfigureTab(TabLayout.Tab tab, Context? context)
		{
			base.ConfigureTab(tab, context);

			tab
				.SetId(Resource.String.library_artist)
				.SetText(Resource.String.library_artist)
				.SetIcon(Resource.Drawable.icon_library_artist);
		}
		public override void ConfigureToolbar(Toolbar? toolbar, Context? context)
		{
			base.ConfigureToolbar(toolbar, context);

			if (toolbar is null || Activity?.GetType() == typeof(Activities.LibraryActivityTabLayout))
				return;

			toolbar.SetTitle(Resource.String.library_artist);
		}
		public override void ConfigureMenuItem(IMenuItem? menuitem, Context? context)
		{
			base.ConfigureMenuItem(menuitem, context);

			menuitem?
				.SetTitle(Resource.String.library_artist)?
				.SetIcon(Resource.Drawable.icon_library_artist);
		}
	}
}