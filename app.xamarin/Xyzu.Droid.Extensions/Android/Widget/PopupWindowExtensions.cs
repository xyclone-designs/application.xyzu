#nullable enable

using System;

namespace Android.Widget
{
	public static class PopupWindowExtensions
	{
		public static void DismissAfter(this PopupWindow popupwindow, Action? action)
		{
			action?.Invoke();

			popupwindow.Dismiss();
		}	
		public static void DismissThen(this PopupWindow popupwindow, Action? action)
		{
			popupwindow.Dismiss();

			action?.Invoke();
		}
	}
}