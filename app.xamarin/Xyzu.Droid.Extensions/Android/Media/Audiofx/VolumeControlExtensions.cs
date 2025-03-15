using Xyzu.Settings.Audio;

namespace Android.Media.Audiofx
{
	public static class VolumeControlExtensions
	{
		public static BassBoost SetSettingsPreset(this BassBoost bassboost, IVolumeControlSettings.IPreset? settings, IVolumeControlSettings.IPreset? @default = null)
		{
			if ((settings?.BassBoostStrength ?? @default?.BassBoostStrength) is short strength)
				bassboost.SetStrength(strength);

			return bassboost;
		}
		public static LoudnessEnhancer SetSettingsPreset(this LoudnessEnhancer loudnessenhancer, IVolumeControlSettings.IPreset? settings, IVolumeControlSettings.IPreset? @default = null)
		{
			if ((settings?.LoudnessEnhancerTargetGain ?? @default?.LoudnessEnhancerTargetGain) is short targetgain)
				loudnessenhancer.SetTargetGain(targetgain);

			return loudnessenhancer;
		}
	}
}