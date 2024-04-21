#nullable enable

using Android.Content;
using Android.Graphics.Drawables;

using System;

using Xyzu.Droid;

namespace Xyzu.Player.Enums
{
	public static class ShuffleModesExtensions
	{  
		public static int AsResoureIdTitle(this ShuffleModes shufflemode)
		{
			return shufflemode switch
			{
				ShuffleModes.NoShuffle => Resource.String.enums_shufflemodes_noshuffle_title,
				ShuffleModes.ShuffleSubQueues => Resource.String.enums_shufflemodes_shufflesubqueues_title,
				ShuffleModes.ShuffleSubQueueSongs => Resource.String.enums_shufflemodes_shufflesubqueuesongs_title,
				ShuffleModes.ShuffleAll => Resource.String.enums_shufflemodes_shuffleall_title,

				_ => throw new ArgumentException()
			};
		}	
		public static int AsResoureIdDescription(this ShuffleModes shufflemode)
		{
			return shufflemode switch
			{
				ShuffleModes.NoShuffle => Resource.String.enums_shufflemodes_noshuffle_description,
				ShuffleModes.ShuffleSubQueues => Resource.String.enums_shufflemodes_shufflesubqueues_description,
				ShuffleModes.ShuffleSubQueueSongs => Resource.String.enums_shufflemodes_shufflesubqueuesongs_description,
				ShuffleModes.ShuffleAll => Resource.String.enums_shufflemodes_shuffleall_description,

				_ => throw new ArgumentException()
			};
		}		
		public static int AsResoureIdDrawable(this ShuffleModes shufflemode)
		{
			return shufflemode switch
			{
				ShuffleModes.NoShuffle => Resource.Drawable.icon_player_shufflemode_noshuffle,
				ShuffleModes.ShuffleSubQueues => Resource.Drawable.icon_player_shufflemode_shufflesubqueues,
				ShuffleModes.ShuffleSubQueueSongs => Resource.Drawable.icon_player_shufflemode_shufflesubqueuesongs,
				ShuffleModes.ShuffleAll => Resource.Drawable.icon_player_shufflemode_shuffleall,

				_ => throw new ArgumentException()
			};
		}
		public static string? AsStringTitle(this ShuffleModes shufflemode, Context? context)
		{
			if (shufflemode.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this ShuffleModes shufflemode, Context? context)
		{
			if (shufflemode.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}

		public static Drawable? AsDrawable(this ShuffleModes shufflemode, Context? context)
		{
			if (shufflemode.AsResoureIdDrawable() is int resourcedrawable)
				return context?.Resources?.GetDrawable(resourcedrawable, context.Theme);

			return null;
		}
	}
}