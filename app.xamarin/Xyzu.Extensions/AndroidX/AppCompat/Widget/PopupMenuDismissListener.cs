#nullable enable

using System;

namespace AndroidX.AppCompat.Widget
{
	public class PopupMenuDismissListener : Java.Lang.Object, PopupMenu.IOnDismissListener
	{
		public PopupMenuDismissListener() { }
		public PopupMenuDismissListener(Action<PopupMenu?> ondismissaction)
		{
			OnDismissAction = ondismissaction;
		}

		public bool MenuOptionClicked { get; set; }
		public Action<PopupMenu?>? OnDismissAction { get; set; }

		public void OnDismiss(PopupMenu? menu)
		{
			OnDismissAction?.Invoke(menu);
		}
		public void SetMenuOptionClicked(bool menuoptionclicked)
		{
			MenuOptionClicked = menuoptionclicked;
		}
	}
}