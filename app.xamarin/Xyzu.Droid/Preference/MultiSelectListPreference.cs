#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Preference;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;

using AndroidXMultiSelectListPreference = AndroidX.Preference.MultiSelectListPreference;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/MultiSelectListPreference")]
	public class MultiSelectListPreference : AndroidXMultiSelectListPreference
	{
		public MultiSelectListPreference(Context context) : base(context) 
		{
			Init(context, null);
		}
		public MultiSelectListPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public MultiSelectListPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public MultiSelectListPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public MultiSelectListPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_dropdownpreference;
		}

		public AlertDialog? Dialog { get; protected set; }
		public View? DialogView { get; protected set; }
		public AppCompatTextView? ValueSummary { get; protected set; }

		public int? NeutralButtonTextId { get; set; }
		public View.IOnClickListener? NeutralButtonOnClickListener { get; set; }

		protected override void OnClick()
		{
			// base.OnClick();

			AlertDialog.Builder? alertdialogbuilder = PreferenceUtils.StyledAlertDialog(Context, this, () => Dialog);

			if (NeutralButtonTextId.HasValue)
				alertdialogbuilder?.SetNeutralButton(NeutralButtonTextId.Value, (sender, args) => { });

			Dialog = alertdialogbuilder?.Create();
			Dialog?.Show();

			if (NeutralButtonTextId.HasValue) Dialog?
				.GetButton((int)DialogButtonType.Neutral)?
				.SetOnClickListener(NeutralButtonOnClickListener);
		}

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			ValueSummary = holder.FindViewById(Resource.Id.xyzu_preference_preference_valuesummary) as AppCompatTextView;

			SetValueSummary(null);
		}

		public override bool CallChangeListener(Java.Lang.Object? newValue)
		{
			SetValueSummary(null);

			return base.CallChangeListener(newValue);
		}

		public void SetValueSummary(string? valuesummary)
		{
			if (ValueSummary is null)
				return;

			valuesummary ??= Values?.Any() ?? false
				? null
				: string.Join(", ", Values);

			ValueSummary.SetText(valuesummary, null);
			ValueSummary.Visibility = valuesummary is null
				? Android.Views.ViewStates.Gone
				: Android.Views.ViewStates.Visible;
		}
	}
}