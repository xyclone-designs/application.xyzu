#nullable enable

using Android.OS;
using Android.Runtime;

using Xyzu.Droid;
using Xyzu.Settings.UserInterface;

namespace Xyzu.Fragments.Settings.UserInterface
{
	[Register(FragmentName)]
	public class UserInterfacePreferenceFragment : BasePreferenceFragment, IUserInterfaceSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.UserInterfacePreferenceFragment";

		public static class Keys
		{
			public const string Languages = "settings_userinterface_languages_preferencescreen_key";
			public const string Library = "settings_userinterface_library_preferencescreen_key";
			public const string NowPlaying = "settings_userinterface_nowplaying_preferencescreen_key";
			public const string Themes = "settings_userinterface_themes_preferencescreen_key";
		}

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_title);
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface, rootKey);
			InitPreferences(
				FindPreference(Keys.Languages),
				FindPreference(Keys.Library),
				FindPreference(Keys.NowPlaying),
				FindPreference(Keys.Themes));
		}
	}
}