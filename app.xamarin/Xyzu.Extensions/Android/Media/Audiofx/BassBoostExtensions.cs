#nullable enable

using Xyzu.Settings.Audio;

namespace Android.Media.Audiofx
{
	public static class BassBoostExtensions
	{
		public static BassBoost SetSettingsPreset(this BassBoost bassboost, IBassBoostSettings.IPreset? settings, IBassBoostSettings.IPreset? @default = null)
		{
			if ((settings?.Strength ?? @default?.Strength) is short strength)
				bassboost.SetStrength(strength);

			return bassboost;
		}
	}
}