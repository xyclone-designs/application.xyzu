#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.System;

using AndroidXPreference = AndroidX.Preference.Preference;
using JavaFile = Java.IO.File;
using XyzuDialogPreference = Xyzu.Preference.DialogPreference;
using XyzuListPreference = Xyzu.Preference.ListPreference;

namespace Xyzu.Fragments.Settings.System
{
	[Register(FragmentName)]
	public class SystemPreferenceFragment : BasePreferenceFragment, ISystemSettingsDroid
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.System.SystemPreferenceFragment";

		public static class Keys
		{
			public const string LanguageCurrent = "settings_system_languagecurrent_listpreference_key";
			public const string LanguageMode = "settings_system_languagemode_listpreference_key";
			public const string ThemeMode = "settings_system_thememode_listpreference_key";
			public const string ErrorLogs = "settings_system_errorlogs_dialogpreference_key";
		}

		private ThemeModes _ThemeMode;
		private LanguageModes _LanguageMode;
		private CultureInfo? _LanguageCurrent;
		private IEnumerable<ISystemSettings.IErrorLog>? _ErrorLogs;

		public ThemeModes ThemeMode
		{
			get => _ThemeMode;
			set
			{
				_ThemeMode = value;

				XyzuSettings.Instance
					.Edit()?
					.PutEnum(ISystemSettingsDroid.Keys.ThemeMode, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LanguageModes LanguageMode
		{
			get => _LanguageMode;
			set
			{
				_LanguageMode = value;

				XyzuSettings.Instance
					.Edit()?
					.PutEnum(ISystemSettingsDroid.Keys.LanguageMode, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public CultureInfo LanguageCurrent
		{
			get => _LanguageCurrent ?? ISystemSettingsDroid.Defaults.LanguageCurrent;
			set
			{
				_LanguageCurrent = value;

				XyzuSettings.Instance
					.Edit()?
					.PutCultureInfo(ISystemSettingsDroid.Keys.LanguageCurrent, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public IEnumerable<ISystemSettings.IErrorLog> ErrorLogs
		{
			get => _ErrorLogs ?? Enumerable.Empty<ISystemSettings.IErrorLog>();
			set
			{
				_ErrorLogs = value;

				OnPropertyChanged();
			}
		}

		public XyzuListPreference? ThemeModePreference { get; set; }
		public XyzuListPreference? LanguageCurrentPreference { get; set; }
		public XyzuListPreference? LanguageModePreference { get; set; }
		public XyzuDialogPreference? ErrorLogsPreference { get; set; }

		AlertDialog? ErrorLogDialog { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_system_title);

			AddPreferenceChangeHandler(
				LanguageCurrentPreference,
				LanguageModePreference,
				ErrorLogsPreference);

			ISystemSettingsDroid settings = XyzuSettings.Instance.GetSystemDroid();

			_LanguageMode = settings.LanguageMode; OnPropertyChanged(nameof(LanguageMode));
			_LanguageCurrent = settings.LanguageCurrent; OnPropertyChanged(nameof(LanguageCurrent));
			_ThemeMode = settings.ThemeMode; OnPropertyChanged(nameof(ThemeMode));
			ErrorLogs = settings.ErrorLogs = ISystemSettingsDroid.GetErrorLogs();
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				LanguageCurrentPreference,
				LanguageModePreference,
				ThemeModePreference,
				ErrorLogsPreference);

			XyzuSettings.Instance
				.Edit()?
				.PutSystemDroid(this)
				.Apply();
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_system, rootKey);
			InitPreferences(
				LanguageCurrentPreference = FindPreference(Keys.LanguageCurrent) as XyzuListPreference,
				LanguageModePreference = FindPreference(Keys.LanguageMode) as XyzuListPreference,
				ThemeModePreference = FindPreference(Keys.ThemeMode) as XyzuListPreference,
				ErrorLogsPreference = FindPreference(Keys.ErrorLogs) as XyzuDialogPreference);

			LanguageCurrentPreference?.SetEntries(
				entries: ISystemSettingsDroid.Options.LanguageCurrent.AsEnumerable()
					.Select(language => language.ThreeLetterISOLanguageName)
					.ToArray());
			LanguageCurrentPreference?.SetEntryValues(
				entryValues: ISystemSettingsDroid.Options.LanguageCurrent.AsEnumerable()
					.Select(language => language.DisplayName)
					.ToArray());

			LanguageModePreference?.SetEntries(
				entries: ISystemSettingsDroid.Options.LanguageMode.AsEnumerable()
					.Select(mode => mode.ToString())
					.ToArray());
			LanguageModePreference?.SetEntryValues(
				entryValues: ISystemSettingsDroid.Options.LanguageMode.AsEnumerable()
					.Select(mode => mode.AsStringTitle(Context) ?? mode.ToString())
					.ToArray());

			ThemeModePreference?.SetEntries(
				entries: ISystemSettingsDroid.Options.ThemeMode.AsEnumerable()
					.Select(mode => mode.ToString())
					.ToArray());
			ThemeModePreference?.SetEntryValues(
				entryValues: ISystemSettingsDroid.Options.ThemeMode.AsEnumerable()
					.Select(mode => mode.AsStringTitle(Context) ?? mode.ToString())
					.ToArray());

			if (ErrorLogsPreference != null)
				ErrorLogsPreference.DialogOnBuild = dialogbuilder =>
				{
					if (ErrorLogsPreference.PositiveButtonText != null)
						dialogbuilder.SetPositiveButton(ErrorLogsPreference.PositiveButtonText, (sender, args) => ErrorLogsPreference?.CurrentAlertDialog?.Dismiss());

					if (ErrorLogsPreference.NegativeButtonText != null)
						dialogbuilder.SetNegativeButton(ErrorLogsPreference.NegativeButtonText, (sender, args) =>
						{
							ErrorLogDialog = XyzuUtils.Dialogs.Alert(Context!, (alertdialogbuilder, alertdialog) =>
							{
								if (alertdialogbuilder != null)
								{
									alertdialogbuilder.SetTitle(Resource.String.settings_system_errorlogs_confirmationdialog_title);
									alertdialogbuilder.SetMessage(Resource.String.settings_system_errorlogs_confirmationdialog_message);
									alertdialogbuilder.SetNegativeButton(Resource.String.settings_system_errorlogs_confirmationdialog_negativebutton, (sender, args) =>
									{
										ErrorLogDialog?.Dismiss();

										ErrorLogsPreference.DialogShow();
									});
									alertdialogbuilder.SetPositiveButton(Resource.String.settings_system_errorlogs_confirmationdialog_positivebutton, async (sender, args) =>
									{
										ErrorLogDialog?.Dismiss();

										await ISystemSettingsDroid.ClearErrorLogs();
									});
								}
							});

							ErrorLogDialog.Show();
						});

					dialogbuilder.SetItems(ErrorLogs.Select(errorlog =>
					{
						return string.Format("{0} - {1}", errorlog.Date.ToString("yyyy-MM-dd HH:mm"), errorlog.Id);

					}).ToArray(), (sender, args) =>
					{
						ISystemSettings.IErrorLog errorlog = ErrorLogs.ElementAt(args.Which);

						if (errorlog.Path is null || Context is null)
							return;

						Intent? intentchooser = XyzuUtils.Intents.Chooser_ViewErrorLog(Context, new JavaFile(errorlog.Path));

						Activity?.StartActivityForResult(intentchooser, -1);
					});
				};			
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(LanguageCurrent) when
				LanguageCurrentPreference != null:
					if (ISystemSettingsDroid.Options.LanguageCurrent.AsEnumerable().Index(LanguageCurrent) is int languagecurrentindex)
						LanguageCurrentPreference.SetValueIndex(languagecurrentindex);
					break;

				case nameof(LanguageMode) when
				LanguageModePreference != null:
					if (ISystemSettingsDroid.Options.LanguageMode.AsEnumerable().Index(LanguageMode) is int languagemodeindex)
						LanguageModePreference.SetValueIndex(languagemodeindex);
					break;

				case nameof(ThemeMode) when
				ThemeModePreference != null:
					if (ISystemSettingsDroid.Options.ThemeMode.AsEnumerable().Index(ThemeMode) is int thememodeindex)
						ThemeModePreference.SetValueIndex(thememodeindex);
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
				preference == LanguageCurrentPreference &&
				LanguageCurrentPreference.Value is not null &&
				LanguageCurrentPreference.Value != LanguageCurrent?.IetfLanguageTag:
					LanguageCurrent = CultureInfo.GetCultureInfoByIetfLanguageTag(LanguageCurrentPreference.Value);
					return true;

				case true when
				preference == LanguageModePreference &&
				newvalue?.ToString() is string newvaluestring &&
				Enum.TryParse(newvaluestring, out LanguageModes mode) &&
				LanguageMode != mode:
					LanguageMode = mode;
					return true;

				case true when
				preference == ThemeModePreference &&
				newvalue?.ToString() is string newvaluestring &&
				Enum.TryParse(newvaluestring, out ThemeModes mode) &&
				ThemeMode != mode:
					ThemeMode = mode;
					return true;

				default: return result;
			}
		}
	}
}