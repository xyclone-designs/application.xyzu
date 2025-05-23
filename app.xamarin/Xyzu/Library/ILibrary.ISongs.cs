﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibrary
	{
		public interface ISongs
		{
			ISong? Random(IIdentifiers? identifiers = null);
			Task<ISong?> Random(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);

			ISong? GetSong(IIdentifiers? identifiers = null);
			Task<ISong?> GetSong(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);
			IEnumerable<ISong> GetSongs(IIdentifiers? identifiers = null);
			IAsyncEnumerable<ISong> GetSongs(IIdentifiers? identifiers = null, CancellationToken cancellationToken = default);

			bool UpdateSong(ISong old, ISong updated);
			Task<bool> UpdateSong(ISong old, ISong updated, CancellationToken cancellationToken = default);

			bool DeleteSong(ISong song);
			Task<bool> DeleteSong(ISong song, CancellationToken cancellationToken = default);
		}
	}
}
