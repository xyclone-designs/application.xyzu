using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibrary
	{
		public interface IActions 
		{
			ISettings? Settings { get; set; }

			public class Default : IActions
			{
				public ISettings? Settings { get; set; }
			}
		}
		public interface IOnCreateActions : IActions
		{
			IAlbum? Album(string id, IAlbum<bool>? retriever);
			IArtist? Artist(string id, IArtist<bool>? retriever);
			IGenre? Genre(string id, IGenre<bool>? retriever);
			IPlaylist? Playlist(string id, IPlaylist<bool>? retriever);
			ISong? Song(string id, ISong<bool>? retriever);

			public new class Default: IActions.Default, IOnCreateActions
			{
				public virtual IAlbum? Album(string id, IAlbum<bool>? retriever) { return null; }
				public virtual IArtist? Artist(string id, IArtist<bool>? retriever) { return null; }
				public virtual IGenre? Genre(string id, IGenre<bool>? retriever) { return null; }
				public virtual IPlaylist? Playlist(string id, IPlaylist<bool>? retriever) { return null; }
				public virtual ISong? Song(string id, ISong<bool>? retriever) { return null; }
			}
		}
		public interface IOnDeleteActions : IActions
		{
			void Album(IAlbum album);
			void Artist(IArtist artist);
			void Genre(IGenre genre);
			void Playlist(IPlaylist playlist);
			void Song(ISong song);

			public new class Default : IActions.Default, IOnDeleteActions
			{
				public virtual void Album(IAlbum album) { }
				public virtual void Artist(IArtist artist) { }
				public virtual void Genre(IGenre genre) { }
				public virtual void Playlist(IPlaylist playlist) { }
				public virtual void Song(ISong song) { }
			}
		}
		public interface IOnRetrieveActions : IActions
		{
			void Album(IAlbum? retrieved, IAlbum<bool>? retriever, ISong? retrievedsong);
			void Artist(IArtist? retrieved, IArtist<bool>? retriever, IAlbum? retrievedalbum);
			void Artist(IArtist? retrieved, IArtist<bool>? retriever, ISong? retrievedsong);
			void Genre(IGenre? retrieved, IGenre<bool>? retriever, ISong? retrievedsong);
			void Playlist(IPlaylist? retrieved, IPlaylist<bool>? retriever, ISong? retrievedsong);
			void Song(ISong? retrieved, ISong<bool>? retriever);
			void Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes);

			public new class Default : IActions.Default, IOnRetrieveActions
			{
				public virtual void Album(IAlbum? retrieved, IAlbum<bool>? retriever, ISong? retrievedsong) { }
				public virtual void Artist(IArtist? retrieved, IArtist<bool>? retriever, IAlbum? retrievedalbum) { }
				public virtual void Artist(IArtist? retrieved, IArtist<bool>? retriever, ISong? retrievedsong) { }
				public virtual void Genre(IGenre? retrieved, IGenre<bool>? retriever, ISong? retrievedsong) { }
				public virtual void Playlist(IPlaylist? retrieved, IPlaylist<bool>? retriever, ISong? retrievedsong) { }
				public virtual void Song(ISong? retrieved, ISong<bool>? retriever) { }
				public virtual void Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes) { }
			}
		}
		public interface IOnUpdateActions : IActions
		{
			void Album(IAlbum? old, IAlbum? updated);
			void Artist(IArtist? old, IArtist? updated);
			void Genre(IGenre? old, IGenre? updated);
			void Playlist(IPlaylist? old, IPlaylist? updated);
			void Song(ISong? old, ISong? updated);

			public new class Default : IActions.Default, IOnUpdateActions
			{
				public virtual void Album(IAlbum? old, IAlbum? updated) { }
				public virtual void Artist(IArtist? old, IArtist? updated) { }
				public virtual void Genre(IGenre? old, IGenre? updated) { }
				public virtual void Playlist(IPlaylist? old, IPlaylist? updated) { }
				public virtual void Song(ISong? old, ISong? updated) { }

				public Func<string?, ISong?>? OnGetSong { get; set; }
			}
		}
	}
}
