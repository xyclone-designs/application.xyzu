#nullable enable

using Google.Android.Material.BottomSheet;

using System;

using Xyzu.Droid;
using Xyzu.Views.Misc;
using Xyzu.Views.Info;
using Android.Content.Res;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		private static DialogView ViewInfoDialogView(VariableContainer variables, Action<DialogView>? oncreatedialogview)
		{
			if (variables.Context is null)
				throw new ArgumentException();

			DialogView dialogview = new DialogView(variables.Context)
			{
				ContentViewMaxWidth = DialogWidth(variables.Context),
				ContentViewMaxHeight = DialogHeight(variables.Context),

				ButtonsPositiveText = Resource.String.edit,
				ButtonsNegativeText = Resource.String.close,
				OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerEdit),
				OnClickNegative = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerClose),
			};

			oncreatedialogview?.Invoke(dialogview);

			return dialogview;
		}

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
				appcompatdialog.SetContentView(ViewInfoDialogView(variables, _dialogview => 
				{
					_dialogview.ContentView = new InfoAlbumView(variables.Context)
					{
						Album = variables.Album,
						Images = XyzuImages.Instance,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Album);

				}), DialogLayoutParams(variables.Context));
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
				appcompatdialog.SetContentView(ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoArtistView(variables.Context)
					{
						Artist = variables.Artist,
						Images = XyzuImages.Instance,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Artist);

				}), DialogLayoutParams(variables.Context));
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
				appcompatdialog.SetContentView(ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoGenreView(variables.Context)
					{
						Genre = variables.Genre,
						Images = XyzuImages.Instance,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Genre);

				}), DialogLayoutParams(variables.Context));
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
				appcompatdialog.SetContentView(ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoPlaylistView(variables.Context)
					{
						Playlist = variables.Playlist,
						Images = XyzuImages.Instance,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Playlist);

				}), DialogLayoutParams(variables.Context));
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
				appcompatdialog.SetContentView(ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoSongView(variables.Context)
					{
						Song = variables.Song,
						Images = XyzuImages.Instance,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Song);
					_dialogview.ButtonsNeutralText = Resource.String.lyrics;
					_dialogview.OnClickNeutral = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerLyrics);

				}), DialogLayoutParams(variables.Context));
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
				appcompatdialog.SetContentView(ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoSongLyricsView(variables.Context)
					{
						Song = variables.Song,
						Images = XyzuImages.Instance,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Song);
				
				}), DialogLayoutParams(variables.Context));
			});
		}
	}
}