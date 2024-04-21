using System.Collections.Generic;

using Xyzu.Library.Enums;
using Xyzu.Library.Models;

namespace System.Linq
{
	public static class ModelsSortKeysExtensions
	{
		public static IEnumerable<IAlbum> Sort(this IEnumerable<IAlbum> albums, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, _) => albums,
				(ModelSortKeys.AlbumArtist, true) => albums.OrderByDescending(album => album.Artist),
				(ModelSortKeys.AlbumArtist, _) => albums.OrderBy(album => album.Artist),
				(ModelSortKeys.Albums, _) => albums,
				(ModelSortKeys.Artist, true) => albums.OrderByDescending(album => album.Artist),
				(ModelSortKeys.Artist, _) => albums.OrderBy(album => album.Artist),
				(ModelSortKeys.Bitrate, _) => albums,
				(ModelSortKeys.DateAdded, _) => albums,
				(ModelSortKeys.DateCreated, _) => albums,
				(ModelSortKeys.DateModified, _) => albums,
				(ModelSortKeys.DiscNumber, _) => albums,
				(ModelSortKeys.Discs, true) => albums.OrderByDescending(album => album.DiscCount),
				(ModelSortKeys.Discs, _) => albums.OrderBy(album => album.DiscCount),
				(ModelSortKeys.Duration, true) => albums.OrderByDescending(album => album.Duration),
				(ModelSortKeys.Duration, _) => albums.OrderBy(album => album.Duration),
				(ModelSortKeys.Genre, _) => albums,
				(ModelSortKeys.Name, _) => albums,
				(ModelSortKeys.Playlist, _) => albums,
				(ModelSortKeys.Position, _) => albums,
				(ModelSortKeys.Rating, true) => albums.OrderByDescending(album => album.Rating),
				(ModelSortKeys.Rating, _) => albums.OrderBy(album => album.Rating),
				(ModelSortKeys.ReleaseDate, true) => albums.OrderByDescending(album => album.ReleaseDate),
				(ModelSortKeys.ReleaseDate, _) => albums.OrderBy(album => album.ReleaseDate),
				(ModelSortKeys.Size, _) => albums,
				(ModelSortKeys.Songs, true) => albums.OrderByDescending(album => album.SongIds.Count()),
				(ModelSortKeys.Songs, _) => albums.OrderBy(album => album.SongIds.Count()),
				(ModelSortKeys.Title, true) => albums.OrderByDescending(album => album.Title),
				(ModelSortKeys.Title, _) => albums.OrderBy(album => album.Title),
				(ModelSortKeys.TrackNumber, _) => albums,

				_ => albums
			};
		}
		public static IEnumerable<IArtist> Sort(this IEnumerable<IArtist> artists, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, _) => artists,
				(ModelSortKeys.AlbumArtist, _) => artists,
				(ModelSortKeys.Albums, true) => artists.OrderByDescending(artist => artist.AlbumIds.Count()),
				(ModelSortKeys.Albums, _) => artists.OrderBy(artist => artist.AlbumIds.Count()),
				(ModelSortKeys.Artist, _) => artists,
				(ModelSortKeys.Bitrate, _) => artists,
				(ModelSortKeys.DateAdded, _) => artists,
				(ModelSortKeys.DateCreated, _) => artists,
				(ModelSortKeys.DateModified, _) => artists,
				(ModelSortKeys.DiscNumber, _) => artists,
				(ModelSortKeys.Discs, _) => artists,
				(ModelSortKeys.Duration, _) => artists,
				(ModelSortKeys.Genre, _) => artists,
				(ModelSortKeys.Name, true) => artists.OrderByDescending(artist => artist.Name),
				(ModelSortKeys.Name, _) => artists.OrderBy(artist => artist.Name),
				(ModelSortKeys.Playlist, _) => artists,
				(ModelSortKeys.Position, _) => artists,
				(ModelSortKeys.Rating, true) => artists.OrderByDescending(artist => artist.Rating),
				(ModelSortKeys.Rating, _) => artists.OrderBy(artist => artist.Rating),
				(ModelSortKeys.ReleaseDate, _) => artists,
				(ModelSortKeys.Size, _) => artists,
				(ModelSortKeys.Songs, true) => artists.OrderByDescending(artist => artist.SongIds.Count()),
				(ModelSortKeys.Songs, _) => artists.OrderBy(artist => artist.SongIds.Count()),
				(ModelSortKeys.Title, _) => artists,
				(ModelSortKeys.TrackNumber, _) => artists,

				_ => artists
			};
		}
		public static IEnumerable<IGenre> Sort(this IEnumerable<IGenre> genres, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, _) => genres,
				(ModelSortKeys.AlbumArtist, _) => genres,
				(ModelSortKeys.Albums, _) => genres,
				(ModelSortKeys.Artist, _) => genres,
				(ModelSortKeys.Bitrate, _) => genres,
				(ModelSortKeys.DateAdded, _) => genres,
				(ModelSortKeys.DateCreated, _) => genres,
				(ModelSortKeys.DateModified, _) => genres,
				(ModelSortKeys.DiscNumber, _) => genres,
				(ModelSortKeys.Discs, _) => genres,
				(ModelSortKeys.Duration, _) => genres,
				(ModelSortKeys.Genre, _) => genres,
				(ModelSortKeys.Name, true) => genres.OrderByDescending(genre => genre.Name),
				(ModelSortKeys.Name, _) => genres.OrderBy(genre => genre.Name),
				(ModelSortKeys.Playlist, _) => genres,
				(ModelSortKeys.Position, _) => genres,
				(ModelSortKeys.Rating, true) => genres.OrderByDescending(genre => genre.Rating),
				(ModelSortKeys.Rating, _) => genres.OrderBy(genre => genre.Rating),
				(ModelSortKeys.ReleaseDate, _) => genres,
				(ModelSortKeys.Size, _) => genres,
				(ModelSortKeys.Songs, true) => genres.OrderByDescending(genre => genre.SongIds.Count()),
				(ModelSortKeys.Songs, _) => genres.OrderBy(genre => genre.SongIds.Count()),
				(ModelSortKeys.Title, _) => genres,
				(ModelSortKeys.TrackNumber, _) => genres,

				_ => genres
			};
		}
		public static IEnumerable<IPlaylist> Sort(this IEnumerable<IPlaylist> playlists, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, _) => playlists,
				(ModelSortKeys.AlbumArtist, _) => playlists,
				(ModelSortKeys.Albums, _) => playlists,
				(ModelSortKeys.Artist, _) => playlists,
				(ModelSortKeys.Bitrate, _) => playlists,
				(ModelSortKeys.DateAdded, _) => playlists,
				(ModelSortKeys.DateCreated, true) => playlists.OrderByDescending(playlist => playlist.DateCreated),
				(ModelSortKeys.DateCreated, _) => playlists.OrderBy(playlist => playlist.DateCreated),
				(ModelSortKeys.DateModified, true) => playlists.OrderByDescending(playlist => playlist.DateModified),
				(ModelSortKeys.DateModified, _) => playlists.OrderBy(playlist => playlist.DateModified),
				(ModelSortKeys.DiscNumber, _) => playlists,
				(ModelSortKeys.Discs, _) => playlists,
				(ModelSortKeys.Duration, _) => playlists,
				(ModelSortKeys.Genre, _) => playlists,
				(ModelSortKeys.Name, true) => playlists.OrderByDescending(playlist => playlist.Name),
				(ModelSortKeys.Name, _) => playlists.OrderBy(playlist => playlist.Name),
				(ModelSortKeys.Playlist, _) => playlists,
				(ModelSortKeys.Position, _) => playlists,
				(ModelSortKeys.Rating, true) => playlists.OrderByDescending(playlist => playlist.Rating),
				(ModelSortKeys.Rating, _) => playlists.OrderBy(playlist => playlist.Rating),
				(ModelSortKeys.ReleaseDate, _) => playlists,
				(ModelSortKeys.Size, _) => playlists,
				(ModelSortKeys.Songs, true) => playlists.OrderByDescending(playlist => playlist.SongIds.Count()),
				(ModelSortKeys.Songs, _) => playlists.OrderBy(playlist => playlist.SongIds.Count()),
				(ModelSortKeys.Title, _) => playlists,
				(ModelSortKeys.TrackNumber, _) => playlists,

				_ => playlists
			};
		}
		public static IEnumerable<ISong> Sort(this IEnumerable<ISong> songs, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, true) => songs.OrderByDescending(song => song.Album).ThenByDescending(song => song.TrackNumber),
				(ModelSortKeys.Album, _) => songs.OrderBy(song => song.Album).ThenBy(song => song.TrackNumber),
				(ModelSortKeys.AlbumArtist, true) => songs.OrderByDescending(song => song.AlbumArtist),
				(ModelSortKeys.AlbumArtist, _) => songs.OrderBy(song => song.AlbumArtist),
				(ModelSortKeys.Albums, _) => songs,
				(ModelSortKeys.Artist, true) => songs.OrderByDescending(song => song.Artist),
				(ModelSortKeys.Artist, _) => songs.OrderBy(song => song.Artist),
				(ModelSortKeys.Bitrate, _) => songs.OrderBy(song => song.Bitrate),
				(ModelSortKeys.DateAdded, true) => songs.OrderByDescending(song => song.DateAdded),
				(ModelSortKeys.DateAdded, _) => songs.OrderBy(song => song.DateAdded),
				(ModelSortKeys.DateCreated, _) => songs,
				(ModelSortKeys.DateModified, true) => songs.OrderByDescending(song => song.DateModified),
				(ModelSortKeys.DateModified, _) => songs.OrderBy(song => song.DateModified),
				(ModelSortKeys.DiscNumber, true) => songs.OrderByDescending(song => song.DiscNumber),
				(ModelSortKeys.DiscNumber, _) => songs.OrderBy(song => song.DiscNumber),
				(ModelSortKeys.Discs, _) => songs,
				(ModelSortKeys.Duration, true) => songs.OrderByDescending(song => song.Duration),
				(ModelSortKeys.Duration, _) => songs.OrderBy(song => song.Duration),
				(ModelSortKeys.Genre, true) => songs.OrderByDescending(song => song.Genre),
				(ModelSortKeys.Genre, _) => songs.OrderBy(song => song.Genre),
				(ModelSortKeys.Name, _) => songs,
				(ModelSortKeys.Playlist, _) => songs,
				(ModelSortKeys.Position, _) => songs,
				(ModelSortKeys.Rating, true) => songs.OrderByDescending(song => song.Rating),
				(ModelSortKeys.Rating, _) => songs.OrderBy(song => song.Rating),
				(ModelSortKeys.ReleaseDate, true) => songs.OrderByDescending(song => song.ReleaseDate),
				(ModelSortKeys.ReleaseDate, _) => songs.OrderBy(song => song.ReleaseDate),
				(ModelSortKeys.Size, true) => songs.OrderByDescending(song => song.Size),
				(ModelSortKeys.Size, _) => songs.OrderBy(song => song.Size),
				(ModelSortKeys.Songs, _) => songs,
				(ModelSortKeys.Title, true) => songs.OrderByDescending(song => song.Title),
				(ModelSortKeys.Title, _) => songs.OrderBy(song => song.Title),
				(ModelSortKeys.TrackNumber, true) => songs.OrderByDescending(song => song.DiscNumber).ThenByDescending(song => song.TrackNumber),
				(ModelSortKeys.TrackNumber, _) => songs.OrderBy(song => song.DiscNumber).ThenBy(song => song.TrackNumber),

				_ => songs
			};
		}

		public static IAsyncEnumerable<IAlbum> Sort(this IAsyncEnumerable<IAlbum> albums, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, _) => albums,
				(ModelSortKeys.AlbumArtist, true) => albums.OrderByDescending(album => album.Artist),
				(ModelSortKeys.AlbumArtist, _) => albums.OrderBy(album => album.Artist),
				(ModelSortKeys.Albums, _) => albums,
				(ModelSortKeys.Artist, true) => albums.OrderByDescending(album => album.Artist),
				(ModelSortKeys.Artist, _) => albums.OrderBy(album => album.Artist),
				(ModelSortKeys.Bitrate, _) => albums,
				(ModelSortKeys.DateAdded, _) => albums,
				(ModelSortKeys.DateCreated, _) => albums,
				(ModelSortKeys.DateModified, _) => albums,
				(ModelSortKeys.DiscNumber, _) => albums,
				(ModelSortKeys.Discs, true) => albums.OrderByDescending(album => album.DiscCount),
				(ModelSortKeys.Discs, _) => albums.OrderBy(album => album.DiscCount),
				(ModelSortKeys.Duration, true) => albums.OrderByDescending(album => album.Duration),
				(ModelSortKeys.Duration, _) => albums.OrderBy(album => album.Duration),
				(ModelSortKeys.Genre, _) => albums,
				(ModelSortKeys.Name, _) => albums,
				(ModelSortKeys.Playlist, _) => albums,
				(ModelSortKeys.Position, _) => albums,
				(ModelSortKeys.Rating, true) => albums.OrderByDescending(album => album.Rating),
				(ModelSortKeys.Rating, _) => albums.OrderBy(album => album.Rating),
				(ModelSortKeys.ReleaseDate, true) => albums.OrderByDescending(album => album.ReleaseDate),
				(ModelSortKeys.ReleaseDate, _) => albums.OrderBy(album => album.ReleaseDate),
				(ModelSortKeys.Size, _) => albums,
				(ModelSortKeys.Songs, true) => albums.OrderByDescending(album => album.SongIds.Count()),
				(ModelSortKeys.Songs, _) => albums.OrderBy(album => album.SongIds.Count()),
				(ModelSortKeys.Title, true) => albums.OrderByDescending(album => album.Title),
				(ModelSortKeys.Title, _) => albums.OrderBy(album => album.Title),
				(ModelSortKeys.TrackNumber, _) => albums,

				_ => albums
			};
		}
		public static IAsyncEnumerable<IArtist> Sort(this IAsyncEnumerable<IArtist> artists, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, _) => artists,
				(ModelSortKeys.AlbumArtist, _) => artists,
				(ModelSortKeys.Albums, true) => artists.OrderByDescending(artist => artist.AlbumIds.Count()),
				(ModelSortKeys.Albums, _) => artists.OrderBy(artist => artist.AlbumIds.Count()),
				(ModelSortKeys.Artist, _) => artists,
				(ModelSortKeys.Bitrate, _) => artists,
				(ModelSortKeys.DateAdded, _) => artists,
				(ModelSortKeys.DateCreated, _) => artists,
				(ModelSortKeys.DateModified, _) => artists,
				(ModelSortKeys.DiscNumber, _) => artists,
				(ModelSortKeys.Discs, _) => artists,
				(ModelSortKeys.Duration, _) => artists,
				(ModelSortKeys.Genre, _) => artists,
				(ModelSortKeys.Name, true) => artists.OrderByDescending(artist => artist.Name),
				(ModelSortKeys.Name, _) => artists.OrderBy(artist => artist.Name),
				(ModelSortKeys.Playlist, _) => artists,
				(ModelSortKeys.Position, _) => artists,
				(ModelSortKeys.Rating, true) => artists.OrderByDescending(artist => artist.Rating),
				(ModelSortKeys.Rating, _) => artists.OrderBy(artist => artist.Rating),
				(ModelSortKeys.ReleaseDate, _) => artists,
				(ModelSortKeys.Size, _) => artists,
				(ModelSortKeys.Songs, true) => artists.OrderByDescending(artist => artist.SongIds.Count()),
				(ModelSortKeys.Songs, _) => artists.OrderBy(artist => artist.SongIds.Count()),
				(ModelSortKeys.Title, _) => artists,
				(ModelSortKeys.TrackNumber, _) => artists,

				_ => artists
			};
		}
		public static IAsyncEnumerable<IGenre> Sort(this IAsyncEnumerable<IGenre> genres, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, _) => genres,
				(ModelSortKeys.AlbumArtist, _) => genres,
				(ModelSortKeys.Albums, _) => genres,
				(ModelSortKeys.Artist, _) => genres,
				(ModelSortKeys.Bitrate, _) => genres,
				(ModelSortKeys.DateAdded, _) => genres,
				(ModelSortKeys.DateCreated, _) => genres,
				(ModelSortKeys.DateModified, _) => genres,
				(ModelSortKeys.DiscNumber, _) => genres,
				(ModelSortKeys.Discs, _) => genres,
				(ModelSortKeys.Duration, _) => genres,
				(ModelSortKeys.Genre, _) => genres,
				(ModelSortKeys.Name, true) => genres.OrderByDescending(genre => genre.Name),
				(ModelSortKeys.Name, _) => genres.OrderBy(genre => genre.Name),
				(ModelSortKeys.Playlist, _) => genres,
				(ModelSortKeys.Position, _) => genres,
				(ModelSortKeys.Rating, true) => genres.OrderByDescending(genre => genre.Rating),
				(ModelSortKeys.Rating, _) => genres.OrderBy(genre => genre.Rating),
				(ModelSortKeys.ReleaseDate, _) => genres,
				(ModelSortKeys.Size, _) => genres,
				(ModelSortKeys.Songs, true) => genres.OrderByDescending(genre => genre.SongIds.Count()),
				(ModelSortKeys.Songs, _) => genres.OrderBy(genre => genre.SongIds.Count()),
				(ModelSortKeys.Title, _) => genres,
				(ModelSortKeys.TrackNumber, _) => genres,

				_ => genres
			};
		}
		public static IAsyncEnumerable<IPlaylist> Sort(this IAsyncEnumerable<IPlaylist> playlists, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, _) => playlists,
				(ModelSortKeys.AlbumArtist, _) => playlists,
				(ModelSortKeys.Albums, _) => playlists,
				(ModelSortKeys.Artist, _) => playlists,
				(ModelSortKeys.Bitrate, _) => playlists,
				(ModelSortKeys.DateAdded, _) => playlists,
				(ModelSortKeys.DateCreated, true) => playlists.OrderByDescending(playlist => playlist.DateCreated),
				(ModelSortKeys.DateCreated, _) => playlists.OrderBy(playlist => playlist.DateCreated),
				(ModelSortKeys.DateModified, true) => playlists.OrderByDescending(playlist => playlist.DateModified),
				(ModelSortKeys.DateModified, _) => playlists.OrderBy(playlist => playlist.DateModified),
				(ModelSortKeys.DiscNumber, _) => playlists,
				(ModelSortKeys.Discs, _) => playlists,
				(ModelSortKeys.Duration, _) => playlists,
				(ModelSortKeys.Genre, _) => playlists,
				(ModelSortKeys.Name, true) => playlists.OrderByDescending(playlist => playlist.Name),
				(ModelSortKeys.Name, _) => playlists.OrderBy(playlist => playlist.Name),
				(ModelSortKeys.Playlist, _) => playlists,
				(ModelSortKeys.Position, _) => playlists,
				(ModelSortKeys.Rating, true) => playlists.OrderByDescending(playlist => playlist.Rating),
				(ModelSortKeys.Rating, _) => playlists.OrderBy(playlist => playlist.Rating),
				(ModelSortKeys.ReleaseDate, _) => playlists,
				(ModelSortKeys.Size, _) => playlists,
				(ModelSortKeys.Songs, true) => playlists.OrderByDescending(playlist => playlist.SongIds.Count()),
				(ModelSortKeys.Songs, _) => playlists.OrderBy(playlist => playlist.SongIds.Count()),
				(ModelSortKeys.Title, _) => playlists,
				(ModelSortKeys.TrackNumber, _) => playlists,

				_ => playlists
			};
		}
		public static IAsyncEnumerable<ISong> Sort(this IAsyncEnumerable<ISong> songs, ModelSortKeys modelsortkey, bool descending = false)
		{
			return (modelsortkey, descending) switch
			{
				(ModelSortKeys.Album, true) => songs.OrderByDescending(song => song.Album).ThenByDescending(song => song.TrackNumber),
				(ModelSortKeys.Album, _) => songs.OrderBy(song => song.Album).ThenBy(song => song.TrackNumber),
				(ModelSortKeys.AlbumArtist, true) => songs.OrderByDescending(song => song.AlbumArtist),
				(ModelSortKeys.AlbumArtist, _) => songs.OrderBy(song => song.AlbumArtist),
				(ModelSortKeys.Albums, _) => songs,
				(ModelSortKeys.Artist, true) => songs.OrderByDescending(song => song.Artist),
				(ModelSortKeys.Artist, _) => songs.OrderBy(song => song.Artist),
				(ModelSortKeys.Bitrate, _) => songs.OrderBy(song => song.Bitrate),
				(ModelSortKeys.DateAdded, true) => songs.OrderByDescending(song => song.DateAdded),
				(ModelSortKeys.DateAdded, _) => songs.OrderBy(song => song.DateAdded),
				(ModelSortKeys.DateCreated, _) => songs,
				(ModelSortKeys.DateModified, true) => songs.OrderByDescending(song => song.DateModified),
				(ModelSortKeys.DateModified, _) => songs.OrderBy(song => song.DateModified),
				(ModelSortKeys.DiscNumber, true) => songs.OrderByDescending(song => song.DiscNumber),
				(ModelSortKeys.DiscNumber, _) => songs.OrderBy(song => song.DiscNumber),
				(ModelSortKeys.Discs, _) => songs,
				(ModelSortKeys.Duration, true) => songs.OrderByDescending(song => song.Duration),
				(ModelSortKeys.Duration, _) => songs.OrderBy(song => song.Duration),
				(ModelSortKeys.Genre, true) => songs.OrderByDescending(song => song.Genre),
				(ModelSortKeys.Genre, _) => songs.OrderBy(song => song.Genre),
				(ModelSortKeys.Name, _) => songs,
				(ModelSortKeys.Playlist, _) => songs,
				(ModelSortKeys.Position, _) => songs,
				(ModelSortKeys.Rating, true) => songs.OrderByDescending(song => song.Rating),
				(ModelSortKeys.Rating, _)=> songs.OrderBy(song => song.Rating),
				(ModelSortKeys.ReleaseDate, true) => songs.OrderByDescending(song => song.ReleaseDate),
				(ModelSortKeys.ReleaseDate, _) => songs.OrderBy(song => song.ReleaseDate),
				(ModelSortKeys.Size, true) => songs.OrderByDescending(song => song.Size),
				(ModelSortKeys.Size, _) => songs.OrderBy(song => song.Size),
				(ModelSortKeys.Songs, _) => songs,
				(ModelSortKeys.Title, true) => songs.OrderByDescending(song => song.Title),
				(ModelSortKeys.Title, _) => songs.OrderBy(song => song.Title),
				(ModelSortKeys.TrackNumber, true) => songs.OrderByDescending(song => song.DiscNumber).ThenByDescending(song => song.TrackNumber),
				(ModelSortKeys.TrackNumber, _) => songs.OrderBy(song => song.DiscNumber).ThenBy(song => song.TrackNumber),

				_ => songs
			};
		}
	}
}
