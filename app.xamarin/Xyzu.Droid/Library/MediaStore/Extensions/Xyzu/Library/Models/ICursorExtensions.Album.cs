#nullable enable

using Android.App;
using Android.Database;
using Android.Net;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;
using Xyzu.Library.MediaStore;

using AndroidUri = Android.Net.Uri;
using SystemUri = System.Uri;

namespace Xyzu.Library.Models
{
	public static partial class ICursorExtensions
	{
		public static readonly IAlbum<bool> AlbumCursorRetrieveTypes_Id = new IAlbum.Default<bool>(false)
		{
			Id = true,
		};
		public static readonly IAlbum<bool> AlbumCursorRetrieveTypes_Data = new IAlbum.Default<bool>(false)
		{
			Id = true,
			SongIds = true,
		};

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

		public static IAlbum? Retrieve(this ICursor cursor, IAlbum<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All)
		{
			IAlbum? retrieved = null;

			if (cursor.GetAlbumId() is string id)
				retrieved = new IAlbum.Default(id);

			if (retrieved != null)
				retrieved.Retrieve(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => AlbumCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => AlbumCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever
				});

			return retrieved;
		}										   
		public static async Task<IAlbum?> RetrieveAsync(this ICursor cursor, IAlbum<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All, CancellationToken cancellationToken = default)
		{
			IAlbum? retrieved = null;

			if (cursor.GetAlbumId() is string id)
				retrieved = new IAlbum.Default(id);

			if (retrieved != null)
				await retrieved.RetrieveAsync(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => AlbumCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => AlbumCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever

				}, false, cancellationToken);

			return retrieved;
		}
		public static IAlbum Retrieve(this IAlbum retrieved, ICursor cursor, IAlbum<bool>? retriever, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			return retrieved;
		}					 
		public static async Task<IAlbum> RetrieveAsync(this IAlbum retrieved, ICursor cursor, IAlbum<bool>? retriever, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			return await Task.FromResult(retrieved);
		}

		private static IAlbum RetrieveSync(this IAlbum retrieved, ICursor cursor, IAlbum<bool>? retriever, bool overwrite = false)
		{
			if (
				(retriever?.Artist ?? true) &&
				(retrieved.Artist is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.AlbumArtist, null) is string albumartist)
				retrieved.Artist = albumartist;

			if (retriever?.DiscCount ?? true)
				retrieved.DiscCount = Math.Max(retrieved.DiscCount ?? 1, 1);

			if (
				(retriever?.ReleaseDate ?? true) &&
				(retrieved.ReleaseDate is null || overwrite) &&
				cursor.GetColumnValue<DateTime?>(MediaStoreUtils.ColumnNames.Year, null) is DateTime releasedate)
				retrieved.ReleaseDate = releasedate;																	  

			if (
				(retriever?.Title ?? true) &&
				(retrieved.Title is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Album, null) is string title)
				retrieved.Title = title;

			return retrieved;
		}
	}
}
