using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibrary
	{
		public interface IArtists
		{
			IArtist? Random(IIdentifiers? identifiers = null, IArtist<bool>? retriever = null);
			Task<IArtist?> Random(IIdentifiers? identifiers = null, IArtist<bool>? retriever = null, CancellationToken cancellationToken = default);

			IArtist? GetArtist(IIdentifiers? identifiers = null, IArtist<bool>? retriever = null);
			Task<IArtist?> GetArtist(IIdentifiers? identifiers = null, IArtist<bool>? retriever = null, CancellationToken cancellationToken = default);
			IEnumerable<IArtist> GetArtists(IIdentifiers? identifiers = null, IArtist<bool>? retriever = null);
			IAsyncEnumerable<IArtist> GetArtists(IIdentifiers? identifiers = null, IArtist<bool>? retriever = null, CancellationToken cancellationToken = default);

			IArtist? PopulateArtist(IArtist? artist);
			Task<IArtist?> PopulateArtist(IArtist? artist, CancellationToken cancellationToken = default);
			IEnumerable<IArtist>? PopulateArtists(IEnumerable<IArtist>? artists);
			Task<IEnumerable<IArtist>?> PopulateArtists(IEnumerable<IArtist>? artists, CancellationToken cancellationToken = default);

			bool UpdateArtist(IArtist old, IArtist updated);
			Task<bool> UpdateArtist(IArtist old, IArtist updated, CancellationToken cancellationToken = default);

			bool DeleteArtist(IArtist artist);
			Task<bool> DeleteArtist(IArtist artist, CancellationToken cancellationToken = default);
		}
	}
}
