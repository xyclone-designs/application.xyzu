﻿#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;

using AndroidX.AppCompat.App;
using AndroidX.Preference;
using System;
using Xyzu.Droid;
using Xyzu.Views.Preference;
using AndroidXDialogPreference = AndroidX.Preference.DialogPreference;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/DialogPreference")]
	public class DialogPreference : AndroidXDialogPreference
	{
		public DialogPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public DialogPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public DialogPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public DialogPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public DialogPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_preference;
			View = new PreferenceView();
		}

		public AlertDialog? CurrentAlertDialog { get; protected set; }
		public Action<AlertDialog.Builder>? DialogOnBuild { get; set; }
		public PreferenceView? View { get; set; }

		public void DialogShow()
		{
			OnClick();
		}
		protected override void OnClick()
		{
			// base.OnClick();

			CurrentAlertDialog = XyzuUtils.Dialogs.Alert(
				context: Context,
				style: Resource.Style.Xyzu_Preference_AlertDialog,
				action: (dialogbuilder, dialog) =>
				{
					if (dialogbuilder is null)
						return;

					dialogbuilder.ProcessDialog(this);
					DialogOnBuild?.Invoke(dialogbuilder);
				});
			CurrentAlertDialog.Show();
		}

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			View?.OnBindViewHolder(holder);
			base.OnBindViewHolder(holder);
		}
	}
}