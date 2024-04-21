#nullable enable

using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Settings.Enums
{
	public static class LibraryPagesExtensions
	{  
		public static int AsResoureIdTitle(this LibraryPages librarypage)
		{
			return librarypage switch
			{
				LibraryPages.Album => Resource.String.enums_librarypages_album_title,
				LibraryPages.Albums => Resource.String.enums_librarypages_albums_title,
				LibraryPages.Artist => Resource.String.enums_librarypages_artist_title,
				LibraryPages.Artists => Resource.String.enums_librarypages_artists_title,
				LibraryPages.Genre => Resource.String.enums_librarypages_genre_title,
				LibraryPages.Genres => Resource.String.enums_librarypages_genres_title,
				LibraryPages.Playlist => Resource.String.enums_librarypages_playlist_title,
				LibraryPages.Playlists => Resource.String.enums_librarypages_playlists_title,
				LibraryPages.Queue => Resource.String.enums_librarypages_queue_title,
				LibraryPages.Search => Resource.String.enums_librarypages_search_title,
				LibraryPages.Songs => Resource.String.enums_librarypages_songs_title,

				_ => throw new ArgumentException(string.Format("Invalid LibraryPages '{0}'", librarypage))
			};
		}	
		public static int AsResoureIdDescription(this LibraryPages librarypage)
		{
			return librarypage switch
			{
				LibraryPages.Album => Resource.String.enums_librarypages_album_description,
				LibraryPages.Albums => Resource.String.enums_librarypages_albums_description,
				LibraryPages.Artist => Resource.String.enums_librarypages_artist_description,
				LibraryPages.Artists => Resource.String.enums_librarypages_artists_description,
				LibraryPages.Genre => Resource.String.enums_librarypages_genre_description,
				LibraryPages.Genres => Resource.String.enums_librarypages_genres_description,
				LibraryPages.Playlist => Resource.String.enums_librarypages_playlist_description,
				LibraryPages.Playlists => Resource.String.enums_librarypages_playlists_description,
				LibraryPages.Queue => Resource.String.enums_librarypages_queue_description,
				LibraryPages.Search => Resource.String.enums_librarypages_search_description,
				LibraryPages.Songs => Resource.String.enums_librarypages_songs_description,

				_ => throw new ArgumentException(string.Format("Invalid LibraryPages '{0}'", librarypage))
			};
		}
		public static string? AsStringTitle(this LibraryPages librarypages, Context? context)
		{
			if (librarypages.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this LibraryPages librarypages, Context? context)
		{
			if (librarypages.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}