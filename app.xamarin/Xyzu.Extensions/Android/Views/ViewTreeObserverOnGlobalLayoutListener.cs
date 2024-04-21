#nullable enable

using System;

namespace Android.Views
{
	public class ViewTreeObserverOnGlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
	{
		public ViewTreeObserverOnGlobalLayoutListener() { }
		public ViewTreeObserverOnGlobalLayoutListener(Action ongloballayoutaction)
		{
			OnGlobalLayoutAction = ongloballayoutaction;
		}

		public Action? OnGlobalLayoutAction { get; set; }

		public void OnGlobalLayout()
		{
			OnGlobalLayoutAction?.Invoke();
		}
	}
}