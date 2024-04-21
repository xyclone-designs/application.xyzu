using System;
using System.ComponentModel;

using Xyzu.Player.Enums;

namespace Xyzu.Player
{
	public partial interface IPlayer 
	{
		public static class Defaults
		{
			public const long Position = 0;

			public const PlayerStates State = PlayerStates.Uninitialised;
			public const RepeatModes Repeat = RepeatModes.NoRepeat;
			public const ShuffleModes Shuffle = ShuffleModes.NoShuffle;
		}

		IQueue Queue { get; set; }
		long? Position { get; set; }
		RepeatModes Repeat { get; set; }
		PlayerStates State { get; set; }
		ShuffleModes Shuffle { get; set; }
		PlayerStates? PreviousState { get; }

		event EventHandler<PropertyChangedEventArgs> OnPropertyChanged;
		event EventHandler<PlayerOperationsEventArgs> OnPlayerOperation;

		void Init();
		void Play();
		void Pause();
		void PlayPause();
		void Stop();
		void Previous();
		void Next();
		void Seek(long position);  
		void Skip(int queueindex);

		public class PlayerOperationsEventArgs : EventArgs
		{
			public PlayerOperationsEventArgs(PlayerOperations playeroperation)
			{
				PlayerOperation = playeroperation;
			}

			public PlayerOperations PlayerOperation { get; }
			
			public long? Position { get; set; }
			public int? QueueIndex { get; set; }
		}
	}
}
