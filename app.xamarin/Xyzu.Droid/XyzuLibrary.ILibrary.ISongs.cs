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
	public sealed partial class XyzuLibrary : ILibrary.ISongs
	{
		ISong? ILibrary.ISongs.Random(ILibrary.IIdentifiers? identifiers, ISong<bool>? retriever)
		{
			IEnumerable<ISong> songs = SqliteSongsTableQuery;

			if (identifiers != null)
				songs = songs.Where(song => identifiers.MatchesSong(song));

			Random random = new Random();
			int index = random.Next(0, songs.Count() - 1);
			ISong song = songs.ElementAt(index);

			return song;
		}
		async Task<ISong?> ILibrary.ISongs.Random(ILibrary.IIdentifiers? identifiers, ISong<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.ISongs).Random(identifiers, retriever);
			});
		}

		ISong? ILibrary.ISongs.GetSong(ILibrary.IIdentifiers? identifiers, ISong<bool>? retriever)
		{
			if (identifiers is null)
				return null;

			IEnumerable<ISong> songs = SqliteSongsTableQuery;
			ISong? song = songs.FirstOrDefault(song => identifiers.MatchesSong(song));

			return song;
		}
		async Task<ISong?> ILibrary.ISongs.GetSong(ILibrary.IIdentifiers? identifiers, ISong<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.ISongs).GetSong(identifiers, retriever);
			});
		}
		IEnumerable<ISong> ILibrary.ISongs.GetSongs(ILibrary.IIdentifiers? identifiers, ISong<bool>? retriever)
		{
			IEnumerable<ISong> songs = SqliteSongsTableQuery;

			if (identifiers != null)
				songs = songs.Where(song => identifiers.MatchesSong(song));

			if (retriever != null)
				songs = songs.Select(song =>
				{
					return song;
				});

			foreach (ISong song in songs)
				yield return song;
		}
		async IAsyncEnumerable<ISong> ILibrary.ISongs.GetSongs(ILibrary.IIdentifiers? identifiers, ISong<bool>? retriever, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			IEnumerable<ISong> songs = await Task.Run(() =>
			{
				return (this as ILibrary.ISongs).GetSongs(identifiers, retriever);
			});

			foreach (ISong song in songs)
				yield return song;
		}

		ISong? ILibrary.ISongs.PopulateSong(ISong? song)
		{
			if (song is null)
				return null;

			IEnumerable<ISong> songs = SqliteSongsTableQuery;
			
			if (songs.FirstOrDefault(predicate: sqlsong => sqlsong.Id == song.Id) is SongEntity songentity)
				song.Populate(songentity);

			return song;
		}
		async Task<ISong?> ILibrary.ISongs.PopulateSong(ISong? song, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.ISongs).PopulateSong(song);
			});
		}
		IEnumerable<ISong>? ILibrary.ISongs.PopulateSongs(IEnumerable<ISong>? songs)
		{
			if (songs is null)
				return null;

			foreach (ISong song in songs)
				(this as ILibrary.ISongs).PopulateSong(song);

			return songs;
		}
		async Task<IEnumerable<ISong>?> ILibrary.ISongs.PopulateSongs(IEnumerable<ISong>? songs, CancellationToken cancellationToken)
		{
			if (songs is null)
				return null;

			foreach (ISong song in songs)
				await (this as ILibrary.ISongs).PopulateSong(song, cancellationToken);

			return songs;
		}

		bool ILibrary.ISongs.DeleteSong(ISong song)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Song(song);

			SongEntity songentity = song as SongEntity ?? new SongEntity(song);

			SqliteConnection.Delete(songentity);

			return true;
		}
		async Task<bool> ILibrary.ISongs.DeleteSong(ISong song, CancellationToken cancellationToken)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Song(song);

			SongEntity songentity = song as SongEntity ?? new SongEntity(song);

			await SqliteConnectionAsync.DeleteAsync(songentity);

			return true;
		}

		bool ILibrary.ISongs.UpdateSong(ISong old, ISong updated)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Song(old, updated);

			SongEntity songentity = updated as SongEntity ?? new SongEntity(updated);

			SqliteConnection.Update(songentity);

			return true;
		}
		async Task<bool> ILibrary.ISongs.UpdateSong(ISong old, ISong updated, CancellationToken cancellationToken)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Song(old, updated);

			SongEntity songentity = updated as SongEntity ?? new SongEntity(updated);

			await SqliteConnectionAsync.UpdateAsync(songentity);

			return true;
		}
	}
}