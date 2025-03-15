#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.Preference;

using System;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/LicensePreference")]
	public class LicensePreference : DropdownPreference
	{
		public LicensePreference(Context context) : base(context) { }
		public LicensePreference(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public LicensePreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
		public LicensePreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }
		public LicensePreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			if (View?.ViewIcon is not null)
				View.ViewIcon.ImageTintList = null;
		}
	}
}