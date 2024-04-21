using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Models;

namespace Xyzu.Library.Enums
{
	public enum ModelSortKeys
	{
		Album,
		Albums,
		AlbumArtist,
		Artist,
		Bitrate,
		DateAdded,
		DateCreated,
		DateModified,
		DiscNumber,
		Discs,
		Duration,
		Genre,
		Name,
		Playlist,
		Position,
		Rating,
		ReleaseDate,
		Size,
		Songs,
		Title,
		TrackNumber,
	}

	public static class ModelSortKeysExtensions
	{
		public static bool CanSort(this ModelSortKeys modelsortkey, IEnumerable<IAlbum> albums)
		{
			return modelsortkey switch
			{
				ModelSortKeys.Album => true,
				ModelSortKeys.Albums or
				ModelSortKeys.AlbumArtist => albums.All(album => string.IsNullOrWhiteSpace(album.Title)),
				ModelSortKeys.Artist => true,
				ModelSortKeys.Bitrate => true,
				ModelSortKeys.DateAdded => true,
				ModelSortKeys.DateCreated => true,
				ModelSortKeys.DateModified => true,
				ModelSortKeys.DiscNumber => true,
				ModelSortKeys.Discs => albums.All(album => album.DiscCount is null) is false,
				ModelSortKeys.Duration => true,
				ModelSortKeys.Genre => true,
				ModelSortKeys.Name => true,
				ModelSortKeys.Playlist => true,
				ModelSortKeys.Position => true,
				ModelSortKeys.Rating => true,
				ModelSortKeys.ReleaseDate => albums.All(album => album.ReleaseDate is null) is false,
				ModelSortKeys.Size => true,
				ModelSortKeys.Title => albums.All(album => string.IsNullOrWhiteSpace(album.Title)),
				ModelSortKeys.TrackNumber => true,

				_ => false
			};
		}
		public static bool CanSort(this ModelSortKeys modelsortkey, IEnumerable<IArtist> artists)
		{
			return modelsortkey switch
			{
				ModelSortKeys.Album => true,
				ModelSortKeys.AlbumArtist => true,
				ModelSortKeys.Artist => true,
				ModelSortKeys.Bitrate => true,
				ModelSortKeys.DateAdded => true,
				ModelSortKeys.DateCreated => true,
				ModelSortKeys.DateModified => true,
				ModelSortKeys.DiscNumber => true,
				ModelSortKeys.Discs => true,
				ModelSortKeys.Duration => true,
				ModelSortKeys.Genre => true,
				ModelSortKeys.Name => artists.All(artist => string.IsNullOrWhiteSpace(artist.Name)),
				ModelSortKeys.Playlist => true,
				ModelSortKeys.Position => true,
				ModelSortKeys.Rating => true,
				ModelSortKeys.ReleaseDate => true,
				ModelSortKeys.Size => true,
				ModelSortKeys.Title => true,
				ModelSortKeys.TrackNumber => true,

				_ => false
			};
		}
		public static bool CanSort(this ModelSortKeys modelsortkey, IEnumerable<IGenre> genres)
		{
			return modelsortkey switch
			{
				ModelSortKeys.Album => true,
				ModelSortKeys.Albums => true,
				ModelSortKeys.AlbumArtist => true,
				ModelSortKeys.Artist => true,
				ModelSortKeys.Bitrate => true,
				ModelSortKeys.DateAdded => true,
				ModelSortKeys.DateCreated => true,
				ModelSortKeys.DateModified => true,
				ModelSortKeys.DiscNumber => true,
				ModelSortKeys.Discs => true,
				ModelSortKeys.Duration => true,
				ModelSortKeys.Genre => true,
				ModelSortKeys.Name => genres.All(genre => string.IsNullOrWhiteSpace(genre.Name)),
				ModelSortKeys.Playlist => true,
				ModelSortKeys.Position => true,
				ModelSortKeys.Rating => true,
				ModelSortKeys.ReleaseDate => true,
				ModelSortKeys.Size => true,
				ModelSortKeys.Title => true,
				ModelSortKeys.TrackNumber => true,

				_ => false
			};
		}
		public static bool CanSort(this ModelSortKeys modelsortkey, IEnumerable<IPlaylist> playlists)
		{
			return modelsortkey switch
			{
				ModelSortKeys.Album => true,
				ModelSortKeys.Albums => true,
				ModelSortKeys.AlbumArtist => true,
				ModelSortKeys.Artist => true,
				ModelSortKeys.Bitrate => true,
				ModelSortKeys.DateAdded => true,
				ModelSortKeys.DateCreated => true,
				ModelSortKeys.DateModified => playlists.All(playlist => playlist.DateModified is null) is false,
				ModelSortKeys.DiscNumber => true,
				ModelSortKeys.Discs => true,
				ModelSortKeys.Duration => true,
				ModelSortKeys.Genre => true,
				ModelSortKeys.Name => playlists.All(playlist => string.IsNullOrWhiteSpace(playlist.Name)),
				ModelSortKeys.Playlist => true,
				ModelSortKeys.Position => true,
				ModelSortKeys.Rating => true,
				ModelSortKeys.ReleaseDate => true,
				ModelSortKeys.Size => true,
				ModelSortKeys.Songs => true,
				ModelSortKeys.Title => true,
				ModelSortKeys.TrackNumber => true,

				_ => false
			};
		}
		public static bool CanSort(this ModelSortKeys modelsortkey, IEnumerable<ISong> songs)
		{
			return modelsortkey switch
			{
				ModelSortKeys.Album => songs.All(song => string.IsNullOrWhiteSpace(song.Album)) is false,
				ModelSortKeys.Albums => true,
				ModelSortKeys.AlbumArtist => songs.All(song => string.IsNullOrWhiteSpace(song.AlbumArtist)) is false,
				ModelSortKeys.Artist => songs.All(song => string.IsNullOrWhiteSpace(song.Artist)) is false,
				ModelSortKeys.Bitrate => songs.All(song => song.Bitrate is null) is false,
				ModelSortKeys.DateAdded => songs.All(song => song.DateAdded is null) is false,
				ModelSortKeys.DateCreated => true,
				ModelSortKeys.DateModified => songs.All(song => song.DateModified is null) is false,
				ModelSortKeys.DiscNumber => songs.All(song => song.DiscNumber is null) is false,
				ModelSortKeys.Discs => true,
				ModelSortKeys.Duration => songs.All(song => song.Duration is null) is false,
				ModelSortKeys.Genre => songs.All(song => string.IsNullOrWhiteSpace(song.Genre)) is false,
				ModelSortKeys.Name => true,
				ModelSortKeys.Playlist => true,
				ModelSortKeys.Position => true,
				ModelSortKeys.Rating => true,
				ModelSortKeys.ReleaseDate => songs.All(song => song.ReleaseDate is null) is false,
				ModelSortKeys.Size => songs.All(song => song.Size is null) is false,
				ModelSortKeys.Songs => true,
				ModelSortKeys.Title => songs.All(song => string.IsNullOrWhiteSpace(song.Title)),
				ModelSortKeys.TrackNumber => songs.All(song => song.TrackNumber is null) is false,

				_ => false
			};
		}

		public static void UpdateForSort(this ModelSortKeys modelsortkey, IAlbum album, IAlbum? updated)
		{
			if (updated is null)
				return;

			switch (modelsortkey)
			{
				case ModelSortKeys.Album:
				case ModelSortKeys.Albums:
				case ModelSortKeys.AlbumArtist:
					break;

				case ModelSortKeys.Artist:
					album.Artist = updated.Artist;
					break;

				case ModelSortKeys.Bitrate:
				case ModelSortKeys.DateAdded:
				case ModelSortKeys.DateCreated:
				case ModelSortKeys.DateModified:
				case ModelSortKeys.DiscNumber:
					break;

				case ModelSortKeys.Discs:
					album.DiscCount = updated.DiscCount;
					break;

				case ModelSortKeys.Duration:
					album.Duration = updated.Duration;
					break;

				case ModelSortKeys.Genre:
				case ModelSortKeys.Name:
				case ModelSortKeys.Playlist:
				case ModelSortKeys.Position:
					break;

				case ModelSortKeys.Rating:
					album.Rating = updated.Rating;
					break;

				case ModelSortKeys.ReleaseDate:
					album.ReleaseDate = updated.ReleaseDate;
					break;

				case ModelSortKeys.Size:
					break;

				case ModelSortKeys.Songs:
					album.SongIds = updated.SongIds;
					break;

				case ModelSortKeys.Title:
					album.Title = updated?.Title;
					break;

				case ModelSortKeys.TrackNumber:
				default: break;
			}
		}
		public static void UpdateForSort(this ModelSortKeys modelsortkey, IArtist artist, IArtist? updated)
		{
			if (updated is null)
				return;

			switch (modelsortkey)
			{
				case ModelSortKeys.Album:
					break;

				case ModelSortKeys.Albums:
					artist.AlbumIds = updated.AlbumIds;
					break;

				case ModelSortKeys.AlbumArtist:
				case ModelSortKeys.Artist:
				case ModelSortKeys.Bitrate:
				case ModelSortKeys.DateAdded:
				case ModelSortKeys.DateCreated:
				case ModelSortKeys.DateModified:
				case ModelSortKeys.DiscNumber:
				case ModelSortKeys.Discs:
				case ModelSortKeys.Duration:
				case ModelSortKeys.Genre:
					break;

				case ModelSortKeys.Name:
					artist.Name = updated?.Name;
					break;

				case ModelSortKeys.Playlist:
				case ModelSortKeys.Position:
					break;

				case ModelSortKeys.Rating:
					artist.Rating = updated.Rating;
					break;

				case ModelSortKeys.ReleaseDate:
				case ModelSortKeys.Size:
					break;

				case ModelSortKeys.Songs:
					artist.SongIds = updated.SongIds;
					break;

				case ModelSortKeys.Title:
				case ModelSortKeys.TrackNumber:

				default: break;
			}
		}
		public static void UpdateForSort(this ModelSortKeys modelsortkey, IGenre genre, IGenre? updated)
		{
			if (updated is null)
				return;

			switch (modelsortkey)
			{
				case ModelSortKeys.Album:
				case ModelSortKeys.Albums:
				case ModelSortKeys.AlbumArtist:
				case ModelSortKeys.Artist:
				case ModelSortKeys.Bitrate:
				case ModelSortKeys.DateAdded:
				case ModelSortKeys.DateCreated:
				case ModelSortKeys.DateModified:
				case ModelSortKeys.DiscNumber:
				case ModelSortKeys.Discs:
				case ModelSortKeys.Duration:
				case ModelSortKeys.Genre:
					break;

				case ModelSortKeys.Name:
					genre.Name = updated?.Name;
					break;

				case ModelSortKeys.Playlist:
				case ModelSortKeys.Position:
					break;

				case ModelSortKeys.Rating:
					genre.Rating = updated.Rating;
					break;

				case ModelSortKeys.ReleaseDate:
				case ModelSortKeys.Size:
					break;

				case ModelSortKeys.Songs:
					genre.SongIds = updated.SongIds;
					break;

				case ModelSortKeys.Title:
				case ModelSortKeys.TrackNumber:

				default: break;
			}
		}
		public static void UpdateForSort(this ModelSortKeys modelsortkey, IPlaylist playlist, IPlaylist? updated)
		{
			if (updated is null)
				return;

			switch (modelsortkey)
			{
				case ModelSortKeys.Album:
				case ModelSortKeys.Albums:
				case ModelSortKeys.AlbumArtist:
				case ModelSortKeys.Artist:
				case ModelSortKeys.Bitrate:
				case ModelSortKeys.DateAdded:
					break;

				case ModelSortKeys.DateCreated:
					playlist.DateCreated = updated.DateCreated;
					break;

				case ModelSortKeys.DateModified:
					playlist.DateModified = updated?.DateModified;
					break;

				case ModelSortKeys.DiscNumber:
				case ModelSortKeys.Discs:
					break;

				case ModelSortKeys.Duration:
					playlist.Duration = updated.Duration;
					break;

				case ModelSortKeys.Genre:
					break;

				case ModelSortKeys.Name:
					playlist.Name = updated?.Name;
					break;

				case ModelSortKeys.Playlist:
				case ModelSortKeys.Position:
					break;

				case ModelSortKeys.Rating:
					playlist.Rating = updated.Rating;
					break;

				case ModelSortKeys.ReleaseDate:
				case ModelSortKeys.Size:
					break;

				case ModelSortKeys.Songs:
					playlist.SongIds = updated.SongIds;
					break;

				case ModelSortKeys.Title:
				case ModelSortKeys.TrackNumber:

				default: break;
			}
		}
		public static void UpdateForSort(this ModelSortKeys modelsortkey, ISong song, ISong? updated)
		{
			if (updated is null)
				return;

			switch (modelsortkey)
			{
				case ModelSortKeys.Album:
					song.Album = updated.Album;
					song.TrackNumber = updated.TrackNumber;
					break;

				case ModelSortKeys.Albums:
					break;

				case ModelSortKeys.AlbumArtist:
					song.AlbumArtist = updated.AlbumArtist;
					break;

				case ModelSortKeys.Artist:
					song.Artist = updated.Artist;
					break;

				case ModelSortKeys.Bitrate:
					song.Bitrate = updated.Bitrate;
					break;

				case ModelSortKeys.DateAdded:
					song.DateAdded = updated.DateAdded;
					break;

				case ModelSortKeys.DateCreated:
					break;

				case ModelSortKeys.DateModified:
					song.DateModified = updated.DateModified;
					break;

				case ModelSortKeys.DiscNumber:
					song.DiscNumber = updated.DiscNumber;
					break;

				case ModelSortKeys.Discs:
					break;

				case ModelSortKeys.Duration:
					song.Duration = updated.Duration;
					break;

				case ModelSortKeys.Genre:
					song.Genre = updated.Genre;
					break;

				case ModelSortKeys.Name:
					break;

				case ModelSortKeys.Playlist:
					break;

				case ModelSortKeys.Position:
					break;

				case ModelSortKeys.Rating:
					song.Rating = updated.Rating;
					break;

				case ModelSortKeys.ReleaseDate:
					song.ReleaseDate = updated.ReleaseDate;
					break;

				case ModelSortKeys.Size:
					song.Size = updated.Size;
					break;

				case ModelSortKeys.Songs:
					break;

				case ModelSortKeys.Title:
					song.Title = updated.Title;
					break;

				case ModelSortKeys.TrackNumber:
					song.TrackNumber = updated.TrackNumber;
					break;

				default: break;
			}
		}
	}
}