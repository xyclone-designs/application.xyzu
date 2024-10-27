using Android.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;
using Xyzu.Library.MediaStore;

using SystemUri = System.Uri;
using JavaFile = Java.IO.File;

namespace Xyzu.Library.Models
{
	public static partial class ICursorExtensions
	{
		public static IEnumerable<string> Append(this IEnumerable<string> projection, ISong<bool> retriever) 
		{
			if (retriever.Album) projection = projection.Append(MediaStoreUtils.ColumnNames.Album);
			if (retriever.AlbumArtist) projection = projection.Append(MediaStoreUtils.ColumnNames.AlbumArtist);
			if (retriever.Artist) projection = projection.Append(MediaStoreUtils.ColumnNames.Artist);
			if (retriever.Artwork != null && (retriever.Artwork.Buffer || retriever.Artwork.BufferKey || retriever.Artwork.Uri)) projection = projection.Append(MediaStoreUtils.ColumnNames.AlbumId);
			if (retriever.Duration) projection = projection.Append(MediaStoreUtils.ColumnNames.Duration);
			if (retriever.MimeType) projection = projection.Append(MediaStoreUtils.ColumnNames.MimeType);
			if (retriever.ReleaseDate) projection = projection.Append(MediaStoreUtils.ColumnNames.Year);
			if (retriever.Size) projection = projection.Append(MediaStoreUtils.ColumnNames.Size);
			if (retriever.Title) projection = projection.Append(MediaStoreUtils.ColumnNames.Title);
			if (retriever.TrackNumber) projection = projection.Append(MediaStoreUtils.ColumnNames.Track);

			return projection.Distinct();
		}
		public static IEnumerable<string> ToProjection(this ISong<bool> retriever)
		{
			return BaseProjection()
				.Append(retriever);
		}

		public static ISong Retrieve(this ISong retrieved, ICursor cursor, JavaFile? directory, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			if (retrieved.Filepath is null &&
				cursor.GetFilepath(directory) is string filepath)
				retrieved.Filepath = filepath;

			if (retrieved.Uri is null &&
				cursor.GetUri(directory) is SystemUri systemuri)
				retrieved.Uri = systemuri;

			return retrieved;
		}		   
		public static async Task<ISong> RetrieveAsync(this ISong retrieved, ICursor cursor, JavaFile? directory, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			if (cancellationToken.IsCancellationRequested is false &&
				retrieved.Filepath is null &&
				await cursor.GetFilepathAsync(directory, cancellationToken) is string filepath)
				retrieved.Filepath = filepath;
			
			if (cancellationToken.IsCancellationRequested is false && 
				retrieved.Uri is null &&
				await cursor.GetUriAsync(directory) is SystemUri systemuri)
				retrieved.Uri = systemuri;

			return retrieved;
		}  

		private static ISong RetrieveSync(this ISong retrieved, ICursor cursor, bool overwrite = false)
		{
			if ((retrieved.Album is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Album, null) is string album)
				retrieved.Album = album;													

			if ((retrieved.AlbumArtist is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.AlbumArtist, null) is string albumartist)
				retrieved.AlbumArtist = albumartist;

			if ((retrieved.Artist is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Artist, null) is string artist)
				retrieved.Artist = artist;

			if ((retrieved.Duration is null || overwrite) &&
				cursor.GetColumnValue<int?>(MediaStoreUtils.ColumnNames.Duration, null) is int duration)
				retrieved.Duration = TimeSpan.FromMilliseconds(duration);

			if ((retrieved.ReleaseDate is null || overwrite) &&
				cursor.GetColumnValue<DateTime?>(MediaStoreUtils.ColumnNames.Year, null) is DateTime releasedate)
				retrieved.ReleaseDate = releasedate;

			if ((retrieved.Title is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Title, null) is string title)
				retrieved.Title = title;						

			if ((retrieved.Size is null || overwrite) &&
				cursor.GetColumnValue<int?>(MediaStoreUtils.ColumnNames.Size, null) is int size)
				retrieved.Size = size;													 

			if ((retrieved.TrackNumber is null || overwrite) &&
				cursor.GetColumnValue<int?>(MediaStoreUtils.ColumnNames.Track, null) is int tracknumber)
				retrieved.TrackNumber = tracknumber;

			return retrieved;
		}
	}
}
