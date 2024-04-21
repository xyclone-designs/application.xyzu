#nullable enable

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
		public AppCompatSeekBarVertical(Context context) : base(context)
		{
			Init(null);
		}
		public AppCompatSeekBarVertical(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(attrs);
		}
		public AppCompatSeekBarVertical(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(attrs);
		}
		public AppCompatSeekBarVertical(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		protected void Init(IAttributeSet? attrs)
		{ }

        public override int Progress
		{
            get => base.Progress;
            set
			{
                base.Progress = value;

                OnSizeChanged(Width, Height, 0, 0);
            }

        }
        public override bool OnTouchEvent(MotionEvent? @event)
        {
            if (Enabled is false)
                return false;

            switch (@event?.Action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.Move:
                case MotionEventActions.Up:
                    Progress = Max - (int)(Max * @event.GetY() / Height);
                    OnSizeChanged(Width, Height, 0, 0);
                    break;

                case MotionEventActions.Cancel:
                    break;

                default: break;
            }

            return true;
        }

        protected override void OnDraw(Canvas? c)
        {
            c?.Rotate(-90);
            c?.Translate(-Height, 0);

            base.OnDraw(c);
        }
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(heightMeasureSpec, widthMeasureSpec);
            
            SetMeasuredDimension(MeasuredHeight, MeasuredWidth);
        }
	}
}