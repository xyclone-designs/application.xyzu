#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidXPreference = AndroidX.Preference.Preference;

using System;

using Xyzu.Droid;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/NavigationBarInsetPreference")] 
	public class NavigationBarInsetPreference : AndroidXPreference
	{
		public NavigationBarInsetPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public NavigationBarInsetPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public NavigationBarInsetPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public NavigationBarInsetPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public NavigationBarInsetPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_navigationbarinsetpreference;
		}
	}
}