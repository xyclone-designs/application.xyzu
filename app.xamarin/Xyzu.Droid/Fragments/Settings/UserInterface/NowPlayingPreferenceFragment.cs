#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Settings.UserInterface;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuSwitchPreference = Xyzu.Preference.SwitchPreference;

namespace Xyzu.Fragments.Settings.UserInterface
{
	[Register(FragmentName)]
	public class NowPlayingPreferenceFragment : BasePreferenceFragment, INowPlayingSettingsDroid
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.NowPlayingPreferenceFragment";

		public static class Keys
		{
			public const string ForceShowNowPlaying = "settings_userinterface_nowplaying_forceshownowplaying_switchpreference_key";
		}

		private bool _ForceShowNowPlaying;

		public bool ForceShowNowPlaying
		{
			get => _ForceShowNowPlaying;
			set
			{

				_ForceShowNowPlaying = value;

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? ForceShowNowPlayingPreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_nowplaying_title);

			AddPreferenceChangeHandler(
				ForceShowNowPlayingPreference);

			INowPlayingSettingsDroid settings = XyzuSettings.Instance.GetUserInterfaceNowPlayingDroid();

			ForceShowNowPlaying = settings.ForceShowNowPlaying;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				ForceShowNowPlayingPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutUserInterfaceNowPlayingDroid(this)
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface_nowplaying, rootKey);
			InitPreferences(
				ForceShowNowPlayingPreference = FindPreference(Keys.ForceShowNowPlaying) as XyzuSwitchPreference);
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(ForceShowNowPlaying) when
				ForceShowNowPlayingPreference != null &&
				ForceShowNowPlayingPreference.Checked != ForceShowNowPlaying:
					ForceShowNowPlayingPreference.Checked = ForceShowNowPlaying;
					break;

				default: break;
			}
		}
		public override bool OnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			switch (true)
			{
				case true when
				preference == ForceShowNowPlayingPreference:
					ForceShowNowPlaying = newvalue?.JavaCast<Java.Lang.Boolean>().BooleanValue() ?? false;
					return true;

				default: return result;
			}
		}
	}
}