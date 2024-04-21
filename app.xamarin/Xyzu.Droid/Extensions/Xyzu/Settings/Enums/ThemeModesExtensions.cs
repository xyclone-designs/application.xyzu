#nullable enable

using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Settings.Enums
{
	public static class ThemeModesExtensions
	{  
		public static int AsResoureIdTitle(this ThemeModes thememode)
		{
			return thememode switch
			{
				ThemeModes.FollowSystem => Resource.String.enums_thememodes_followsystem_title,
				ThemeModes.ForceDark => Resource.String.enums_thememodes_forcedark_title,
				ThemeModes.ForceLight => Resource.String.enums_thememodes_forcelight_title,

				_ => throw new ArgumentException(string.Format("Invalid ThemeModes '{0}'", thememode))
			};
		}	
		public static int AsResoureIdDescription(this ThemeModes thememode)
		{
			return thememode switch
			{
				ThemeModes.FollowSystem => Resource.String.enums_thememodes_followsystem_description,
				ThemeModes.ForceDark => Resource.String.enums_thememodes_forcedark_description,
				ThemeModes.ForceLight => Resource.String.enums_thememodes_forcelight_description,

				_ => throw new ArgumentException(string.Format("Invalid ThemeModes '{0}'", thememode))
			};
		}
		public static string? AsStringTitle(this ThemeModes thememodes, Context? context)
		{
			if (thememodes.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this ThemeModes thememodes, Context? context)
		{
			if (thememodes.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}