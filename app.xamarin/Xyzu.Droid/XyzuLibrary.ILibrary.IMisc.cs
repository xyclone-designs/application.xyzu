#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Library.MediaStore;

namespace Xyzu
{
	public sealed partial class XyzuLibrary : ILibrary.IMisc
	{
		void ILibrary.IMisc.SetImage(IModel? model)
		{
			if (model is null)
				return;

			using Parameters parameters = new Parameters
			{
				CursorPositions = new Dictionary<string, int> { },
				RetrieverSong = new ISong.Default<bool>(false)
				{
					Artwork = new IImage.Default<bool>(true) { Uri = false },
					Filepath = true,
					TrackNumber = true,
					Uri = true,
				},
				CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
					.WithRetriever(parameters.RetrieverSong)
					.WithLibraryIdentifiers(parameters.Identifiers)
					.Build()),
			};

			SetImage(model, parameters);
		}
		async Task ILibrary.IMisc.SetImage(IModel? model, CancellationToken cancellationToken)
		{
			if (model is null)
				return;

			using Parameters parameters = new Parameters
			{
				CursorPositions = new Dictionary<string, int> { },
				RetrieverSong = new ISong.Default<bool>(false)
				{
					Artwork = new IImage.Default<bool>(true) { Uri = false },
					Filepath = true,
					TrackNumber = true,
					Uri = true,
				},
				CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
					.WithCancellationToken(cancellationToken)
					.WithRetriever(parameters.RetrieverSong)
					.WithLibraryIdentifiers(parameters.Identifiers)
					.Build()),
			};

			if (cancellationToken.IsCancellationRequested is false)
				await SetImage(model, parameters, cancellationToken);
		}

		void ILibrary.IMisc.SetImages(IEnumerable<IModel> models)
		{
			using Parameters parameters = new Parameters
			{
				CursorPositions = new Dictionary<string, int> { },
				RetrieverSong = new ISong.Default<bool>(false)
				{
					Artwork = new IImage.Default<bool>(true) { Uri = false },
					Filepath = true,
					TrackNumber = true,
					Uri = true,
				},
				CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
					.WithRetriever(parameters.RetrieverSong)
					.WithLibraryIdentifiers(parameters.Identifiers)
					.Build()),
			};

			foreach (IModel model in models)
				SetImage(model, parameters);
		}
		async Task ILibrary.IMisc.SetImages(IEnumerable<IModel> models, CancellationToken cancellationToken)
		{
			using Parameters parameters = new Parameters
			{
				CursorPositions = new Dictionary<string, int> { },
				RetrieverSong = new ISong.Default<bool>(false)
				{
					Artwork = new IImage.Default<bool>(true) { Uri = false },
					Filepath = true,
					TrackNumber = true,
					Uri = true,
				},
				CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
					.WithCancellationToken(cancellationToken)
					.WithRetriever(parameters.RetrieverSong)
					.WithLibraryIdentifiers(parameters.Identifiers)
					.Build()),
			};
			
			using IEnumerator<IModel> modelsenumerator = models.GetEnumerator();
			
			while (cancellationToken.IsCancellationRequested is false && modelsenumerator.MoveNext())
				await SetImage(modelsenumerator.Current, parameters, cancellationToken);
		}

		IImage? ILibrary.IMisc.GetImage(ILibrary.IIdentifiers? identifiers, params ModelTypes[] modeltypes)
		{
			using Parameters parameters = new Parameters
			{
				Identifiers = identifiers,
				ImageModelTypes = modeltypes,
				CursorPositions = new Dictionary<string, int> { },
				RetrieverSong = new ISong.Default<bool>(false)
				{
					Artwork = new IImage.Default<bool>(true) { Uri = false },
					Filepath = true,
					Uri = true,
				},
				CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
					.WithRetriever(parameters.RetrieverSong)
					.WithLibraryIdentifiers(parameters.Identifiers)
					.Build()),
			};

			foreach (ISong song in Filepaths(parameters).Select(id => CreateSong(id, null, parameters.RetrieverSong)))
			{
				PerformOnRetrieveActions(OnRetrieveActions, song, parameters);

				if (identifiers?.MatchesSong(song) ?? true)
				{
					IImage retrieved = new IImage.Default
					{
						Id = song.Id
					};

					if (retrieved.Buffer != null)
					{
						(parameters.Filepath, parameters.Uri) = (song.Filepath, song.Uri);

						PerformOnRetrieveActions(OnRetrieveActions, retrieved, parameters);
					}

					return retrieved;
				}
			}

			return null;
		}
		async Task<IImage?> ILibrary.IMisc.GetImage(ILibrary.IIdentifiers? identifiers, CancellationToken cancellationToken, params ModelTypes[] modeltypes)
		{
			using Parameters parameters = new Parameters
			{
				Identifiers = identifiers,
				ImageModelTypes = modeltypes,
				CursorPositions = new Dictionary<string, int> { },
				RetrieverSong = new ISong.Default<bool>(false)
				{
					Artwork = new IImage.Default<bool>(true) { Uri = false },
					Filepath = true,
					Uri = true,
				},
				CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
					.WithCancellationToken(cancellationToken)
					.WithRetriever(parameters.RetrieverSong)
					.WithLibraryIdentifiers(parameters.Identifiers)
					.Build()),
			};

			using IEnumerator<string> idsenumerator = Filepaths(parameters).GetEnumerator();

			while (cancellationToken.IsCancellationRequested is false && idsenumerator.MoveNext())
			{
				ISong song = CreateSong(idsenumerator.Current, null, parameters.RetrieverSong);

				if (cancellationToken.IsCancellationRequested is false)
					await PerformOnRetrieveActions(OnRetrieveActions, song, parameters, cancellationToken);

				if (identifiers?.MatchesSong(song) ?? true)
				{
					IImage retrieved = new IImage.Default
					{
						Id = song.Id
					};

					if (cancellationToken.IsCancellationRequested is false && retrieved.Buffer != null)
					{
						(parameters.Filepath, parameters.Uri) = (song.Filepath, song.Uri);

						await PerformOnRetrieveActions(OnRetrieveActions, retrieved, parameters, cancellationToken);
					}

					return retrieved;
				}
			}

			return null;
		}

		IEnumerable<ILibrary.ISearchResult> ILibrary.IMisc.Search(ILibrary.ISearcher searcher)
		{
			if (searcher.String is null || string.IsNullOrWhiteSpace(searcher.String))
				yield break;

			IList<string>? alreadymatched = new List<string>();

			foreach (ISong song in SqliteSongsTableQuery)
				foreach (ILibrary.ISearchResult searchresult in SearchResults(searcher, song, alreadymatched))
					yield return searchresult;
		}
		async IAsyncEnumerable<ILibrary.ISearchResult> ILibrary.IMisc.SearchAsync(ILibrary.ISearcher searcher, [EnumeratorCancellation] CancellationToken cancellationtoken)
		{
			if (searcher.String is null || string.IsNullOrWhiteSpace(searcher.String))
				yield break;

			IList<string>? alreadymatched = new List<string>();

			foreach (ISong song in SqliteSongsTableQuery)
				foreach (ILibrary.ISearchResult searchresult in SearchResults(searcher, song, alreadymatched))
					yield return searchresult;

			await Task.CompletedTask;
		}

		void SetImage(IModel model, Parameters parameters)
		{
			switch (true)
			{
				case true when
				model is IAlbum album &&
				album.Artwork?.Buffer is null &&
				album.SongIds.Any():
					{
						album.Artwork ??= new IImage.Default
						{
							Id = album.Id
						};

						using IEnumerator<ISong> albumsongsenumerator = album.SongIds
							.Select(songid => CreateSong(songid, null, parameters.RetrieverSong))
							.OrderBy(song => song.TrackNumber)
							.GetEnumerator();

						while (album.Artwork.Buffer is null && albumsongsenumerator.MoveNext())
						{
							album.Artwork.Buffer ??= albumsongsenumerator.Current.Artwork?.Buffer;
							album.Artwork.BufferHash ??= albumsongsenumerator.Current.Artwork?.BufferHash;
							album.Artwork.Uri ??= albumsongsenumerator.Current.Artwork?.Uri;

							if (album.Artwork.Buffer is null)
							{
								parameters.Uri = albumsongsenumerator.Current.Uri;
								parameters.Filepath = albumsongsenumerator.Current.Filepath;
								parameters.ImageModelTypes ??= new ModelTypes[] { ModelTypes.Album };

								PerformOnRetrieveActions(
									parameters: parameters,
									onretrieveactions: OnRetrieveActions,
									retrieved: album.Artwork ??= new IImage.Default
									{
										Id = album.Id
									});
							}
						}
					}
					break;

				case true when
				model is IArtist artist &&
				artist.Image?.Buffer is null &&
				artist.SongIds.Any():
					{
						artist.Image ??= new IImage.Default
						{
							Id = artist.Id
						};

						using IEnumerator<ISong> artistsongsenumerator = artist.SongIds
							.Select(songid => CreateSong(songid, null, parameters.RetrieverSong))
							.GetEnumerator();

						while (artist.Image.Buffer is null && artistsongsenumerator.MoveNext())
						{
							artist.Image.Buffer ??= artistsongsenumerator.Current.Artwork?.Buffer;
							artist.Image.BufferHash ??= artistsongsenumerator.Current.Artwork?.BufferHash;
							artist.Image.Uri ??= artistsongsenumerator.Current.Artwork?.Uri;

							if (artist.Image.Buffer is null)
							{
								parameters.Uri = artistsongsenumerator.Current.Uri;
								parameters.Filepath = artistsongsenumerator.Current.Filepath;
								parameters.ImageModelTypes ??= new ModelTypes[] { ModelTypes.Artist };

								PerformOnRetrieveActions(
									parameters: parameters,
									onretrieveactions: OnRetrieveActions,
									retrieved: artist.Image ??= new IImage.Default
									{
										Id = artist.Id
									});
							}
						}
					}
					break;

				case true when
				model is ISong song &&
				song.Artwork?.Buffer is null:
					{
						parameters.Uri = song.Uri;
						parameters.Filepath = song.Filepath;
						parameters.ImageModelTypes = new ModelTypes[] { ModelTypes.Song };

						PerformOnRetrieveActions(
							parameters: parameters,
							onretrieveactions: OnRetrieveActions,
							retrieved: song.Artwork ??= new IImage.Default
							{
								Id = song.Id
							});
					}
					break;

				default: break;
			}
		}
		async Task SetImage(IModel model, Parameters parameters, CancellationToken cancellationtoken)
		{
			switch (true)
			{
				case true when 
				model is IAlbum album &&
				album.Artwork?.Buffer is null &&
				album.SongIds.Any():
					{
						album.Artwork ??= new IImage.Default
						{
							Id = album.Id
						};

						using IEnumerator<ISong> albumsongsenumerator = album.SongIds
							.Select(songid => CreateSong(songid, null, parameters.RetrieverSong))
							.OrderBy(song => song.TrackNumber)
							.GetEnumerator();

						while (cancellationtoken.IsCancellationRequested is false && album.Artwork.Buffer is null && albumsongsenumerator.MoveNext())
						{
							album.Artwork.Buffer ??= albumsongsenumerator.Current.Artwork?.Buffer;
							album.Artwork.BufferHash ??= albumsongsenumerator.Current.Artwork?.BufferHash;
							album.Artwork.Uri ??= albumsongsenumerator.Current.Artwork?.Uri;

							if (album.Artwork.Buffer is null)
							{
								parameters.Uri = albumsongsenumerator.Current.Uri;
								parameters.Filepath = albumsongsenumerator.Current.Filepath;
								parameters.ImageModelTypes ??= new ModelTypes[] { ModelTypes.Album };

								await PerformOnRetrieveActions(
									parameters: parameters,
									cancellationtoken: cancellationtoken,
									onretrieveactions: OnRetrieveActions,
									retrieved: album.Artwork ??= new IImage.Default
									{
										Id = album.Id
									});
							}
						}
					}
					break;

				case true when
				model is IArtist artist &&
				artist.Image?.Buffer is null &&
				artist.SongIds.Any():
					{
						artist.Image ??= new IImage.Default
						{
							Id = artist.Id
						};

						using IEnumerator<ISong> artistsongsenumerator = artist.SongIds
							.Select(songid => CreateSong(songid, null, parameters.RetrieverSong))
							.GetEnumerator();

						while (cancellationtoken.IsCancellationRequested is false && artist.Image.Buffer is null && artistsongsenumerator.MoveNext())
						{
							artist.Image.Buffer ??= artistsongsenumerator.Current.Artwork?.Buffer;
							artist.Image.BufferHash ??= artistsongsenumerator.Current.Artwork?.BufferHash;
							artist.Image.Uri ??= artistsongsenumerator.Current.Artwork?.Uri;

							if (artist.Image.Buffer is null)
							{
								parameters.Uri = artistsongsenumerator.Current.Uri;
								parameters.Filepath = artistsongsenumerator.Current.Filepath;
								parameters.ImageModelTypes ??= new ModelTypes[] { ModelTypes.Artist };

								await PerformOnRetrieveActions(
									parameters: parameters,
									cancellationtoken: cancellationtoken,
									onretrieveactions: OnRetrieveActions,
									retrieved: artist.Image ??= new IImage.Default
									{
										Id = artist.Id
									});
							}
						}
					}
					break;

				case true when 
				model is ISong song &&
				song.Artwork?.Buffer is null:
					{
						parameters.Uri = song.Uri;
						parameters.Filepath = song.Filepath;
						parameters.ImageModelTypes = new ModelTypes[] { ModelTypes.Song };

						await PerformOnRetrieveActions(
							parameters: parameters,
							cancellationtoken: cancellationtoken,
							onretrieveactions: OnRetrieveActions,
							retrieved: song.Artwork ??= new IImage.Default
							{
								Id = song.Id
							});
					}
					break;

				default: break;
			}
		}

		IEnumerable<ILibrary.ISearchResult> SearchResults(ILibrary.ISearcher searcher, ISong song, IList<string>? alreadymatched)
		{
			if (searcher.String is null)
				yield break;

			string AlbumMatchText(string albumid) => nameof(IAlbum) + albumid;
			string ArtistMatchText(string artistid) => nameof(IArtist) + artistid;
			string GenreMatchText(string genreid) => nameof(IGenre) + genreid;
			string SongMatchText(string songid) => nameof(ISong) + songid;

			bool AlbumAlreadyMatched(string albumid) => alreadymatched?.Contains(AlbumMatchText(albumid)) ?? false;
			bool ArtistAlreadyMatched(string artistid) => alreadymatched?.Contains(ArtistMatchText(artistid)) ?? false;
			bool GenreAlreadyMatched(string genreid) => alreadymatched?.Contains(GenreMatchText(genreid)) ?? false;
			bool SongAlreadyMatched(string songid) => alreadymatched?.Contains(SongMatchText(songid)) ?? false;

			bool albumtitlematches = song.Album != null && (searcher.String.Contains(song.Album, StringComparison.OrdinalIgnoreCase) || song.Album.Contains(searcher.String, StringComparison.OrdinalIgnoreCase));
			bool albumartistnamematches = song.AlbumArtist != null && (searcher.String.Contains(song.AlbumArtist, StringComparison.OrdinalIgnoreCase) || song.AlbumArtist.Contains(searcher.String, StringComparison.OrdinalIgnoreCase));
			bool artistnamematches = song.Artist != null && (searcher.String.Contains(song.Artist, StringComparison.OrdinalIgnoreCase) || song.Artist.Contains(searcher.String, StringComparison.OrdinalIgnoreCase));
			bool genrenamematches = song.Genre != null && (searcher.String.Contains(song.Genre, StringComparison.OrdinalIgnoreCase) || song.Genre.Contains(searcher.String, StringComparison.OrdinalIgnoreCase));
			bool songtitlematches = song.Title != null && (searcher.String.Contains(song.Title, StringComparison.OrdinalIgnoreCase) || song.Title.Contains(searcher.String, StringComparison.OrdinalIgnoreCase));

			if (searcher.SearchAlbums && AlbumTitleToId(song.Album, song.AlbumArtist) is string albumid && AlbumAlreadyMatched(albumid) is false && (albumtitlematches || albumartistnamematches))
			{
				alreadymatched?.Add(AlbumMatchText(albumid));

				yield return new ILibrary.ISearchResult.Default(albumid, ModelTypes.Album);
			}

			if (searcher.SearchArtists && ArtistNameToId(song.Artist) is string artistid && ArtistAlreadyMatched(artistid) is false && (albumartistnamematches || artistnamematches))
			{
				alreadymatched?.Add(ArtistMatchText(artistid));

				yield return new ILibrary.ISearchResult.Default(artistid, ModelTypes.Artist);
			}

			if (searcher.SearchGenres && GenreNameToId(song.Genre) is string genreid && GenreAlreadyMatched(genreid) is false && (genrenamematches))
			{
				alreadymatched?.Add(GenreMatchText(genreid!));

				yield return new ILibrary.ISearchResult.Default(genreid!, ModelTypes.Genre);
			}

			if (searcher.SearchSongs && SongAlreadyMatched(song.Id) is false && (albumtitlematches || albumartistnamematches || artistnamematches || genrenamematches || songtitlematches))
			{
				alreadymatched?.Add(SongMatchText(song.Id));

				yield return new ILibrary.ISearchResult.Default(song.Id, ModelTypes.Song);
			}
		}
	}
}