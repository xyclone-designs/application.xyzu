using Android.OS;

using Xyzu.Droid;
using Xyzu.Settings;

namespace Xyzu.Fragments.Settings
{
	public class SettingsPreferenceFragment : BasePreferenceFragment, ISettings
	{
		public static class Keys
		{
			public const string Audio = "settings_audio_preferencescreen_key";
			public const string Files = "settings_files_preferencescreen_key";
			public const string Notification = "settings_notification_preferencescreen_key";
			public const string UserInterface = "settings_userinterface_preferencescreen_key";
			public const string Lockscreen = "settings_lockscreen_preferencescreen_key";
			public const string System = "settings_system_preferencescreen_key";
			public const string About = "settings_about_preferencescreen_key";
		}

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_title);
		}
		public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings, rootKey);
			InitPreferences(
				FindPreference(Keys.Audio),
				FindPreference(Keys.Files),
				FindPreference(Keys.Notification),
				FindPreference(Keys.UserInterface),
				FindPreference(Keys.Lockscreen),
				FindPreference(Keys.System),
				FindPreference(Keys.About));

			FindPreference(Keys.Lockscreen).Visible = false;
		}
	}
}