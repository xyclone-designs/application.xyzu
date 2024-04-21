#nullable enable

using Android.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;
using Xyzu.Library.MediaStore;

using SystemUri = System.Uri;

namespace Xyzu.Library.Models
{
	public static partial class ICursorExtensions
	{
		public static readonly ISong<bool> SongCursorRetrieveTypes_Id = new ISong.Default<bool>(false)
		{
			Id = true,
		};
		public static readonly ISong<bool> SongCursorRetrieveTypes_Data = new ISong.Default<bool>(false)
		{
			Id = true,
			Filepath = true,
			Uri = true,
		};

		public static IEnumerable<string> Append(this IEnumerable<string> projection, ISong<bool> retriever) 
		{
			if (retriever.Album) projection = projection.Append(MediaStoreUtils.ColumnNames.Album);
			if (retriever.AlbumArtist) projection = projection.Append(MediaStoreUtils.ColumnNames.AlbumArtist);
			if (retriever.Artist) projection = projection.Append(MediaStoreUtils.ColumnNames.Artist);
			if (retriever.Artwork != null && (retriever.Artwork.Buffer || retriever.Artwork.BufferHash || retriever.Artwork.Uri)) projection = projection.Append(MediaStoreUtils.ColumnNames.AlbumId);
			if (retriever.DateAdded) projection = projection.Append(MediaStoreUtils.ColumnNames.DateAdded);
			if (retriever.DateModified) projection = projection.Append(MediaStoreUtils.ColumnNames.DateModified);
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

		public static ISong? Retrieve(this ICursor cursor, ISong<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All)
		{
			ISong? retrieved = null;

			if (cursor.GetId() is string id)
				retrieved = new ISong.Default(id);

			if (retrieved != null)
				retrieved.Retrieve(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => SongCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => SongCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever
				});

			return retrieved;
		}
		public async static Task<ISong?> RetrieveAsync(this ICursor cursor, ISong<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All, CancellationToken cancellationToken = default)
		{
			ISong? retrieved = null;

			if (cursor.GetId() is string id)
				retrieved = new ISong.Default(id);

			if (retrieved != null)
				await retrieved.RetrieveAsync(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => SongCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => SongCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever

				}, false, cancellationToken);

			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, ICursor cursor, ISong<bool>? retriever, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			if (
				(retriever?.Filepath ?? true) &&
				retrieved.Filepath is null &&
				cursor.GetFilepath() is string filepath)
				retrieved.Filepath = filepath;

			if (
				(retriever?.Uri ?? true) &&
				retrieved.Uri is null &&
				cursor.GetUri() is SystemUri systemuri)
				retrieved.Uri = systemuri;

			return retrieved;
		}		   
		public static async Task<ISong> RetrieveAsync(this ISong retrieved, ICursor cursor, ISong<bool>? retriever, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			if (
				cancellationToken.IsCancellationRequested is false &&
				(retriever?.Filepath ?? true) &&
				retrieved.Filepath is null &&
				await cursor.GetFilepathAsync(cancellationToken) is string filepath)
				retrieved.Filepath = filepath;
			
			if (
				cancellationToken.IsCancellationRequested is false && 
				(retriever?.Uri ?? true) &&
				retrieved.Uri is null &&
				await cursor.GetUriAsync(cancellationToken) is SystemUri systemuri)
				retrieved.Uri = systemuri;

			return retrieved;
		}  

		private static ISong RetrieveSync(this ISong retrieved, ICursor cursor, ISong<bool>? retriever, bool overwrite = false)
		{
			if (
				(retriever?.Album ?? true) &&
				(retrieved.Album is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Album, null) is string album)
				retrieved.Album = album;													

			if (
				(retriever?.AlbumArtist ?? true) &&
				(retrieved.AlbumArtist is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.AlbumArtist, null) is string albumartist)
				retrieved.AlbumArtist = albumartist;

			if (
				(retriever?.Artist ?? true) &&
				(retrieved.Artist is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Artist, null) is string artist)
				retrieved.Artist = artist;

			if (
				(retriever?.Duration ?? true) &&
				(retrieved.Duration is null || overwrite) &&
				cursor.GetColumnValue<int?>(MediaStoreUtils.ColumnNames.Duration, null) is int duration)
				retrieved.Duration = TimeSpan.FromMilliseconds(duration);

			if (
				(retriever?.ReleaseDate ?? true) &&
				(retrieved.ReleaseDate is null || overwrite) &&
				cursor.GetColumnValue<DateTime?>(MediaStoreUtils.ColumnNames.Year, null) is DateTime releasedate)
				retrieved.ReleaseDate = releasedate;

			if (
				(retriever?.Title ?? true) &&
				(retrieved.Title is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Title, null) is string title)
				retrieved.Title = title;						

			if (
				(retriever?.Size ?? true) &&
				(retrieved.Size is null || overwrite) &&
				cursor.GetColumnValue<int?>(MediaStoreUtils.ColumnNames.Size, null) is int size)
				retrieved.Size = size;													 

			if (
				(retriever?.TrackNumber ?? true) &&
				(retrieved.TrackNumber is null || overwrite) &&
				cursor.GetColumnValue<int?>(MediaStoreUtils.ColumnNames.Track, null) is int tracknumber)
				retrieved.TrackNumber = tracknumber;

			return retrieved;
		}
	}
}
