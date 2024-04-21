#nullable enable

using Android.Content;
using Android.Graphics.Drawables;

using System;

using Xyzu.Droid;

namespace Xyzu.Player.Enums
{
	public static class RepeatModesExtensions
	{  
		public static int AsResoureIdTitle(this RepeatModes repeatmode)
		{
			return repeatmode switch
			{
				RepeatModes.NoRepeat => Resource.String.enums_repeatmodes_norepeat_title,
				RepeatModes.RepeatEntireQueue => Resource.String.enums_repeatmodes_repeatentirequeue_title,
				RepeatModes.RepeatSong => Resource.String.enums_repeatmodes_repeatsong_title,
				RepeatModes.RepeatSubQueue => Resource.String.enums_repeatmodes_repeatsubqueue_title,

				_ => throw new ArgumentException()
			};
		}	
		public static int AsResoureIdDescription(this RepeatModes repeatmode)
		{
			return repeatmode switch
			{
				RepeatModes.NoRepeat => Resource.String.enums_repeatmodes_norepeat_description,
				RepeatModes.RepeatEntireQueue => Resource.String.enums_repeatmodes_repeatentirequeue_description,
				RepeatModes.RepeatSong => Resource.String.enums_repeatmodes_repeatsong_description,
				RepeatModes.RepeatSubQueue => Resource.String.enums_repeatmodes_repeatsubqueue_description,

				_ => throw new ArgumentException()
			};
		}				 				
		public static int AsResoureIdDrawable(this RepeatModes repeatmode)
		{
			return repeatmode switch
			{
				RepeatModes.NoRepeat => Resource.Drawable.icon_player_repeatmode_norepeat,
				RepeatModes.RepeatEntireQueue => Resource.Drawable.exo_media_action_repeat_all,
				RepeatModes.RepeatSong => Resource.Drawable.exo_controls_repeat_one,
				RepeatModes.RepeatSubQueue => Resource.Drawable.exo_media_action_repeat_all,

				_ => throw new ArgumentException()
			};
		}

		public static string? AsStringTitle(this RepeatModes repeatmode, Context? context)
		{
			if (repeatmode.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this RepeatModes repeatmode, Context? context)
		{
			if (repeatmode.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}									  
		public static Drawable? AsDrawable(this RepeatModes repeatmode, Context? context)
		{
			if (repeatmode.AsResoureIdDrawable() is int resourcedrawable)
				return context?.Resources?.GetDrawable(resourcedrawable, context.Theme);

			return null;
		}
	}
}