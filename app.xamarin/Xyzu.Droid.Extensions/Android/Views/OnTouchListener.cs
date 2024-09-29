#nullable enable

using System;

namespace Android.Views
{
	public class OnTouchListener : Java.Lang.Object, View.IOnTouchListener
	{
		public OnTouchListener() { }

		public Func<View?, MotionEvent?, bool>? OnTouchFunc { get; set; }


		public bool OnTouch(View? view, MotionEvent? motionevent)
		{
			return OnTouchFunc?.Invoke(view, motionevent) ?? false;
		}
	}
}