#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Globalization;
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
	public class LanguagesPreferenceFragment : BasePreferenceFragment, ILanguagesSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.LanguagesPreferenceFragment";

		public static class Keys
		{
			public const string CurrentLanguage = "settings_userinterface_languages_currentlanguage_listpreference_key";
			public const string Mode = "settings_userinterface_languages_mode_listpreference_key";
		}

		private LanguageModes _Mode;
		private CultureInfo? _CurrentLanguage;  

		public LanguageModes Mode
		{
			get => _Mode;
			set
			{
				_Mode = value;

				OnPropertyChanged();
			}
		}
		public CultureInfo? CurrentLanguage
		{
			get => _CurrentLanguage;
			set
			{
				_CurrentLanguage = value;

				OnPropertyChanged();
			}
		}

		public XyzuListPreference? CurrentLanguagePreference { get; set; }
		public XyzuListPreference? ModePreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_languages_title);

			AddPreferenceChangeHandler(
				CurrentLanguagePreference,
				ModePreference);

			ILanguagesSettings settings = XyzuSettings.Instance.GetUserInterfaceLanguages();

			Mode = settings.Mode;
			CurrentLanguage = settings.CurrentLanguage;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				CurrentLanguagePreference,
				ModePreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutUserInterfaceLanguages(this)
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface_languages, rootKey);
			InitPreferences(
				CurrentLanguagePreference = FindPreference(Keys.CurrentLanguage) as XyzuListPreference,
				ModePreference = FindPreference(Keys.Mode) as XyzuListPreference);

			CurrentLanguagePreference?.SetEntries(
				entries: ILanguagesSettings.Options.CurrentLanguage.AsEnumerable()
					.Select(language => language.ThreeLetterISOLanguageName)
					.ToArray());
			CurrentLanguagePreference?.SetEntryValues(
				entryValues: ILanguagesSettings.Options.CurrentLanguage.AsEnumerable()
					.Select(language => language.DisplayName)
					.ToArray());	

			ModePreference?.SetEntries(
				entries: ILanguagesSettings.Options.Modes.AsEnumerable()
					.Select(mode => mode.ToString())
					.ToArray());
			ModePreference?.SetEntryValues(
				entryValues: ILanguagesSettings.Options.Modes.AsEnumerable()
					.Select(mode => mode.AsStringTitle(Context) ?? mode.ToString())
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(CurrentLanguage) when
				CurrentLanguagePreference != null:
					if (ILanguagesSettings.Options.CurrentLanguage.AsEnumerable().Index(CurrentLanguage) is int currentlanguageindex)
						CurrentLanguagePreference.SetValueIndex(currentlanguageindex);
					break;
																	   
				case nameof(Mode) when
				ModePreference != null:
					if (ILanguagesSettings.Options.Modes.AsEnumerable().Index(Mode) is int modeindex)
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
				preference == CurrentLanguagePreference &&
				CurrentLanguagePreference.Value != CurrentLanguage?.IetfLanguageTag:
					CurrentLanguage = CultureInfo.GetCultureInfoByIetfLanguageTag(CurrentLanguagePreference.Value);
					return true;					

				case true when
				preference == ModePreference &&
				newvalue?.ToString() is string newvaluestring &&
				Enum.TryParse(newvaluestring, out LanguageModes mode) &&
				Mode != mode:
					Mode = mode;
					return true;

				default: return result;
			}
		}
	}
}