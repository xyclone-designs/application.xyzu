
namespace Android.Views
{
	public static class IMenuExtensions
	{
		public static IMenuItem? Add(this IMenu menu, IMenuItem menuitem)
		{
			IMenuItem? newmenuitem = menu.Add(null);

			if (newmenuitem is null)
				return newmenuitem;

			newmenuitem.SetActionProvider(menuitem.ActionProvider);
			newmenuitem.SetActionView(menuitem.ActionView);
			newmenuitem.SetAlphabeticShortcut(menuitem.AlphabeticShortcut);
			newmenuitem.SetCheckable(menuitem.IsCheckable);
			newmenuitem.SetChecked(menuitem.IsChecked);
			newmenuitem.SetContentDescription(menuitem.ContentDescription);
			newmenuitem.SetEnabled(menuitem.IsEnabled);
			newmenuitem.SetIcon(menuitem.Icon);
			newmenuitem.SetIconTintList(menuitem.IconTintList);
			newmenuitem.SetIconTintMode(menuitem.IconTintMode);
			newmenuitem.SetIntent(menuitem.Intent);
			newmenuitem.SetNumericShortcut(menuitem.NumericShortcut);
			newmenuitem.SetTitle(menuitem.TitleFormatted);
			newmenuitem.SetTitleCondensed(menuitem.TitleCondensedFormatted);
			newmenuitem.SetTooltipText(menuitem.TooltipTextFormatted);
			newmenuitem.SetVisible(menuitem.IsVisible);

			return newmenuitem;
		}
		public static IMenu Add(this IMenu menu, params IMenuItem[] menuitems)
		{
			foreach (IMenuItem menuitem in menuitems)
				menu.Add(menuitem);

			return menu;
		}
	}
}