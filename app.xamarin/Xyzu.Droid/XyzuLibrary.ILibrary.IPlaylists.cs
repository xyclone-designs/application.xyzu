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
	public sealed partial class XyzuLibrary : ILibrary.IPlaylists
	{
		IPlaylist? ILibrary.IPlaylists.Random(ILibrary.IIdentifiers? identifiers, IPlaylist<bool>? retriever)
		{
			IEnumerable<IPlaylist> playlists = SqlitePlaylistsTableQuery;

			if (identifiers != null)
				playlists = playlists.Where(playlist => identifiers.MatchesPlaylist(playlist));

			Random random = new Random();
			int index = random.Next(0, playlists.Count() - 1);
			IPlaylist playlist = playlists.ElementAt(index);

			return playlist;
		}
		async Task<IPlaylist?> ILibrary.IPlaylists.Random(ILibrary.IIdentifiers? identifiers, IPlaylist<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IPlaylists).Random(identifiers, retriever);
			});
		}

		IPlaylist? ILibrary.IPlaylists.GetPlaylist(ILibrary.IIdentifiers? identifiers, IPlaylist<bool>? retriever)
		{
			if (identifiers is null)
				return null;

			IEnumerable<IPlaylist> playlists = SqlitePlaylistsTableQuery;
			IPlaylist? playlist = playlists.FirstOrDefault(playlist => identifiers.MatchesPlaylist(playlist));

			return playlist;
		}
		async Task<IPlaylist?> ILibrary.IPlaylists.GetPlaylist(ILibrary.IIdentifiers? identifiers, IPlaylist<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IPlaylists).GetPlaylist(identifiers, retriever);
			});
		}
		IEnumerable<IPlaylist> ILibrary.IPlaylists.GetPlaylists(ILibrary.IIdentifiers? identifiers, IPlaylist<bool>? retriever)
		{
			IEnumerable<IPlaylist> playlists = SqlitePlaylistsTableQuery;

			if (identifiers != null)
				playlists = playlists.Where(playlist => identifiers.MatchesPlaylist(playlist));

			if (retriever != null)
				playlists = playlists.Select(playlist =>
				{
					return playlist;
				});

			foreach (IPlaylist playlist in playlists)
				yield return playlist;
		}
		async IAsyncEnumerable<IPlaylist> ILibrary.IPlaylists.GetPlaylists(ILibrary.IIdentifiers? identifiers, IPlaylist<bool>? retriever, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			IEnumerable<IPlaylist> playlists = await Task.Run(() =>
			{
				return (this as ILibrary.IPlaylists).GetPlaylists(identifiers, retriever);
			});

			foreach (IPlaylist playlist in playlists)
				yield return playlist;
		}

		IPlaylist? ILibrary.IPlaylists.PopulatePlaylist(IPlaylist? playlist)
		{
			if (playlist is null)
				return null;

			IEnumerable<IPlaylist> playlists = SqlitePlaylistsTableQuery;

			if (playlists.FirstOrDefault(predicate: sqlplaylist => sqlplaylist.Id == playlist.Id) is PlaylistEntity playlistentity)
				playlist.Populate(playlistentity);

			return playlist;
		}
		async Task<IPlaylist?> ILibrary.IPlaylists.PopulatePlaylist(IPlaylist? playlist, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IPlaylists).PopulatePlaylist(playlist);
			});
		}
		IEnumerable<IPlaylist>? ILibrary.IPlaylists.PopulatePlaylists(IEnumerable<IPlaylist>? playlists)
		{
			if (playlists is null)
				return null;

			foreach (IPlaylist playlist in playlists)
				(this as ILibrary.IPlaylists).PopulatePlaylist(playlist);

			return playlists;
		}
		async Task<IEnumerable<IPlaylist>?> ILibrary.IPlaylists.PopulatePlaylists(IEnumerable<IPlaylist>? playlists, CancellationToken cancellationToken)
		{
			if (playlists is null)
				return null;

			foreach (IPlaylist playlist in playlists)
				await (this as ILibrary.IPlaylists).PopulatePlaylist(playlist, cancellationToken);

			return playlists;
		}

		bool ILibrary.IPlaylists.CreatePlaylist(IPlaylist playlist)
		{
			PlaylistEntity playlistentity = new PlaylistEntity(playlist);

			SqliteConnection.Insert(playlistentity);

			return true;
		}
		async Task<bool> ILibrary.IPlaylists.CreatePlaylist(IPlaylist playlist, CancellationToken cancellationToken)
		{
			PlaylistEntity playlistentity = new PlaylistEntity(playlist);

			await SqliteConnectionAsync.InsertAsync(playlistentity);

			return true;
		}	   

		bool ILibrary.IPlaylists.DeletePlaylist(IPlaylist playlist)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Playlist(playlist);

			PlaylistEntity playlistentity = playlist as PlaylistEntity ?? new PlaylistEntity(playlist);

			SqliteConnection.Delete(playlistentity);

			return true;
		}
		async Task<bool> ILibrary.IPlaylists.DeletePlaylist(IPlaylist playlist, CancellationToken cancellationToken)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Playlist(playlist);

			PlaylistEntity playlistentity = playlist as PlaylistEntity ?? new PlaylistEntity(playlist);

			await SqliteConnectionAsync.DeleteAsync(playlistentity);

			return true;
		}

		bool ILibrary.IPlaylists.UpdatePlaylist(IPlaylist old, IPlaylist updated)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Playlist(old, updated);

			PlaylistEntity playlistentity = updated as PlaylistEntity ?? new PlaylistEntity(updated);

			SqliteConnection.Update(playlistentity);

			return true;
		}
		async Task<bool> ILibrary.IPlaylists.UpdatePlaylist(IPlaylist old, IPlaylist updated, CancellationToken cancellationToken)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Playlist(old, updated);

			PlaylistEntity playlistentity = updated as PlaylistEntity ?? new PlaylistEntity(updated);

			await SqliteConnectionAsync.UpdateAsync(playlistentity);

			return true;
		}
	}
}