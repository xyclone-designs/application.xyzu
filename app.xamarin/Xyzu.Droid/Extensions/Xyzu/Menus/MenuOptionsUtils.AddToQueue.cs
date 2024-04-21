#nullable enable

using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library;
using Xyzu.Library.Models;
using Xyzu.Player;
using Xyzu.Player.Enums;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		private static IEnumerable<IQueueItem> QueueItemsFrom(IEnumerable<IAlbum> albums)
		{
			ILibrary.IIdentifiers identifiers;
			ISong<bool> retriever = new ISong.Default<bool>(false)
			{
				Album = true,
				AlbumArtist = true,
				Uri = true,
			};

			foreach (IAlbum album in albums)
			{
				IEnumerable<ISong> songs = XyzuLibrary.Instance.Songs
					.GetSongs(identifiers = ILibrary.IIdentifiers.FromAlbum(album), retriever);

				foreach (ISong song in songs)
					yield return IQueueItem.FromSong(song, queueitem =>
					{
						queueitem.SecondaryId = album.Id;
					});
			}
		}
		private static IEnumerable<IQueueItem> QueueItemsFrom(IEnumerable<IArtist> artists)
		{
			ILibrary.IIdentifiers identifiers;
			ISong<bool> retriever = new ISong.Default<bool>(false)
			{
				Artist = true,
				Uri = true,
			};

			foreach (IArtist artist in artists)
			{
				IEnumerable<ISong> songs = XyzuLibrary.Instance.Songs
					.GetSongs(identifiers = ILibrary.IIdentifiers.FromArtist(artist), retriever);

				foreach (ISong song in songs)
					yield return IQueueItem.FromSong(song, queueitem =>
					{
						queueitem.SecondaryId = artist.Id;
					});
			}
		}
		private static IEnumerable<IQueueItem> QueueItemsFrom(IEnumerable<IGenre> genres)
		{
			ILibrary.IIdentifiers identifiers;
			ISong<bool> retriever = new ISong.Default<bool>(false)
			{
				Genre = true,
				Uri = true,
			};

			foreach (IGenre genre in genres)
			{
				IEnumerable<ISong> songs = XyzuLibrary.Instance.Songs
					.GetSongs(identifiers = ILibrary.IIdentifiers.FromGenre(genre), retriever);

				foreach (ISong song in songs)
					yield return IQueueItem.FromSong(song, queueitem =>
					{
						queueitem.SecondaryId = genre.Id;
					});
			}
		}
		private static IEnumerable<IQueueItem> QueueItemsFrom(IEnumerable<IPlaylist> playlists)
		{
			ILibrary.IIdentifiers identifiers;
			ISong<bool> retriever = new ISong.Default<bool>(false)
			{
				Uri = true,
			};

			foreach (IPlaylist playlist in playlists)
			{
				IEnumerable<ISong> songs = XyzuLibrary.Instance.Songs
					.GetSongs(identifiers = ILibrary.IIdentifiers.FromPlaylist(playlist), retriever);

				foreach (ISong song in songs)
					yield return IQueueItem.FromSong(song, queueitem =>
					{
						queueitem.SecondaryId = playlist.Id;
					});
			}
		}
		private static IEnumerable<IQueueItem> QueueItemsFrom(IEnumerable<ISong> songs)
		{
			return songs
				.Select(song => IQueueItem.FromSong(song));
		}


		public static void AddToQueue(VariableContainer variables)
		{
			int itemcount = 0;
			IEnumerable<IQueueItem> queueitems = Enumerable.Empty<IQueueItem>();

			if ((variables.Albums?.Any() ?? false) is true)
			{
				itemcount += variables.Albums.Count();
				queueitems = queueitems.Concat(QueueItemsFrom(variables.Albums));
			}

			if ((variables.Artists?.Any() ?? false) is true)
			{
				itemcount += variables.Artists.Count();
				queueitems = queueitems.Concat(QueueItemsFrom(variables.Artists));
			}

			if ((variables.Genres?.Any() ?? false) is true)
			{
				itemcount += variables.Genres.Count();
				queueitems = queueitems.Concat(QueueItemsFrom(variables.Genres));
			}

			if ((variables.Playlists?.Any() ?? false) is true)
			{
				itemcount += variables.Playlists.Count();
				queueitems = queueitems.Concat(QueueItemsFrom(variables.Playlists));
			}

			if ((variables.Songs?.Any() ?? false) is true)
			{
				itemcount += variables.Songs.Count();
				queueitems = queueitems.Concat(QueueItemsFrom(variables.Songs));
			}

			if (itemcount == 0)
			{
				CreateSnackbar(variables, Resource.String.snackbars_items_none_selected)?
					.Show();

				return;
			}

			XyzuPlayer.Instance.Player.Queue.Add(QueuePositions.Last, queueitems);

			CreateSnackbar(variables, itemcount, Resource.String.snackbars_items_added)?
				.Show();
		}
		public static void AddToQueueAlbums(VariableContainer variables)
		{
			if (variables.Albums.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_albums_none_selected)?
					.Show();

				return;
			}

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(variables.Albums);

			XyzuPlayer.Instance.Player.Queue.Add(QueuePositions.Last, queueitems);

			CreateSnackbar(variables, variables.Albums.Count(), Resource.String.snackbars_albums_added)?
				.Show();
		}
		public static void AddToQueueArtists(VariableContainer variables)
		{
			if (variables.Artists.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_artists_none_selected)?
					.Show();

				return;
			}

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(variables.Artists);

			XyzuPlayer.Instance.Player.Queue.Add(QueuePositions.Last, queueitems);

			CreateSnackbar(variables, variables.Artists.Count(), Resource.String.snackbars_artists_added)?
				.Show();
		}
		public static void AddToQueueGenres(VariableContainer variables)
		{
			if (variables.Genres.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_genres_none_selected)?
					.Show();

				return;
			}

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(variables.Genres);

			XyzuPlayer.Instance.Player.Queue.Add(QueuePositions.Last, queueitems);

			CreateSnackbar(variables, variables.Genres.Count(), Resource.String.snackbars_genres_added)?
				.Show();
		}
		public static void AddToQueuePlaylists(VariableContainer variables)
		{
			if (variables.Playlists.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_playlists_none_selected)?
					.Show();

				return;
			}

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(variables.Playlists);

			XyzuPlayer.Instance.Player.Queue.Add(QueuePositions.Last, queueitems);

			CreateSnackbar(variables, variables.Playlists.Count(), Resource.String.snackbars_playlists_added)?
				.Show();
		}
		public static void AddToQueueSongs(VariableContainer variables)
		{
			if (variables.Songs.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_songs_none_selected)?
					.Show();

				return;
			}

			IEnumerable<IQueueItem> queueitems = QueueItemsFrom(variables.Songs);

			XyzuPlayer.Instance.Player.Queue.Add(QueuePositions.Last, queueitems);

			CreateSnackbar(variables, variables.Songs.Count(), Resource.String.snackbars_songs_added)?
				.Show();
		}
	}
}