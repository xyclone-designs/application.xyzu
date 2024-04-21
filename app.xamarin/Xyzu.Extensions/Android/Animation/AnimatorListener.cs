#nullable enable

using System;

namespace Android.Animation
{
	public class AnimatorListener : Java.Lang.Object, Animator.IAnimatorListener
	{
		public Action<Animator?>? OnAnimationCancelAction { get; set; }
		public Action<Animator?>? OnAnimationEndAction { get; set; }
		public Action<Animator?>? OnAnimationRepeatAction { get; set; }
		public Action<Animator?>? OnAnimationStartAction { get; set; }

		public void OnAnimationCancel(Animator? animation) 
		{
			OnAnimationCancelAction?.Invoke(animation);
		}
		public void OnAnimationEnd(Animator? animation) 
		{
			OnAnimationEndAction?.Invoke(animation);
		}
		public void OnAnimationRepeat(Animator? animation) 
		{
			OnAnimationRepeatAction?.Invoke(animation);
		}
		public void OnAnimationStart(Animator? animation) 
		{
			OnAnimationStartAction?.Invoke(animation);
		}
	}
}