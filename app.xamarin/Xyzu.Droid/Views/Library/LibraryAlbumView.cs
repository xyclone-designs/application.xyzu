#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Menus;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Widgets.RecyclerViews;
using Xyzu.Views.LibraryItem;
using Xyzu.Views.LibraryItem.Header;

using ILibraryIdentifiers = Xyzu.Library.ILibrary.IIdentifiers;

namespace Xyzu.Views.Library
{
	public class LibraryAlbumView : LibraryView, ILibraryAlbum
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_album;

			public const int AlbumSongs_SongsRecyclerView = Resource.Id.xyzu_view_library_album_albumsongs_songsrecyclerview;
		}

		public LibraryAlbumView(Context context) : base(context) { }
		public LibraryAlbumView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.AlbumSong
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.AlbumSong.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.AlbumSong.GoToArtist => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.AlbumSong.GoToGenre => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.AlbumSong.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.AlbumSong.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.AlbumSong.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.AlbumSong.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.AlbumSong.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.AlbumSong.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			AlbumHeader.Images = Images;
			AlbumHeader.Library = Library;
			AlbumHeader.OnClickOptions = (sender, args) =>
			{
				CreateOptionsMenuAlertDialog((optionsmenuview, alertdialog) =>
				{
					optionsmenuview.Text.Text = Album?.Title;
					optionsmenuview.MenuOptions = Menus.LibraryItem.Album.AsEnumerable();
					optionsmenuview.OnMenuOptionClicked = menuoption =>
					{
						MenuOptionsUtils.VariableContainer menuvariables = new MenuOptionsUtils.VariableContainer
						{
							Albums = Albums,

							AnchorView = sender as View,
							AnchorViewGroup = sender as ViewGroup,
						};

						base.ProcessMenuVariables(menuvariables, null);

						switch (menuoption)
						{
							case Menus.LibraryItem.Album.AddToPlaylist:
								MenuOptionsUtils.AddToPlaylistAlbums(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Album.AddToQueue:
								alertdialog.Dismiss();
								MenuOptionsUtils.AddToQueueAlbums(menuvariables);
								return true;

							case Menus.LibraryItem.Album.Delete:
								MenuOptionsUtils.DeleteAlbums(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Album.EditInfo:
								MenuOptionsUtils.EditInfoAlbum(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Album.GoToAlbum:
								DismissMenuOptions();
								MenuOptionsUtils.GoToAlbum(menuvariables);
								return true;

							case Menus.LibraryItem.Album.GoToArtist:
								alertdialog.Dismiss();
								MenuOptionsUtils.GoToArtist(menuvariables);
								return true;

							case Menus.LibraryItem.Album.Play:
								alertdialog.Dismiss();
								MenuOptionsUtils.PlayAlbums(menuvariables, Album);
								return true;

							case Menus.LibraryItem.Album.Share:
								MenuOptionsUtils.Share(menuvariables);
								return true;

							case Menus.LibraryItem.Album.ViewInfo:
								MenuOptionsUtils.ViewInfoAlbum(menuvariables)?
									.Show();
								return true;

							default: return false;
						}
					};

				}).Show();
			};
			AlbumHeader.OnClickPlay = (sender, args) =>
			{
				ProcessMenuVariables(MenuVariables, null);

				if (AlbumSongs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
					MenuOptionsUtils.PlaySongs(MenuVariables, null);

				else MenuOptionsUtils.PlayAlbums(MenuVariables, Album);
			};

			AlbumHeader.AlbumSongs.LibraryItemsOptions.IsReversed = Settings.SongsIsReversed;
			AlbumHeader.AlbumSongs.LibraryItemsOptions.LayoutTypes = IAlbumSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered();
			AlbumHeader.AlbumSongs.LibraryItemsOptions.LayoutTypeSelected = Settings.SongsLayoutType;
			AlbumHeader.AlbumSongs.LibraryItemsOptions.SortKeys = IAlbumSettings.Options.SongsSortKeys.AsEnumerable().OrderBy(_ => _);
			AlbumHeader.AlbumSongs.LibraryItemsOptions.SortKeySelected = Settings.SongsSortKey;
			AlbumHeader.AlbumSongs.LibraryItemsOptions.OnOptionsIsReversedClicked = isreversed => Settings.SongsIsReversed = isreversed;
			AlbumHeader.AlbumSongs.LibraryItemsOptions.OnOptionsLayoutTypeItemSelected = layouttype => Settings.SongsLayoutType = layouttype;
			AlbumHeader.AlbumSongs.LibraryItemsOptions.OnOptionsSortKeyItemSelected = sortkey => Settings.SongsSortKey = sortkey;

			AlbumSongs.LibraryItemsItemDecoration.HeaderView = AlbumHeader;
			AlbumSongs.LibraryItemsItemDecoration.FooterView = InsetBottomView;

			AlbumSongs.LibraryItemsLayoutManager.LibraryLayoutType = Settings.SongsLayoutType;

			AlbumSongs.LibraryItemsAdapter.Images = Images;
			AlbumSongs.LibraryItemsAdapter.Library = Library;
			AlbumSongs.LibraryItemsAdapter.LibraryLayoutType = Settings.SongsLayoutType;
			AlbumSongs.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			AlbumSongs.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				viewholder.LibraryItem.SetSong(AlbumSongs.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Song(Context));
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
			};
			AlbumSongs.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, AlbumSongs.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						OnMenuOptionClick(MenuOptions.Play);
						Navigatable?.NavigateSong(AlbumSongs.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						AlbumSongs.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					AlbumSongs.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						AlbumSongs.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						AlbumSongs.LibraryItemsAdapter.SelectLibraryItemsFocused();
						MenuOptionsDialog.Show();
						break;

					default: break;
				}
			};
		}
		protected override void Init(Context context, IAttributeSet? attrs)
		{
			base.Init(context, attrs);

			Inflate(context, Ids.Layout, this);	
		}
		protected override void ProcessMenuVariables(MenuOptionsUtils.VariableContainer menuvariables, MenuOptions? menuoption)
		{
			base.ProcessMenuVariables(menuvariables, menuoption);

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(AlbumSongs.LibraryItemsAdapter);

			switch (AlbumSongs.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Songs = AlbumSongs.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = AlbumSongs.LibraryItemsAdapter.LibraryItems.Index(AlbumSongs.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Songs = AlbumSongs.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.PropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Album):
					SetAlbum(Album);
					break;

				default: break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(IAlbumSettings.SongsLayoutType):
					AlbumSongs.ReloadLayout(Settings.SongsLayoutType);
					break;
				case nameof(IAlbumSettings.SongsIsReversed):
					AlbumSongs.LibraryItemsAdapter.LibraryItems.Reverse();
					break;
				case nameof(IAlbumSettings.SongsSortKey):
					AlbumSongs.LibraryItemsAdapter.SetLibraryItems(AlbumSongs.LibraryItemsAdapter.LibraryItems.Sort(Settings.SongsSortKey, Settings.SongsIsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryAlbum(Settings)?
					.Apply();
		}

		private IAlbum? _Album;
		private IAlbumSettings? _Settings;
		private ILibraryIdentifiers? _Identifiers;
		private SongsRecyclerView? _AlbumSongs;

		protected override string? QueueId
		{
			get => MenuOptionsUtils.QueueIdAlbum(Album?.Id, Settings);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(AlbumSongs.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => AlbumSongs.LibraryItemsAdapter;
		}

		public IAlbum? Album
		{
			get => _Album;
			set
			{
				_Album = value;
				_Identifiers = null;

				PropertyChanged();
			}
		}
		public IEnumerable<IAlbum> Albums
		{
			get
			{
				IEnumerable<IAlbum> albums = Enumerable.Empty<IAlbum>();

				if (Album != null)
					albums = albums.Append(Album);

				return albums;
			}
		}
		public IAlbumSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryAlbum();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IAlbumSettings.Defaults.AlbumSettings;
			}
			set
			{
				if (_Settings != null)
					_Settings.PropertyChanged -= PropertyChangedSettings;

				_Settings = value;

				if (_Settings != null)
					_Settings.PropertyChanged += PropertyChangedSettings;
			}
		}
		public ILibraryIdentifiers Identifiers
		{
			set => _Identifiers = value;
			get => _Identifiers ??= new ILibraryIdentifiers.Default().WithAlbum(Album);
		}
		public HeaderAlbumView AlbumHeader
		{
			get => (HeaderAlbumView)(AlbumSongs.LibraryItemsAdapter.Header ??= new HeaderAlbumView(Context!)
			{
				Images = Images
			});
		}
		public SongsRecyclerView AlbumSongs 
		{
			get => _AlbumSongs ??= FindViewById(Ids.AlbumSongs_SongsRecyclerView) as SongsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || Album is null || (force is false && AlbumSongs.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			if (force)
				AlbumHeader.Album = null;

			Album = await Library.Albums.PopulateAlbum(Album, Cancellationtoken);

			IAsyncEnumerable<ISong> libraryitems = Library.Songs.GetSongs(
				identifiers: Identifiers,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GenerateSongRetriever(
					songs: AlbumSongs.LibraryItemsAdapter.LibraryItems,
					modelsortkey: Settings.SongsSortKey,
					librarylayouttype: Settings.SongsLayoutType))
				.Sort(Settings.SongsSortKey, Settings.SongsIsReversed);

			await SetAlbum(Album, libraryitems);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && AlbumSongs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				AlbumSongs.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.AlbumSong.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.AlbumSong.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueSongs(MenuVariables);
					return true;

				case Menus.LibraryItem.AlbumSong.Delete:
					MenuOptionsUtils.DeleteSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.AlbumSong.EditInfo:
					MenuOptionsUtils.EditInfoSong(MenuVariables)?
						.Show();
					return true;			  

				case Menus.LibraryItem.AlbumSong.GoToArtist:
					DismissMenuOptions();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;

				case Menus.LibraryItem.AlbumSong.GoToGenre:
					DismissMenuOptions();
					MenuOptionsUtils.GoToGenre(MenuVariables);
					return true;										  

				case Menus.LibraryItem.AlbumSong.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlaySongs(MenuVariables, null);
					return true;

				case Menus.LibraryItem.AlbumSong.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.AlbumSong.ViewInfo:
					MenuOptionsUtils.ViewInfoSong(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			AlbumSongs.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}

		public void SetAlbum(IAlbum? album)
		{
			if (album is null)
				AlbumHeader.Album = null;
			else AlbumHeader.Album ??= album;

			AlbumHeader.AlbumSongs.Text.SetCompoundDrawablesRelativeWithIntrinsicBounds(Context?.Resources?.GetDrawable(Resource.Drawable.icon_library_songs, Context.Theme), null, null, null);
			AlbumHeader.AlbumSongs.Text.Text = string.Format("{0} ({1})", Context?.Resources?.GetString(Resource.String.library_songs) ?? string.Empty, AlbumSongs.LibraryItemsAdapter.LibraryItems.Count);
		}
		public void SetAlbum(IAlbum? album, IEnumerable<ISong>? albumsongs)
		{
			AlbumSongs.LibraryItemsAdapter.LibraryItems.Clear();
			AlbumSongs.LibraryItemsAdapter.LibraryItems.AddRange(albumsongs);

			SetAlbum(album);
		}
		public async Task SetAlbum(IAlbum? album, IAsyncEnumerable<ISong>? albumsongs)
		{
			AlbumSongs.LibraryItemsAdapter.LibraryItems.Clear();

			await AlbumSongs.LibraryItemsAdapter.LibraryItems.AddRange(albumsongs, Cancellationtoken);

			SetAlbum(album);
		}
	}
}