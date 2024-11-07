using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibrary
	{
		public interface IAlbums
		{
			IAlbum? Random(IIdentifiers? identifiers = null);
			Task<IAlbum?> Random(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);

			IAlbum? GetAlbum(IIdentifiers? identifiers = null);
			Task<IAlbum?> GetAlbum(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);
			IEnumerable<IAlbum> GetAlbums(IIdentifiers? identifiers = null);
			IAsyncEnumerable<IAlbum> GetAlbums(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);

			bool UpdateAlbum(IAlbum old, IAlbum updated);
			Task<bool> UpdateAlbum(IAlbum old, IAlbum updated, CancellationToken cancellationToken = default);

			bool DeleteAlbum(IAlbum album);
			Task<bool> DeleteAlbum(IAlbum album, CancellationToken cancellationToken = default);
		}
	}
}
