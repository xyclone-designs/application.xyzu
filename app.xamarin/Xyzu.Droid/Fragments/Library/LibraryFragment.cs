#nullable enable

using Android.Content;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;

using Google.Android.Material.AppBar;
using Google.Android.Material.Tabs;

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Activities;
using Xyzu.Menus;
using Xyzu.Views.Library;
using Xyzu.Views.Option;
using Xyzu.Widgets.RecyclerViews;

namespace Xyzu.Fragments.Library
{
	public class LibraryFragment : Fragment, LibraryActivity.IConfigurable, LibraryActivityTabLayout.ITabLayoutable, LibraryActivityDrawerLayout.IDrawerLayoutable, IDialogInterfaceOnShowListener, IDialogInterfaceOnDismissListener
	{
		private LibraryActivity? _Activity;

		public new LibraryActivity? Activity
		{
			set => _Activity = value;
			get => _Activity ?? base.Activity as LibraryActivity;
		}
		public virtual LibraryView? LibraryView
		{
			get;
		}				  
		public Fragment Fragment
		{
			get => this;
		}
		public ILibrary.INavigatable? Navigatable
		{
			get; set;
		}
		protected OptionsLibraryListView? ListOptions 
		{ 
			get; set;
		}												   
		protected bool ListOptionsDisplayed 
		{ 
			get; set;
		}

		public event EventHandler<string>? OnReconfigure;

		public void OnShow(IDialogInterface? dialog) { }
		public void OnDismiss(IDialogInterface? dialog) { }

		public virtual bool OnBackPressed()
		{
			if (ListOptionsDisplayed)
			{
				ListOptionsDisplayed = false;
				OnReconfigureRaise(LibraryActivity.IConfigurable.ReconfigureType_AppBar);
				return true;
			}

			return false;
		}
		public virtual bool OnMenuOptionClick(MenuOptions menuoption)
		{
			switch(menuoption)
			{
				case Menus.Library.ListOptions:
					ListOptionsDisplayed = !ListOptionsDisplayed;
					OnReconfigureRaise(LibraryActivity.IConfigurable.ReconfigureType_AppBar);
					return true;

				default: return false;
			}
		}
		public virtual bool OnMenuItemClick(IMenuItem? item) 
		{
			if ((MenuOptions?)item?.ItemId is MenuOptions menuoption)
				return OnMenuOptionClick(menuoption);

			return false;
		}
		public virtual Task Refresh(bool force = false)
		{
			if (LibraryView != null)
				return LibraryView.OnRefresh(force);

			return Task.CompletedTask;
		}
		public virtual void ConfigureMenu(IMenu? menu, Context? context)
		{
			if (menu is null)
				return;

			menu.Clear();

			foreach (MenuOptions menuoption in Menus.Library.AsEnumerable())
				switch (menuoption)
				{
					case Menus.Library.ListOptions:
						menu.AddMenuOption(menuoption, context ?? Context, out IMenuItem? listoptionsmenuitem);
						listoptionsmenuitem?.SetShowAsAction(ShowAsAction.IfRoom);
						break;																  

					case Menus.Library.Search:
						menu.AddMenuOption(menuoption, context ?? Context, out IMenuItem? searchmenuitem);
						searchmenuitem?.SetShowAsAction(ShowAsAction.Always);
						break;

					default:
						menu.AddMenuOption(menuoption, context ?? Context, out IMenuItem? _);
						break;
				}
		}
		public virtual void ConfigureAppBar(AppBarLayout appBarLayout, Context? context)
		{
			if (ListOptionsDisplayed)
			{
				if (ListOptions is null && Context != null)
					ConfigureListOptions(ListOptions = new OptionsLibraryListView(Context) { Background = null });

				if (appBarLayout.GetAllChildren(0).Contains(ListOptions) is false)
					appBarLayout.AddView(ListOptions, new AppBarLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent));
			}
			else appBarLayout.RemoveView(ListOptions);
		}		 
		public virtual void ConfigureTab(TabLayout.Tab tab, Context? context) 
		{ }
		public virtual void ConfigureToolbar(Toolbar? toolbar, Context? context)
		{ }
		public virtual void ConfigureMenuItem(IMenuItem? menuitem, Context? context) 
		{ }
		public virtual void ConfigureListOptions(OptionsLibraryListView listoptions)
		{ }

		protected virtual void OnReconfigureRaise(string reconfiguretype)
		{
			OnReconfigure?.Invoke(this, reconfiguretype);
		}
		protected virtual void LibraryItemsAdapter_PropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(LibraryItemsRecyclerView.Adapter.RecyclerViewAdapterState):
					OnReconfigureRaise(LibraryActivity.IConfigurable.ReconfigureType_Menu);
					OnReconfigureRaise(LibraryActivity.IConfigurable.ReconfigureType_Toolbar);
					break;										   

				default: break;
			}
		}
	}
}