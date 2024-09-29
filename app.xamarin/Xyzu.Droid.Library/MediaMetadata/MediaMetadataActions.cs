#nullable enable

using Android.Content;
using Android.Media;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;

namespace Xyzu.Library.MediaMetadata
{
	public partial class MediaMetadataActions
	{
		public class OnCreate : ILibraryDroid.IOnCreateActions.Default { }
		public class OnDelete : ILibraryDroid.IOnDeleteActions.Default { }
		public class OnRetrieve : ILibraryDroid.IOnRetrieveActions.Default
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
				bool value = false;

				if (uri?.ToAndroidUri() is AndroidUri androiduri)
					try
					{
						MetadataRetriever.SetDataSource(Context, androiduri);

						value = true;
					}
					catch (Exception) { }

				if (filepath != null)
					try
					{
						MetadataRetriever.SetDataSource(filepath);

						value = true;
					}
					catch (Exception) { }

				return value;
			}

			public async override Task Album(IAlbum? retrieved, ISong? retrievedsong)
			{
				if (SetMetadataRetrieverDataSource(retrievedsong?.Filepath, retrievedsong?.Uri))
					await Task
						.Run(() => retrieved?.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Album(retrieved, retrievedsong);
			}
			public async override Task Artist(IArtist? retrieved, ISong? retrievedsong)
			{
				if (SetMetadataRetrieverDataSource(retrievedsong?.Filepath, retrievedsong?.Uri))
					await Task
						.Run(() => retrieved?.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Artist(retrieved, retrievedsong);
			}
			public async override Task Genre(IGenre? retrieved, ISong? retrievedsong)
			{
				if (SetMetadataRetrieverDataSource(retrievedsong?.Filepath, retrievedsong?.Uri))
					await Task
						.Run(() => retrieved?.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Genre(retrieved, retrievedsong);
			}
			public async override Task Playlist(IPlaylist? retrieved, ISong? retrievedsong)
			{
				if (SetMetadataRetrieverDataSource(retrievedsong?.Filepath, retrievedsong?.Uri))
					await Task
						.Run(() => retrieved?.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Playlist(retrieved, retrievedsong);
			}
			public async override Task Song(ISong? retrieved)
			{
				if (SetMetadataRetrieverDataSource(retrieved?.Filepath, retrieved?.Uri))
					await Task
						.Run(() => retrieved?.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Song(retrieved);
			}
			public async override Task Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes)
			{
				if (SetMetadataRetrieverDataSource(filepath, uri))
					await Task
						.Run(() => retrieved?.Retrieve(retriever, MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Image(retrieved, retriever, filepath, uri, modeltypes);
			}
		}
		public class OnUpdate : ILibraryDroid.IOnUpdateActions.Default { }
	}
}
