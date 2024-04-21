#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Support.V4.Media.Session;
using AndroidX.Core.App;

using System;
using System.Collections.Generic;

using IExoPlayer2Player = Com.Google.Android.Exoplayer2.IPlayer;

namespace Com.Google.Android.Exoplayer2.UI
{
	public class ExoPlayerNotificationManager : PlayerNotificationManager
	{
		public ExoPlayerNotificationManager(Context context, string channelId, int notificationId, IMediaDescriptionAdapter mediadescriptionadapter) : base(context, channelId, notificationId, mediadescriptionadapter) { }
		public ExoPlayerNotificationManager(Context context, string channelId, int notificationId, IMediaDescriptionAdapter mediadescriptionadapter, INotificationListener notificationlistener) : base(context, channelId, notificationId, mediadescriptionadapter, notificationlistener) { }
		public ExoPlayerNotificationManager(Context context, string channelId, int notificationId, IMediaDescriptionAdapter mediadescriptionadapter, ICustomActionReceiver customactionreceiver) : base(context, channelId, notificationId, mediadescriptionadapter, customactionreceiver) { }
		public ExoPlayerNotificationManager(Context context, string channelId, int notificationId, IMediaDescriptionAdapter mediadescriptionadapter, INotificationListener notificationlistener, ICustomActionReceiver customactionreceiver) : base(context, channelId, notificationId, mediadescriptionadapter, notificationlistener, customactionreceiver) { }

		public Bitmap? NotificationLargeIcon { get; set; }
		public NotificationCompat.Builder? NotificationBuilder { get; set; }
		public bool? NotificationOnGoing { get; set; }
		public IExoPlayer2Player? NotificationExoPlayer { get; set; }
		public int[]? ActionIndicesForCompactView { get; set; }
		public MediaSessionCompat.Token MediaSessionToken
		{
			set => SetMediaSessionToken(value);
		}
		public Action<IExoPlayer2Player, NotificationCompat.Builder, bool, Bitmap>? OnUpdate { get; set; }

		protected override int[] GetActionIndicesForCompactView(IList<string> actionNames, IExoPlayer2Player player)
		{
			return ActionIndicesForCompactView ?? base.GetActionIndicesForCompactView(actionNames, player);
		}
		protected override NotificationCompat.Builder CreateNotification(IExoPlayer2Player player, NotificationCompat.Builder builder, bool ongoing, Bitmap largeIcon)
		{
			NotificationBuilder ??= base.CreateNotification(player, builder, ongoing, largeIcon)
				.SetAllowSystemGeneratedContextualActions(false)
				.SetAutoCancel(true)
				.SetBadgeIconType(NotificationCompat.BadgeIconSmall)
				.SetColorized(true)
				.SetDefaults(NotificationCompat.DefaultAll)
				.SetPriority(NotificationCompat.PriorityMax)
				.SetSilent(true)
				.SetUsesChronometer(false)
				.SetVisibility(NotificationCompat.VisibilityPublic);

			NotificationLargeIcon = largeIcon;
			NotificationOnGoing = ongoing;
			NotificationExoPlayer = player;

			OnUpdate?.Invoke(player, builder ?? NotificationBuilder, ongoing, largeIcon);

			return NotificationBuilder;
		}
	}
}