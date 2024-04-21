
#nullable enable

using Google.Android.Material.BottomSheet;

using Xyzu.Droid;
using Xyzu.Views.Misc;
using Xyzu.Views.Info;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		public static BottomSheetDialog? ViewInfoAlbum(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Album is null)
			{
				CreateSnackbar(variables, Resource.String.snackbars_albums_none_selected)?
					.Show();

				return null;
			}

			XyzuLibrary.Instance.Albums.PopulateAlbum(variables.Album);
			XyzuLibrary.Instance.Misc.SetImage(variables.Album);

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetContentView(new DialogView(variables.Context)
				{
					ContentView = new InfoAlbumView(variables.Context)
					{
						Album = variables.Album,
						Images = XyzuImages.Instance,
					},

					Dialog = appcompatdialog,
					ContentViewMaxHeight = DialogHeight(variables.Context),
					Palette = XyzuImages.Instance.GetPalette(variables.Album),

					ButtonsPositiveText = Resource.String.edit,
					ButtonsNegativeText = Resource.String.close,
					OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerEdit),
					OnClickNegative = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerClose),
				});
			});
		}
		public static BottomSheetDialog? ViewInfoArtist(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Artists is null)
			{
				CreateSnackbar(variables, Resource.String.snackbars_artists_none_selected)?
					.Show();

				return null;
			}

			XyzuLibrary.Instance.Artists.PopulateArtist(variables.Artist);
			XyzuLibrary.Instance.Misc.SetImage(variables.Artist);

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetContentView(new DialogView(variables.Context)
				{
					ContentView = new InfoArtistView(variables.Context)
					{
						Artist = variables.Artist,
						Images = XyzuImages.Instance,
					},

					Dialog = appcompatdialog,
					ContentViewMaxHeight = DialogHeight(variables.Context),
					Palette = XyzuImages.Instance.GetPalette(variables.Artist),

					ButtonsPositiveText = Resource.String.edit,
					ButtonsNegativeText = Resource.String.close,
					OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerEdit),
					OnClickNegative = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerClose),
				});
			});
		}
		public static BottomSheetDialog? ViewInfoGenre(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Genre is null)
			{
				CreateSnackbar(variables, Resource.String.snackbars_genres_none_selected)?
					.Show();

				return null;
			}

			XyzuLibrary.Instance.Genres.PopulateGenre(variables.Genre);
			XyzuLibrary.Instance.Misc.SetImage(variables.Genre);

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetContentView(new DialogView(variables.Context)
				{
					ContentView = new InfoGenreView(variables.Context)
					{
						Genre = variables.Genre,
						Images = XyzuImages.Instance,
					},

					Dialog = appcompatdialog,
					ContentViewMaxHeight = DialogHeight(variables.Context),
					Palette = XyzuImages.Instance.GetPalette(variables.Genre),

					ButtonsPositiveText = Resource.String.edit,
					ButtonsNegativeText = Resource.String.close,
					OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerEdit),
					OnClickNegative = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerClose),
				});
			});
		}
		public static BottomSheetDialog? ViewInfoPlaylist(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Playlist is null)
			{
				CreateSnackbar(variables, Resource.String.snackbars_playlists_none_selected)?
					.Show();

				return null;
			}

			XyzuLibrary.Instance.Playlists.PopulatePlaylist(variables.Playlist);
			XyzuLibrary.Instance.Misc.SetImage(variables.Playlist);

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetContentView(new DialogView(variables.Context)
				{
					ContentView = new InfoPlaylistView(variables.Context)
					{
						Playlist = variables.Playlist,
						Images = XyzuImages.Instance,
					},

					Dialog = appcompatdialog,
					ContentViewMaxHeight = DialogHeight(variables.Context),
					Palette = XyzuImages.Instance.GetPalette(variables.Playlist),

					ButtonsPositiveText = Resource.String.edit,
					ButtonsNegativeText = Resource.String.close,
					OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerEdit),
					OnClickNegative = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerClose),
				});
			});
		}
		public static BottomSheetDialog? ViewInfoSong(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Song is null)
			{
				CreateSnackbar(variables, Resource.String.snackbars_songs_none_selected)?
					.Show();

				return null;
			}

			XyzuLibrary.Instance.Songs.PopulateSong(variables.Song);
			XyzuLibrary.Instance.Misc.SetImage(variables.Song);

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetContentView(new DialogView(variables.Context)
				{
					ContentView = new InfoSongView(variables.Context)
					{
						Song = variables.Song,
						Images = XyzuImages.Instance,
					},

					Dialog = appcompatdialog,
					ContentViewMaxHeight = DialogHeight(variables.Context),
					Palette = XyzuImages.Instance.GetPalette(variables.Song),

					ButtonsPositiveText = Resource.String.edit,
					ButtonsNegativeText = Resource.String.close,
					ButtonsNeutralText = Resource.String.lyrics,
					OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerEdit),
					OnClickNegative = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerClose),
					OnClickNeutral = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerLyrics),
				});
			});
		}
		public static BottomSheetDialog? ViewInfoSongLyrics(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Song is null)
			{
				CreateSnackbar(variables, Resource.String.snackbars_songs_none_selected)?
					.Show();

				return null;
			}

			XyzuLibrary.Instance.Songs.PopulateSong(variables.Song);
			XyzuLibrary.Instance.Misc.SetImage(variables.Song);

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetContentView(new DialogView(variables.Context)
				{
					ContentView = new InfoSongLyricsView(variables.Context)
					{
						Song = variables.Song,
						Images = XyzuImages.Instance,
					},

					Dialog = appcompatdialog,
					ContentViewMaxHeight = DialogHeight(variables.Context),
					Palette = XyzuImages.Instance.GetPalette(variables.Song),

					ButtonsPositiveText = Resource.String.edit,
					ButtonsNegativeText = Resource.String.close,
					OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerEdit),
					OnClickNegative = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerClose),
				});
			});
		}
	}
}