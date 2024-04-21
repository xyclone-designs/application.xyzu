#nullable enable

using System;

using Xyzu.Library.Models;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		public static void GoToAlbum(VariableContainer variables)
		{
			switch (true)
			{
				case true when variables.Song != null && variables.Album is null:
					GoToAlbum(variables.Song, variables.LibraryNavigatable?.OnNavigateAlbum);
					break;

				default:
					GoToAlbum(variables.Album, variables.LibraryNavigatable?.OnNavigateAlbum);
					break;
			}
		}
		public static void GoToAlbum(IAlbum? album, Action<IAlbum?>? navigate = null)
		{
			navigate?.Invoke(album);
		}
		public static void GoToAlbum(ISong? albumsong, Action<IAlbum?>? navigate = null)
		{
			string? albumid = XyzuLibrary.Instance.AlbumTitleToId(albumsong?.Album, albumsong?.AlbumArtist);
			IAlbum? libraryalbum = albumid is null ? null : new IAlbum.Default(albumid)
			{
				Artist = albumsong?.AlbumArtist,
				Title = albumsong?.Album,
			};

			navigate?.Invoke(libraryalbum);
		}
		public static void GoToArtist(VariableContainer variables)
		{
			switch (true)
			{
				case true when variables.Song != null && variables.Artist is null:
					GoToArtist(variables.Song, variables.LibraryNavigatable?.OnNavigateArtist);
					break;
				case true when variables.Album != null && variables.Artist is null:
					GoToArtist(variables.Album, variables.LibraryNavigatable?.OnNavigateArtist);
					break;

				default:
					GoToArtist(variables.Artist, variables.LibraryNavigatable?.OnNavigateArtist);
					break;
			}
		}
		public static void GoToArtist(IArtist? artist, Action<IArtist?>? navigate = null)
		{
			navigate?.Invoke(artist);
		}
		public static void GoToArtist(IAlbum? artistalbum, Action<IArtist?>? navigate = null)
		{
			string? artistid = XyzuLibrary.Instance.ArtistNameToId(artistalbum?.Artist);
			IArtist? libraryartist = artistid is null ? null : new IArtist.Default(artistid)
			{
				Name = artistalbum?.Artist,
			};

			navigate?.Invoke(libraryartist);
		}
		public static void GoToArtist(ISong? artistsong, Action<IArtist?>? navigate = null)
		{
			string? artistid = XyzuLibrary.Instance.ArtistNameToId(artistsong?.Artist);
			IArtist? libraryartist = artistid is null ? null : new IArtist.Default(artistid)
			{
				Name = artistsong?.Artist,
			};

			navigate?.Invoke(libraryartist);
		}
		public static void GoToGenre(VariableContainer variables)
		{
			switch (true)
			{
				case true when variables.Song != null && variables.Genre is null:
					GoToGenre(variables.Song, variables.LibraryNavigatable?.OnNavigateGenre);
					break;

				default:
					GoToGenre(variables.Genre, variables.LibraryNavigatable?.OnNavigateGenre);
					break;
			}
		}
		public static void GoToGenre(IGenre? genre, Action<IGenre?>? navigate = null)
		{
			navigate?.Invoke(genre);
		}
		public static void GoToGenre(ISong? genresong, Action<IGenre?>? navigate = null)
		{
			string? genreid = XyzuLibrary.Instance.GenreNameToId(genresong?.Genre);
			IGenre? librarygenre = genreid is null ? null : new IGenre.Default(genreid)
			{
				Name = genresong?.Genre,
			};

			navigate?.Invoke(librarygenre);
		}
		public static void GoToQueue(VariableContainer variables)
		{
			GoToQueue(variables.LibraryNavigatable?.OnNavigateQueue);
		}
		public static void GoToQueue(Action? navigate = null)
		{
			navigate?.Invoke();
		}
		public static void GoToPlaylist(VariableContainer variables)
		{
			GoToPlaylist(variables.Playlist, variables.LibraryNavigatable?.OnNavigatePlaylist);
		}
		public static void GoToPlaylist(IPlaylist? playlist, Action<IPlaylist?>? navigate = null)
		{
			navigate?.Invoke(playlist);
		}
	}
}