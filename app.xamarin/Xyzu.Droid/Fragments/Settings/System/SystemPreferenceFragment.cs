#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Settings.System;

using JavaFile = Java.IO.File;
using XyzuDialogPreference = Xyzu.Preference.DialogPreference;

namespace Xyzu.Fragments.Settings.System
{
	[Register(FragmentName)]
	public class SystemPreferenceFragment : BasePreferenceFragment, ISystemSettingsDroid
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.System.SystemPreferenceFragment";

		public static class Keys
		{
			public const string ErrorLogs = "settings_system_errorlogs_dialogpreference_key";
		}

		private IEnumerable<ISystemSettings.IErrorLog>? _ErrorLogs;

		public IEnumerable<ISystemSettings.IErrorLog> ErrorLogs
		{
			get => _ErrorLogs ?? Enumerable.Empty<ISystemSettings.IErrorLog>();
			set
			{
				_ErrorLogs = value;

				OnPropertyChanged();
			}
		}

		public XyzuDialogPreference? ErrorLogsPreference { get; set; }

		AlertDialog? ErrorLogDialog { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_system_title);

			AddPreferenceChangeHandler(
				ErrorLogsPreference);

			ISystemSettingsDroid settings = XyzuSettings.Instance.GetSystemDroid();
			settings.ErrorLogs = ISystemSettingsDroid.GetErrorLogs(Context!);

			ErrorLogs = settings.ErrorLogs;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				ErrorLogsPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutSystemDroid(this)
				.Apply();
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_system, rootKey);
			InitPreferences(
				ErrorLogsPreference = FindPreference(Keys.ErrorLogs) as XyzuDialogPreference);

			if (ErrorLogsPreference != null)
				ErrorLogsPreference.DialogOnBuild = dialogbuilder =>
				{
					if (ErrorLogsPreference.PositiveButtonText != null)
						dialogbuilder.SetPositiveButton(ErrorLogsPreference.PositiveButtonText, (sender, args) => ErrorLogsPreference?.CurrentAlertDialog?.Dismiss());

					if (ErrorLogsPreference.NegativeButtonText != null)
						dialogbuilder.SetNegativeButton(ErrorLogsPreference.NegativeButtonText, (sender, args) =>
						{
							ErrorLogDialog = XyzuUtils.Dialogs.Alert(Context!, alertdialogbuilder =>
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

									await ISystemSettingsDroid.ClearErrorLogs(Context!);
								});
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
	}
}