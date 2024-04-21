#nullable enable

using Android.App;
using Android.Content;
using Android.Database;

using System;
using System.Collections.Generic;
using System.Threading;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;
using Store = Android.Provider.MediaStore;

namespace Xyzu.Library.MediaStore
{
	public partial class MediaStoreActions
	{
		public class OnCreate : ILibrary.IOnCreateActions.Default
		{
			// TODO Asynchronise
			public ICursor? Cursor { get; set; }
			public CancellationToken? Cancellationtoken { get; set; }

			public override IAlbum? Album(string id, IAlbum<bool>? retriever)
			{
				return base.Album(id, retriever);
			}
			public override IArtist? Artist(string id, IArtist<bool>? retriever)
			{
				return base.Artist(id, retriever);
			}
			public override IGenre? Genre(string id, IGenre<bool>? retriever)
			{
				return base.Genre(id, retriever);
			}
			public override IPlaylist? Playlist(string id, IPlaylist<bool>? retriever)
			{
				return base.Playlist(id, retriever);
			}
			public override ISong? Song(string id, ISong<bool>? retriever)
			{
				ISong song = new ISong.Default(id);

				if (Cursor != null)
					song.Retrieve(Cursor, retriever);

				return song ?? base.Song(id, retriever);
			}
		}
		public class OnDelete : ILibrary.IOnDeleteActions.Default
		{
			public Context? Context { get; set; }
			
			public override void Album(IAlbum album)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri albumuri = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromAlbum(album);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: albumuri,
					where: selection,
					selectionArgs: selectionArgs);

				base.Album(album);
			}
			public override void Artist(IArtist artist)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri artisturi = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromArtist(artist);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: artisturi,
					where: selection,
					selectionArgs: selectionArgs);

				base.Artist(artist);
			}
			public override void Genre(IGenre genre)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri genreuri = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromGenre(genre);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: genreuri,
					where: selection,
					selectionArgs: selectionArgs);

				base.Genre(genre);
			}
			public override void Playlist(IPlaylist playlist)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri playlisturi = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromPlaylist(playlist);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: playlisturi,
					where: selection,
					selectionArgs: selectionArgs);

				base.Playlist(playlist);
			}
			public override void Song(ISong song)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null)
					return;

				AndroidUri songuri = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromSong(song);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);

				Context.ContentResolver.Delete(
					url: songuri,
					where: selection,
					selectionArgs: selectionArgs);

				base.Song(song);
			}
		}
		public class OnRetrieve : ILibrary.IOnRetrieveActions.Default
		{
			public Context? Context { get; set; }
			public ICursor? Cursor { get; set; }
			public CancellationToken? Cancellationtoken { get; set; }

			public override async void Album(IAlbum? retrieved, IAlbum<bool>? retriever, ISong? retrievedsong)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, retriever, false, Cancellationtoken ?? default);

				base.Album(retrieved, retriever, retrievedsong);
			}
			public override async void Artist(IArtist? retrieved, IArtist<bool>? retriever, IAlbum? retrievedalbum)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, retriever, false, Cancellationtoken ?? default);

				base.Artist(retrieved, retriever, retrievedalbum);
			}
			public override async void Artist(IArtist? retrieved, IArtist<bool>? retriever, ISong? retrievedsong)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, retriever, false, Cancellationtoken ?? default);

				base.Artist(retrieved, retriever, retrievedsong);
			}
			public override async void Genre(IGenre? retrieved, IGenre<bool>? retriever, ISong? retrievedsong)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, retriever, false, Cancellationtoken ?? default);

				base.Genre(retrieved, retriever, retrievedsong);
			}
			public override async void Playlist(IPlaylist? retrieved, IPlaylist<bool>? retriever, ISong? retrievedsong)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, retriever, false, Cancellationtoken ?? default);

				base.Playlist(retrieved, retriever, retrievedsong);
			}
			public override async void Song(ISong? retrieved, ISong<bool>? retriever)
			{
				if (Cursor != null && retrieved != null)
					await retrieved.RetrieveAsync(Cursor, retriever, false, Cancellationtoken ?? default);

				base.Song(retrieved, retriever);
			}
			public override void Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes)
			{
				if (Cursor != null && Context != null)
					retrieved?.Retrieve(Cursor, Context, retriever);

				base.Image(retrieved, retriever, filepath, uri, modeltypes);
			}
		}
		public class OnUpdate : ILibrary.IOnUpdateActions.Default
		{
			public Context? Context { get; set; }

			public override void Album(IAlbum? old, IAlbum? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri albumuri = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromAlbum(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdateAlbum(old, updated);

				Context.ContentResolver.Update(
					uri: albumuri,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				base.Album(old, updated);
			}
			public override void Artist(IArtist? old, IArtist? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri artisturi = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromArtist(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdateArtist(old, updated);

				Context.ContentResolver.Update(
					uri: artisturi,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				base.Artist(old, updated);
			}
			public override void Genre(IGenre? old, IGenre? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri genreuri = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromGenre(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdateGenre(old, updated);

				Context.ContentResolver.Update(
					uri: genreuri,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				base.Genre(old, updated);
			}
			public override void Playlist(IPlaylist? old, IPlaylist? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri playlisturi = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromPlaylist(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdatePlaylist(old, updated);

				Context.ContentResolver.Update(
					uri: playlisturi,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				base.Playlist(old, updated);
			}
			public override void Song(ISong? old, ISong? updated)
			{
				if (Context?.ContentResolver is null || Store.Audio.Media.ExternalContentUri is null || old is null || updated is null)
					return;

				AndroidUri songuri = Store.Audio.Media.ExternalContentUri;
				ILibrary.IIdentifiers identifiers = ILibrary.IIdentifiers.FromSong(old);
				(string selection, string[] selectionArgs) = identifiers.ToSelectionAndArgs(true, Settings);
				ContentValues contentvalues = new ContentValues()
					.UpdateSong(old, updated);

				Context.ContentResolver.Update(
					uri: songuri,
					values: contentvalues,
					where: selection,
					selectionArgs: selectionArgs);

				base.Song(old, updated);
			}
		}
	}
}
