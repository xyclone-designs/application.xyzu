#nullable enable

using System;

namespace Android.Views
{
	public class MenuItemOnActionExpandListener : Java.Lang.Object, IMenuItemOnActionExpandListener
	{
		public MenuItemOnActionExpandListener() { }

		public Func<IMenuItem?, bool>? OnMenuItemActionCollapseAction { get; set; }
		public Func<IMenuItem?, bool>? OnMenuItemActionExpandAction { get; set; }

		public bool OnMenuItemActionCollapse(IMenuItem? item)
		{
			return OnMenuItemActionCollapseAction?.Invoke(item) ?? false;
		}
		public bool OnMenuItemActionExpand(IMenuItem? item)
		{
			return OnMenuItemActionExpandAction?.Invoke(item) ?? false;
		}
	}
}