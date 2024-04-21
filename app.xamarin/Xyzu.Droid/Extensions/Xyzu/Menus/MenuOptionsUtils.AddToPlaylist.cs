#nullable enable

using Android.Text;
using Android.Content;

using Google.Android.Material.Snackbar;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library;
using Xyzu.Library.Models;
using Xyzu.Views.Option;

using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		private static IEnumerable<string> PlaylistNames()
		{
			return XyzuLibrary.Instance.Playlists
				.GetPlaylists(null, null)
				.Select(playlist => playlist.Name)
				.OfType<string>()
				.Distinct()
				.OrderBy(_ => _);
		}
		private static bool UpdatePlaylist(string? playlistname, IEnumerable<IAlbum> albums)
		{
			IEnumerable<string> songids = XyzuLibrary.Instance.Songs
				.GetSongs(
					identifiers: ILibrary.IIdentifiers.FromAlbums(albums),
					retriever: new ISong.Default<bool>(false)
					{
						Id = true

					}).Select(song => song.Id);

			return UpdatePlaylist(playlistname, songids);
		}
		private static bool UpdatePlaylist(string? playlistname, IEnumerable<IArtist> artists)
		{
			IEnumerable<string> songids = XyzuLibrary.Instance.Songs
				.GetSongs(
					identifiers: ILibrary.IIdentifiers.FromArtists(artists),
					retriever: new ISong.Default<bool>(false)
					{
						Id = true

					}).Select(song => song.Id);

			return UpdatePlaylist(playlistname, songids);
		}
		private static bool UpdatePlaylist(string? playlistname, IEnumerable<IGenre> genres)
		{
			IEnumerable<string> songids = XyzuLibrary.Instance.Songs
				.GetSongs(
					identifiers: ILibrary.IIdentifiers.FromGenres(genres),
					retriever: new ISong.Default<bool>(false)
					{
						Id = true

					}).Select(song => song.Id);

			return UpdatePlaylist(playlistname, songids);
		}
		private static bool UpdatePlaylist(string? playlistname, IEnumerable<ISong> songs)
		{
			IEnumerable<string> songids = songs.Select(song => song.Id);

			return UpdatePlaylist(playlistname, songids);
		}
		private static bool UpdatePlaylist(string? playlistname, IEnumerable<string> songids)
		{
			if (playlistname is null)
				return false;

			IPlaylist? playlist = XyzuLibrary.Instance.Playlists
				.GetPlaylist(
					retriever: null,
					identifiers: new ILibrary.IIdentifiers.Default
					{
						PlaylistNames = new string[1] { playlistname }
					});

			if (playlist is null)
				return false;

			IPlaylist editedplaylist = new IPlaylist.Default(playlist)
			{
				SongIds = songids
					.Concat(playlist.SongIds ?? Enumerable.Empty<string>())
					.ToList()
			};

			return XyzuLibrary.Instance.Playlists.UpdatePlaylist(playlist, editedplaylist);
		}


		public static Func<string?, bool> PlaylistOnCreate(OptionsCreateAndViewView optionscreateandviewview, VariableContainer? variables)
		{
			return playlistname =>
			{
				switch (true)
				{
					case true when playlistname is null:
					case true when string.IsNullOrWhiteSpace(playlistname):
						return false;

					default: return XyzuLibrary.Instance.Playlists.CreatePlaylist(new IPlaylist.Default(playlistname)
					{
						DateCreated = DateTime.Now,
						Name = playlistname,
					});
				}
			};
		}
		public static Func<OptionsCreateAndViewView, string?, bool> PlaylistOnCreateTextChanged(IEnumerable<string> playlistnames)
		{
			return (optioncreateandviewview, text) =>
			{
				switch (true)
				{
					case true when text is null:
					case true when string.IsNullOrWhiteSpace(text):
						optioncreateandviewview.CreateButton.Enabled = false;
						break;

					case true when playlistnames.Contains(text):
						optioncreateandviewview.CreateButton.Enabled = false;
						optioncreateandviewview.SetMessageError(string.Format("'{0}' {1}", text, optioncreateandviewview.Context?.Resources?.GetString(Resource.String.already_exists) ?? string.Empty));
						break;

					default:
						optioncreateandviewview.CreateButton.Enabled = true;
						optioncreateandviewview.SetMessageInfo(null);
						break;
				}

				return true;
			};
		}

		public static AppCompatAlertDialog? AddToPlaylist(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			IEnumerable<IAlbum> albums = Enumerable.Empty<IAlbum?>()
				.Append(variables.Album)
				.Concat(variables.Albums)
				.OfType<IAlbum>();
			IEnumerable<IArtist> artists = Enumerable.Empty<IArtist?>()
				.Append(variables.Artist)
				.Concat(variables.Artists)
				.OfType<IArtist>();
			IEnumerable<IGenre> genres = Enumerable.Empty<IGenre?>()
				.Append(variables.Genre)
				.Concat(variables.Genres)
				.OfType<IGenre>();
			IEnumerable<ISong> songs = Enumerable.Empty<ISong?>()
				.Append(variables.Song)
				.Concat(variables.Songs)
				.OfType<ISong>();

			int itemcount = albums.Count() + artists.Count() + genres.Count() + songs.Count();

			if (itemcount == 0)
			{
				CreateSnackbar(variables, Resource.String.snackbars_items_none_selected)?
					.Show();
				return null;
			}

			IEnumerable<string> playlistnames = PlaylistNames();

			return XyzuUtils.Dialogs.Alert(variables.Context, (alertdialogbuilder, alertdialog) =>
			{
				if (alertdialogbuilder != null)
				{
					alertdialogbuilder.SetTitle(Resource.String.add_to_playlist);
					alertdialogbuilder.SetMessage(Resource.String.add_to_playlist_albums);
					alertdialogbuilder.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
					alertdialogbuilder.SetNegativeButton(Resource.String.cancel, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerCancel));
				}

				if (alertdialog != null)
				{
					OptionsCreateAndViewView optioncreateandviewview = new OptionsCreateAndViewView(variables.Context)
					{
						OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames),
						OnCreate = (optioncreateandviewview, playlistname) =>
						{
							bool createresult = PlaylistOnCreate(optioncreateandviewview, variables)
								.Invoke(playlistname);

							if (createresult)
							{
								playlistnames = PlaylistNames();

								optioncreateandviewview.OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames);
								optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
							}

							return createresult;
						},
						ViewViewHolderOnBindAction = (viewholderdefault, position) =>
						{
							OptionsCreateAndViewView.ViewHolder viewholder = (OptionsCreateAndViewView.ViewHolder)viewholderdefault;

							viewholder.ItemView.SetText(playlistnames.ElementAt(position), null);
						},
						ViewViewHolderOnClickAction = args =>
						{
							if (UpdatePlaylist(playlistnames.ElementAtOrDefault(args.ViewHolder.AbsoluteAdapterPosition), songs
								.Concat(GetSongs(albums))
								.Concat(GetSongs(artists))
								.Concat(GetSongs(genres))
								.Select(song => song.Id)) is false)
								CreateSnackbar(variables, Resource.String.snackbars_items_could_not_be_added)?
									.Show();
							else
							{
								alertdialog.Dismiss();

								CreateSnackbar(variables, itemcount, Resource.String.added)?
									.Show();
							}
						}
					};

					alertdialog.SetView(optioncreateandviewview);
					alertdialog.SetOnShowListener(new DialogInterfaceOnShowListener
					{
						OnShowAction = dialoginterface =>
						{
							optioncreateandviewview.ViewGetItemCountFunc = () => playlistnames.Count();

							optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
						}
					});
				}
			});
		}
		public static AppCompatAlertDialog? AddToPlaylistAlbums(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if ((variables.Albums?.Any() ?? false) is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_albums_none_selected)?
					.Show();

				return null;
			}

			IEnumerable<string> playlistnames = PlaylistNames();

			return XyzuUtils.Dialogs.Alert(variables.Context, (alertdialogbuilder, alertdialog) =>
			{
				if (alertdialogbuilder != null)
				{
					alertdialogbuilder.SetTitle(Resource.String.add_to_playlist);
					alertdialogbuilder.SetMessage(Resource.String.add_to_playlist_albums);
					alertdialogbuilder.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
					alertdialogbuilder.SetNegativeButton(Resource.String.cancel, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerCancel));
				}

				if (alertdialog != null)
				{
					OptionsCreateAndViewView optioncreateandviewview = new OptionsCreateAndViewView(variables.Context)
					{
						OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames),
						OnCreate = (optioncreateandviewview, playlistname) =>
						{
							bool createresult = PlaylistOnCreate(optioncreateandviewview, variables)
								.Invoke(playlistname);

							if (createresult)
							{
								playlistnames = PlaylistNames();

								optioncreateandviewview.OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames);
								optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
							}

							return createresult;
						},
						ViewViewHolderOnBindAction = (viewholderdefault, position) =>
						{
							OptionsCreateAndViewView.ViewHolder viewholder = (OptionsCreateAndViewView.ViewHolder)viewholderdefault;

							viewholder.ItemView.SetText(playlistnames.ElementAt(position), null);
						},
						ViewViewHolderOnClickAction = args =>
						{
							if (UpdatePlaylist(playlistnames.ElementAtOrDefault(args.ViewHolder.AbsoluteAdapterPosition), variables.Albums) is false)
								CreateSnackbar(variables, Resource.String.snackbars_albums_could_not_be_added)?
									.Show();
							else
							{
								alertdialog.Dismiss();

								CreateSnackbar(variables, variables.Albums.Count(), Resource.String.added)?
									.Show();
							}
						}
					};

					alertdialog.SetView(optioncreateandviewview);
					alertdialog.SetOnShowListener(new DialogInterfaceOnShowListener
					{
						OnShowAction = dialoginterface =>
						{
							optioncreateandviewview.ViewGetItemCountFunc = () => playlistnames.Count();

							optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
						}
					});
				}
			});
		}
		public static AppCompatAlertDialog? AddToPlaylistArtists(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if ((variables.Artists?.Any() ?? false) is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_artists_none_selected)?
					.Show();

				return null;
			}

			IEnumerable<string> playlistnames = PlaylistNames();

			return XyzuUtils.Dialogs.Alert(variables.Context, (alertdialogbuilder, alertdialog) =>
			{
				if (alertdialogbuilder != null)
				{
					alertdialogbuilder.SetTitle(Resource.String.add_to_playlist);
					alertdialogbuilder.SetMessage(Resource.String.add_to_playlist_artists);
					alertdialogbuilder.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
					alertdialogbuilder.SetNegativeButton(Resource.String.cancel, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerCancel));
				}

				if (alertdialog != null)
				{
					OptionsCreateAndViewView optioncreateandviewview = new OptionsCreateAndViewView(variables.Context)
					{
						OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames),
						OnCreate = (optioncreateandviewview, playlistname) =>
						{
							bool createresult = PlaylistOnCreate(optioncreateandviewview, variables)
								.Invoke(playlistname);

							if (createresult)
							{
								playlistnames = PlaylistNames();

								optioncreateandviewview.OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames);
								optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
							}

							return createresult;
						},
						ViewViewHolderOnBindAction = (viewholderdefault, position) =>
						{
							OptionsCreateAndViewView.ViewHolder viewholder = (OptionsCreateAndViewView.ViewHolder)viewholderdefault;

							viewholder.ItemView.SetText(playlistnames.ElementAt(position), null);
						},
						ViewViewHolderOnClickAction = args =>
						{
							if (UpdatePlaylist(playlistnames.ElementAtOrDefault(args.ViewHolder.AbsoluteAdapterPosition), variables.Artists) is false)
								CreateSnackbar(variables, Resource.String.snackbars_artists_could_not_be_added)?
									.Show();
							else
							{
								alertdialog.Dismiss();

								CreateSnackbar(variables, variables.Artists.Count(), Resource.String.added)?
									.Show();
							}
						}
					};

					alertdialog.SetView(optioncreateandviewview);
					alertdialog.SetOnShowListener(new DialogInterfaceOnShowListener
					{
						OnShowAction = dialoginterface =>
						{
							optioncreateandviewview.ViewGetItemCountFunc = () => playlistnames.Count();

							optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
						}
					});
				}
			});
		}
		public static AppCompatAlertDialog? AddToPlaylistGenres(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if ((variables.Genres?.Any() ?? false) is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_genres_none_selected)?
					.Show();

				return null;
			}

			IEnumerable<string> playlistnames = PlaylistNames();

			return XyzuUtils.Dialogs.Alert(variables.Context, (alertdialogbuilder, alertdialog) =>
			{
				if (alertdialogbuilder != null)
				{
					alertdialogbuilder.SetTitle(Resource.String.add_to_playlist);
					alertdialogbuilder.SetMessage(Resource.String.add_to_playlist_genres);
					alertdialogbuilder.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
					alertdialogbuilder.SetNegativeButton(Resource.String.cancel, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerCancel));
				}

				if (alertdialog != null)
				{
					OptionsCreateAndViewView optioncreateandviewview = new OptionsCreateAndViewView(variables.Context)
					{
						OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames),
						OnCreate = (optioncreateandviewview, playlistname) =>
						{
							bool createresult = PlaylistOnCreate(optioncreateandviewview, variables)
								.Invoke(playlistname);

							if (createresult)
							{
								playlistnames = PlaylistNames();

								optioncreateandviewview.OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames);
								optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
							}

							return createresult;
						},
						ViewViewHolderOnBindAction = (viewholderdefault, position) =>
						{
							OptionsCreateAndViewView.ViewHolder viewholder = (OptionsCreateAndViewView.ViewHolder)viewholderdefault;

							viewholder.ItemView.SetText(playlistnames.ElementAt(position), null);
						},
						ViewViewHolderOnClickAction = args =>
						{
							if (UpdatePlaylist(playlistnames.ElementAtOrDefault(args.ViewHolder.AbsoluteAdapterPosition), variables.Genres) is false)
								CreateSnackbar(variables, Resource.String.snackbars_genres_could_not_be_added)?
									.Show();
							else
							{
								alertdialog.Dismiss();

								CreateSnackbar(variables, variables.Genres.Count(), Resource.String.added)?
									.Show();
							}
						}
					};

					alertdialog.SetView(optioncreateandviewview);
					alertdialog.SetOnShowListener(new DialogInterfaceOnShowListener
					{
						OnShowAction = dialoginterface =>
						{
							optioncreateandviewview.ViewGetItemCountFunc = () => playlistnames.Count();

							optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
						}
					});
				}
			});
		}
		public static AppCompatAlertDialog? AddToPlaylistSongs(VariableContainer variables)
		{
			if (variables.Context is null)
				return null;

			if ((variables.Songs?.Any() ?? false) is false)
			{
				CreateSnackbar(variables, Resource.String.snackbars_songs_none_selected)?
					.Show();

				return null;
			}

			IEnumerable<string> playlistnames = PlaylistNames();

			return XyzuUtils.Dialogs.Alert(variables.Context, (alertdialogbuilder, alertdialog) =>
			{
				if (alertdialogbuilder != null)
				{
					alertdialogbuilder.SetTitle(Resource.String.add_to_playlist);
					alertdialogbuilder.SetMessage(Resource.String.add_to_playlist_songs);
					alertdialogbuilder.SetOnDismissListener(variables.DialogInterfaceListenerOnDismiss);
					alertdialogbuilder.SetNegativeButton(Resource.String.cancel, CreateDialogInterfaceOnClickListener(variables.DialogInterfaceListenerCancel));
				}

				if (alertdialog != null)
				{
					OptionsCreateAndViewView optioncreateandviewview = new OptionsCreateAndViewView(variables.Context)
					{
						OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames),
						OnCreate = (optioncreateandviewview, playlistname) =>
						{
							bool createresult = PlaylistOnCreate(optioncreateandviewview, variables)
								.Invoke(playlistname);

							if (createresult)
							{
								playlistnames = PlaylistNames();

								optioncreateandviewview.OnCreateTextChanged = PlaylistOnCreateTextChanged(playlistnames);
								optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();

								optioncreateandviewview.SetMessageSuccess(string.Format("{0} {1}", playlistname, optioncreateandviewview.Context?.Resources?.GetString(Resource.String.created) ?? string.Empty));
							}

							return createresult;
						},
						ViewViewHolderOnBindAction = (viewholderdefault, position) =>
						{
							OptionsCreateAndViewView.ViewHolder viewholder = (OptionsCreateAndViewView.ViewHolder)viewholderdefault;

							viewholder.ItemView.SetText(playlistnames.ElementAt(position), null);
						},
						ViewViewHolderOnClickAction = args =>
						{
							if (UpdatePlaylist(playlistnames.ElementAtOrDefault(args.ViewHolder.AbsoluteAdapterPosition), variables.Songs) is false)
								CreateSnackbar(variables, Resource.String.snackbars_songs_could_not_be_added)?
									.Show();
							else
							{
								alertdialog.Dismiss();

								CreateSnackbar(variables, variables.Songs.Count(), Resource.String.added)?
									.Show();
							}
						}
					};

					alertdialog.SetView(optioncreateandviewview);
					alertdialog.SetOnShowListener(new DialogInterfaceOnShowListener
					{
						OnShowAction = dialoginterface =>
						{
							optioncreateandviewview.ViewGetItemCountFunc = () => playlistnames.Count();

							optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
						}
					});
				}
			});
		}
	}
}