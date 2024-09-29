using System;
using System.IO;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Library.IO
{
	public partial class IOActions
	{
		public class OnCreate : ILibrary.IOnCreateActions.Default { }
		public class OnDelete : ILibrary.IOnDeleteActions.Default { }
		public class OnRetrieve : ILibrary.IOnRetrieveActions.Default
		{
			static FileInfo? GetFileInfo(string? filepath, Uri? uri)
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

			public override Task Album(IAlbum? retrieved, ISong? retrievedsong)
			{
				if (GetFileInfo(retrievedsong?.Filepath, retrievedsong?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo);

				return base.Album(retrieved, retrievedsong);
			}
			public override Task Artist(IArtist? retrieved, ISong? retrievedsong)
			{
				if (GetFileInfo(retrievedsong?.Filepath, retrievedsong?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo);

				return base.Artist(retrieved, retrievedsong);
			}
			public override Task Genre(IGenre? retrieved, ISong? retrievedsong)
			{
				if (GetFileInfo(retrievedsong?.Filepath, retrievedsong?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo);

				return base.Genre(retrieved, retrievedsong);
			}
			public override Task Playlist(IPlaylist? retrieved, ISong? retrievedsong)
			{
				if (GetFileInfo(retrievedsong?.Filepath, retrievedsong?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo);

				return base.Playlist(retrieved, retrievedsong);
			}
			public override Task Song(ISong? retrieved)
			{
				if (GetFileInfo(retrieved?.Filepath, retrieved?.Uri) is FileInfo fileinfo)
					retrieved?.Retrieve(fileinfo);

				return base.Song(retrieved);
			}
		}
		public class OnUpdate : ILibrary.IOnUpdateActions.Default
		{
			public override Task Song(ISong? old, ISong? updated)
			{
				if (updated?.Filepath is not null)
					try
					{
						FileInfo fileinfo = new(updated.Filepath);

						DateTime datetimenow = DateTime.Now;

						fileinfo.LastWriteTime = datetimenow;
						fileinfo.LastWriteTimeUtc = datetimenow.ToUniversalTime();
					}
					catch (Exception) { }

				return base.Song(old, updated);
			}
		}
	}
}
