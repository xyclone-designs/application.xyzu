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
			IAlbum? Random(IIdentifiers? identifiers = null, IAlbum<bool>? retriever = null);
			Task<IAlbum?> Random(IIdentifiers? identifiers = null, IAlbum<bool>? retriever = null, CancellationToken cancellationToken = default);

			IAlbum? GetAlbum(IIdentifiers? identifiers = null, IAlbum<bool>? retriever = null);
			Task<IAlbum?> GetAlbum(IIdentifiers? identifiers = null, IAlbum<bool>? retriever = null, CancellationToken cancellationToken = default);
			IEnumerable<IAlbum> GetAlbums(IIdentifiers? identifiers = null, IAlbum<bool>? retriever = null);
			IAsyncEnumerable<IAlbum> GetAlbums(IIdentifiers? identifiers = null, IAlbum<bool>? retriever = null, CancellationToken cancellationToken = default);

			IAlbum? PopulateAlbum(IAlbum? album);
			Task<IAlbum?> PopulateAlbum(IAlbum? album, CancellationToken cancellationToken = default);
			IEnumerable<IAlbum>? PopulateAlbums(IEnumerable<IAlbum>? albums);
			Task<IEnumerable<IAlbum>?> PopulateAlbums(IEnumerable<IAlbum>? albums, CancellationToken cancellationToken = default);

			bool UpdateAlbum(IAlbum old, IAlbum updated);
			Task<bool> UpdateAlbum(IAlbum old, IAlbum updated, CancellationToken cancellationToken = default);

			bool DeleteAlbum(IAlbum album);
			Task<bool> DeleteAlbum(IAlbum album, CancellationToken cancellationToken = default);
		}
	}
}
