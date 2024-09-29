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
		public partial class Default : ILibraryDroid.ISongs
		{
			ISong? ILibraryDroid.ISongs.Random(ILibraryDroid.IIdentifiers? identifiers)
			{
				TableQuery<SongEntity> songs = identifiers is null 
					? SQLiteLibrary.SongsTable
					: SQLiteLibrary.SongsTable.Where(identifiers.MatchesSong<SongEntity>());

				Random random = new ();
				int index = random.Next(0, songs.Count() - 1);
				ISong song = songs.ElementAt(index);

				return song;
			}
			async Task<ISong?> ILibraryDroid.ISongs.Random(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				AsyncTableQuery<SongEntity> songs = identifiers is null
					? SQLiteLibrary.SongsTableAsync
					: SQLiteLibrary.SongsTableAsync.Where(identifiers.MatchesSong<SongEntity>());

				Random random = new();
				int index = random.Next(0, await songs.CountAsync() - 1);
				ISong song = await songs.ElementAtAsync(index);

				return song;
			}

			ISong? ILibraryDroid.ISongs.GetSong(ILibraryDroid.IIdentifiers? identifiers)
			{
				if (identifiers is null)
					return null;

				ISong? song = (SQLiteLibrary.SongsTable as IEnumerable<ISong>).FirstOrDefault(song => identifiers.MatchesSong(song));

				return song;
			}
			async Task<ISong?> ILibraryDroid.ISongs.GetSong(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				if (identifiers is null)
					return null;

				ISong song = await SQLiteLibrary.SongsTableAsync.FirstOrDefaultAsync(identifiers.MatchesSong<SongEntity>());

				return song;
			}
			IEnumerable<ISong> ILibraryDroid.ISongs.GetSongs(ILibraryDroid.IIdentifiers? identifiers)
			{
				TableQuery<SongEntity> songs = identifiers is null
					? SQLiteLibrary.SongsTable
					: SQLiteLibrary.SongsTable.Where(identifiers.MatchesSong<SongEntity>());

				foreach (ISong song in songs)
					yield return song;
			}
			async IAsyncEnumerable<ISong> ILibraryDroid.ISongs.GetSongs(ILibraryDroid.IIdentifiers? identifiers, [EnumeratorCancellation] CancellationToken cancellationToken)
			{
				AsyncTableQuery<SongEntity> songs = identifiers is null
					? SQLiteLibrary.SongsTableAsync
					: SQLiteLibrary.SongsTableAsync.Where(identifiers.MatchesSong<SongEntity>());

				foreach (ISong song in await songs.ToListAsync())
					yield return song;
			}

			ISong? ILibraryDroid.ISongs.PopulateSong(ISong? song)
			{
				if (song is null)
					return null;

				if (SQLiteLibrary.SongsTable.FirstOrDefault(sqlsong => sqlsong.Id == song.Id) is SongEntity songentity)
					song.Populate(songentity);

				return song;
			}
			async Task<ISong?> ILibraryDroid.ISongs.PopulateSong(ISong? song, CancellationToken cancellationToken)
			{
				if (song is null)
					return null;

				if (await SQLiteLibrary.SongsTableAsync.FirstOrDefaultAsync(sqlsong => sqlsong.Id == song.Id) is SongEntity songentity)
					song.Populate(songentity);

				return song;
			}
			IEnumerable<ISong>? ILibraryDroid.ISongs.PopulateSongs(IEnumerable<ISong>? songs)
			{
				if (songs is null)
					return null;

				foreach (ISong song in songs)
					(this as ILibraryDroid.ISongs).PopulateSong(song);

				return songs;
			}
			async Task<IEnumerable<ISong>?> ILibraryDroid.ISongs.PopulateSongs(IEnumerable<ISong>? songs, CancellationToken cancellationToken)
			{
				if (songs is null)
					return null;

				foreach (ISong song in songs)
					await (this as ILibraryDroid.ISongs).PopulateSong(song, cancellationToken);

				return songs;
			}

			bool ILibraryDroid.ISongs.DeleteSong(ISong song)
			{
				if (Actions?.OnDelete != null)
					foreach (ILibraryDroid.IOnDeleteActions ondeleteaction in Actions.OnDelete)
						ondeleteaction.Song(song);

				SongEntity songentity = song as SongEntity ?? new SongEntity(song);

				SQLiteLibrary.Connection.Delete(songentity);

				return true;
			}
			async Task<bool> ILibraryDroid.ISongs.DeleteSong(ISong song, CancellationToken cancellationToken)
			{
				if (Actions?.OnDelete != null)
					await Task.WhenAll(Actions.OnDelete.Select(_ => _.Song(song)));

				SongEntity songentity = song as SongEntity ?? new SongEntity(song);

				await SQLiteLibrary.ConnectionAsync.DeleteAsync(songentity);

				return true;
			}

			bool ILibraryDroid.ISongs.UpdateSong(ISong old, ISong updated)
			{
				if (Actions?.OnUpdate != null)
					foreach (ILibraryDroid.IOnUpdateActions onupdateaction in Actions.OnUpdate)
						onupdateaction.Song(old, updated);

				SongEntity songentity = updated as SongEntity ?? new SongEntity(updated);

				SQLiteLibrary.Connection.Update(songentity);

				return true;
			}
			async Task<bool> ILibraryDroid.ISongs.UpdateSong(ISong old, ISong updated, CancellationToken cancellationToken)
			{
				if (Actions?.OnUpdate != null)
					await Task.WhenAll(Actions.OnUpdate.Select(_ => _.Song(old, updated)));

				SongEntity songentity = updated as SongEntity ?? new SongEntity(updated);

				await SQLiteLibrary.ConnectionAsync.UpdateAsync(songentity);

				return true;
			}
		}
	}
}