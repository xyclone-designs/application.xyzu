using Android.Content;

using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface;

namespace Xyzu
{
	public sealed partial class XyzuSettings
	{
		public static class Utils
		{
			public static Intent MainActivityIntent(Context context, LibraryNavigationTypes? librarynavigationtype)
			{
				if (librarynavigationtype is null)
				{
					IUserInterfaceSettingsDroid? librarynavigationsettingsdroid = Instance.SharedPreferences?.GetUserInterfaceDroid();

					librarynavigationtype = librarynavigationsettingsdroid?.NavigationType ?? IUserInterfaceSettingsDroid.Defaults.NavigationType;
				}

				return librarynavigationtype switch
				{
					LibraryNavigationTypes.Drawer => new Intent(context, typeof(Activities.LibraryActivityDrawerLayout)),
					LibraryNavigationTypes.Tabs => new Intent(context, typeof(Activities.LibraryActivityTabLayout)),

					_ => new Intent(context, typeof(Activities.LibraryActivityTabLayout)),
				};
			}	
		}
	}
}