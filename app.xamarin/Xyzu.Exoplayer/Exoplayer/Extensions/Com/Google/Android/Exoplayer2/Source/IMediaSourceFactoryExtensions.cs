#nullable enable

using System;

using Xyzu.Player;

using AndroidUri = Android.Net.Uri;
using SystemUri = System.Uri;

namespace Com.Google.Android.Exoplayer2.Source
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
			if (uri.ToAndroidUri() is AndroidUri androiduri)
				return mediasourcefactory.CreateMediaSource(androiduri);

			return null;
		}
	}
}