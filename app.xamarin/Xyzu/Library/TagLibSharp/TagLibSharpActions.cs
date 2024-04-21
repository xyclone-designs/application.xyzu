using System;
using System.Collections.Generic;
using System.Linq;

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
			public IDictionary<string, string>? Paths { get; set; }

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
				string? filepath = Paths is null || Paths.TryGetValue(id, out string outpath) is false ? null : outpath;
				Uri? uri = filepath is null ? null : Uri.TryCreate(filepath, UriKind.RelativeOrAbsolute, out Uri outuri) ? outuri : null;

				ISong song = new ISong.Default(id)
				{
					Filepath = filepath,
					Uri = uri, 
				};

				if (filepath is not null)
					try
					{
						using File file = File.Create(filepath);

						song.Retrieve(file, retriever);
					}
					catch (Exception) { }

				return song;
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
			File? GetFile(string? filepath, Uri? uri)
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

			public override void Album(IAlbum? retrieved, IAlbum<bool>? retriever, ISong? retrievedsong)
			{
				if (GetFile(retrievedsong?.Filepath, retrievedsong?.Uri) is File file)
				{
					retrieved?.Retrieve(file, retriever);

					file.Dispose();
				}

				base.Album(retrieved, retriever, retrievedsong);
			}
			public override void Artist(IArtist? retrieved, IArtist<bool>? retriever, IAlbum? retrievedalbum)
			{
				base.Artist(retrieved, retriever, retrievedalbum);
			}
			public override void Artist(IArtist? retrieved, IArtist<bool>? retriever, ISong? retrievedsong)
			{
				if (GetFile(retrievedsong?.Filepath, retrievedsong?.Uri) is File file)
				{
					retrieved?.Retrieve(file, retriever);

					file.Dispose();
				}

				base.Artist(retrieved, retriever, retrievedsong);
			}
			public override void Genre(IGenre? retrieved, IGenre<bool>? retriever, ISong? retrievedsong)
			{
				if (GetFile(retrievedsong?.Filepath, retrievedsong?.Uri) is File file)
				{
					retrieved?.Retrieve(file, retriever);

					file.Dispose();
				}

				base.Genre(retrieved, retriever, retrievedsong);
			}
			public override void Playlist(IPlaylist? retrieved, IPlaylist<bool>? retriever, ISong? retrievedsong) 
			{
				if (GetFile(retrievedsong?.Filepath, retrievedsong?.Uri) is File file)
				{
					retrieved?.Retrieve(file, retriever);

					file.Dispose();
				}

				base.Playlist(retrieved, retriever, retrievedsong);
			}
			public override void Song(ISong? retrieved, ISong<bool>? retriever)
			{
				if (GetFile(retrieved?.Filepath, retrieved?.Uri) is File file)
				{
					retrieved?.Retrieve(file, retriever);

					file.Dispose();
				}

				base.Song(retrieved, retriever);
			}
			public override void Image(IImage? retrieved, IImage<bool>? retriever, string? filepath, Uri? uri, IEnumerable<ModelTypes>? modeltypes)
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

				base.Image(retrieved, retriever, filepath, uri, modeltypes);
			}
		}
		public class OnUpdate : ILibrary.IOnUpdateActions.Default
		{
			// TODO All Commented Outs
			public override void Album(IAlbum? old, IAlbum? updated)
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
					foreach (string songid in old.SongIds)
						if (OnGetSong.Invoke(songid) is ISong song && string.IsNullOrWhiteSpace(song.Filepath) is false)
							try
							{
								using File file = File.Create(song.Filepath);

								//if (distincts.ReleaseDate) file.Tag.Year = updated.ReleaseDate?.Year;
								if (distincts.Title) file.Tag.Album = updated.Title;

								file?.Save();
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
								using File file = File.Create(song.Filepath);

								if (distincts.Name && updated.Name != null) file.Tag.Performers = new string[] { updated.Name };

								file?.Save();
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
								using File file = File.Create(song.Filepath);

								if (distincts.Name) file.Tag.Genres = updated.Name?.Split(';').ToArray() ?? Array.Empty<string>();

								file?.Save();
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

				if (old.Filepath is not null)
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

						file?.Save();
					}
					catch (Exception) { }

				base.Song(old, updated);
			}
		}
	}
}
