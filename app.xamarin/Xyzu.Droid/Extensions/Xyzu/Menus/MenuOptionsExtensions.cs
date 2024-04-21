#nullable enable

using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;
using System.Collections.Generic;

using Xyzu.Droid;

namespace Xyzu.Menus
{
	public static class MenuOptionsExtensions
	{
		public static PopupMenu AsPopupMenu(
			this IEnumerable<MenuOptions> menuoptions,
			Context? context,
			View anchor,
			PopupMenu.IOnMenuItemClickListener? menuitemclicklistener = null,
			PopupMenu.IOnDismissListener? ondismisslistener = null)
		{
			return MenuOptionsUtils.CreatePopupMenu(
				anchor: anchor,
				context: context,
				menuoptions: menuoptions,
				ondismisslistener: ondismisslistener,
				menuitemclicklistener: menuitemclicklistener);
		}

		public static int AsResoureIdDrawable(this MenuOptions menuoption)
		{
			return menuoption switch
			{
				MenuOptions.AddToPlaylist => Resource.Drawable.icon_menuoptions_addtoplaylist,
				MenuOptions.AddToQueue => Resource.Drawable.icon_menuoptions_addtoqueue,
				MenuOptions.AudioEffects => Resource.Drawable.icon_menuoptions_audioeffects,
				MenuOptions.Back => Resource.Drawable.icon_menuoptions_back,
				MenuOptions.Cancel => Resource.Drawable.icon_menuoptions_cancel,
				MenuOptions.Delete => Resource.Drawable.icon_menuoptions_delete,
				MenuOptions.EditInfo => Resource.Drawable.icon_menuoptions_editinfo,
				MenuOptions.GoToAlbum => Resource.Drawable.icon_menuoptions_gotoalbum,
				MenuOptions.GoToArtist => Resource.Drawable.icon_menuoptions_gotoartist,
				MenuOptions.GoToGenre => Resource.Drawable.icon_menuoptions_gotogenre,
				MenuOptions.GoToNowPlaying => Resource.Drawable.icon_menuoptions_gotonowplaying,
				MenuOptions.GoToPlaylist => Resource.Drawable.icon_menuoptions_gotoplaylist,
				MenuOptions.GoToQueue => Resource.Drawable.icon_menuoptions_gotoqueue,
				MenuOptions.ListOptions => Resource.Drawable.icon_menuoptions_listoptions,
				MenuOptions.MoveDown => Resource.Drawable.icon_menuoptions_movedown,
				MenuOptions.MoveToBottom => Resource.Drawable.icon_menuoptions_movetobottom,
				MenuOptions.MoveToTop => Resource.Drawable.icon_menuoptions_movetotop,
				MenuOptions.MoveUp => Resource.Drawable.icon_menuoptions_moveup,
				MenuOptions.Options => Resource.Drawable.icon_menuoptions_options,
				MenuOptions.Play => Resource.Drawable.icon_menuoptions_play,
				MenuOptions.Remove => Resource.Drawable.icon_menuoptions_remove,
				MenuOptions.Rescan => Resource.Drawable.icon_menuoptions_rescan,
				MenuOptions.Search => Resource.Drawable.icon_menuoptions_search,
				MenuOptions.Select => Resource.Drawable.icon_menuoptions_select,
				MenuOptions.Settings => Resource.Drawable.icon_menuoptions_settings,
				MenuOptions.Share => Resource.Drawable.icon_menuoptions_share,
				MenuOptions.ViewInfo => Resource.Drawable.icon_menuoptions_viewinfo,

				_ => throw new ArgumentException(string.Format("Invalid MenuOptions '{0}'", menuoption))
			};
		}
		public static int AsResoureIdTitle(this MenuOptions menuoption)
		{
			return menuoption switch
			{
				MenuOptions.AddToPlaylist => Resource.String.enums_menuoptions_addtoplaylist_title,
				MenuOptions.AddToQueue => Resource.String.enums_menuoptions_addtoqueue_title,
				MenuOptions.AudioEffects => Resource.String.enums_menuoptions_audioeffects_title,
				MenuOptions.Back => Resource.String.enums_menuoptions_back_title,
				MenuOptions.Cancel => Resource.String.enums_menuoptions_cancel_title,
				MenuOptions.Delete => Resource.String.enums_menuoptions_delete_title,
				MenuOptions.EditInfo => Resource.String.enums_menuoptions_editinfo_title,
				MenuOptions.GoToAlbum => Resource.String.enums_menuoptions_gotoalbum_title,
				MenuOptions.GoToArtist => Resource.String.enums_menuoptions_gotoartist_title,
				MenuOptions.GoToGenre => Resource.String.enums_menuoptions_gotogenre_title,
				MenuOptions.GoToNowPlaying => Resource.String.enums_menuoptions_gotonowplaying_title,
				MenuOptions.GoToPlaylist => Resource.String.enums_menuoptions_gotoplaylist_title,
				MenuOptions.GoToQueue => Resource.String.enums_menuoptions_gotoqueue_title,
				MenuOptions.ListOptions => Resource.String.enums_menuoptions_listoptions_title,
				MenuOptions.MoveDown => Resource.String.enums_menuoptions_movedown_title,
				MenuOptions.MoveToBottom => Resource.String.enums_menuoptions_movetobottom_title,
				MenuOptions.MoveToTop => Resource.String.enums_menuoptions_movetotop_title,
				MenuOptions.MoveUp => Resource.String.enums_menuoptions_moveup_title,
				MenuOptions.Options => Resource.String.enums_menuoptions_options_title,
				MenuOptions.Play => Resource.String.enums_menuoptions_play_title,
				MenuOptions.Remove => Resource.String.enums_menuoptions_remove_title,
				MenuOptions.Rescan => Resource.String.enums_menuoptions_rescan_title,
				MenuOptions.Search => Resource.String.enums_menuoptions_search_title,
				MenuOptions.Select => Resource.String.enums_menuoptions_select_title,
				MenuOptions.Settings => Resource.String.enums_menuoptions_settings_title,
				MenuOptions.Share => Resource.String.enums_menuoptions_share_title,
				MenuOptions.ViewInfo => Resource.String.enums_menuoptions_viewinfo_title,

				_ => throw new ArgumentException(string.Format("Invalid MenuOptions '{0}'", menuoption))
			};
		}	
		public static int AsResoureIdDescription(this MenuOptions menuoption)
		{
			return menuoption switch
			{
				MenuOptions.AddToPlaylist => Resource.String.enums_menuoptions_addtoplaylist_description,
				MenuOptions.AddToQueue => Resource.String.enums_menuoptions_addtoqueue_description,
				MenuOptions.AudioEffects => Resource.String.enums_menuoptions_audioeffects_description,
				MenuOptions.Back => Resource.String.enums_menuoptions_back_description,
				MenuOptions.Cancel => Resource.String.enums_menuoptions_cancel_description,
				MenuOptions.Delete => Resource.String.enums_menuoptions_delete_description,
				MenuOptions.EditInfo => Resource.String.enums_menuoptions_editinfo_description,
				MenuOptions.GoToAlbum => Resource.String.enums_menuoptions_gotoalbum_description,
				MenuOptions.GoToArtist => Resource.String.enums_menuoptions_gotoartist_description,
				MenuOptions.GoToGenre => Resource.String.enums_menuoptions_gotogenre_description,
				MenuOptions.GoToNowPlaying => Resource.String.enums_menuoptions_gotonowplaying_description,
				MenuOptions.GoToPlaylist => Resource.String.enums_menuoptions_gotoplaylist_description,
				MenuOptions.GoToQueue => Resource.String.enums_menuoptions_gotoqueue_description,
				MenuOptions.ListOptions => Resource.String.enums_menuoptions_listoptions_description,
				MenuOptions.MoveDown => Resource.String.enums_menuoptions_movedown_description,
				MenuOptions.MoveToBottom => Resource.String.enums_menuoptions_movetobottom_description,
				MenuOptions.MoveToTop => Resource.String.enums_menuoptions_movetotop_description,
				MenuOptions.MoveUp => Resource.String.enums_menuoptions_moveup_description,
				MenuOptions.Options => Resource.String.enums_menuoptions_options_description,
				MenuOptions.Play => Resource.String.enums_menuoptions_play_description,
				MenuOptions.Remove => Resource.String.enums_menuoptions_remove_description,
				MenuOptions.Rescan => Resource.String.enums_menuoptions_rescan_description,
				MenuOptions.Search => Resource.String.enums_menuoptions_search_description,
				MenuOptions.Select => Resource.String.enums_menuoptions_select_description,
				MenuOptions.Settings => Resource.String.enums_menuoptions_settings_description,
				MenuOptions.Share => Resource.String.enums_menuoptions_share_description,
				MenuOptions.ViewInfo => Resource.String.enums_menuoptions_viewinfo_description,

				_ => throw new ArgumentException(string.Format("Invalid MenuOptions '{0}'", menuoption))
			};
		}
		public static Drawable? AsDrawable(this MenuOptions menuoptions, Context? context)
		{
			if (menuoptions.AsResoureIdDrawable() is int resourcedrawable)
				return context?.Resources?.GetDrawable(resourcedrawable, context.Theme);

			return null;
		}		  
		public static string? AsStringTitle(this MenuOptions menuoptions, Context? context)
		{
			if (menuoptions.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this MenuOptions menuoptions, Context? context)
		{
			if (menuoptions.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}

		public static IMenu AddMenuOption(this IMenu menu, MenuOptions menuoption, Context? context, out IMenuItem? menuitem)
		{
			menuitem = null;

			int? titleres = menuoption.AsResoureIdTitle();
			Drawable? icon = menuoption.AsDrawable(context);

			if (titleres is null)
				return menu;

			menuitem = menu.Add(0, (int)menuoption, IMenu.None, titleres.Value)?
				.SetIcon(icon);

			return menu;
		}
		public static IMenu AddMenuOptions(this IMenu menu, Context? context, Action<IMenuItem?>? onadd = null, params MenuOptions[] menuoptions)
		{
			foreach (MenuOptions menuoption in menuoptions)
			{
				menu.AddMenuOption(menuoption, context, out IMenuItem? menuitem);

				onadd?.Invoke(menuitem);
			}

			return menu;
		}
		public static IMenu AddMenuOptions(this IMenu menu, Context? context, Action<IMenuItem?>? onadd = null, IEnumerable<MenuOptions>? menuoptions = null)
		{
			if (menuoptions is null)
				return menu;

			foreach (MenuOptions menuoption in menuoptions)
			{
				menu.AddMenuOption(menuoption, context, out IMenuItem? menuitem);

				onadd?.Invoke(menuitem);
			}

			return menu;
		}
		public static IMenu AddSubMenuOption(this IMenu menu, MenuOptions menuoption, Context? context, bool withback, out ISubMenu? submenu)
		{
			submenu = null;

			if (withback)
			{
				menu.AddSubMenuOption(menuoption, context, null, out ISubMenu? provisionalsubmenu);

				submenu = provisionalsubmenu;

				return menu;
			}

			int? titleres = menuoption.AsResoureIdTitle();
			Drawable? icon = menuoption.AsDrawable(context);

			if (titleres is null)
				return menu;

			submenu = menu.AddSubMenu(0, (int)menuoption, IMenu.None, titleres.Value)?
				.SetIcon(icon)?
				.SetHeaderIcon(icon);

			return menu;
		}
		public static IMenu AddSubMenuOption(this IMenu menu, MenuOptions menuoption, Context? context, IMenuItemOnMenuItemClickListener? backmenuitemclicklistener, out ISubMenu? submenu)
		{
			submenu = null;

			menu.AddSubMenuOption(menuoption, context, false, out ISubMenu? provisionalsubmenu);

			if (provisionalsubmenu is null)
				return menu;

			provisionalsubmenu.ClearHeader();
			provisionalsubmenu.AddMenuOption(MenuOptions.Back, context, out IMenuItem? layoutsubmenubackmenuitem);
			provisionalsubmenu.Item?.SetOnMenuItemClickListener(new MenuItemOnMenuItemClickListener
			{
				OnMenuItemClickAction = item =>
				{
					item?.ExpandActionView();

					return true;
				}
			});

			layoutsubmenubackmenuitem?.SetOnMenuItemClickListener(backmenuitemclicklistener ?? new MenuItemOnMenuItemClickListener
			{
				OnMenuItemClickAction = item =>
				{
					provisionalsubmenu?.Item?.CollapseActionView();

					return true;
				},
			});

			submenu = provisionalsubmenu;

			return menu;
		}
	}
}