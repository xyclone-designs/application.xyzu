#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Preference;

using System;

using Xyzu.Droid;

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
			LayoutResource = Resource.Layout.xyzu_preference_dropdownpreference;
		}

		int _ValueIndex;

		AlertDialog? Dialog { get; set; }
		AppCompatTextView? ValueSummary { get; set; }

		void ReloadValueSummary()
		{
			if (ValueSummary is null)
				return;

			string? valuesummary = GetEntryValues()?[_ValueIndex];

			ValueSummary.SetText(valuesummary, null);
			ValueSummary.Visibility = valuesummary is null
				? Android.Views.ViewStates.Gone
				: Android.Views.ViewStates.Visible;
		}

		protected override void OnClick()
		{
			if (PreferenceUtils.StyledAlertDialog(Context, this, () => Dialog) is AlertDialog.Builder alertdialogbuilder)
			{
				Dialog = alertdialogbuilder.Create();
				Dialog.Show();
			}
		}

		public override void SetValueIndex(int index)
		{
			base.SetValueIndex(_ValueIndex = index);

			ReloadValueSummary();
		}
		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			ValueSummary = holder.FindViewById(Resource.Id.xyzu_preference_preference_valuesummary) as AppCompatTextView;

			ReloadValueSummary();
		}
	}
}