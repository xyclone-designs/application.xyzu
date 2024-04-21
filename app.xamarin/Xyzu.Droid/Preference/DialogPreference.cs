#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.App;

using System;

using Xyzu.Droid;

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
		}

		public AlertDialog? CurrentAlertDialog { get; protected set; }

		public Action<AlertDialog.Builder>? DialogOnBuild { get; set; }

		public void DialogShow()
		{
			OnClick();
		}
		protected override void OnClick()
		{
			// base.OnClick();

			if (PreferenceUtils.StyledAlertDialog(Context, this) is AlertDialog.Builder alertdialogbuilder)
			{
				DialogOnBuild?.Invoke(alertdialogbuilder);

				CurrentAlertDialog = alertdialogbuilder.Create();
				CurrentAlertDialog.Show();
			}
		}
	}
}