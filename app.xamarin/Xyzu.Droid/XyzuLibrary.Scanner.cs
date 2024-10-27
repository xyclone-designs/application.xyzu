using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

using System;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library;

namespace Xyzu
{
	public sealed partial class XyzuLibrary : Java.Lang.Object, IServiceConnection
	{
		private ServiceConnectionChangedEventArgs.Events? _ServiceConnectionState;
		public ServiceConnectionChangedEventArgs.Events ScannerServiceConnectionState
		{
			private set => _ServiceConnectionState = value;
			get => _ServiceConnectionState ??= ServiceConnectionChangedEventArgs.Events.Disconnected;
		}
		public Action<ServiceConnectionChangedEventArgs>? ServiceConnectionChangedAction { get; set; }
	
		public event EventHandler<ServiceConnectionChangedEventArgs>? OnServiceConnectionChanged;

		public IScanner.ServiceBinder? ScanServiceBinder { get; set; }

		public void OnServiceConnected(ComponentName? name, IBinder? binder)
		{
			if (binder is null)
				return;

			if (binder is IScanner.ServiceBinder scannerbinder)
			{
				ScannerServiceConnectionState = ServiceConnectionChangedEventArgs.Events.Connected;

				ScanServiceBinder = scannerbinder;
				ScanServiceBinder.Filepaths = XyzuSettings.Instance.GetFilesDroid().Files().Select(file => file.AbsolutePath);
				ScanServiceBinder.Library = Library;
				ScanServiceBinder.ServiceConnection = this;
				ScanServiceBinder.Service.Componentname = name;
				ScanServiceBinder.Service.Notification ??= new IScanner.ServiceNotification(
					IScanner.Notifications.Id,
					IScanner.Notifications.ChannelId,
					IScanner.Notifications.ChannelName);
				ScanServiceBinder.Service.Notification.ContentTextsPreparing = new IScanner.IContentTexts.Default
				{
					Title = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing),
					TextAlbums = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_albums),
					TextArtists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_artists),
					TextGenres = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_genres),
					TextPlaylists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_playlists),
					TextSongs = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_preparing_songs),
				};
				ScanServiceBinder.Service.Notification.ContentTextsReading = new IScanner.IContentTexts.Default
				{
					TextAlbums = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_albums),
					TextArtists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_artists),
					TextGenres = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_genres),
					TextPlaylists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_playlists),
					TextSongs = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_reading_songs),
				};
				ScanServiceBinder.Service.Notification.ContentTextsRemoving = new IScanner.IContentTexts.Default
				{
					TextAlbums = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_albums),
					TextArtists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_artists),
					TextGenres = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_genres),
					TextPlaylists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_playlists),
					TextSongs = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_removing_songs),
				};
				ScanServiceBinder.Service.Notification.ContentTextsSaving = new IScanner.IContentTexts.Default
				{
					TextAlbums = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_albums),
					TextArtists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_artists),
					TextGenres = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_genres),
					TextPlaylists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_playlists),
					TextSongs = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_saving_songs),
				};
				ScanServiceBinder.Service.Notification.ContentTextsScanning = new IScanner.IContentTexts.Default
				{
					TextAlbums = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_albums),
					TextArtists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_artists),
					TextGenres = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_genres),
					TextPlaylists = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_playlists),
					TextSongs = Application.Context.Resources?.GetString(Resource.String.scannerservice_notification_contenttext_scanning_songs),
				};

				ScanServiceBinder.Service.Notification.CompatBuilder = ScanServiceBinder.Service.Notification.CompatBuilder
					.SetContentTitle(ScanServiceBinder.Service.Notification.ContentTextsPreparing.Title)
					.SetSmallIcon(Resource.Drawable.icon_xyzu);

				ScanServiceBinder.Service.Notification.CompatActionCancel = new NotificationCompat.Action(
					icon: Resource.Drawable.icon_general_x,
					title: Application.Context.Resources?.GetString(Resource.String.cancel),
					intent: PendingIntent.GetService(Application.Context, 0, new Intent(IScanner.IntentActions.Destroy), PendingIntentFlags.CancelCurrent));

				(ScanServiceBinder.Service as Service)?.OnStartCommand(
					flags: StartCommandFlags.Redelivery,
					intent: ScanServiceBinder.Service.LatestIntent,
					startId: ScanServiceBinder.Service.LatestStartId);

			}

			ServiceConnectionChangedEventArgs args = new (ScannerServiceConnectionState)
			{
				Binder = binder,
				Name = name,
			};

			OnServiceConnectionChanged?.Invoke(this, args);
			ServiceConnectionChangedAction?.Invoke(args);
		}
		public void OnServiceDisconnected(ComponentName? name)
		{
			ScannerServiceConnectionState = ServiceConnectionChangedEventArgs.Events.Disconnected;

			ServiceConnectionChangedEventArgs args = new (ScannerServiceConnectionState)
			{
				Name = name,
			};

			OnServiceConnectionChanged?.Invoke(this, args);
			ServiceConnectionChangedAction?.Invoke(args);

			ScanServiceBinder = null;
		}

		public void ScannerServiceScan(bool hard)
		{
			Intent service = new Intent(Context, typeof(IScanner.ScannerService))
				.SetAction(
					action: hard
						? IScanner.IntentActions.ScanHard
						: IScanner.IntentActions.ScanSoft);

			Context.StartService(service);
			Context.BindService(service, this, Bind.AdjustWithActivity | Bind.AutoCreate | Bind.Important);
		}
	}
}