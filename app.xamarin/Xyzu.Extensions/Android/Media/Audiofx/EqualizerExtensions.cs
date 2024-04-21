#nullable enable

using Xyzu.Settings.Audio;

namespace Android.Media.Audiofx
{
	public static class EqualizerExtensions
	{
		public static Equalizer SetSettingsPreset(this Equalizer equalizer, IEqualiserSettings.IPreset? settings, IEqualiserSettings.IPreset? @default = null)
		{
			if ((settings?.FrequencyLevels ?? @default?.FrequencyLevels) is short[] frequencylevels)
				for (short band = 0; band < frequencylevels.Length; band++)
					equalizer.SetBandLevel(band, frequencylevels[band]);

			return equalizer;
		}
	}
}