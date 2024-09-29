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

		public static IArtist Retrieve(this IArtist retrieved, ICursor cursor, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			return retrieved;
		}					
		public static async Task<IArtist> RetrieveAsync(this IArtist retrieved, ICursor cursor, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			return await Task.FromResult(retrieved);
		}		  

		private static IArtist RetrieveSync(this IArtist retrieved, ICursor cursor, bool overwrite = false)
		{
			if ((retrieved.Name is null || overwrite) &&
				cursor.GetColumnValue<string?>(MediaStoreUtils.ColumnNames.Artist, null) is string name)
				retrieved.Name = name;

			return retrieved;
		}
	}
}
