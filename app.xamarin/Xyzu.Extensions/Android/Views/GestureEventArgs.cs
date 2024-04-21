#nullable enable

using System;
using System.Linq;

namespace Android.Views
{
	public class GestureEventArgs : EventArgs
	{
		public View? View { get; set; }
		public Gestures? Gesture { get; set; }
		public float DistanceX { get; set; }
		public float DistanceY { get; set; }
		public float VelocityX { get; set; }
		public float VelocityY { get; set; }
		public MotionEvent?[]? MotionEvents { get; set; }

		public bool HasMotionEventAction(MotionEventActions motioneventaction)
		{
			return MotionEvents?.Any(motionevent => motionevent?.Action == motioneventaction) ?? false;
		}
	}
}