﻿using SQLite;

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
		public partial class Default : ILibraryDroid.IPlaylists
		{
			IPlaylist? ILibraryDroid.IPlaylists.Random(ILibraryDroid.IIdentifiers? identifiers)
			{
				IEnumerable<IPlaylist> playlists = identifiers is null
					? SQLiteLibrary.PlaylistsTable
					: SQLiteLibrary.PlaylistsTable.AsEnumerable().Where(_ => identifiers.MatchesPlaylist(_));

				Random random = new();
				int index = random.Next(0, playlists.Count() - 1);
				IPlaylist playlist = playlists.ElementAt(index);

				return playlist;
			}
			async Task<IPlaylist?> ILibraryDroid.IPlaylists.Random(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				IEnumerable<IPlaylist> playlists = identifiers is null
					? SQLiteLibrary.PlaylistsTable
					: SQLiteLibrary.PlaylistsTable.AsEnumerable().Where(_ => identifiers.MatchesPlaylist(_));

				Random random = new();
				int index = random.Next(0, playlists.Count() - 1);
				IPlaylist playlist = await Task.FromResult(playlists.ElementAt(index));

				return playlist;
			}

			IPlaylist? ILibraryDroid.IPlaylists.GetPlaylist(ILibraryDroid.IIdentifiers? identifiers)
			{
				if (identifiers is null)
					return null;

				IPlaylist? playlist = SQLiteLibrary.PlaylistsTable.AsEnumerable().FirstOrDefault(playlist => identifiers.MatchesPlaylist(playlist));

				return playlist;
			}
			async Task<IPlaylist?> ILibraryDroid.IPlaylists.GetPlaylist(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				if (identifiers is null)
					return null;

				IPlaylist? playlist = SQLiteLibrary.PlaylistsTable.AsEnumerable().FirstOrDefault(playlist => identifiers.MatchesPlaylist(playlist));

				return await Task.FromResult(playlist);
			}
			IEnumerable<IPlaylist> ILibraryDroid.IPlaylists.GetPlaylists(ILibraryDroid.IIdentifiers? identifiers)
			{
				IEnumerable<IPlaylist> playlists = identifiers is null
					? SQLiteLibrary.PlaylistsTable
					: SQLiteLibrary.PlaylistsTable.AsEnumerable().Where(_ => identifiers.MatchesPlaylist(_));

				foreach (IPlaylist playlist in playlists)
					yield return playlist;
			}
			async IAsyncEnumerable<IPlaylist> ILibraryDroid.IPlaylists.GetPlaylists(ILibraryDroid.IIdentifiers? identifiers, [EnumeratorCancellation] CancellationToken cancellationToken)
			{
				IEnumerable<IPlaylist> playlists = identifiers is null
					? SQLiteLibrary.PlaylistsTable
					: SQLiteLibrary.PlaylistsTable.AsEnumerable().Where(_ => identifiers.MatchesPlaylist(_));

				foreach (IPlaylist playlist in playlists)
					yield return await Task.FromResult(playlist);
			}

			IPlaylist? ILibraryDroid.IPlaylists.PopulatePlaylist(IPlaylist? playlist)
			{
				if (playlist is null)
					return null;

				if (SQLiteLibrary.PlaylistsTable.FirstOrDefault(sqlplaylist => sqlplaylist.Id == playlist.Id) is PlaylistEntity playlistentity)
					playlist.Populate(playlistentity);

				return playlist;
			}
			async Task<IPlaylist?> ILibraryDroid.IPlaylists.PopulatePlaylist(IPlaylist? playlist, CancellationToken cancellationToken)
			{
				if (playlist is null)
					return null;

				if (await SQLiteLibrary.PlaylistsTableAsync.FirstOrDefaultAsync(sqlplaylist => sqlplaylist.Id == playlist.Id) is PlaylistEntity playlistentity)
					playlist.Populate(playlistentity);

				return playlist;
			}
			IEnumerable<IPlaylist>? ILibraryDroid.IPlaylists.PopulatePlaylists(IEnumerable<IPlaylist>? playlists)
			{
				if (playlists is null)
					return null;

				foreach (IPlaylist playlist in playlists)
					(this as ILibraryDroid.IPlaylists).PopulatePlaylist(playlist);

				return playlists;
			}
			async Task<IEnumerable<IPlaylist>?> ILibraryDroid.IPlaylists.PopulatePlaylists(IEnumerable<IPlaylist>? playlists, CancellationToken cancellationToken)
			{
				if (playlists is null)
					return null;

				foreach (IPlaylist playlist in playlists)
					await (this as ILibraryDroid.IPlaylists).PopulatePlaylist(playlist, cancellationToken);

				return playlists;
			}

			bool ILibraryDroid.IPlaylists.CreatePlaylist(IPlaylist playlist)
			{
				PlaylistEntity playlistentity = new (playlist);

				SQLiteLibrary.Connection.Insert(playlistentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IPlaylists.CreatePlaylist(IPlaylist playlist, CancellationToken cancellationToken)
			{
				PlaylistEntity playlistentity = new (playlist);

				await SQLiteLibrary.ConnectionAsync.InsertAsync(playlistentity);

				return true;
			}

			bool ILibraryDroid.IPlaylists.DeletePlaylist(IPlaylist playlist)
			{
				if (Actions?.OnDelete != null)
					foreach (ILibraryDroid.IOnDeleteActions ondeleteaction in Actions.OnDelete)
						ondeleteaction.Playlist(playlist);

				PlaylistEntity playlistentity = playlist as PlaylistEntity ?? new PlaylistEntity(playlist);

				SQLiteLibrary.Connection.Delete(playlistentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IPlaylists.DeletePlaylist(IPlaylist playlist, CancellationToken cancellationToken)
			{
				if (Actions?.OnDelete != null)
					await Task.WhenAll(Actions.OnDelete.Select(_ => _.Playlist(playlist)));

				PlaylistEntity playlistentity = playlist as PlaylistEntity ?? new PlaylistEntity(playlist);

				await SQLiteLibrary.ConnectionAsync.DeleteAsync(playlistentity);

				return true;
			}

			bool ILibraryDroid.IPlaylists.UpdatePlaylist(IPlaylist old, IPlaylist updated)
			{
				if (Actions?.OnUpdate != null)
					foreach (ILibraryDroid.IOnUpdateActions onupdateaction in Actions.OnUpdate)
						onupdateaction.Playlist(old, updated);

				PlaylistEntity playlistentity = updated as PlaylistEntity ?? new PlaylistEntity(updated);

				SQLiteLibrary.Connection.Update(playlistentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IPlaylists.UpdatePlaylist(IPlaylist old, IPlaylist updated, CancellationToken cancellationToken)
			{
				if (Actions?.OnUpdate != null)
					await Task.WhenAll(Actions.OnUpdate.Select(_ => _.Playlist(old, updated)));

				PlaylistEntity playlistentity = updated as PlaylistEntity ?? new PlaylistEntity(updated);

				await SQLiteLibrary.ConnectionAsync.UpdateAsync(playlistentity);

				return true;
			}
		}
	}
}