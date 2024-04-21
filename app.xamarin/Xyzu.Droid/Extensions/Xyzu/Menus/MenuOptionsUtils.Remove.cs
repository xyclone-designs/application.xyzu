#nullable enable

using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		public static void Remove(VariableContainer variables)
		{
			IEnumerable<string> ids = Enumerable.Empty<string>();

			if (variables.Albums.SelectMany(album => album.SongIds) is IEnumerable<string> albumids)
				ids = ids.Concat(albumids);
			if (variables.Artists.SelectMany(artist => artist.SongIds) is IEnumerable<string> artistids)
				ids = ids.Concat(artistids);
			if (variables.Genres.SelectMany(genre => genre.SongIds) is IEnumerable<string> genreids)
				ids = ids.Concat(genreids);
			if (variables.Playlists.SelectMany(playlist => playlist.SongIds) is IEnumerable<string> playlistids)
				ids = ids.Concat(playlistids);
			if (variables.Songs.Select(song => song.Id) is IEnumerable<string> songids)
				ids = ids.Concat(songids);


			if (ids.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_items_none_selected)?
					.Show();

				return;
			}

			XyzuPlayer.Instance.Player.Queue.Remove(ids);

			CreateSnackbar(variables, ids.Count(), Resource.String.snackbars_items_removed)?
				.Show();
		}
		public static void RemoveAlbums(VariableContainer variables)
		{
			if (variables.Albums.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_albums_none_selected)?
					.Show();

				return;
			}

			IEnumerable<string> ids = variables.Albums.SelectMany(album => album.SongIds);

			XyzuPlayer.Instance.Player.Queue.Remove(ids);

			CreateSnackbar(variables, variables.Albums.Count(), Resource.String.snackbars_albums_removed)?
				.Show();
		}
		public static void RemoveArtists(VariableContainer variables)
		{
			if (variables.Artists.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_artists_none_selected)?
					.Show();

				return;
			}

			IEnumerable<string> ids = variables.Artists.SelectMany(artist => artist.SongIds);

			XyzuPlayer.Instance.Player.Queue.Remove(ids);

			CreateSnackbar(variables, variables.Artists.Count(), Resource.String.snackbars_artists_removed)?
				.Show();
		}
		public static void RemoveGenres(VariableContainer variables)
		{
			if (variables.Genres.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_genres_none_selected)?
					.Show();

				return;
			}

			IEnumerable<string> ids = variables.Genres.SelectMany(genre => genre.SongIds);

			XyzuPlayer.Instance.Player.Queue.Remove(ids);

			CreateSnackbar(variables, variables.Genres.Count(), Resource.String.snackbars_genres_removed)?
				.Show();
		}
		public static void RemovePlaylists(VariableContainer variables)
		{
			if (variables.Playlists.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_playlists_none_selected)?
					.Show();

				return;
			}

			IEnumerable<string> ids = variables.Playlists.SelectMany(playlist => playlist.SongIds);

			XyzuPlayer.Instance.Player.Queue.Remove(ids);

			CreateSnackbar(variables, variables.Playlists.Count(), Resource.String.snackbars_playlists_removed)?
				.Show();
		}
		public static void RemoveSongs(VariableContainer variables)
		{
			if (variables.Songs.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_songs_none_selected)?
					.Show();

				return;
			}

			IEnumerable<string> ids = variables.Songs.Select(song => song.Id);

			XyzuPlayer.Instance.Player.Queue.Remove(ids);

			CreateSnackbar(variables, variables.Songs.Count(), Resource.String.snackbars_songs_removed)?
				.Show();
		}
	}
}