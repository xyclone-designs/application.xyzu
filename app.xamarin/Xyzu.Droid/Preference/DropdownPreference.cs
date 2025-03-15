#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.Preference;

using System;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/DropdownPreference")]
	public class DropdownPreference : PlainPreference
	{
		public DropdownPreference(Context context) : base(context) { }
		public DropdownPreference(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public DropdownPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
		public DropdownPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }
		public DropdownPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected override void OnClick()
		{
			base.OnClick();

			if (View is not null)
				View.Expanded = !View.Expanded;
		}

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			View?.SetIsDropDown(true, _ => OnClick());
		}
	}
}