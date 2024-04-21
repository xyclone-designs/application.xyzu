#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;

namespace Xyzu.Fragments.Settings.UserInterface
{
	[Register(FragmentName)]
	public class ThemesPreferenceFragment : BasePreferenceFragment, IThemesSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.ThemesPreferenceFragment";

		public static class Keys										
		{
			public const string Mode = "settings_userinterface_themes_mode_listpreference_key";
		}

		private ThemeModes _Mode;

		public ThemeModes Mode
		{
			get => _Mode;
			set
			{
				_Mode = value;

				OnPropertyChanged();
			}
		}

		public XyzuListPreference? ModePreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_themes_title);

			AddPreferenceChangeHandler(ModePreference);

			IThemesSettings settings = XyzuSettings.Instance.GetUserInterfaceThemes();

			Mode = settings.Mode;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(ModePreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutUserInterfaceThemes(this)
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface_themes, rootKey);
			InitPreferences(
				ModePreference = FindPreference(Keys.Mode) as XyzuListPreference);

			ModePreference?.SetEntries(
				entries: IThemesSettings.Options.Modes.AsEnumerable()
					.Select(mode => mode.ToString())
					.ToArray());
			ModePreference?.SetEntryValues(
				entryValues: IThemesSettings.Options.Modes.AsEnumerable()
					.Select(mode => mode.AsStringTitle(Context) ?? mode.ToString())    
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(Mode) when
				ModePreference != null:
					if (IThemesSettings.Options.Modes.AsEnumerable().Index(Mode) is int modeindex)
						ModePreference.SetValueIndex(modeindex);
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
				preference == ModePreference &&
				newvalue?.ToString() is string newvaluestring &&
				Enum.TryParse(newvaluestring, out ThemeModes mode) &&
				Mode != mode:
					Mode = mode;
					return true;

				default: return result;
			}
		}
	}
}