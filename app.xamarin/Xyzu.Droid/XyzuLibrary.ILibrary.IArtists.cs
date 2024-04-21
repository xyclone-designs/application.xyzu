#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library;
using Xyzu.Library.Models;

namespace Xyzu
{
	public sealed partial class XyzuLibrary : ILibrary.IArtists
	{
		IArtist? ILibrary.IArtists.Random(ILibrary.IIdentifiers? identifiers, IArtist<bool>? retriever)
		{
			IEnumerable<IArtist> artists = SqliteArtistsTableQuery;

			if (identifiers != null)
				artists = artists.Where(artist => identifiers.MatchesArtist(artist));

			Random random = new Random();
			int index = random.Next(0, artists.Count() - 1);
			IArtist artist = artists.ElementAt(index);

			return artist;
		}
		async Task<IArtist?> ILibrary.IArtists.Random(ILibrary.IIdentifiers? identifiers, IArtist<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IArtists).Random(identifiers, retriever);
			});
		}

		IArtist? ILibrary.IArtists.GetArtist(ILibrary.IIdentifiers? identifiers, IArtist<bool>? retriever)
		{
			if (identifiers is null)
				return null;

			IEnumerable<IArtist> artists = SqliteArtistsTableQuery;
			IArtist? artist = artists.FirstOrDefault(artist => identifiers.MatchesArtist(artist));

			return artist;
		}
		async Task<IArtist?> ILibrary.IArtists.GetArtist(ILibrary.IIdentifiers? identifiers, IArtist<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IArtists).GetArtist(identifiers, retriever);
			});
		}
		IEnumerable<IArtist> ILibrary.IArtists.GetArtists(ILibrary.IIdentifiers? identifiers, IArtist<bool>? retriever)
		{
			IEnumerable<IArtist> artists = SqliteArtistsTableQuery;

			if (identifiers != null)
				artists = artists.Where(artist => identifiers.MatchesArtist(artist));

			if (retriever != null)
				artists = artists.Select(artist =>
				{
					return artist;
				});

			foreach (IArtist artist in artists)
				yield return artist;
		}
		async IAsyncEnumerable<IArtist> ILibrary.IArtists.GetArtists(ILibrary.IIdentifiers? identifiers, IArtist<bool>? retriever, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			IEnumerable<IArtist> artists = await Task.Run(() =>
			{
				return (this as ILibrary.IArtists).GetArtists(identifiers, retriever);
			});

			foreach (IArtist artist in artists)
				yield return artist;
		}

		IArtist? ILibrary.IArtists.PopulateArtist(IArtist? artist)
		{
			if (artist is null)
				return null;

			IEnumerable<IArtist> artists = SqliteArtistsTableQuery;

			if (artists.FirstOrDefault(predicate: sqlartist => sqlartist.Id == artist.Id) is ArtistEntity artistentity)
				artist.Populate(artistentity);

			return artist;
		}
		async Task<IArtist?> ILibrary.IArtists.PopulateArtist(IArtist? artist, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IArtists).PopulateArtist(artist);
			});
		}
		IEnumerable<IArtist>? ILibrary.IArtists.PopulateArtists(IEnumerable<IArtist>? artists)
		{
			if (artists is null)
				return null;

			foreach (IArtist artist in artists)
				(this as ILibrary.IArtists).PopulateArtist(artist);

			return artists;
		}
		async Task<IEnumerable<IArtist>?> ILibrary.IArtists.PopulateArtists(IEnumerable<IArtist>? artists, CancellationToken cancellationToken)
		{
			if (artists is null)
				return null;

			foreach (IArtist artist in artists)
				await (this as ILibrary.IArtists).PopulateArtist(artist, cancellationToken);

			return artists;
		}

		bool ILibrary.IArtists.DeleteArtist(IArtist artist)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Artist(artist);

			ArtistEntity artistentity = artist as ArtistEntity ?? new ArtistEntity(artist);

			SqliteConnection.Delete(artistentity);

			return true;
		}
		async Task<bool> ILibrary.IArtists.DeleteArtist(IArtist artist, CancellationToken cancellationToken)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Artist(artist);

			ArtistEntity artistentity = artist as ArtistEntity ?? new ArtistEntity(artist);

			await SqliteConnectionAsync.DeleteAsync(artistentity);

			return true;
		}

		bool ILibrary.IArtists.UpdateArtist(IArtist old, IArtist updated)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Artist(old, updated);

			ArtistEntity artistentity = updated as ArtistEntity ?? new ArtistEntity(updated);

			SqliteConnection.Update(artistentity);

			return true;
		}
		async Task<bool> ILibrary.IArtists.UpdateArtist(IArtist old, IArtist updated, CancellationToken cancellationToken)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Artist(old, updated);

			ArtistEntity artistentity = updated as ArtistEntity ?? new ArtistEntity(updated);

			await SqliteConnectionAsync.UpdateAsync(artistentity);

			return true;
		}
	}
}