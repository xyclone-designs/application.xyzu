#nullable enable

using Android.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;
using Xyzu.Library.MediaStore;

namespace Xyzu.Library.Models
{
	public static partial class ICursorExtensions
	{
		public static IEnumerable<string> Append(this IEnumerable<string> projection, IAlbum<bool> retriever)
		{
			projection = projection.Append(MediaStoreUtils.ColumnNames.AlbumId);

			if (retriever.ArtistId) projection = projection.Append(MediaStoreUtils.ColumnNames.ArtistId);
			if (retriever.Artist) projection = projection.Append(MediaStoreUtils.ColumnNames.AlbumArtist);
			if (retriever.Artwork != null && (retriever.Artwork.Buffer || retriever.Artwork.BufferHash || retriever.Artwork.Uri)) projection = projection.Append(MediaStoreUtils.ColumnNames.AlbumId);
			if (retriever.ReleaseDate) projection = projection.Append(MediaStoreUtils.ColumnNames.Year);
			if (retriever.Title) projection = projection.Append(MediaStoreUtils.ColumnNames.Album);

			return projection.Distinct();
		}
		public static IEnumerable<string> ToProjection(this IAlbum<bool> retriever)
		{
			return BaseProjection()
				.Append(retriever);
		}

		public static IAlbum Retrieve(this IAlbum retrieved, ICursor cursor, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			return retrieved;
		}					 
		public static async Task<IAlbum> RetrieveAsync(this IAlbum retrieved, ICursor cursor, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			return await Task.FromResult(retrieved);
		}

		private static IAlbum RetrieveSync(this IAlbum retrieved, ICursor cursor, bool overwrite = false)
		{
			if ((retrieved.Artist is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.AlbumArtist, null) is string albumartist)
				retrieved.Artist = albumartist;

			if ((retrieved.ReleaseDate is null || overwrite) &&
				cursor.GetColumnValue<DateTime?>(MediaStoreUtils.ColumnNames.Year, null) is DateTime releasedate)
				retrieved.ReleaseDate = releasedate;																	  

			if ((retrieved.Title is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Album, null) is string title)
				retrieved.Title = title;

			return retrieved;
		}
	}
}
