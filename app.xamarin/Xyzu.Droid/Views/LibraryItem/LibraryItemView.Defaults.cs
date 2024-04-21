#nullable enable

using Android.Content;

using System;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.LibraryItem
{
	public partial class LibraryItemView
	{
		public static class Defaults
		{
			public static IImage Image(Context? context)
			{
				string uristring = string.Format(
					"{0}://{1}/{2}/{3}",
					ContentResolver.SchemeAndroidResource,
					context?.Resources?.GetResourcePackageName(Resource.Drawable.icon_xyzu),
					context?.Resources?.GetResourceTypeName(Resource.Drawable.icon_xyzu),
					context?.Resources?.GetResourceEntryName(Resource.Drawable.icon_xyzu));

				Uri? uri = Uri.TryCreate(uristring, UriKind.RelativeOrAbsolute, out Uri outuri) ? outuri : null;

				return new IImage.Default
				{
					Uri = uri
				};
			}
			public static IAlbum Album(Context? context)
			{
				return new IAlbum.Default(string.Empty)
				{
					Artist = context?.Resources?.GetString(Resource.String.library_unknown_albumartist),
					// Artwork = Image(context),
					Title = context?.Resources?.GetString(Resource.String.library_unknown_album),
				};
			}
			public static IArtist Artist(Context? context)
			{
				return new IArtist.Default(string.Empty)
				{
					// Image = Image(context),
					Name = context?.Resources?.GetString(Resource.String.library_unknown_artist),
				};
			}
			public static IGenre Genre(Context? context)
			{
				return new IGenre.Default(string.Empty)
				{
					Name = context?.Resources?.GetString(Resource.String.library_unknown_genre),
				};
			}
			public static IPlaylist Playlist(Context? context)
			{
				return new IPlaylist.Default(string.Empty)
				{
					Name = context?.Resources?.GetString(Resource.String.library_unknown_playlist),
				};
			}
			public static ISong Song(Context? context)
			{
				return new ISong.Default(string.Empty)
				{
					// Artwork = Image(context),
					Album = context?.Resources?.GetString(Resource.String.library_unknown_album),
					AlbumArtist = context?.Resources?.GetString(Resource.String.library_unknown_albumartist),
					Artist = context?.Resources?.GetString(Resource.String.library_unknown_artist),
					Genre = context?.Resources?.GetString(Resource.String.library_unknown_genre),
					Title = context?.Resources?.GetString(Resource.String.library_unknown_title),
				};
			}
		}
	}
}