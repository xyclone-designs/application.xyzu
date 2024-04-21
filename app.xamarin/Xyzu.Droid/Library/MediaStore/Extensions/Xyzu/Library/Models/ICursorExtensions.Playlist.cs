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
		public static readonly IPlaylist<bool> PlaylistCursorRetrieveTypes_Id = new IPlaylist.Default<bool>(false)
		{
			Id = true,
		};
		public static readonly IPlaylist<bool> PlaylistCursorRetrieveTypes_Data = new IPlaylist.Default<bool>(false)
		{
			Id = true,
			SongIds = true,
		};

		public static IEnumerable<string> Append(this IEnumerable<string> projection, IPlaylist<bool> retriever)
		{
			if (retriever.DateCreated) projection = projection.Append(MediaStoreUtils.ColumnNames.DateAdded);
			if (retriever.DateModified) projection = projection.Append(MediaStoreUtils.ColumnNames.DateModified);

			return projection.Distinct();
		}
		public static IEnumerable<string> ToProjection(this IPlaylist<bool> retriever)
		{
			return BaseProjection()
				.Append(retriever);
		}

		public static IPlaylist? Retrieve(this ICursor cursor, IPlaylist<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All)
		{
			IPlaylist? retrieved = null;

			if (cursor.GetPlaylistId() is string id)
				retrieved = new IPlaylist.Default(id);

			if (retrieved != null)
				retrieved.Retrieve(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => PlaylistCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => PlaylistCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever
				});

			return retrieved;
		}
		public static async Task<IPlaylist?> RetrieveAsync(this ICursor cursor, IPlaylist<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All, CancellationToken cancellationToken = default)
		{
			IPlaylist? retrieved = null;

			if (cursor.GetPlaylistId() is string id)
				retrieved = new IPlaylist.Default(id);

			if (retrieved != null)
				await retrieved.RetrieveAsync(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => PlaylistCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => PlaylistCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever

				}, false, cancellationToken);

			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, ICursor cursor, IPlaylist<bool>? retriever, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			return retrieved;
		}		  
		public static async Task<IPlaylist> RetrieveAsync(this IPlaylist retrieved, ICursor cursor, IPlaylist<bool>? retriever, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			return await Task.FromResult(retrieved);
		}  

		private static IPlaylist RetrieveSync(this IPlaylist retrieved, ICursor cursor, IPlaylist<bool>? retriever, bool overwrite = false)
		{
			if (
				(retriever?.DateCreated ?? true) &&
				(retrieved.DateCreated == default || overwrite) &&
				cursor.GetColumnValue<DateTime?>(MediaStoreUtils.ColumnNames.DateAdded, null) is DateTime datecreated)
				retrieved.DateCreated = datecreated;
													  
			if (
				(retriever?.DateModified ?? true) &&
				(retrieved.DateModified is null || overwrite) &&
				cursor.GetColumnValue<DateTime?>(MediaStoreUtils.ColumnNames.DateModified, null) is DateTime datemodified)
				retrieved.DateModified = datemodified;

			return retrieved;
		}
	}
}
