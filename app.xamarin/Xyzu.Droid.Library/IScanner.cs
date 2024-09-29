using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library.Models;

using IAndroidOSBinder = Android.OS.IBinder;

namespace Xyzu.Library
{
	public partial interface IScanner
	{
		ServiceBinder? Binder { get; set; }
		ILibraryDroid? Library { get; set; }
		INotification? Notification { get; set; }
		ComponentName? Componentname { get; set; }

		int LatestStartId { get; set; }
		Intent? LatestIntent { get; set; }

		Task ScanSoft(CancellationToken cancellationtoken = default);
		Task ScanHard(CancellationToken cancellationtoken = default);

		public static bool ShouldScan(IEnumerable<string> filepaths, IEnumerable<ISong> songs, out IEnumerable<string> filepathstoadd, out IEnumerable<string> filepathstodelete)
		{
			filepathstoadd = Enumerable.Empty<string>();
			filepathstodelete = Enumerable.Empty<string>();

			IEnumerable<string> sqlitefilepaths = songs
				.Select(songentity => songentity.Filepath)
				.OfType<string>();

			filepathstoadd = filepaths.Where(filepath => sqlitefilepaths.Contains(filepath) is false);
			filepathstodelete = sqlitefilepaths.Where(filepath => filepaths.Contains(filepath) is false);

			return filepathstoadd.Any() || filepathstodelete.Any();
		}

		public class Notifications
		{
			public const int Id = 9;
			public const string ChannelId = "XyzuScannerNotifiationChannel";
			public const string ChannelName = "Xyzu Scanner Notifiation Channel";
		}
		public class IntentActions
		{
			public const string Destroy = "co.za.xyclonedesigns.xyzu.library.scanner.action.DESTROY";
			public const string ScanHard = "co.za.xyclonedesigns.xyzu.library.scanner.action.SCAN_HARD";
			public const string ScanSoft = "co.za.xyclonedesigns.xyzu.library.scanner.action.SCAN_SOFT";
		}
		public class MetadataKeys
		{
			public const string Ids = "Metadata.Ids";
		}

		[Service(
			Enabled = true,
			Exported = false,
			Label = "Xyzu Media & File Scanner Service",
			Name = "co.za.xyclonedesigns.xyzu.library.scannerservice",
			ForegroundServiceType = ForegroundService.TypeDataSync)]
		[IntentFilter(new[]
		{
			IntentActions.Destroy,
			IntentActions.ScanHard,
			IntentActions.ScanSoft,
		})]
		public partial class ScannerService : Service, IScanner
		{
			public Action? OnBindAction { get; set; }
			public ServiceBinder? Binder { get; set; }
			public ILibraryDroid? Library { get; set; }
			public INotification? Notification { get; set; }
			public ComponentName? Componentname { get; set; }

			public int LatestStartId { get; set; }
			public Intent? LatestIntent { get; set; }
			public CancellationTokenSource? Cancellationtokensource { get; set; }

			public override void OnDestroy()
			{
				Cancellationtokensource?.Cancel();

				Binder?.ServiceConnection?.OnServiceDisconnected(Componentname);
				StopForeground(StopForegroundFlags.Detach);
				StopSelf(LatestStartId);

				base.OnDestroy();
			}
			public override IAndroidOSBinder? OnBind(Intent? intent)
			{
				return Binder ??= new ServiceBinder(this);
			}
			public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
			{
				base.OnStartCommand(LatestIntent = intent, flags, LatestStartId = startId);

				Notification ??= new INotification.Default(Notifications.Id, Notifications.ChannelId, Notifications.ChannelName);

				switch (intent?.Action)
				{
					case IntentActions.Destroy:
						OnDestroy();
						break;

					case IntentActions.ScanSoft when Binder is not null:
						Task.Run(async () => await ScanSoft()); 
						break;

					case IntentActions.ScanHard when Binder is not null:
						Task.Run(async () => await ScanHard());
						break;

					case IntentActions.ScanSoft:
					case IntentActions.ScanHard:
						StartForeground(Notification.Id, Notification.CompatBuilder.Build());
						break;

					default: break;

				}

				return StartCommandResult.Sticky;
			}

			private async IAsyncEnumerable<AlbumEntity> CreateAlbums(IEnumerable<ISong> songs, ILibraryDroid.IParameters parameters, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
			{
				if (Library is null || songs.Any() is false)
					yield break;

				IAlbum? album = null;
				IEnumerable<ISong> albumsongs = songs
					.OrderBy(songentity => songentity.Album)
					.ThenBy(songentity => songentity.AlbumArtist)
					.ThenBy(songentity => songentity.TrackNumber);

				foreach (ISong albumsong in albumsongs)
					if (ILibraryDroid.IdFromAlbumTitle(albumsong.Album, albumsong.AlbumArtist) is string albumid)
					{
						if (album != null && albumid != album.Id)
						{
							yield return new AlbumEntity(album);

							album = null;
						}

						Notification?.Update(
							Notification?.ContentTextsScanning?.TextAlbums,
							string.Format("{0} - {1}", albumsong.Album, albumsong.AlbumArtist));

						album ??= new IAlbum.Default(albumid);
						album.ArtistId ??= ILibraryDroid.IdFromArtistName(albumsong.AlbumArtist);

						if (albumsong.Duration.HasValue)
							album.Duration += albumsong.Duration.Value;

						if (album.SongIds is null || album.SongIds.Contains(albumsong.Id) is false)
							album.SongIds = (album.SongIds ??= Enumerable.Empty<string>())
								.Append(albumsong.Id);

						parameters.RetrievedSongExtra = albumsong;
						if (Binder is not null) await ILibraryDroid.OnRetrieve(album, Binder.Actions?.OnRetrieve, parameters);

						if (album.Artwork?.Buffer is null && album.Artwork?.Uri is null)
						{
							parameters.Filepath = albumsong.Filepath;
							parameters.Uri = albumsong.Uri;

							if (Binder is not null) await ILibraryDroid.OnRetrieve(album.Artwork ??= new IImage.Default(), Binder.Actions?.OnRetrieve, parameters);
						}
					}

				if (album != null)
					yield return new AlbumEntity(album);
			}
			private async IAsyncEnumerable<ArtistEntity> CreateArtists(IEnumerable<ISong> songs, ILibraryDroid.IParameters parameters, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
			{
				if (Library is null || songs.Any() is false)
					yield break;

				IArtist? artist = null;
				IEnumerable<ISong> artistsongs = songs
					.OrderBy(song => song.Artist);

				foreach (ISong artistsong in artistsongs)
					if (ILibraryDroid.IdFromArtistName(artistsong.Artist) is string artistid)
					{
						Notification?.Update(Notification?.ContentTextsScanning?.TextArtists, artistsong.Artist);

						if (artist != null && artistid != artist.Id)
						{
							yield return new ArtistEntity(artist);

							artist = null;
						}

						artist ??= new IArtist.Default(artistid);

						if (ILibraryDroid.IdFromAlbumTitle(artistsong.Album, artistsong.AlbumArtist) is string albumid && (artist.AlbumIds is null || artist.AlbumIds.Contains(albumid) is false))
							artist.AlbumIds = (artist.AlbumIds ??= Enumerable.Empty<string>())
								.Append(albumid);

						if (artist.SongIds is null || artist.SongIds.Contains(artistsong.Id) is false)
							artist.SongIds = (artist.SongIds ??= Enumerable.Empty<string>())
								.Append(artistsong.Id);

						parameters.RetrievedSongExtra = artistsong;
						if (Binder is not null) await ILibraryDroid.OnRetrieve(artist, Binder.Actions?.OnRetrieve, parameters);

						if (artist.Image?.Buffer is null && artist.Image?.Uri is null)
						{
							parameters.Filepath = artistsong.Filepath;
							parameters.Uri = artistsong.Uri;

							if (Binder is not null) await ILibraryDroid.OnRetrieve(artist.Image ??= new IImage.Default(), Binder.Actions?.OnRetrieve, parameters);
						}
					}

				if (artist != null)
					yield return new ArtistEntity(artist);
			}
			private async IAsyncEnumerable<GenreEntity> CreateGenres(IEnumerable<ISong> songs, ILibraryDroid.IParameters parameters, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
			{
				if (Library is null || songs.Any() is false)
					yield break;

				IGenre? genre = null;
				IEnumerable<ISong> genresongs = songs
					.OrderBy(song => song.Genre);

				foreach (ISong genresong in genresongs)
					if (ILibraryDroid.IdFromGenreName(genresong.Genre) is string genreid)
					{
						Notification?.Update(Notification?.ContentTextsScanning?.TextGenres, genresong.Genre);

						if (genre != null && genreid != genre.Id)
						{
							yield return new GenreEntity(genre);

							genre = null;
						}

						genre ??= new IGenre.Default(genreid);

						if (genresong.Duration.HasValue)
							genre.Duration += genresong.Duration.Value;

						if (genre.SongIds is null || genre.SongIds.Contains(genresong.Id) is false)
							genre.SongIds = (genre.SongIds ??= Enumerable.Empty<string>())
								.Append(genresong.Id);

						parameters.RetrievedSongExtra = genresong;
						if (Binder is not null) await ILibraryDroid.OnRetrieve(genre, Binder.Actions?.OnRetrieve, parameters);
					}

				if (genre != null)
					yield return new GenreEntity(genre);
			}
			private async IAsyncEnumerable<SongEntity> CreateSongs(IEnumerable<string> songfilepaths, ILibraryDroid.IParameters parameters, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
			{
				if (Library is null || songfilepaths.Any() is false)
					yield break;

				if (Binder?.Actions?.OnCreate is ILibraryDroid.IOnCreateActions actions)
					foreach (string songfilepath in songfilepaths)
						if (await actions.Song(songfilepath) is ISong song)
						{
							Notification?.Update(Notification?.ContentTextsScanning?.TextSongs, song.Filepath);

							await ILibraryDroid.OnRetrieve(song, Binder.Actions?.OnRetrieve, parameters);

							parameters.Filepath = song.Filepath;
							parameters.Uri = song.Uri;

							await ILibraryDroid.OnRetrieve(song.Artwork ??= new IImage.Default(), Binder.Actions?.OnRetrieve, parameters);

							yield return new SongEntity(song);
						}
			}

			public async Task ScanHard(CancellationToken cancellationtoken = default)
			{
				if (Library is null || Binder?.Filepaths is null)
					return;

				await Binder.SQLiteLibrary.AlbumsTableAsync.DeleteAsync(_ => true);
				await Binder.SQLiteLibrary.ArtistsTableAsync.DeleteAsync(_ => true);
				await Binder.SQLiteLibrary.GenresTableAsync.DeleteAsync(_ => true);
				await Binder.SQLiteLibrary.SongsTableAsync.DeleteAsync(_ => true);

				using ILibraryDroid.IParameters parameters = new ILibraryDroid.IParameters.Default
				{
					RetrieverImage = new IImage.Default<bool>(false)
					{
						BufferHash = true,
						Uri = true,
					},
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => Binder.CursorToActions(() => Binder.DefaultCursorBuilder?
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				await foreach (SongEntity songentity in CreateSongs(Binder.Filepaths, parameters))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextSongs, songentity.Id);

					await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(songentity);
				}

				await foreach (AlbumEntity albumentity in CreateAlbums(Binder.SQLiteLibrary.SongsTable, parameters))
				{
					Notification?.Update(Notification?.ContentTextsSaving?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(albumentity);
				}

				await foreach (ArtistEntity artistentity in CreateArtists(Binder.SQLiteLibrary.SongsTable, parameters))
				{
					Notification?.Update(Notification?.ContentTextsSaving?.TextArtists, artistentity.Name);

					await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(artistentity);
				}

				await foreach (GenreEntity genreentity in CreateGenres(Binder.SQLiteLibrary.SongsTable, parameters))
				{
					Notification?.Update(Notification?.ContentTextsSaving?.TextGenres, genreentity.Name);

					await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(genreentity);
				}

				OnStartCommand(
					startId: 0,
					flags: StartCommandFlags.Retry,
					intent: new Intent(this, typeof(IScanner.ScannerService)).SetAction(IntentActions.Destroy));
			}
			public async Task ScanSoft(CancellationToken cancellationtoken = default)
			{
				if (Library is null || Binder?.Filepaths is null)
					return;

				ShouldScan(Binder.Filepaths, Binder.SQLiteLibrary.SongsTable, out IEnumerable<string> filepathstoadd, out IEnumerable<string> filepathstodelete);

				Notification?.Update(Notification.ContentTextsPreparing?.TextSongs, null);

				using ILibraryDroid.IParameters parameters = new ILibraryDroid.IParameters.Default()
				{
					RetrieverImage = new IImage.Default<bool>(false)
					{
						BufferHash = true,
						Uri = true,
					},
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => Binder.CursorToActions(() => Binder?.DefaultCursorBuilder?
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				List<SongEntity> songs_add = new();
				List<SongEntity> songs_remove = new();

				// Songs
				#region
				await foreach (SongEntity songentity in CreateSongs(filepathstoadd, parameters))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextSongs, songentity.Id);

					songs_add.Add(songentity);

					await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(songentity);
				}

				foreach (SongEntity song_remove in Binder.SQLiteLibrary.SongsTable)
					if (song_remove.Filepath != null && filepathstodelete.Contains(song_remove.Filepath))
					{
						Notification?.Update(Notification.ContentTextsRemoving?.TextSongs, song_remove.Id);

						songs_remove.Add(song_remove);

						await Binder.SQLiteLibrary.ConnectionAsync.DeleteAsync(song_remove);
					}
				#endregion
				// Songs

				Notification?.Update(Notification.ContentTextsPreparing?.TextAlbums, null);

				List<AlbumEntity> albums_add = new();
				List<AlbumEntity> albums_remove = new();

				// Albums
				#region
				await foreach (AlbumEntity albumentity in CreateAlbums(
					parameters: parameters,
					cancellationtoken: cancellationtoken,
					songs: songs_add.Where((Func<SongEntity, bool>)(song_add =>
					{
						return
							ILibraryDroid.IdFromAlbumTitle(song_add.Album, song_add.AlbumArtist) is string albumid &&
							Binder.SQLiteLibrary.AlbumsTable.Any<AlbumEntity>(album => album.Id == albumid) is false;

					}))))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					albums_add.Add(albumentity);

					await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(albumentity);
				}

				foreach (AlbumEntity albumentity in Binder.SQLiteLibrary.AlbumsTable)
				{
					IAlbum album = albumentity;

					IEnumerable<SongEntity> adds_songs = songs_add.Where(song_add => albumentity.Id == ILibraryDroid.IdFromAlbumTitle(song_add.Album, song_add.AlbumArtist));
					IEnumerable<SongEntity> removes_songs = songs_remove.Where(song_remove => albumentity.Id == ILibraryDroid.IdFromAlbumTitle(song_remove.Album, song_remove.AlbumArtist));

					if (adds_songs.Any() is false && removes_songs.Any() is false)
						continue;

					Notification?.Update(Notification.ContentTextsReading?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					album.Duration += adds_songs.Select(add => add.Duration).Sum() ?? default;
					album.SongIds = album.SongIds.Concat(adds_songs.Select(add => add.Id));

					album.Duration -= removes_songs.Select(remove => remove.Duration).Sum() ?? default;
					album.SongIds = album.SongIds.Except(removes_songs.Select(remove => remove.Id));

					album.SongIds = album.SongIds.Distinct();

					if (album.SongIds.Any() is false)
					{
						Notification?.Update(Notification.ContentTextsRemoving?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

						albums_remove.Add(albumentity);

						await Binder.SQLiteLibrary.ConnectionAsync.DeleteAsync(albumentity);
					}
					else
					{
						Notification?.Update(Notification.ContentTextsSaving?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

						await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(albumentity);
					}
				}
				#endregion
				// Albums

				Notification?.Update(Notification.ContentTextsPreparing?.TextArtists, null);

				List<ArtistEntity> artists_add = new();
				List<ArtistEntity> artists_remove = new();

				// Artists
				#region
				await foreach (ArtistEntity artistentity in CreateArtists(
					parameters: parameters,
					cancellationtoken: cancellationtoken,
					songs: songs_add.Where((Func<SongEntity, bool>)(song_add =>
					{
						return
							ILibraryDroid.IdFromArtistName(song_add.Artist ?? song_add.AlbumArtist) is string artistid &&
							Binder.SQLiteLibrary.ArtistsTable.Any<ArtistEntity>(artist => artist.Id == artistid) is false;

					}))))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextArtists, artistentity.Name);

					artists_add.Add(artistentity);

					await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(artistentity);
				}

				foreach (ArtistEntity artistentity in Binder.SQLiteLibrary.ArtistsTable)
				{
					IArtist artist = artistentity;

					IEnumerable<AlbumEntity> add_albums = albums_add.Where(album_add => album_add.ArtistId == artist.Id);
					IEnumerable<AlbumEntity> removes_albums = albums_remove.Where(album_remove => artist.AlbumIds.Contains(album_remove.Id));

					IEnumerable<SongEntity> adds_songs = songs_add.Where(song_add => artistentity.Id == ILibraryDroid.IdFromArtistName(song_add.Artist ?? song_add.AlbumArtist));
					IEnumerable<SongEntity> removes_songs = songs_remove.Where(song_remove => artistentity.Id == ILibraryDroid.IdFromArtistName(song_remove.Artist ?? song_remove.AlbumArtist));

					if (add_albums.Any() is false && removes_albums.Any() is false && adds_songs.Any() is false && removes_songs.Any() is false)
						continue;

					Notification?.Update(Notification.ContentTextsReading?.TextArtists, artistentity.Name);

					artist.AlbumIds = artist.AlbumIds.Concat(add_albums.Select(add => add.Id));
					artist.SongIds = artist.SongIds.Concat(adds_songs.Select(add => add.Id));

					artist.AlbumIds = artist.AlbumIds.Except(removes_albums.Select(remove => remove.Id));
					artist.SongIds = artist.SongIds.Except(removes_songs.Select(remove => remove.Id));

					artist.AlbumIds = artist.AlbumIds.Distinct();
					artist.SongIds = artist.SongIds.Distinct();

					if (artist.SongIds.Any() is false)
					{
						Notification?.Update(Notification.ContentTextsRemoving?.TextArtists, artistentity.Name);

						artists_remove.Add(artistentity);

						await Binder.SQLiteLibrary.ConnectionAsync.DeleteAsync(artistentity);
					}
					else
					{
						Notification?.Update(Notification.ContentTextsSaving?.TextArtists, artist.Name);

						await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(artistentity);
					}
				}
				#endregion
				// Artists

				Notification?.Update(Notification.ContentTextsPreparing?.TextGenres, null);

				List<GenreEntity> genres_add = new();
				List<GenreEntity> genres_remove = new();

				// Genres
				#region
				await foreach (GenreEntity genreentity in CreateGenres(
					parameters: parameters,
					cancellationtoken: cancellationtoken,
					songs: songs_add.Where((Func<SongEntity, bool>)(song_add =>
					{
						return
							ILibraryDroid.IdFromGenreName(song_add.Genre) is string genreid &&
							Binder.SQLiteLibrary.GenresTable.Any<GenreEntity>(genre => genre.Id == genreid) is false;

					}))))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextGenres, genreentity.Name);

					genres_add.Add(genreentity);

					await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(genreentity);
				}

				foreach (GenreEntity genreentity in Binder.SQLiteLibrary.GenresTable)
				{
					IGenre genre = genreentity;

					IEnumerable<SongEntity> adds_songs = songs_add.Where(song_add => genreentity.Id == ILibraryDroid.IdFromGenreName(song_add.Genre));
					IEnumerable<SongEntity> removes_songs = songs_remove.Where(song_remove => genreentity.Id == ILibraryDroid.IdFromGenreName(song_remove.Genre));

					if (adds_songs.Any() is false && removes_songs.Any() is false)
						continue;

					Notification?.Update(Notification.ContentTextsReading?.TextGenres, genreentity.Name);

					genre.Duration += adds_songs.Select(add => add.Duration).Sum() ?? default;
					genre.SongIds = genre.SongIds.Concat(adds_songs.Select(add => add.Id));

					genre.Duration -= removes_songs.Select(remove => remove.Duration).Sum() ?? default;
					genre.SongIds = genre.SongIds.Except(removes_songs.Select(remove => remove.Id));

					genre.SongIds = genre.SongIds.Distinct();

					if (genre.SongIds.Any() is false)
					{
						Notification?.Update(Notification.ContentTextsRemoving?.TextGenres, genreentity.Name);

						genres_remove.Add(genreentity);

						await Binder.SQLiteLibrary.ConnectionAsync.DeleteAsync(genreentity);
					}
					else
					{
						Notification?.Update(Notification.ContentTextsSaving?.TextGenres, genreentity.Name);

						await Binder.SQLiteLibrary.ConnectionAsync.InsertAsync(genreentity);
					}
				}
				#endregion
				// Genres		  

				OnStartCommand(
					startId: 0,
					flags: StartCommandFlags.Retry,
					intent: new Intent(this, typeof(IScanner.ScannerService)).SetAction(IntentActions.Destroy));
			}
		}
	}
}