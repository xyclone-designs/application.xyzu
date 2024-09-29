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
			public class Container
			{
				public ILibrary.IOnCreateActions? OnCreate { get; set; }
				public IEnumerable<ILibrary.IOnDeleteActions>? OnDelete { get; set; }
				public IEnumerable<ILibrary.IOnRetrieveActions>? OnRetrieve { get; set; }
				public IEnumerable<ILibrary.IOnUpdateActions>? OnUpdate { get; set; }
			}
		}
		public interface IOnCreateActions : IActions
		{
			IDictionary<string, string>? Paths { get; set; }

			Task<IAlbum?> Album(string id);
			Task<IArtist?> Artist(string id);
			Task<IGenre?> Genre(string id);
			Task<IPlaylist?> Playlist(string id);
			Task<ISong?> Song(string id);

			public new class Default: IActions.Default, IOnCreateActions
			{
				public IDictionary<string, string>? Paths { get; set; }

				public virtual Task<IAlbum?> Album(string id) { return Task.FromResult<IAlbum?>(null); }
				public virtual Task<IArtist?> Artist(string id) { return Task.FromResult<IArtist?>(null); }
				public virtual Task<IGenre?> Genre(string id) { return Task.FromResult<IGenre?>(null); }
				public virtual Task<IPlaylist?> Playlist(string id) { return Task.FromResult<IPlaylist?>(null); }
				public virtual Task<ISong?> Song(string id) { return Task.FromResult<ISong?>(null); }
			}
		}
		public interface IOnDeleteActions : IActions
		{
			Task Album(IAlbum album);
			Task Artist(IArtist artist);
			Task Genre(IGenre genre);
			Task Playlist(IPlaylist playlist);
			Task Song(ISong song);

			public new class Default : IActions.Default, IOnDeleteActions
			{
				public virtual Task Album(IAlbum album) { return Task.CompletedTask; }
				public virtual Task Artist(IArtist artist) { return Task.CompletedTask; }
				public virtual Task Genre(IGenre genre) { return Task.CompletedTask; }
				public virtual Task Playlist(IPlaylist playlist) { return Task.CompletedTask; }
				public virtual Task Song(ISong song) { return Task.CompletedTask; }
			}
		}
		public interface IOnRetrieveActions : IActions
		{
			Task Album(IAlbum? retrieved, ISong? retrievedsong);
			Task Artist(IArtist? retrieved, IAlbum? retrievedalbum);
			Task Artist(IArtist? retrieved, ISong? retrievedsong);
			Task Genre(IGenre? retrieved, ISong? retrievedsong);
			Task Playlist(IPlaylist? retrieved, ISong? retrievedsong);
			Task Song(ISong? retrieved);
			Task Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes);

			public new class Default : IActions.Default, IOnRetrieveActions
			{
				public virtual Task Album(IAlbum? retrieved, ISong? retrievedsong) { return Task.CompletedTask; }
				public virtual Task Artist(IArtist? retrieved, IAlbum? retrievedalbum) { return Task.CompletedTask; }
				public virtual Task Artist(IArtist? retrieved, ISong? retrievedsong) { return Task.CompletedTask; }
				public virtual Task Genre(IGenre? retrieved, ISong? retrievedsong) { return Task.CompletedTask; }
				public virtual Task Playlist(IPlaylist? retrieved, ISong? retrievedsong) { return Task.CompletedTask; }
				public virtual Task Song(ISong? retrieved) { return Task.CompletedTask; }
				public virtual Task Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes) { return Task.CompletedTask; }
			}
		}
		public interface IOnUpdateActions : IActions
		{
			Task Album(IAlbum? old, IAlbum? updated);
			Task Artist(IArtist? old, IArtist? updated);
			Task Genre(IGenre? old, IGenre? updated);
			Task Playlist(IPlaylist? old, IPlaylist? updated);
			Task Song(ISong? old, ISong? updated);

			public new class Default : IActions.Default, IOnUpdateActions
			{
				public virtual Task Album(IAlbum? old, IAlbum? updated) { return Task.CompletedTask; }
				public virtual Task Artist(IArtist? old, IArtist? updated) { return Task.CompletedTask; }
				public virtual Task Genre(IGenre? old, IGenre? updated) { return Task.CompletedTask; }
				public virtual Task Playlist(IPlaylist? old, IPlaylist? updated) { return Task.CompletedTask; }
				public virtual Task Song(ISong? old, ISong? updated) { return Task.CompletedTask; }

				public Func<string?, Task<ISong?>>? OnGetSong { get; set; }
			}
		}
	}
}
