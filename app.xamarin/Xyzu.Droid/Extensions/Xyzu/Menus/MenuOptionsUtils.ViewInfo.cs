using Android.Content;
using Android.Content.Res;
using Android.Views;

using Google.Android.Material.BottomSheet;

using System;

using Xyzu.Droid;
using Xyzu.Views.Misc;
using Xyzu.Views.Info;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		private static DialogInterfaceOnShowListener ViewInfoOnShowListener(View view, IDialogInterfaceOnShowListener? listener = null)
		{
			view.SetVisibility(ViewStates.Invisible);

			return new DialogInterfaceOnShowListener
			{
				OnShowAction = dialoginterface =>
				{
					if
					(
						view.Resources?.Configuration?.Orientation is Orientation.Landscape &&
						view.Parent is View parent &&
						parent.Parent is View grandparent
					) parent.TranslationX = grandparent.Width - view.Width - ((int)parent.GetX());

					view.SetVisibility(ViewStates.Visible);
					listener?.OnShow(dialoginterface);
				}
			};
		}
		private static DialogView ViewInfoDialogView(VariableContainer variables, Action<DialogView>? oncreatedialogview)
		{
			if (variables.Context is null)
				throw new ArgumentException(string.Empty);

			DialogView dialogview = new (variables.Context)
			{
				ContentViewMaxWidth = DialogMaxWidth(variables.Context),
				//ContentViewMaxHeight = DialogMaxHeight(variables.Context),

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

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				DialogView view = ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoAlbumView(variables.Context)
					{
						Album = variables.Album,
						Images = XyzuImages.Instance,
						OnPalette = palette => _dialogview.Palette = palette,
					};

					_dialogview.Dialog = appcompatdialog;
				});

				appcompatdialog.SetContentView(view, DialogLayoutParams(variables.Context));
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetOnShowListener(ViewInfoOnShowListener(view, variables.DialogInterfaceListenerOnShow));
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

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				DialogView view = ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoArtistView(variables.Context)
					{
						Artist = variables.Artist,
						Images = XyzuImages.Instance,
						OnPalette = palette => _dialogview.Palette = palette,
					};

					_dialogview.Dialog = appcompatdialog;
				});

				appcompatdialog.SetContentView(view, DialogLayoutParams(variables.Context));
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetOnShowListener(ViewInfoOnShowListener(view, variables.DialogInterfaceListenerOnShow));
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

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				DialogView view = ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoGenreView(variables.Context)
					{
						Genre = variables.Genre,
						Images = XyzuImages.Instance,
						OnPalette = palette => _dialogview.Palette = palette,
					};

					_dialogview.Dialog = appcompatdialog;
				});

				appcompatdialog.SetContentView(view, DialogLayoutParams(variables.Context));
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetOnShowListener(ViewInfoOnShowListener(view, variables.DialogInterfaceListenerOnShow));
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

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				DialogView view = ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoPlaylistView(variables.Context)
					{
						Playlist = variables.Playlist,
						Images = XyzuImages.Instance,
						OnPalette = palette => _dialogview.Palette = palette,
					};

					_dialogview.Dialog = appcompatdialog;
				});

				appcompatdialog.SetContentView(view, DialogLayoutParams(variables.Context));
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetOnShowListener(ViewInfoOnShowListener(view, variables.DialogInterfaceListenerOnShow));
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

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				DialogView view = ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.SetVisibility(ViewStates.Visible);
					_dialogview.ContentView = new InfoSongView(variables.Context)
					{
						Song = variables.Song,
						Images = XyzuImages.Instance,
						OnPalette = palette => _dialogview.Palette = palette,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.ButtonsNeutralText = Resource.String.lyrics;
					_dialogview.OnClickNeutral = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerLyrics);
				});

				appcompatdialog.SetContentView(view, DialogLayoutParams(variables.Context));
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetOnShowListener(ViewInfoOnShowListener(view, variables.DialogInterfaceListenerOnShow));
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

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				DialogView view = ViewInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoSongLyricsView(variables.Context)
					{
						Images = XyzuImages.Instance,
						Song = variables.Song,
						OnPalette = palette => _dialogview.Palette = palette,
					};

					_dialogview.Dialog = appcompatdialog;
				});

				appcompatdialog.SetContentView(view, DialogLayoutParams(variables.Context));
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetOnShowListener(ViewInfoOnShowListener(view, variables.DialogInterfaceListenerOnShow));
			});
		}
	}
}