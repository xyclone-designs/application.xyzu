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
		public partial class Default : IAlbums
		{
			IAlbum? IAlbums.Random(IIdentifiers? identifiers)
			{
				IEnumerable<IAlbum> albums = identifiers is null
					? SQLiteLibrary.AlbumsTable
					: SQLiteLibrary.AlbumsTable.AsEnumerable().Where(_ => identifiers.MatchesAlbum(_));

				Random random = new();
				int index = random.Next(0, albums.Count() - 1);
				IAlbum album = albums.ElementAt(index);

				return album;
			}
			async Task<IAlbum?> IAlbums.Random(IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				IEnumerable<IAlbum> albums = identifiers is null
					? SQLiteLibrary.AlbumsTable
					: SQLiteLibrary.AlbumsTable.AsEnumerable().Where(_ => identifiers.MatchesAlbum(_));

				Random random = new();
				int index = random.Next(0, albums.Count() - 1);
				IAlbum album = await Task.FromResult(albums.ElementAt(index));

				return album;
			}

			IAlbum? IAlbums.GetAlbum(IIdentifiers? identifiers)
			{
				if (identifiers is null)
					return null;

				IAlbum? album = SQLiteLibrary.AlbumsTable.AsEnumerable().FirstOrDefault(album => identifiers.MatchesAlbum(album));

				return album;
			}
			async Task<IAlbum?> IAlbums.GetAlbum(IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				if (identifiers is null)
					return null;

				IAlbum? album = SQLiteLibrary.AlbumsTable.AsEnumerable().FirstOrDefault(album => identifiers.MatchesAlbum(album));

				return await Task.FromResult(album);
			}
			IEnumerable<IAlbum> IAlbums.GetAlbums(IIdentifiers? identifiers)
			{
				IEnumerable<IAlbum> albums = identifiers is null
					? SQLiteLibrary.AlbumsTable
					: SQLiteLibrary.AlbumsTable.AsEnumerable().Where(_ => identifiers.MatchesAlbum(_));

				foreach (IAlbum album in albums)
					yield return album;
			}
			async IAsyncEnumerable<IAlbum> IAlbums.GetAlbums(IIdentifiers? identifiers, [EnumeratorCancellation] CancellationToken cancellationToken)
			{
				IEnumerable<IAlbum> albums = identifiers is null
					? SQLiteLibrary.AlbumsTable
					: SQLiteLibrary.AlbumsTable.AsEnumerable().Where(_ => identifiers.MatchesAlbum(_));

				foreach (IAlbum album in albums)
					yield return await Task.FromResult(album);
			}

			bool IAlbums.DeleteAlbum(IAlbum album)
			{
				if (Actions?.OnDelete != null)
					foreach (IOnDeleteActions ondeleteaction in Actions.OnDelete)
						ondeleteaction.Album(album);

				AlbumEntity albumentity = album as AlbumEntity ?? new AlbumEntity(album);

				SQLiteLibrary.Connection.Delete(albumentity);

				return true;
			}
			async Task<bool> IAlbums.DeleteAlbum(IAlbum album, CancellationToken cancellationToken)
			{
				if (Actions?.OnDelete != null)
					await Task.WhenAll(Actions.OnDelete.Select(_ => _.Album(album)));

				AlbumEntity albumentity = album as AlbumEntity ?? new AlbumEntity(album);

				await SQLiteLibrary.ConnectionAsync.DeleteAsync(albumentity);

				return true;
			}

			bool IAlbums.UpdateAlbum(IAlbum old, IAlbum updated)
			{
				if (Actions?.OnUpdate != null)
					foreach (IOnUpdateActions onupdateaction in Actions.OnUpdate)
						onupdateaction.Album(old, updated);

				AlbumEntity albumentity = updated as AlbumEntity ?? new AlbumEntity(updated);

				SQLiteLibrary.Connection.Update(albumentity);

				return true;
			}
			async Task<bool> IAlbums.UpdateAlbum(IAlbum old, IAlbum updated, CancellationToken cancellationToken)
			{
				if (Actions?.OnUpdate != null)
					await Task.WhenAll(Actions.OnUpdate.Select(_ => _.Album(old, updated)));

				AlbumEntity albumentity = updated as AlbumEntity ?? new AlbumEntity(updated);

				await SQLiteLibrary.ConnectionAsync.UpdateAsync(albumentity);

				return true;
			}
		}
	}
}