using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Images;
using Xyzu.Library.Models;
using Xyzu.Player;

namespace Xyzu.Views.LibraryItem
{
	public interface ILibraryItem
	{
		bool Playing { get; set; }
		string? LibraryItemId { get; set; }
		IEnumerable<string>? Details { get; set; }

		void Reset();
		void SetArtwork(IImage? image);
		void SetArtwork(IModel? model);
		void SetPlaying(IPlayer? player, string? queueid)
		{
			switch (true)
			{
				case true when player is null:
				case true when player.Queue.Id is null:
				case true when player.Queue.Current is null:
				case true when queueid != null && player.Queue.Id != queueid:
				case true when LibraryItemId is null:
				case true when LibraryItemId != player.Queue.Current.PrimaryId:
					Playing = false;
					break;		 

				default: 
					Playing = true;
					break;
			}					
		}

		void SetModel(IModel? model, IModel? defaults = null)
		{
			switch (true)
			{
				case true when model is IAlbum album:
					SetAlbum(album);
					break;

				case true when model is IArtist artist:
					SetArtist(artist);
					break;

				case true when model is IGenre genre:
					SetGenre(genre);
					break;

				case true when model is IPlaylist playlist:
					SetPlaylist(playlist);
					break;

				case true when model is ISong song:
					SetSong(song);
					break;

				default:
					break;
			}
		}			  
		void SetArtist(IArtist? artist, IArtist? defaults = null)
		{
			Reset();

			LibraryItemId = artist?.Id ?? defaults?.Id;
			Details = DetailsArtist(artist, defaults);

			SetArtwork(artist ?? defaults);
		}
		void SetArtistAlbum(IAlbum? artistalbum, IAlbum? defaults = null)
		{
			Reset();

			LibraryItemId = artistalbum?.Id ?? defaults?.Id;
			Details = DetailsArtistAlbum(artistalbum, defaults);

			SetArtwork(artistalbum ?? defaults);
		}
		void SetArtistSong(ISong? artistsong, ISong? defaults = null)
		{
			Reset();

			LibraryItemId = artistsong?.Id ?? defaults?.Id;
			Details = DetailsArtistSong(artistsong, defaults);

			SetArtwork(artistsong ?? defaults);
		}
		void SetAlbum(IAlbum? album, IAlbum? defaults = null)
		{
			Reset();

			LibraryItemId = album?.Id ?? defaults?.Id;
			Details = DetailsAlbum(album, defaults);

			SetArtwork(album ?? defaults);
		}
		void SetAlbumSong(ISong? albumsong, ISong? defaults = null)
		{
			Reset();

			LibraryItemId = albumsong?.Id ?? defaults?.Id;
			Details = DetailsSong(albumsong, defaults);

			SetArtwork(albumsong ?? defaults);
		}
		void SetGenre(IGenre? genre, IGenre? defaults = null)
		{
			Reset();

			LibraryItemId = genre?.Id ?? defaults?.Id;
			Details = DetailsGenre(genre, defaults);

			SetArtwork(genre ?? defaults);
		}
		void SetGenreSong(ISong? genresong, ISong? defaults = null)
		{
			Reset();

			LibraryItemId = genresong?.Id ?? defaults?.Id;
			Details = DetailsGenreSong(genresong, defaults);

			SetArtwork(genresong ?? defaults);
		}
		void SetPlaylist(IPlaylist? playlist, IPlaylist? defaults = null)
		{
			Reset();

			LibraryItemId = playlist?.Id ?? defaults?.Id;
			Details = DetailsPlaylist(playlist, defaults);

			SetArtwork(playlist ?? defaults);
		}
		void SetPlaylistSong(ISong? playlistsong, ISong? defaults = null, int? position = null)
		{
			Reset();

			LibraryItemId = playlistsong?.Id ?? defaults?.Id;
			Details = DetailsPlaylistSong(playlistsong, defaults);

			SetArtwork(playlistsong ?? defaults);
		}
		void SetQueueSong(ISong? queuesong, ISong? defaults = null, int? position = null)
		{
			Reset();

			LibraryItemId = queuesong?.Id ?? defaults?.Id;
			Details = DetailsQueueSong(queuesong, defaults);

			SetArtwork(queuesong ?? defaults);
		}
		void SetSong(ISong? song, ISong? defaults = null)
		{
			Reset();

			LibraryItemId = song?.Id ?? defaults?.Id;
			Details = DetailsSong(song, defaults);

			SetArtwork(song ?? defaults);
		}

		public static IEnumerable<string> DetailsArtist(IArtist? artist, IArtist? defaults = null)
		{
			string name = artist?.Name ?? defaults?.Name ?? string.Empty;
			string albumcount = (artist?.AlbumIds.Count() ?? defaults?.AlbumIds.Count() ?? 0).ToString("00");
			string songcount = (artist?.SongIds.Count() ?? defaults?.SongIds.Count() ?? 0).ToString("00");

			return Enumerable.Empty<string>()
				.Append(name)
				.Append(albumcount)
				.Append(songcount);
		}
		public static IEnumerable<string> DetailsArtistAlbum(IAlbum? artistalbum, IAlbum? defaults = null)
		{
			string title = artistalbum?.Title ?? defaults?.Title ?? string.Empty;
			string artistname = artistalbum?.Artist ?? defaults?.Artist ?? string.Empty;
			string songcount = (artistalbum?.SongIds.Count() ?? defaults?.SongIds.Count() ?? 0).ToString("00");
			string duration = (artistalbum?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(title)
				.Append(artistname)
				.Append(duration)
				.Append(songcount);
		}
		public static IEnumerable<string> DetailsArtistSong(ISong? artistsong, ISong? defaults = null)
		{
			string title = artistsong?.Title ?? defaults?.Title ?? string.Empty;
			string artist = (artistsong ?? defaults)?.Artist ?? string.Empty;
			string duration = (artistsong?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;
			string tracknumber = (artistsong?.TrackNumber ?? defaults?.TrackNumber)?.ToString("00") ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(title)
				.Append(artist)
				.Append(duration)
				.Append(tracknumber);
		}
		public static IEnumerable<string> DetailsAlbum(IAlbum? album, IAlbum? defaults = null)
		{
			string title = album?.Title ?? defaults?.Title ?? string.Empty;
			string artistname = album?.Artist ?? defaults?.Artist ?? string.Empty;
			string songcount = (album?.SongIds.Count() ?? defaults?.SongIds.Count() ?? 0).ToString("00");
			string duration = (album?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(title)
				.Append(artistname)
				.Append(duration)
				.Append(songcount);
		}
		public static IEnumerable<string> DetailsAlbumSong(ISong? albumsong, ISong? defaults = null)
		{
			string title = albumsong?.Title ?? defaults?.Title ?? string.Empty;
			string artist = albumsong?.Artist ?? defaults?.Artist ?? string.Empty;
			string duration = (albumsong?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;
			string tracknumber = (albumsong?.TrackNumber ?? defaults?.TrackNumber)?.ToString("00") ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(title)
				.Append(artist)
				.Append(duration)
				.Append(tracknumber);
		}
		public static IEnumerable<string> DetailsGenre(IGenre? genre, IGenre? defaults = null)
		{
			string name = genre?.Name ?? string.Empty;
			string songcount = (genre?.SongIds.Count() ?? defaults?.SongIds.Count() ?? 0).ToString("00");
			string duration = (genre?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(name)
				.Append(duration)
				.Append(songcount);
		}
		public static IEnumerable<string> DetailsGenreSong(ISong? genresong, ISong? defaults = null)
		{
			string title = genresong?.Title ?? defaults?.Title ?? string.Empty;
			string artist = genresong?.Artist ?? defaults?.Artist ?? string.Empty;
			string duration = (genresong?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;
			string tracknumber = (genresong?.TrackNumber ?? defaults?.TrackNumber)?.ToString("00") ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(title)
				.Append(artist)
				.Append(duration)
				.Append(tracknumber);
		}
		public static IEnumerable<string> DetailsPlaylist(IPlaylist? playlist, IPlaylist? defaults = null)
		{
			string name = playlist?.Name ?? defaults?.Name ?? string.Empty;
			string songcount = (playlist?.SongIds.Count() ?? defaults?.SongIds.Count() ?? 0).ToString("00");
			string duration = playlist?.Duration.ToMicrowaveFormat() ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(name)
				.Append(duration)
				.Append(songcount);
		}
		public static IEnumerable<string> DetailsPlaylistSong(ISong? playlistsong, ISong? defaults = null, int? position = null)
		{
			string title = playlistsong?.Title ?? defaults?.Title ?? string.Empty;
			string artist = playlistsong?.Artist ?? defaults?.Artist ?? string.Empty;
			string duration = (playlistsong?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(title)
				.Append(artist)
				.Append(duration)
				.Append(position?.ToString() ?? string.Empty);
		}
		public static IEnumerable<string> DetailsQueueSong(ISong? queuesong, ISong? defaults = null, int? position = null)
		{
			string title = queuesong?.Title ?? defaults?.Title ?? string.Empty;
			string artist = queuesong?.Artist ?? defaults?.Artist ?? string.Empty;
			string album = queuesong?.Album ?? defaults?.Album ?? string.Empty;
			string duration = (queuesong?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(title)
				.Append(string.Format("{0} - {1}", artist, album))
				.Append(duration)
				.Append(position?.ToString() ?? string.Empty);
		}
		public static IEnumerable<string> DetailsSong(ISong? song, ISong? defaults = null)
		{
			string title = song?.Title ?? defaults?.Title ?? string.Empty;
			string artist = song?.Artist ?? defaults?.Artist ?? string.Empty;
			string album = song?.Album ?? defaults?.Album ?? string.Empty;
			string duration = (song?.Duration ?? defaults?.Duration)?.ToMicrowaveFormat() ?? string.Empty;
			string tracknumber = (song?.TrackNumber ?? defaults?.TrackNumber)?.ToString("00") ?? string.Empty;

			return Enumerable.Empty<string>()
				.Append(title)
				.Append(string.Format("{0} - {1}", artist, album))
				.Append(duration)
				.Append(tracknumber);
		}		
	}
}
