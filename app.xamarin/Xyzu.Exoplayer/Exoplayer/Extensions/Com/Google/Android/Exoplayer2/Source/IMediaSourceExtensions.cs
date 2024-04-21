#nullable enable

using Xyzu.Player;

namespace Com.Google.Android.Exoplayer2.Source
{
	public static class IMediaSourceExtensions
	{
		public static bool Equals(this IMediaSource mediasource, IQueueItem queueItem)
		{
			return
				mediasource.Tag != null &&
				queueItem.PrimaryId == mediasource.Tag.ToString();
		}
	}
}