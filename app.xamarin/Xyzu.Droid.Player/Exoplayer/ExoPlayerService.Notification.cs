using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidX.Core.App;
using AndroidX.Media3.Session;
using AndroidX.Media3.UI;

using Java.Lang;

using Xyzu.Droid.Player;

using Media3Player = AndroidX.Media3.Common.IPlayer;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService : PlayerNotificationManager.INotificationListener, PlayerNotificationManager.IMediaDescriptionAdapter
	{
		public const int NotificationId = 7;
		public const string NotificationChannelId = "XyzuMusicPlaybackNotifiationChannel";
		public const string NotificationChannelName = "Xyzu Music Playback Notifiation Channel";
		public static readonly int[] NotificationChannelActionIndicesForCompactView = new int[] { 1, 3 };

		private NotificationChannel? _NotificationChannel;
		private ExoPlayerNotificationManager? _NotificationManager;

		private NotificationCompat.Action? _NotificationActionPrevious;
		private NotificationCompat.Action? _NotificationActionPause;
		private NotificationCompat.Action? _NotificationActionPlay;
		private NotificationCompat.Action? _NotificationActionNext;
		private NotificationCompat.Action? _NotificationActionDestroy;	

		public NotificationCompat.Action? NotificationActionPrevious
		{
			set => _NotificationActionPrevious = value;
			get => _NotificationActionPrevious ??= new NotificationCompat.Action(
				title: Intents.Actions.Previous,
				intent: PendingIntent.GetService(this, 0, new Intent(this, typeof(ExoPlayerService)).SetAction(Intents.Actions.Previous), PendingIntentFlags.UpdateCurrent),
				icon:
					Options?.Notification?.Icons != null &&
					Options.Notification.Icons.TryGetValue(IPlayerService.IOptions.INotificationOptions.IconKeys.Previous, out int iconres)
						? iconres
						: Resource.Drawable.exo_notification_previous);
		}
		public NotificationCompat.Action? NotificationActionPause
		{
			set => _NotificationActionPause = value;
			get => _NotificationActionPause ??= new NotificationCompat.Action(
				title: Intents.Actions.Pause,
				intent: PendingIntent.GetService(this, 0, new Intent(this, typeof(ExoPlayerService)).SetAction(Intents.Actions.PlayPause), PendingIntentFlags.UpdateCurrent),
				icon:
					Options?.Notification?.Icons != null &&
					Options.Notification.Icons.TryGetValue(IPlayerService.IOptions.INotificationOptions.IconKeys.Pause, out int iconres)
						? iconres
						: Resource.Drawable.exo_notification_pause);
		}
		public NotificationCompat.Action? NotificationActionPlay
		{
			set => _NotificationActionPlay = value;
			get => _NotificationActionPlay ??= new NotificationCompat.Action(
				title: Intents.Actions.Play,
				intent: PendingIntent.GetService(this, 0, new Intent(this, typeof(ExoPlayerService)).SetAction(Intents.Actions.PlayPause), PendingIntentFlags.UpdateCurrent),
				icon:
					Options?.Notification?.Icons != null &&
					Options.Notification.Icons.TryGetValue(IPlayerService.IOptions.INotificationOptions.IconKeys.Play, out int iconres)
						? iconres
						: Resource.Drawable.exo_notification_play);
		}
		public NotificationCompat.Action? NotificationActionNext
		{
			set => _NotificationActionNext = value;
			get => _NotificationActionNext ??= new NotificationCompat.Action(
				title: Intents.Actions.Next,
				intent: PendingIntent.GetService(this, 0, new Intent(this, typeof(ExoPlayerService)).SetAction(Intents.Actions.Next), PendingIntentFlags.UpdateCurrent),
				icon:
					Options?.Notification?.Icons != null &&
					Options.Notification.Icons.TryGetValue(IPlayerService.IOptions.INotificationOptions.IconKeys.Next, out int iconres)
						? iconres
						: Resource.Drawable.exo_notification_next);
		}
		public NotificationCompat.Action? NotificationActionDestroy
		{
			set => _NotificationActionDestroy = value;
			get => _NotificationActionDestroy ??= new NotificationCompat.Action(
				title: Intents.Actions.Destroy,
				intent: PendingIntent.GetService(this, 0, new Intent(this, typeof(ExoPlayerService)).SetAction(Intents.Actions.Destroy), PendingIntentFlags.UpdateCurrent),
				icon:
					Options?.Notification?.Icons != null &&
					Options.Notification.Icons.TryGetValue(IPlayerService.IOptions.INotificationOptions.IconKeys.Destroy, out int iconres)
						? iconres
						: Resource.Drawable.exo_notification_stop);
		}

		public NotificationChannel NotificationChannel
		{
			get
			{
				if (_NotificationChannel is null && GetSystemService(NotificationService) is NotificationManager notificationmanager)
				{
					if (notificationmanager.GetNotificationChannel(NotificationChannelId) is NotificationChannel notificationchannel)
						_NotificationChannel = notificationchannel;
					else  notificationmanager.CreateNotificationChannel(
							_NotificationChannel = new NotificationChannel(NotificationChannelId, NotificationChannelName, NotificationImportance.Max));
				}

				return _NotificationChannel ??= new NotificationChannel(NotificationChannelId, NotificationChannelName, NotificationImportance.Max);
			}
		}
		public ExoPlayerNotificationManager NotificationManager
		{
			get => _NotificationManager ??= 
				new ExoPlayerNotificationManager.Builder(
				context: this,
				notificationId: NotificationId,
				channelId: NotificationChannel.Id ?? NotificationChannelId,
				onbuild: (notificationmanager, notificationmanagerbuilder) =>
				{
					if (notificationmanagerbuilder is not null)
					{
						notificationmanagerbuilder.SetNotificationListener(this);
						notificationmanagerbuilder.SetMediaDescriptionAdapter(this);
					}

					if (notificationmanager is not null)
					{
						notificationmanager.SetMediaSessionToken(MediaSession.SessionCompatToken);
						notificationmanager.ActionIndicesForCompactView = NotificationChannelActionIndicesForCompactView;
						notificationmanager.OnUpdate = (player, builder, ongoing, largeIcon) =>
						{
							if (builder is null)
								return;

							builder.ClearActions();
							builder.AddAction(NotificationActionPrevious);
							builder.AddAction(player?.IsPlaying ?? false ? NotificationActionPause : NotificationActionPlay);
							builder.AddAction(NotificationActionNext);
							builder.AddAction(NotificationActionDestroy);

							if (Options?.Notification?.ContentIntent != null)
								builder.SetContentIntent(Options.Notification.ContentIntent);

							if (Options?.Notification?.Icons != null && Options.Notification.Icons.TryGetValue(IPlayerService.IOptions.INotificationOptions.IconKeys.SmallIcon, out int iconres))
								builder.SetSmallIcon(iconres);

							ICharSequence? currentcontenttextformatted = GetCurrentContentTextFormatted(player);
							ICharSequence? currentcontenttitleformatted = GetCurrentContentTitleFormatted(player);
							Bitmap? currentlargeicon = GetCurrentLargeIcon(player, null);

							builder.SetContentText(currentcontenttextformatted);
							builder.SetContentTitle(currentcontenttitleformatted);
							builder.SetLargeIcon(currentlargeicon);
						};
					}

				}).Build();
		}

		public PendingIntent? CreateCurrentContentIntent(Media3Player? player)
		{
			return Options?.Notification?.ContentIntent;
		}
		public ICharSequence? GetCurrentContentTextFormatted(Media3Player? player)
		{
			string? artist = Player.Queue.CurrentSong?.Artist ?? Options.Notification?.DefaultArtist;
			string? album = Player.Queue.CurrentSong?.Album ?? Options.Notification?.DefaultAlbum;

			string contenttext =
				string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(album)
					? string.Empty
					: string.Format("{0} - {1}", artist, album);

			return new Java.Lang.String(contenttext);
		}
		public ICharSequence? GetCurrentContentTitleFormatted(Media3Player? player)
		{
			string contenttitle =
				Player.Queue.CurrentSong?.Title ??
				Options.Notification?.DefaultTitle ??
				string.Empty;

			return new Java.Lang.String(contenttitle);
		}
		public Bitmap? GetCurrentLargeIcon(Media3Player? player, PlayerNotificationManager.BitmapCallback? bitmapcallback)
		{
			Bitmap? bitmap = Options.Notification?.OnBitmap?.Invoke(new BitmapFactory.Options { }, Player.Queue.CurrentSong);

			return bitmap;
		}

		void PlayerNotificationManager.INotificationListener.OnNotificationCancelled(int notificationid, bool dismissedbyuser)
		{
			StopForeground(StopForegroundFlags.Remove);
		}
		void PlayerNotificationManager.INotificationListener.OnNotificationPosted(int notificationid, Notification? notification, bool ongoing)
		{
			if (notification != null)
				notification.Flags =
					NotificationFlags.AutoCancel |
					NotificationFlags.ForegroundService |
					NotificationFlags.HighPriority |
					NotificationFlags.OnlyAlertOnce |
					NotificationFlags.OngoingEvent;

			StartForeground(notificationid, notification);
		}
	}
}