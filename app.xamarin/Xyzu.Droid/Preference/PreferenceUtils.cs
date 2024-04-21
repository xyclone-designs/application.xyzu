#nullable enable

using Android.Content;
using AndroidX.AppCompat.App;

using Google.Android.Material.Dialog;

using System;
using System.Linq;

using Xyzu.Droid;

using AndroidXDialogPreference = AndroidX.Preference.DialogPreference;
using AndroidXListPreference = AndroidX.Preference.ListPreference;
using AndroidXMultiSelectListPreference = AndroidX.Preference.MultiSelectListPreference;

namespace Xyzu.Preference
{
	public class PreferenceUtils
	{
		public static AlertDialog.Builder? StyledAlertDialog(Context context, AndroidXDialogPreference dialogpreference)
		{
			AlertDialog.Builder? alertdialogbuilder = new MaterialAlertDialogBuilder(context, Resource.Style.Xyzu_Preference_AlertDialog)
				.SetTitle(dialogpreference.DialogTitle);

			return alertdialogbuilder;
		}
		public static AlertDialog.Builder? StyledAlertDialog(Context context, AndroidXListPreference listpreference, Func<AlertDialog?>? getalertdialog)
		{
			void SingleChoiceItemsHandler(object sender, DialogClickEventArgs args)
			{
				if (listpreference.GetEntryValues()?[args.Which] is string value)
					listpreference.CallChangeListener(value);

				AlertDialog? alertdialog = getalertdialog?.Invoke();
				alertdialog?.Dismiss();
			};

			AlertDialog.Builder? alertdialogbuilder = StyledAlertDialog(context, listpreference)?
				.SetSingleChoiceItems(
					handler: SingleChoiceItemsHandler,
					items: listpreference.GetEntryValues() ?? Array.Empty<string>(), 
					checkedItem: listpreference.FindIndexOfValue(listpreference.Value));

			return alertdialogbuilder;
		}	  
		public static AlertDialog.Builder? StyledAlertDialog(Context context, AndroidXMultiSelectListPreference multiselectlistpreference, Func<AlertDialog?>? getalertdialog)
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

			AlertDialog.Builder? alertdialogbuilder = StyledAlertDialog(context, multiselectlistpreference)?
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
				alertdialogbuilder?.SetPositiveButton(multiselectlistpreference.PositiveButtonText, PositiveButtonHandler);

			return alertdialogbuilder;
		}
	}
}