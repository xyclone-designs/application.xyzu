#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.App;
using AndroidX.Preference;

using System;

using Xyzu.Droid;
using Xyzu.Views.Preference;

using AndroidXListPreference = AndroidX.Preference.ListPreference;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/ListPreference")]
	public class ListPreference : AndroidXListPreference
	{
		public ListPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public ListPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public ListPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public ListPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public ListPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_preference;
			View = new PreferenceView();
		}

		int _ValueIndex;

		public AlertDialog? Dialog { get; set; }
		public PreferenceView? View { get; set; }

		void ReloadValueSummary()
		{
			if (View?.ViewValueSummary is null)
				return;

			string? valuesummary = GetEntryValues()?[_ValueIndex];

			View.ViewValueSummary.SetText(valuesummary, null);
			View.ViewValueSummary.Visibility = valuesummary is null
				? Android.Views.ViewStates.Gone
				: Android.Views.ViewStates.Visible;
		}

		protected override void OnClick()
		{
			Dialog = XyzuUtils.Dialogs.Alert(
				context: Context,
				style: Resource.Style.Xyzu_Preference_AlertDialog,
				action: (dialogbuilder, dialog) => dialogbuilder?.ProcessList(this, () => Dialog));
			Dialog.Show();
		}

		public override void SetValueIndex(int index)
		{
			base.SetValueIndex(_ValueIndex = index);

			ReloadValueSummary();
		}
		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			View?.OnBindViewHolder(holder);
			base.OnBindViewHolder(holder);

			ReloadValueSummary();
		}
	}
}