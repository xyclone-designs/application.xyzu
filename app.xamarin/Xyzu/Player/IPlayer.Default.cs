using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xyzu.Player.Enums;

namespace Xyzu.Player
{
	public partial interface IPlayer 
	{
		public abstract class Default : IPlayer
		{
			protected bool _IsDisposed;
			protected long? _Position;
			protected IQueue? _Queue;

			protected RepeatModes _Repeat = RepeatModes.NoRepeat;
			protected ShuffleModes _Shuffle = ShuffleModes.NoShuffle;
			protected PlayerStates _State = PlayerStates.Uninitialised;
			protected PlayerStates? _PreviousState = null;

			public virtual long? Position
			{
				get => _Position;
				set
				{
					_Position = value;

					PropertyChanged();
				}
			}
			public IQueue Queue
			{
				get	=> _Queue ??= new IQueue.Default();
				set
				{
					_Queue = value;

					PropertyChanged();
				}
			}
			public RepeatModes Repeat
			{
				get => _Repeat;
				set
				{
					_Repeat = value;

					PropertyChanged();
				}
			}
			public PlayerStates State
			{
				get => _State;
				set
				{
					_PreviousState = _State;
					_State = value;

					PropertyChanged();
				}
			}
			public ShuffleModes Shuffle
			{
				get => _Shuffle;
				set
				{
					_Shuffle = value;

					PropertyChanged();
				}
			}
			public PlayerStates? PreviousState
			{
				get => _PreviousState;
			}

			public event EventHandler<PropertyChangedEventArgs>? OnPropertyChanged;
			public event EventHandler<PlayerOperationsEventArgs>? OnPlayerOperation;

			public virtual void Init() { }
			public abstract void Next();
			public abstract void Pause();
			public abstract void Play();
			public abstract void PlayPause();
			public abstract void Previous();
			public abstract void Seek(long position);
			public abstract void Skip(int queueindex);
			public abstract void Stop();

			public void Dispose()
			{
				if (_IsDisposed is false)
				{
					_IsDisposed = true;
				}

				GC.SuppressFinalize(this);
			}

			protected virtual void RaisePlayerOperation(PlayerOperationsEventArgs args)
			{
				OnPlayerOperation?.Invoke(this, args);
			}
			protected virtual void PropertyChanged([CallerMemberName] string? propertyname = null)
			{
				OnPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname ?? string.Empty));
			}
		}
	}
}
