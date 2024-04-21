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
	public sealed partial class XyzuLibrary : ILibrary.IAlbums
	{
		IAlbum? ILibrary.IAlbums.Random(ILibrary.IIdentifiers? identifiers, IAlbum<bool>? retriever)
		{
			IEnumerable<IAlbum> albums = SqliteAlbumsTableQuery;

			if (identifiers != null)
				albums = albums.Where(album => identifiers.MatchesAlbum(album));

			Random random = new Random();
			int index = random.Next(0, albums.Count() - 1);
			IAlbum album = albums.ElementAt(index);

			return album;
		}
		async Task<IAlbum?> ILibrary.IAlbums.Random(ILibrary.IIdentifiers? identifiers, IAlbum<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IAlbums).Random(identifiers, retriever);
			});
		}

		IAlbum? ILibrary.IAlbums.GetAlbum(ILibrary.IIdentifiers? identifiers, IAlbum<bool>? retriever)
		{
			if (identifiers is null)
				return null;

			IEnumerable<IAlbum> albums = SqliteAlbumsTableQuery;
			IAlbum? album = albums.FirstOrDefault(album => identifiers.MatchesAlbum(album));

			return album;
		}
		async Task<IAlbum?> ILibrary.IAlbums.GetAlbum(ILibrary.IIdentifiers? identifiers, IAlbum<bool>? retriever, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IAlbums).GetAlbum(identifiers, retriever);
			});
		}
		IEnumerable<IAlbum> ILibrary.IAlbums.GetAlbums(ILibrary.IIdentifiers? identifiers, IAlbum<bool>? retriever)
		{
			IEnumerable<IAlbum> albums = SqliteAlbumsTableQuery;

			if (identifiers != null)
				albums = albums.Where(album => identifiers.MatchesAlbum(album));

			if (retriever != null)
				albums = albums.Select(album =>
				{
					return album;
				});

			foreach (IAlbum album in albums)
				yield return album;
		}
		async IAsyncEnumerable<IAlbum> ILibrary.IAlbums.GetAlbums(ILibrary.IIdentifiers? identifiers, IAlbum<bool>? retriever, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			IEnumerable<IAlbum> albums = await Task.Run(() =>
			{
				return (this as ILibrary.IAlbums).GetAlbums(identifiers, retriever);
			});

			foreach (IAlbum album in albums)
				yield return album;
		}

		IAlbum? ILibrary.IAlbums.PopulateAlbum(IAlbum? album)
		{
			if (album is null)
				return null;

			IEnumerable<IAlbum> albums = SqliteAlbumsTableQuery;

			if (albums.FirstOrDefault(predicate: sqlalbum => sqlalbum.Id == album.Id) is AlbumEntity albumentity)
				album.Populate(albumentity);

			return album;
		}
		async Task<IAlbum?> ILibrary.IAlbums.PopulateAlbum(IAlbum? album, CancellationToken cancellationToken)
		{
			return await Task.Run(() =>
			{
				return (this as ILibrary.IAlbums).PopulateAlbum(album);
			});
		}
		IEnumerable<IAlbum>? ILibrary.IAlbums.PopulateAlbums(IEnumerable<IAlbum>? albums)
		{
			if (albums is null)
				return null;

			foreach (IAlbum album in albums)
				(this as ILibrary.IAlbums).PopulateAlbum(album);

			return albums;
		}
		async Task<IEnumerable<IAlbum>?> ILibrary.IAlbums.PopulateAlbums(IEnumerable<IAlbum>? albums, CancellationToken cancellationToken)
		{
			if (albums is null)
				return null;

			foreach (IAlbum album in albums)
				await (this as ILibrary.IAlbums).PopulateAlbum(album, cancellationToken);

			return albums;
		}

		bool ILibrary.IAlbums.DeleteAlbum(IAlbum album)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Album(album);

			AlbumEntity albumentity = album as AlbumEntity ?? new AlbumEntity(album);

			SqliteConnection.Delete(albumentity);

			return true;
		}
		async Task<bool> ILibrary.IAlbums.DeleteAlbum(IAlbum album, CancellationToken cancellationToken)
		{
			if (OnDeleteActions != null)
				foreach (ILibrary.IOnDeleteActions ondeleteaction in OnDeleteActions)
					ondeleteaction.Album(album);

			AlbumEntity albumentity = album as AlbumEntity ?? new AlbumEntity(album);

			await SqliteConnectionAsync.DeleteAsync(albumentity);

			return true;
		}

		bool ILibrary.IAlbums.UpdateAlbum(IAlbum old, IAlbum updated)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Album(old, updated);

			AlbumEntity albumentity = updated as AlbumEntity ?? new AlbumEntity(updated);

			SqliteConnection.Update(albumentity);

			return true;
		}
		async Task<bool> ILibrary.IAlbums.UpdateAlbum(IAlbum old, IAlbum updated, CancellationToken cancellationToken)
		{
			if (OnUpdateActions != null)
				foreach (ILibrary.IOnUpdateActions onupdateaction in OnUpdateActions)
					onupdateaction.Album(old, updated);

			AlbumEntity albumentity = updated as AlbumEntity ?? new AlbumEntity(updated);

			await SqliteConnectionAsync.UpdateAsync(albumentity);

			return true;
		}
	}
}