
namespace Xyzu.Player.Enums
{
	public enum ShuffleModes
	{
        NoShuffle,
        ShuffleAll,
        ShuffleSubQueues,
        ShuffleSubQueueSongs
    }

    public static class ShuffleModesExtensions
	{
        public static ShuffleModes Next(this ShuffleModes shufflemode)
		{
            return shufflemode switch
            {
                ShuffleModes.NoShuffle => ShuffleModes.ShuffleSubQueues,
                ShuffleModes.ShuffleSubQueues => ShuffleModes.ShuffleSubQueueSongs,
                ShuffleModes.ShuffleSubQueueSongs => ShuffleModes.ShuffleAll,
                ShuffleModes.ShuffleAll => ShuffleModes.NoShuffle,

                _ => ShuffleModes.NoShuffle
            };
		}   
        public static ShuffleModes Previous(this ShuffleModes shufflemode)
		{
            return shufflemode switch
            {
                ShuffleModes.ShuffleSubQueues => ShuffleModes.NoShuffle,
                ShuffleModes.ShuffleSubQueueSongs => ShuffleModes.ShuffleSubQueues,
                ShuffleModes.ShuffleAll => ShuffleModes.ShuffleSubQueueSongs,
                ShuffleModes.NoShuffle => ShuffleModes.ShuffleAll,

                _ => ShuffleModes.NoShuffle
            };
		}
	}
}