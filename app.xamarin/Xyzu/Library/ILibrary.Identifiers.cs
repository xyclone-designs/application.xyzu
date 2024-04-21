
using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;
using Xyzu.Library.Models;

namespace Xyzu.Library
{
    public partial interface ILibrary
	{
		public interface IIdentifiers
		{
			IEnumerable<string>? AlbumIds { get; set; }
			IEnumerable<string>? AlbumTitles { get; set; }
			IEnumerable<string>? AlbumArtistNames { get; set; }
			IEnumerable<string>? ArtistIds { get; set; }
			IEnumerable<string>? ArtistNames { get; set; }
			IEnumerable<string>? GenreIds { get; set; }
			IEnumerable<string>? GenreNames { get; set; }
			IEnumerable<string>? PlaylistIds { get; set; }
			IEnumerable<string>? PlaylistNames { get; set; }
			IEnumerable<string>? SongIds { get; set; }
			IEnumerable<string>? SongTitles { get; set; }
			IEnumerable<string>? WithoutAlbumIds { get; set; }
			IEnumerable<string>? WithoutAlbumTitles { get; set; }
			IEnumerable<string>? WithoutAlbumArtistNames { get; set; }
			IEnumerable<string>? WithoutArtistIds { get; set; }
			IEnumerable<string>? WithoutArtistNames { get; set; }
			IEnumerable<string>? WithoutGenreIds { get; set; }
			IEnumerable<string>? WithoutGenreNames { get; set; }
			IEnumerable<string>? WithoutPlaylistIds { get; set; }
			IEnumerable<string>? WithoutPlaylistNames { get; set; }
			IEnumerable<string>? WithoutSongIds { get; set; }
			IEnumerable<string>? WithoutSongTitles { get; set; }

			IIdentifiers Clear();
			IIdentifiers WithAlbum(IAlbum? album);
			IIdentifiers WithAlbumSong(ISong? albumsong);
			IIdentifiers WithArtist(IArtist? artist);
			IIdentifiers WithArtistAlbum(IAlbum? artistalbum);
			IIdentifiers WithArtistSong(ISong? artistsong);
			IIdentifiers WithGenre(IGenre? genre);
			IIdentifiers WithGenreSong(ISong? genresong);
			IIdentifiers WithPlaylist(IPlaylist? playlist);
			IIdentifiers WithPlaylistSong(ISong? playlistsong);
			IIdentifiers WithSong(ISong? song);	   
			IIdentifiers WithoutAlbum(IAlbum album);
			IIdentifiers WithoutAlbumSong(ISong? albumsong);
			IIdentifiers WithoutArtist(IArtist? artist);
			IIdentifiers WithoutArtistAlbum(IAlbum? artistalbum);
			IIdentifiers WithoutArtistSong(ISong? artistsong);
			IIdentifiers WithoutGenre(IGenre? genre);
			IIdentifiers WithoutGenreSong(ISong? genresong);
			IIdentifiers WithoutPlaylist(IPlaylist? playlist);
			IIdentifiers WithoutPlaylistSong(ISong? playlistsong);
			IIdentifiers WithoutSong(ISong? song);

			bool MatchesAlbum(IAlbum album);
			bool MatchesArtist(IArtist artist);
			bool MatchesGenre(IGenre genre);
			bool MatchesPlaylist(IPlaylist playlist);
			bool MatchesSong(ISong song);

			public static IIdentifiers FromAlbum(IAlbum? album)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithAlbum(album);

				return identifiers;
			}						
			public static IIdentifiers FromAlbums(IEnumerable<IAlbum> albums)
			{
				IIdentifiers identifiers = new Default();

				foreach (IAlbum album in albums)
					identifiers.WithAlbum(album);

				return identifiers;
			}
			public static IIdentifiers FromAlbumSong(ISong? albumsong)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithAlbumSong(albumsong);

				return identifiers;
			}					  
			public static IIdentifiers FromAlbumSongs(IEnumerable<ISong> albumsongs)
			{
				IIdentifiers identifiers = new Default();

				foreach (ISong? albumsong in albumsongs)
					identifiers.WithAlbumSong(albumsong);

				return identifiers;
			}
			public static IIdentifiers FromArtist(IArtist? artist)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithArtist(artist);

				return identifiers;
			}						  
			public static IIdentifiers FromArtists(IEnumerable<IArtist> artists)
			{
				IIdentifiers identifiers = new Default();

				foreach (IArtist artist in artists)
					identifiers.WithArtist(artist);

				return identifiers;
			}
			public static IIdentifiers FromArtistAlbum(IAlbum? artistalbum)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithArtistAlbum(artistalbum);

				return identifiers;
			}				 
			public static IIdentifiers FromArtistAlbums(IEnumerable<IAlbum> artistalbums)
			{
				IIdentifiers identifiers = new Default();

				foreach (IAlbum artistalbum in artistalbums)
					identifiers.WithArtistAlbum(artistalbum);

				return identifiers;
			}
			public static IIdentifiers FromArtistSong(ISong? artistsong)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithArtistSong(artistsong);

				return identifiers;
			}					
			public static IIdentifiers FromArtistSongs(IEnumerable<ISong> artistsongs)
			{
				IIdentifiers identifiers = new Default();

				foreach (ISong artistsong in artistsongs)
					identifiers.WithArtistSong(artistsong);

				return identifiers;
			}
			public static IIdentifiers FromGenre(IGenre? genre)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithGenre(genre);

				return identifiers;
			}							 
			public static IIdentifiers FromGenres(IEnumerable<IGenre> genres)
			{
				IIdentifiers identifiers = new Default();

				foreach (IGenre genre in genres)
					identifiers.WithGenre(genre);

				return identifiers;
			}
			public static IIdentifiers FromGenreSong(ISong? genresong)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithGenreSong(genresong);

				return identifiers;
			}					 
			public static IIdentifiers FromGenreSongs(IEnumerable<ISong> genresongs)
			{
				IIdentifiers identifiers = new Default();

				foreach (ISong genresong in genresongs)
					identifiers.WithGenreSong(genresong);

				return identifiers;
			}
			public static IIdentifiers FromPlaylist(IPlaylist? playlist)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithPlaylist(playlist);

				return identifiers;
			}					   
			public static IIdentifiers FromPlaylists(IEnumerable<IPlaylist> playlists)
			{
				IIdentifiers identifiers = new Default();

				foreach (IPlaylist playlist in playlists)
					identifiers.WithPlaylist(playlist);

				return identifiers;
			}
			public static IIdentifiers FromPlaylistSong(ISong? playlistsong)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithPlaylistSong(playlistsong);

				return identifiers;
			}				   
			public static IIdentifiers FromPlaylistSongs(IEnumerable<ISong> playlistsongs)
			{
				IIdentifiers identifiers = new Default();

				foreach (ISong playlistsong in playlistsongs)
					identifiers.WithPlaylistSong(playlistsong);

				return identifiers;
			}
			public static IIdentifiers FromSong(ISong? song)
			{
				IIdentifiers identifiers = new Default();

				identifiers.WithSong(song);

				return identifiers;
			}						
			public static IIdentifiers FromSongs(IEnumerable<ISong> songs)
			{
				IIdentifiers identifiers = new Default();

				foreach (ISong song in songs)
					identifiers.WithSong(song);

				return identifiers;
			}

			public class Default : IIdentifiers
			{
				public IEnumerable<string>? AlbumIds { get; set; }
				public IEnumerable<string>? AlbumTitles { get; set; }
				public IEnumerable<string>? AlbumArtistNames { get; set; }
				public IEnumerable<string>? ArtistIds { get; set; }
				public IEnumerable<string>? ArtistNames { get; set; }
				public IEnumerable<string>? GenreIds { get; set; }
				public IEnumerable<string>? GenreNames { get; set; }
				public IEnumerable<string>? PlaylistIds { get; set; }
				public IEnumerable<string>? PlaylistNames { get; set; }
				public IEnumerable<string>? SongIds { get; set; }
				public IEnumerable<string>? SongTitles { get; set; }
				public IEnumerable<string>? WithoutAlbumIds { get; set; }
				public IEnumerable<string>? WithoutAlbumTitles { get; set; }
				public IEnumerable<string>? WithoutAlbumArtistNames { get; set; }
				public IEnumerable<string>? WithoutArtistIds { get; set; }
				public IEnumerable<string>? WithoutArtistNames { get; set; }
				public IEnumerable<string>? WithoutGenreIds { get; set; }
				public IEnumerable<string>? WithoutGenreNames { get; set; }
				public IEnumerable<string>? WithoutPlaylistIds { get; set; }
				public IEnumerable<string>? WithoutPlaylistNames { get; set; }
				public IEnumerable<string>? WithoutSongIds { get; set; }
				public IEnumerable<string>? WithoutSongTitles { get; set; }

				public IIdentifiers Clear()
				{
					AlbumIds = null;
					AlbumTitles = null;
					AlbumArtistNames = null;
					ArtistIds = null;
					ArtistNames = null;
					GenreIds = null;
					GenreNames = null;
					PlaylistIds = null;
					PlaylistNames = null;
					SongIds = null;
					SongTitles = null;
					WithoutAlbumIds = null;
					WithoutAlbumTitles = null;
					WithoutAlbumArtistNames = null;
					WithoutArtistIds = null;
					WithoutArtistNames = null;
					WithoutGenreIds = null;
					WithoutGenreNames = null;
					WithoutPlaylistIds = null;
					WithoutPlaylistNames = null;
					WithoutSongIds = null;
					WithoutSongTitles = null;

					return this;
				}
				public IIdentifiers WithAlbum(IAlbum? album)
				{
					if (album is null)
						return this;

					AlbumIds = (AlbumIds ??= Enumerable.Empty<string>())
						.Append(album.Id);

					if (string.IsNullOrWhiteSpace(album.Title) is false)
						AlbumTitles = (AlbumTitles ??= Enumerable.Empty<string>())
							.Append(album.Title!);

					if (string.IsNullOrWhiteSpace(album.Artist) is false)
						AlbumArtistNames = (AlbumArtistNames ??= Enumerable.Empty<string>())
							.Append(album.Artist!);

					if (album.SongIds is not null)
						SongIds = (SongIds ??= Enumerable.Empty<string>())
							.Concat(album.SongIds);

					return this;
				}
				public IIdentifiers WithAlbumSong(ISong? albumsong)
				{
					if (albumsong is null)
						return this;

					SongIds = (SongIds ??= Enumerable.Empty<string>())
						.Append(albumsong.Id);

					if (string.IsNullOrWhiteSpace(albumsong.Album) is false)
						AlbumTitles = (AlbumTitles ??= Enumerable.Empty<string>())
							.Append(albumsong.Album!);

					if (string.IsNullOrWhiteSpace(albumsong.AlbumArtist) is false)
						AlbumArtistNames = (AlbumArtistNames ??= Enumerable.Empty<string>())
							.Append(albumsong.AlbumArtist!);

					return this;
				}
				public IIdentifiers WithArtist(IArtist? artist)
				{
					if (artist is null)
						return this;

					ArtistIds = (ArtistIds ??= Enumerable.Empty<string>())
						.Append(artist.Id);

					if (string.IsNullOrWhiteSpace(artist.Name) is false)
						ArtistNames = (ArtistNames ??= Enumerable.Empty<string>())
							.Append(artist.Name!);

					if (artist.AlbumIds is not null)
						AlbumIds = (AlbumIds ??= Enumerable.Empty<string>())
							.Concat(artist.AlbumIds);

					if (artist.SongIds is not null)
						SongIds = (SongIds ??= Enumerable.Empty<string>())
							.Concat(artist.SongIds);

					return this;
				}
				public IIdentifiers WithArtistAlbum(IAlbum? artistalbum)
				{
					if (artistalbum is null)
						return this;

					AlbumIds = (AlbumIds ??= Enumerable.Empty<string>())
						.Append(artistalbum.Id);

					if (string.IsNullOrWhiteSpace(artistalbum.Title) is false)
						AlbumTitles = (AlbumTitles ??= Enumerable.Empty<string>())
							.Append(artistalbum.Title!);

					if (string.IsNullOrWhiteSpace(artistalbum.Artist) is false)
						AlbumArtistNames = (AlbumArtistNames ??= Enumerable.Empty<string>())
							.Append(artistalbum.Artist!);

					if (artistalbum.SongIds is not null)
						SongIds = (SongIds ??= Enumerable.Empty<string>())
							.Concat(artistalbum.SongIds);

					return this;
				}
				public IIdentifiers WithArtistSong(ISong? artistsong)
				{
					if (artistsong is null)
						return this;

					SongIds = (SongIds ??= Enumerable.Empty<string>())
						.Append(artistsong.Id);

					if (string.IsNullOrWhiteSpace(artistsong.Artist) is false)
						ArtistNames = (ArtistNames ??= Enumerable.Empty<string>())
							.Append(artistsong.Artist!);

					return this;
				}
				public IIdentifiers WithGenre(IGenre? genre)
				{
					if (genre is null)
						return this;

					GenreIds = (GenreIds ??= Enumerable.Empty<string>())
						.Append(genre.Id);

					if (string.IsNullOrWhiteSpace(genre.Name) is false)
						GenreNames = (GenreNames ??= Enumerable.Empty<string>())
							.Append(genre.Name!);

					if (genre.SongIds is not null)
						SongIds = (SongIds ??= Enumerable.Empty<string>())
							.Concat(genre.SongIds);

					return this;
				}
				public IIdentifiers WithGenreSong(ISong? genresong)
				{
					if (genresong is null)
						return this;

					SongIds = (SongIds ??= Enumerable.Empty<string>())
						.Append(genresong.Id);

					if (string.IsNullOrWhiteSpace(genresong.Genre) is false)
						GenreNames = (GenreNames ??= Enumerable.Empty<string>())
							.Append(genresong.Genre!);

					return this;
				}
				public IIdentifiers WithPlaylist(IPlaylist? playlist)
				{
					if (playlist is null)
						return this;

					PlaylistIds = (PlaylistIds ??= Enumerable.Empty<string>())
						.Append(playlist.Id);

					if (string.IsNullOrWhiteSpace(playlist.Name) is false)
						PlaylistNames = (PlaylistNames ??= Enumerable.Empty<string>())
							.Append(playlist.Name!);

					if (playlist.SongIds is not null)
						SongIds = (SongIds ??= Enumerable.Empty<string>())
							.Concat(playlist.SongIds);

					return this;
				}
				public IIdentifiers WithPlaylistSong(ISong? playlistsong)
				{
					if (playlistsong is null)
						return this;

					SongIds = (SongIds ??= Enumerable.Empty<string>())
						.Append(playlistsong.Id);

					return this;
				}
				public IIdentifiers WithSong(ISong? song)
				{
					if (song is null)
						return this;

					SongIds = (SongIds ??= Enumerable.Empty<string>())
						.Append(song.Id);

					if (string.IsNullOrWhiteSpace(song.Album) is false)
						AlbumTitles = (AlbumTitles ??= Enumerable.Empty<string>())
							.Append(song.Album!);

					if (string.IsNullOrWhiteSpace(song.Artist) is false)
						ArtistNames = (ArtistNames ??= Enumerable.Empty<string>())
							.Append(song.Artist!);

					if (string.IsNullOrWhiteSpace(song.Genre) is false)
						GenreNames = (GenreNames ??= Enumerable.Empty<string>())
							.Append(song.Genre!);

					if (string.IsNullOrWhiteSpace(song.Title) is false)
						SongTitles = (SongTitles ??= Enumerable.Empty<string>())
							.Append(song.Title!);

					return this;
				}				   
				public IIdentifiers WithoutAlbum(IAlbum? album)
				{
					if (album is null)
						return this;

					WithoutAlbumIds = (WithoutAlbumIds ??= Enumerable.Empty<string>())
						.Append(album.Id);

					if (string.IsNullOrWhiteSpace(album.Title) is false)
						WithoutAlbumTitles = (WithoutAlbumTitles ??= Enumerable.Empty<string>())
							.Append(album.Title!);

					if (string.IsNullOrWhiteSpace(album.Artist) is false)
						WithoutAlbumArtistNames = (WithoutAlbumArtistNames ??= Enumerable.Empty<string>())
							.Append(album.Artist!);

					return this;
				}
				public IIdentifiers WithoutAlbumSong(ISong? albumsong)
				{
					if (albumsong is null)
						return this;

					WithoutSongIds = (WithoutSongIds ??= Enumerable.Empty<string>())
						.Append(albumsong.Id);

					if (string.IsNullOrWhiteSpace(albumsong.Album) is false)
						WithoutAlbumTitles = (WithoutAlbumTitles ??= Enumerable.Empty<string>())
							.Append(albumsong.Album!);

					if (string.IsNullOrWhiteSpace(albumsong.AlbumArtist) is false)
						WithoutAlbumArtistNames = (WithoutAlbumArtistNames ??= Enumerable.Empty<string>())
							.Append(albumsong.AlbumArtist!);

					return this;
				}
				public IIdentifiers WithoutArtist(IArtist? artist)
				{
					if (artist is null)
						return this;

					WithoutArtistIds = (WithoutArtistIds ??= Enumerable.Empty<string>())
						.Append(artist.Id);

					if (string.IsNullOrWhiteSpace(artist.Name) is false)
						WithoutArtistNames = (WithoutArtistNames ??= Enumerable.Empty<string>())
							.Append(artist.Name!);

					return this;
				}
				public IIdentifiers WithoutArtistAlbum(IAlbum? artistalbum)
				{
					if (artistalbum is null)
						return this;

					WithoutAlbumIds = (WithoutAlbumIds ??= Enumerable.Empty<string>())
						.Append(artistalbum.Id);

					if (string.IsNullOrWhiteSpace(artistalbum.Title) is false)
						WithoutAlbumTitles = (WithoutAlbumTitles ??= Enumerable.Empty<string>())
							.Append(artistalbum.Title!);

					if (string.IsNullOrWhiteSpace(artistalbum.Artist) is false)
						WithoutAlbumArtistNames = (WithoutAlbumArtistNames ??= Enumerable.Empty<string>())
							.Append(artistalbum.Artist!);

					return this;
				}
				public IIdentifiers WithoutArtistSong(ISong? artistsong)
				{
					if (artistsong is null)
						return this;

					WithoutSongIds = (WithoutSongIds ??= Enumerable.Empty<string>())
						.Append(artistsong.Id);

					if (string.IsNullOrWhiteSpace(artistsong.Artist) is false)
						WithoutArtistNames = (WithoutArtistNames ??= Enumerable.Empty<string>())
							.Append(artistsong.Artist!);

					return this;
				}
				public IIdentifiers WithoutGenre(IGenre? genre)
				{
					if (genre is null)
						return this;

					WithoutGenreIds = (WithoutGenreIds ??= Enumerable.Empty<string>())
						.Append(genre.Id);

					if (string.IsNullOrWhiteSpace(genre.Name) is false)
						WithoutGenreNames = (WithoutGenreNames ??= Enumerable.Empty<string>())
							.Append(genre.Name!);

					return this;
				}
				public IIdentifiers WithoutGenreSong(ISong? genresong)
				{
					if (genresong is null)
						return this;

					WithoutSongIds = (WithoutSongIds ??= Enumerable.Empty<string>())
						.Append(genresong.Id);

					if (string.IsNullOrWhiteSpace(genresong.Genre) is false)
						WithoutGenreNames = (WithoutGenreNames ??= Enumerable.Empty<string>())
							.Append(genresong.Genre!);

					return this;
				}
				public IIdentifiers WithoutPlaylist(IPlaylist? playlist)
				{
					if (playlist is null)
						return this;

					WithoutPlaylistIds = (WithoutPlaylistIds ??= Enumerable.Empty<string>())
						.Append(playlist.Id);

					if (string.IsNullOrWhiteSpace(playlist.Name) is false)
						WithoutPlaylistNames = (WithoutPlaylistNames ??= Enumerable.Empty<string>())
							.Append(playlist.Name!);

					return this;
				}
				public IIdentifiers WithoutPlaylistSong(ISong? playlistsong)
				{
					if (playlistsong is null)
						return this;

					WithoutSongIds = (WithoutSongIds ??= Enumerable.Empty<string>())
						.Append(playlistsong.Id);

					return this;
				}
				public IIdentifiers WithoutSong(ISong? song)
				{
					if (song is null)
						return this;

					WithoutSongIds = (WithoutSongIds ??= Enumerable.Empty<string>())
						.Append(song.Id);

					if (string.IsNullOrWhiteSpace(song.Album) is false)
						WithoutAlbumTitles = (WithoutAlbumTitles ??= Enumerable.Empty<string>())
							.Append(song.Album!);

					if (string.IsNullOrWhiteSpace(song.Artist) is false)
						WithoutArtistNames = (WithoutArtistNames ??= Enumerable.Empty<string>())
							.Append(song.Artist!);

					if (string.IsNullOrWhiteSpace(song.Genre) is false)
						WithoutGenreNames = (WithoutGenreNames ??= Enumerable.Empty<string>())
							.Append(song.Genre!);

					if (string.IsNullOrWhiteSpace(song.Title) is false)
						WithoutSongTitles = (WithoutSongTitles ??= Enumerable.Empty<string>())
							.Append(song.Title!);

					return this;
				}

				public bool MatchesAlbum(IAlbum album)
				{
					bool nopoint =
						AlbumIds is null && WithoutAlbumIds is null &&
						AlbumTitles is null && WithoutAlbumTitles is null &&
						AlbumArtistNames is null && WithoutAlbumArtistNames is null;

					if (nopoint)
						return false;

					// AlbumIds
					// WithoutAlbumIds
					if (((AlbumIds is not null && AlbumIds.Contains(album.Id) is false) || (WithoutAlbumIds is not null && WithoutAlbumIds.Contains(album.Id))))
						return false;

					// AlbumTitles
					// WithoutAlbumTitles
					if (album.Title is not null)
					{
						if (AlbumTitles is not null && AlbumTitles.Contains(album.Title) is false)
							return false;

						if (WithoutAlbumTitles is not null && WithoutAlbumTitles.Contains(album.Title))
							return false;
					}

					// AlbumArtistNames
					// WithoutAlbumArtistNames
					if (album.Artist is not null)
					{
						if (AlbumArtistNames is not null && AlbumArtistNames.Contains(album.Artist) is false)
							return false;

						if (WithoutAlbumArtistNames is not null && WithoutAlbumArtistNames.Contains(album.Artist))
							return false;
					}

					return true;
				}
				public bool MatchesArtist(IArtist artist)
				{
					bool nopoint =
						ArtistIds is null && WithoutArtistIds is null &&
						ArtistNames is null && WithoutArtistNames is null;

					if (nopoint)
						return false;

					// ArtistIds
					// WithoutArtistIds
					if (((ArtistIds is not null && ArtistIds.Contains(artist.Id) is false) || (WithoutArtistIds is not null && WithoutArtistIds.Contains(artist.Id))))
						return false;

					// ArtistNames
					// WithoutArtistNames
					if (artist.Name is not null)
					{
						if (ArtistNames is not null && ArtistNames.Contains(artist.Name) is false)
							return false;

						if (WithoutArtistNames is not null && WithoutArtistNames.Contains(artist.Name))
							return false;
					}

					return true;
				}
				public bool MatchesGenre(IGenre genre)
				{
					bool nopoint =
						GenreIds is null && WithoutGenreIds is null &&
						GenreNames is null && WithoutGenreNames is null;

					if (nopoint)
						return false;

					// GenreIds
					// WithoutGenreIds
					if (((GenreIds is not null && GenreIds.Contains(genre.Id) is false) || (WithoutGenreIds is not null && WithoutGenreIds.Contains(genre.Id))))
						return false;

					// GenreNames
					// WithoutGenreNames
					if (genre.Name is not null)
					{
						if (GenreNames is not null && GenreNames.Contains(genre.Name) is false)
							return false;

						if (WithoutGenreNames is not null && WithoutGenreNames.Contains(genre.Name))
							return false;
					}

					return true;
				}
				public bool MatchesPlaylist(IPlaylist playlist)
				{
					bool nopoint =
						PlaylistIds is null && WithoutPlaylistIds is null &&
						PlaylistNames is null && WithoutPlaylistNames is null;

					if (nopoint)
						return false;

					// PlaylistIds
					// WithoutPlaylistIds
					if (((PlaylistIds is not null && PlaylistIds.Contains(playlist.Id) is false) || (WithoutPlaylistIds is not null && WithoutPlaylistIds.Contains(playlist.Id))))
						return false;

					// PlaylistNames
					// WithoutPlaylistNames
					if (playlist.Name is not null)
					{
						if (PlaylistNames is not null && PlaylistNames.Contains(playlist.Name) is false)
							return false;

						if (WithoutPlaylistNames is not null && WithoutPlaylistNames.Contains(playlist.Name))
							return false;
					}

					return true;
				}
				public bool MatchesSong(ISong song) 
				{
					bool nopoint =
						SongIds is null && WithoutSongIds is null &&
						SongTitles is null && WithoutSongTitles is null &&
						AlbumTitles is null && WithoutAlbumTitles is null &&
						AlbumArtistNames is null && WithoutAlbumArtistNames is null &&
						ArtistNames is null && WithoutArtistNames is null &&
						GenreNames is null && WithoutGenreNames is null;

					if (nopoint)
						return false;

					// SongIds
					// WithoutSongIds
					if ((SongIds is not null && SongIds.Contains(song.Id) is false) || (WithoutSongIds is not null && WithoutSongIds.Contains(song.Id)))
						return false;

					// SongTitles
					// WithoutSongTitles
					if (song.Title is not null)
					{
						if (SongTitles is not null && SongTitles.Contains(song.Title) is false)
							return false;

						if (WithoutSongTitles is not null && WithoutSongTitles.Contains(song.Title))
							return false;
					}

					// AlbumTitles
					// WithoutAlbumTitles
					if (song.Album is not null)
					{
						if (AlbumTitles is not null && AlbumTitles.Contains(song.Album) is false)
							return false;
																								 
						if (WithoutAlbumTitles is not null && WithoutAlbumTitles.Contains(song.Album))
							return false;
					}			 

					// AlbumArtistNames
					// WithoutAlbumArtistNames
					if (song.AlbumArtist is not null)
					{
						if (AlbumArtistNames is not null && AlbumArtistNames.Contains(song.AlbumArtist) is false)
							return false;
																								 
						if (WithoutAlbumArtistNames is not null && WithoutAlbumArtistNames.Contains(song.AlbumArtist))
							return false;
					}			 

					// ArtistNames
					// WithoutArtistNames
					if (song.Artist is not null)
					{
						if (ArtistNames is not null && ArtistNames.Contains(song.Artist) is false)
							return false;
																								 
						if (WithoutArtistNames is not null && WithoutArtistNames.Contains(song.Artist))
							return false;
					}			 

					// GenreNames
					// WithoutGenreNames
					if (song.Genre is not null)
					{
						if (GenreNames is not null && GenreNames.Contains(song.Genre) is false)
							return false;
																								 
						if (WithoutGenreNames is not null && WithoutGenreNames.Contains(song.Genre))
							return false;
					}

					return true;
				}
			}
		}
	}
}
