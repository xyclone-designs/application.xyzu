
namespace Xyzu.Player.Enums
{
	public enum RepeatModes
	{
		NoRepeat,
		RepeatSong,
		RepeatSubQueue,
		RepeatEntireQueue,
	}

	public static class RepeatModesExtensions
	{
		public static RepeatModes Next(this RepeatModes repeatmode)
		{
			return repeatmode switch
			{
				RepeatModes.NoRepeat => RepeatModes.RepeatSong,
				RepeatModes.RepeatSong => RepeatModes.RepeatSubQueue,
				RepeatModes.RepeatSubQueue => RepeatModes.RepeatEntireQueue,
				RepeatModes.RepeatEntireQueue => RepeatModes.NoRepeat,

				_ => RepeatModes.NoRepeat,
			};
		}
		public static RepeatModes Previous(this RepeatModes repeatmode)
		{
			return repeatmode switch
			{
				 RepeatModes.RepeatSong => RepeatModes.NoRepeat,
				 RepeatModes.RepeatSubQueue => RepeatModes.RepeatSong,
				 RepeatModes.RepeatEntireQueue => RepeatModes.RepeatSubQueue,
				 RepeatModes.NoRepeat => RepeatModes.RepeatEntireQueue,

				_ => RepeatModes.NoRepeat,
			};
		}
	}
}