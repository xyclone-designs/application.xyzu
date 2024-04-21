#nullable enable

using System;

using Xyzu.Library.MediaStore;
using Xyzu.Library.Models;

namespace Android.Database
{
	public static partial class ICursorExtensions
	{
		public static string? GetAlbumId(this ICursor cursor)
		{
			return cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.AlbumId, null);
		}
		public static string? GetArtistId(this ICursor cursor)
		{
			return cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.ArtistId, null);
		}							
		public static string? GetGenreId(this ICursor cursor)
		{
			return null;
		}
		public static string? GetId(this ICursor cursor)
		{
			return cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Id, null);
		}							
		public static string? GetPlaylistId(this ICursor cursor)
		{
			return null;
		}

		public static T GetColumnValue<T>(this ICursor cursor, int? columnindex, T defaultvalue)
		{
			if (columnindex is null || columnindex is -1)
				return defaultvalue;

			Type type = typeof(T);
			object? value = null;

			try
			{
				value = true switch
				{
					true when type == typeof(byte[]) => cursor.GetBlob(columnindex.Value),
					true when type == typeof(double) || type == typeof(double?) => cursor.GetDouble(columnindex.Value),
					true when type == typeof(float) || type == typeof(float?) => cursor.GetFloat(columnindex.Value),
					true when type == typeof(int) || type == typeof(int?) => cursor.GetInt(columnindex.Value),
					true when type == typeof(long) || type == typeof(long?) => cursor.GetLong(columnindex.Value),
					true when type == typeof(short) || type == typeof(short?) => cursor.GetShort(columnindex.Value),
					true when type == typeof(string) => cursor.GetString(columnindex.Value),
					true when type == typeof(DateTime) || type == typeof(DateTime?) => cursor.GetType(columnindex.Value) switch
					{
						FieldType.Integer => cursor.GetLong(columnindex.Value) is long longdatetime
							? new DateTime(longdatetime)
							: new DateTime?(),
						FieldType.String => DateTime.TryParse(cursor.GetString(columnindex.Value) ?? string.Empty, out DateTime outdatetime)
							? outdatetime
							: new DateTime?(),

						_ => new DateTime?(),

					},
					true when type == typeof(Uri) &&
						cursor.GetString(columnindex.Value) is string uristring => Uri.TryCreate(uristring, UriKind.RelativeOrAbsolute, out Uri outuri)
						? outuri
						: null,

					_ => null,
				};
			}
			// TODO BUG: Get Index Out Of Bounds For Absolutely ''''''''No'''''''' Reason. Sometimes.
			catch (CursorIndexOutOfBoundsException exception)
			{
				Util.Log.Error(
					tr: exception,
					tag: "Android.Database.ICursorExtensions",
					format: "exception.Message: {0}, columnindex: {1}, defaultvalue: {2}",
					args: new object[] { exception.Message ?? "No Message", columnindex, defaultvalue?.ToString() ?? "null" });
			}

			if (value != null)
				return (T)value;

			return defaultvalue;
		}
		public static T GetColumnValue<T>(this ICursor cursor, string? columnname, T defaultvalue) 
		{
			int columnindex = cursor.GetColumnIndex(columnname);

			return columnindex switch
			{
				-1 => defaultvalue,

				_ => cursor.GetColumnValue<T>(columnindex, defaultvalue),
			};
		}

		public static bool ShouldRetrieveAlbum(this ICursor cursor, IAlbum album, string? albumid = null)
		{
			return string.Equals(albumid ?? cursor.GetAlbumId(), album.Id, StringComparison.OrdinalIgnoreCase);
		}
		public static bool ShouldRetrieveArtist(this ICursor cursor, IArtist artist, string? artistid = null)
		{
			return string.Equals(artistid ?? cursor.GetArtistId(), artist.Id, StringComparison.OrdinalIgnoreCase);
		}
		public static bool ShouldRetrieveGenre(this ICursor cursor, IGenre genre, string? genreid = null)
		{
			return string.Equals(genreid ?? cursor.GetGenreId(), genre.Id, StringComparison.OrdinalIgnoreCase);
		}
		public static bool ShouldRetrievePlaylist(this ICursor cursor, IPlaylist playlist, string? playlistid = null)
		{
			return string.Equals(playlistid ?? cursor.GetPlaylistId(), playlist.Id, StringComparison.OrdinalIgnoreCase);
		}
		public static bool ShouldRetrieveSong(this ICursor cursor, ISong song, string? songid = null)
		{
			return string.Equals(songid ?? cursor.GetId(), song.Id, StringComparison.OrdinalIgnoreCase);
		}
	}
}