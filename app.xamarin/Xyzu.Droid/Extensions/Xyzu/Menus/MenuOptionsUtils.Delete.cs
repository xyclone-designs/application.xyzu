#nullable enable

using Android.Content;
using Android.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library.Models;

using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		public static AppCompatAlertDialog? Delete(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			int itemcount = variables.Albums.Count() + variables.Artists.Count() + variables.Genres.Count() + variables.Playlists.Count() + variables.Songs.Count();

			if (itemcount == 0)
			{
				CreateSnackbar(variables, Resource.String.snackbars_items_none_selected)?
					.Show();
				return null;
			}

			return XyzuUtils.Dialogs.Alert(variables.Context, Delete_AlertDialogBuilderAction(
				variables: variables,
				title: itemcount == 1
					? Resource.String.delete_selected_item
					: Resource.String.delete_selected_items,
				additional: alertdialogbuilder =>
				{
					alertdialogbuilder.SetPositiveButton(Resource.String.confirm, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerConfirm, new DialogInterfaceOnClickListener
					{
						OnClickAction = (alertdialog, which) =>
						{
							int successcount = 0;
							int failcount = 0;

							foreach (IAlbum album in variables.Albums)
								if (XyzuLibrary.Instance.Albums.DeleteAlbum(album))
									successcount += 1;
								else failcount += 1;

							foreach (IArtist artist in variables.Artists)
								if (XyzuLibrary.Instance.Artists.DeleteArtist(artist))
									successcount += 1;
								else failcount += 1;

							foreach (IGenre genre in variables.Genres)
								if (XyzuLibrary.Instance.Genres.DeleteGenre(genre))
									successcount += 1;
								else failcount += 1;

							foreach (IPlaylist playlist in variables.Playlists)
								if (XyzuLibrary.Instance.Playlists.DeletePlaylist(playlist))
									successcount += 1;
								else failcount += 1;

							foreach (ISong song in variables.Songs)
								if (XyzuLibrary.Instance.Songs.DeleteSong(song))
									successcount += 1;
								else failcount += 1;

							if (successcount > 0)
								CreateSnackbar(variables, successcount, Resource.String.deleted)?
									.Show();

							if (failcount > 0)
								CreateSnackbar(variables, failcount, Resource.String.snackbars_items_could_not_be_deleted)?
									.Show();

							alertdialog?.Dismiss();
						},
					}));
				}));
		}		
		public static AppCompatAlertDialog? DeleteAlbums(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Albums.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_albums_none_selected)?
					.Show();

				return null;
			}

			return XyzuUtils.Dialogs.Alert(variables.Context, Delete_AlertDialogBuilderAction(
				variables: variables,
				title: variables.Albums.Count() == 1
					? Resource.String.delete_selected_album
					: Resource.String.delete_selected_albums,
				additional: alertdialogbuilder =>
				{
					alertdialogbuilder.SetPositiveButton(Resource.String.confirm, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerConfirm, new DialogInterfaceOnClickListener
					{
						OnClickAction = (alertdialog, which) =>
						{
							int successcount = 0;
							int failcount = 0;
																				  
							foreach (IAlbum album in variables.Albums)
								if (XyzuLibrary.Instance.Albums.DeleteAlbum(album))
									successcount += 1;
								else failcount += 1;

							if (successcount > 0)
								CreateSnackbar(variables, successcount, Resource.String.deleted)?
									.Show();

							if (failcount > 0)
								CreateSnackbar(variables, failcount, Resource.String.snackbars_albums_could_not_be_deleted)?
									.Show();

							alertdialog?.Dismiss();
						},
					}));
				}));
		}
		public static AppCompatAlertDialog? DeleteArtists(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Artists.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_artists_none_selected)?
					.Show();

				return null;
			}

			return XyzuUtils.Dialogs.Alert(variables.Context, Delete_AlertDialogBuilderAction(
				variables: variables,
				title: variables.Artists.Count() == 1
					? Resource.String.delete_selected_artist
					: Resource.String.delete_selected_artists,
				additional: alertdialogbuilder =>
				{
					alertdialogbuilder.SetPositiveButton(Resource.String.confirm, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerConfirm, new DialogInterfaceOnClickListener
					{
						OnClickAction = (alertdialog, which) =>
						{
							int successcount = 0;
							int failcount = 0;
																					   
							foreach (IArtist artist in variables.Artists)
								if (XyzuLibrary.Instance.Artists.DeleteArtist(artist))
									successcount += 1;
								else failcount += 1;

							if (successcount > 0)
								CreateSnackbar(variables, successcount, Resource.String.deleted)?
									.Show();

							if (failcount > 0)
								CreateSnackbar(variables, failcount, Resource.String.snackbars_artists_could_not_be_deleted)?
									.Show();

							alertdialog?.Dismiss();
						},
					}));
				}));
		}
		public static AppCompatAlertDialog? DeleteGenres(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Genres.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_genres_none_selected)?
					.Show();

				return null;
			}

			return XyzuUtils.Dialogs.Alert(variables.Context, Delete_AlertDialogBuilderAction(
				variables: variables,
				title: variables.Genres.Count() == 1
					? Resource.String.delete_selected_genre
					: Resource.String.delete_selected_genres,
				additional: alertdialogbuilder =>
				{
					alertdialogbuilder.SetPositiveButton(Resource.String.confirm, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerConfirm, new DialogInterfaceOnClickListener
					{
						OnClickAction = (alertdialog, which) =>
						{
							int successcount = 0;
							int failcount = 0;
							
							foreach (IGenre genre in variables.Genres)
								if (XyzuLibrary.Instance.Genres.DeleteGenre(genre))
									successcount += 1;
								else failcount += 1;

							if (successcount > 0)
								CreateSnackbar(variables, successcount, Resource.String.deleted)?
									.Show();

							if (failcount > 0)
								CreateSnackbar(variables, failcount, Resource.String.snackbars_genres_could_not_be_deleted)?
									.Show();

							alertdialog?.Dismiss();
						},
					}));
				}));
		}
		public static AppCompatAlertDialog? DeletePlaylists(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if ((variables.Playlists?.Any() ?? false) is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_playlists_none_selected)?
					.Show();

				return null;
			}

			return XyzuUtils.Dialogs.Alert(variables.Context, Delete_AlertDialogBuilderAction(
				variables: variables,
				title: variables.Playlists.Count() == 1
					? Resource.String.delete_selected_playlist
					: Resource.String.delete_selected_playlists,
				additional: alertdialogbuilder =>
				{
					alertdialogbuilder.SetPositiveButton(Resource.String.confirm, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerConfirm, new DialogInterfaceOnClickListener
					{
						OnClickAction = (alertdialog, which) =>
						{
							int successcount = 0;
							int failcount = 0;

							foreach (IPlaylist playlist in variables.Playlists)
								if (XyzuLibrary.Instance.Playlists.DeletePlaylist(playlist))
									successcount += 1;
								else failcount += 1;

							if (successcount > 0)
								CreateSnackbar(variables, successcount, Resource.String.deleted)?
									.Show();

							if (failcount > 0)
								CreateSnackbar(variables, failcount, Resource.String.snackbars_playlists_could_not_be_deleted)?
									.Show();

							alertdialog?.Dismiss();
						},
					}));
				}));
		}
		public static AppCompatAlertDialog? DeleteSongs(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if (variables.Songs.Any() is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_songs_none_selected)?
					.Show();

				return null;
			}

			return XyzuUtils.Dialogs.Alert(variables.Context, Delete_AlertDialogBuilderAction(
				variables: variables,
				title: variables.Songs.Count() == 1
					? Resource.String.delete_selected_song
					: Resource.String.delete_selected_songs,
				additional: alertdialogbuilder =>
				{
					alertdialogbuilder.SetPositiveButton(Resource.String.confirm, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerConfirm, new DialogInterfaceOnClickListener
					{
						OnClickAction = (alertdialog, which) =>
						{
							int successcount = 0;
							int failcount = 0;

							foreach (ISong song in variables.Songs)
								if (XyzuLibrary.Instance.Songs.DeleteSong(song))
									successcount += 1;
								else failcount += 1;

							if (successcount > 0)
								CreateSnackbar(variables, successcount, Resource.String.deleted)?
									.Show();

							if (failcount > 0)
								CreateSnackbar(variables, failcount, Resource.String.snackbars_songs_could_not_be_deleted)?
									.Show();

							alertdialog?.Dismiss();
						},
					}));
				}));
		}

		private static Action<AppCompatAlertDialog.Builder>? Delete_AlertDialogBuilderAction(int title, VariableContainer variables, Action<AppCompatAlertDialog.Builder>? additional)
		{
			return alertdialogbuilder =>
			{
				alertdialogbuilder.SetTitle(title);
				alertdialogbuilder.SetMessage(Resource.String.delete_warning_action_cant_be_undone);
				alertdialogbuilder.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
				alertdialogbuilder.SetNegativeButton(Resource.String.cancel, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerCancel));

				additional?.Invoke(alertdialogbuilder);
			};
		}
	}
}