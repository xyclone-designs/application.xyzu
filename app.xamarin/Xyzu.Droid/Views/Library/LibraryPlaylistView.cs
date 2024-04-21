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
	public partial class LibraryPlaylistView : LibraryView, ILibraryPlaylist
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_playlist;

			public const int PlaylistSongs_SongsRecyclerView = Resource.Id.xyzu_view_library_playlist_playlistsongs_songsrecyclerview;
		}

		public LibraryPlaylistView(Context context) : base(context) { }
		public LibraryPlaylistView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.PlaylistSong
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.PlaylistSong.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.PlaylistSong.GoToAlbum => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.PlaylistSong.GoToArtist => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.PlaylistSong.GoToGenre => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.PlaylistSong.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.PlaylistSong.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.PlaylistSong.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.PlaylistSong.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.PlaylistSong.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			PlaylistHeader.Images = Images;
			PlaylistHeader.Library = Library;
			PlaylistHeader.OnClickOptions = (sender, args) =>
			{
				CreateOptionsMenuAlertDialog((optionsmenuview, alertdialog) =>
				{
					optionsmenuview.Text.Text = Playlist?.Name;
					optionsmenuview.MenuOptions = Menus.LibraryItem.Playlist.AsEnumerable();
					optionsmenuview.OnMenuOptionClicked = menuoption =>
					{
						MenuOptionsUtils.VariableContainer menuvariables = new MenuOptionsUtils.VariableContainer
						{
							Playlists = Playlists,

							AnchorView = sender as View,
							AnchorViewGroup = sender as ViewGroup,
						};

						base.ProcessMenuVariables(menuvariables, null);

						switch (menuoption)
						{
							case Menus.LibraryItem.Playlist.AddToQueue:
								alertdialog.Dismiss();
								MenuOptionsUtils.AddToQueuePlaylists(menuvariables);
								return true;

							case Menus.LibraryItem.Playlist.Delete:
								MenuOptionsUtils.DeletePlaylists(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Playlist.EditInfo:
								MenuOptionsUtils.EditInfoPlaylist(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Playlist.GoToPlaylist:
								alertdialog.Dismiss();
								MenuOptionsUtils.GoToPlaylist(menuvariables);
								return true;

							case Menus.LibraryItem.Playlist.Play:
								alertdialog.Dismiss();
								MenuOptionsUtils.PlayPlaylists(menuvariables, Playlist);
								return true;

							case Menus.LibraryItem.Playlist.Share:
								MenuOptionsUtils.Share(menuvariables);
								return true;

							case Menus.LibraryItem.Playlist.ViewInfo:
								MenuOptionsUtils.ViewInfoPlaylist(menuvariables)?
									.Show();
								return true;

							default: return false;
						}
					};

				}).Show();
			};
			PlaylistHeader.OnClickPlay = (sender, args) =>
			{
				ProcessMenuVariables(MenuVariables, null);

				if (PlaylistSongs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
					MenuOptionsUtils.PlaySongs(MenuVariables, null);

				else MenuOptionsUtils.PlayPlaylists(MenuVariables, Playlist);
			};

			PlaylistHeader.PlaylistSongs.LibraryItemsOptions.IsReversed = Settings.SongsIsReversed;
			PlaylistHeader.PlaylistSongs.LibraryItemsOptions.LayoutTypes = IPlaylistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered();
			PlaylistHeader.PlaylistSongs.LibraryItemsOptions.LayoutTypeSelected = Settings.SongsLayoutType;
			PlaylistHeader.PlaylistSongs.LibraryItemsOptions.SortKeys = IPlaylistSettings.Options.SongsSortKeys.AsEnumerable().OrderBy(_ => _);
			PlaylistHeader.PlaylistSongs.LibraryItemsOptions.SortKeySelected = Settings.SongsSortKey;
			PlaylistHeader.PlaylistSongs.LibraryItemsOptions.OnOptionsIsReversedClicked = isreversed => Settings.SongsIsReversed = isreversed;
			PlaylistHeader.PlaylistSongs.LibraryItemsOptions.OnOptionsLayoutTypeItemSelected = layouttype => Settings.SongsLayoutType = layouttype;
			PlaylistHeader.PlaylistSongs.LibraryItemsOptions.OnOptionsSortKeyItemSelected = sortkey => Settings.SongsSortKey = sortkey;

			PlaylistSongs.LibraryItemsItemDecoration.HeaderView = PlaylistHeader;
			PlaylistSongs.LibraryItemsItemDecoration.FooterView = InsetBottomView;

			PlaylistSongs.LibraryItemsLayoutManager.LibraryLayoutType = Settings.SongsLayoutType;

			PlaylistSongs.LibraryItemsAdapter.Images = Images;
			PlaylistSongs.LibraryItemsAdapter.Library = Library;
			PlaylistSongs.LibraryItemsAdapter.LibraryLayoutType = Settings.SongsLayoutType;
			PlaylistSongs.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			PlaylistSongs.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				viewholder.LibraryItem.SetSong(PlaylistSongs.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Song(Context));
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
			};
			PlaylistSongs.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, PlaylistSongs.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						OnMenuOptionClick(MenuOptions.Play);
						Navigatable?.NavigateSong(PlaylistSongs.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						PlaylistSongs.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					PlaylistSongs.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						PlaylistSongs.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						PlaylistSongs.LibraryItemsAdapter.SelectLibraryItemsFocused();
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

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(PlaylistSongs.LibraryItemsAdapter);

			switch (PlaylistSongs.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Songs = PlaylistSongs.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = PlaylistSongs.LibraryItemsAdapter.LibraryItems.Index(PlaylistSongs.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Songs = PlaylistSongs.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.PropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Playlist):
					SetPlaylist(Playlist);
					break;

				default: break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(IPlaylistSettings.SongsLayoutType):
					PlaylistSongs.ReloadLayout(Settings.SongsLayoutType);
					break;
				case nameof(IPlaylistSettings.SongsIsReversed):
					PlaylistSongs.LibraryItemsAdapter.LibraryItems.Reverse();
					break;
				case nameof(IPlaylistSettings.SongsSortKey):
					PlaylistSongs.LibraryItemsAdapter.SetLibraryItems(PlaylistSongs.LibraryItemsAdapter.LibraryItems.Sort(Settings.SongsSortKey, Settings.SongsIsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryPlaylist(Settings)?
					.Apply();
		}

		private IPlaylist? _Playlist;
		private IPlaylistSettings? _Settings;
		private ILibraryIdentifiers? _Identifiers;
		private SongsRecyclerView? _PlaylistSongs;

		protected override string? QueueId
		{
			get => MenuOptionsUtils.QueueIdPlaylist(Playlist?.Id, Settings);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(PlaylistSongs.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => PlaylistSongs.LibraryItemsAdapter;
		}

		public IPlaylist? Playlist
		{
			get => _Playlist;
			set
			{
				_Playlist = value;
				_Identifiers = null;

				PropertyChanged();
			}
		}
		public IEnumerable<IPlaylist> Playlists
		{
			get
			{
				IEnumerable<IPlaylist> playlists = Enumerable.Empty<IPlaylist>();

				if (Playlist != null)
					playlists = playlists.Append(Playlist);

				return playlists;
			}
		}
		public IPlaylistSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryPlaylist();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IPlaylistSettings.Defaults.PlaylistSettings;
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
			get => _Identifiers ??= ILibraryIdentifiers.FromPlaylist(Playlist);
		}
		public HeaderPlaylistView PlaylistHeader
		{
			get => (HeaderPlaylistView)(PlaylistSongs.LibraryItemsAdapter.Header ??= new HeaderPlaylistView(Context!)
			{
				Images = Images
			});
		}
		public SongsRecyclerView PlaylistSongs
		{
			get => _PlaylistSongs ??= FindViewById(Ids.PlaylistSongs_SongsRecyclerView) as SongsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || Playlist is null || (force is false && PlaylistSongs.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			if (force)
				PlaylistHeader.Playlist = null;

			await Library.Playlists.PopulatePlaylist(Playlist, Cancellationtoken);

			IAsyncEnumerable<ISong> playlistsongs = Library.Songs.GetSongs(
				identifiers: Identifiers,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GenerateSongRetriever(
					songs: PlaylistSongs.LibraryItemsAdapter.LibraryItems,
					modelsortkey: Settings.SongsSortKey,
					librarylayouttype: Settings.SongsLayoutType))
				.Sort(Settings.SongsSortKey, Settings.SongsIsReversed);

			await SetPlaylist(Playlist, playlistsongs);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && PlaylistSongs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				PlaylistSongs.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.PlaylistSong.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueSongs(MenuVariables);
					return true;

				case Menus.LibraryItem.PlaylistSong.Delete:
					MenuOptionsUtils.DeleteSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.PlaylistSong.EditInfo:
					MenuOptionsUtils.EditInfoSong(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.PlaylistSong.GoToAlbum:
					DismissMenuOptions();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;

				case Menus.LibraryItem.PlaylistSong.GoToArtist:
					DismissMenuOptions();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;

				case Menus.LibraryItem.PlaylistSong.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlaySongs(MenuVariables, null);
					return true;

				case Menus.LibraryItem.PlaylistSong.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.PlaylistSong.ViewInfo:
					MenuOptionsUtils.ViewInfoSong(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			PlaylistSongs.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}

		public void SetPlaylist(IPlaylist? playlist)
		{
			if (playlist is null)
				PlaylistHeader.Playlist = null;
			else PlaylistHeader.Playlist ??= playlist;

			PlaylistHeader.PlaylistSongs.Text.SetCompoundDrawablesRelativeWithIntrinsicBounds(Context?.Resources?.GetDrawable(Resource.Drawable.icon_library_songs, Context.Theme), null, null, null);
			PlaylistHeader.PlaylistSongs.Text.Text = string.Format("{0} ({1})", Context?.Resources?.GetString(Resource.String.library_songs) ?? string.Empty, PlaylistSongs.LibraryItemsAdapter.LibraryItems.Count);
		}
		public void SetPlaylist(IPlaylist? playlist, IEnumerable<ISong>? playlistsongs)
		{
			PlaylistSongs.LibraryItemsAdapter.LibraryItems.Clear();
			PlaylistSongs.LibraryItemsAdapter.LibraryItems.AddRange(playlistsongs);

			SetPlaylist(playlist);
		}
		public async Task SetPlaylist(IPlaylist? playlist, IAsyncEnumerable<ISong>? playlistsongs)
		{
			PlaylistSongs.LibraryItemsAdapter.LibraryItems.Clear();

			await PlaylistSongs.LibraryItemsAdapter.LibraryItems.AddRange(playlistsongs, Cancellationtoken);

			SetPlaylist(playlist);
		}
	}
}