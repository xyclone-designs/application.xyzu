#nullable enable

using Android.Database;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xyzu.Library.Models
{
	public static partial class ICursorExtensions
	{
		public static readonly IGenre<bool> GenreCursorRetrieveTypes_Id = new IGenre.Default<bool>(false)
		{
			Id = true,
		};
		public static readonly IGenre<bool> GenreCursorRetrieveTypes_Data = new IGenre.Default<bool>(false)
		{
			Id = true,
			SongIds = true,
		};

		public static IEnumerable<string> Append(this IEnumerable<string> projection, IGenre<bool> retriever)
		{
			return projection.Distinct();
		}
		public static IEnumerable<string> ToProjection(this IGenre<bool> retriever)
		{
			return BaseProjection()
				.Append(retriever);
		}

		public static IGenre? Retrieve(this ICursor cursor, IGenre<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All)
		{
			IGenre? retrieved = null;

			if (cursor.GetGenreId() is string id)
				retrieved = new IGenre.Default(id);

			if (retrieved != null)
				retrieved.Retrieve(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => GenreCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => GenreCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever
				});

			return retrieved;
		}
		public static async Task<IGenre?> RetrieveAsync(this ICursor cursor, IGenre<bool>? retriever, CursorRetrieveTypes retrievetype = CursorRetrieveTypes.All, CancellationToken cancellationToken = default)
		{
			IGenre? retrieved = null;

			if (cursor.GetGenreId() is string id)
				retrieved = new IGenre.Default(id);

			if (retrieved != null)
				await retrieved.RetrieveAsync(cursor, retrievetype switch
				{
					CursorRetrieveTypes.Id => GenreCursorRetrieveTypes_Id,
					CursorRetrieveTypes.Data => GenreCursorRetrieveTypes_Data,
					CursorRetrieveTypes.All => retriever,

					_ => retriever

				}, false, cancellationToken);

			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, ICursor cursor, IGenre<bool>? retriever, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			return retrieved;
		}				
		public static async Task<IGenre> RetrieveAsync(this IGenre retrieved, ICursor cursor, IGenre<bool>? retriever, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, retriever, overwrite);

			return await Task.FromResult(retrieved);
		}

		private static IGenre RetrieveSync(this IGenre retrieved, ICursor cursor, IGenre<bool>? retriever, bool overwrite = false)
		{
			return retrieved;
		}
	}
}
