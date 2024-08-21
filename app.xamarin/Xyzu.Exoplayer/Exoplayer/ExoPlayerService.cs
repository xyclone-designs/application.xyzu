#nullable enable

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Media.Session;

using Com.Google.Android.Exoplayer2;

using Xyzu.Player.Enums;

using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Xyzu.Player.Exoplayer
{
	[Service(
		Enabled = true,
		Exported = true,
		Label = "Xyzu Music Playback Service",
		Name = "co.za.xyclonedesigns.xyzu.exoplayerservice",
		ForegroundServiceType = ForegroundService.TypeMediaPlayback)]
	[IntentFilter(new[]
	{
		Intents.Actions.Initialise,
		Intents.Actions.Bind,
		Intents.Actions.Destroy,
		Intents.Actions.Next,
		Intents.Actions.Pause,
		Intents.Actions.Play,
		Intents.Actions.PlayPause,
		Intents.Actions.Previous,
		Intents.Actions.Repeat,
		Intents.Actions.Seek,
		Intents.Actions.Shuffle,
		Intents.Actions.Skip,
		Intents.Actions.Stop,
	})]
	public partial class ExoPlayerService : Service, IPlayerService
	{
		public static class StartIds
		{
			public const int RefreshAll = -1; 
			public const int RefreshPosition = -2; 
			public const int RefreshNotification = -3; 
			public const int RefreshQueue = -4; 
			public const int PostBind = -4; 
		}

		public ExoPlayerService() { }

		private int _LatestStartId = 0;

		private IPlayer? _Player;
		private IPlayerServiceBinder? _Binder;
		private IPlayerServiceOptions? _Options;

		public IPlayer Player
		{
			get
			{
				if (_Player is null)
				{
					_Player = new ServicePlayer(this)
					{
						Service = this,
					};

					_Player.Queue.ListChanged += PlayerQueueListChanged;
					_Player.Queue.PropertyChanged += PlayerQueuePropertyChanged;
				}

				return _Player;
			}
			set
			{
				if (_Player != null)
				{
					_Player.Queue.ListChanged -= PlayerQueueListChanged;
					_Player.Queue.PropertyChanged -= PlayerQueuePropertyChanged;
				}

				_Player = value;

				_Player.Queue.ListChanged += PlayerQueueListChanged;
				_Player.Queue.PropertyChanged += PlayerQueuePropertyChanged;
			}
		}
		public IPlayerServiceBinder Binder
		{
			set => _Binder = value;
			get => _Binder ??= new ServiceBinder(this);
		}
		public IPlayerServiceOptions Options
		{
			set => _Options = value;
			get => _Options ??= new IPlayerServiceOptions.Default();
		}

		public IQueue PlayerQueue
		{
			set
			{
				_Player ??= new ServicePlayer(this)
				{
					Service = this,
				};

				_Player.Queue.ListChanged -= PlayerQueueListChanged;
				_Player.Queue.PropertyChanged -= PlayerQueuePropertyChanged;

				_Player.Queue = value;

				_Player.Queue.ListChanged += PlayerQueueListChanged;
				_Player.Queue.PropertyChanged += PlayerQueuePropertyChanged;
			}
		}

		public ComponentName? Componentname { get; set; }

		void RefreshPosition()
		{
			if (Player.Queue.CurrentIndex.HasValue && Player.Queue.CurrentIndex.Value != Exoplayer.CurrentWindowIndex)
				Exoplayer.SeekTo(Player.Queue.CurrentIndex.Value, 0);
		}
		void RefreshNotification()
		{
			NotificationInited = false;

			_NotificationActionPrevious = null;
			_NotificationActionPause = null;
			_NotificationActionPlay = null;
			_NotificationActionNext = null;
			_NotificationActionDestroy = null;

			NotificationManager?.OnUpdate?.Invoke(
				arg1: Exoplayer,
				arg2: NotificationManager.NotificationBuilder,
				arg3: NotificationManager.NotificationOnGoing ?? true,
				arg4: NotificationManager.NotificationLargeIcon);
		}
		void RefreshQueue()
		{
			PlayerQueueListChanged(this, NotifyListChangedEventArgs.FromRefresh());
		}

		public void ProcessIntent(Intent? intent)
		{
			MediaButtonReceiver.HandleIntent(MediaSession, intent);

			PlayerCurrentIndexRefresh();

			switch (intent?.Action)
			{
				case Intents.Actions.Initialise:
					Player.State = PlayerStates.Loading;
					if (Player.Queue.Any() is false)
						Player.State = PlayerStates.NoSong;
					else Player.State = PlayerStates.Stopped;
					break;
				case Intents.Actions.Destroy:
					Player.Queue.Id = null;
					Player.Queue.CurrentIndex = null;
					Player.State = PlayerStates.Uninitialised;
					Exoplayer.Stop(true);
					break;
				case Intents.Actions.Play:
					Exoplayer.PlayWhenReady = true;
					Player.State = PlayerStates.Playing;
					break;
				case Intents.Actions.PlayPause when Exoplayer.IsPlaying is false:
					Exoplayer.PlayWhenReady = true;
					Player.State = PlayerStates.Playing;
					break;
				case Intents.Actions.Pause:
				case Intents.Actions.PlayPause when Exoplayer.IsPlaying is true:
					Exoplayer.PlayWhenReady = false;
					Player.State = PlayerStates.Paused;
					break;
				case Intents.Actions.Next:
					Exoplayer.Next();
					break;
				case Intents.Actions.Previous:
					Exoplayer.Previous();
					break;
				case Intents.Actions.Repeat:
					Exoplayer.RepeatMode = Player.Repeat switch
					{
						RepeatModes.NoRepeat => IExoPlayer.RepeatModeOff,
						RepeatModes.RepeatSong => IExoPlayer.RepeatModeOne,
						RepeatModes.RepeatSubQueue => IExoPlayer.RepeatModeAll,
						RepeatModes.RepeatEntireQueue => IExoPlayer.RepeatModeAll,

						_ => IExoPlayer.RepeatModeOff
					};
					break;
				case Intents.Actions.Seek when
				intent.GetLongExtra(Intents.Extras.Seek.Millisecond_Long, 0) is long position:
					PlayerStates seekpreviplayerstate = Player.State;
					Player.State = PlayerStates.Seeking;
					Exoplayer.SeekTo(Exoplayer.CurrentWindowIndex, position);
					Player.State = seekpreviplayerstate;
					break;
				case Intents.Actions.Shuffle:
					Exoplayer.ShuffleModeEnabled = Player.Shuffle != ShuffleModes.NoShuffle;
					break;
				case Intents.Actions.Skip when
				intent.GetIntExtra(Intents.Extras.Skip.QueueIndex_Int, 0) is int index:
					Exoplayer.SeekTo(index, 0);
					break;
				case Intents.Actions.Stop:
					Player.Queue.Id = null;
					Player.Queue.CurrentIndex = null;
					Exoplayer.Stop(true);
					Player.State = PlayerStates.Stopped;
					break;

				default: break;
			}

			if (intent != null)
				OnIntent?.Invoke(this, intent);
		}

		public event System.EventHandler<Intent>? OnIntent;

		public override void OnCreate()
		{
			base.OnCreate();

			NotificationManager.SetPlayer(Exoplayer);
		}			 
		public override void OnDestroy()
		{
			Binder?.ServiceConnection?.OnServiceDisconnected(Componentname);
			StopForeground(true);
			StopSelf(_LatestStartId);
			NotificationManager.SetPlayer(null!);

			Player.Queue.Clear();

			base.OnDestroy();
		}
		public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			switch (startId)
			{
				case StartIds.PostBind when Binder.PreviouslyBound:
					return StartCommandResult.Sticky;
				case StartIds.PostBind when Binder.PreviouslyBound is false:
				case StartIds.RefreshAll:
					RefreshQueue();
					RefreshPosition();
					RefreshNotification();
					break;
				case StartIds.RefreshPosition:
					RefreshPosition();
					break;
				case StartIds.RefreshNotification:
					RefreshNotification();
					break;								
				case StartIds.RefreshQueue:
					RefreshQueue();
					break;

				default:
					_LatestStartId = startId;
					break;
			}

			base.OnStartCommand(intent, flags, _LatestStartId);

			ProcessIntent(intent);

			return StartCommandResult.Sticky;
		}
		public override IBinder? OnBind(Intent? intent)
		{
			return Binder as Binder;
		}
		public override void OnRebind(Intent? intent)
		{
			base.OnRebind(intent);
		}
		public override bool OnUnbind(Intent? intent)
		{
			base.OnUnbind(intent);

			Binder.PreviouslyBound = true;

			return false;
		}
		
		protected override void Dispose(bool disposing)
		{
			_Binder?.Dispose();
			_ConcatenatingMediaSource?.Dispose();
			_DataSourceFactory?.Dispose();
			_EffectsBassBoost?.Dispose();
			_EffectsEnvironmentalReverb?.Dispose();
			_EffectsEqualizer?.Dispose();
			_EffectsLoudnessEnhancer?.Dispose();
			_Exoplayer?.Dispose();
			_ExtractorsFactory?.Dispose();
			_MediaSession?.Dispose();
			_MediaSessionConnector?.Dispose();
			_NotificationActionDestroy?.Dispose();
			_NotificationActionNext?.Dispose();
			_NotificationActionPause?.Dispose();
			_NotificationActionPlay?.Dispose();
			_NotificationActionPrevious?.Dispose();
			_NotificationChannel?.Dispose();
			_NotificationManager?.Dispose();
			_ProgressiveMediaSourceFactory?.Dispose();

			_Binder = null;
			_ConcatenatingMediaSource = null;
			_DataSourceFactory = null;
			_EffectsBassBoost = null;
			_EffectsEnvironmentalReverb = null;
			_EffectsEqualizer = null;
			_EffectsLoudnessEnhancer = null;
			_Exoplayer = null;
			_ExtractorsFactory = null;
			_MediaSession = null;
			_MediaSessionConnector = null;
			_NotificationActionDestroy = null;
			_NotificationActionNext = null;
			_NotificationActionPause = null;
			_NotificationActionPlay = null;
			_NotificationActionPrevious = null;
			_NotificationChannel = null;
			_NotificationManager = null;
			_Options = null;
			_Player = null;
			_ProgressiveMediaSourceFactory = null;
			
			base.Dispose(disposing);
		}
	}
}