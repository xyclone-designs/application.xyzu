#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;
using AndroidX.Preference;

using System;

using Xyzu.Droid;
using Xyzu.Views.Preference;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/SwitchPreference")]
	public class SwitchPreference : SwitchPreferenceCompat
	{
		public SwitchPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public SwitchPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public SwitchPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public SwitchPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public SwitchPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_preference;
			View = new PreferenceView();
		}

		public PreferenceView? View { get; protected set; }

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			View?.OnBindViewHolder(holder);
			View?.SetIsSwitch(true, _ => OnClick());
		}
	}
}