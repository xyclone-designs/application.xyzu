using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
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
		ServiceNotification? Notification { get; set; }
		ComponentName? Componentname { get; set; }

		int LatestStartId { get; set; }
		Intent? LatestIntent { get; set; }

		ScanTypes? ScanType { get; }

		void ScanSoft(CancellationToken cancellationtoken = default);
		void ScanHard(CancellationToken cancellationtoken = default);

		public static bool ShouldScan(
			IEnumerable<string> dirfilepaths, IEnumerable<string> sqlitefilepaths, 
			out IEnumerable<string> filepathstoadd, out IEnumerable<string> filepathstodelete)
		{
			filepathstoadd = Enumerable.Empty<string>();
			filepathstodelete = Enumerable.Empty<string>();

			filepathstoadd = dirfilepaths.Where(filepath => sqlitefilepaths.Contains(filepath) is false);
			filepathstodelete = sqlitefilepaths.Where(filepath => dirfilepaths.Contains(filepath) is false);

			return filepathstoadd.Any() || filepathstodelete.Any();
		}

		public enum ScanTypes
		{
			Album,
			Artist,
			Genre,
			Playlist,
			Song,
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
			public ServiceNotification? Notification { get; set; } 
			public ComponentName? Componentname { get; set; }

			public int LatestStartId { get; set; }
			public Intent? LatestIntent { get; set; }
			public CancellationTokenSource? Cancellationtokensource { get; set; }
			
			public ScanTypes? ScanType { get; set; }

			public override void OnDestroy()
			{
				Cancellationtokensource?.Cancel();

				Binder?.ServiceConnection?.OnServiceDisconnected(Componentname);
				StopForeground(StopForegroundFlags.Remove);
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

				switch (intent?.Action)
				{
					case IntentActions.Destroy:
						OnDestroy();
						break;

					case IntentActions.ScanSoft when Binder is not null:
						if (Notification?.Built is false) StartForeground(Notification.Id, Notification.Build());
						ScanSoft();
						break;

					case IntentActions.ScanHard when Binder is not null:
						if (Notification?.Built is false) StartForeground(Notification.Id, Notification.Build());
						ScanHard();
						break;

					default: break;
				}

				return StartCommandResult.Sticky;
			}

			private async IAsyncEnumerable<AlbumEntity> CreateAlbums(IEnumerable<ISong>? songs, ILibraryDroid.IParameters parameters)
			{
				if (Binder?.Library?.Actions is null)
					yield break;

				songs ??= Binder.Library.SQLiteLibrary.SongsTable;

				foreach (IGrouping<string?, ISong> albumgrouping in songs
					.OrderBy(_ => _.TrackNumber)
					.GroupBy(_ => ILibrary.IdFromAlbumTitle(_.Album, _.AlbumArtist))) 
					if (albumgrouping.Key is not null && albumgrouping.FirstOrDefault() is ISong song)
					{
						Notification?.Update(
							Notification?.ContentTextsScanning?.TextAlbums,
							string.Format("{0} - {1}", song.Album, song.AlbumArtist));

						IAlbum.Default album = new (albumgrouping.Key)
						{
							Artist = song.AlbumArtist,
							ArtistId = ILibrary.IdFromArtistName(song.AlbumArtist),
							Artwork = albumgrouping.Select(_ =>
							{
								if (_.Artwork?.BufferKey is not null)
									return _.Artwork;

								return null;

							}).OfType<IImage.Default>().FirstOrDefault(),
							DiscCount = albumgrouping
								.Select(_ => _.DiscNumber)
								.Max(),
							Duration = albumgrouping.Sum(duration => duration.Duration ?? TimeSpan.Zero),
							SongIds = albumgrouping.Select(_ => _.Id),
							ReleaseDate = albumgrouping
								.Select(_ => _.ReleaseDate)
								.OfType<DateTime>()
								.Order()
								.FirstOrDefault(),
							Title = song.Album,
						};

						await Task.CompletedTask;

						yield return new AlbumEntity(album);
					}
			}
			private async IAsyncEnumerable<ArtistEntity> CreateArtists(IEnumerable<ISong>? songs, ILibraryDroid.IParameters parameters)
			{
				if (Binder?.Library?.Actions is null)
					yield break;

				songs ??= Binder.Library.SQLiteLibrary.SongsTable;

				foreach (IGrouping<string?, ISong> artistgrouping in songs
					.GroupBy(_ => ILibrary.IdFromArtistName(_.Artist)))
					if (artistgrouping.Key is not null && artistgrouping.FirstOrDefault() is ISong song)
					{
						Notification?.Update(Notification?.ContentTextsScanning?.TextArtists, song.Artist);

						IArtist.Default artist = new(artistgrouping.Key)
						{
							Name = song.Artist,
							AlbumIds = ILibrary.IdFromAlbumTitle(song.Album, song.AlbumArtist) is not string albumid
								? Enumerable.Empty<string>()
								: Enumerable.Empty<string>()
									.Append(albumid)
									.Distinct(),
							Image = artistgrouping.Select(_ =>
							{
								if (_.Artwork?.BufferKey is not null)
									return _.Artwork;

								return null;

							}).OfType<IImage.Default>().FirstOrDefault(),
							SongIds = Enumerable.Empty<string>()
								.Append(song.Id)
								.Distinct()
						};

						await Task.CompletedTask;

						yield return new ArtistEntity(artist);
					}
			}
			private async IAsyncEnumerable<GenreEntity> CreateGenres(IEnumerable<ISong>? songs, ILibraryDroid.IParameters parameters)
			{
				if (Binder?.Library?.Actions is null)
					yield break;

				songs ??= Binder.Library.SQLiteLibrary.SongsTable;

				foreach (IGrouping<string?, ISong> genregrouping in songs.GroupBy(_ => ILibrary.IdFromGenreName(_.Genre)))
					if (genregrouping.Key is not null)
					{
						Notification?.Update(Notification?.ContentTextsScanning?.TextGenres, genregrouping.Key);

						IGenre.Default genre = new (genregrouping.Key)
						{
							Name = genregrouping.FirstOrDefault(_ => _.Genre is not null)?.Genre,
							Duration = genregrouping.Sum(duration => duration.Duration ?? TimeSpan.Zero),
							SongIds = genregrouping.Select(_ => _.Id)
						};

						await Task.CompletedTask;

						yield return new GenreEntity(genre);
					}
			}
			private async IAsyncEnumerable<SongEntity> CreateSongs(IEnumerable<string> songfilepaths, ILibraryDroid.IParameters parameters)
			{
				if (Binder?.Library?.Actions is null || songfilepaths.Any() is false)
					yield break;

				if (Binder.Library.Actions.OnCreate is not null)
					foreach (string songfilepath in songfilepaths)
						if (await Binder.Library.Actions.OnCreate.Song(songfilepath) is ISong song)
						{
							Notification?.Update(Notification?.ContentTextsScanning?.TextSongs, song.Filepath);

							song.Artwork = new IImage.Default
							{
								Uri = song.Uri,
								Filepath = song.Filepath,
								IsCorrupt = song.Malformed,
							};
							
							await ILibraryDroid.OnRetrieve(song, Binder.Library.Actions.OnRetrieve, parameters);
							await ILibraryDroid.OnRetrieve(song.Artwork, Binder.Library.Actions.OnRetrieve, parameters);
							await Task.CompletedTask;

							yield return new SongEntity(song);
						}
			}
			
			public async void ScanHard(CancellationToken cancellationtoken = default)
			{
				if (Binder?.Library is null || Binder?.Filepaths is null)
					return;

				await Binder.Library.SQLiteLibrary.AlbumsTableAsync.DeleteAsync(_ => true);
				await Binder.Library.SQLiteLibrary.ArtistsTableAsync.DeleteAsync(_ => true);
				await Binder.Library.SQLiteLibrary.GenresTableAsync.DeleteAsync(_ => true);
				await Binder.Library.SQLiteLibrary.SongsTableAsync.DeleteAsync(_ => true);

				using ILibraryDroid.IParameters parameters = new ILibraryDroid.IParameters.Default
				{
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => Binder.CursorToActions(() => Binder.Library.DefaultCursorBuilder?
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				ScanType = ScanTypes.Song;

				await foreach (SongEntity songentity in CreateSongs(Binder.Filepaths, parameters))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextSongs, songentity.Id);

					await Binder.Library.SQLiteLibrary.ConnectionAsync.InsertAsync(songentity);
				}

				ScanType = ScanTypes.Album;

				await foreach (AlbumEntity albumentity in CreateAlbums(null, parameters))
				{
					Notification?.Update(Notification?.ContentTextsSaving?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					await Binder.Library.SQLiteLibrary.ConnectionAsync.InsertAsync(albumentity);
				}

				ScanType = ScanTypes.Artist;

				await foreach (ArtistEntity artistentity in CreateArtists(null, parameters))
				{
					Notification?.Update(Notification?.ContentTextsSaving?.TextArtists, artistentity.Name);

					await Binder.Library.SQLiteLibrary.ConnectionAsync.InsertAsync(artistentity);
				}

				ScanType = ScanTypes.Genre;

				await foreach (GenreEntity genreentity in CreateGenres(null, parameters))
				{
					Notification?.Update(Notification?.ContentTextsSaving?.TextGenres, genreentity.Name);

					await Binder.Library.SQLiteLibrary.ConnectionAsync.InsertAsync(genreentity);
				}

				await Task.CompletedTask;

				OnStartCommand(
					startId: 0,
					flags: StartCommandFlags.Retry,
					intent: new Intent(this, typeof(IScanner.ScannerService)).SetAction(IntentActions.Destroy));
			}
			public async void ScanSoft(CancellationToken cancellationtoken = default)
			{
				if (Binder?.Library is null || Binder.Filepaths is null)
					return;

				if (ShouldScan(
					Binder.Filepaths.ToList(), 
					Binder.Library.SQLiteLibrary.SongsTable.Select(_ => _.Filepath).OfType<string>().ToList(),
					out IEnumerable<string> filepathstoadd, out IEnumerable<string> filepathstodelete) is false)
				{
					OnStartCommand(
						startId: 0,
						flags: StartCommandFlags.Retry,
						intent: new Intent(this, typeof(IScanner.ScannerService)).SetAction(IntentActions.Destroy)); return;
				}

				Notification?.Update(Notification.ContentTextsPreparing?.TextSongs, null);

				using ILibraryDroid.IParameters parameters = new ILibraryDroid.IParameters.Default()
				{
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => Binder.CursorToActions(() => Binder.Library.DefaultCursorBuilder?
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				ScanType = ScanTypes.Song;
				List<SongEntity> songs_add = new();
				List<SongEntity> songs_remove = new();

				// Songs
				#region
				await foreach (SongEntity songentity in CreateSongs(filepathstoadd, parameters))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextSongs, songentity.Id);

					songs_add.Add(songentity);

					await Binder.Library.SQLiteLibrary.ConnectionAsync.InsertAsync(songentity);
				}

				foreach (SongEntity song_remove in Binder.Library.SQLiteLibrary.SongsTable
					.Where(_ => _.Filepath != null && filepathstodelete.Contains(_.Filepath)))
				{
					Notification?.Update(Notification.ContentTextsRemoving?.TextSongs, song_remove.Id);

					songs_remove.Add(song_remove);

					await Binder.Library.SQLiteLibrary.ConnectionAsync.DeleteAsync(song_remove);
				}
				#endregion
				// Songs

				Notification?.Update(Notification.ContentTextsPreparing?.TextAlbums, null);

				ScanType = ScanTypes.Album;
				List<AlbumEntity> albums_add = new();
				List<AlbumEntity> albums_remove = new();

				// Albums
				#region
				await foreach (AlbumEntity albumentity in CreateAlbums(
					parameters: parameters,
					songs: songs_add.Where((Func<SongEntity, bool>)(song_add =>
					{
						return
							ILibraryDroid.IdFromAlbumTitle(song_add.Album, song_add.AlbumArtist) is string albumid &&
							Binder.Library.SQLiteLibrary.AlbumsTable.Any<AlbumEntity>(album => album.Id == albumid) is false;
					}))))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					albums_add.Add(albumentity);

					await Binder.Library.SQLiteLibrary.ConnectionAsync.InsertAsync(albumentity);
				}

				foreach (AlbumEntity albumentity in Binder.Library.SQLiteLibrary.AlbumsTable)
				{
					IAlbum album = albumentity;

					IEnumerable<SongEntity> adds_songs = songs_add.Where(song_add => albumentity.Id == ILibraryDroid.IdFromAlbumTitle(song_add.Album, song_add.AlbumArtist));
					IEnumerable<SongEntity> removes_songs = songs_remove.Where(song_remove => albumentity.Id == ILibraryDroid.IdFromAlbumTitle(song_remove.Album, song_remove.AlbumArtist));

					if (adds_songs.Any() is false && removes_songs.Any() is false)
						continue;

					Notification?.Update(Notification.ContentTextsReading?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					album.Duration += adds_songs.Sum(add => add.Duration ?? TimeSpan.Zero);
					album.SongIds = album.SongIds.Concat(adds_songs.Select(add => add.Id));

					album.Duration -= removes_songs.Sum(remove => remove.Duration ?? TimeSpan.Zero);
					album.SongIds = album.SongIds.Except(removes_songs.Select(remove => remove.Id));

					album.SongIds = album.SongIds.Distinct();

					if (album.SongIds.Any() is false)
					{
						Notification?.Update(Notification.ContentTextsRemoving?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

						albums_remove.Add(albumentity);

						await Binder.Library.SQLiteLibrary.ConnectionAsync.DeleteAsync(albumentity);
					}
					else
					{
						Notification?.Update(Notification.ContentTextsSaving?.TextAlbums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

						await Binder.Library.SQLiteLibrary.ConnectionAsync.UpdateAsync(albumentity);
					}
				}
				#endregion
				// Albums

				Notification?.Update(Notification.ContentTextsPreparing?.TextArtists, null);

				ScanType = ScanTypes.Artist;
				List<ArtistEntity> artists_add = new();
				List<ArtistEntity> artists_remove = new();

				// Artists
				#region
				await foreach (ArtistEntity artistentity in CreateArtists(
					parameters: parameters,
					songs: songs_add.Where((Func<SongEntity, bool>)(song_add =>
					{
						return
							ILibraryDroid.IdFromArtistName(song_add.Artist ?? song_add.AlbumArtist) is string artistid &&
							Binder.Library.SQLiteLibrary.ArtistsTable.Any<ArtistEntity>(artist => artist.Id == artistid) is false;

					}))))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextArtists, artistentity.Name);

					artists_add.Add(artistentity);

					await Binder.Library.SQLiteLibrary.ConnectionAsync.InsertAsync(artistentity);
				}

				foreach (ArtistEntity artistentity in Binder.Library.SQLiteLibrary.ArtistsTable)
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

						await Binder.Library.SQLiteLibrary.ConnectionAsync.DeleteAsync(artistentity);
					}
					else
					{
						Notification?.Update(Notification.ContentTextsSaving?.TextArtists, artistentity.Name);

						await Binder.Library.SQLiteLibrary.ConnectionAsync.UpdateAsync(artistentity);
					}
				}
				#endregion
				// Artists

				Notification?.Update(Notification.ContentTextsPreparing?.TextGenres, null);

				ScanType = ScanTypes.Genre;
				List<GenreEntity> genres_add = new();
				List<GenreEntity> genres_remove = new();

				// Genres
				#region
				await foreach (GenreEntity genreentity in CreateGenres(
					parameters: parameters,
					songs: songs_add.Where((Func<SongEntity, bool>)(song_add =>
					{
						return
							ILibraryDroid.IdFromGenreName(song_add.Genre) is string genreid &&
							Binder.Library.SQLiteLibrary.GenresTable.Any<GenreEntity>(genre => genre.Id == genreid) is false;

					}))))
				{
					Notification?.Update(Notification.ContentTextsSaving?.TextGenres, genreentity.Name);

					genres_add.Add(genreentity);

					await Binder.Library.SQLiteLibrary.ConnectionAsync.InsertAsync(genreentity);
				}

				foreach (GenreEntity genreentity in Binder.Library.SQLiteLibrary.GenresTable)
				{
					IGenre genre = genreentity;

					IEnumerable<SongEntity> adds_songs = songs_add.Where(song_add => genreentity.Id == ILibraryDroid.IdFromGenreName(song_add.Genre));
					IEnumerable<SongEntity> removes_songs = songs_remove.Where(song_remove => genreentity.Id == ILibraryDroid.IdFromGenreName(song_remove.Genre));

					if (adds_songs.Any() is false && removes_songs.Any() is false)
						continue;

					Notification?.Update(Notification.ContentTextsReading?.TextGenres, genreentity.Name);

					genre.Duration += adds_songs.Sum(add => add.Duration ?? TimeSpan.Zero);
					genre.SongIds = genre.SongIds.Concat(adds_songs.Select(add => add.Id));

					genre.Duration -= removes_songs.Sum(remove => remove.Duration ?? TimeSpan.Zero);
					genre.SongIds = genre.SongIds.Except(removes_songs.Select(remove => remove.Id));

					genre.SongIds = genre.SongIds.Distinct();

					if (genre.SongIds.Any() is false)
					{
						Notification?.Update(Notification.ContentTextsRemoving?.TextGenres, genreentity.Name);

						genres_remove.Add(genreentity);

						await Binder.Library.SQLiteLibrary.ConnectionAsync.DeleteAsync(genreentity);
					}
					else
					{
						Notification?.Update(Notification.ContentTextsSaving?.TextGenres, genreentity.Name);

						await Binder.Library.SQLiteLibrary.ConnectionAsync.UpdateAsync(genreentity);
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