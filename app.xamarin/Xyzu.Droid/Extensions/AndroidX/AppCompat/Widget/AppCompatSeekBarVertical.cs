using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;

using System;

namespace AndroidX.AppCompat.Widget
{
	[Register("androidx/appcompat/widget/AppCompatSeekBarVertical")]
	public class AppCompatSeekBarVertical : AppCompatSeekBar
	{
		public AppCompatSeekBarVertical(Context context) : this(context, null) { }
		public AppCompatSeekBarVertical(Context context, IAttributeSet? attrs) : this(context, attrs, default) { }
		public AppCompatSeekBarVertical(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetBackgroundColor(Color.Gray);
		}

		//public override int Progress
		//{
		//	get => base.Progress;
		//	set
		//	{
		//		base.Progress = value;

		//		//OnSizeChanged(Width, Height, 0, 0);
		//	}

		//}
		public new ViewGroup.LayoutParams? LayoutParameters
		{
			get => base.LayoutParameters;
			set
			{
				if (value is not null)
					(value.Width, value.Height) = (value.Height, value.Width);

				base.LayoutParameters = value;
			}
		}

		protected override void OnDraw(Canvas c)
		{
			//c.Rotate(270);
			//c.Translate(-Height, 0);

			base.OnDraw(c);

		}
		//protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		//{
		//	base.OnMeasure(heightMeasureSpec, widthMeasureSpec);

		//	SetMeasuredDimension(MeasuredHeight, MeasuredWidth);
		//}


		//      public override bool OnTouchEvent(MotionEvent? @event)
		//      {
		//          if (Enabled is false)
		//              return false;

		//          switch (@event?.Action)
		//          {
		//              case MotionEventActions.Down:
		//              case MotionEventActions.Move:
		//              case MotionEventActions.Up:
		//                  Progress = Max - (int)(Max * @event.GetY() / Height);
		//                  OnSizeChanged(Width, Height, 0, 0);
		//                  break;

		//              case MotionEventActions.Cancel:
		//                  break;

		//              default: break;
		//          }

		//          return true;
		//      }


	}
}