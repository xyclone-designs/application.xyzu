#nullable enable

using System;

using MediaDescriptionCompat = Android.Support.V4.Media.MediaDescriptionCompat;
using MediaSessionCompat = Android.Support.V4.Media.Session.MediaSessionCompat;

namespace Com.Google.Android.Exoplayer2.Ext.Mediasession
{
	public class MediaSessionQueueNavigator : TimelineQueueNavigator
	{
		public MediaSessionQueueNavigator(MediaSessionCompat mediaSessionCompat) : base(mediaSessionCompat) { }

		public Action<MediaDescriptionCompat.Builder, IPlayer, int>? OnGetMediaDescription { get; set; }

		public override MediaDescriptionCompat GetMediaDescription(IPlayer player, int windowIndex)
		{
			using MediaDescriptionCompat.Builder builder = new MediaDescriptionCompat.Builder();

			OnGetMediaDescription?.Invoke(builder, player, windowIndex);

			return builder.Build();
		}
	}
}