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

		public static IPlaylist Retrieve(this IPlaylist retrieved, ICursor cursor, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			return retrieved;
		}		  
		public static async Task<IPlaylist> RetrieveAsync(this IPlaylist retrieved, ICursor cursor, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			return await Task.FromResult(retrieved);
		}  

		private static IPlaylist RetrieveSync(this IPlaylist retrieved, ICursor cursor, bool overwrite = false)
		{
			if ((retrieved.DateCreated == default || overwrite) &&
				cursor.GetColumnValue<DateTime?>(MediaStoreUtils.ColumnNames.DateAdded, null) is DateTime datecreated)
				retrieved.DateCreated = datecreated;
													  
			if ((retrieved.DateModified is null || overwrite) &&
				cursor.GetColumnValue<DateTime?>(MediaStoreUtils.ColumnNames.DateModified, null) is DateTime datemodified)
				retrieved.DateModified = datemodified;

			return retrieved;
		}
	}
}
