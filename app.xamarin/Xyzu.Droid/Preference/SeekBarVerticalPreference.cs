#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.Preference;

using System;

using Xyzu.Droid;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/SeekBarVerticalPreference")]
	public class SeekBarVerticalPreference : SeekBarPreference
	{
		public SeekBarVerticalPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public SeekBarVerticalPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public SeekBarVerticalPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public SeekBarVerticalPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public SeekBarVerticalPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected override void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_seekbarverticalpreference;
		}
		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			Android.Widget.HorizontalScrollView horizontalscrollview = new Android.Widget.HorizontalScrollView(Context);

			AndroidX.AppCompat.Widget.LinearLayoutCompat linearlayoutcompat = new AndroidX.AppCompat.Widget.LinearLayoutCompat(Context)
			{
				Orientation = AndroidX.AppCompat.Widget.LinearLayoutCompat.Horizontal,
			};

			for (int index = 0; index < 10; index++)
				if (Android.Views.LayoutInflater.FromContext(Context)?.Inflate(Resource.Layout.xyzu_preference_seekbarverticalpreference, null, true) is Android.Views.View view)
					linearlayoutcompat.AddView(view, new AndroidX.AppCompat.Widget.LinearLayoutCompat.LayoutParams(280, AndroidX.AppCompat.Widget.LinearLayoutCompat.LayoutParams.WrapContent));

			//for (int index = 0; index < 10; index++)
			//{
			//	AndroidX.AppCompat.Widget.AppCompatSeekBar appcompatseekbar = new AndroidX.AppCompat.Widget.AppCompatSeekBar(Context)
			//	{
			//		Rotation = 270,
			//		LayoutParameters = new AndroidX.AppCompat.Widget.LinearLayoutCompat.LayoutParams(280, AndroidX.AppCompat.Widget.LinearLayoutCompat.LayoutParams.WrapContent)
			//	};

			//	linearlayoutcompat.AddView(appcompatseekbar, new AndroidX.AppCompat.Widget.LinearLayoutCompat.LayoutParams(280, AndroidX.AppCompat.Widget.LinearLayoutCompat.LayoutParams.WrapContent));
			//}

			horizontalscrollview.AddView(linearlayoutcompat);

			holder.ItemView = horizontalscrollview;
		}
	}
}