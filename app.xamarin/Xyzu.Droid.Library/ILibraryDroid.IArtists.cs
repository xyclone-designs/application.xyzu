using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibraryDroid
	{
		public partial class Default : IArtists
		{
			IArtist? IArtists.Random(IIdentifiers? identifiers)
			{
				IEnumerable<IArtist> artists = identifiers is null
					? SQLiteLibrary.ArtistsTable
					: SQLiteLibrary.ArtistsTable.AsEnumerable().Where(_ => identifiers.MatchesArtist(_));

				Random random = new();
				int index = random.Next(0, artists.Count() - 1);
				IArtist artist = artists.ElementAt(index);

				return artist;
			}
			async Task<IArtist?> IArtists.Random(IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				IEnumerable<IArtist> artists = identifiers is null
					? SQLiteLibrary.ArtistsTable
					: SQLiteLibrary.ArtistsTable.AsEnumerable().Where(_ => identifiers.MatchesArtist(_));

				Random random = new();
				int index = random.Next(0, artists.Count() - 1);
				IArtist artist = await Task.FromResult(artists.ElementAt(index));

				return artist;
			}

			IArtist? IArtists.GetArtist(IIdentifiers? identifiers)
			{
				if (identifiers is null)
					return null;

				IArtist? artist = SQLiteLibrary.ArtistsTable.AsEnumerable().FirstOrDefault(artist => identifiers.MatchesArtist(artist));

				return artist;
			}
			async Task<IArtist?> IArtists.GetArtist(IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				if (identifiers is null)
					return null;

				IArtist? artist = SQLiteLibrary.ArtistsTable.AsEnumerable().FirstOrDefault(artist => identifiers.MatchesArtist(artist));

				return await Task.FromResult(artist);
			}
			IEnumerable<IArtist> IArtists.GetArtists(IIdentifiers? identifiers)
			{
				IEnumerable<IArtist> artists = identifiers is null
					? SQLiteLibrary.ArtistsTable
					: SQLiteLibrary.ArtistsTable.AsEnumerable().Where(_ => identifiers.MatchesArtist(_));

				foreach (IArtist artist in artists)
					yield return artist;
			}
			async IAsyncEnumerable<IArtist> IArtists.GetArtists(IIdentifiers? identifiers, [EnumeratorCancellation] CancellationToken cancellationToken)
			{
				IEnumerable<IArtist> artists = identifiers is null
					? SQLiteLibrary.ArtistsTable
					: SQLiteLibrary.ArtistsTable.AsEnumerable().Where(_ => identifiers.MatchesArtist(_));

				foreach (IArtist artist in artists)
					yield return await Task.FromResult(artist);
			}

			bool IArtists.DeleteArtist(IArtist artist)
			{
				if (Actions?.OnDelete != null)
					foreach (IOnDeleteActions ondeleteaction in Actions.OnDelete)
						ondeleteaction.Artist(artist);

				ArtistEntity artistentity = artist as ArtistEntity ?? new ArtistEntity(artist);

				SQLiteLibrary.Connection.Delete(artistentity);

				return true;
			}
			async Task<bool> IArtists.DeleteArtist(IArtist artist, CancellationToken cancellationToken)
			{
				if (Actions?.OnDelete != null)
					await Task.WhenAll(Actions.OnDelete.Select(_ => _.Artist(artist)));

				ArtistEntity artistentity = artist as ArtistEntity ?? new ArtistEntity(artist);

				await SQLiteLibrary.ConnectionAsync.DeleteAsync(artistentity);

				return true;
			}

			bool IArtists.UpdateArtist(IArtist old, IArtist updated)
			{
				if (Actions?.OnUpdate != null)
					foreach (IOnUpdateActions onupdateaction in Actions.OnUpdate)
						onupdateaction.Artist(old, updated);

				ArtistEntity artistentity = updated as ArtistEntity ?? new ArtistEntity(updated);

				SQLiteLibrary.Connection.Update(artistentity);

				return true;
			}
			async Task<bool> IArtists.UpdateArtist(IArtist old, IArtist updated, CancellationToken cancellationToken)
			{
				if (Actions?.OnUpdate != null)
					await Task.WhenAll(Actions.OnUpdate.Select(_ => _.Artist(old, updated)));

				ArtistEntity artistentity = updated as ArtistEntity ?? new ArtistEntity(updated);

				await SQLiteLibrary.ConnectionAsync.UpdateAsync(artistentity);

				return true;
			}
		}
	}
}