#nullable enable

using AndroidX.Media3.Common;

using System;

using Xyzu.Player;

using AndroidUri = Android.Net.Uri;
using SystemUri = System.Uri;

namespace AndroidX.Media3.ExoPlayer.Source
{
	public static class IMediaSourceFactoryExtensions
	{
		public static IMediaSource? CreateMediaSource(this IMediaSourceFactory mediasourcefactory, IQueueItem queueitem)
		{
			return queueitem.Uri is null
				? null
				: mediasourcefactory.CreateMediaSource(queueitem.Uri);
		}
		public static IMediaSource? CreateMediaSource(this IMediaSourceFactory mediasourcefactory, SystemUri uri)
		{
			if (uri.ToAndroidUri() is AndroidUri androiduri && MediaItem.FromUri(androiduri) is MediaItem mediaitem)
				return mediasourcefactory.CreateMediaSource(mediaitem);

			return null;
		}
	}
}