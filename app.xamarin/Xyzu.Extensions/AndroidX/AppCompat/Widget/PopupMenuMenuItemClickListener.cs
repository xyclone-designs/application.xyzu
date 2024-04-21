#nullable enable

using Android.Views;

using System;

namespace AndroidX.AppCompat.Widget
{
	public class PopupMenuMenuItemClickListener : Java.Lang.Object, PopupMenu.IOnMenuItemClickListener
	{
		public PopupMenuMenuItemClickListener() { }
		public PopupMenuMenuItemClickListener(Action<IMenuItem?> onmenuitemclickaction) : this(item =>
		{
			onmenuitemclickaction?.Invoke(item);

			return true; 
		}) { }
		public PopupMenuMenuItemClickListener(Func<IMenuItem?, bool> onmenuitemclickaction)
		{
			OnMenuItemClickAction = onmenuitemclickaction;
		}

		public Func<IMenuItem?, bool>? OnMenuItemClickAction { get; set; }

		public bool OnMenuItemClick(IMenuItem? item)
		{
			return OnMenuItemClickAction?.Invoke(item) ?? false;
		}
	}
}