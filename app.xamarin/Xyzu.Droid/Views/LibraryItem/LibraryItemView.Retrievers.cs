#nullable enable

using System.Collections.Generic;
using System.Linq;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;

namespace Xyzu.Views.LibraryItem
{
	public partial class LibraryItemView 
	{
		public static class Retrievers
		{
			public static IAlbum<bool> GenerateAlbumRetriever(IEnumerable<IAlbum>? albums, LibraryLayoutTypes? librarylayouttype = null, ModelSortKeys? modelsortkey = null)
			{
				IAlbum<bool> retriever = new IAlbum.Default<bool>(false)
				{
					Id = true,
					Artwork = new IImage.Default<bool>(false)
					{
						BufferHash = true,
					},
				};

				ProcessRetriever(retriever, librarylayouttype, modelsortkey, albums);

				return retriever;
			}
			public static IArtist<bool> GenerateArtistRetriever(IEnumerable<IArtist>? artists, LibraryLayoutTypes? librarylayouttype = null, ModelSortKeys? modelsortkey = null)
			{
				IArtist<bool> retriever = new IArtist.Default<bool>(false)
				{
					Id = true,
					Image = new IImage.Default<bool>(false)
					{
						BufferHash = true,
					},
				};

				ProcessRetriever(retriever, librarylayouttype, modelsortkey, artists);

				return retriever;
			}
			public static IGenre<bool> GenerateGenreRetriever(IEnumerable<IGenre>? genres, LibraryLayoutTypes? librarylayouttype = null, ModelSortKeys? modelsortkey = null)
			{
				IGenre<bool> retriever = new IGenre.Default<bool>(false)
				{
					Id = true,
				};

				ProcessRetriever(retriever, librarylayouttype, modelsortkey, genres);

				return retriever;
			}
			public static IPlaylist<bool> GeneratePlaylistRetriever(IEnumerable<IPlaylist>? playlists, LibraryLayoutTypes? librarylayouttype = null, ModelSortKeys? modelsortkey = null)
			{
				IPlaylist<bool> retriever = new IPlaylist.Default<bool>(false)
				{
					Id = true,
				};

				ProcessRetriever(retriever, librarylayouttype, modelsortkey, playlists);

				return retriever;
			}
			public static ISong<bool> GenerateSongRetriever(IEnumerable<ISong>? songs, LibraryLayoutTypes? librarylayouttype = null, ModelSortKeys? modelsortkey = null)
			{
				ISong<bool> retriever = new ISong.Default<bool>(false)
				{
					Id = true,
					Artwork = new IImage.Default<bool>(false)
					{
						BufferHash = true,
					},
				};

				ProcessRetriever(retriever, librarylayouttype, modelsortkey, songs);

				return retriever;
			}

			public static void ProcessRetriever(IAlbum<bool> retriever, LibraryLayoutTypes? librarylayouttype, ModelSortKeys? modelsortkey, IEnumerable<IAlbum>? albums)
			{
				if ((int?)librarylayouttype is int librarylayouttypevalue)
				{
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridSmall && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListSmall)
					{
						retriever.Artist = true;
						retriever.Title = true;
						retriever.Duration = true;
						retriever.SongIds = true;
					}
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridMedium && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListMedium) { }
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridLarge && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListLarge) { }
				}

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
						break;

					case ModelSortKeys.Discs when albums?
							.All(album => album.DiscCount.HasValue is false) ?? true:
						retriever.DiscCount = true;
						break;

					case ModelSortKeys.Duration:
					case ModelSortKeys.Genre:
					case ModelSortKeys.Name:
					case ModelSortKeys.Playlist:
					case ModelSortKeys.Position:
					case ModelSortKeys.Rating:
						break;

					case ModelSortKeys.ReleaseDate when albums?
							.All(album => album.ReleaseDate.HasValue is false) ?? true:
						retriever.ReleaseDate = true;
						break;

					case ModelSortKeys.Size:
						break;

					case ModelSortKeys.Songs when albums?
							.All(album => album.SongIds is null) ?? true:
						retriever.SongIds = true;
						break;

					case ModelSortKeys.Title when albums?
							.All(album => string.IsNullOrWhiteSpace(album.Title) is true) ?? true:
						retriever.Title = true;
						break;

					case ModelSortKeys.TrackNumber:

					default: break;
				}
			}
			public static void ProcessRetriever(IArtist<bool> retriever, LibraryLayoutTypes? librarylayouttype, ModelSortKeys? modelsortkey, IEnumerable<IArtist>? artists)
			{
				if ((int?)librarylayouttype is int librarylayouttypevalue)
				{
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridSmall && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListSmall)
					{
						retriever.AlbumIds = true;
						retriever.Name = true;
						retriever.SongIds = true;
					}
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridMedium && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListMedium) { }
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridLarge && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListLarge) { }
				}

				switch (modelsortkey)
				{
					case ModelSortKeys.Album:
						break;

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

					case ModelSortKeys.Name when artists?
							.All(artist => string.IsNullOrWhiteSpace(artist.Name) is true) ?? true:
						retriever.Name = true;
						break;

					case ModelSortKeys.Playlist:
					case ModelSortKeys.Position:
					case ModelSortKeys.Rating:
					case ModelSortKeys.ReleaseDate:
					case ModelSortKeys.Size:
					case ModelSortKeys.Songs:
					case ModelSortKeys.Title:
					case ModelSortKeys.TrackNumber:

					default: break;
				}
			}
			public static void ProcessRetriever(IGenre<bool> retriever, LibraryLayoutTypes? librarylayouttype, ModelSortKeys? modelsortkey, IEnumerable<IGenre>? genres)
			{
				if ((int?)librarylayouttype is int librarylayouttypevalue)
				{
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridSmall && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListSmall)
					{
						retriever.Duration = true;
						retriever.Name = true;
						retriever.SongIds = true;
					}
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridMedium && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListMedium) { }
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridLarge && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListLarge) { }
				}

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

					case ModelSortKeys.Name when genres?
							.All(genre => string.IsNullOrWhiteSpace(genre.Name) is true) ?? true:
						retriever.Name = true;
						break;

					case ModelSortKeys.Playlist:
					case ModelSortKeys.Position:
						break;

					case ModelSortKeys.Rating:
					case ModelSortKeys.ReleaseDate:
					case ModelSortKeys.Size:
					case ModelSortKeys.Songs:
					case ModelSortKeys.Title:
					case ModelSortKeys.TrackNumber:

					default: break;
				}
			}
			public static void ProcessRetriever(IPlaylist<bool> retriever, LibraryLayoutTypes? librarylayouttype, ModelSortKeys? modelsortkey, IEnumerable<IPlaylist>? playlists)
			{
				if ((int?)librarylayouttype is int librarylayouttypevalue)
				{
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridSmall && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListSmall)
					{
						retriever.Duration = true;
						retriever.Name = true;
						retriever.SongIds = true;
					}
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridMedium && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListMedium) { }
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridLarge && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListLarge) { }
				}

				switch (modelsortkey)
				{
					case ModelSortKeys.Album:
					case ModelSortKeys.Albums:
					case ModelSortKeys.AlbumArtist:
					case ModelSortKeys.Artist:
					case ModelSortKeys.Bitrate:
					case ModelSortKeys.DateAdded:
					case ModelSortKeys.DateCreated:
						break;

					case ModelSortKeys.DateModified when playlists?
							.All(playlist => playlist.DateModified.HasValue is false) ?? true:
						retriever.DateModified = true;
						break;

					case ModelSortKeys.DiscNumber:
					case ModelSortKeys.Discs:
					case ModelSortKeys.Duration:
					case ModelSortKeys.Genre:
						break;

					case ModelSortKeys.Name when playlists?
							.All(playlist => string.IsNullOrWhiteSpace(playlist.Name) is true) ?? true:
						retriever.Name = true;
						break;

					case ModelSortKeys.Playlist:
					case ModelSortKeys.Position:
						break;

					case ModelSortKeys.Rating:
					case ModelSortKeys.ReleaseDate:
					case ModelSortKeys.Size:
					case ModelSortKeys.Songs:
					case ModelSortKeys.Title:
					case ModelSortKeys.TrackNumber:

					default: break;
				}
			}
			public static void ProcessRetriever(ISong<bool> retriever, LibraryLayoutTypes? librarylayouttype, ModelSortKeys? modelsortkey, IEnumerable<ISong>? songs)
			{
				if ((int?)librarylayouttype is int librarylayouttypevalue)
				{
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridSmall && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListSmall)
					{
						retriever.Album = true;
						retriever.Artist = true;
						retriever.Title = true;
					}
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridMedium && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListMedium)
					{
						retriever.Duration = true;
						retriever.TrackNumber = true;
					}
					if (librarylayouttypevalue >= (int)LibraryLayoutTypes.GridLarge && librarylayouttypevalue >= (int)LibraryLayoutTypes.ListLarge) { }
				}

				switch (modelsortkey)
				{
					case ModelSortKeys.Album when songs?
							.All(song => string.IsNullOrWhiteSpace(song.Album) is true) ?? true:
						retriever.Album = true;
						if (songs?.All(song => string.IsNullOrWhiteSpace(song.Album) is false) ?? true)
							retriever.TrackNumber = true;
						break;

					case ModelSortKeys.Albums:
						break;

					case ModelSortKeys.AlbumArtist when songs?
							.All(song => string.IsNullOrWhiteSpace(song.AlbumArtist) is true) ?? true:
						retriever.AlbumArtist = true;
						break;

					case ModelSortKeys.Artist when songs?
							.All(song => string.IsNullOrWhiteSpace(song.Artist) is true) ?? true:
						retriever.Artist = true;
						break;

					case ModelSortKeys.Bitrate when songs?
							.All(song => song.Bitrate is null) ?? true:
						retriever.Bitrate = true;
						break;

					case ModelSortKeys.DateAdded when songs?
							.All(song => song.DateAdded is null) ?? true:
						retriever.DateAdded = true;
						break;

					case ModelSortKeys.DateCreated:
						break;

					case ModelSortKeys.DateModified when songs?
							.All(song => song.DateModified is null) ?? true:
						retriever.DateModified = true;
						break;

					case ModelSortKeys.DiscNumber when songs?
							.All(song => song.DiscNumber is null) ?? true:
						retriever.DiscNumber = true;
						break;

					case ModelSortKeys.Discs:
						break;

					case ModelSortKeys.Duration when songs?
							.All(song => song.Duration is null) ?? true:
						retriever.Duration = true;
						break;

					case ModelSortKeys.Genre when songs?
							.All(song => string.IsNullOrWhiteSpace(song.Genre) is true) ?? true:
						retriever.Genre = true;
						break;

					case ModelSortKeys.Name:
					case ModelSortKeys.Playlist:
					case ModelSortKeys.Position:
					case ModelSortKeys.Rating:
						break;

					case ModelSortKeys.ReleaseDate when songs?
							.All(song => song.ReleaseDate is null) ?? true:
						retriever.ReleaseDate = true;
						break;

					case ModelSortKeys.Size when songs?
							.All(song => song.Size is null) ?? true:
						retriever.Size = true;
						break;

					case ModelSortKeys.Songs:
						break;

					case ModelSortKeys.Title when songs?
							.All(song => string.IsNullOrWhiteSpace(song.Title) is true) ?? true:
						retriever.Title = true;
						break;

					case ModelSortKeys.TrackNumber when songs?
							.All(song => song.TrackNumber is null) ?? true:
						retriever.TrackNumber = true;
						break;

					default: break;
				}
			}
		}
	}
}