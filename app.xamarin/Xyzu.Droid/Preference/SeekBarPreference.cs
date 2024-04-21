#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;
using AndroidX.Preference;

using System;

using Xyzu.Droid;

using AndroidXSeekBarPreference = AndroidX.Preference.SeekBarPreference;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/SeekBarPreference")]
	public class SeekBarPreference : AndroidXSeekBarPreference
	{
		public SeekBarPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public SeekBarPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public SeekBarPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public SeekBarPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public SeekBarPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_seekbarpreference;
		}

		public AppCompatSeekBar? Seekbar { get; protected set; }
		public AppCompatTextView? MinTextView { get; protected set; }
		public AppCompatTextView? ValueTextView { get; protected set; }
		public AppCompatTextView? MaxTextView { get; protected set; }

		public override int Min
		{
			get => base.Min;
			set
			{
				base.Min = value;

				MinTextView?.SetText(value.ToString(), null);
			}
		}			   
		public override int Value
		{
			get => base.Value;
			set
			{
				base.Value = value;

				ValueTextView?.SetText(value.ToString(), null);
			}
		}
		public new int Max
		{
			get => base.Max;
			set
			{
				base.Max = value;

				MaxTextView?.SetText(value.ToString(), null);
			}
		}

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			Seekbar = holder.FindViewById(Resource.Id.seekbar) as AppCompatSeekBar;
			MinTextView = holder.FindViewById(Resource.Id.seekbar_min) as AppCompatTextView;
			ValueTextView = holder.FindViewById(Resource.Id.seekbar_value) as AppCompatTextView;
			MaxTextView = holder.FindViewById(Resource.Id.seekbar_max) as AppCompatTextView;

			MinTextView?.SetText(Min.ToString(), null);
			ValueTextView?.SetText(Value.ToString(), null);
			MaxTextView?.SetText(Max.ToString(), null);
		}
	}
}