using Android.App;
using Android.Database;

using SQLite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.FFProbe;
using Xyzu.Library.ID3;
using Xyzu.Library.IO;
using Xyzu.Library.MediaMetadata;
using Xyzu.Library.MediaStore;
using Xyzu.Library.Models;
using Xyzu.Library.TagLibSharp;

namespace Xyzu.Library
{
	public partial interface ILibraryDroid : ILibrary
	{
		IActions.Container? Actions { get; set; }
		CursorBuilder DefaultCursorBuilder { get; }
		SQLiteLibraryConnection SQLiteLibrary { get; set; }

		public static async Task OnRetrieve(IImage retrieved, IEnumerable<IOnRetrieveActions>? actions, IParameters? parameters) 
		{
			if (actions is null || actions.Any() is false)
				return;

			foreach (IOnRetrieveActions onretrieveaction in actions)
				switch (true)
				{
					case true when retrieved.Buffer != null:
						break;

					case true when onretrieveaction is FFProbeActions.OnRetrieve:
					case true when onretrieveaction is ID3Actions.OnRetrieve:
					case true when onretrieveaction is IOActions.OnRetrieve:
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve:
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve:
						await onretrieveaction.Image(retrieved, parameters?.ImageModelTypes);
						break;

					case true when 
						onretrieveaction is MediaStoreActions.OnRetrieve && 
						parameters is not null && (parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters)) is not null &&
						await parameters.Cursor.ToPosition(
							directory: parameters?.Directory,
							uri: parameters?.Uri,
							filepath: parameters?.Filepath,
							cursorpositions: parameters?.CursorPositions) is int:
						await onretrieveaction.Image(retrieved, parameters?.ImageModelTypes);
						break;

					default: break;
				}
		}
		public static async Task OnRetrieve<TModel>(TModel model, IEnumerable<IOnRetrieveActions>? actions, IParameters? parameters) where TModel : IModel
		{
			if (actions is null || actions.Any() is false)
				return;

			foreach (IOnRetrieveActions action in actions)
				switch (true)
				{
					case true when model is IAlbum album:
						switch (true)
						{
							case true when action is FFProbeActions.OnRetrieve:
							case true when action is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(album):
							case true when action is IOActions.OnRetrieve && IOUtils.WouldBenefit(album):
							case true when action is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(album):
							case true when action is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(album):
								await action.Album(album, parameters?.RetrievedSongExtra);
								break;

							case true when
								action is MediaStoreActions.OnRetrieve &&
								parameters is not null && (parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters)) is not null &&
								await parameters.Cursor.ToPosition(
									directory: parameters?.Directory,
									cursorpositions: parameters?.CursorPositions,
									uri: parameters?.RetrievedSongExtra?.Uri,
									filepath: parameters?.RetrievedSongExtra?.Filepath) is int:
								await action.Album(album, parameters?.RetrievedSongExtra);
								break;

							default: break;

						}
						break;

					case true when model is IArtist artist:
						switch (true)
						{
							case true when action is FFProbeActions.OnRetrieve:
							case true when action is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(artist):
							case true when action is IOActions.OnRetrieve && IOUtils.WouldBenefit(artist):
							case true when action is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(artist):
							case true when action is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(artist):
								await action.Artist(artist, parameters?.RetrievedSongExtra);
								break;
							case true when
								action is MediaStoreActions.OnRetrieve &&
								parameters is not null && (parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters)) is not null &&
								await parameters.Cursor.ToPosition(
									directory: parameters?.Directory,
									cursorpositions: parameters?.CursorPositions,
									uri: parameters?.RetrievedSongExtra?.Uri,
									filepath: parameters?.RetrievedSongExtra?.Filepath) is int:
								await action.Artist(artist, parameters?.RetrievedSongExtra);
								break;

							default: break;

						}
						break;

					case true when model is IGenre genre:
						switch (true)
						{
							case true when action is FFProbeActions.OnRetrieve:
							case true when action is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(genre):
							case true when action is IOActions.OnRetrieve && IOUtils.WouldBenefit(genre):
							case true when action is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(genre):
							case true when action is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(genre):
								await action.Genre(genre, parameters?.RetrievedSongExtra);
								break;

							case true when
								action is MediaStoreActions.OnRetrieve &&
								parameters is not null && (parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters)) is not null &&
								await parameters.Cursor.ToPosition(
									directory: parameters?.Directory,
									cursorpositions: parameters?.CursorPositions,
									uri: parameters?.RetrievedSongExtra?.Uri,
									filepath: parameters?.RetrievedSongExtra?.Filepath) is int:
								await action.Genre(genre, parameters?.RetrievedSongExtra);
								break;

							default: break;

						}
						break;

					case true when model is IPlaylist playlist:
						switch (true)
						{
							case true when action is FFProbeActions.OnRetrieve:
							case true when action is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(playlist):
							case true when action is IOActions.OnRetrieve && IOUtils.WouldBenefit(playlist):
							case true when action is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(playlist):
							case true when action is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(playlist):
								await action.Playlist(playlist, parameters?.RetrievedSongExtra);
								break;

							case true when
								action is MediaStoreActions.OnRetrieve &&
								parameters is not null && (parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters)) is not null &&
								await parameters.Cursor.ToPosition(
									directory: parameters?.Directory,
									cursorpositions: parameters?.CursorPositions,
									uri: parameters?.RetrievedSongExtra?.Uri,
									filepath: parameters?.RetrievedSongExtra?.Filepath) is int:
								await action.Playlist(playlist, parameters?.RetrievedSongExtra);
								break;

							default: break;

						}
						break;

					case true when model is ISong song:
						switch (true)
						{
							case true when action is FFProbeActions.OnRetrieve:
							case true when action is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(song):
							case true when action is IOActions.OnRetrieve && IOUtils.WouldBenefit(song):
							case true when action is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(song):
							case true when action is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(song):
								await action.Song(song);
								break;

							case true when
								action is MediaStoreActions.OnRetrieve &&
								parameters is not null && (parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters)) is not null &&
								await parameters.Cursor.ToPosition(
									directory: parameters?.Directory,
									cursorpositions: parameters?.CursorPositions,
									uri: parameters?.RetrievedSongExtra?.Uri,
									filepath: parameters?.RetrievedSongExtra?.Filepath) is int:
								await action.Song(song);
								break;

							default: break;

						}
						break;

					default: break;
				}
		}

		public partial class Default : ILibraryDroid 
		{
			public Default()
			{
				SQLiteLibrary = new SQLiteLibraryConnection();
			}

			public IAlbums Albums
			{
				get => this;
			}
			public IArtists Artists
			{
				get => this;
			}
			public IGenres Genres
			{
				get => this;
			}
			public IPlaylists Playlists
			{
				get => this;
			}
			public ISongs Songs
			{
				get => this;
			}
			public IMisc Misc
			{
				get => this;
			}
			public ISettings? Settings { get; set; }

			public CursorBuilder DefaultCursorBuilder
			{
				get => new CursorBuilder()
					.WithContentResolver(Application.Context.ContentResolver)
					.WithSettings(Settings);
			}
			public SQLiteLibraryConnection SQLiteLibrary { get; set; }
			public IActions.Container? Actions { get; set; }

			public ICursor? CursorToActions(Func<ICursor?> getcursor)
			{
				ICursor? cursor = null;

				if (Actions?.OnCreate is MediaStore.MediaStoreActions.OnCreate mediastoreoncreateaction)
					mediastoreoncreateaction.Cursor = cursor ??= getcursor.Invoke();

				if (Actions?.OnRetrieve?.OfType<MediaStore.MediaStoreActions.OnRetrieve>().FirstOrDefault() is MediaStore.MediaStoreActions.OnRetrieve mediastoreonretrieveaction)
					mediastoreonretrieveaction.Cursor = cursor ??= getcursor.Invoke();

				return cursor;
			}
		}
	}
}