using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibrary
	{
		public interface IGenres
		{
			IGenre? Random(IIdentifiers? identifiers = null, IGenre<bool>? retriever = null);
			Task<IGenre?> Random(IIdentifiers? identifiers = null, IGenre<bool>? retriever = null, CancellationToken cancellationToken = default);

			IGenre? GetGenre(IIdentifiers? identifiers = null, IGenre<bool>? retriever = null);
			Task<IGenre?> GetGenre(IIdentifiers? identifiers = null, IGenre<bool>? retriever = null, CancellationToken cancellationToken = default);
			IEnumerable<IGenre> GetGenres(IIdentifiers? identifiers = null, IGenre<bool>? retriever = null);
			IAsyncEnumerable<IGenre> GetGenres(IIdentifiers? identifiers = null, IGenre<bool>? retriever = null, CancellationToken cancellationToken = default);

			IGenre? PopulateGenre(IGenre? genre);
			Task<IGenre?> PopulateGenre(IGenre? genre, CancellationToken cancellationToken = default);
			IEnumerable<IGenre>? PopulateGenres(IEnumerable<IGenre>? genres);
			Task<IEnumerable<IGenre>?> PopulateGenres(IEnumerable<IGenre>? genres, CancellationToken cancellationToken = default);

			bool UpdateGenre(IGenre old, IGenre updated);
			Task<bool> UpdateGenre(IGenre old, IGenre updated, CancellationToken cancellationToken = default);

			bool DeleteGenre(IGenre genre);
			Task<bool> DeleteGenre(IGenre genre, CancellationToken cancellationToken = default);
		}
	}
}
