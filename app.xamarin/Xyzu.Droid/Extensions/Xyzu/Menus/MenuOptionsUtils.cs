#nullable enable

using Android.Content;
using Android.Views;
using AndroidX.Activity.Result;
using AndroidX.AppCompat.Widget;

using Google.Android.Material.Snackbar;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Activities;
using Xyzu.Droid;
using Xyzu.Library;
using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;

using ILibraryNavigatable = Xyzu.Views.Library.ILibrary.INavigatable;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		public partial class VariableContainer
		{
			public Context? Context { get; set; }
			public View? AnchorView { get; set; }
			public ViewGroup? AnchorViewGroup { get; set; }
			public LibraryActivity? Activity { get; set; }
			public ILibraryNavigatable? LibraryNavigatable { get; set; }
			public IActivityResultCaller? ActivityResultCaller { get; set; }

			public IAlbumSettings? AlbumSettings { get; set; }
			public IAlbumsSettings? AlbumsSettings { get; set; }
			public IArtistSettings? ArtistSettings { get; set; }
			public IArtistsSettings? ArtistsSettings { get; set; }
			public IGenreSettings? GenreSettings { get; set; }
			public IGenresSettings? GenresSettings { get; set; }
			public IPlaylistSettings? PlaylistSettings { get; set; }
			public IPlaylistsSettings? PlaylistsSettings { get; set; }
			public ISongsSettings? SongsSettings { get; set; }

			public View? SnackbarParent { get; set; }
			public Action<Snackbar>? SnackbarOnCreate { get; set; }
			public string? SnackbarText { get; set; }

			public IAlbum? Album 
			{
				get => Albums.Count() == 1 ? Albums.FirstOrDefault() : null;
			} 
			public IArtist? Artist 
			{ 
				get => Artists.Count() == 1 ? Artists.FirstOrDefault() : null;
			} 
			public IGenre? Genre 
			{ 
				get => Genres.Count() == 1 ? Genres.FirstOrDefault() : null;
			} 
			public IPlaylist? Playlist 
			{ 
				get => Playlists.Count() == 1 ? Playlists.FirstOrDefault() : null;
			} 
			public ISong? Song 
			{ 
				get => Songs.Count() == 1 ? Songs.FirstOrDefault() : null;
			} 			
			public IEnumerable<IAlbum> Albums { get; set; } = Enumerable.Empty<IAlbum>();
			public IEnumerable<IArtist> Artists { get; set; } = Enumerable.Empty<IArtist>();
			public IEnumerable<IGenre> Genres { get; set; } = Enumerable.Empty<IGenre>();
			public IEnumerable<IPlaylist> Playlists { get; set; } = Enumerable.Empty<IPlaylist>();
			public IEnumerable<ISong> Songs { get; set; } = Enumerable.Empty<ISong>();

			public int? Index { get; set; }
			public string? QueueId { get; set; }

			public Action<IDialogInterface?>? DialogInterfaceListenerCancel { get; set; }
			public Action<IDialogInterface?>? DialogInterfaceListenerClose { get; set; }
			public Action<IDialogInterface?>? DialogInterfaceListenerConfirm { get; set; }
			public Action<IDialogInterface?>? DialogInterfaceListenerEdit { get; set; }
			public Action<IDialogInterface?>? DialogInterfaceListenerLyrics { get; set; }
			public Action<IDialogInterface?>? DialogInterfaceListenerOnSelect { get; set; }
			public Action<IDialogInterface?>? DialogInterfaceListenerSave { get; set; }
			public IDialogInterfaceOnDismissListener? DialogInterfaceListenerOnDismiss { get; set; }
		}
 
		private static Snackbar? CreateSnackbar(VariableContainer variables)
		{
			Snackbar? snackbar =
				variables.Context is null ||
				variables.SnackbarParent is null
					? null
					: XyzuUtils.Dialogs.SnackBar(variables.Context, variables.SnackbarParent, variables.SnackbarOnCreate);

			return snackbar;
		}	   		 
		private static Snackbar? CreateSnackbar(VariableContainer variables, int text)
		{
			Snackbar? snackbar = CreateSnackbar(variables);

			snackbar?.SetText(text);

			return snackbar;
		}	   		
		private static Snackbar? CreateSnackbar(VariableContainer variables, int count, int text)
		{
			Snackbar? snackbar = CreateSnackbar(variables);

			snackbar?.SetText(string.Format(
				"{0} {1}",
				count,
				variables.Context?.Resources?.GetString(text) ?? string.Empty));

			return snackbar;
		}	   
		private static Snackbar? CreateSnackbar(VariableContainer variables, string text)
		{
			Snackbar? snackbar = CreateSnackbar(variables);

			snackbar?.SetText(text);

			return snackbar;
		}

		private static IEnumerable<ISong> GetSongs(IEnumerable<IAlbum> albums)
		{
			return XyzuLibrary.Instance.Songs
				.GetSongs(
					identifiers: ILibrary.IIdentifiers.FromAlbums(albums),
					retriever: new ISong.Default<bool>(false)
					{
						Id = true
					});
		}	  
		private static IEnumerable<ISong> GetSongs(IEnumerable<IArtist> artists)
		{
			return XyzuLibrary.Instance.Songs
				.GetSongs(
					identifiers: ILibrary.IIdentifiers.FromArtists(artists),
					retriever: new ISong.Default<bool>(false)
					{
						Id = true
					});
		}	  
		private static IEnumerable<ISong> GetSongs(IEnumerable<IGenre> genres)
		{
			return XyzuLibrary.Instance.Songs
				.GetSongs(
					identifiers: ILibrary.IIdentifiers.FromGenres(genres),
					retriever: new ISong.Default<bool>(false)
					{
						Id = true
					});
		}

		private static Action<IDialogInterface?> CreateSheetDialogBottomViewAction(Action<IDialogInterface?>? dialoginterfaceaction)
		{
			return dialoginterfaceaction ?? (dialoginterface => dialoginterface?.Dismiss());
		}
		private static IDialogInterfaceOnClickListener CreateDialogInterfaceOnClickListener(Action<IDialogInterface?>? dialoginterfaceaction)
		{
			return new DialogInterfaceOnClickListener
			{
				OnClickAction = (dialoginterface, which) =>
				{
					if (dialoginterfaceaction is null)
						dialoginterface?.Dismiss();
					else if (dialoginterface != null)
						dialoginterfaceaction?.Invoke(dialoginterface);
				}
			};
		}			 
		private static IDialogInterfaceOnClickListener CreateDialogInterfaceOnClickListener(Action<IDialogInterface?>? dialoginterfaceaction, DialogInterfaceOnClickListener @default)
		{
			if (dialoginterfaceaction is null)
				return @default;

			return CreateDialogInterfaceOnClickListener(dialoginterfaceaction);
		}


		public static PopupMenu CreatePopupMenu(
			Context? context,
			View anchor,
			IEnumerable<MenuOptions>? menuoptions,
			PopupMenu.IOnMenuItemClickListener? menuitemclicklistener = null,
			PopupMenu.IOnDismissListener? ondismisslistener = null)
		{
			Context contextthemewrapper = new ContextThemeWrapper(context, Resource.Style.Xyzu_PopupMenu);
			PopupMenu popupmenu = new PopupMenu(contextthemewrapper, anchor, (int)GravityFlags.NoGravity);

			foreach (MenuOptions menuoption in menuoptions ?? Enumerable.Empty<MenuOptions>())
				popupmenu.Menu.AddMenuOption(menuoption, context, out IMenuItem? _);

			if (menuitemclicklistener != null) popupmenu.SetOnMenuItemClickListener(menuitemclicklistener);
			if (ondismisslistener != null) popupmenu.SetOnDismissListener(ondismisslistener);

			return popupmenu;
		}

		private static int DialogHeight(Context context)
		{
			return (int)((context.Resources?.DisplayMetrics?.HeightPixels ?? 0) * 0.70);
		}
	}
}