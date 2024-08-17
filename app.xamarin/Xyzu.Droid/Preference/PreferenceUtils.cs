#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Preference;

using System;
using System.Linq;

using Xyzu.Droid;

using AndroidXPreference = AndroidX.Preference.Preference;
using AndroidXDialogPreference = AndroidX.Preference.DialogPreference;
using AndroidXListPreference = AndroidX.Preference.ListPreference;
using AndroidXMultiSelectListPreference = AndroidX.Preference.MultiSelectListPreference;

namespace Xyzu.Preference
{
	public static class PreferenceUtils
	{
		public static AlertDialog.Builder ProcessDialog(this AlertDialog.Builder dialogbuilder, AndroidXDialogPreference dialogpreference)
		{
			dialogbuilder.SetTitle(dialogpreference.DialogTitle);

			return dialogbuilder;
		}
		public static AlertDialog.Builder ProcessList(this AlertDialog.Builder dialogbuilder, AndroidXListPreference listpreference, Func<AlertDialog?>? getalertdialog)
		{
			void SingleChoiceItemsHandler(object sender, DialogClickEventArgs args)
			{
				if (listpreference.GetEntryValues()?[args.Which] is string value)
					listpreference.CallChangeListener(value);

				AlertDialog? alertdialog = getalertdialog?.Invoke();
				alertdialog?.Dismiss();
			};

			return dialogbuilder
				.ProcessDialog(listpreference)
				.SetSingleChoiceItems(
					handler: SingleChoiceItemsHandler,
					items: listpreference.GetEntryValues() ?? Array.Empty<string>(),
					checkedItem: listpreference.FindIndexOfValue(listpreference.Value));
		}
		public static AlertDialog.Builder ProcessListMulti(this AlertDialog.Builder dialogbuilder, AndroidXMultiSelectListPreference multiselectlistpreference, Func<AlertDialog?>? getalertdialog) 
		{
			void PositiveButtonHandler(object sender, DialogClickEventArgs args)
			{
				AlertDialog? alertdialog = getalertdialog?.Invoke();
				alertdialog?.Dismiss();
			};
			void MultiChoiceItemsHandler(object sender, DialogMultiChoiceClickEventArgs args)
			{
				multiselectlistpreference.CallChangeListener(args.Which);
			};

			dialogbuilder = dialogbuilder
				.ProcessDialog(multiselectlistpreference)
				.SetMultiChoiceItems(
					handler: MultiChoiceItemsHandler,
					items: multiselectlistpreference.GetEntryValues() ?? Array.Empty<string>(),
					checkedItems: multiselectlistpreference.Values
						.Select((value, index) =>
						{
							if (multiselectlistpreference.GetEntryValues()?[index] is string preferencevalue)
								return string.Equals(value, preferencevalue, StringComparison.OrdinalIgnoreCase);

							return false;

						}).ToArray());


			if (multiselectlistpreference.PositiveButtonText != null)
				dialogbuilder.SetPositiveButton(multiselectlistpreference.PositiveButtonText, PositiveButtonHandler);

			return dialogbuilder;
		}
	}
}