#nullable enable

using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Audio;
using Com.Google.Android.Exoplayer2.Metadata;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Text;
using Com.Google.Android.Exoplayer2.Trackselection;

using System.Linq;
using System.Collections.Generic;

using Xyzu.Player.Enums;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService : ITextOutput, IAudioListener, IMetadataOutput, IPlayerEventListener
	{
		private SimpleExoPlayer? _Exoplayer;

		public SimpleExoPlayer Exoplayer
		{
			get
			{
				if (_Exoplayer is null)
				{
					using SimpleExoPlayer.Builder builder = new SimpleExoPlayer.Builder(this);

					_Exoplayer = builder
						.SetUseLazyPreparation(true)
						.SetTrackSelector(new DefaultTrackSelector(this))
						.Build();

					_Exoplayer.AddListener(this);
					_Exoplayer.AddTextOutput(this);
					_Exoplayer.AddAudioListener(this);

					_Exoplayer.Prepare(ConcatenatingMediaSource, true, true);
				}

				return _Exoplayer;
			}
		}

		void ITextOutput.OnCues(IList<Cue> cues)
		{
			Android.Util.Log.Debug(
				tag: nameof(ITextOutput.OnCues),
				format: "Cues: {0}",
				args: string.Join("\n", cues.Select((cue, index) => string.Format("{0}: {1}", index, cue.Text))));
		}

		void IAudioListener.OnAudioAttributesChanged(AudioAttributes audioAttributes)
		{
			Android.Util.Log.Debug(
				tag: nameof(IAudioListener.OnAudioAttributesChanged),
				msg: "Audio Attributes Changed");
		}
		void IAudioListener.OnAudioSessionId(int audioSessionId)
		{
			Android.Util.Log.Debug(
				tag: nameof(IAudioListener.OnAudioSessionId),
				format: "Audio Session Id: '{0}'",
				args: audioSessionId);
		}
		void IAudioListener.OnVolumeChanged(float volume)
		{
			Android.Util.Log.Debug(
				tag: nameof(IAudioListener.OnVolumeChanged),
				format: "Volume: '{0}'",
				args: volume);
		}

		void IMetadataOutput.OnMetadata(Metadata metadata)
		{
			Android.Util.Log.Debug(
				tag: nameof(IMetadataOutput.OnMetadata),
				format: "Metadata Length: '{0}'",
				args: metadata.Length());
		}

		void IPlayerEventListener.OnIsPlayingChanged(bool isPlaying)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnIsPlayingChanged),
				msg: isPlaying ? "Is Playing" : "Is Not Playing");
		}
		void IPlayerEventListener.OnLoadingChanged(bool isLoading)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnLoadingChanged),
				msg: isLoading ? "Is Loading" : "Is Not Loading");
		}
		void IPlayerEventListener.OnPlaybackParametersChanged(PlaybackParameters playbackParameters)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnPlaybackParametersChanged),
				msg: "Audio Attributes Changed");
		}
		void IPlayerEventListener.OnPlayerError(ExoPlaybackException error)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnPlayerError),
				msg: error.LocalizedMessage ?? error.Message ?? string.Empty);
		}
		void IPlayerEventListener.OnPlayerStateChanged(bool playWhenReady, int playbackState)
		{
			(string playbackstate, PlayerStates state) = playbackState switch
			{
				BasePlayer.InterfaceConsts.StateIdle
					=> ("Idle", PlayerStates.NoSong),

				BasePlayer.InterfaceConsts.StateReady
					=> ("Ready", PlayerStates.Paused),

				BasePlayer.InterfaceConsts.StateBuffering
					=> ("Buffering", PlayerStates.Loading),

				BasePlayer.InterfaceConsts.StateEnded
					=> ("Ended", PlayerStates.Stopped),

				_ => (string.Format("Unknown Playback State: '{0}'", playbackState), Player.State)
			};

			Android.Util.Log.Debug(
				args: playbackstate,
				format: "ExoPlayer Playback State: {0}",
				tag: nameof(IPlayerEventListener.OnPlayerStateChanged));

			if (Player.Queue.CurrentIndex != Exoplayer.CurrentWindowIndex)
				Player.Queue.CurrentIndex = Exoplayer.CurrentWindowIndex;
		}
		void IPlayerEventListener.OnPositionDiscontinuity(int reason)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnPositionDiscontinuity),
				format: "ExoPlayer Position Discontinuity Reason: {0}",
				args: reason switch
				{
					BasePlayer.InterfaceConsts.DiscontinuityReasonInternal
						=> "Internal",

					BasePlayer.InterfaceConsts.DiscontinuityReasonAdInsertion
						=> "Ad Insertion",

					BasePlayer.InterfaceConsts.DiscontinuityReasonPeriodTransition
						=> "Period Transition",

					BasePlayer.InterfaceConsts.DiscontinuityReasonSeek
						=> "Seek",

					BasePlayer.InterfaceConsts.DiscontinuityReasonSeekAdjustment
						=> "Seek Adjustment",

					_ => string.Format("Unknown '{0}'", reason)
				});
		}
		void IPlayerEventListener.OnRepeatModeChanged(int repeatMode)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnRepeatModeChanged),
				format: "ExoPlayer Repeat Mode: {0}",
				args: repeatMode switch
				{
					BasePlayer.InterfaceConsts.RepeatModeAll
						=> "All",

					BasePlayer.InterfaceConsts.RepeatModeOne
						=> "One",

					BasePlayer.InterfaceConsts.RepeatModeOff
						=> "None",

					_ => string.Format("Unknown : '{0}'", repeatMode)
				});
		}
		void IPlayerEventListener.OnSeekProcessed()
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnSeekProcessed),
				msg: "Seek Processed");
		}
		void IPlayerEventListener.OnShuffleModeEnabledChanged(bool shuffleModeEnabled)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnShuffleModeEnabledChanged),
				msg: shuffleModeEnabled ? "Shuffle Mode Enabled" : "Shuffle Mode Disabled");
		}
		void IPlayerEventListener.OnTimelineChanged(Timeline timeline, int reason)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnTimelineChanged),
				format: "ExoPlayer Timeline Change: '{0}'",
				args: reason switch
				{
					BasePlayer.InterfaceConsts.TimelineChangeReasonPrepared
						=> "Prepared",

					BasePlayer.InterfaceConsts.TimelineChangeReasonReset
						=> "Reset",

					BasePlayer.InterfaceConsts.TimelineChangeReasonDynamic
						=> "Period Transition",

					_ => string.Format("Unknown ({0})", reason)
				});

			if (Player.Queue.CurrentIndex.HasValue && Exoplayer.CurrentWindowIndex != Player.Queue.CurrentIndex && Player.Queue.CurrentIndex < ConcatenatingMediaSource.Size)
			{
				Exoplayer.SeekTo(Player.Queue.CurrentIndex.Value, 0);
				Player.Queue.CurrentIndex = Exoplayer.CurrentWindowIndex;
			}
		}
		void IPlayerEventListener.OnTracksChanged(TrackGroupArray trackGroups, TrackSelectionArray trackSelections)
		{
			Android.Util.Log.Debug(
				tag: nameof(IPlayerEventListener.OnTracksChanged),
				format: "Track Groups Length: '{0}', Track Selection Length: '{1}'",
				args: new object[] { trackGroups.Length, trackSelections.Length });

			if (Player.Queue.CurrentIndex != Exoplayer.CurrentWindowIndex)
				Player.Queue.CurrentIndex = Exoplayer.CurrentWindowIndex;
		}
	}
}