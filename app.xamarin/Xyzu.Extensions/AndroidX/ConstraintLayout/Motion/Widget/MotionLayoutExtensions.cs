#nullable enable

namespace AndroidX.ConstraintLayout.Motion.Widget
{
	public static class MotionLayoutExtensions
	{
		public static void SetAndTransitionToEnd(this MotionLayout motionlayout, int id, int progress = 0)
		{
			motionlayout.SetTransition(id);
			motionlayout.SetInterpolatedProgress(progress);
			motionlayout.TransitionToEnd();
		}						
		public static void SetAndTransitionToStart(this MotionLayout motionlayout, int id, int progress = 1)
		{
			motionlayout.SetTransition(id);
			motionlayout.SetInterpolatedProgress(progress);
			motionlayout.TransitionToStart();
		}									 
	}
}