using Id3;
using Id3.Frames;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

namespace Xyzu.Library.ID3
{
	public partial class ID3Actions
	{
		public static async Task<Mp3?> GetMp3Async(string? filepath, Uri? uri)
		{
			Mp3? mp3 = null;

			if (mp3 is null && filepath is not null)
				await Task
					.Run(() => { try { mp3 = new Mp3(filepath); } catch (Exception) { } })
					.ConfigureAwait(false);

			if (mp3 is null && uri is not null)
				await Task
					.Run(() => { try { mp3 = new Mp3(uri.AbsolutePath); } catch (Exception) { } })
					.ConfigureAwait(false);

			return mp3;
		}

		public class OnCreate : ILibrary.IOnCreateActions.Default { }
		public class OnDelete : ILibrary.IOnDeleteActions.Default { }
		public class OnRetrieve : ILibrary.IOnRetrieveActions.Default
		{
			public async override Task Album(IAlbum? retrieved, ISong? retrievedsong)
			{
				if (retrieved is not null &&
					retrievedsong is not null &&
					await GetMp3Async(retrievedsong.Filepath, retrievedsong.Uri) is Mp3 mp3)
				{
					retrieved.Retrieve(mp3);

					mp3.Dispose();
				}

				await base.Album(retrieved, retrievedsong);
			}
			public async override Task Artist(IArtist? retrieved, ISong? retrievedsong)
			{
				if (retrieved is not null &&
					retrievedsong is not null &&
					await GetMp3Async(retrievedsong.Filepath, retrievedsong.Uri) is Mp3 mp3)
				{
					retrieved.Retrieve(mp3);

					mp3.Dispose();
				}

				await base.Artist(retrieved, retrievedsong);
			}
			public async override Task Genre(IGenre? retrieved, ISong? retrievedsong)
			{
				if (retrieved is not null &&
					retrievedsong is not null &&
					await GetMp3Async(retrievedsong.Filepath, retrievedsong.Uri) is Mp3 mp3)
				{
					retrieved.Retrieve(mp3);

					mp3.Dispose();
				}

				await base.Genre(retrieved, retrievedsong);
			}
			public async override Task Playlist(IPlaylist? retrieved, ISong? retrievedsong)
			{
				if (retrieved is not null && 
					retrievedsong is not null &&
					await GetMp3Async(retrievedsong.Filepath, retrievedsong.Uri) is Mp3 mp3)
				{
					retrieved.Retrieve(mp3);

					mp3.Dispose();
				}

				await base.Playlist(retrieved, retrievedsong);
			}
			public async override Task Song(ISong? retrieved)
			{
				if (retrieved is not null && await GetMp3Async(retrieved.Filepath, retrieved.Uri) is Mp3 mp3)
				{
					retrieved.Retrieve(mp3);

					mp3.Dispose();
				}

				await base.Song(retrieved);
			}
			public async override Task Image(IImage? retrieved, IEnumerable<ModelTypes>? modeltypes)
			{
				if (retrieved?.Buffer is null && (retrieved?.IsCorrupt ?? false) && await GetMp3Async(retrieved.Filepath, retrieved.Uri) is Mp3 mp3)
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

					retrieved.Retrieve(mp3, picturetypecomparer);

					mp3.Dispose();
				}

				await base.Image(retrieved, modeltypes);
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
									using Mp3 mp3 = new(song.Filepath, Mp3Permissions.Write);

									foreach (Id3Tag tag in mp3.GetAllTags())
									{
										int index = 0;

										if (old.Artist is not null)
										{
											if (tag.Artists.Value.AsEnumerable().Index(old.Artist) is int i)
												index = i;

											tag.Artists.Value.Remove(old.Artist);
										}

										if (updated.Artist is not null)
											tag.Artists.Value.Insert(index, updated.Artist);

										if (distincts.ReleaseDate) tag.Year.Value = updated.ReleaseDate?.Year;
										if (distincts.Title) tag.Title.Value = updated.Title;

										mp3.UpdateTag(tag);
									}
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
									using Mp3 mp3 = new(song.Filepath, Mp3Permissions.Write);

									foreach (Id3Tag tag in mp3.GetAllTags())
									{
										if (distincts.Name)
										{
											int index = tag.Artists.Value.AsEnumerable().Index(old.Name) ?? 0;
											tag.Artists.Value.Remove(old.Name);
											tag.Artists.Value.Insert(index, updated.Name);
										}

										mp3.UpdateTag(tag);
									}
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
									using Mp3 mp3 = new(song.Filepath, Mp3Permissions.Write);

									foreach (Id3Tag tag in mp3.GetAllTags())
									{
										if (distincts.Name) tag.Genre.Value = updated.Name;

										mp3.UpdateTag(tag);
									}
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

				if (string.IsNullOrWhiteSpace(old.Filepath) is false)
					await Task.Run(() =>
					{
						try
						{
							using Mp3 mp3 = new(old.Filepath, Mp3Permissions.Write);

							foreach (Id3Tag tag in mp3.GetAllTags())
							{
								if (distincts.Album) tag.Album.Value = updated.Album;
								if (distincts.Artist)
								{
									tag.Artists.Value.Clear();
									tag.Artists.Value.Add(updated.Artist);
								}
								if (distincts.Genre) tag.Genre.Value = updated.Genre;
								if (distincts.Lyrics)
								{
									tag.Lyrics.Clear();
									tag.Lyrics.Add(updated.Lyrics, string.Empty);
								}
								if (distincts.ReleaseDate) tag.Year.Value = updated.ReleaseDate?.Year;
								if (distincts.Title) tag.Title.Value = updated.Title;
								if (distincts.TrackNumber) tag.Track.Value = updated.TrackNumber ?? 0;

								bool f = mp3.UpdateTag(tag);
							}
						}
						catch (Exception) { }
					});

				await base.Song(old, updated);
			}
		}
	}
}
