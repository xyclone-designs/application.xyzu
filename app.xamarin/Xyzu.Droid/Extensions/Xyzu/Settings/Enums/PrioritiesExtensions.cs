#nullable enable

using Android.App;
using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Settings.Enums
{
	public static class PrioritiesExtensions
	{
		public static int ToNotificationPriority(this Priorities priority, int @default)
		{
			return priority switch
			{
				Priorities.High => (int)NotificationPriority.High,
				Priorities.Highest => (int)NotificationPriority.Max,
				Priorities.Low => (int)NotificationPriority.Low,
				Priorities.Lowest => (int)NotificationPriority.Min,
				Priorities.Medium => (int)NotificationPriority.Default,

				_ => @default
			};
		}
		public static NotificationPriority ToNotificationPriority(this Priorities priority, NotificationPriority @default)
		{
			return priority switch
			{
				Priorities.High => NotificationPriority.High,
				Priorities.Highest => NotificationPriority.Max,
				Priorities.Low => NotificationPriority.Low,
				Priorities.Lowest => NotificationPriority.Min,
				Priorities.Medium => NotificationPriority.Default,

				_ => @default
			};
		}	   

		public static int AsResoureIdTitle(this Priorities priority)
		{
			return priority switch
			{
				Priorities.High => Resource.String.enums_priorities_high_title,
				Priorities.Highest => Resource.String.enums_priorities_highest_title,
				Priorities.Low => Resource.String.enums_priorities_low_title,
				Priorities.Lowest => Resource.String.enums_priorities_lowest_title,
				Priorities.Medium => Resource.String.enums_priorities_medium_title,

				_ => throw new ArgumentException(string.Format("Invalid Priorities '{0}'", priority))
			};
		}	
		public static int AsResoureIdDescription(this Priorities priority)
		{
			return priority switch
			{
				Priorities.High => Resource.String.enums_priorities_high_description,
				Priorities.Highest => Resource.String.enums_priorities_highest_description,	
				Priorities.Low => Resource.String.enums_priorities_low_description,
				Priorities.Lowest => Resource.String.enums_priorities_lowest_description,
				Priorities.Medium => Resource.String.enums_priorities_medium_description,

				_ => throw new ArgumentException(string.Format("Invalid Priorities '{0}'", priority))
			};
		}
		public static string? AsStringTitle(this Priorities priority, Context? context)
		{
			if (priority.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this Priorities priority, Context? context)
		{
			if (priority.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}