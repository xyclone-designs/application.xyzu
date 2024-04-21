#nullable enable

using Android.Content;
using Android.Database;
using Android.Net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.MediaStore;

using AndroidUri = Android.Net.Uri;
using AndroidEnvironment = Android.OS.Environment;
using JavaFile = Java.IO.File;
using SystemUri = System.Uri;

namespace Xyzu.Library.Models
{
	public static partial class ICursorExtensions
	{
		public static IEnumerable<string> BaseProjection()
		{
			return Enumerable.Empty<string>()
				.Append(MediaStoreUtils.ColumnNames.Id)
				.Append(MediaStoreUtils.ColumnNames.DisplayName)
				.Append(MediaStoreUtils.ColumnNames.RelativePath);
		}

		public static (string?, SystemUri?) GetFilepathAndUri(this ICursor cursor)
		{
			string? displayname = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.DisplayName, null);
			string? relativepath = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.RelativePath, null);

			if (string.IsNullOrWhiteSpace(displayname) is false && string.IsNullOrWhiteSpace(relativepath) is false && AndroidEnvironment.StorageDirectory.ListFiles() is IEnumerable<JavaFile> javafiles)
				foreach (string path in javafiles.Select(javafile => Path.Combine(javafile.AbsolutePath, relativepath, displayname)))
					if (File.Exists(path))
						return
						(
							path,
							SystemUri.TryCreate(path, UriKind.RelativeOrAbsolute, out SystemUri outuri)
								? outuri
								: null
						);

			return (null, null);
		}			  
		public static async Task<(string?, SystemUri?)> GetFilepathAndUriAsync(this ICursor cursor, CancellationToken cancellationToken = default)
		{
			string? displayname = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.DisplayName, null);
			string? relativepath = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.RelativePath, null);

			if (string.IsNullOrWhiteSpace(displayname) is false && string.IsNullOrWhiteSpace(relativepath) is false && await AndroidEnvironment.StorageDirectory.ListFilesAsync() is IEnumerable<JavaFile> javafiles)
				foreach (string path in javafiles.Select(javafile => Path.Combine(javafile.AbsolutePath, relativepath, displayname)))
					if (File.Exists(path))
						return
						(
							path,
							SystemUri.TryCreate(path, UriKind.RelativeOrAbsolute, out SystemUri outuri)
								? outuri
								: null
						);

			return (null, null);
		}
		public static string? GetFilepath(this ICursor cursor)
		{
			string? filepath = null;

			string? displayname = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.DisplayName, null);
			string? relativepath = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.RelativePath, null);

			if (string.IsNullOrWhiteSpace(displayname) is false && string.IsNullOrWhiteSpace(relativepath) is false && AndroidEnvironment.StorageDirectory.ListFiles() is IEnumerable<JavaFile> javafiles)
				foreach (JavaFile javafile in javafiles)
					if (filepath is null)
					{
						filepath = Path.Combine(javafile.AbsolutePath, relativepath, displayname);
							
						if (File.Exists(filepath) is false)
							filepath = null;
					}

			return filepath;
		}				  
		public static async Task<string?> GetFilepathAsync(this ICursor cursor, CancellationToken cancellationToken = default)
		{
			string? filepath = null;

			string? displayname = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.DisplayName, null);
			string? relativepath = cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.RelativePath, null);

			if (string.IsNullOrWhiteSpace(displayname) is false && string.IsNullOrWhiteSpace(relativepath) is false && await AndroidEnvironment.StorageDirectory.ListFilesAsync() is IEnumerable<JavaFile> javafiles)
				foreach (JavaFile javafile in javafiles)
					if (filepath is null)
					{
						filepath = Path.Combine(javafile.AbsolutePath, relativepath, displayname);

						if (File.Exists(filepath) is false)
							filepath = null;
					}

			return filepath;
		}			  
		public static SystemUri? GetUri(this ICursor cursor)
		{
			if (cursor.GetFilepath() is string filepath)
				return SystemUri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out SystemUri outuri) ? outuri : null;

			return null;
		}				  
		public static async Task<SystemUri?> GetUriAsync(this ICursor cursor, CancellationToken cancellationToken = default)
		{
			if (await cursor.GetFilepathAsync() is string filepath)
				return SystemUri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out SystemUri outuri) ? outuri : null;

			return null;
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
