#nullable enable

using AndroidX.ConstraintLayout.Motion.Widget;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Models;
using Xyzu.Player;
using Xyzu.Player.Enums;

namespace Xyzu.Views.NowPlaying
{
	public partial class NowPlayingView 
	{
		public enum ViewOperations
		{
			LongPressPlayPause,
			PressEffects,
			PressOptions,
			PressQueue,
			PressPlayerSettings,
			PressPlayPause,
			PressView,
			PressNext,
			PressPrevious,
			Seek,
			SwipeNext,
			SwipePrevious,
		}

		public event EventHandler<ViewOperationEventArgs>? OnViewOperation;
		public Action<ViewOperationEventArgs>? OnViewOperationAction { get; set; }

		protected void InvokeOnViewOperation(object sender, ViewOperationEventArgs viewoperationeventargs)
		{
			switch (viewoperationeventargs.ViewOperation)
			{
				case ViewOperations.PressEffects:
					OnClick_Buttons_Menu_AudioEffects();
					break;

				case ViewOperations.PressOptions:
					OnClick_Buttons_Menu_Options();
					break;

				case ViewOperations.PressQueue:
					OnClick_Buttons_Menu_Queue();
					break;

				case ViewOperations.PressPlayerSettings:
					OnClick_Buttons_Menu_PlayerSettings();
					break;

				case ViewOperations.LongPressPlayPause:
					Player?.Stop();
					break;

				case ViewOperations.PressPlayPause when
				Player != null:
					switch (Player.State)
					{
						case PlayerStates.NoSong:
						case PlayerStates.Uninitialised:
							if (Library is null)
								break;

							IEnumerable<IQueueItem> songs = Library.Songs
								.GetSongs(null, new ISong.Default<bool>(false))
								.Select(song => IQueueItem.FromSong(song, null));

							Player.Queue.Clear();
							Player.Queue.AddRange(songs);
							Player.Queue.CurrentIndex = 1;

							Player.PlayPause();
							break;

						default:
							Player.PlayPause();
							break;
					}
					break;

				case ViewOperations.Seek:
					if (viewoperationeventargs.SeekValue.HasValue)
						Player?.Seek(viewoperationeventargs.SeekValue.Value);
					break;

				case ViewOperations.PressNext:
					Player?.Next();
					break;

				case ViewOperations.PressPrevious:
					Player?.Previous();
					break;

				case ViewOperations.SwipeNext when
				TransitionCurrent?.Id == Ids.MotionScene.Transitions.Collapse:
					this.SetAndTransitionToStart(Ids.MotionScene.Transitions.Collapsed_OutRight);
					break;

				case ViewOperations.SwipePrevious when
				TransitionCurrent?.Id == Ids.MotionScene.Transitions.Collapse:
					this.SetAndTransitionToStart(Ids.MotionScene.Transitions.Collapsed_OutLeft);
					break;

				default: break;
			}

			OnViewOperationAction?.Invoke(viewoperationeventargs);
			OnViewOperation?.Invoke(sender, viewoperationeventargs);
		}

		public class ViewOperationEventArgs : EventArgs
		{
			public ViewOperationEventArgs(ViewOperations viewoperation)
			{
				ViewOperation = viewoperation;
			}

			public ViewOperations ViewOperation { get; }
			public long? SeekValue { get; set; }
		}
	}
}