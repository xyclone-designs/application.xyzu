#nullable enable

using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;

using Google.Android.Material.BottomSheet;
using Google.Android.Material.Dialog;
using Google.Android.Material.Snackbar;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Menus;

using AndroidUri = Android.Net.Uri;
using JavaFile = Java.IO.File;
using JavaURI = Java.Net.URI;

namespace Xyzu
{
	public sealed class XyzuUtils
	{
		public static class Dialogs
		{
			public static AlertDialog Alert(Context context, Action<AlertDialog.Builder?, AlertDialog?>? action, int? style = null)
			{
				AlertDialog.Builder alertdialogbuilder = new MaterialAlertDialogBuilder(context, style ?? Resource.Style.Xyzu_Material_Dialog_Alert);

				action?.Invoke(alertdialogbuilder, null);

				AlertDialog alertdialog = alertdialogbuilder.Create();

				if (alertdialog.Window != null)
				{
					alertdialog.Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
					alertdialog.Window.AddFlags(WindowManagerFlags.TranslucentStatus);
					alertdialog.Window.AddFlags(WindowManagerFlags.LayoutNoLimits);
					alertdialog.Window.SetLayout(MenuOptionsUtils.DialogWidth(context), MenuOptionsUtils.DialogHeight(context));
					alertdialog.Window.SetSoftInputMode(SoftInput.AdjustPan | SoftInput.AdjustResize | SoftInput.StateHidden);
				}

				action?.Invoke(null, alertdialog);

				return alertdialog;
			}
			public static BottomSheetDialog BottomSheet(Context context, Action<BottomSheetDialog>? bottomsheetdialogaction, int? style = null)
			{
				BottomSheetDialog bottomsheetdialog = new BottomSheetDialog(context, style ?? Resource.Style.Xyzu_Material_Dialog_Sheet_Bottom);
				
				bottomsheetdialog.Behavior.State = BottomSheetBehavior.StateExpanded;
				bottomsheetdialog.Behavior.MaxWidth = MenuOptionsUtils.DialogMaxWidth(context);
				bottomsheetdialog.Behavior.MaxHeight = MenuOptionsUtils.DialogMaxHeight(context);

				if (bottomsheetdialog.Window != null)
				{
					bottomsheetdialog.Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
					bottomsheetdialog.Window.AddFlags(WindowManagerFlags.TranslucentStatus);
					bottomsheetdialog.Window.AddFlags(WindowManagerFlags.LayoutNoLimits);
					bottomsheetdialog.Window.SetLayout(MenuOptionsUtils.DialogWidth(context), MenuOptionsUtils.DialogHeight(context));
					bottomsheetdialog.Window.SetGravity(MenuOptionsUtils.DialogGravityFlags(context));
					bottomsheetdialog.Window.SetSoftInputMode(SoftInput.AdjustPan | SoftInput.AdjustResize | SoftInput.StateHidden);
				}

				bottomsheetdialogaction?.Invoke(bottomsheetdialog);

				return bottomsheetdialog;
			}
			public static Snackbar SnackBar(Context context, View parent, Action<Snackbar>? snackbaraction)
			{
				Snackbar snackbar = Snackbar.Make(context, parent, string.Empty, Snackbar.LengthLong);

				if (context.Resources?.GetColor(Resource.Color.ColorPrimary, context.Theme) is Color colorprimary)
					snackbar.SetBackgroundTint(colorprimary);
				
				if (context.Resources?.GetColor(Resource.Color.ColorOnPrimary, context.Theme) is Color coloronprimary)
					snackbar.SetTextColor(coloronprimary);

				snackbar.SetGestureInsetBottomIgnored(true);

				snackbar.View.Background = context.Resources?.GetDrawable(Resource.Drawable.xyzu_material_snackbar_background, context.Theme);

				snackbaraction?.Invoke(snackbar);

				return snackbar;
			}
		}

		public static class Intents
		{
			public const string FilePrividerAuthority = "co.za.xyclonedesigns.xyzu.provider";

			public static Intent? App_Email(Context? context, string? address)
			{
				Intent intent = new Intent(Intent.ActionSendto)
					.SetData(AndroidUri.Parse(string.Format("mailto:{0}", address)));

				Intent? intentchooser = Intent.CreateChooser(intent, null as string);

				return intentchooser;
			}
			public static Intent? App_Browser(Context? context, string? url)
			{
				AndroidUri? uri = AndroidUri.Parse(url);
				Intent intent = new Intent(Intent.ActionView, uri);

				Intent? intentchooser = Intent.CreateChooser(intent, null as string);

				return intent;
			}

			public static Intent? Chooser_ShareFiles(Context? context, params JavaURI[] uris)
			{
				IList<IParcelable> parcelables = context is null ? new List<IParcelable> { } : uris
					.Select(uri => FileProvider.GetUriForFile(context, FilePrividerAuthority, new JavaFile(uri)))
					.OfType<IParcelable>()
					.ToList();

				Intent intent = new Intent(Intent.ActionSendMultiple)
					.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantPersistableUriPermission)
					.SetType(IntentTypes.Audio.AsIntentType())
					.PutParcelableArrayListExtra(Intent.ExtraStream, parcelables);

				Intent? intentchooser = Intent.CreateChooser(intent, context?.GetString(Resource.String.intentchooser_share_files))?
					.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantPersistableUriPermission);

				return intentchooser;
			}		 
			public static Intent? Chooser_ViewErrorLog(Context? context, JavaFile file)
			{
				AndroidUri? androiduri = context is null ? null : FileProvider.GetUriForFile(context, FilePrividerAuthority, file);

				Intent intent = new Intent(Intent.ActionView);
				intent.SetDataAndType(androiduri, "text/plain");

				Intent? intentchooser = Intent.CreateChooser(intent, context?.GetString(Resource.String.intentchooser_view_errorlog));

				return intentchooser;
			}
		}
	}
}