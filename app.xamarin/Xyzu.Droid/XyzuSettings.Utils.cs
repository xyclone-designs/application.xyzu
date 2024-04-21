#nullable enable

using Android.Content;

using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;

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
					ILibrarySettingsDroid? librarynavigationsettingsdroid = Instance.SharedPreferences?.GetUserInterfaceLibraryDroid();

					librarynavigationtype = librarynavigationsettingsdroid?.NavigationType ?? ILibrarySettingsDroid.Defaults.NavigationType;
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