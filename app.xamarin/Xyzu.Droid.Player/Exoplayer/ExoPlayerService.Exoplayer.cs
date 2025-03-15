using AndroidX.Media3.Common;
using AndroidX.Media3.ExoPlayer;
using AndroidX.Media3.ExoPlayer.Analytics;
using AndroidX.Media3.ExoPlayer.Source;
using AndroidX.Media3.ExoPlayer.TrackSelection;

using Java.IO;

using System;

using Xyzu.Player.Enums;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService : IPlayerListener, IAnalyticsListener
	{
		private IExoPlayer? _Exoplayer;

		public IExoPlayer Exoplayer
		{
			get
			{
				if (_Exoplayer is null)
				{
					using ExoPlayerBuilder builder = new (Application?.ApplicationContext ?? this);

					_Exoplayer = builder
						.SetUseLazyPreparation(false)?
						.SetTrackSelector(new DefaultTrackSelector(this))?
						.Build() ?? throw new Exception("Could not create exoplayer");

					_Exoplayer.AddListener(this);
					_Exoplayer.AddAnalyticsListener(this);
					_Exoplayer.Prepare();
				}

				return _Exoplayer;
			}
		}

		void IAnalyticsListener.OnLoadError(AnalyticsListenerEventTime? eventTime, LoadEventInfo? loadEventInfo, MediaLoadData? mediaLoadData, IOException? error, bool wasCanceled)
		{
			(Player as ServicePlayer)?.Next();
			(Player as ServicePlayer)?.RaisePlayerError(PlayerErrors.Load);
		}
		void IAnalyticsListener.OnPlayerError(AnalyticsListenerEventTime? eventTime, PlaybackException? error)
		{
			(Player as ServicePlayer)?.Next();
			(Player as ServicePlayer)?.RaisePlayerError(PlayerErrors.Play);
		}

		void IPlayerListener.OnPlayerStateChanged(bool playWhenReady, int playbackState)
		{
			if (Player.Queue.CurrentIndex != Exoplayer.CurrentWindowIndex)
				Player.Queue.CurrentIndex = Exoplayer.CurrentWindowIndex;
		}
		void IPlayerListener.OnTimelineChanged(Timeline? timeline, int reason)
		{
			if (Player.Queue.CurrentIndex.HasValue && Exoplayer.CurrentWindowIndex != Player.Queue.CurrentIndex && Player.Queue.CurrentIndex < Exoplayer.MediaItemCount)
			{
				Exoplayer.SeekTo(Player.Queue.CurrentIndex.Value, 0);
				Player.Queue.CurrentIndex = Exoplayer.CurrentWindowIndex;
			}
		}
		void IPlayerListener.OnTracksChanged(Tracks? tracks)
		{
			if (Player.Queue.CurrentIndex != Exoplayer.CurrentWindowIndex)
				Player.Queue.CurrentIndex = Exoplayer.CurrentWindowIndex;
		}
	}
}