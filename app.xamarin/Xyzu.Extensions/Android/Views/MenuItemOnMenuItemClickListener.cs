#nullable enable

using System;

namespace Android.Views
{
	public class MenuItemOnMenuItemClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
	{
		public MenuItemOnMenuItemClickListener() { }
		public MenuItemOnMenuItemClickListener(Action<IMenuItem?> onmenuitemclickaction) : this(item =>
		{
			onmenuitemclickaction.Invoke(item);

			return true;

		}) { }
		public MenuItemOnMenuItemClickListener(Func<IMenuItem?, bool> onmenuitemclickaction)
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