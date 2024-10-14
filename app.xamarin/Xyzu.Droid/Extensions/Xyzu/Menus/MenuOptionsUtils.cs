using Android.Content;
using Android.Content.Res;
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

using AndroidWidgetFrameLayout = Android.Widget.FrameLayout;
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
				get => Albums.Count() == 1 ? Albums.First() : null;
			} 
			public IArtist? Artist 
			{ 
				get => Artists.Count() == 1 ? Artists.First() : null;
			} 
			public IGenre? Genre 
			{ 
				get => Genres.Count() == 1 ? Genres.First() : null;
			} 
			public IPlaylist? Playlist 
			{ 
				get => Playlists.Count() == 1 ? Playlists.First() : null;
			} 
			public ISong? Song 
			{ 
				get => Songs.Count() == 1 ? Songs.First() : null;
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
			public IDialogInterfaceOnShowListener? DialogInterfaceListenerOnShow { get; set; }
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
				.GetSongs(ILibrary.IIdentifiers.FromAlbums(albums));
		}	  
		private static IEnumerable<ISong> GetSongs(IEnumerable<IArtist> artists)
		{
			return XyzuLibrary.Instance.Songs
				.GetSongs(ILibrary.IIdentifiers.FromArtists(artists));
		}	  
		private static IEnumerable<ISong> GetSongs(IEnumerable<IGenre> genres)
		{
			return XyzuLibrary.Instance.Songs
				.GetSongs(ILibrary.IIdentifiers.FromGenres(genres));
		}

		private static Action<IDialogInterface?> CreateSheetDialogBottomViewAction(Action<IDialogInterface?>? dialoginterfaceaction)
		{
			return dialoginterfaceaction ?? (dialoginterface => dialoginterface?.Dismiss());
		}
		private static DialogInterfaceOnClickListener CreateDialogInterfaceOnClickListener(Action<IDialogInterface?>? dialoginterfaceaction)
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
		private static DialogInterfaceOnClickListener CreateDialogInterfaceOnClickListener(Action<IDialogInterface?>? dialoginterfaceaction, DialogInterfaceOnClickListener @default)
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
			ContextThemeWrapper contextthemewrapper = new (context, Resource.Style.Xyzu_PopupMenu);
			PopupMenu popupmenu = new (contextthemewrapper, anchor, (int)GravityFlags.NoGravity);

			foreach (MenuOptions menuoption in menuoptions ?? Enumerable.Empty<MenuOptions>())
				popupmenu.Menu.AddMenuOption(menuoption, context, out IMenuItem? _);

			if (menuitemclicklistener != null) popupmenu.SetOnMenuItemClickListener(menuitemclicklistener);
			if (ondismisslistener != null) popupmenu.SetOnDismissListener(ondismisslistener);

			return popupmenu;
		}

		public static int DialogWidth(Context context)
		{
			if (context.Resources?.Configuration?.Orientation is Orientation.Landscape)
				return ViewGroup.LayoutParams.WrapContent;
			else return ViewGroup.LayoutParams.MatchParent;
		}
		public static int DialogHeight(Context _)
		{
			return ViewGroup.LayoutParams.WrapContent;
		}
		public static int DialogMaxWidth(Context context)
		{
			if (context.Resources?.Configuration?.Orientation is Orientation.Landscape)
				return (int)((context.Resources?.DisplayMetrics?.WidthPixels ?? 0) * 0.60);
			else return context.Resources?.DisplayMetrics?.WidthPixels ?? 0;
		}
		public static int DialogMaxHeight(Context context)
		{
			if (context.Resources?.Configuration?.Orientation is Orientation.Portrait)
				return (int)((context.Resources?.DisplayMetrics?.HeightPixels ?? 0) * 0.70);
			else return context.Resources?.DisplayMetrics?.HeightPixels ?? 0;
		}
		public static GravityFlags DialogGravityFlags(Context context)
		{
			if (context.Resources?.Configuration?.Orientation is Orientation.Landscape)
				return GravityFlags.End | GravityFlags.Bottom;
			else return GravityFlags.CenterVertical | GravityFlags.Bottom;
		}
		public static AndroidWidgetFrameLayout.LayoutParams DialogLayoutParams(Context context)
		{
			return new AndroidWidgetFrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
			{
				Gravity = DialogGravityFlags(context)
			};
		}
	}
}