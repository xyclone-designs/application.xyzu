using Android.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.MediaStore;

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
		public static async Task<(string?, SystemUri?)> GetFilepathAndUriAsync(this ICursor cursor, JavaFile? directory, CancellationToken cancellationToken = default)
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
		public static async Task<string?> GetFilepathAsync(this ICursor cursor, JavaFile? directory, CancellationToken cancellationToken = default)
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
	}
}
