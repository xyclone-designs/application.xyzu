#nullable enable

using Android.OS;
using Android.Runtime;

using Xyzu.Droid;
using Xyzu.Settings.Audio;

namespace Xyzu.Fragments.Settings.Audio
{
	[Register(FragmentName)]
	public class AudioPreferenceFragment : BasePreferenceFragment, IAudioSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.Audio.AudioPreferenceFragment";

		public static class Keys
		{
			public const string BassBoost = "settings_audio_bassboost_preferencescreen_key";
			public const string EnvironmentalReverb = "settings_audio_environmentalreverb_preferencescreen_key";
			public const string Equaliser = "settings_audio_equaliser_preferencescreen_key";
			public const string LoudnessEnhancer = "settings_audio_loudnessenhancer_preferencescreen_key";
		}

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_audio_title);
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_audio, rootKey);
			InitPreferences(
				FindPreference(Keys.BassBoost),
				FindPreference(Keys.EnvironmentalReverb),
				FindPreference(Keys.Equaliser),
				FindPreference(Keys.LoudnessEnhancer));
		}
	}
}