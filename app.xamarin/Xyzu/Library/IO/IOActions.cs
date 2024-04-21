using System;
using System.IO;

using Xyzu.Library.Models;

namespace Xyzu.Library.IO
{
	public partial class IOActions
	{
		public class OnCreate : ILibrary.IOnCreateActions.Default
		{
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
				return base.Song(id, retriever);
			}
		}
		public class OnDelete : ILibrary.IOnDeleteActions.Default
		{
			public override void Album(IAlbum album)
			{
				base.Album(album);
			}
			public override void Artist(IArtist artist)
			{
				base.Artist(artist);
			}
			public override void Genre(IGenre genre)
			{
				base.Genre(genre);
			}
			public override void Playlist(IPlaylist playlist)
			{
				base.Playlist(playlist);
			}
			public override void Song(ISong song)
			{
				base.Song(song);
			}
		}
		public class OnRetrieve : ILibrary.IOnRetrieveActions.Default
		{
			FileInfo? GetFileInfo(string? filepath, Uri? uri)
			{
				FileInfo? fileinfo = null;

				if (fileinfo is null && filepath is not null)
					try { fileinfo = new FileInfo(filepath); } 
					catch (Exception) { }		 

				if (fileinfo is null && uri is not null)
					try { fileinfo = new FileInfo(uri.AbsolutePath); } 
					catch (Exception) { }

				return fileinfo;
			}

			public override void Album(IAlbum? retrieved, IAlbum<bool>? retriever, ISong? retrievedsong)
			{
				if (GetFileInfo(retrievedsong?.Filepath, retrievedsong?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo, retriever);

				base.Album(retrieved, retriever, retrievedsong);
			}
			public override void Artist(IArtist? retrieved, IArtist<bool>? retriever, IAlbum? retrievedalbum)
			{
				base.Artist(retrieved, retriever, retrievedalbum);
			}
			public override void Artist(IArtist? retrieved, IArtist<bool>? retriever, ISong? retrievedsong)
			{
				if (GetFileInfo(retrievedsong?.Filepath, retrievedsong?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo, retriever);

				base.Artist(retrieved, retriever, retrievedsong);
			}
			public override void Genre(IGenre? retrieved, IGenre<bool>? retriever, ISong? retrievedsong)
			{
				if (GetFileInfo(retrievedsong?.Filepath, retrievedsong?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo, retriever);

				base.Genre(retrieved, retriever, retrievedsong);
			}
			public override void Playlist(IPlaylist? retrieved, IPlaylist<bool>? retriever, ISong? retrievedsong)
			{
				if (GetFileInfo(retrievedsong?.Filepath, retrievedsong?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo, retriever);

				base.Playlist(retrieved, retriever, retrievedsong);
			}
			public override void Song(ISong? retrieved, ISong<bool>? retriever)
			{
				if (GetFileInfo(retrieved?.Filepath, retrieved?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo, retriever);

				base.Song(retrieved, retriever);
			}
		}
		public class OnUpdate : ILibrary.IOnUpdateActions.Default
		{
			public override void Album(IAlbum? old, IAlbum? updated)
			{
				base.Album(old, updated);
			}
			public override void Artist(IArtist? old, IArtist? updated)
			{
				base.Artist(old, updated);
			}
			public override void Genre(IGenre? old, IGenre? updated)
			{
				base.Genre(old, updated);
			}
			public override void Playlist(IPlaylist? old, IPlaylist? updated)
			{
				base.Playlist(old, updated);
			}
			public override void Song(ISong? old, ISong? updated)
			{
				if (updated?.Filepath is not null)
					try
					{
						FileInfo fileinfo = new(updated?.Filepath);

						DateTime datetimenow = DateTime.Now;

						fileinfo.LastWriteTime = datetimenow;
						fileinfo.LastWriteTimeUtc = datetimenow.ToUniversalTime();
					}
					catch (Exception) { }

				base.Song(old, updated);
			}
		}
	}
}
