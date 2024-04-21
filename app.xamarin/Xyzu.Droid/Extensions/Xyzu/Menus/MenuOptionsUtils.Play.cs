#nullable enable

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Xyzu.Library;
using Xyzu.Library.Models;
using Xyzu.Player;
using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		private static string Album = "Album";
		private static string Albums = "Albums";
		private static string Artist = "Artist";
		private static string Artists = "Artists";
		private static string Genre = "Genre";
		private static string Genres = "Genres";
		private static string Playlist = "Playlist";
		private static string Playlists = "Playlists";
		private static string Search = "Search";
		private static string Songs = "Songs";

		private static bool SkippedAhead(VariableContainer variables)
		{
			if
			(
				variables.QueueId is null ||
				XyzuPlayer.Instance.Player.Queue.Id is null ||
				string.Equals(XyzuPlayer.Instance.Player.Queue.Id, variables.QueueId) is false ||
				variables.Index is null

			) return false;

			XyzuPlayer.Instance.Player.Queue.CurrentIndex = variables.Index.Value;
			XyzuPlayer.Instance.Player.Skip(variables.Index.Value);

			return true;
		}

		public static string QueueIdAlbums(IAlbumsSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}", 
				Albums, 
				settings?.SortKey ?? IAlbumsSettings.Defaults.SortKey, 
				settings?.IsReversed ?? IAlbumsSettings.Defaults.IsReversed);
		}														  
		public static string QueueIdAlbum(string? albumid, IAlbumSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}.{3}", 
				Album,
				albumid ?? string.Empty,
				settings?.SongsSortKey ?? IAlbumSettings.Defaults.SongsSortKey, 
				settings?.SongsIsReversed ?? IAlbumSettings.Defaults.SongsIsReversed);
		}					   
		public static string QueueIdArtists(IArtistsSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}", 
				Artists, 
				settings?.SortKey ?? IArtistsSettings.Defaults.SortKey, 
				settings?.IsReversed ?? IArtistsSettings.Defaults.IsReversed);
		}														  
		public static string QueueIdArtistAlbums(string? artistid, IArtistSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}.{3}", 
				Artist,
				artistid ?? string.Empty,
				settings?.AlbumsSortKey ?? IArtistSettings.Defaults.AlbumsSortKey, 
				settings?.AlbumsIsReversed ?? IArtistSettings.Defaults.AlbumsIsReversed);
		}					   																 
		public static string QueueIdArtistSongs(string? artistid, IArtistSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}.{3}", 
				Artist,
				artistid ?? string.Empty,
				settings?.SongsSortKey ?? IArtistSettings.Defaults.SongsSortKey, 
				settings?.SongsIsReversed ?? IArtistSettings.Defaults.SongsIsReversed);
		}					   
		public static string QueueIdGenres(IGenresSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}", 
				Genres, 
				settings?.SortKey ?? IGenresSettings.Defaults.SortKey, 
				settings?.IsReversed ?? IGenresSettings.Defaults.IsReversed);
		}														  
		public static string QueueIdGenre(string? genreid, IGenreSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}.{3}", 
				Genre,
				genreid ?? string.Empty,
				settings?.SongsSortKey ?? IGenreSettings.Defaults.SongsSortKey, 
				settings?.SongsIsReversed ?? IGenreSettings.Defaults.SongsIsReversed);
		}					   
		public static string QueueIdPlaylists(IPlaylistsSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}", 
				Playlists, 
				settings?.SortKey ?? IPlaylistsSettings.Defaults.SortKey, 
				settings?.IsReversed ?? IPlaylistsSettings.Defaults.IsReversed);
		}														  
		public static string QueueIdPlaylist(string? playlistid, IPlaylistSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}.{3}", 
				Playlist,
				playlistid ?? string.Empty,
				settings?.SongsSortKey ?? IPlaylistSettings.Defaults.SongsSortKey, 
				settings?.SongsIsReversed ?? IPlaylistSettings.Defaults.SongsIsReversed);
		}
		public static string QueueIdSearch(ILibrary.ISearcher? searcher)
		{
			return string.Format(
				"{0}.{1}.{2}.{3}.{4}.{5}.{6}",
				Search,
				searcher?.String ?? string.Empty,
				searcher?.SearchAlbums ?? true,
				searcher?.SearchArtists ?? true,
				searcher?.SearchGenres ?? true,
				searcher?.SearchPlaylists ?? true,
				searcher?.SearchSongs ?? true);
		}									 
		public static string QueueIdSongs(ISongsSettings? settings)
		{
			return string.Format(
				"{0}.{1}.{2}",
				Songs,
				settings?.SortKey ?? IPlaylistsSettings.Defaults.SortKey,
				settings?.IsReversed ?? IPlaylistsSettings.Defaults.IsReversed);
		}

		public static void Play(VariableContainer variables)
		{
			if (SkippedAhead(variables))
				return;

			IEnumerable<IQueueItem> queueitems = Enumerable.Empty<IQueueItem>()
				.Concat(QueueItemsFrom(variables.Albums))
				.Concat(QueueItemsFrom(variables.Artists))
				.Concat(QueueItemsFrom(variables.Genres))
				.Concat(QueueItemsFrom(variables.Playlists))
				.Concat(QueueItemsFrom(variables.Songs));

			XyzuPlayer.Instance.Player.Queue.Refresh(variables.Index ?? 0, queueitems);
			XyzuPlayer.Instance.Player.Play();
		}
		public static void PlayAlbums(VariableContainer variables, IAlbum? album)
		{
			if (SkippedAhead(variables))
				return;

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(album is null
				? variables.Albums
				: Enumerable.Empty<IAlbum>().Append(album));

			XyzuPlayer.Instance.Player.Queue.Refresh(variables.Index ?? 0, queueitems);
			XyzuPlayer.Instance.Player.Queue.Id ??= variables.QueueId ??=
				variables.Album?.Id is string albumid
					? QueueIdAlbum(albumid, variables.AlbumSettings)
					: QueueIdAlbums(variables.AlbumsSettings);

			XyzuPlayer.Instance.Player.Play();
		}
		public static void PlayArtists(VariableContainer variables, IArtist? artist)
		{
			if (SkippedAhead(variables))
				return;

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(artist is null
				? variables.Artists
				: Enumerable.Empty<IArtist>().Append(artist));

			XyzuPlayer.Instance.Player.Queue.Refresh(variables.Index ?? 0, queueitems);
			XyzuPlayer.Instance.Player.Queue.Id ??= variables.QueueId ??=
				variables.Artist?.Id is string albumid
					? QueueIdArtistSongs(albumid, variables.ArtistSettings)
					: QueueIdArtists(variables.ArtistsSettings);

			XyzuPlayer.Instance.Player.Play();
		}
		public static void PlayGenres(VariableContainer variables, IGenre? genre)
		{
			if (SkippedAhead(variables))
				return;

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(genre is null
				? variables.Genres
				: Enumerable.Empty<IGenre>().Append(genre));

			XyzuPlayer.Instance.Player.Queue.Refresh(variables.Index ?? 0, queueitems);
			XyzuPlayer.Instance.Player.Queue.Id ??= variables.QueueId ??=
				variables.Genre?.Id is string genreid
					? QueueIdGenre(genreid, variables.GenreSettings)
					: QueueIdGenres(variables.GenresSettings);

			XyzuPlayer.Instance.Player.Play();
		}
		public static void PlayPlaylists(VariableContainer variables, IPlaylist? playlist)
		{
			if (SkippedAhead(variables))
				return;

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(playlist is null
				? variables.Playlists
				: Enumerable.Empty<IPlaylist>().Append(playlist));

			XyzuPlayer.Instance.Player.Queue.Refresh(variables.Index ?? 0, queueitems);
			XyzuPlayer.Instance.Player.Queue.Id ??= variables.QueueId ??=
				variables.Playlist?.Id is string playlistid
					? QueueIdPlaylist(playlistid, variables.PlaylistSettings)
					: QueueIdPlaylists(variables.PlaylistsSettings);

			XyzuPlayer.Instance.Player.Play();
		}
		public static void PlaySongs(VariableContainer variables, ISong? song)
		{
			if (SkippedAhead(variables))
				return;

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(song is null
				? variables.Songs
				: Enumerable.Empty<ISong>().Append(song));

			XyzuPlayer.Instance.Player.Queue.Refresh(variables.Index ?? 0, queueitems);
			XyzuPlayer.Instance.Player.Queue.Id ??= variables.QueueId ??= QueueIdSongs(variables.SongsSettings);

			XyzuPlayer.Instance.Player.Play();
		}
	}
}