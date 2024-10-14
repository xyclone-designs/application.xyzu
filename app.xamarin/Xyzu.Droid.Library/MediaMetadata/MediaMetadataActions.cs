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
		public static bool SetMetadataRetrieverDataSource(Context? context, MediaMetadataRetriever metadataretriever, ISong? song)
		{
			return SetMetadataRetrieverDataSource(context, metadataretriever, song?.Uri?.ToAndroidUri(), song?.Filepath);
		}
		public static bool SetMetadataRetrieverDataSource(Context? context, MediaMetadataRetriever metadataretriever, AndroidUri? uri, string? filepath)
		{
			bool value = false;

			if (filepath is not null)
				try
				{
					metadataretriever.SetDataSource(filepath);

					value = true;
				}
				catch (Exception) { }

			if (value is false && uri is not null)
				try
				{
					metadataretriever.SetDataSource(context, uri);

					value = true;
				}
				catch (Exception) { }

			return value;
		}

		public class OnCreate : ILibraryDroid.IOnCreateActions.Default 
		{
			private MediaMetadataRetriever? _MetadataRetriever;

			public Context? Context { get; set; }
			public MediaMetadataRetriever MetadataRetriever
			{
				set => _MetadataRetriever = value;
				get => _MetadataRetriever ??= new MediaMetadataRetriever();
			}

			public override async Task<ISong?> Song(string id)
			{
				Paths ??= OnPaths?.Invoke();

				if (Paths?[id] is not string filepath)
					return null;

				ISong song = new ISong.Default(id)
				{
					Filepath = filepath,
					Uri = Uri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out Uri? outuri) ? outuri : null,
				};

				if (SetMetadataRetrieverDataSource(Context, MetadataRetriever, song) is false)
					return null;

				await Task
					.Run(() => song.Retrieve(MetadataRetriever))
					.ContinueWith(_ => MetadataRetriever.Release());

				return song;
			}
		}
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

			public async override Task Album(IAlbum? retrieved, ISong? retrievedsong)
			{
				if (retrieved is not null &&
					retrievedsong is not null &&
					SetMetadataRetrieverDataSource(Context, MetadataRetriever, retrievedsong))
					await Task
						.Run(() => retrieved.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Album(retrieved, retrievedsong);
			}
			public async override Task Artist(IArtist? retrieved, ISong? retrievedsong)
			{
				if (retrieved is not null &&
					retrievedsong is not null &&
					SetMetadataRetrieverDataSource(Context, MetadataRetriever, retrievedsong))
					await Task
						.Run(() => retrieved.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Artist(retrieved, retrievedsong);
			}
			public async override Task Genre(IGenre? retrieved, ISong? retrievedsong)
			{
				if (retrieved is not null &&
					retrievedsong is not null &&
					SetMetadataRetrieverDataSource(Context, MetadataRetriever, retrievedsong))
					await Task
						.Run(() => retrieved.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Genre(retrieved, retrievedsong);
			}
			public async override Task Playlist(IPlaylist? retrieved, ISong? retrievedsong)
			{
				if (retrieved is not null &&
					retrievedsong is not null &&
					SetMetadataRetrieverDataSource(Context, MetadataRetriever, retrievedsong))
					await Task
						.Run(() => retrieved.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Playlist(retrieved, retrievedsong);
			}
			public async override Task Song(ISong? retrieved)
			{
				if (retrieved is not null &&
					SetMetadataRetrieverDataSource(Context, MetadataRetriever, retrieved))
					await Task
						.Run(() => retrieved.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Song(retrieved);
			}
			public async override Task Image(IImage? retrieved, IEnumerable<ModelTypes>? modeltypes)
			{
				if (retrieved is not null &&
					SetMetadataRetrieverDataSource(Context, MetadataRetriever, retrieved.Uri?.ToAndroidUri(), retrieved.Filepath))
					await Task
						.Run(() => retrieved.Retrieve(MetadataRetriever))
						.ContinueWith(_ => MetadataRetriever.Release());

				await base.Image(retrieved, modeltypes);
			}
		}
		public class OnUpdate : ILibraryDroid.IOnUpdateActions.Default { }
	}
}
