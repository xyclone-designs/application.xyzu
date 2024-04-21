#nullable enable

using Android.Content;
using Android.Views;

using System;
using System.Collections.Generic;

using Xyzu.Droid;

namespace Xyzu.Library.Enums
{
	public static class ModelSortKeysExtensions
	{  
		public static int AsResoureIdTitle(this ModelSortKeys modelsortkey)
		{
			return modelsortkey switch
			{
				ModelSortKeys.Album => Resource.String.enums_modelsortkeys_album_title,
				ModelSortKeys.Albums => Resource.String.enums_modelsortkeys_albums_title,
				ModelSortKeys.AlbumArtist => Resource.String.enums_modelsortkeys_albumartist_title,
				ModelSortKeys.Artist => Resource.String.enums_modelsortkeys_artist_title,
				ModelSortKeys.Bitrate => Resource.String.enums_modelsortkeys_bitrate_title,
				ModelSortKeys.DateAdded => Resource.String.enums_modelsortkeys_dateadded_title,
				ModelSortKeys.DateCreated => Resource.String.enums_modelsortkeys_datecreated_title,
				ModelSortKeys.DateModified => Resource.String.enums_modelsortkeys_datemodified_title,
				ModelSortKeys.DiscNumber => Resource.String.enums_modelsortkeys_discnumber_title,
				ModelSortKeys.Discs => Resource.String.enums_modelsortkeys_discs_title,
				ModelSortKeys.Duration => Resource.String.enums_modelsortkeys_duration_title,
				ModelSortKeys.Genre => Resource.String.enums_modelsortkeys_genre_title,
				ModelSortKeys.Name => Resource.String.enums_modelsortkeys_name_title,
				ModelSortKeys.Playlist => Resource.String.enums_modelsortkeys_playlist_title,
				ModelSortKeys.Position => Resource.String.enums_modelsortkeys_position_title,
				ModelSortKeys.Rating => Resource.String.enums_modelsortkeys_rating_title,
				ModelSortKeys.ReleaseDate => Resource.String.enums_modelsortkeys_releasedate_title,
				ModelSortKeys.Size => Resource.String.enums_modelsortkeys_size_title,
				ModelSortKeys.Songs => Resource.String.enums_modelsortkeys_songs_title,
				ModelSortKeys.Title => Resource.String.enums_modelsortkeys_title_title,
				ModelSortKeys.TrackNumber => Resource.String.enums_modelsortkeys_tracknumber_title,

				_ => throw new ArgumentException(string.Format("Invalid ModelSortKeys '{0}'", modelsortkey))
			};
		}	
		public static int AsResoureIdDescription(this ModelSortKeys modelsortkey)
		{
			return modelsortkey switch
			{
				ModelSortKeys.Album => Resource.String.enums_modelsortkeys_album_description,
				ModelSortKeys.Albums => Resource.String.enums_modelsortkeys_albums_description,
				ModelSortKeys.AlbumArtist => Resource.String.enums_modelsortkeys_albumartist_description,
				ModelSortKeys.Artist => Resource.String.enums_modelsortkeys_artist_description,
				ModelSortKeys.Bitrate => Resource.String.enums_modelsortkeys_bitrate_description,
				ModelSortKeys.DateAdded => Resource.String.enums_modelsortkeys_dateadded_description,
				ModelSortKeys.DateCreated => Resource.String.enums_modelsortkeys_datecreated_description,
				ModelSortKeys.DateModified => Resource.String.enums_modelsortkeys_datemodified_description,
				ModelSortKeys.DiscNumber => Resource.String.enums_modelsortkeys_discnumber_description,
				ModelSortKeys.Discs => Resource.String.enums_modelsortkeys_discs_description,
				ModelSortKeys.Duration => Resource.String.enums_modelsortkeys_duration_description,
				ModelSortKeys.Genre => Resource.String.enums_modelsortkeys_genre_description,
				ModelSortKeys.Name => Resource.String.enums_modelsortkeys_name_description,
				ModelSortKeys.Playlist => Resource.String.enums_modelsortkeys_playlist_description,
				ModelSortKeys.Position => Resource.String.enums_modelsortkeys_position_description,
				ModelSortKeys.Rating => Resource.String.enums_modelsortkeys_rating_description,
				ModelSortKeys.ReleaseDate => Resource.String.enums_modelsortkeys_releasedate_description,
				ModelSortKeys.Size => Resource.String.enums_modelsortkeys_size_description,
				ModelSortKeys.Songs => Resource.String.enums_modelsortkeys_songs_description,
				ModelSortKeys.Title => Resource.String.enums_modelsortkeys_title_description,
				ModelSortKeys.TrackNumber => Resource.String.enums_modelsortkeys_tracknumber_description,

				_ => throw new ArgumentException(string.Format("Invalid ModelSortKeys '{0}'", modelsortkey))
			};
		}
		public static string? AsStringTitle(this ModelSortKeys modelsortkey, Context? context)
		{
			if (modelsortkey.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this ModelSortKeys modelsortkey, Context? context)
		{
			if (modelsortkey.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}

		public static IMenu AddModelSortKey(this IMenu menu, ModelSortKeys modelsortkey, Context? context, out IMenuItem? menuitem)
		{
			menuitem = null;

			int? titleres = modelsortkey.AsResoureIdTitle();

			if (titleres is null)
				return menu;

			menuitem = menu.Add(0, (int)modelsortkey, IMenu.None, titleres.Value);

			return menu;
		}
		public static IMenu AddModelSortKeys(this IMenu menu, Context? context, Action<IMenuItem?>? onadd = null, params ModelSortKeys[] modelsortkeys)
		{
			foreach (ModelSortKeys modelsortkey in modelsortkeys)
			{
				menu.AddModelSortKey(modelsortkey, context, out IMenuItem? menuitem);

				onadd?.Invoke(menuitem);
			}

			return menu;
		}
		public static IMenu AddModelSortKeys(this IMenu menu, Context? context, Action<IMenuItem?>? onadd = null, IEnumerable<ModelSortKeys>? modelsortkeys = null)
		{
			if (modelsortkeys is null)
				return menu;

			foreach (ModelSortKeys modelsortkey in modelsortkeys)
			{
				menu.AddModelSortKey(modelsortkey, context, out IMenuItem? menuitem);

				onadd?.Invoke(menuitem);
			}

			return menu;
		}
		public static IMenu AddModelSortKeys(
			this IMenu menu, 
			Context? context,
			ModelSortKeys checkedmodelsortkey,
			IEnumerable<ModelSortKeys> modelsortkeys,
			Func<ModelSortKeys> getpreviousmodelsortkey, 
			Action<ModelSortKeys> nextmodelsortkeychosen,   
			out IMenuItemOnMenuItemClickListener? menuitemonmenuitemclicklistener)
		{
			menuitemonmenuitemclicklistener = null;

			if (modelsortkeys is null)
				return menu;

			foreach (ModelSortKeys modelsortkey in modelsortkeys)
			{
				menu.AddModelSortKey(modelsortkey, context, out IMenuItem? menuitem);
				menuitem?
					.SetCheckable(true)?
					.SetChecked(modelsortkey == checkedmodelsortkey)?
					.SetOnMenuItemClickListener(menuitemonmenuitemclicklistener ??= new MenuItemOnMenuItemClickListener
					{
						OnMenuItemClickAction = item =>
						{
							if (!((ModelSortKeys?)item?.ItemId is ModelSortKeys nextmodelsortkey))
								return false;

							ModelSortKeys previousmodelsortkey = getpreviousmodelsortkey.Invoke();

							if (previousmodelsortkey == nextmodelsortkey)
								return true;

							IMenuItem? nextmenuItem = menu.FindItem((int)nextmodelsortkey);
							IMenuItem? previousmenuItem = menu.FindItem((int)previousmodelsortkey);

							nextmenuItem?.SetChecked(true);
							previousmenuItem?.SetChecked(false);

							nextmodelsortkeychosen.Invoke(nextmodelsortkey);

							return true;
						},
					});
			}

			return menu;
		}
	}
}