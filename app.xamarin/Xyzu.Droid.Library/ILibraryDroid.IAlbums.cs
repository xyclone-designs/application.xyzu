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
		public partial class Default : ILibraryDroid.IAlbums
		{
			IAlbum? ILibraryDroid.IAlbums.Random(ILibraryDroid.IIdentifiers? identifiers)
			{
				IEnumerable<IAlbum> albums = identifiers is null
					? SQLiteLibrary.AlbumsTable
					: SQLiteLibrary.AlbumsTable.AsEnumerable().Where(_ => identifiers.MatchesAlbum(_));

				Random random = new();
				int index = random.Next(0, albums.Count() - 1);
				IAlbum album = albums.ElementAt(index);

				return album;
			}
			async Task<IAlbum?> ILibraryDroid.IAlbums.Random(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				IEnumerable<IAlbum> albums = identifiers is null
					? SQLiteLibrary.AlbumsTable
					: SQLiteLibrary.AlbumsTable.AsEnumerable().Where(_ => identifiers.MatchesAlbum(_));

				Random random = new();
				int index = random.Next(0, albums.Count() - 1);
				IAlbum album = await Task.FromResult(albums.ElementAt(index));

				return album;
			}

			IAlbum? ILibraryDroid.IAlbums.GetAlbum(ILibraryDroid.IIdentifiers? identifiers)
			{
				if (identifiers is null)
					return null;

				IAlbum? album = SQLiteLibrary.AlbumsTable.AsEnumerable().FirstOrDefault(album => identifiers.MatchesAlbum(album));

				return album;
			}
			async Task<IAlbum?> ILibraryDroid.IAlbums.GetAlbum(ILibraryDroid.IIdentifiers? identifiers, CancellationToken cancellationToken)
			{
				if (identifiers is null)
					return null;

				IAlbum? album = SQLiteLibrary.AlbumsTable.AsEnumerable().FirstOrDefault(album => identifiers.MatchesAlbum(album));

				return await Task.FromResult(album);
			}
			IEnumerable<IAlbum> ILibraryDroid.IAlbums.GetAlbums(ILibraryDroid.IIdentifiers? identifiers)
			{
				IEnumerable<IAlbum> albums = identifiers is null
					? SQLiteLibrary.AlbumsTable
					: SQLiteLibrary.AlbumsTable.AsEnumerable().Where(_ => identifiers.MatchesAlbum(_));

				foreach (IAlbum album in albums)
					yield return album;
			}
			async IAsyncEnumerable<IAlbum> ILibraryDroid.IAlbums.GetAlbums(ILibraryDroid.IIdentifiers? identifiers, [EnumeratorCancellation] CancellationToken cancellationToken)
			{
				IEnumerable<IAlbum> albums = identifiers is null
					? SQLiteLibrary.AlbumsTable
					: SQLiteLibrary.AlbumsTable.AsEnumerable().Where(_ => identifiers.MatchesAlbum(_));

				foreach (IAlbum album in albums)
					yield return await Task.FromResult(album);
			}

			IAlbum? ILibraryDroid.IAlbums.PopulateAlbum(IAlbum? album)
			{
				if (album is null)
					return null;

				if (SQLiteLibrary.AlbumsTable.FirstOrDefault(sqlalbum => sqlalbum.Id == album.Id) is AlbumEntity albumentity)
					album.Populate(albumentity);

				return album;
			}
			async Task<IAlbum?> ILibraryDroid.IAlbums.PopulateAlbum(IAlbum? album, CancellationToken cancellationToken)
			{
				if (album is null)
					return null;

				if (await SQLiteLibrary.AlbumsTableAsync.FirstOrDefaultAsync(sqlalbum => sqlalbum.Id == album.Id) is AlbumEntity albumentity)
					album.Populate(albumentity);

				return album;
			}
			IEnumerable<IAlbum>? ILibraryDroid.IAlbums.PopulateAlbums(IEnumerable<IAlbum>? albums)
			{
				if (albums is null)
					return null;

				foreach (IAlbum album in albums)
					(this as ILibraryDroid.IAlbums).PopulateAlbum(album);

				return albums;
			}
			async Task<IEnumerable<IAlbum>?> ILibraryDroid.IAlbums.PopulateAlbums(IEnumerable<IAlbum>? albums, CancellationToken cancellationToken)
			{
				if (albums is null)
					return null;

				foreach (IAlbum album in albums)
					await (this as ILibraryDroid.IAlbums).PopulateAlbum(album, cancellationToken);

				return albums;
			}

			bool ILibraryDroid.IAlbums.DeleteAlbum(IAlbum album)
			{
				if (Actions?.OnDelete != null)
					foreach (ILibraryDroid.IOnDeleteActions ondeleteaction in Actions.OnDelete)
						ondeleteaction.Album(album);

				AlbumEntity albumentity = album as AlbumEntity ?? new AlbumEntity(album);

				SQLiteLibrary.Connection.Delete(albumentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IAlbums.DeleteAlbum(IAlbum album, CancellationToken cancellationToken)
			{
				if (Actions?.OnDelete != null)
					await Task.WhenAll(Actions.OnDelete.Select(_ => _.Album(album)));

				AlbumEntity albumentity = album as AlbumEntity ?? new AlbumEntity(album);

				await SQLiteLibrary.ConnectionAsync.DeleteAsync(albumentity);

				return true;
			}

			bool ILibraryDroid.IAlbums.UpdateAlbum(IAlbum old, IAlbum updated)
			{
				if (Actions?.OnUpdate != null)
					foreach (ILibraryDroid.IOnUpdateActions onupdateaction in Actions.OnUpdate)
						onupdateaction.Album(old, updated);

				AlbumEntity albumentity = updated as AlbumEntity ?? new AlbumEntity(updated);

				SQLiteLibrary.Connection.Update(albumentity);

				return true;
			}
			async Task<bool> ILibraryDroid.IAlbums.UpdateAlbum(IAlbum old, IAlbum updated, CancellationToken cancellationToken)
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