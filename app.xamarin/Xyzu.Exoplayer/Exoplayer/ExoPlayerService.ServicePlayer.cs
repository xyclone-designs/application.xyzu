#nullable enable

using Android.Content;

using Com.Google.Android.Exoplayer2.Source;

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using Xyzu.Player.Enums;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService
	{
		public class ServicePlayer : IPlayer.Default
		{
			public ServicePlayer(Context context)
			{
				Context = context;
				_IsBound = false;
			}

			private bool _IsBound;
			private Intent? _Intent;

			public Context Context { get; }
			public Intent Intent
			{
				set => _Intent = value;
				get => _Intent ??= new Intent(Context, typeof(ExoPlayerService));
			}
			public ExoPlayerService? Service { get; set; }
			public IServiceConnection? ServiceConnection { get; set; }

			public override long? Position
			{
				set => base.Position = value;
				get => Service?.Exoplayer.CurrentPosition ?? base.Position;
			}

			public void ProcessIntent(Intent? intent)
			{
				switch (true)
				{
					case true when
					Service != null:
						Service.ProcessIntent(intent);
						break;

					default:
						Init();
						break;
				}
			}

			public override void Init()
			{
				base.Init();

				Intent.SetAction(Intents.Actions.Initialise);

				if (_IsBound is false && ServiceConnection != null)
				{
					Context.StartService(Intent);
					_IsBound = Context.BindService(Intent, ServiceConnection, Bind.AutoCreate);
				}
			}
			public override void Next()
			{
				ProcessIntent(Intent.SetAction(Intents.Actions.Next));

				RaisePlayerOperation(new IPlayer.PlayerOperationsEventArgs(PlayerOperations.Next));
			}
			public override void Pause()
			{
				ProcessIntent(Intent.SetAction(Intents.Actions.Pause));

				RaisePlayerOperation(new IPlayer.PlayerOperationsEventArgs(PlayerOperations.Pause));
			}
			public override void Play()
			{
				ProcessIntent(Intent.SetAction(Intents.Actions.Play));

				RaisePlayerOperation(new IPlayer.PlayerOperationsEventArgs(PlayerOperations.Play));
			}
			public override void PlayPause()
			{
				ProcessIntent(Intent.SetAction(Intents.Actions.PlayPause));

				RaisePlayerOperation(new IPlayer.PlayerOperationsEventArgs(PlayerOperations.PlayPause));
			}
			public override void Previous()
			{
				ProcessIntent(Intent.SetAction(Intents.Actions.Previous));

				RaisePlayerOperation(new IPlayer.PlayerOperationsEventArgs(PlayerOperations.Previous));
			}
			public override void Seek(long position)
			{
				ProcessIntent(Intent
					.SetAction(Intents.Actions.Seek)
					.PutExtra(Intents.Extras.Seek.Millisecond_Long, position));

				RaisePlayerOperation(new IPlayer.PlayerOperationsEventArgs(PlayerOperations.Seek) { Position = position });
			}
			public override void Skip(int queueindex)
			{
				ProcessIntent(Intent
					.SetAction(Intents.Actions.Skip)
					.PutExtra(Intents.Extras.Skip.QueueIndex_Int, queueindex));

				RaisePlayerOperation(new IPlayer.PlayerOperationsEventArgs(PlayerOperations.Skip) { QueueIndex = queueindex });
			}
			public override void Stop()
			{
				ProcessIntent(Intent.SetAction(Intents.Actions.Stop));

				RaisePlayerOperation(new IPlayer.PlayerOperationsEventArgs(PlayerOperations.Stop));
			}
		}

		public IEnumerable<IMediaSource>? AsMediaSources(IList? queueItems)
		{
			return AsMediaSources(queueItems?.Cast<IQueueItem>());
		}
		public IEnumerable<IMediaSource>? AsMediaSources(IEnumerable<IQueueItem>? queueItems)
		{
			return queueItems?.Select(queueitem =>
			{
				return
					ProgressiveMediaSourceFactory.CreateMediaSource(queueitem) ??
					SilenceMediaSource;
			});
		}

		public void PlayerCurrentIndexRefresh()
		{
			if
			(
				Player.Queue.CurrentIndex is null ||
				Player.Queue.CurrentIndex == Exoplayer.CurrentWindowIndex

			) return;

			Exoplayer.SeekTo(Player.Queue.CurrentIndex.Value, 0);
		}
		public void PlayerQueueListChanged(object sender, NotifyListChangedEventArgs args)
		{
			switch (args.Action)
			{
				case NotifyListChangedAction.Add:
				case NotifyListChangedAction.AddRange:

					if (AsMediaSources(args.NewItems) is IEnumerable<IMediaSource> addmediasources)
					{
						if (args.NewStartingIndex == -1)
							ConcatenatingMediaSource.AddMediaSources(addmediasources.ToList());
						else ConcatenatingMediaSource.AddMediaSources(args.NewStartingIndex, addmediasources.ToList());
					}

					break;

				case NotifyListChangedAction.Move:
				case NotifyListChangedAction.MoveRange:
					ConcatenatingMediaSource.MoveMediaSource(
						newIndex: args.NewStartingIndex,
						currentIndex: args.OldStartingIndex);
					break;

				case NotifyListChangedAction.Refresh:
					if (AsMediaSources(Player.Queue) is IEnumerable<IMediaSource> refreshmediasources)
					{
						ConcatenatingMediaSource.Clear();
						ConcatenatingMediaSource.AddMediaSources(refreshmediasources.ToList());
						
						Exoplayer.Prepare(ConcatenatingMediaSource, true, true);
					}
					break;

				case NotifyListChangedAction.Remove:
				case NotifyListChangedAction.RemoveRange:
					ConcatenatingMediaSource.RemoveMediaSourceRange(
						fromIndex: args.OldStartingIndex,
						toIndex: args.OldStartingIndex + args.OldItems?.Count ?? 0);
					break;

				case NotifyListChangedAction.Reset:
					ConcatenatingMediaSource.Clear();
					break;

				case NotifyListChangedAction.Replace:
				case NotifyListChangedAction.ReplaceRange:

					if (AsMediaSources(args.NewItems) is IEnumerable<IMediaSource> replacemediasources) 
					{
						ConcatenatingMediaSource.RemoveMediaSource(args.OldStartingIndex);

						if (args.NewStartingIndex == -1)
							ConcatenatingMediaSource.AddMediaSources(replacemediasources.ToList());
						else ConcatenatingMediaSource.AddMediaSources(args.NewStartingIndex, replacemediasources.ToList());
					}

					break;

				default: break;
			}

			PlayerCurrentIndexRefresh();
		}
		public void PlayerQueuePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(IQueue.CurrentIndex):
					PlayerCurrentIndexRefresh();
					break;

				default: break;
			}
		}
	}
}