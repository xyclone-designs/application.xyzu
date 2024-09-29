#nullable enable

using Android.Content;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;

using Google.Android.Material.AppBar;

using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Xyzu.Menus;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Activities
{
	public partial class LibraryActivity 
	{
		private ILibrarySettingsDroid? _Settings;

		protected ILibrarySettingsDroid Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = XyzuSettings.Instance.GetUserInterfaceLibraryDroid();
					_Settings.PropertyChanged += OnSettingsPropertyChanged;
				}

				return _Settings;
			}
			set
			{
				if (_Settings != null)
					_Settings.PropertyChanged -= OnSettingsPropertyChanged;

				_Settings = value;

				if (_Settings != null)
					_Settings.PropertyChanged += OnSettingsPropertyChanged;
			}
		}
		protected int SettingsAppbarLayoutParamsScrollFlags
		{
			get => Settings.HeaderScrollType switch
			{
				LibraryHeaderScrollTypes.PinToTop =>
					AppBarLayout.LayoutParams.ScrollFlagEnterAlways |
					AppBarLayout.LayoutParams.ScrollFlagSnap,

				LibraryHeaderScrollTypes.PinToTopScroll =>
					AppBarLayout.LayoutParams.ScrollFlagSnap |
					AppBarLayout.LayoutParams.ScrollFlagScroll,

				LibraryHeaderScrollTypes.Scroll =>
					AppBarLayout.LayoutParams.ScrollFlagEnterAlways |
					AppBarLayout.LayoutParams.ScrollFlagExitUntilCollapsed |
					AppBarLayout.LayoutParams.ScrollFlagScroll |
					AppBarLayout.LayoutParams.ScrollFlagSnap,

				_ => 0
			};
		}

		protected virtual void OnReconfigure(object? sender, string reconfiguretype) 
		{
			OnReconfigure(sender, null, reconfiguretype); 
		}
		protected virtual void OnReconfigure(object? sender, IConfigurable? toolbarconfigurable, params string[] reconfiguretypes) 
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

		protected virtual void ReconfigureLayout() { }
		protected virtual void ReconfigureAppBar() 
		{
			foreach (View view in Appbarlayout.GetAllChildren(0))
				if (view.LayoutParameters is AppBarLayout.LayoutParams appbarlayoutparams)
					appbarlayoutparams.ScrollFlags = SettingsAppbarLayoutParamsScrollFlags;
		}						
		protected virtual void ReconfigureMenu() 
		{
			if (ActivityToolbar?.Menu is null)
				return;

			if (ActivityToolbar.Menu.FindItem((int)MenuOptions.Rescan) is null)
			{
				ActivityToolbar.Menu.AddMenuOption(MenuOptions.Rescan, BaseContext, out IMenuItem? rescansmenuitem);
				rescansmenuitem?.SetShowAsAction(ShowAsAction.IfRoom);
			}		   

			if (ActivityToolbar.Menu.FindItem((int)MenuOptions.Settings) is null)
			{
				ActivityToolbar.Menu.AddMenuOption(MenuOptions.Settings, BaseContext, out IMenuItem? settingsmenuitem);
				settingsmenuitem?.SetShowAsAction(ShowAsAction.IfRoom);
			}
		}
		protected virtual void ReconfigureMenuItem() { }
		protected virtual void ReconfigureToolbar() { }

		public virtual void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs args) { }

		public interface IConfigurable
		{
			public const string ReconfigureType_All = "All";
			public const string ReconfigureType_AppBar = "AppBar";
			public const string ReconfigureType_Menu = "Menu";
			public const string ReconfigureType_MenuItem = "MenuItem";
			public const string ReconfigureType_Toolbar = "Toolbar";

			Fragment Fragment { get; }
			LibraryTypes LibraryType { get; }

			event EventHandler<string> OnReconfigure;

			bool OnOptionsItemSelected(IMenuItem menuitem);

			void ConfigureAppBar(AppBarLayout AppBarLayout, Context? context);
			void ConfigureMenu(IMenu? menu, Context? context);
			void ConfigureMenuItem(IMenuItem? menuitem, Context? context);
			void ConfigureToolbar(Toolbar? toolbar, Context? context);

			Task Refresh(bool force = false);
		}
	}
}