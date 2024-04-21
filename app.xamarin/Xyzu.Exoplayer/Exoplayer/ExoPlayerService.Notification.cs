#nullable enable

using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidX.Core.App;

using Com.Google.Android.Exoplayer2.Ext.Mediasession;
using Com.Google.Android.Exoplayer2.UI;

using System.Collections.Generic;

using Xyzu.Exoplayer;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService : PlayerNotificationManager.INotificationListener, PlayerNotificationManager.IMediaDescriptionAdapter
	{
		public const int NotificationId = 7;
		public const string NotificationChannelId = "XyzuMusicPlaybackNotifiationChannel";
		public const string NotificationChannelName = "Xyzu Music Playback Notifiation Channel";

		private bool NotificationInited = false;
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
					Options.Notification.Icons.TryGetValue(IPlayerServiceOptions.INotificationOptions.IconKeys.Previous, out int iconres)
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
					Options.Notification.Icons.TryGetValue(IPlayerServiceOptions.INotificationOptions.IconKeys.Pause, out int iconres)
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
					Options.Notification.Icons.TryGetValue(IPlayerServiceOptions.INotificationOptions.IconKeys.Play, out int iconres)
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
					Options.Notification.Icons.TryGetValue(IPlayerServiceOptions.INotificationOptions.IconKeys.Next, out int iconres)
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
					Options.Notification.Icons.TryGetValue(IPlayerServiceOptions.INotificationOptions.IconKeys.Destroy, out int iconres)
						? iconres
						: Resource.Drawable.exo_notification_stop);
		}

		public NotificationChannel NotificationChannel
		{
			get
			{
				if (_NotificationChannel is null && GetSystemService(NotificationService) is NotificationManager notificationmanager)
				{
					_NotificationChannel = notificationmanager.GetNotificationChannel(NotificationChannelId);

					if (_NotificationChannel is null)
						notificationmanager.CreateNotificationChannel(
							_NotificationChannel = new NotificationChannel(NotificationChannelId, NotificationChannelName, NotificationImportance.Max));
				}

				return _NotificationChannel ??= new NotificationChannel(NotificationChannelId, NotificationChannelName, NotificationImportance.Max);
			}
		}
		public ExoPlayerNotificationManager NotificationManager
		{
			get => _NotificationManager ??= 
				new ExoPlayerNotificationManager(
				context: this,
				notificationlistener: this,
				mediadescriptionadapter: this,
				notificationId: NotificationId,
				channelId: NotificationChannel.Id ?? NotificationChannelId)
				{
					ActionIndicesForCompactView = new int[] { 1, 3 },
					MediaSessionToken = MediaSessionConnector.MediaSession.SessionToken,
					OnUpdate = (player, builder, ongoing, largeIcon) =>
					{
						builder.MActions ??= new List<NotificationCompat.Action>();

						if (NotificationInited is true)
							builder.MActions[1] = player.IsPlaying ? NotificationActionPause : NotificationActionPlay;
						else
						{
							NotificationInited = true;

							builder.MActions.Clear();

							builder.MActions.Add(NotificationActionPrevious);
							builder.MActions.Add(player.IsPlaying ? NotificationActionPause : NotificationActionPlay);
							builder.MActions.Add(NotificationActionNext);
							builder.MActions.Add(NotificationActionDestroy);

							if (Options?.Notification?.ContentIntent != null)
								builder.SetContentIntent(Options.Notification.ContentIntent);

							if (Options?.Notification?.Icons != null && Options.Notification.Icons.TryGetValue(IPlayerServiceOptions.INotificationOptions.IconKeys.SmallIcon, out int iconres))
								builder.SetSmallIcon(iconres);
						}

						Java.Lang.ICharSequence currentcontenttextformatted = (this as PlayerNotificationManager.IMediaDescriptionAdapter).GetCurrentContentTextFormatted(player);
						Java.Lang.ICharSequence currentcontenttitleformatted = (this as PlayerNotificationManager.IMediaDescriptionAdapter).GetCurrentContentTitleFormatted(player);
						Java.Lang.ICharSequence currentsubtextformatted = (this as PlayerNotificationManager.IMediaDescriptionAdapter).GetCurrentSubTextFormatted(player);
						Bitmap currentlargeicon = (this as PlayerNotificationManager.IMediaDescriptionAdapter).GetCurrentLargeIcon(player, null);

						builder.SetContentText(currentcontenttextformatted);
						builder.SetContentTitle(currentcontenttitleformatted);
						builder.SetSubText(currentsubtextformatted);
						builder.SetLargeIcon(currentlargeicon);
					},
				};
		}

		void PlayerNotificationManager.INotificationListener.OnNotificationCancelled(int notificationid, bool dismissedbyuser)
		{
			StopForeground(dismissedbyuser);
		}
		void PlayerNotificationManager.INotificationListener.OnNotificationStarted(int notificationid, Notification notification)
		{
			StartForeground(notificationid, notification);
		}
		void PlayerNotificationManager.INotificationListener.OnNotificationPosted(int notificationid, Notification notification, bool ongoing) 
		{
			notification.Flags =
				NotificationFlags.AutoCancel |
				NotificationFlags.ForegroundService |
				NotificationFlags.HighPriority |
				//NotificationFlags.NoClear |
				NotificationFlags.OnlyAlertOnce |
				NotificationFlags.OngoingEvent;
		}

		PendingIntent PlayerNotificationManager.IMediaDescriptionAdapter.CreateCurrentContentIntent(Com.Google.Android.Exoplayer2.IPlayer player)
		{
			return Options?.Notification?.ContentIntent;
		}
		Java.Lang.ICharSequence PlayerNotificationManager.IMediaDescriptionAdapter.GetCurrentContentTextFormatted(Com.Google.Android.Exoplayer2.IPlayer player)
		{
			string? artist = Player.Queue.CurrentSong?.Artist ?? Options.Notification?.DefaultArtist;
			string? album = Player.Queue.CurrentSong?.Album ?? Options.Notification?.DefaultAlbum;

			string contenttext =
				string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(album)
					? string.Empty
					: string.Format("{0} - {1}", artist, album);

			return new Java.Lang.String(contenttext);
		}
		Java.Lang.ICharSequence PlayerNotificationManager.IMediaDescriptionAdapter.GetCurrentContentTitleFormatted(Com.Google.Android.Exoplayer2.IPlayer player)
		{
			string contenttitle = 
				Player.Queue.CurrentSong?.Title ?? 
				Options.Notification?.DefaultTitle ?? 
				string.Empty;

			return new Java.Lang.String(contenttitle);
		}
		Java.Lang.ICharSequence PlayerNotificationManager.IMediaDescriptionAdapter.GetCurrentSubTextFormatted(Com.Google.Android.Exoplayer2.IPlayer player)
		{
			string subtext = string.Empty;

			return new Java.Lang.String(subtext);
		}
		Bitmap PlayerNotificationManager.IMediaDescriptionAdapter.GetCurrentLargeIcon(Com.Google.Android.Exoplayer2.IPlayer player, PlayerNotificationManager.BitmapCallback bitmapcallback)
		{
			Bitmap? bitmap = Options.Notification?.OnBitmap?.Invoke(new BitmapFactory.Options { }, Player.Queue.CurrentSong);
			
			return bitmap;
		}
	}
}