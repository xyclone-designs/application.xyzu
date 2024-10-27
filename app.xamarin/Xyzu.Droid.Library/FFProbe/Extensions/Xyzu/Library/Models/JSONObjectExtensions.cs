using Org.Json;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Images;
using static Android.Icu.Text.CaseMap;

namespace Xyzu.Library.Models
{
	public static class JSONObjectExtensions
	{
		private static class Keys
		{
			public static string[] AlbumArtist = new string[] { "albumartist", "album_artist" };
			public static string[] DiscNumber = new string[] { "disc", "discnumber", "disc_number" };
			public static string[] ReleaseDate = new string[] { "date", "release", "releasedate", "release_date", };
			public static string[] Title = new string[] { "name", "title", };
			public static string[] TrackNumber = new string[] { "track", "tracknumber", "track_number" };

			public static string? Check(IEnumerable<string> names, params string[] keys)
			{
				return names.FirstOrDefault(_ => keys.Any(__ => string.Equals(_, __, StringComparison.OrdinalIgnoreCase)));
			}
		}

		public static IAlbum Retrieve(this IAlbum retrieved, JSONObject jsonobject, bool overwrite = false)
		{
			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, JSONObject jsonobject, bool overwrite = false)
		{
			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, JSONObject jsonobject, bool overwrite = false)
		{
			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, JSONObject jsonobject, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, JSONObject jsonobject, bool overwrite = false)
		{
			if (overwrite)
			{
				retrieved.Album = null;
				retrieved.AlbumArtist = null;
				retrieved.Artist = null;
				retrieved.Copyright = null;
				retrieved.DiscNumber = null;
				retrieved.Genre = null;
				retrieved.Lyrics = null;
				retrieved.ReleaseDate = null;
				retrieved.Title = null;
				retrieved.TrackNumber = null;
			}

			if (jsonobject.Names()?.AsEnumerable((_names, _index) =>
			{
				return _names.OptString(_index)?.Trim();

			}).OfType<string>() is not IEnumerable<string> names) return retrieved;

			if (retrieved.Album is null &&
				jsonobject.OptString(Keys.Check(names, nameof(ISong.Album))) is string album && 
				string.IsNullOrWhiteSpace(album) is false) retrieved.Album = album;

			if (retrieved.AlbumArtist is null &&
				jsonobject.OptString(Keys.Check(names, Keys.AlbumArtist)) is string albumartist &&
				string.IsNullOrWhiteSpace(albumartist) is false) retrieved.AlbumArtist = albumartist;

			if (retrieved.Artist is null &&
				jsonobject.OptString(Keys.Check(names, nameof(ISong.Artist))) is string artist &&
				string.IsNullOrWhiteSpace(artist) is false) retrieved.Artist = artist;

			if (retrieved.Copyright is null &&
				jsonobject.OptString(Keys.Check(names, nameof(ISong.Copyright))) is string copyright &&
				string.IsNullOrWhiteSpace(copyright) is false) retrieved.Copyright = copyright;

			if (retrieved.DiscNumber is null &&
				jsonobject.OptIntNumerator(Keys.Check(names, Keys.DiscNumber)) is int discnumber)
				retrieved.DiscNumber = discnumber;

			if (retrieved.Genre is null &&
				jsonobject.OptString(Keys.Check(names, nameof(ISong.Genre))) is string genre &&
				string.IsNullOrWhiteSpace(genre) is false) retrieved.Genre = genre;

			if (retrieved.Lyrics is null &&
				jsonobject.OptString(Keys.Check(names, nameof(ISong.Lyrics))) is string lyrics &&
				string.IsNullOrWhiteSpace(lyrics) is false) retrieved.Lyrics = lyrics;

			if (retrieved.ReleaseDate is null &&
				jsonobject.OptDateTime(Keys.Check(names, Keys.ReleaseDate)) is DateTime releasedate)
				retrieved.ReleaseDate = releasedate;

			if (retrieved.Title is null &&
				jsonobject.OptString(Keys.Check(names, Keys.Title)) is string title &&
				string.IsNullOrWhiteSpace(title) is false) retrieved.Title = title;

			if (retrieved.TrackNumber is null &&
				jsonobject.OptIntNumerator(Keys.Check(names, Keys.TrackNumber)) is int tracknumber)
				retrieved.TrackNumber = tracknumber;

			return retrieved;
		}
		public static IImage Retrieve(this IImage retrieved, JSONObject jsonobject)
		{
			return retrieved;
		}
	}
}
