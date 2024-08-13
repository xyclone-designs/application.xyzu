#nullable enable

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
	[Register("xyzu/preference/ViewPreference")]
	public class ViewPreference : AndroidXPreference
	{
		public ViewPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public ViewPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public ViewPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public ViewPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public ViewPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void Init(Context context, IAttributeSet? atts)
		{
			LayoutResource = Resource.Layout.xyzu_preference_viewpreference;
		}

		private View? _View;
		private ContentFrameLayout? _ViewFrame;

		public View? View
		{
			get => _View;
			set
			{
				_View = value;

				if (ViewFrame is null)
					return;

				ViewFrame.RemoveAllViews();

				if (_View is null)
					ViewFrame.Visibility = ViewStates.Gone;
				else
				{
					ViewFrame.Visibility = ViewStates.Visible;
					ViewFrame.AddView(_View, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent));
				}
			}
		}
		public ContentFrameLayout? ViewFrame
		{
			get => _ViewFrame;
			set
			{
				_ViewFrame = value;
				View = _View;
			}
		}

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			ViewFrame = holder.FindViewById(Resource.Id.xyzu_preference_viewpreference_contentframelayout) as ContentFrameLayout;
		}
	}
}