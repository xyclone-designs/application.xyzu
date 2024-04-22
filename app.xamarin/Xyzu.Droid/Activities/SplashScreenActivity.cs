#nullable enable

using Android.Content;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

using AndroidX.AppCompat.App;

using Java.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library;
using Xyzu.Library.Models;
using Xyzu.Player;
using Xyzu.Settings.Files;
using Xyzu.Settings.System;
using Xyzu.Settings.UserInterface.Library;

using AndroidOSEnvironment = Android.OS.Environment;
using XyzuResource = Xyzu.Droid.Resource;
using ExoPlayerService = Xyzu.Player.Exoplayer.ExoPlayerService;

namespace Xyzu.Activities
{
    [Activity(
		Exported = true,
		NoHistory = true,
		MainLauncher = true,

		Theme = "@style/SplashScreenTheme",

		LaunchMode = LaunchMode.SingleTop,
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
	public class SplashScreenActivity : AppCompatActivity
	{
		private IFilesSettingsDroid? _FilesSettings;
		private ILibrarySettingsDroid? _LibrarySettings;

		public IFilesSettingsDroid FilesSettings
		{
			get => _FilesSettings ??= XyzuSettings.Instance.GetFilesDroid();
		}
		public ILibrarySettingsDroid LibrarySettings
		{
			get => _LibrarySettings ??= XyzuSettings.Instance.GetUserInterfaceLibraryDroid();
		}

		private void XyzuBroadcastInit(XyzuBroadcast xyzubroadcast)
		{
			xyzubroadcast.ReceiveActions = new Dictionary<string, Action<object, OnReceiveEventArgs>>
			{
				{ "XyzuLibrary.LibraryCache.Action",  (sender, args) =>
				{
					switch (args.Intent?.Action)
					{
						case XyzuBroadcast.Intents.Actions.MediaMounted:
							XyzuLibrary.Instance.ScannerServiceScan(false);
							break;

						case XyzuBroadcast.Intents.Actions.MediaChecking:
						case XyzuBroadcast.Intents.Actions.MediaScannerFinished:
						case XyzuBroadcast.Intents.Actions.MediaScannerStarted:
						default:
							break;
					}
				} },
			};
		}
		private void XyzuImagesInit(XyzuImages xyzuimages)
		{
			xyzuimages.LibraryMisc = XyzuLibrary.Instance.Misc;
		}				   
		private void XyzuLibraryInit(XyzuLibrary xyzulibrary)
		{
			ISong? OnGetSong(string? songid)
			{
				if (songid is null)
					return null;

				return xyzulibrary.Songs.GetSong(
					retriever: null,
					identifiers: new ILibrary.IIdentifiers.Default
					{
						SongIds = Enumerable.Empty<string>()
							.Append(songid)
					});
			}

			xyzulibrary.Settings = new ILibrary.ISettings.Default
			{
				Directories = FilesSettings?.Directories ?? IFilesSettingsDroid.Defaults.Directories,
				Mimetypes = FilesSettings?.Mimetypes ?? IFilesSettingsDroid.Defaults.Mimetypes,
			};
			xyzulibrary.OnCreateAction = new Library.TagLibSharp.TagLibSharpActions.OnCreate
			{
				Paths = AndroidOSEnvironment.StorageDirectory
					.ListFilesSettingsFiles(FilesSettings ?? IFilesSettingsDroid.Defaults.FilesSettingsDroid)
					.ToDictionary(file => file.AbsolutePath, file => file.AbsolutePath),
			};
			xyzulibrary.OnDeleteActions = new List<ILibrary.IOnDeleteActions>
			{
				new Library.TagLibSharp.TagLibSharpActions.OnDelete { },
				new Library.MediaStore.MediaStoreActions.OnDelete { Context = xyzulibrary.Context },
				new Library.MediaMetadata.MediaMetadataActions.OnDelete { },
				new Library.ID3.ID3Actions.OnDelete { },
				new Library.IO.IOActions.OnDelete { },
			};
			xyzulibrary.OnRetrieveActions = new List<ILibrary.IOnRetrieveActions>
			{
				new Library.TagLibSharp.TagLibSharpActions.OnRetrieve { },
				new Library.MediaStore.MediaStoreActions.OnRetrieve { Context = xyzulibrary.Context },
				new Library.MediaMetadata.MediaMetadataActions.OnRetrieve { Context = xyzulibrary.Context },
				new Library.ID3.ID3Actions.OnRetrieve { },
				new Library.IO.IOActions.OnRetrieve { },
			};
			xyzulibrary.OnUpdateActions = new List<ILibrary.IOnUpdateActions>
			{
				new Library.TagLibSharp.TagLibSharpActions.OnUpdate { OnGetSong = OnGetSong },
				new Library.MediaStore.MediaStoreActions.OnUpdate { OnGetSong = OnGetSong, Context = xyzulibrary.Context },
				new Library.ID3.ID3Actions.OnUpdate { OnGetSong = OnGetSong },
				new Library.MediaMetadata.MediaMetadataActions.OnUpdate { OnGetSong = OnGetSong },
				new Library.IO.IOActions.OnUpdate { OnGetSong = OnGetSong },
			};

			xyzulibrary.ScannerServiceScan(false);
		}
		private void XyzuPlayerInit(XyzuPlayer xyzuplayer)
		{
			xyzuplayer.ServiceConnectionChangedAction = args =>
			{
				switch (args.Event)
				{
					case ServiceConnectionChangedEventArgs.Events.Connected when xyzuplayer.ServiceBinder != null:
						xyzuplayer.ServiceBinder.PlayerService.Options ??= new IPlayerServiceOptions.Default();
						xyzuplayer.ServiceBinder.PlayerService.Options.Notification = new IPlayerServiceOptions.INotificationOptions.Default
						{
							Id = XyzuResource.String.playerserviceoptions_notificationoptions_id,
							ChannelName = XyzuResource.String.playerserviceoptions_notificationoptions_channel_name,
							ChannelDescription = XyzuResource.String.playerserviceoptions_notificationoptions_channel_description,
							DefaultId = "",
							DefaultAlbum = Resources?.GetString(XyzuResource.String.playerserviceoptions_notificationoptions_channel_unkownalbum),
							DefaultArtist = Resources?.GetString(XyzuResource.String.playerserviceoptions_notificationoptions_channel_unkownartist),
							DefaultTitle = Resources?.GetString(XyzuResource.String.playerserviceoptions_notificationoptions_channel_unkowntitle),
							OnBitmap = (bitmapfactoryoptions, song) =>
							{
								return XyzuImages.Instance.GetBitmap(
									options: bitmapfactoryoptions,
									operations: IImages.DefaultOperations.Downsample,
									sources: new object?[]
									{
											song,
											XyzuResource.Drawable.icon_xyzu
									});
							},
							ContentIntent = PendingIntent.GetActivity(
								requestCode: 0,
								context: xyzuplayer.Context,
								flags: PendingIntentFlags.OneShot,
								intent: XyzuSettings.Utils.MainActivityIntent(this, null).PutExtra(LibraryActivity.IntentKeys.IsFromNotification, true)),
							Icons = new Dictionary<string, int>
							{
								{ IPlayerServiceOptions.INotificationOptions.IconKeys.SmallIcon, XyzuResource.Drawable.icon_xyzu },
								{ IPlayerServiceOptions.INotificationOptions.IconKeys.Destroy, XyzuResource.Drawable.icon_general_x },
								{ IPlayerServiceOptions.INotificationOptions.IconKeys.Next, XyzuResource.Drawable.icon_player_skip_foward },
								{ IPlayerServiceOptions.INotificationOptions.IconKeys.Pause, XyzuResource.Drawable.icon_player_pause },
								{ IPlayerServiceOptions.INotificationOptions.IconKeys.Play, XyzuResource.Drawable.icon_player_play },
								{ IPlayerServiceOptions.INotificationOptions.IconKeys.Previous, XyzuResource.Drawable.icon_player_skip_backward },
								{ IPlayerServiceOptions.INotificationOptions.IconKeys.Stop, XyzuResource.Drawable.icon_player_stop },
							},
						};

						ExoPlayerService exoplayerservice = (xyzuplayer.ServiceBinder.PlayerService as ExoPlayerService)!;

						if (exoplayerservice.Binder.PreviouslyBound)
							xyzuplayer.Player = exoplayerservice.Player;
						else
						{
							exoplayerservice.PlayerQueue = xyzuplayer.Player.Queue;

							xyzuplayer.Player = exoplayerservice.Player;
							xyzuplayer.Player.Queue.OnRetreiveSong = queuesong =>
							{
								if (queuesong is null)
									return null;

								ISong? song = XyzuLibrary.Instance.Songs.GetSong(
									identifiers: new ILibrary.IIdentifiers.Default
									{
										SongIds = new string[] { queuesong.PrimaryId },
									},
									retriever: new ISong.Default<bool>(false)
									{
										Album = true,
										Artist = true,
										Filepath = true,
										Title = true,
										Uri = true,
									});

								return song;
							};
						}

						exoplayerservice.OnStartCommand(xyzuplayer.Intent, StartCommandFlags.Redelivery, ExoPlayerService.StartIds.PostBind);
						break;

					default: break;
				}
			};
		}

		private async void Start(bool permissionchecked)
		{
			if (permissionchecked is false)
				permissionchecked = 
					CheckCallingPermission(Manifest.Permission.ReadExternalStorage) is Permission.Granted &&
					CheckCallingPermission(Manifest.Permission.ManageExternalStorage) is Permission.Granted;

			if (permissionchecked is false)
			{
				RequestPermissions(requestCode: 0, permissions: new string[]
				{
					Manifest.Permission.ReadExternalStorage,
					Manifest.Permission.ManageExternalStorage,
				});

				return;
			}

			if (CacheDir != null)
				await CacheDir.ClearDirectoryAsync();

			XyzuBroadcast.Init(Application.Context, XyzuBroadcastInit);
			XyzuSettings.Init(Application.Context);
			XyzuLibrary.Init(Application.Context, XyzuLibraryInit);
			XyzuImages.Init(Application.Context, XyzuImagesInit);
			XyzuPlayer.Init(Application.Context, typeof(ExoPlayerService), XyzuPlayerInit);

			StartActivity(XyzuSettings.Utils.MainActivityIntent(this, null));
		}

		protected override void OnCreate(Bundle? savedInstaneState)
		{
			base.OnCreate(savedInstaneState);

			AppDomain.CurrentDomain.UnhandledException += ISystemSettingsDroid.OnUnhandledException;
			TaskScheduler.UnobservedTaskException += ISystemSettingsDroid.OnUnobservedTaskException;
		}
		protected override void OnStart()
		{
			base.OnStart();

			Start(false);
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			AppDomain.CurrentDomain.UnhandledException -= ISystemSettingsDroid.OnUnhandledException;
			TaskScheduler.UnobservedTaskException -= ISystemSettingsDroid.OnUnobservedTaskException;
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			if (permissions.Contains(Manifest.Permission.ReadExternalStorage))
				Start(true);
		}
	}
}