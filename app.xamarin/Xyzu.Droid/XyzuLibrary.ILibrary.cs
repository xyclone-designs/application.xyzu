#nullable enable

using Android.Database;

using System;		
using System.Collections.Generic;		
using System.Linq;		
using System.Threading;		
using System.Threading.Tasks;		

using Xyzu.Images;
using Xyzu.Library;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Library.ID3;
using Xyzu.Library.IO;
using Xyzu.Library.MediaMetadata;
using Xyzu.Library.MediaStore;
using Xyzu.Library.TagLibSharp;

namespace Xyzu
{
	public sealed partial class XyzuLibrary : ILibrary
	{
		public int RandomCount { get; set; } = 5;

		public ILibrary.IAlbums Albums
		{
			get => this;
		}
		public ILibrary.IArtists Artists
		{
			get => this;
		}
		public ILibrary.IGenres Genres
		{
			get => this;
		}
		public ILibrary.IPlaylists Playlists
		{
			get => this;
		}
		public ILibrary.ISongs Songs
		{
			get => this;
		}
		public ILibrary.IMisc Misc
		{
			get => this;
		}

		public ILibrary.ISettings? Settings { get; set; }

		public ILibrary.IOnCreateActions? OnCreateAction { get; set; }
		public IEnumerable<ILibrary.IOnDeleteActions>? OnDeleteActions { get; set; }
		public IEnumerable<ILibrary.IOnRetrieveActions>? OnRetrieveActions { get; set; }
		public IEnumerable<ILibrary.IOnUpdateActions>? OnUpdateActions { get; set; }

		public IEnumerable<string> Filepaths(Parameters? parameters)
		{
			switch (true)
			{
				case true when OnCreateAction is MediaStoreActions.OnCreate mediastoreoncreateaction:

					if ((mediastoreoncreateaction.Cursor = parameters?.Cursor) != null && mediastoreoncreateaction.Cursor.MoveToFirst())
						do
						{
							if (mediastoreoncreateaction.Cursor.GetId() is string id &&
								(parameters?.Identifiers?.SongIds is null || parameters.Identifiers.SongIds.Contains(id)) &&
								(parameters?.Identifiers?.WithoutSongIds is null || parameters.Identifiers.WithoutSongIds.Contains(id) is false))
								yield return id;
						}
						while (mediastoreoncreateaction.Cursor.MoveToNext());

					break;

				case true when OnCreateAction is TagLibSharpActions.OnCreate taglibsharponcreateaction:

					if (taglibsharponcreateaction.Paths != null)
						foreach (string key in taglibsharponcreateaction.Paths.Keys)
							if ((parameters?.Identifiers?.SongIds is null || parameters.Identifiers.SongIds.Contains(key)) &&
								(parameters?.Identifiers?.WithoutSongIds is null || parameters.Identifiers.WithoutSongIds.Contains(key) is false))
								yield return key;

					break;

				default: yield break;
			}
		}
		public string? AlbumTitleToId(string? albumtitle, string? albumartistname)
		{
			return albumtitle + albumartistname;
		}
		public string? ArtistNameToId(string? artistname)
		{
			return artistname;
		}
		public string? GenreNameToId(string? genrename)
		{
			return genrename;
		}
		public string? PlaylistTitleToId(string? playlisttitle)
		{
			return playlisttitle;
		}

		public ISong CreateSong(string id, bool? usecache, ISong<bool>? retriever)
		{
			ISong? song = OnCreateAction?.Song(id, retriever) ?? new ISong.Default(id);

			return song;
		}

		private CursorBuilder DefaultCursorBuilder
		{
			get => new CursorBuilder()
				.WithContentResolver(Context.ContentResolver)
				.WithSettings(Settings);
		}
		private ICursor? CursorToActions(Func<ICursor?> getcursor)
		{
			ICursor? cursor = null;

			if (OnCreateAction is MediaStoreActions.OnCreate mediastoreoncreateaction)
				mediastoreoncreateaction.Cursor = cursor ??= getcursor.Invoke();

			if (OnRetrieveActions?.OfType<MediaStoreActions.OnRetrieve>().FirstOrDefault() is MediaStoreActions.OnRetrieve mediastoreonretrieveaction)
				mediastoreonretrieveaction.Cursor = cursor ??= getcursor.Invoke();

			return cursor;
		}
		private int? CursorToPosition(ICursor? cursor, string? filepath, Uri? uri, IDictionary<string, int>? cursorpositions, bool addifabsent = true)
		{
			if (cursor is null)
				return null;

			string? key = filepath ?? uri?.ToString();
			int? cursorposition =
				cursorpositions is null
					? new int?()
				: key != null && cursorpositions.TryGetValue(key, out int keyposition)
					? keyposition
				: Math.Max(0, Math.Min(cursorpositions.Count, cursor.Count) - 1);

			if (cursor.MoveToPosition(cursorposition ?? 0))
				do
				{
					(string? cursorfilepath, Uri? cursoruri) = cursor.GetFilepathAndUri();

					if ((cursorfilepath ?? cursoruri?.ToString()) is string cursorkey)
					{
						cursorpositions?.TryAdd(cursorkey, cursor.Position);

						if (cursorkey == key)
						{
							cursorposition = cursor.Position;

							break;
						}
					}

				} while (cursor.MoveToNext());

			return cursorposition;
		}
		private async Task<int?> CursorToPosition(ICursor? cursor, string? filepath, Uri? uri, IDictionary<string, int>? cursorpositions, bool addifabsent = true, CancellationToken cancellationtoken = default)
		{
			if (cursor is null)
				return null;

			string? key = filepath ?? uri?.ToString();
			int? cursorposition =
				cursorpositions is null
					? new int?()
				: key != null && cursorpositions.TryGetValue(key, out int keyposition)
					? keyposition
				: Math.Max(0, Math.Min(cursorpositions.Count, cursor.Count) - 1);

			if (cursor.MoveToPosition(cursorposition ?? 0) && cancellationtoken.IsCancellationRequested is false)
				do
				{
					(string? cursorfilepath, Uri? cursoruri) = await cursor.GetFilepathAndUriAsync(cancellationtoken);

					if ((cursorfilepath ?? cursoruri?.ToString()) is string cursorkey)
					{
						cursorpositions?.TryAdd(cursorkey, cursor.Position);

						if (cursorkey == key)
						{
							cursorposition = cursor.Position;

							break;
						}
					}

				} while (cursor.MoveToNext() && cancellationtoken.IsCancellationRequested is false);

			return cursorposition;
		}

		private void PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IAlbum retrieved, Parameters? parameters)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverAlbum):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverAlbum):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverAlbum):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverAlbum):
						onretrieveaction.Album(retrieved, parameters?.RetrieverAlbum, parameters?.RetrievedSongExtra);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						CursorToPosition(
							addifabsent: true,
							cursorpositions: parameters?.CursorPositions,
							uri: parameters?.RetrievedSongExtra?.Uri,
							filepath: parameters?.RetrievedSongExtra?.Filepath,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Album(retrieved, parameters?.RetrieverAlbum, parameters?.RetrievedSongExtra);
						break;

					default: break;
				}
		}
		private void PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IArtist retrieved, Parameters? parameters)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverArtist):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverArtist):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverArtist):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverArtist):
						if (parameters?.RetrievedAlbumExtra != null)
							onretrieveaction.Artist(retrieved, parameters?.RetrieverArtist, parameters?.RetrievedAlbumExtra);
						else onretrieveaction.Artist(retrieved, parameters?.RetrieverArtist, parameters?.RetrievedSongExtra);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						CursorToPosition(
							addifabsent: true,
							cursorpositions: parameters?.CursorPositions,
							uri: parameters?.RetrievedSongExtra?.Uri,
							filepath: parameters?.RetrievedSongExtra?.Filepath,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						if (parameters?.RetrievedAlbumExtra != null)
							onretrieveaction.Artist(retrieved, parameters?.RetrieverArtist, parameters?.RetrievedAlbumExtra);
						else onretrieveaction.Artist(retrieved, parameters?.RetrieverArtist, parameters?.RetrievedSongExtra);
						break;

					default: break;
				}
		}
		private void PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IGenre retrieved, Parameters? parameters)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverGenre):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverGenre):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverGenre):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverGenre):
						onretrieveaction.Genre(retrieved, parameters?.RetrieverGenre, parameters?.RetrievedSongExtra);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						CursorToPosition(
							addifabsent: true,
							cursorpositions: parameters?.CursorPositions,
							uri: parameters?.RetrievedSongExtra?.Uri,
							filepath: parameters?.RetrievedSongExtra?.Filepath,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Genre(retrieved, parameters?.RetrieverGenre, parameters?.RetrievedSongExtra);
						break;

					default: break;
				}
		}
		private void PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IPlaylist retrieved, Parameters? parameters)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverPlaylist):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverPlaylist):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverPlaylist):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverPlaylist):
						onretrieveaction.Playlist(retrieved, parameters?.RetrieverPlaylist, parameters?.RetrievedSongExtra);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						CursorToPosition(
							addifabsent: true,
							cursorpositions: parameters?.CursorPositions,
							uri: parameters?.RetrievedSongExtra?.Uri,
							filepath: parameters?.RetrievedSongExtra?.Filepath,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Playlist(retrieved, parameters?.RetrieverPlaylist, parameters?.RetrievedSongExtra);
						break;

					default: break;
				}
		}
		private void PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, ISong retrieved, Parameters? parameters)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverSong):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverSong):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverSong):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverSong):
						onretrieveaction.Song(retrieved, parameters?.RetrieverSong);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						CursorToPosition(
							addifabsent: true,
							uri: retrieved?.Uri,
							filepath: retrieved?.Filepath,
							cursorpositions: parameters?.CursorPositions,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Song(retrieved, parameters?.RetrieverSong);
						break;

					default: break;
				}
		}
		private void PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IImage retrieved, Parameters? parameters)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when retrieved.Buffer != null:
						break;

					case true when onretrieveaction is ID3Actions.OnRetrieve:
					case true when onretrieveaction is IOActions.OnRetrieve:
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve:
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve:
						onretrieveaction.Image(retrieved, parameters?.RetrieverImage, parameters?.Filepath, parameters?.Uri, parameters?.ImageModelTypes);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						CursorToPosition(
							addifabsent: true,
							uri: parameters?.Uri,
							filepath: parameters?.Filepath,
							cursorpositions: parameters?.CursorPositions,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Image(retrieved, parameters?.RetrieverImage, parameters?.Filepath, parameters?.Uri, parameters?.ImageModelTypes);
						break;

					default: break;
				}
		}

		private async Task PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IAlbum retrieved, Parameters? parameters, CancellationToken cancellationtoken = default)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverAlbum):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverAlbum):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverAlbum):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverAlbum):
						onretrieveaction.Album(retrieved, parameters?.RetrieverAlbum, parameters?.RetrievedSongExtra);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						await CursorToPosition(
							addifabsent: true,
							cancellationtoken: cancellationtoken,
							cursorpositions: parameters?.CursorPositions,
							uri: parameters?.RetrievedSongExtra?.Uri,
							filepath: parameters?.RetrievedSongExtra?.Filepath,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Album(retrieved, parameters?.RetrieverAlbum, parameters?.RetrievedSongExtra);
						break;

					default: break;
				}
		}
		private async Task PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IArtist retrieved, Parameters? parameters, CancellationToken cancellationtoken = default)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverArtist):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverArtist):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverArtist):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverArtist):
						if (parameters?.RetrievedAlbumExtra != null)
							onretrieveaction.Artist(retrieved, parameters?.RetrieverArtist, parameters?.RetrievedAlbumExtra);
						else onretrieveaction.Artist(retrieved, parameters?.RetrieverArtist, parameters?.RetrievedSongExtra);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						await CursorToPosition(
							addifabsent: true,
							cancellationtoken: cancellationtoken,
							cursorpositions: parameters?.CursorPositions,
							uri: parameters?.RetrievedSongExtra?.Uri,
							filepath: parameters?.RetrievedSongExtra?.Filepath,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						if (parameters?.RetrievedAlbumExtra != null)
							onretrieveaction.Artist(retrieved, parameters?.RetrieverArtist, parameters?.RetrievedAlbumExtra);
						else onretrieveaction.Artist(retrieved, parameters?.RetrieverArtist, parameters?.RetrievedSongExtra);
						break;

					default: break;
				}
		}
		private async Task PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IGenre retrieved, Parameters? parameters, CancellationToken cancellationtoken = default)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverGenre):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverGenre):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverGenre):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverGenre):
						onretrieveaction.Genre(retrieved, parameters?.RetrieverGenre, parameters?.RetrievedSongExtra);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						await CursorToPosition(
							addifabsent: true,
							cancellationtoken: cancellationtoken,
							cursorpositions: parameters?.CursorPositions,
							uri: parameters?.RetrievedSongExtra?.Uri,
							filepath: parameters?.RetrievedSongExtra?.Filepath,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Genre(retrieved, parameters?.RetrieverGenre, parameters?.RetrievedSongExtra);
						break;

					default: break;
				}
		}
		private async Task PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IPlaylist retrieved, Parameters? parameters, CancellationToken cancellationtoken = default)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverPlaylist):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverPlaylist):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverPlaylist):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverPlaylist):
						onretrieveaction.Playlist(retrieved, parameters?.RetrieverPlaylist, parameters?.RetrievedSongExtra);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						await CursorToPosition(
							addifabsent: true,
							cancellationtoken: cancellationtoken,
							cursorpositions: parameters?.CursorPositions,
							uri: parameters?.RetrievedSongExtra?.Uri,
							filepath: parameters?.RetrievedSongExtra?.Filepath,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Playlist(retrieved, parameters?.RetrieverPlaylist, parameters?.RetrievedSongExtra);
						break;

					default: break;
				}
		}
		private async Task PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, ISong retrieved, Parameters? parameters, CancellationToken cancellationtoken = default)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when onretrieveaction is ID3Actions.OnRetrieve && ID3Utils.WouldBenefit(retrieved, parameters?.RetrieverSong):
					case true when onretrieveaction is IOActions.OnRetrieve && IOUtils.WouldBenefit(retrieved, parameters?.RetrieverSong):
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve && MediaMetadataUtils.WouldBenefit(retrieved, parameters?.RetrieverSong):
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve && TagLibSharpUtils.WouldBenefit(retrieved, parameters?.RetrieverSong):
						onretrieveaction.Song(retrieved, parameters?.RetrieverSong);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						await CursorToPosition(
							addifabsent: true,
							uri: retrieved?.Uri,
							filepath: retrieved?.Filepath,
							cancellationtoken: cancellationtoken,
							cursorpositions: parameters?.CursorPositions,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Song(retrieved, parameters?.RetrieverSong);
						break;

					default: break;
				}
		}
		private async Task PerformOnRetrieveActions(IEnumerable<ILibrary.IOnRetrieveActions>? onretrieveactions, IImage retrieved, Parameters? parameters, CancellationToken cancellationtoken = default)
		{
			if (onretrieveactions is null || onretrieveactions.Any() is false)
				return;

			foreach (ILibrary.IOnRetrieveActions onretrieveaction in onretrieveactions)
				switch (true)
				{
					case true when retrieved.Buffer != null:
						break;

					case true when onretrieveaction is ID3Actions.OnRetrieve:
					case true when onretrieveaction is IOActions.OnRetrieve:
					case true when onretrieveaction is MediaMetadataActions.OnRetrieve:
					case true when onretrieveaction is TagLibSharpActions.OnRetrieve:
						onretrieveaction.Image(retrieved, parameters?.RetrieverImage, parameters?.Filepath, parameters?.Uri, parameters?.ImageModelTypes);
						break;

					case true when onretrieveaction is MediaStoreActions.OnRetrieve:
						await CursorToPosition(
							addifabsent: true,
							uri: parameters?.Uri,
							filepath: parameters?.Filepath,
							cancellationtoken: cancellationtoken,
							cursorpositions: parameters?.CursorPositions,
							cursor: parameters is null ? null : parameters.Cursor ??= parameters.CursorLazy?.Invoke(parameters));
						onretrieveaction.Image(retrieved, parameters?.RetrieverImage, parameters?.Filepath, parameters?.Uri, parameters?.ImageModelTypes);
						break;

					default: break;
				}
		}

		public class Parameters : IDisposable
		{
			public ICursor? Cursor { get; set; }
			public Func<Parameters, ICursor?>? CursorLazy { get; set; }
			public IDictionary<string, int>? CursorPositions { get; set; }
			public ILibrary.IIdentifiers? Identifiers { get; set; }

			public string? Filepath { get; set; }
			public Uri? Uri { get; set; }
			public ModelTypes[]? ImageModelTypes { get; set; }

			public IAlbum<bool>? RetrieverAlbum { get; set; }
			public IArtist<bool>? RetrieverArtist { get; set; }
			public IGenre<bool>? RetrieverGenre { get; set; }
			public IPlaylist<bool>? RetrieverPlaylist { get; set; }
			public ISong<bool>? RetrieverSong { get; set; }				 
			
			public IImage<bool>? RetrieverImage { get; set; }				 

			public IAlbum? RetrievedAlbumExtra { get; set; }
			public IArtist? RetrievedArtistExtra { get; set; }
			public IGenre? RetrievedGenreExtra { get; set; }
			public IPlaylist? RetrievedPlaylistExtra { get; set; }
			public ISong? RetrievedSongExtra { get; set; }

			public void Dispose()
			{
				// Cursor?.Close();
				// Cursor?.Dispose();
				CursorPositions?.Clear();

				Cursor = null;
				CursorLazy = null;
				CursorPositions = null;
				Identifiers = null;
				Filepath = null;
				Uri = null;
				RetrieverAlbum = null;
				RetrieverArtist = null;
				RetrieverGenre = null;
				RetrieverPlaylist = null;
				RetrieverSong = null;
				RetrievedAlbumExtra = null;
				RetrievedArtistExtra = null;
				RetrievedGenreExtra = null;
				RetrievedPlaylistExtra = null;
				RetrievedSongExtra = null;
			}
		}
	}
}