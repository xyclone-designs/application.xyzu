#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using AndroidX.Core.App;
using AndroidX.Media3.Common;

using System;
using System.Collections.Generic;

namespace AndroidX.Media3.UI
{
	public class ExoPlayerNotificationManager : PlayerNotificationManager
	{
		protected ExoPlayerNotificationManager(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
		protected ExoPlayerNotificationManager(Context? context, string? channelId, int notificationId, IMediaDescriptionAdapter? mediaDescriptionAdapter, INotificationListener? notificationListener, ICustomActionReceiver? customActionReceiver, int smallIconResourceId, int playActionIconResourceId, int pauseActionIconResourceId, int stopActionIconResourceId, int rewindActionIconResourceId, int fastForwardActionIconResourceId, int previousActionIconResourceId, int nextActionIconResourceId, string? groupKey) : base(context, channelId, notificationId, mediaDescriptionAdapter, notificationListener, customActionReceiver, smallIconResourceId, playActionIconResourceId, pauseActionIconResourceId, stopActionIconResourceId, rewindActionIconResourceId, fastForwardActionIconResourceId, previousActionIconResourceId, nextActionIconResourceId, groupKey) { }

		public Bitmap? NotificationLargeIcon { get; set; }
		public NotificationCompat.Builder? NotificationBuilder { get; set; }
		public bool? NotificationOnGoing { get; set; }
		public IPlayer? NotificationExoPlayer { get; set; }
		public int[]? ActionIndicesForCompactView { get; set; }
		public Action<IPlayer?, NotificationCompat.Builder?, bool, Bitmap?>? OnUpdate { get; set; }

		protected override int[]? GetActionIndicesForCompactView(IList<string>? actionNames, IPlayer? player)
		{
			return ActionIndicesForCompactView ?? base.GetActionIndicesForCompactView(actionNames, player);
		}
		protected override NotificationCompat.Builder? CreateNotification(IPlayer? player, NotificationCompat.Builder? builder, bool ongoing, Bitmap? largeIcon)
		{
			NotificationBuilder ??= base.CreateNotification(player, builder, ongoing, largeIcon)?
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

		public new class Builder : PlayerNotificationManager.Builder
		{
			public Builder(Context? context, int notificationId, string? channelId) : this(context, notificationId, channelId, (_, _) => { }) { }
			public Builder(Context? context, int notificationId, string? channelId, Action<ExoPlayerNotificationManager?, Builder?> onbuild) : base(context, notificationId, channelId) 
			{
				OnBuild = onbuild;
			}

			public Action<ExoPlayerNotificationManager?, Builder?> OnBuild { get; set; }

			public new ExoPlayerNotificationManager Build()
			{
				OnBuild.Invoke(null, this);

				ExoPlayerNotificationManager _ = new (
					Context, 
					ChannelId, 
					NotificationId, 
					MediaDescriptionAdapter, 
					NotificationListener, 
					CustomActionReceiver,
					SmallIconResourceId, 
					PlayActionIconResourceId, 
					PauseActionIconResourceId, 
					StopActionIconResourceId, 
					RewindActionIconResourceId, 
					FastForwardActionIconResourceId, 
					PreviousActionIconResourceId, 
					NextActionIconResourceId, 
					GroupKey);

				OnBuild.Invoke(_, null);

				return _;
			}
		}
	}
}