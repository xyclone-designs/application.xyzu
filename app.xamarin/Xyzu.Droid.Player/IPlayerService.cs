using Android.Content;
using Android.Media.Audiofx;

using System;

namespace Xyzu.Player
{
	public partial interface IPlayerService
	{
		IPlayer Player { get; set; }
		IBinder Binder { get; set; }
		IOptions Options { get; set; }

		ComponentName? Componentname { get; set; }

		BassBoost? EffectsBassBoost { get; set; }
		EnvironmentalReverb? EffectsEnvironmentalReverb { get; set; }
		Equalizer? EffectsEqualizer { get; set; }
		LoudnessEnhancerEffect? EffectsLoudnessEnhancer { get; set; }
		float EffectsPlaybackBalance { get; set; }
		float EffectsPlaybackPitch { get; set; }
		float EffectsPlaybackSpeed { get; set; }

		void ProcessIntent(Intent? intent);

		event EventHandler<Intent> OnIntent;
	}
}