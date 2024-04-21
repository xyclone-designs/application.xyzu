#nullable enable

using Android.Content;
using AndroidX.Core.App;

using System;

using Xyzu.Droid;

namespace Xyzu.Settings.Enums
{
	public static class BadgeIconTypesExtensions
	{
		public static int ToNotifiationCompatInt(this BadgeIconTypes badgeicontype, int @default)
		{
			return badgeicontype switch
			{
				BadgeIconTypes.None => NotificationCompat.BadgeIconNone,
				BadgeIconTypes.Small => NotificationCompat.BadgeIconSmall,
				BadgeIconTypes.Large => NotificationCompat.BadgeIconLarge,

				_ => @default
			};
		}

		public static int AsResoureIdTitle(this BadgeIconTypes badgeicontype)
		{
			return badgeicontype switch
			{
				BadgeIconTypes.None => Resource.String.enums_badgeicontypes_none_title,
				BadgeIconTypes.Small => Resource.String.enums_badgeicontypes_small_title,
				BadgeIconTypes.Large => Resource.String.enums_badgeicontypes_large_title,

				_ => throw new ArgumentException(string.Format("Invalid BadgeIconTypes '{0}'", badgeicontype))
			};
		}	
		public static int AsResoureIdDescription(this BadgeIconTypes badgeicontype)
		{
			return badgeicontype switch
			{
				BadgeIconTypes.None => Resource.String.enums_badgeicontypes_none_description,
				BadgeIconTypes.Small => Resource.String.enums_badgeicontypes_small_description,
				BadgeIconTypes.Large => Resource.String.enums_badgeicontypes_large_description,

				_ => throw new ArgumentException(string.Format("Invalid BadgeIconTypes '{0}'", badgeicontype))
			};
		}
		public static string? AsStringTitle(this BadgeIconTypes badgeicontypes, Context? context)
		{
			if (badgeicontypes.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this BadgeIconTypes badgeicontypes, Context? context)
		{
			if (badgeicontypes.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}