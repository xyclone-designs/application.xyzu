using System;

namespace Android.Views.Animations
{
	public class AnimationListener : Java.Lang.Object, Animation.IAnimationListener
	{
		public Action<Animation?>? OnAnimationCancelAction { get; set; }
		public Action<Animation?>? OnAnimationEndAction { get; set; }
		public Action<Animation?>? OnAnimationRepeatAction { get; set; }
		public Action<Animation?>? OnAnimationStartAction { get; set; }

		public void OnAnimationEnd(Animation? animation) 
		{
			OnAnimationEndAction?.Invoke(animation);
		}
		public void OnAnimationRepeat(Animation? animation) 
		{
			OnAnimationRepeatAction?.Invoke(animation);
		}
		public void OnAnimationStart(Animation? animation) 
		{
			OnAnimationStartAction?.Invoke(animation);
		}
	}
}