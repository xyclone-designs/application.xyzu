using Id3;
using Id3.Frames;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Images;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;

namespace Xyzu.Library.ID3
{
	public partial class ID3Actions
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
			Mp3? GetMp3(string? filepath, Uri? uri)
			{
				Mp3? mp3 = null;

				if (mp3 is null && filepath is not null)
					try { mp3 = new Mp3(filepath); } catch (Exception) { }

				if (mp3 is null && uri is not null)
					try { mp3 = new Mp3(uri.AbsolutePath); } catch (Exception) { }

				return mp3;
			}

			public override void Album(IAlbum? retrieved, IAlbum<bool>? retriever, ISong? retrievedsong)
			{
				if (GetMp3(retrievedsong?.Filepath, retrievedsong?.Uri) is Mp3 mp3)
				{
					retrieved?.Retrieve(mp3, retriever);

					mp3.Dispose();
				}

				base.Album(retrieved, retriever, retrievedsong);
			}
			public override void Artist(IArtist? retrieved, IArtist<bool>? retriever, IAlbum? retrievedalbum)
			{
				base.Artist(retrieved, retriever, retrievedalbum);
			}
			public override void Artist(IArtist? retrieved, IArtist<bool>? retriever, ISong? retrievedsong)
			{
				if (GetMp3(retrievedsong?.Filepath, retrievedsong?.Uri) is Mp3 mp3)
				{
					retrieved?.Retrieve(mp3, retriever);

					mp3.Dispose();
				}

				base.Artist(retrieved, retriever, retrievedsong);
			}
			public override void Genre(IGenre? retrieved, IGenre<bool>? retriever, ISong? retrievedsong)
			{
				if (GetMp3(retrievedsong?.Filepath, retrievedsong?.Uri) is Mp3 mp3)
				{
					retrieved?.Retrieve(mp3, retriever);

					mp3.Dispose();
				}

				base.Genre(retrieved, retriever, retrievedsong);
			}
			public override void Playlist(IPlaylist? retrieved, IPlaylist<bool>? retriever, ISong? retrievedsong)
			{
				if (GetMp3(retrievedsong?.Filepath, retrievedsong?.Uri) is Mp3 mp3)
				{
					retrieved?.Retrieve(mp3, retriever);

					mp3.Dispose();
				}

				base.Playlist(retrieved, retriever, retrievedsong);
			}
			public override void Song(ISong? retrieved, ISong<bool>? retriever)
			{
				if (GetMp3(retrieved?.Filepath, retrieved?.Uri) is Mp3 mp3)
				{
					retrieved?.Retrieve(mp3, retriever);

					mp3.Dispose();
				}

				base.Song(retrieved, retriever);
			}
			public override void Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes)
			{
				if (GetMp3(filepath, uri) is Mp3 mp3)
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

					retrieved?.Retrieve(mp3, picturetypecomparer);

					mp3.Dispose();
				}

				base.Image(retrieved, retriever, filepath, uri, modeltypes);
			}
		}
		public class OnUpdate : ILibrary.IOnUpdateActions.Default
		{
			public override void Album(IAlbum? old, IAlbum? updated)
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
					foreach (string songid in old.SongIds)
						if (OnGetSong.Invoke(songid) is ISong song && string.IsNullOrWhiteSpace(song.Filepath) is false)
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

				base.Album(old, updated);
			}
			public override void Artist(IArtist? old, IArtist? updated)
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
					foreach (string songid in old.SongIds)
						if (OnGetSong.Invoke(songid) is ISong song && string.IsNullOrWhiteSpace(song.Filepath) is false)
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

				base.Artist(old, updated);
			}
			public override void Genre(IGenre? old, IGenre? updated)
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
					foreach (string songid in old.SongIds)
						if (OnGetSong.Invoke(songid) is ISong song && string.IsNullOrWhiteSpace(song.Filepath) is false)
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

				base.Genre(old, updated);
			}
			public override void Playlist(IPlaylist? old, IPlaylist? updated)
			{
				base.Playlist(old, updated);
			}
			public override void Song(ISong? old, ISong? updated)
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

				base.Song(old, updated);
			}
		}
	}
}
