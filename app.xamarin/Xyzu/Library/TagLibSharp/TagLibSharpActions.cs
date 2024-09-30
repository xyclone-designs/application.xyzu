using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TagLib;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

namespace Xyzu.Library.TagLibSharp
{
	public partial class TagLibSharpActions
	{
		public class OnCreate : ILibrary.IOnCreateActions.Default
		{
			public IDictionary<string, string>? Paths { get; private set; }
			public Func<IDictionary<string, string>>? OnPaths { get; set; }

			public override async Task<ISong?> Song(string id)
			{
				Paths ??= OnPaths?.Invoke();

				string? filepath = Paths is null || Paths.TryGetValue(id, out string? outpath) is false ? null : outpath;
				Uri? uri = filepath is null ? null : Uri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out Uri? outuri) ? outuri : null;

				ISong song = new ISong.Default(id)
				{
					Filepath = filepath,
					Uri = uri, 
				};

				if (filepath is not null)
					await Task.Run(() =>
					{
						try
						{
							using File file = File.Create(filepath);

							song.Retrieve(file);
						}
						catch (Exception) { }

					}).ConfigureAwait(false);

				return song;
			}
		}
		public class OnDelete : ILibrary.IOnDeleteActions.Default { }
		public class OnRetrieve : ILibrary.IOnRetrieveActions.Default
		{
			static File? GetFile(string? filepath, Uri? uri)
			{
				File? file = null;

				if (file is null && filepath is not null)
					try { file = File.Create(filepath); }
					catch (Exception) { }
				if (file is null && uri is not null)
					try { file = File.Create(uri.AbsolutePath); }
					catch (Exception) { }

				return file;
			}

			public override Task Album(IAlbum? retrieved, ISong? retrievedsong)
			{
				if (GetFile(retrievedsong?.Filepath, retrievedsong?.Uri) is File file)
				{
					retrieved?.Retrieve(file);

					file.Dispose();
				}

				return base.Album(retrieved, retrievedsong);
			}
			public override Task Artist(IArtist? retrieved, ISong? retrievedsong)
			{
				if (GetFile(retrievedsong?.Filepath, retrievedsong?.Uri) is File file)
				{
					retrieved?.Retrieve(file);

					file.Dispose();
				}

				return base.Artist(retrieved, retrievedsong);
			}
			public override Task Genre(IGenre? retrieved, ISong? retrievedsong)
			{
				if (GetFile(retrievedsong?.Filepath, retrievedsong?.Uri) is File file)
				{
					retrieved?.Retrieve(file);

					file.Dispose();
				}

				return base.Genre(retrieved, retrievedsong);
			}
			public override Task Playlist(IPlaylist? retrieved, ISong? retrievedsong) 
			{
				if (GetFile(retrievedsong?.Filepath, retrievedsong?.Uri) is File file)
				{
					retrieved?.Retrieve(file);

					file.Dispose();
				}

				return base.Playlist(retrieved, retrievedsong);
			}
			public override Task Song(ISong? retrieved)
			{
				if (GetFile(retrieved?.Filepath, retrieved?.Uri) is File file)
				{
					retrieved?.Retrieve(file);

					file.Dispose();
				}

				return base.Song(retrieved);
			}
			public override Task Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes)
			{
				if (GetFile(filepath, uri) is File file)
				{
					PictureTypeComparer? picturetypecomparer = modeltypes?.Where(modeltype => modeltype switch
					{
						ModelTypes.Album => true,
						ModelTypes.Artist => true,
						ModelTypes.Song => true,

						_ => false,

					}).FirstOrDefault() switch
					{
						ModelTypes.Album => PictureTypeComparer.ForAlbumArtwork(),
						ModelTypes.Artist => PictureTypeComparer.ForArtistImage(),
						ModelTypes.Song => PictureTypeComparer.ForSongArtwork(),

						_ => null,
					};

					retrieved?.Retrieve(file, retriever, picturetypecomparer);

					file.Dispose();
				}

				return base.Image(retrieved, retriever, filepath, uri, modeltypes);
			}
		}
		public class OnUpdate : ILibrary.IOnUpdateActions.Default
		{
			public async override Task Album(IAlbum? old, IAlbum? updated)
			{
				if (updated is null || old is null)
					return;

				IAlbum<bool>? distincts = old.Distinct(updated, new IAlbum.Default<bool>(false)
				{
					Artist = true,
					ReleaseDate = true,
					Title = true,
				});

				if (distincts is null)
					return;

				if (old.SongIds != null && OnGetSong != null)
					await Task.WhenAll(
						old.SongIds
							.Select(_ => OnGetSong.Invoke(_) is ISong song && string.IsNullOrWhiteSpace(song.Filepath) is false ? song : null)
							.OfType<ISong>()
							.Select(song => Task.Run(() =>
							{
								try
								{
									using File file = File.Create(song.Filepath);

									if (distincts.ReleaseDate) file.Tag.Year = (uint?)updated.ReleaseDate?.Year ?? default;
									if (distincts.Title) file.Tag.Album = updated.Title;

									file?.Save();
								}
								catch (Exception) { }

							})).ToArray());

				await base.Album(old, updated);
			}
			public async override Task Artist(IArtist? old, IArtist? updated)
			{
				if (updated is null || old is null)
					return;

				IArtist<bool>? distincts = old.Distinct(updated, new IArtist.Default<bool>(false)
				{
					Name = true,
				});

				if (distincts is null)
					return;

				if (old.SongIds != null && OnGetSong != null)
					await Task.WhenAll(
						old.SongIds
							.Select(_ => OnGetSong.Invoke(_) is ISong song && string.IsNullOrWhiteSpace(song.Filepath) is false ? song : null)
							.OfType<ISong>()
							.Select(song => Task.Run(() =>
							{
								try
								{
									using File file = File.Create(song.Filepath);

									if (distincts.Name && updated.Name != null) file.Tag.Performers = new string[] { updated.Name };

									file?.Save();
								}
								catch (Exception) { }

							})).ToArray());

				await base.Artist(old, updated);
			}
			public async override Task Genre(IGenre? old, IGenre? updated)
			{
				if (updated is null || old is null)
					return;

				IGenre<bool>? distincts = old.Distinct(updated, new IGenre.Default<bool>(false)
				{
					Name = true,
				});

				if (distincts is null)
					return;

				if (old.SongIds != null && OnGetSong != null)
					await Task.WhenAll(
						old.SongIds
							.Select(_ => OnGetSong.Invoke(_) is ISong song && string.IsNullOrWhiteSpace(song.Filepath) is false ? song : null)
							.OfType<ISong>()
							.Select(song => Task.Run(() =>
							{
								try
								{
									using File file = File.Create(song.Filepath);

									if (distincts.Name) file.Tag.Genres = updated.Name?.Split(';').ToArray() ?? Array.Empty<string>();

									file?.Save();
								}
								catch (Exception) { }

							})).ToArray());

				await base.Genre(old, updated);
			}
			public async override Task Song(ISong? old, ISong? updated)
			{
				if (old?.Filepath is null || updated is null)
					return;

				ISong<bool>? distincts = old.Distinct(updated, new ISong.Default<bool>(false)
				{
					Album = true,
					Artist = true,
					Genre = true,
					Lyrics = true,
					ReleaseDate = true,
					Title = true,
					TrackNumber = true,
				});

				if (distincts is null)
					return;

				if (old.Filepath is not null)
					await Task.Run(() =>
					{
						try
						{
							using File file = File.Create(old.Filepath);

							if (distincts.Album) file.Tag.Album = updated.Album;
							if (distincts.AlbumArtist && updated.AlbumArtist != null) file.Tag.AlbumArtists = new string[] { updated.AlbumArtist };
							if (distincts.Artist && updated.Artist != null) file.Tag.Performers = new string[] { updated.Artist };
							if (distincts.DiscNumber) file.Tag.Disc = (uint?)updated.DiscNumber ?? default;
							if (distincts.Genre) file.Tag.Genres = updated.Genre?.Split(';').ToArray() ?? Array.Empty<string>();
							if (distincts.Lyrics) file.Tag.Lyrics = updated.Lyrics;
							if (distincts.ReleaseDate) file.Tag.Year = (uint?)updated.ReleaseDate?.Year ?? default;
							if (distincts.Title) file.Tag.Title = updated.Title;
							if (distincts.TrackNumber) file.Tag.Track = (uint?)updated.TrackNumber ?? default;

							file.Save();
						}
						catch (Exception) { }
					});

				await base.Song(old, updated);
			}
		}
	}
}
