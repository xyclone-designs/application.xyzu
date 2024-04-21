#nullable enable

using Android.Content;
using Android.Media;

using System;
using System.Collections.Generic;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;

namespace Xyzu.Library.MediaMetadata
{
	public partial class MediaMetadataActions
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
			private MediaMetadataRetriever? _MetadataRetriever;

			public Context? Context { get; set; }
			public MediaMetadataRetriever MetadataRetriever
			{
				set => _MetadataRetriever = value;
				get => _MetadataRetriever ??= new MediaMetadataRetriever();
			}

			bool SetMetadataRetrieverDataSource(string? filepath, Uri? uri)
			{
				if (uri?.ToAndroidUri() is AndroidUri androiduri)
					try
					{
						MetadataRetriever.SetDataSource(Context, androiduri);

						return true;
					}
					catch (Exception) { }

				if (filepath != null)
					try
					{
						MetadataRetriever.SetDataSource(filepath);

						return true;
					}
					catch (Exception) { }

				return false;
			}

			public override void Album(IAlbum? retrieved, IAlbum<bool>? retriever, ISong? retrievedsong)
			{
				if (SetMetadataRetrieverDataSource(retrievedsong?.Filepath, retrievedsong?.Uri))
				{
					retrieved?.Retrieve(MetadataRetriever, retriever);

					MetadataRetriever?.Close();
				}

				base.Album(retrieved, retriever, retrievedsong);
			}
			public override void Artist(IArtist? retrieved, IArtist<bool>? retriever, IAlbum? retrievedalbum)
			{
				base.Artist(retrieved, retriever, retrievedalbum);
			}
			public override void Artist(IArtist? retrieved, IArtist<bool>? retriever, ISong? retrievedsong)
			{
				if (SetMetadataRetrieverDataSource(retrievedsong?.Filepath, retrievedsong?.Uri))
				{
					retrieved?.Retrieve(MetadataRetriever, retriever);

					MetadataRetriever?.Close();
				}

				base.Artist(retrieved, retriever, retrievedsong);
			}
			public override void Genre(IGenre? retrieved, IGenre<bool>? retriever, ISong? retrievedsong)
			{
				if (SetMetadataRetrieverDataSource(retrievedsong?.Filepath, retrievedsong?.Uri))
				{
					retrieved?.Retrieve(MetadataRetriever, retriever);

					MetadataRetriever?.Close();
				}

				base.Genre(retrieved, retriever, retrievedsong);
			}
			public override void Playlist(IPlaylist? retrieved, IPlaylist<bool>? retriever, ISong? retrievedsong)
			{
				if (SetMetadataRetrieverDataSource(retrievedsong?.Filepath, retrievedsong?.Uri))
				{
					retrieved?.Retrieve(MetadataRetriever, retriever);

					MetadataRetriever?.Close();
				}

				base.Playlist(retrieved, retriever, retrievedsong);
			}
			public override void Song(ISong? retrieved, ISong<bool>? retriever)
			{
				if (SetMetadataRetrieverDataSource(retrieved?.Filepath, retrieved?.Uri))
				{
					retrieved?.Retrieve(MetadataRetriever, retriever);

					MetadataRetriever?.Close();
				}

				base.Song(retrieved, retriever);
			}
			public override void Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes)
			{
				if (SetMetadataRetrieverDataSource(filepath, uri))
				{
					retrieved?.Retrieve(retriever, MetadataRetriever);

					MetadataRetriever?.Close();
				}

				base.Image(retrieved, retriever, filepath, uri, modeltypes);
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
				base.Song(old, updated);
			}
		}
	}
}
