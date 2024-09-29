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
		public static IEnumerable<string> Append(this IEnumerable<string> projection, IGenre<bool> retriever)
		{
			return projection.Distinct();
		}
		public static IEnumerable<string> ToProjection(this IGenre<bool> retriever)
		{
			return BaseProjection()
				.Append(retriever);
		}

		public static IGenre Retrieve(this IGenre retrieved, ICursor cursor, bool overwrite = false)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			return retrieved;
		}				
		public static async Task<IGenre> RetrieveAsync(this IGenre retrieved, ICursor cursor, bool overwrite = false, CancellationToken cancellationToken = default)
		{
			retrieved = retrieved.RetrieveSync(cursor, overwrite);

			return await Task.FromResult(retrieved);
		}

		private static IGenre RetrieveSync(this IGenre retrieved, ICursor cursor, bool overwrite = false)
		{
			return retrieved;
		}
	}
}
