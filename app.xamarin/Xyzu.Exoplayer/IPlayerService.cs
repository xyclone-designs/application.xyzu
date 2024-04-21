#nullable enable

using Android.Content;
using Android.Media.Audiofx;

using System;

namespace Xyzu.Player
{
	public interface IPlayerService
	{
		IPlayer Player { get; set; }
		IPlayerServiceBinder Binder { get; set; }
		IPlayerServiceOptions Options { get; set; }

		ComponentName? Componentname { get; set; }

		BassBoost? EffectsBassBoost { get; }
		EnvironmentalReverb? EffectsEnvironmentalReverb { get; }
		Equalizer? EffectsEqualizer { get; }
		LoudnessEnhancerEffect? EffectsLoudnessEnhancer { get; }

		void ProcessIntent(Intent? intent);

		event EventHandler<Intent> OnIntent;
	}
}