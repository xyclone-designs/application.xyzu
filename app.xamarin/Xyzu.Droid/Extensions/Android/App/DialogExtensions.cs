#nullable enable

using Android.Views;

using System;

namespace Android.App
{
	public static class DialogExtensions
	{
		public static void SetDim(this Dialog dialog, bool dimmed)
		{
			if (dimmed)
				dialog.Window?.AddFlags(WindowManagerFlags.DimBehind);
			else
				dialog.Window?.ClearFlags(WindowManagerFlags.DimBehind);
		}
		public static void SetAllowTouchOutside(this Dialog dialog, bool allow, Func<MotionEvent?, bool>? ondispatch)
		{
			View? touchoutside = dialog.FindViewById(Xyzu.Droid.Resource.Id.touch_outside);

			touchoutside?.SetOnTouchListener(allow is false ? null : new OnTouchListener
			{
				OnTouchFunc = (view, motionevent) =>
				{
					if (view is null || motionevent is null)
						return false;

					motionevent.SetLocation(motionevent.RawX - view.GetX(), motionevent.RawY - view.GetY());

					return ondispatch?.Invoke(motionevent) ?? false;
				}
			});
		}
	}
}