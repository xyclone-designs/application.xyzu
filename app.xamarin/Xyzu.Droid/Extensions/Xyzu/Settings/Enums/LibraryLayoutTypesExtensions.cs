#nullable enable

using Android.Content;
using Android.Views;

using System;
using System.Collections.Generic;

using Xyzu.Droid;

namespace Xyzu.Settings.Enums
{
	public static class LibraryLayoutTypesExtensions
	{  
		public static bool IsGrid(this LibraryLayoutTypes librarylayouttypes)
		{
			return
				librarylayouttypes == LibraryLayoutTypes.GridSmall ||
				librarylayouttypes == LibraryLayoutTypes.GridMedium ||
				librarylayouttypes == LibraryLayoutTypes.GridLarge;
		}														   
		public static bool IsList(this LibraryLayoutTypes librarylayouttypes)
		{
			return
				librarylayouttypes == LibraryLayoutTypes.ListSmall ||
				librarylayouttypes == LibraryLayoutTypes.ListMedium ||
				librarylayouttypes == LibraryLayoutTypes.ListLarge;
		}

		public static int AsResoureIdTitle(this LibraryLayoutTypes librarylayouttype)
		{
			return librarylayouttype switch
			{
				LibraryLayoutTypes.GridLarge => Resource.String.enums_librarylayouttypes_gridlarge_title,
				LibraryLayoutTypes.GridMedium => Resource.String.enums_librarylayouttypes_gridmedium_title,
				LibraryLayoutTypes.GridSmall => Resource.String.enums_librarylayouttypes_gridsmall_title,
				LibraryLayoutTypes.ListLarge => Resource.String.enums_librarylayouttypes_listlarge_title,
				LibraryLayoutTypes.ListMedium => Resource.String.enums_librarylayouttypes_listmedium_title,
				LibraryLayoutTypes.ListSmall => Resource.String.enums_librarylayouttypes_listsmall_title,

				_ => throw new ArgumentException(string.Format("Invalid LibraryLayoutTypes '{0}'", librarylayouttype))
			};
		}	
		public static int AsResoureIdDescription(this LibraryLayoutTypes librarylayouttype)
		{
			return librarylayouttype switch
			{
				LibraryLayoutTypes.GridLarge => Resource.String.enums_librarylayouttypes_gridlarge_description,
				LibraryLayoutTypes.GridMedium => Resource.String.enums_librarylayouttypes_gridmedium_description,
				LibraryLayoutTypes.GridSmall => Resource.String.enums_librarylayouttypes_gridsmall_description,
				LibraryLayoutTypes.ListLarge => Resource.String.enums_librarylayouttypes_listlarge_description,
				LibraryLayoutTypes.ListMedium => Resource.String.enums_librarylayouttypes_listmedium_description,
				LibraryLayoutTypes.ListSmall => Resource.String.enums_librarylayouttypes_listsmall_description,

				_ => throw new ArgumentException(string.Format("Invalid LibraryLayoutTypes '{0}'", librarylayouttype))
			};
		}
		public static string? AsStringTitle(this LibraryLayoutTypes librarylayouttypes, Context? context)
		{
			if (librarylayouttypes.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this LibraryLayoutTypes librarylayouttypes, Context? context)
		{
			if (librarylayouttypes.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}

		public static IMenu AddLibraryLayoutType(this IMenu menu, LibraryLayoutTypes librarylayouttype, Context? context, out IMenuItem? menuitem)
		{
			menuitem = null;

			int? titleres = librarylayouttype.AsResoureIdTitle();

			if (titleres is null)
				return menu;

			menuitem = menu.Add(0, (int)librarylayouttype, IMenu.None, titleres.Value);

			return menu;
		}
		public static IMenu AddLibraryLayoutTypes(this IMenu menu, Context? context, Action<IMenuItem?>? onadd = null, params LibraryLayoutTypes[] librarylayouttypes)
		{
			foreach (LibraryLayoutTypes librarylayouttype in librarylayouttypes)
			{
				menu.AddLibraryLayoutType(librarylayouttype, context, out IMenuItem? menuitem);

				onadd?.Invoke(menuitem);
			}

			return menu;
		}
		public static IMenu AddLibraryLayoutTypes(this IMenu menu, Context? context, Action<IMenuItem?>? onadd = null, IEnumerable<LibraryLayoutTypes>? librarylayouttypes = null)
		{
			if (librarylayouttypes is null)
				return menu;

			foreach (LibraryLayoutTypes librarylayouttype in librarylayouttypes)
			{
				menu.AddLibraryLayoutType(librarylayouttype, context, out IMenuItem? menuitem);

				onadd?.Invoke(menuitem);
			}

			return menu;
		}
		public static IMenu AddLibraryLayoutTypes(
			this IMenu menu,
			Context? context,
			LibraryLayoutTypes checkedlibrarylayouttype,
			IEnumerable<LibraryLayoutTypes> librarylayouttypes,
			Func<LibraryLayoutTypes> getpreviouslibrarylayouttype,
			Action<LibraryLayoutTypes> nextlibrarylayouttypechosen,
			out IMenuItemOnMenuItemClickListener? menuitemonmenuitemclicklistener)
		{
			menuitemonmenuitemclicklistener = null;

			if (librarylayouttypes is null)
				return menu;

			foreach (LibraryLayoutTypes librarylayouttype in librarylayouttypes)
			{
				menu.AddLibraryLayoutType(librarylayouttype, context, out IMenuItem? menuitem);
				menuitem?
					.SetCheckable(true)?
					.SetChecked(librarylayouttype == checkedlibrarylayouttype)?
					.SetOnMenuItemClickListener(menuitemonmenuitemclicklistener ??= new MenuItemOnMenuItemClickListener
					{
						OnMenuItemClickAction = item =>
						{
							if (!((LibraryLayoutTypes?)item?.ItemId is LibraryLayoutTypes nextlibrarylayouttype))
								return false;

							LibraryLayoutTypes previouslibrarylayouttype = getpreviouslibrarylayouttype.Invoke();

							if (previouslibrarylayouttype == nextlibrarylayouttype)
								return true;

							IMenuItem? nextmenuItem = menu.FindItem((int)nextlibrarylayouttype);
							IMenuItem? previousmenuItem = menu.FindItem((int)previouslibrarylayouttype);

							nextmenuItem?.SetChecked(true);
							previousmenuItem?.SetChecked(false);

							nextlibrarylayouttypechosen.Invoke(nextlibrarylayouttype);

							return true;
						},
					});
			}

			return menu;
		}
	}
}