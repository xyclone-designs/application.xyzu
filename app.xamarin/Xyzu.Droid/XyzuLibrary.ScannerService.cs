#nullable enable

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

using SQLite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Library;
using Xyzu.Library.Models;

namespace Xyzu
{
	public sealed partial class XyzuLibrary : Java.Lang.Object, IServiceConnection
	{

		private ServiceConnectionChangedEventArgs.Events? _ServiceConnectionState;
		public ServiceConnectionChangedEventArgs.Events ServiceConnectionState
		{
			private set => _ServiceConnectionState = value;
			get => _ServiceConnectionState ??= ServiceConnectionChangedEventArgs.Events.Disconnected;
		}
		public Action<ServiceConnectionChangedEventArgs>? ServiceConnectionChangedAction { get; set; }
	
		public event EventHandler<ServiceConnectionChangedEventArgs>? OnServiceConnectionChanged;

		public ScannerService.ServiceBinder? ScanServiceBinder { get; private set; }

		public void OnServiceConnected(ComponentName? name, IBinder? binder)
		{
			if (binder is null)
				return;

			ScanServiceBinder = binder as ScannerService.ServiceBinder;

			if (ScanServiceBinder != null)
			{
				ScanServiceBinder.ServiceConnection = this;
				ScanServiceBinder.Service.Componentname = name;
				ScanServiceBinder.Service.Library = (Instance as XyzuLibrary);

				ServiceConnectionState = ServiceConnectionChangedEventArgs.Events.Connected;

				ScanServiceBinder.Service.OnBindAction?.Invoke();
			}

			ServiceConnectionChangedEventArgs args = new ServiceConnectionChangedEventArgs(ServiceConnectionState)
			{
				Binder = binder,
				Name = name,
			};

			OnServiceConnectionChanged?.Invoke(this, args);
			ServiceConnectionChangedAction?.Invoke(args);
		}
		public void OnServiceDisconnected(ComponentName? name)
		{
			ScanServiceBinder = null;
			ServiceConnectionState = ServiceConnectionChangedEventArgs.Events.Disconnected;

			ServiceConnectionChangedEventArgs args = new ServiceConnectionChangedEventArgs(ServiceConnectionState)
			{
				Name = name,
			};

			OnServiceConnectionChanged?.Invoke(this, args);
			ServiceConnectionChangedAction?.Invoke(args);
		}

		public void ScannerServiceScan(bool hard)
		{
			if (hard is false && ScannerService.ShouldScan(XyzuLibrary.Instance, out IEnumerable<string> _, out IEnumerable<string> _) is false)
				return;

			Intent service = new Intent(Context, typeof(ScannerService))
				.SetAction(
					action: hard
						? ScannerService.IntentActions.ScanHard
						: ScannerService.IntentActions.ScanSoft);

			Context.StartService(service);
			Context.BindService(service, this, Bind.AutoCreate | Bind.Important);
		}

		[Service(
			Enabled = true,
			Exported = false,
			Label = "Xyzu Media & File Scanner Service",
			Name = "co.za.xyclonedesigns.xyzu.scannerservice",
			ForegroundServiceType = ForegroundService.TypeDataSync)]
		[IntentFilter(new[]
		{
			IntentActions.Destroy,
			IntentActions.ScanHard,
			IntentActions.ScanSoft,
		})]
		public partial class ScannerService : Service
		{
			public class IntentActions
			{
				public const string Destroy = "co.za.xyclonedesigns.xyzu.scanner.action.DESTROY";
				public const string ScanHard = "co.za.xyclonedesigns.xyzu.scanner.action.SCAN_HARD";
				public const string ScanSoft = "co.za.xyclonedesigns.xyzu.scanner.action.SCAN_SOFT";
			}
			public class MetadataKeys
			{
				public const string Ids = "Metadata.Ids";
			}

			private int _LatestStartId = 0;
			
			public Action? OnBindAction { get; set; }
			public XyzuLibrary? Library { get; set; }
			public ServiceBinder? Binder { get; set; }
			public ComponentName? Componentname { get; set; }
			public CancellationTokenSource? Cancellationtokensource { get; set; }

			public override void OnDestroy()
			{
				Cancellationtokensource?.Cancel();

				Binder?.ServiceConnection?.OnServiceDisconnected(Componentname);
				StopForeground(true);
				StopSelf(_LatestStartId);

				base.OnDestroy();
			}
			public override IBinder? OnBind(Intent? intent) 
			{
				return Binder ??= new ServiceBinder(this);
			}
			public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId) 
			{
				base.OnStartCommand(intent, flags, _LatestStartId = startId);

				switch (intent?.Action)
				{
					case IntentActions.Destroy:
						OnDestroy();
						break;

					case IntentActions.ScanSoft:
						OnBindAction = async () => await ScanSoft();
						StartForeground(NotificationId, NotificationBuilder.Build());
						break;

					case IntentActions.ScanHard:
						OnBindAction = async () => await ScanHard();
						StartForeground(NotificationId, NotificationBuilder.Build());
						break;

					default: break;
						
				}

				return StartCommandResult.Sticky;
			}

			private async IAsyncEnumerable<AlbumEntity> CreateAlbums(IEnumerable<ISong> songs, Parameters parameters, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
			{
				if (Library is null || songs.Any() is false)
					yield break;

				IAlbum? album = null;
				IEnumerable<ISong> albumsongs = songs
					.OrderBy(songentity => songentity.Album)
					.ThenBy(songentity => songentity.AlbumArtist)
					.ThenBy(songentity => songentity.TrackNumber);

				foreach (ISong albumsong in albumsongs)
					if (Library.AlbumTitleToId(albumsong.Album, albumsong.AlbumArtist) is string albumid)
					{
						if (album != null && albumid != album.Id)
						{
							yield return new AlbumEntity(album);

							album = null;
						}

						UpdateNotification(NotificationContentText_Scanning_Albums, string.Format("{0} - {1}", albumsong.Album, albumsong.AlbumArtist));

						album ??= new IAlbum.Default(albumid);
						album.ArtistId ??= Library.ArtistNameToId(albumsong.AlbumArtist);

						if (albumsong.Duration.HasValue)
							album.Duration += albumsong.Duration.Value;

						if (album.SongIds is null || album.SongIds.Contains(albumsong.Id) is false)
							album.SongIds = (album.SongIds ??= Enumerable.Empty<string>())
								.Append(albumsong.Id);

						parameters.RetrievedSongExtra = albumsong;
						await Library.PerformOnRetrieveActions(Library.OnRetrieveActions, album, parameters, cancellationtoken);

						if (album.Artwork?.Buffer is null && album.Artwork?.Uri is null)
						{
							parameters.Filepath = albumsong.Filepath;
							parameters.Uri = albumsong.Uri;

							await Library.PerformOnRetrieveActions(Library.OnRetrieveActions, album.Artwork ??= new IImage.Default(), parameters, cancellationtoken);
						}
					}

				if (album != null)
					yield return new AlbumEntity(album);
			}
			private async IAsyncEnumerable<ArtistEntity> CreateArtists(IEnumerable<ISong> songs, Parameters parameters, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
			{
				if (Library is null || songs.Any() is false)
					yield break;

				IArtist? artist = null;
				IEnumerable<ISong> artistsongs = songs
					.OrderBy(song => song.Artist);

				foreach (ISong artistsong in artistsongs)
					if (Library.ArtistNameToId(artistsong.Artist) is string artistid)
					{
						UpdateNotification(NotificationContentText_Scanning_Artists, artistsong.Artist);

						if (artist != null && artistid != artist.Id)
						{
							yield return new ArtistEntity(artist);

							artist = null;
						}

						artist ??= new IArtist.Default(artistid);

						if (Library.AlbumTitleToId(artistsong.Album, artistsong.AlbumArtist) is string albumid && (artist.AlbumIds is null || artist.AlbumIds.Contains(albumid) is false))
							artist.AlbumIds = (artist.AlbumIds ??= Enumerable.Empty<string>())
								.Append(albumid);

						if (artist.SongIds is null || artist.SongIds.Contains(artistsong.Id) is false)
							artist.SongIds = (artist.SongIds ??= Enumerable.Empty<string>())
								.Append(artistsong.Id);

						parameters.RetrievedSongExtra = artistsong;
						await Library.PerformOnRetrieveActions(Library.OnRetrieveActions, artist, parameters, cancellationtoken);

						if (artist.Image?.Buffer is null && artist.Image?.Uri is null)
						{
							parameters.Filepath = artistsong.Filepath;
							parameters.Uri = artistsong.Uri;

							await Library.PerformOnRetrieveActions(Library.OnRetrieveActions, artist.Image ??= new IImage.Default(), parameters, cancellationtoken);
						}
					}

				if (artist != null)
					yield return new ArtistEntity(artist);
			}
			private async IAsyncEnumerable<GenreEntity> CreateGenres(IEnumerable<ISong> songs, Parameters parameters, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
			{
				if (Library is null || songs.Any() is false)
					yield break;

				IGenre? genre = null;
				IEnumerable<ISong> genresongs = songs
					.OrderBy(song => song.Genre);

				foreach (ISong genresong in genresongs)
					if (Library.GenreNameToId(genresong.Genre) is string genreid)
					{
						UpdateNotification(NotificationContentText_Scanning_Genres, genresong.Genre);

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
						await Library.PerformOnRetrieveActions(Library.OnRetrieveActions, genre, parameters, cancellationtoken);
					}

				if (genre != null)
					yield return new GenreEntity(genre);
			}
			private async IAsyncEnumerable<SongEntity> CreateSongs(IEnumerable<string> songfilepaths, Parameters parameters, [EnumeratorCancellation] CancellationToken cancellationtoken = default)
			{
				if (Library is null || songfilepaths.Any() is false)
					yield break;

				IEnumerable<ISong> songs = songfilepaths
					.Select(filepath => Library.CreateSong(filepath, null, null));

				foreach (ISong song in songs)
				{
					UpdateNotification(NotificationContentText_Scanning_Songs, song.Filepath);

					await Library.PerformOnRetrieveActions(Library.OnRetrieveActions, song, parameters, cancellationtoken);

					parameters.Filepath = song.Filepath;
					parameters.Uri = song.Uri;

					await Library.PerformOnRetrieveActions(Library.OnRetrieveActions, song.Artwork ??= new IImage.Default(), parameters, cancellationtoken);

					yield return new SongEntity(song);
				};
			}

			public async Task ScanHard(CancellationToken cancellationtoken = default) 
			{
				if (Library is null)
					return;

				await Library.SqliteAlbumsTableQueryAsync.DeleteAsync(_ => true);
				await Library.SqliteArtistsTableQueryAsync.DeleteAsync(_ => true);
				await Library.SqliteGenresTableQueryAsync.DeleteAsync(_ => true);
				await Library.SqliteSongsTableQueryAsync.DeleteAsync(_ => true);	  		

				using Parameters parameters = new Parameters
				{
					RetrieverImage = new IImage.Default<bool>(false)
					{
						BufferHash = true,
						Uri = true,
					},
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => Library.CursorToActions(() => Library.DefaultCursorBuilder
						.WithRetriever(parameters.RetrieverSong)
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				await foreach (SongEntity songentity in CreateSongs(Library.Filepaths(null), parameters, cancellationtoken))
				{
					UpdateNotification(NotificationContentText_Saving_Songs, songentity.Id);

					await Library.SqliteConnectionAsync.InsertOrReplaceAsync(songentity);
				}

				await foreach (AlbumEntity albumentity in CreateAlbums(Library.SqliteSongsTableQuery, parameters, cancellationtoken))
				{
					UpdateNotification(NotificationContentText_Saving_Albums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					await Library.SqliteConnectionAsync.InsertOrReplaceAsync(albumentity);
				}

				await foreach (ArtistEntity artistentity in CreateArtists(Library.SqliteSongsTableQuery, parameters, cancellationtoken))
				{
					UpdateNotification(NotificationContentText_Saving_Artists, artistentity.Name);

					await Library.SqliteConnectionAsync.InsertOrReplaceAsync(artistentity);
				}

				await foreach (GenreEntity genreentity in CreateGenres(Library.SqliteSongsTableQuery, parameters, cancellationtoken))
				{
					UpdateNotification(NotificationContentText_Saving_Genres, genreentity.Name);

					await Library.SqliteConnectionAsync.InsertOrReplaceAsync(genreentity);
				}

				OnStartCommand(
					startId: 0,
					flags: StartCommandFlags.Retry,
					intent: new Intent(this, typeof(ScannerService)).SetAction(IntentActions.Destroy));
			}
			public async Task ScanSoft(CancellationToken cancellationtoken = default)
			{
				if (Library is null)
					return;

				ShouldScan(Library, out IEnumerable<string> filepathstoadd, out IEnumerable<string> filepathstodelete);

				UpdateNotification(NotificationContentText_Preparing_Songs, null);

				using Parameters parameters = new Parameters
				{
					RetrieverImage = new IImage.Default<bool>(false)
					{
						BufferHash = true,
						Uri = true,
					},
					CursorPositions = new Dictionary<string, int> { },
					CursorLazy = parameters => Library.CursorToActions(() => Library.DefaultCursorBuilder
						.WithRetriever(parameters.RetrieverSong)
						.WithLibraryIdentifiers(parameters.Identifiers)
						.Build()),
				};

				IList<SongEntity> songs_add = new List<SongEntity> { };
				IList<SongEntity> songs_remove = new List<SongEntity> { };

				// Songs
				#region
				await foreach (SongEntity songentity in CreateSongs(filepathstoadd, parameters, cancellationtoken))
				{
					UpdateNotification(NotificationContentText_Saving_Songs, songentity.Id);

					songs_add.Add(songentity);

					await Library.SqliteConnectionAsync.InsertOrReplaceAsync(songentity);
				}

				foreach (SongEntity song_remove in Library.SqliteSongsTableQuery)
					if (song_remove.Filepath != null && filepathstodelete.Contains(song_remove.Filepath))
					{
						UpdateNotification(NotificationContentText_Removing_Songs, song_remove.Id);

						songs_remove.Add(song_remove);

						await Library.SqliteConnectionAsync.DeleteAsync(song_remove);
					}
				#endregion
				// Songs

				UpdateNotification(NotificationContentText_Preparing_Albums, null);

				IList<AlbumEntity> albums_add = new List<AlbumEntity> { };
				IList<AlbumEntity> albums_remove = new List<AlbumEntity> { };

				// Albums
				#region
				await foreach (AlbumEntity albumentity in CreateAlbums(
					parameters: parameters,
					cancellationtoken: cancellationtoken,
					songs: songs_add.Where(song_add =>
					{
						return
							Library.AlbumTitleToId(song_add.Album, song_add.AlbumArtist) is string albumid &&
							Library.SqliteAlbumsTableQuery.Any(album => album.Id == albumid) is false;

					})))
				{
					UpdateNotification(NotificationContentText_Saving_Albums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					albums_add.Add(albumentity);

					await Library.SqliteConnectionAsync.InsertOrReplaceAsync(albumentity);
				}

				foreach (AlbumEntity albumentity in Library.SqliteAlbumsTableQuery)
				{
					IAlbum album = albumentity;

					IEnumerable<SongEntity> adds_songs = songs_add.Where(song_add => albumentity.Id == Library.AlbumTitleToId(song_add.Album, song_add.AlbumArtist));
					IEnumerable<SongEntity> removes_songs = songs_remove.Where(song_remove => albumentity.Id == Library.AlbumTitleToId(song_remove.Album, song_remove.AlbumArtist));

					if (adds_songs.Any() is false && removes_songs.Any() is false)
						continue;

					UpdateNotification(NotificationContentText_Reading_Albums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

					album.Duration += adds_songs.Select(add => add.Duration).Sum() ?? default;
					album.SongIds = album.SongIds.Concat(adds_songs.Select(add => add.Id));

					album.Duration -= removes_songs.Select(remove => remove.Duration).Sum() ?? default;
					album.SongIds = album.SongIds.Except(removes_songs.Select(remove => remove.Id));

					album.SongIds = album.SongIds.Distinct();

					if (album.SongIds.Any() is false)
					{
						UpdateNotification(NotificationContentText_Removing_Albums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

						albums_remove.Add(albumentity);

						await Library.SqliteConnectionAsync.DeleteAsync(albumentity);
					}
					else
					{
						UpdateNotification(NotificationContentText_Saving_Albums, string.Format("{0} - {1}", albumentity.Title, albumentity.Artist));

						await Library.SqliteConnectionAsync.InsertOrReplaceAsync(albumentity);
					}
				}
				#endregion
				// Albums

				UpdateNotification(NotificationContentText_Preparing_Artists, null);

				IList<ArtistEntity> artists_add = new List<ArtistEntity> { };
				IList<ArtistEntity> artists_remove = new List<ArtistEntity> { };

				// Artists
				#region
				await foreach (ArtistEntity artistentity in CreateArtists(
					parameters: parameters,
					cancellationtoken: cancellationtoken,
					songs: songs_add.Where(song_add =>
					{
						return
							Library.ArtistNameToId(song_add.Artist ?? song_add.AlbumArtist) is string artistid &&
							Library.SqliteArtistsTableQuery.Any(artist => artist.Id == artistid) is false;

					})))
				{
					UpdateNotification(NotificationContentText_Saving_Artists, artistentity.Name);

					artists_add.Add(artistentity);

					await Library.SqliteConnectionAsync.InsertOrReplaceAsync(artistentity);
				}

				foreach (ArtistEntity artistentity in Library.SqliteArtistsTableQuery)
				{
					IArtist artist = artistentity;

					IEnumerable<AlbumEntity> add_albums = albums_add.Where(album_add => album_add.ArtistId == artist.Id);
					IEnumerable<AlbumEntity> removes_albums = albums_remove.Where(album_remove => artist.AlbumIds.Contains(album_remove.Id));

					IEnumerable<SongEntity> adds_songs = songs_add.Where(song_add => artistentity.Id == Library.ArtistNameToId(song_add.Artist ?? song_add.AlbumArtist));
					IEnumerable<SongEntity> removes_songs = songs_remove.Where(song_remove => artistentity.Id == Library.ArtistNameToId(song_remove.Artist ?? song_remove.AlbumArtist));

					if (add_albums.Any() is false && removes_albums.Any() is false && adds_songs.Any() is false && removes_songs.Any() is false)
						continue;

					UpdateNotification(NotificationContentText_Reading_Artists, artistentity.Name);

					artist.AlbumIds = artist.AlbumIds.Concat(add_albums.Select(add => add.Id));
					artist.SongIds = artist.SongIds.Concat(adds_songs.Select(add => add.Id));

					artist.AlbumIds = artist.AlbumIds.Except(removes_albums.Select(remove => remove.Id));
					artist.SongIds = artist.SongIds.Except(removes_songs.Select(remove => remove.Id));

					artist.AlbumIds = artist.AlbumIds.Distinct();
					artist.SongIds = artist.SongIds.Distinct();

					if (artist.SongIds.Any() is false)
					{
						UpdateNotification(NotificationContentText_Removing_Artists, artistentity.Name);

						artists_remove.Add(artistentity);

						await Library.SqliteConnectionAsync.DeleteAsync(artistentity);
					}
					else
					{
						UpdateNotification(NotificationContentText_Saving_Artists, artist.Name);

						await Library.SqliteConnectionAsync.InsertOrReplaceAsync(artistentity);
					}
				}
				#endregion
				// Artists

				UpdateNotification(NotificationContentText_Preparing_Genres, null);

				IList<GenreEntity> genres_add = new List<GenreEntity> { };
				IList<GenreEntity> genres_remove = new List<GenreEntity> { };

				// Genres
				#region
				await foreach (GenreEntity genreentity in CreateGenres(
					parameters: parameters,
					cancellationtoken: cancellationtoken,
					songs: songs_add.Where(song_add =>
					{
						return
							Library.GenreNameToId(song_add.Genre) is string genreid &&
							Library.SqliteGenresTableQuery.Any(genre => genre.Id == genreid) is false;

					})))
				{
					UpdateNotification(NotificationContentText_Saving_Genres, genreentity.Name);

					genres_add.Add(genreentity);

					await Library.SqliteConnectionAsync.InsertOrReplaceAsync(genreentity);
				}

				foreach (GenreEntity genreentity in Library.SqliteGenresTableQuery)
				{
					IGenre genre = genreentity;

					IEnumerable<SongEntity> adds_songs = songs_add.Where(song_add => genreentity.Id == Library.GenreNameToId(song_add.Genre));
					IEnumerable<SongEntity> removes_songs = songs_remove.Where(song_remove => genreentity.Id == Library.GenreNameToId(song_remove.Genre));

					if (adds_songs.Any() is false && removes_songs.Any() is false)
						continue;

					UpdateNotification(NotificationContentText_Reading_Genres, genreentity.Name);

					genre.Duration += adds_songs.Select(add => add.Duration).Sum() ?? default;
					genre.SongIds = genre.SongIds.Concat(adds_songs.Select(add => add.Id));

					genre.Duration -= removes_songs.Select(remove => remove.Duration).Sum() ?? default;
					genre.SongIds = genre.SongIds.Except(removes_songs.Select(remove => remove.Id));

					genre.SongIds = genre.SongIds.Distinct();

					if (genre.SongIds.Any() is false)
					{

						UpdateNotification(NotificationContentText_Removing_Genres, genreentity.Name);

						genres_remove.Add(genreentity);

						await Library.SqliteConnectionAsync.DeleteAsync(genreentity);
					}
					else
					{
						UpdateNotification(NotificationContentText_Saving_Genres, genreentity.Name);

						await Library.SqliteConnectionAsync.InsertOrReplaceAsync(genreentity);
					}
				}
				#endregion
				// Genres		  

				OnStartCommand(
					startId: 0,
					flags: StartCommandFlags.Retry,
					intent: new Intent(this, typeof(ScannerService)).SetAction(IntentActions.Destroy));
			}
			public static bool ShouldScan(XyzuLibrary library, out IEnumerable<string> filepathstoadd, out IEnumerable<string> filepathstodelete)
			{
				filepathstoadd = Enumerable.Empty<string>();
				filepathstodelete = Enumerable.Empty<string>();

				IEnumerable<string> libraryfilepaths = library.Filepaths(null);
				IEnumerable<string> sqlitefilepaths = library.SqliteSongsTableQuery
					.Select(songentity => songentity.Filepath)
					.OfType<string>();

				filepathstoadd = libraryfilepaths.Where(filepath => sqlitefilepaths.Contains(filepath) is false);
				filepathstodelete = sqlitefilepaths.Where(filepath => libraryfilepaths.Contains(filepath) is false);

				return filepathstoadd.Any() || filepathstodelete.Any();
			}

			public class ServiceBinder : Binder
			{
				public ServiceBinder(ScannerService scannerservice)
				{
					Service = scannerservice;
				}

				public ScannerService Service { get; }
				public IServiceConnection? ServiceConnection { get; set; }
			}
		}
	}
}