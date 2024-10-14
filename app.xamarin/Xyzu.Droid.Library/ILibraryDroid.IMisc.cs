using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

namespace Xyzu.Library
{
	public partial interface ILibraryDroid
	{
		public partial class Default : IMisc
		{
			private readonly static ISong<bool> RetrieverImageSong = new ISong.Default<bool>(false)
			{
				Artwork = new IImage.Default<bool>(true) { Uri = false },
				Filepath = true,
				TrackNumber = true,
				Uri = true,
			};

			void IMisc.SetImage(IModel? model)
			{
				if (model is null)
					return;

				using IParameters parameters = new IParameters.Default ()
				{
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
						.WithRetriever(RetrieverImageSong)
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				SetImage(model, parameters);
			}
			async Task IMisc.SetImage(IModel? model, CancellationToken cancellationToken)
			{
				if (model is null)
					return;

				using IParameters parameters = new IParameters.Default ()
				{
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
						.WithRetriever(RetrieverImageSong)
						.WithCancellationToken(cancellationToken)
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				if (cancellationToken.IsCancellationRequested is false)
					await SetImage(model, parameters, cancellationToken);
			}

			void IMisc.SetImages(IEnumerable<IModel> models)
			{
				using IParameters parameters = new IParameters.Default ()
				{
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
						.WithRetriever(RetrieverImageSong)
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				foreach (IModel model in models)
					SetImage(model, parameters);
			}
			async Task IMisc.SetImages(IEnumerable<IModel> models, CancellationToken cancellationToken)
			{
				using IParameters parameters = new IParameters.Default ()
				{
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
						.WithRetriever(RetrieverImageSong)
						.WithCancellationToken(cancellationToken)
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};
			
				using IEnumerator<IModel> modelsenumerator = models.GetEnumerator();
			
				while (cancellationToken.IsCancellationRequested is false && modelsenumerator.MoveNext())
					await SetImage(modelsenumerator.Current, parameters, cancellationToken);
			}

			IImage? IMisc.GetImage(IIdentifiers? identifiers, params ModelTypes[] modeltypes)
			{
				using IParameters.Default parameters = new ()
				{
					Identifiers = identifiers,
					ImageModelTypes = modeltypes,
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
						.WithRetriever(RetrieverImageSong)
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				foreach (ISong song in parameters.Filepaths().Select(filepath => new ISong.Default(filepath)))
				{
					Task.WaitAny(OnRetrieve(song, Actions?.OnRetrieve, parameters));

					if (identifiers?.MatchesSong(song) ?? true)
					{
						IImage retrieved = new IImage.Default
						{
							Id = song.Id
						};

						if (retrieved.Buffer != null)
						{
							(parameters.Filepath, parameters.Uri) = (song.Filepath, song.Uri);

							Task.WaitAny(OnRetrieve(retrieved, Actions?.OnRetrieve, parameters));
						}

						return retrieved;
					}
				}

				return null;
			}
			async Task<IImage?> IMisc.GetImage(IIdentifiers? identifiers, CancellationToken cancellationToken, params ModelTypes[] modeltypes)
			{
				using IParameters parameters = new IParameters.Default ()
				{
					Identifiers = identifiers,
					ImageModelTypes = modeltypes,
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => CursorToActions(() => DefaultCursorBuilder
						.WithRetriever(RetrieverImageSong)
						.WithCancellationToken(cancellationToken)
						.WithLibraryIdentifiers(parameters.Identifiers)
					.Build()),
				};

				using IEnumerator<string> idsenumerator = parameters.Filepaths().GetEnumerator();

				while (cancellationToken.IsCancellationRequested is false && idsenumerator.MoveNext())
				{
					ISong song = new ISong.Default(idsenumerator.Current);

					if (cancellationToken.IsCancellationRequested is false)
						await OnRetrieve(song, Actions?.OnRetrieve, parameters);

					if (identifiers?.MatchesSong(song) ?? true)
					{
						IImage retrieved = new IImage.Default
						{
							Id = song.Id
						};

						if (cancellationToken.IsCancellationRequested is false && retrieved.Buffer != null)
						{
							(parameters.Filepath, parameters.Uri) = (song.Filepath, song.Uri);

							await OnRetrieve(retrieved, Actions?.OnRetrieve, parameters);
						}

						return retrieved;
					}
				}

				return null;
			}

			IEnumerable<ISearchResult> IMisc.Search(ISearcher searcher)
			{
				if (searcher.String is null || string.IsNullOrWhiteSpace(searcher.String))
					yield break;

				IList<string>? alreadymatched = new List<string>();

				foreach (ISong song in SQLiteLibrary.SongsTable)
					foreach (ISearchResult searchresult in SearchResults(searcher, song, alreadymatched))
						yield return searchresult;
			}
			async IAsyncEnumerable<ISearchResult> IMisc.SearchAsync(ISearcher searcher, [EnumeratorCancellation] CancellationToken cancellationtoken)
			{
				if (searcher.String is null || string.IsNullOrWhiteSpace(searcher.String))
					yield break;

				IList<string>? alreadymatched = new List<string>();

				foreach (ISong song in SQLiteLibrary.SongsTable)
					foreach (ISearchResult searchresult in SearchResults(searcher, song, alreadymatched))
						yield return searchresult;

				await Task.CompletedTask;
			}

			void SetImage(IModel model, IParameters parameters)
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
								.Select(_ => new ISong.Default(_))
								.GetEnumerator();

							while (album.Artwork.Buffer is null && albumsongsenumerator.MoveNext())
							{
								album.Artwork.Buffer ??= albumsongsenumerator.Current.Artwork?.Buffer;
								album.Artwork.BufferKey ??= albumsongsenumerator.Current.Artwork?.BufferKey;
								album.Artwork.Uri ??= albumsongsenumerator.Current.Artwork?.Uri;

								if (album.Artwork.Buffer is null)
								{
									parameters.Uri = albumsongsenumerator.Current.Uri;
									parameters.Filepath = albumsongsenumerator.Current.Filepath;
									parameters.ImageModelTypes ??= new ModelTypes[] { ModelTypes.Album };

									Task.WaitAny(OnRetrieve(
										parameters: parameters,
										actions: Actions?.OnRetrieve,
										retrieved: album.Artwork ??= new IImage.Default
										{
											Id = album.Id
										}));
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
								.Select(_ => new ISong.Default(_))
								.GetEnumerator();

							while (artist.Image.Buffer is null && artistsongsenumerator.MoveNext())
							{
								artist.Image.Buffer ??= artistsongsenumerator.Current.Artwork?.Buffer;
								artist.Image.BufferKey ??= artistsongsenumerator.Current.Artwork?.BufferKey;
								artist.Image.Uri ??= artistsongsenumerator.Current.Artwork?.Uri;

								if (artist.Image.Buffer is null)
								{
									parameters.Uri = artistsongsenumerator.Current.Uri;
									parameters.Filepath = artistsongsenumerator.Current.Filepath;
									parameters.ImageModelTypes ??= new ModelTypes[] { ModelTypes.Artist };

									Task.WaitAny(OnRetrieve(
										parameters: parameters,
										actions: Actions?.OnRetrieve,
										retrieved: artist.Image ??= new IImage.Default
										{
											Id = artist.Id
										}));
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

							Task.WaitAny(OnRetrieve(
								parameters: parameters,
								actions: Actions?.OnRetrieve,
								retrieved: song.Artwork ??= new IImage.Default
								{
									Id = song.Id
								}));
						}
						break;

					default: break;
				}
			}
			async Task SetImage(IModel model, IParameters parameters, CancellationToken cancellationtoken)
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
								.Select(_ => new ISong.Default(_))
								.GetEnumerator();

							while (cancellationtoken.IsCancellationRequested is false && album.Artwork.Buffer is null && albumsongsenumerator.MoveNext())
							{
								album.Artwork.Buffer ??= albumsongsenumerator.Current.Artwork?.Buffer;
								album.Artwork.BufferKey ??= albumsongsenumerator.Current.Artwork?.BufferKey;
								album.Artwork.Uri ??= albumsongsenumerator.Current.Artwork?.Uri;

								if (album.Artwork.Buffer is null)
								{
									parameters.Uri = albumsongsenumerator.Current.Uri;
									parameters.Filepath = albumsongsenumerator.Current.Filepath;
									parameters.ImageModelTypes ??= new ModelTypes[] { ModelTypes.Album };

									await OnRetrieve(
										parameters: parameters,
										actions: Actions?.OnRetrieve,
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
								.Select(_ => new ISong.Default(_))
								.GetEnumerator();

							while (cancellationtoken.IsCancellationRequested is false && artist.Image.Buffer is null && artistsongsenumerator.MoveNext())
							{
								artist.Image.Buffer ??= artistsongsenumerator.Current.Artwork?.Buffer;
								artist.Image.BufferKey ??= artistsongsenumerator.Current.Artwork?.BufferKey;
								artist.Image.Uri ??= artistsongsenumerator.Current.Artwork?.Uri;

								if (artist.Image.Buffer is null)
								{
									parameters.Uri = artistsongsenumerator.Current.Uri;
									parameters.Filepath = artistsongsenumerator.Current.Filepath;
									parameters.ImageModelTypes ??= new ModelTypes[] { ModelTypes.Artist };

									await OnRetrieve(
										parameters: parameters,
										actions: Actions?.OnRetrieve,
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

							await OnRetrieve(
								parameters: parameters,
								actions: Actions?.OnRetrieve,
								retrieved: song.Artwork ??= new IImage.Default
								{
									Id = song.Id
								});
						}
						break;

					default: break;
				}
			}

			IEnumerable<ISearchResult> SearchResults(ISearcher searcher, ISong song, IList<string>? alreadymatched)
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

				if (searcher.SearchAlbums && IdFromAlbumTitle(song.Album, song.AlbumArtist) is string albumid && AlbumAlreadyMatched(albumid) is false && (albumtitlematches || albumartistnamematches))
				{
					alreadymatched?.Add(AlbumMatchText(albumid));

					yield return new ISearchResult.Default(albumid, ModelTypes.Album);
				}

				if (searcher.SearchArtists && IdFromArtistName(song.Artist) is string artistid && ArtistAlreadyMatched(artistid) is false && (albumartistnamematches || artistnamematches))
				{
					alreadymatched?.Add(ArtistMatchText(artistid));

					yield return new ISearchResult.Default(artistid, ModelTypes.Artist);
				}

				if (searcher.SearchGenres && IdFromGenreName(song.Genre) is string genreid && GenreAlreadyMatched(genreid) is false && (genrenamematches))
				{
					alreadymatched?.Add(GenreMatchText(genreid!));

					yield return new ISearchResult.Default(genreid!, ModelTypes.Genre);
				}

				if (searcher.SearchSongs && SongAlreadyMatched(song.Id) is false && (albumtitlematches || albumartistnamematches || artistnamematches || genrenamematches || songtitlematches))
				{
					alreadymatched?.Add(SongMatchText(song.Id));

					yield return new ISearchResult.Default(song.Id, ModelTypes.Song);
				}
			}
		}
	}
}