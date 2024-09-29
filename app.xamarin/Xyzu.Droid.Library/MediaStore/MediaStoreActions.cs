#nullable enable

using Android.Content;
using Android.Database;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;
using Store = Android.Provider.MediaStore;
using JavaFile = Java.IO.File;

namespace Xyzu.Library.MediaStore
{
	public partial class MediaStoreActions
	{
		public class OnCreate : ILibraryDroid.IOnCreateActions.Default
		{
			public ICursor? Cursor { get; set; }
			public JavaFile? Directory { get; set; }
			public CancellationToken? Cancellationtoken { get; set; }

			public async override Task<ISong?> Song(string id)
			{
				ISong song = await base.Song(id) ?? new ISong.Default(id);

				if (Cursor != null)
					await Task
						.Run(() => song.Retrieve(Cursor, Directory))
						.ConfigureAwait(false);

				return song;
			}
		}
		public class OnDelete : ILibraryDroid.IOnDeleteActions.Default
		{
			public Context? Context { get; set; }
			
			public async override Task Album(IAlbum album)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri albumuri = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromAlbum(album);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: albumuri,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Album(album);
			}
			public async override Task Artist(IArtist artist)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri artisturi = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromArtist(artist);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: artisturi,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Artist(artist);
			}
			public async override Task Genre(IGenre genre)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri genreuri = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromGenre(genre);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: genreuri,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Genre(genre);
			}
			public async override Task Playlist(IPlaylist playlist)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri playlisturi = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromPlaylist(playlist);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: playlisturi,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Playlist(playlist);
			}
			public async override Task Song(ISong song)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri songuri = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromSong(song);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: songuri,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Song(song);
			}
		}
		public class OnRetrieve : ILibraryDroid.IOnRetrieveActions.Default
		{
			public Context? Context { get; set; }
			public ICursor? Cursor { get; set; }
			public JavaFile? Directory { get; set; }
			public CancellationToken? Cancellationtoken { get; set; }

			public async override Task Album(IAlbum? retrieved, ISong? retrievedsong)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, false, Cancellationtoken ?? default);

				await base.Album(retrieved, retrievedsong);
			}
			public async override Task Artist(IArtist? retrieved, IAlbum? retrievedalbum)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, false, Cancellationtoken ?? default);

				await base.Artist(retrieved, retrievedalbum);
			}
			public async override Task Artist(IArtist? retrieved, ISong? retrievedsong)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, false, Cancellationtoken ?? default);

				await base.Artist(retrieved, retrievedsong);
			}
			public async override Task Genre(IGenre? retrieved, ISong? retrievedsong)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, false, Cancellationtoken ?? default);

				await base.Genre(retrieved, retrievedsong);
			}
			public async override Task Playlist(IPlaylist? retrieved, ISong? retrievedsong)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, false, Cancellationtoken ?? default);

				await base.Playlist(retrieved, retrievedsong);
			}
			public async override Task Song(ISong? retrieved)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, Directory, false, Cancellationtoken ?? default);

				await base.Song(retrieved);
			}
			public async override Task Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes)
			{
				if (Cursor != null && Context != null)
					retrieved?.Retrieve(Cursor, Context, retriever);

				await base.Image(retrieved, retriever, filepath, uri, modeltypes);
			}
		}
		public class OnUpdate : ILibraryDroid.IOnUpdateActions.Default
		{
			public Context? Context { get; set; }

			public async override Task Album(IAlbum? old, IAlbum? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri albumuri = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromAlbum(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdateAlbum(old, updated);

				Context.ContentResolver.Update(
					uri: albumuri,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Album(old, updated);
			}
			public async override Task Artist(IArtist? old, IArtist? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri artisturi = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromArtist(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdateArtist(old, updated);

				Context.ContentResolver.Update(
					uri: artisturi,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Artist(old, updated);
			}
			public async override Task Genre(IGenre? old, IGenre? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri genreuri = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromGenre(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdateGenre(old, updated);

				Context.ContentResolver.Update(
					uri: genreuri,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Genre(old, updated);
			}
			public async override Task Playlist(IPlaylist? old, IPlaylist? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri playlisturi = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromPlaylist(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdatePlaylist(old, updated);

				Context.ContentResolver.Update(
					uri: playlisturi,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Playlist(old, updated);
			}
			public async override Task Song(ISong? old, ISong? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri songuri = Store.Audio.Media.ExternalContentUri;
				ILibraryDroid.IIdentifiers identifiers = ILibraryDroid.IIdentifiers.FromSong(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdateSong(old, updated);

				Context.ContentResolver.Update(
					uri: songuri,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				await base.Song(old, updated);
			}
		}
	}
}
