#nullable enable

using SQLite;

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
		public partial class Default : ILibraryDroid.IArtists
		{
			IArtist? ILibraryDroid.IArtists.Random(ILibraryDroid.IIdentifiers? identifiers)
			{
				IEnumerable<IArtist> artists = identifiers is null
					? SQLiteLibrary.ArtistsTable
					: SQLiteLibrary.ArtistsTable.AsEnumerable().Where(_ => identifiers.MatchesArtist(_));

				Random random = new();
				int index = random.Next(0, artists.Count() - 1);
				IArtist artist = artists.ElementAt(index);

				return artist;
			}
			async Task<IArtist?> ILibraryDroid.IArtists.Random(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				IEnumerable<IArtist> artists = identifiers is null
					? SQLiteLibrary.ArtistsTable
					: SQLiteLibrary.ArtistsTable.AsEnumerable().Where(_ => identifiers.MatchesArtist(_));

				Random random = new();
				int index = random.Next(0, artists.Count() - 1);
				IArtist artist = await Task.FromResult(artists.ElementAt(index));

				return artist;
			}

			IArtist? ILibraryDroid.IArtists.GetArtist(ILibraryDroid.IIdentifiers? identifiers)
			{
				if (identifiers is null)
					return null;

				IArtist? artist = SQLiteLibrary.ArtistsTable.AsEnumerable().FirstOrDefault(artist => identifiers.MatchesArtist(artist));

				return artist;
			}
			async Task<IArtist?> ILibraryDroid.IArtists.GetArtist(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				if (identifiers is null)
					return null;

				IArtist? artist = SQLiteLibrary.ArtistsTable.AsEnumerable().FirstOrDefault(artist => identifiers.MatchesArtist(artist));

				return await Task.FromResult(artist);
			}
			IEnumerable<IArtist> ILibraryDroid.IArtists.GetArtists(ILibraryDroid.IIdentifiers? identifiers)
			{
				IEnumerable<IArtist> artists = identifiers is null
					? SQLiteLibrary.ArtistsTable
					: SQLiteLibrary.ArtistsTable.AsEnumerable().Where(_ => identifiers.MatchesArtist(_));

				foreach (IArtist artist in artists)
					yield return artist;
			}
			async IAsyncEnumerable<IArtist> ILibraryDroid.IArtists.GetArtists(ILibraryDroid.IIdentifiers? identifiers, [EnumeratorCancellation] CancellationToken cancellationToken)
			{
				IEnumerable<IArtist> artists = identifiers is null
					? SQLiteLibrary.ArtistsTable
					: SQLiteLibrary.ArtistsTable.AsEnumerable().Where(_ => identifiers.MatchesArtist(_));

				foreach (IArtist artist in artists)
					yield return await Task.FromResult(artist);
			}

			IArtist? ILibraryDroid.IArtists.PopulateArtist(IArtist? artist)
			{
				if (artist is null)
					return null;

				if (SQLiteLibrary.ArtistsTable.FirstOrDefault(sqlartist => sqlartist.Id == artist.Id) is ArtistEntity artistentity)
					artist.Populate(artistentity);

				return artist;
			}
			async Task<IArtist?> ILibraryDroid.IArtists.PopulateArtist(IArtist? artist, CancellationToken cancellationToken)
			{
				if (artist is null)
					return null;

				if (await SQLiteLibrary.ArtistsTableAsync.FirstOrDefaultAsync(sqlartist => sqlartist.Id == artist.Id) is ArtistEntity artistentity)
					artist.Populate(artistentity);

				return artist;
			}
			IEnumerable<IArtist>? ILibraryDroid.IArtists.PopulateArtists(IEnumerable<IArtist>? artists)
			{
				if (artists is null)
					return null;

				foreach (IArtist artist in artists)
					(this as ILibraryDroid.IArtists).PopulateArtist(artist);

				return artists;
			}
			async Task<IEnumerable<IArtist>?> ILibraryDroid.IArtists.PopulateArtists(IEnumerable<IArtist>? artists, CancellationToken cancellationToken)
			{
				if (artists is null)
					return null;

				foreach (IArtist artist in artists)
					await (this as ILibraryDroid.IArtists).PopulateArtist(artist, cancellationToken);

				return artists;
			}

			bool ILibraryDroid.IArtists.DeleteArtist(IArtist artist)
			{
				if (Actions?.OnDelete != null)
					foreach (ILibraryDroid.IOnDeleteActions ondeleteaction in Actions.OnDelete)
						ondeleteaction.Artist(artist);

				ArtistEntity artistentity = artist as ArtistEntity ?? new ArtistEntity(artist);

				SQLiteLibrary.Connection.Delete(artistentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IArtists.DeleteArtist(IArtist artist, CancellationToken cancellationToken)
			{
				if (Actions?.OnDelete != null)
					await Task.WhenAll(Actions.OnDelete.Select(_ => _.Artist(artist)));

				ArtistEntity artistentity = artist as ArtistEntity ?? new ArtistEntity(artist);

				await SQLiteLibrary.ConnectionAsync.DeleteAsync(artistentity);

				return true;
			}

			bool ILibraryDroid.IArtists.UpdateArtist(IArtist old, IArtist updated)
			{
				if (Actions?.OnUpdate != null)
					foreach (ILibraryDroid.IOnUpdateActions onupdateaction in Actions.OnUpdate)
						onupdateaction.Artist(old, updated);

				ArtistEntity artistentity = updated as ArtistEntity ?? new ArtistEntity(updated);

				SQLiteLibrary.Connection.Update(artistentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IArtists.UpdateArtist(IArtist old, IArtist updated, CancellationToken cancellationToken)
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