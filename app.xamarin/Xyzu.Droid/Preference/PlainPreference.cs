﻿#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;			  
using AndroidX.Preference;

using System;

using Xyzu.Droid;

using AndroidXPreference = AndroidX.Preference.Preference;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/PlainPreference")]
	public class PlainPreference : AndroidXPreference
	{
		public PlainPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public PlainPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public PlainPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public PlainPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public PlainPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_preference;
		}

		protected override void OnClick()
		{
			base.OnClick();
		}

		public Action? OnClickAction { get; set; }
	}
}