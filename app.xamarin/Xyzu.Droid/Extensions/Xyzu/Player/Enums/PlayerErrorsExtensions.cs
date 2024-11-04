using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Player.Enums
{
	public static class PlayerErrorsExtensions
	{  
		public static int AsResoureIdTitle(this PlayerErrors shufflemode)
		{
			return shufflemode switch
			{
				PlayerErrors.Load => Resource.String.enums_playererrors_load_title,
				PlayerErrors.Play => Resource.String.enums_playererrors_play_title,
				PlayerErrors.Source => Resource.String.enums_playererrors_source_title,

				_ => throw new ArgumentException()
			};
		}	
		public static int AsResoureIdDescription(this PlayerErrors shufflemode)
		{
			return shufflemode switch
			{
				PlayerErrors.Load => Resource.String.enums_playererrors_load_description,
				PlayerErrors.Play => Resource.String.enums_playererrors_play_description,
				PlayerErrors.Source => Resource.String.enums_playererrors_source_description,

				_ => throw new ArgumentException()
			};
		}		
		public static string? AsStringTitle(this PlayerErrors shufflemode, Context? context)
		{
			if (shufflemode.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this PlayerErrors shufflemode, Context? context)
		{
			if (shufflemode.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}