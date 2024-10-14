using System;
using System.Collections.Generic;

namespace Android.Views
{
	public static class ViewPropertyAnimatorExtensions
	{
		public static void Start(this IEnumerable<ViewPropertyAnimator> viewpropertyanimators, Action<ViewPropertyAnimator>? beforestart = null)
		{
			foreach (ViewPropertyAnimator viewpropertyanimator in viewpropertyanimators)
			{
				beforestart?.Invoke(viewpropertyanimator);
				viewpropertyanimator.Start();
			}
		}
	}
}