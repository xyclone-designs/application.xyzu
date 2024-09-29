using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibrary
	{
		public interface IPlaylists
		{
			IPlaylist? Random(IIdentifiers? identifiers = null);
			Task<IPlaylist?> Random(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);

			IPlaylist? GetPlaylist(IIdentifiers? identifiers = null);
			Task<IPlaylist?> GetPlaylist(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);
			IEnumerable<IPlaylist> GetPlaylists(IIdentifiers? identifiers = null);
			IAsyncEnumerable<IPlaylist> GetPlaylists(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);

			IPlaylist? PopulatePlaylist(IPlaylist? playlist);
			Task<IPlaylist?> PopulatePlaylist(IPlaylist? playlist, CancellationToken cancellationToken = default);
			IEnumerable<IPlaylist>? PopulatePlaylists(IEnumerable<IPlaylist>? playlists);
			Task<IEnumerable<IPlaylist>?> PopulatePlaylists(IEnumerable<IPlaylist>? playlists, CancellationToken cancellationToken = default);

			bool CreatePlaylist(IPlaylist playlist);
			Task<bool> CreatePlaylist(IPlaylist playlist, CancellationToken cancellationToken = default);								

			bool UpdatePlaylist(IPlaylist old, IPlaylist updated);
			Task<bool> UpdatePlaylist(IPlaylist old, IPlaylist updated, CancellationToken cancellationToken = default);

			bool DeletePlaylist(IPlaylist playlist);
			Task<bool> DeletePlaylist(IPlaylist playlist, CancellationToken cancellationToken = default);
		}
	}
}
