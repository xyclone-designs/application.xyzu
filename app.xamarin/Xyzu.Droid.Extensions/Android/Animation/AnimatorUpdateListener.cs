#nullable enable

using System;

namespace Android.Animation
{
	public class AnimatorUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
	{
		public Action<ValueAnimator?>? OnAnimationUpdateAction { get; set; }

		public void OnAnimationUpdate(ValueAnimator? animation) 
		{
			OnAnimationUpdateAction?.Invoke(animation);
		}
	}
}