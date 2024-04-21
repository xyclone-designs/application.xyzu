#nullable enable

using Android.App;
using Android.Content;
using AndroidX.Core.App;

using Xyzu.Droid;

namespace Xyzu
{
	public sealed partial class XyzuLibrary 
	{
		public partial class ScannerService
		{
			public const int NotificationId = 9;
			public const string NotificationChannelId = "XyzuScannerNotifiationChannel";
			public const string NotificationChannelName = "Xyzu Scanner Notifiation Channel";

			NotificationChannel? _NotificationChannel;
			NotificationCompat.Builder? _NotificationBuilder;
			NotificationCompat.Action? _NotificationActionCancel;
			NotificationCompat.BigTextStyle? _NotificationCompatBigTextStyle;

			string? _NotificationContentText_Preparing;
			string? _NotificationContentText_Preparing_Albums;
			string? _NotificationContentText_Preparing_Artists;
			string? _NotificationContentText_Preparing_Genres;
			string? _NotificationContentText_Preparing_Playlists;
			string? _NotificationContentText_Preparing_Songs;
			string? _NotificationContentText_Scanning_Songs;
			string? _NotificationContentText_Reading_Albums;
			string? _NotificationContentText_Reading_Artists;
			string? _NotificationContentText_Reading_Genres;
			string? _NotificationContentText_Reading_Playlists;
			string? _NotificationContentText_Reading_Songs;
			string? _NotificationContentText_Removing_Albums;
			string? _NotificationContentText_Removing_Artists;
			string? _NotificationContentText_Removing_Genres;
			string? _NotificationContentText_Removing_Playlists;
			string? _NotificationContentText_Removing_Songs;
			string? _NotificationContentText_Saving_Albums;
			string? _NotificationContentText_Saving_Artists;
			string? _NotificationContentText_Saving_Genres;
			string? _NotificationContentText_Saving_Playlists;
			string? _NotificationContentText_Saving_Songs;
			string? _NotificationContentText_Scanning_Albums;
			string? _NotificationContentText_Scanning_Artists;
			string? _NotificationContentText_Scanning_Genres;
			string? _NotificationContentText_Scanning_Playlists;

			private string? NotificationContentText_Preparing
			{
				get => _NotificationContentText_Preparing ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing);
			}		  
			private string? NotificationContentText_Preparing_Albums
			{
				get => _NotificationContentText_Preparing_Albums ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_albums);
			}
			private string? NotificationContentText_Preparing_Artists
			{
				get => _NotificationContentText_Preparing_Artists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_artists);
			}
			private string? NotificationContentText_Preparing_Genres
			{
				get => _NotificationContentText_Preparing_Genres ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_genres);
			}
			private string? NotificationContentText_Preparing_Playlists
			{
				get => _NotificationContentText_Preparing_Playlists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_playlists);
			}
			private string? NotificationContentText_Preparing_Songs
			{
				get => _NotificationContentText_Preparing_Songs ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_songs);
			}

			private string? NotificationContentText_Reading_Albums
			{
				get => _NotificationContentText_Reading_Albums ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_albums);
			}
			private string? NotificationContentText_Reading_Artists
			{
				get => _NotificationContentText_Reading_Artists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_artists);
			}
			private string? NotificationContentText_Reading_Genres
			{
				get => _NotificationContentText_Reading_Genres ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_genres);
			}
			private string? NotificationContentText_Reading_Playlists
			{
				get => _NotificationContentText_Reading_Playlists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_playlists);
			}
			private string? NotificationContentText_Reading_Songs
			{
				get => _NotificationContentText_Reading_Songs ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_songs);
			}

			private string? NotificationContentText_Removing_Albums
			{
				get => _NotificationContentText_Removing_Albums ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_albums);
			}
			private string? NotificationContentText_Removing_Artists
			{
				get => _NotificationContentText_Removing_Artists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_artists);
			}
			private string? NotificationContentText_Removing_Genres
			{
				get => _NotificationContentText_Removing_Genres ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_genres);
			}
			private string? NotificationContentText_Removing_Playlists
			{
				get => _NotificationContentText_Removing_Playlists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_playlists);
			}
			private string? NotificationContentText_Removing_Songs
			{
				get => _NotificationContentText_Removing_Songs ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_songs);
			}

			private string? NotificationContentText_Saving_Albums
			{
				get => _NotificationContentText_Saving_Albums ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_albums);
			}
			private string? NotificationContentText_Saving_Artists
			{
				get => _NotificationContentText_Saving_Artists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_artists);
			}
			private string? NotificationContentText_Saving_Genres
			{
				get => _NotificationContentText_Saving_Genres ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_genres);
			}
			private string? NotificationContentText_Saving_Playlists
			{
				get => _NotificationContentText_Saving_Playlists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_playlists);
			}
			private string? NotificationContentText_Saving_Songs
			{
				get => _NotificationContentText_Saving_Songs ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_songs);
			}
			
			private string? NotificationContentText_Scanning_Albums
			{
				get => _NotificationContentText_Scanning_Albums ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_albums);
			}
			private string? NotificationContentText_Scanning_Artists
			{
				get => _NotificationContentText_Scanning_Artists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_artists);
			}
			private string? NotificationContentText_Scanning_Genres
			{
				get => _NotificationContentText_Scanning_Genres ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_genres);
			}
			private string? NotificationContentText_Scanning_Playlists
			{
				get => _NotificationContentText_Scanning_Playlists ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_playlists);
			}
			private string? NotificationContentText_Scanning_Songs
			{
				get => _NotificationContentText_Scanning_Songs ??= Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_songs);
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
			private NotificationCompat.Builder NotificationBuilder
			{
				set => _NotificationBuilder = value;
				get => _NotificationBuilder ??= new NotificationCompat.Builder(this, NotificationChannelId)
					.AddAction(NotificationActionCancel)
					.SetChannelId(NotificationChannel.Id ?? NotificationChannelId)
					.SetContentTitle(NotificationContentText_Preparing)
					.SetPriority(NotificationCompat.PriorityHigh)
					.SetSilent(true)
					.SetSmallIcon(Resource.Drawable.icon_xyzu);
			}
			private NotificationCompat.Action NotificationActionCancel
			{
				set => _NotificationActionCancel = value;
				get => _NotificationActionCancel ??= new NotificationCompat.Action(
					icon: Resource.Drawable.icon_general_x,
					title: Resources?.GetString(Resource.String.cancel) ?? IntentActions.Destroy,
					intent: PendingIntent.GetService(this, 0, new Intent(IntentActions.Destroy), PendingIntentFlags.CancelCurrent));
			}
			private NotificationCompat.BigTextStyle NotificationCompatBigTextStyle
			{
				set => _NotificationCompatBigTextStyle = value;
				get => _NotificationCompatBigTextStyle ??= new NotificationCompat.BigTextStyle();
			}

			public void UpdateNotification(string? contenttext, string? subtext)
			{
				NotificationCompatBigTextStyle = NotificationCompatBigTextStyle
					.SetBigContentTitle(contenttext ?? string.Empty)
					.BigText(subtext ?? string.Empty);

				NotificationBuilder	= NotificationBuilder
					.SetContentTitle(contenttext ?? string.Empty)
					.SetStyle(NotificationCompatBigTextStyle);

				(GetSystemService(NotificationService) as NotificationManager)?		   
					.Notify(NotificationId, NotificationBuilder.Build());
			}
		}
	}
}