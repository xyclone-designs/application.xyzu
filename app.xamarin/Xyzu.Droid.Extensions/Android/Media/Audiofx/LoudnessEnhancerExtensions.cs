#nullable enable

using Xyzu.Settings.Audio;

namespace Android.Media.Audiofx
{
	public static class LoudnessEnhancerExtensions
	{
		public static LoudnessEnhancer SetSettingsPreset(this LoudnessEnhancer loudnessenhancer, ILoudnessEnhancerSettings.IPreset? settings, ILoudnessEnhancerSettings.IPreset? @default = null)
		{
			if ((settings?.TargetGain ?? @default?.TargetGain) is short targetgain)
				loudnessenhancer.SetTargetGain(targetgain);

			return loudnessenhancer;
		}
	}
}