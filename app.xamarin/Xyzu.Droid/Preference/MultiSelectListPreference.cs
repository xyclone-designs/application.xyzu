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
using Xyzu.Views.Preference;

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
			LayoutResource = Resource.Layout.xyzu_preference_preference;
			View = new PreferenceView();
		}

		public AlertDialog? Dialog { get; protected set; }
		public View? DialogView { get; protected set; }
		public PreferenceView? View { get; set; }

		public int? NeutralButtonTextId { get; set; }
		public View.IOnClickListener? NeutralButtonOnClickListener { get; set; }

		protected override void OnClick()
		{
			// base.OnClick();

			Dialog = XyzuUtils.Dialogs.Alert(
				context: Context,
				style: Resource.Style.Xyzu_Preference_AlertDialog,
				action: (dialogbuilder, dialog) =>
				{
					if (dialogbuilder is null)
						return;

					dialogbuilder.ProcessListMulti(this, () => Dialog);

					if (NeutralButtonTextId.HasValue)
						dialogbuilder.SetNeutralButton(NeutralButtonTextId.Value, (sender, args) => { });
				});			
			Dialog.Show();
		}

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			View?.OnBindViewHolder(holder);
			base.OnBindViewHolder(holder);

			SetValueSummary(null);
		}
		public override bool CallChangeListener(Java.Lang.Object? newValue)
		{
			SetValueSummary(null);

			return base.CallChangeListener(newValue);
		}

		public void SetValueSummary(string? valuesummary)
		{
			if (View?.ViewValueSummary is null)
				return;

			valuesummary ??= Values?.Any() ?? false
				? null
				: string.Join(", ", Values);

			View.ViewValueSummary.SetText(valuesummary, null);
			View.ViewValueSummary.Visibility = valuesummary is null
				? ViewStates.Gone
				: ViewStates.Visible;
		}
	}
}