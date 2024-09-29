using Android.Content;
using Android.Net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.MediaStore;
using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;
using JavaFile = Java.IO.File;
using SystemUri = System.Uri;

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
					true when type == typeof(SystemUri) &&
						cursor.GetString(columnindex.Value) is string uristring => SystemUri.TryCreate(uristring, UriKind.RelativeOrAbsolute, out SystemUri? outuri)
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

		public static async Task<int?> ToPosition(this ICursor? cursor, JavaFile? directory, string? filepath, SystemUri? uri, IDictionary<string, int>? cursorpositions)
		{
			if (cursor is null)
				return null;

			string? key = filepath ?? uri?.ToString();
			int? cursorposition =
				cursorpositions is null
					? new int?()
				: key != null && cursorpositions.TryGetValue(key, out int keyposition)
					? keyposition
				: Math.Max(0, Math.Min(cursorpositions.Count, cursor.Count) - 1);

			if (cursor.MoveToPosition(cursorposition ?? 0))
				do
				{
					(string? cursorfilepath, SystemUri? cursoruri) = await cursor.GetFilepathAndUriAsync(directory);

					if ((cursorfilepath ?? cursoruri?.ToString()) is string cursorkey)
					{
						cursorpositions?.TryAdd(cursorkey, cursor.Position);

						if (cursorkey == key)
						{
							cursorposition = cursor.Position;

							break;
						}
					}

				} while (cursor.MoveToNext());

			return cursorposition;
		}
	
		public static IEnumerable<string> BaseProjection()
		{
			return Enumerable.Empty<string>()
				.Append(MediaStoreUtils.ColumnNames.Id)
				.Append(MediaStoreUtils.ColumnNames.DisplayName)
				.Append(MediaStoreUtils.ColumnNames.RelativePath);
		}

		public static string? GetFilepath(this ICursor cursor, JavaFile? directory)
		{
			string? filepath = null;

			string? displayname = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.DisplayName, null);
			string? relativepath = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.RelativePath, null);

			if (string.IsNullOrWhiteSpace(displayname) is false && 
				string.IsNullOrWhiteSpace(relativepath) is false &&
				directory?.ListFiles() is IEnumerable<JavaFile> javafiles)
				foreach (JavaFile javafile in javafiles)
					if (filepath is null)
					{
						filepath = Path.Combine(javafile.AbsolutePath, relativepath, displayname);
							
						if (File.Exists(filepath) is false)
							filepath = null;
					}

			return filepath;
		}				  
		public static async Task<string?> GetFilepathAsync(this ICursor cursor, JavaFile? directory)
		{
			string? filepath = null;

			string? displayname = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.DisplayName, null);
			string? relativepath = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.RelativePath, null);

			if (string.IsNullOrWhiteSpace(displayname) is false && 
				string.IsNullOrWhiteSpace(relativepath) is false &&
				directory is not null &&
				await directory.ListFilesAsync() is IEnumerable<JavaFile> javafiles)
				foreach (JavaFile javafile in javafiles)
					if (filepath is null)
					{
						filepath = Path.Combine(javafile.AbsolutePath, relativepath, displayname);

						if (File.Exists(filepath) is false)
							filepath = null;
					}

			return filepath;
		}			  
		public static SystemUri? GetUri(this ICursor cursor, JavaFile? directory)
		{
			if (cursor.GetFilepath(directory) is string filepath)
				return SystemUri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out SystemUri? outuri) ? outuri : null;

			return null;
		}				  
		public static async Task<SystemUri?> GetUriAsync(this ICursor cursor, JavaFile? directory)
		{
			if (await cursor.GetFilepathAsync(directory) is string filepath)
				return SystemUri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out SystemUri? outuri) ? outuri : null;

			return null;
		}
		public static (string?, SystemUri?) GetFilepathAndUri(this ICursor cursor, JavaFile? directory)
		{
			string? displayname = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.DisplayName, null);
			string? relativepath = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.RelativePath, null);

			if (string.IsNullOrWhiteSpace(displayname) is false &&
				string.IsNullOrWhiteSpace(relativepath) is false &&
				directory?.ListFiles() is IEnumerable<JavaFile> javafiles)
				foreach (string path in javafiles.Select(javafile => Path.Combine(javafile.AbsolutePath, relativepath, displayname)))
					if (File.Exists(path))
						return
						(
							path,
							SystemUri.TryCreate(path, UriKind.RelativeOrAbsolute, out SystemUri? outuri)
								? outuri
								: null
						);

			return (null, null);
		}
		public static async Task<(string?, SystemUri?)> GetFilepathAndUriAsync(this ICursor cursor, JavaFile? directory)
		{
			string? displayname = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.DisplayName, null);
			string? relativepath = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.RelativePath, null);

			if (string.IsNullOrWhiteSpace(displayname) is false &&
				string.IsNullOrWhiteSpace(relativepath) is false &&
				directory is not null &&
				await directory.ListFilesAsync() is IEnumerable<JavaFile> javafiles)
				foreach (string path in javafiles.Select(javafile => Path.Combine(javafile.AbsolutePath, relativepath, displayname)))
					if (File.Exists(path))
						return
						(
							path,
							SystemUri.TryCreate(path, UriKind.RelativeOrAbsolute, out SystemUri? outuri)
								? outuri
								: null
						);

			return (null, null);
		}

		public static IImage Retrieve(this IImage retrieved, ICursor cursor, Context context, IImage<bool>? retriever)
		{
			if (retriever is null || (retriever.Uri && retrieved.Uri is null))
				if (long.TryParse(cursor.GetAlbumId(), out long albumid) && MediaStoreUtils.Uris.SongArtwork(albumid) is AndroidUri androiduri)
					try
					{
						context.ContentResolver?.OpenFileDescriptor(androiduri, "r");

						if (retrieved.Uri is null && androiduri.ToSystemUri() is SystemUri artworksystemuri)
							retrieved.Uri = artworksystemuri;
					}
					catch (Exception) { }

			return retrieved;
		}
	}
}