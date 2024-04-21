#nullable enable

using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Settings.Enums
{
	public enum LibraryNavigationTypes
	{
		Drawer,
		Tabs,
	}

	public static class LibraryNavigationTypesExtensions
	{
		public static int AsResoureIdTitle(this LibraryNavigationTypes librarynavigationtype)
		{
			return librarynavigationtype switch
			{
				LibraryNavigationTypes.Drawer => Resource.String.enums_librarynavigationtypes_drawer_title,
				LibraryNavigationTypes.Tabs => Resource.String.enums_librarynavigationtypes_tabs_title,

				_ => throw new ArgumentException(string.Format("Invalid LibraryNavigationTypes '{0}'", librarynavigationtype))
			};
		}
		public static int AsResoureIdDescription(this LibraryNavigationTypes librarynavigationtype)
		{
			return librarynavigationtype switch
			{
				LibraryNavigationTypes.Drawer => Resource.String.enums_librarynavigationtypes_drawer_description,
				LibraryNavigationTypes.Tabs => Resource.String.enums_librarynavigationtypes_tabs_description,

				_ => throw new ArgumentException(string.Format("Invalid LibraryNavigationTypes '{0}'", librarynavigationtype))
			};
		}
		public static string? AsStringTitle(this LibraryNavigationTypes librarynavigationtypes, Context? context)
		{
			if (librarynavigationtypes.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this LibraryNavigationTypes librarynavigationtypes, Context? context)
		{
			if (librarynavigationtypes.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}