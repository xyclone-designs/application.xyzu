#nullable enable

using Android.Content;
using Android.OS;

using System;
using System.ComponentModel;

using Xyzu.Player;

namespace Xyzu
{
	public sealed partial class XyzuPlayer : Java.Lang.Object, IServiceConnection
	{
		private XyzuPlayer(Context context) 
		{
			Context = context;
			Intent = new Intent();
		}	

		private static XyzuPlayer? _Instance;
		public static XyzuPlayer Instance
		{
			get => _Instance ?? throw new Exception("Instance is null. Init AppPlayer before use");
		}

		public static void Init(Context context, Type servietype, Action<XyzuPlayer>? oninit)
		{
			_Instance = new XyzuPlayer(context)
			{
				Intent = new Intent(context, servietype)
			};

			oninit?.Invoke(_Instance);
		}

		private IPlayer? _Player;

		private ServiceConnectionChangedEventArgs.Events? _ServiceConnectionState;
	
		public Context Context { get; set; }
		public Intent Intent { get; set; }

		public IPlayer Player
		{
			set => _Player = value;
			get => _Player ??= new DefaultPlayer
			{
				Intent = Intent,
				OnInit = OnInit,
			};
		}
		public IPlayerServiceBinder? ServiceBinder { get; private set; }
		public ServiceConnectionChangedEventArgs.Events ServiceConnectionState
		{
			private set => _ServiceConnectionState = value;
			get => _ServiceConnectionState ??= ServiceConnectionChangedEventArgs.Events.Disconnected;
		}
		public Action<ServiceConnectionChangedEventArgs>? ServiceConnectionChangedAction { get; set; }
	
		public event EventHandler<ServiceConnectionChangedEventArgs>? OnServiceConnectionChanged;

		private void OnInit()
		{
			string? previousaction = Intent.Action;
			
			Intent.SetAction(Intents.Actions.Initialise);

			Context.StartService(Intent);
			Context.BindService(Intent, this, Bind.AutoCreate);

			Intent.SetAction(previousaction);
		}
		private void OnDestroy()
		{
			_Player = null;

			UnbindPlayer();
			Context.StopService(Intent);
		}

		public void BindPlayer()
		{
			if (ServiceBinder != null)
				return;

			Context.BindService(Intent.SetAction(Intents.Actions.Bind), this, Bind.None);
		}
		public void UnbindPlayer()
		{
			if (ServiceBinder is null)
				return;

			Context.UnbindService(this);
		}
		public void OnServiceConnected(ComponentName? name, IBinder? binder)
		{
			if (binder is null)
				return;

			ServiceBinder = binder as IPlayerServiceBinder ?? throw new ArgumentException(string.Format("'{0}' is not of type '{1}'", binder?.GetType(), typeof(IPlayerServiceBinder)));
			ServiceBinder.ServiceConnection = this;
			ServiceBinder.PlayerService.Componentname = name;
			ServiceBinder.PlayerService.OnIntent += OnServiceIntent;

			SettingsBassBoostPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			SettingsEnvironmentalReverbPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			SettingsEqualiserPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			SettingsLoudnessEnhancerPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			SettingsNotificationPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));

			ServiceConnectionChangedEventArgs serviceconnectionchangedeventargs =
				new ServiceConnectionChangedEventArgs(ServiceConnectionState = ServiceConnectionChangedEventArgs.Events.Connected)
				{
					Binder = binder,
					Name = name,
				};

			ServiceConnectionChangedAction?.Invoke(serviceconnectionchangedeventargs);
			OnServiceConnectionChanged?.Invoke(this, serviceconnectionchangedeventargs);
		}
		public void OnServiceIntent(object sender, Intent intent)
		{
			switch (intent.Action)
			{
				case Intents.Actions.Destroy:
					OnDestroy();
					break;

				default: break;
			}
		}
		public void OnServiceDisconnected(ComponentName? name) 
		{
			if (ServiceBinder != null)
			{
				ServiceBinder.PlayerService.OnIntent += OnServiceIntent;
			}
			
			ServiceBinder = null;

			ServiceConnectionChangedEventArgs serviceconnectionchangedeventargs =
				new ServiceConnectionChangedEventArgs(ServiceConnectionState = ServiceConnectionChangedEventArgs.Events.Disconnected)
				{
					Name = name,
				};

			ServiceConnectionChangedAction?.Invoke(serviceconnectionchangedeventargs);
			OnServiceConnectionChanged?.Invoke(this, serviceconnectionchangedeventargs);
		}

		private class DefaultPlayer : IPlayer.Default, IPlayer
		{
			public Intent? Intent { get; set; }
			public Action? OnInit { get; set; }

			public override void Init()
			{
				base.Init();

				OnInit?.Invoke();
			}
			public override void Next()
			{
				Intent?.SetAction(Intents.Actions.Next);

				OnInit?.Invoke();
			}
			public override void Pause()
			{
				Intent?.SetAction(Intents.Actions.Pause);

				OnInit?.Invoke();
			}
			public override void Play()
			{
				Intent?.SetAction(Intents.Actions.Play);

				OnInit?.Invoke();
			}
			public override void PlayPause()
			{
				Intent?.SetAction(Intents.Actions.PlayPause);

				OnInit?.Invoke();
			}
			public override void Previous()
			{
				Intent?.SetAction(Intents.Actions.Previous);

				OnInit?.Invoke();
			}
			public override void Seek(long position)
			{
				Intent?
					.SetAction(Intents.Actions.Seek)
					.PutExtra(Intents.Extras.Seek.Millisecond_Long, position);

				OnInit?.Invoke();
			}
			public override void Skip(int queueindex)
			{
				Intent?
					.SetAction(Intents.Actions.Skip)
					.PutExtra(Intents.Extras.Skip.QueueIndex_Int, queueindex);

				OnInit?.Invoke();
			}
			public override void Stop()
			{
				Intent?.SetAction(Intents.Actions.Stop);

				OnInit?.Invoke();
			}
		}
	}
}