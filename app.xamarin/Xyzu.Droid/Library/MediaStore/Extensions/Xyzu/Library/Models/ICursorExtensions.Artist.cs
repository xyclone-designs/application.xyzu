#nullable enable

using Android.Database;

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
		public static readonly IArtist<bool> ArtistCursorRetrieveTypes_Id = new IArtist.Default<bool>(false)
		{
			Id = true,
		};
		public static readonly IArtist<bool> ArtistCursorRetrieveTypes_Data = new IArtist.Default<bool>(false)
		{
			Id = true,
			AlbumIds = true,
			SongIds = true,
		};

		public static IEnumerable<string> Append(this IEnumerable<string> projection, IArtist<bool> retriever)
		{
			projection = projection.Append(MediaStoreUtils.ColumnNames.ArtistId);

			if (retriever.AlbumIds) projection = projection.Append(MediaStoreUtils.ColumnNames.AlbumId);
			if (retriever.Name) projection = projection.Append(MediaStoreUtils.ColumnNames.Artist);

			return projection.Distinct();
		}
		public static IEnumerable<string> ToProjection(this IArtist<bool> retriever)
		{
			return BaseProjection()
				.Append(retriever);
		}

		public static IArtist? Retrieve(this ICursor cursor, IArtist<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All)
		{
			IArtist? retrieved = null;

			if (cursor.GetArtistId() is string id)
				retrieved = new IArtist.Default(id);

			if (retrieved != null)
				retrieved.Retrieve(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => ArtistCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => ArtistCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever
				});

			return retrieved;
		}
		public static async Task<IArtist?> RetrieveAsync(this ICursor cursor, IArtist<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All, CancellationToken cancellationToken = default)
		{
			IArtist? retrieved = null;

			if (cursor.GetArtistId() is string id)
				retrieved = new IArtist.Default(id);

			if (retrieved != null)
				await retrieved.RetrieveAsync(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => ArtistCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => ArtistCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever

				}, false, cancellationToken);

			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, ICursor cursor, IArtist<bool>? retriever, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			return retrieved;
		}					
		public static async Task<IArtist> RetrieveAsync(this IArtist retrieved, ICursor cursor, IArtist<bool>? retriever, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			return await Task.FromResult(retrieved);
		}		  

		private static IArtist RetrieveSync(this IArtist retrieved, ICursor cursor, IArtist<bool>? retriever, bool overwrite = false)
		{
			if (
				(retriever?.Name ?? true) &&
				(retrieved.Name is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Artist, null) is string name)
				retrieved.Name = name;

			return retrieved;
		}
	}
}
