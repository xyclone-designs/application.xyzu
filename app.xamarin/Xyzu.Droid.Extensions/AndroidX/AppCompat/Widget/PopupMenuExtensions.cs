using System;

namespace AndroidX.AppCompat.Widget
{
	public static class PopupMenuExtensions
	{
		public static void DismissAfter(this PopupMenu popupmenu, Action? action)
		{
			action?.Invoke();

			popupmenu.Dismiss();
		}	
		public static void DismissThen(this PopupMenu popupmenu, Action? action)
		{
			popupmenu.Dismiss();

			action?.Invoke();
		}
	}
}