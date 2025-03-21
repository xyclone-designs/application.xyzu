﻿using Android.Media.Audiofx;
using AndroidX.Media3.Common;

using System;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService
	{
		private BassBoost? _EffectsBassBoost;
		private EnvironmentalReverb? _EffectsEnvironmentalReverb;
		private Equalizer? _EffectsEqualizer;
		private LoudnessEnhancerEffect? _EffectsLoudnessEnhancer;

		public BassBoost? EffectsBassBoost
		{
			set => _EffectsBassBoost = value;
			get
			{
				try
				{
					_EffectsBassBoost ??= new BassBoost(0, Exoplayer.AudioSessionId);
				}
				catch (Exception) { }

				return _EffectsBassBoost;
			}
		}
		public EnvironmentalReverb? EffectsEnvironmentalReverb
		{
			set => _EffectsEnvironmentalReverb = value;
			get
			{
				try
				{
					_EffectsEnvironmentalReverb = new EnvironmentalReverb(0, Exoplayer.AudioSessionId);
				}
				catch (Exception) { }

				return _EffectsEnvironmentalReverb;
			}
		}
		public Equalizer? EffectsEqualizer
		{
			set => _EffectsEqualizer = value;
			get
			{
				try
				{
					_EffectsEqualizer ??= new Equalizer(0, Exoplayer.AudioSessionId);
				}
				catch (Exception) { }

				return _EffectsEqualizer;
			}
		}
		public LoudnessEnhancerEffect? EffectsLoudnessEnhancer
		{
			set => _EffectsLoudnessEnhancer = value;
			get
			{
				try
				{
					_EffectsLoudnessEnhancer ??= new LoudnessEnhancerEffect(Exoplayer.AudioSessionId);
				}
				catch (Exception) { }

				return _EffectsLoudnessEnhancer;
			}
		}
		public float EffectsPlaybackBalance
		{
			get; set;
		}
		public float EffectsPlaybackPitch
		{
			get => Exoplayer.PlaybackParameters?.Pitch ?? 1F;
			set
			{
				Exoplayer.PlaybackParameters ??= new PlaybackParameters(EffectsPlaybackSpeed, 1F);

				if (Exoplayer.PlaybackParameters.Pitch != value)
				{
					Exoplayer.PlaybackParameters.Pitch = value;
					Exoplayer.SeekTo(Exoplayer.CurrentMediaItemIndex, Exoplayer.CurrentPosition);
				}
			}
		}
		public float EffectsPlaybackSpeed
		{
			get => Exoplayer.PlaybackParameters?.Speed ?? 1F;
			set
			{
				Exoplayer.PlaybackParameters ??= new PlaybackParameters(1F, EffectsPlaybackPitch);
				
				if (Exoplayer.PlaybackParameters.Speed != value)
				{
					Exoplayer.PlaybackParameters.Speed = value;
					Exoplayer.SeekTo(Exoplayer.CurrentMediaItemIndex, Exoplayer.CurrentPosition);
				}
			}
		}
	}
}