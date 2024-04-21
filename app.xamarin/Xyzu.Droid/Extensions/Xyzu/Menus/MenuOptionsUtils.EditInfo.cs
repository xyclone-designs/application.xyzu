#nullable enable

using Android.Content;
using Android.Net;
using AndroidX.Activity.Result;
using AndroidX.Core.App;

using Google.Android.Material.BottomSheet;

using System;
using System.IO;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Views.Misc;
using Xyzu.Views.InfoEdit;

using Android.Views;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		private static DialogView EditInfoDialogView(VariableContainer variables, Action<DialogView>? oncreatedialogview)
		{
			if (variables.Context is null)
				throw new ArgumentException();

			DialogView dialogview = new DialogView(variables.Context)
			{
				ContentViewMaxHeight = DialogHeight(variables.Context),

				ButtonsPositiveText = Resource.String.save,
				ButtonsNegativeText = Resource.String.cancel,
				OnClickNegative = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerCancel),
			};

			oncreatedialogview?.Invoke(dialogview);

			return dialogview;
		}

		private static Intent CntractCreateIntentAction(Context context, Java.Lang.Object input)
		{
			IntentTypes intenttype = IntentTypes.Image;

			Intent actionpickcontent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.ExternalContentUri)
				.SetType(intenttype.AsIntentType());

			return actionpickcontent;
		}
		private static Java.Lang.Object ContractParseResultAction(int resultcode, Intent intent)
		{
			return intent;
		}

		public static BottomSheetDialog? EditInfoAlbum(VariableContainer variables)
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
				appcompatdialog.SetContentView(view: EditInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoEditAlbumView(variables.Context)
					{
						Images = XyzuImages.Instance,
						Album = variables.Album,
						AlbumArtworkClick = (view, sender, args) =>
						{
							if (variables.Activity?.RegisterForActivityResult((contract, callback) =>
							{
								contract.CreateIntentAction = CntractCreateIntentAction;
								contract.ParseResultAction = ContractParseResultAction;
								callback.OnActivityResultAction = result =>
								{
									Intent? intentresult = result as Intent;

									if (intentresult is null || view.Album is null)
										return;

									using Stream? stream = intentresult.Data is null ? null : view.Context?.ContentResolver?.OpenInputStream(intentresult.Data);

									view.Album.Artwork ??= new IImage.Default();
									view.Album.Artwork.Buffer = stream?.ToBytes();
									view.Album.Artwork.BufferHash = null;
									view.Album.Artwork.Uri = intentresult?.Data?.ToSystemUri();

									_dialogview.Palette = XyzuImages.Instance.GetPalette(view.Context, view.Album);
								};

							}) is ActivityResultLauncher activityresultlauncher)
								activityresultlauncher.Launch(null, ActivityOptionsCompat.MakeBasic());
						},
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Album);
					_dialogview.OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerSave ?? (dialoginterface =>
					{
						InfoEditAlbumView infoeditalbumview = (InfoEditAlbumView)_dialogview.ContentView;

						if (infoeditalbumview.Album is null || variables.Album is null)
							return;

						CreateSnackbar(variables, XyzuLibrary.Instance.Albums.UpdateAlbum(variables.Album, infoeditalbumview.Album)
							? Resource.String.edited
							: Resource.String.snackbars_albums_could_not_be_edited)?.Show();

						dialoginterface?.Dismiss();
					}));

				}));
			});
		}
		public static BottomSheetDialog? EditInfoArtist(VariableContainer variables)
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
				appcompatdialog.SetContentView(view: EditInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoEditArtistView(variables.Context)
					{
						Images = XyzuImages.Instance,
						Artist = variables.Artist,
						ArtistImageClick = (view, sender, args) =>
						{
							if (variables.Activity?.RegisterForActivityResult((contract, callback) =>
							{
								contract.CreateIntentAction = CntractCreateIntentAction;
								contract.ParseResultAction = ContractParseResultAction;
								callback.OnActivityResultAction = result =>
								{
									Intent? intentresult = result as Intent;

									if (intentresult is null || view.Artist is null)
										return;

									using Stream? stream = intentresult.Data is null ? null : view.Context?.ContentResolver?.OpenInputStream(intentresult.Data);

									view.Artist.Image ??= new IImage.Default();
									view.Artist.Image.Buffer = stream?.ToBytes();
									view.Artist.Image.BufferHash = null;
									view.Artist.Image.Uri = intentresult?.Data?.ToSystemUri();

									_dialogview.Palette = XyzuImages.Instance.GetPalette(view.Context, view.Artist);
								};

							}) is ActivityResultLauncher activityresultlauncher)
								activityresultlauncher.Launch(null, ActivityOptionsCompat.MakeBasic());
						},
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Artist);
					_dialogview.OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerSave ?? (dialoginterface =>
					{
						InfoEditArtistView infoeditartistview = (InfoEditArtistView)_dialogview.ContentView;

						if (infoeditartistview.Artist is null || variables.Artist is null)
							return;

						CreateSnackbar(variables, XyzuLibrary.Instance.Artists.UpdateArtist(variables.Artist, infoeditartistview.Artist)
							? Resource.String.edited
							: Resource.String.snackbars_artists_could_not_be_edited)?.Show();

						dialoginterface?.Dismiss();
					}));
				}));
			});
		}
		public static BottomSheetDialog? EditInfoGenre(VariableContainer variables)
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
				appcompatdialog.SetContentView(view: EditInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoEditGenreView(variables.Context)
					{
						Genre = variables.Genre,
						Images = XyzuImages.Instance,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Genre);
					_dialogview.OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerSave ?? (dialoginterface =>
					{
						InfoEditGenreView infoeditgenreview = (InfoEditGenreView)_dialogview.ContentView;

						if (infoeditgenreview.Genre is null || variables.Genre is null)
							return;

						CreateSnackbar(variables, XyzuLibrary.Instance.Genres.UpdateGenre(variables.Genre, infoeditgenreview.Genre)
							? Resource.String.edited
							: Resource.String.snackbars_genres_could_not_be_edited)?.Show();

						dialoginterface?.Dismiss();
					}));
				}));
			});
		}
		public static BottomSheetDialog? EditInfoPlaylist(VariableContainer variables)
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
				appcompatdialog.SetContentView(view: EditInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoEditPlaylistView(variables.Context)
					{
						Images = XyzuImages.Instance,
						Playlist = variables.Playlist,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Playlist);
					_dialogview.OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerSave ?? (dialoginterface =>
					{
						InfoEditPlaylistView infoeditplaylistview = (InfoEditPlaylistView)_dialogview.ContentView;

						if (infoeditplaylistview.Playlist is null || variables.Playlist is null)
							return;

						CreateSnackbar(variables, XyzuLibrary.Instance.Playlists.UpdatePlaylist(variables.Playlist, infoeditplaylistview.Playlist)
							? Resource.String.edited
							: Resource.String.snackbars_playlists_could_not_be_edited)?.Show();

						dialoginterface?.Dismiss();
					}));
				}));
			});
		}
		public static BottomSheetDialog? EditInfoSong(VariableContainer variables)
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
				appcompatdialog.SetContentView(view: EditInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoEditSongView(variables.Context)
					{
						Images = XyzuImages.Instance,
						Song = variables.Song,
						SongArtworkClick = (view, sender, args) =>
						{
							if (variables.Activity?.RegisterForActivityResult((contract, callback) =>
							{
								contract.CreateIntentAction = CntractCreateIntentAction;
								contract.ParseResultAction = ContractParseResultAction;
								callback.OnActivityResultAction = result =>
								{
									Intent? intentresult = result as Intent;

									if (intentresult is null || view.Song is null)
										return;

									using Stream? stream = intentresult.Data is null ? null : view.Context?.ContentResolver?.OpenInputStream(intentresult.Data);

									view.Song.Artwork ??= new IImage.Default();
									view.Song.Artwork.Buffer = stream?.ToBytes();
									view.Song.Artwork.BufferHash = null;
									view.Song.Artwork.Uri = intentresult?.Data?.ToSystemUri();

									view.ReloadImage();

									_dialogview.Palette = XyzuImages.Instance.GetPalette(view.Context, view.Song);
								};

							}) is ActivityResultLauncher activityresultlauncher)
								activityresultlauncher.Launch(null, ActivityOptionsCompat.MakeBasic());
						}
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Song);
					_dialogview.OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerSave ?? (dialoginterface =>
					{
						InfoEditSongView infoeditsongview = (InfoEditSongView)_dialogview.ContentView;

						if (infoeditsongview.Song is null || variables.Song is null)
							return;

						CreateSnackbar(variables, XyzuLibrary.Instance.Songs.UpdateSong(variables.Song, infoeditsongview.Song)
							? Resource.String.edited
							: Resource.String.snackbars_songs_could_not_be_edited)?.Show();

						dialoginterface?.Dismiss();
					}));
				}));
			});
		}
		public static BottomSheetDialog? EditInfoSongLyrics(VariableContainer variables)
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

			return XyzuUtils.Dialogs.BottomSheet(variables.Context, appcompatdialog =>
			{
				appcompatdialog.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				appcompatdialog.SetContentView(view: EditInfoDialogView(variables, _dialogview =>
				{
					_dialogview.ContentView = new InfoEditSongLyricsView(variables.Context)
					{
						Images = XyzuImages.Instance,
						Song = variables.Song,
					};

					_dialogview.Dialog = appcompatdialog;
					_dialogview.Palette = XyzuImages.Instance.GetPalette(variables.Song);
					_dialogview.OnClickPositive = CreateSheetDialogBottomViewAction(variables.DialogInterfaceListenerSave ?? (dialoginterface =>
					{
						InfoEditSongLyricsView infoeditsonglyricsview = (InfoEditSongLyricsView)_dialogview.ContentView;

						if (infoeditsonglyricsview.Song is null || variables.Song is null)
							return;

						CreateSnackbar(variables, XyzuLibrary.Instance.Songs.UpdateSong(variables.Song, infoeditsonglyricsview.Song)
							? Resource.String.edited
							: Resource.String.snackbars_songs_could_not_be_edited)?.Show();

						dialoginterface?.Dismiss();
					}));
				}));
			});
		}
	}
}