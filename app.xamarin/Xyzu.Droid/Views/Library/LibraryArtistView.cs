#nullable enable

using Android.Content;
using Android.Graphics.Drawables;
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
using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Widgets.RecyclerViews;
using Xyzu.Views.LibraryItem;
using Xyzu.Views.LibraryItem.Header;

using ILibraryIdentifiers = Xyzu.Library.ILibrary.IIdentifiers;

namespace Xyzu.Views.Library
{
	public partial class LibraryArtistView : LibraryView, ILibraryArtist
	{
		enum ArtistItemType
		{
			Albums,
			Songs
		}

		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_artist;

			public const int ArtistItems_ModelsRecyclerView = Resource.Id.xyzu_view_library_artist_artistitems_modelsrecyclerview;
		}

		public LibraryArtistView(Context context) : base(context) { }
		public LibraryArtistView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get
			{
				switch (Showing)
				{
					case ArtistItemType.Albums:
						return Menus.LibraryItem.ArtistAlbum
							.AsEnumerable()
							.ToDictionary<MenuOptions, MenuOptions, int[]?>(
								keySelector: menuoption => menuoption,
								elementSelector: menuoption => menuoption switch
								{
									Menus.LibraryItem.ArtistAlbum.EditInfo => MenuOptionEnabledArray_Single,
									Menus.LibraryItem.ArtistAlbum.GoToAlbum => MenuOptionEnabledArray_Single,
									Menus.LibraryItem.ArtistAlbum.ViewInfo => MenuOptionEnabledArray_Single,

									Menus.LibraryItem.ArtistAlbum.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
									Menus.LibraryItem.ArtistAlbum.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
									Menus.LibraryItem.ArtistAlbum.Delete => MenuOptionEnabledArray_SingleMultiple,
									Menus.LibraryItem.ArtistAlbum.Play => MenuOptionEnabledArray_SingleMultiple,
									Menus.LibraryItem.ArtistAlbum.Share => MenuOptionEnabledArray_SingleMultiple,

									_ => MenuOptionEnabledArray_All,
								});

					case ArtistItemType.Songs:
						return Menus.LibraryItem.ArtistSong
							.AsEnumerable()
							.ToDictionary<MenuOptions, MenuOptions, int[]?>(
								keySelector: menuoption => menuoption,
								elementSelector: menuoption => menuoption switch
								{
									Menus.LibraryItem.ArtistSong.EditInfo => MenuOptionEnabledArray_Single,
									Menus.LibraryItem.ArtistSong.GoToAlbum => MenuOptionEnabledArray_Single,
									Menus.LibraryItem.ArtistSong.GoToGenre => MenuOptionEnabledArray_Single,
									Menus.LibraryItem.ArtistSong.ViewInfo => MenuOptionEnabledArray_Single,

									Menus.LibraryItem.ArtistSong.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
									Menus.LibraryItem.ArtistSong.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
									Menus.LibraryItem.ArtistSong.Delete => MenuOptionEnabledArray_SingleMultiple,
									Menus.LibraryItem.ArtistSong.Play => MenuOptionEnabledArray_SingleMultiple,
									Menus.LibraryItem.ArtistSong.Share => MenuOptionEnabledArray_SingleMultiple,

									_ => MenuOptionEnabledArray_All,
								});

					default: throw new ArgumentException();
				}
			}
		}
		protected override void Configure()
		{
			base.Configure();

			ArtistHeader.Images = Images;
			ArtistHeader.Library = Library;
			ArtistHeader.ArtistItems.TextClick = async (sender, args) =>
			{
				Showing = Showing == ArtistItemType.Albums ? ArtistItemType.Songs : ArtistItemType.Albums;

				ArtistItems.LibraryItemsAdapter.LibraryItems.Clear();
				ArtistItems.LibraryItemsLayoutManager.RemoveAllViews();
				ArtistItems.LibraryItemsLayoutManager.RequestLayout();

				await OnRefresh();
			};
			ArtistHeader.OnClickOptions = (sender, args) =>
			{
				CreateOptionsMenuAlertDialog((optionsmenuview, alertdialog) =>
				{
					optionsmenuview.Text.Text = Artist?.Name;
					optionsmenuview.MenuOptions = Menus.LibraryItem.Artist.AsEnumerable();
					optionsmenuview.OnMenuOptionClicked = menuoption =>
					{
						MenuOptionsUtils.VariableContainer menuvariables = new MenuOptionsUtils.VariableContainer
						{
							Artists = Artists,

							AnchorView = sender as View,
							AnchorViewGroup = sender as ViewGroup,
						};

						base.ProcessMenuVariables(menuvariables, null);

						switch (menuoption)
						{
							case Menus.LibraryItem.Artist.AddToPlaylist:
								MenuOptionsUtils.AddToPlaylistArtists(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Artist.AddToQueue:
								alertdialog.Dismiss();
								MenuOptionsUtils.AddToQueueArtists(menuvariables);
								return true;

							case Menus.LibraryItem.Artist.Delete:
								MenuOptionsUtils.DeleteArtists(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Artist.EditInfo:
								MenuOptionsUtils.EditInfoArtist(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Artist.GoToArtist:
								alertdialog.Dismiss();
								MenuOptionsUtils.GoToArtist(menuvariables);
								return true;

							case Menus.LibraryItem.Artist.Play:
								alertdialog.Dismiss();
								MenuOptionsUtils.PlayArtists(menuvariables, Artist);
								return true;

							case Menus.LibraryItem.Artist.Share:
								MenuOptionsUtils.Share(menuvariables);
								return true;

							case Menus.LibraryItem.Artist.ViewInfo:
								MenuOptionsUtils.ViewInfoArtist(menuvariables)?
									.Show();
								return true;

							default: return false;
						}
					};

				}).Show();
			};

			ArtistItems.LibraryItemsItemDecoration.HeaderView = ArtistHeader;
			ArtistItems.LibraryItemsItemDecoration.FooterView = InsetBottomView;

			ArtistItems.LibraryItemsAdapter.Images = Images;
			ArtistItems.LibraryItemsAdapter.Library = Library;
			ArtistItems.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			ArtistItems.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (Showing, viewholdereventargs.Gesture, ArtistItems.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (ArtistItemType.Albums, Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (ArtistItemType.Albums, Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						Navigatable?.NavigateAlbum(ArtistItems.LibraryItemsAdapter.FocusedLibraryItem as IAlbum);
						break;

					case (ArtistItemType.Songs, Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (ArtistItemType.Songs, Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						Navigatable?.NavigateSong(ArtistItems.LibraryItemsAdapter.FocusedLibraryItem as ISong);
						break;

					case (_, Gestures.Click, RecyclerViewAdapterStates.Select):
					case (_, Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						ArtistItems.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (_, Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					ArtistItems.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						ArtistItems.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (_, Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						ArtistItems.LibraryItemsAdapter.SelectLibraryItemsFocused();
						MenuOptionsDialog.Show();
						break;

					default: break;
				}
			};

			ArtistItems.LibraryItemsLayoutManager.SpanCount = 12;
			ArtistItems.LibraryItemsLayoutManager.SetSpanSizeLookup(new LibraryItemsRecyclerView.Adapter.NewSpanSizeLookup
			{
				GetSpanSizeAction = position => ArtistItems.LibraryItemsAdapter.GetItemViewType(position) switch
				{
					LibraryItemsRecyclerView.Adapter.ItemViewType_Header => ArtistItems.LibraryItemsLayoutManager.SpanCount,
					LibraryItemsRecyclerView.Adapter.ItemViewType_Footer => ArtistItems.LibraryItemsLayoutManager.SpanCount,

					_ => (Showing, Settings.AlbumsLayoutType, Settings.SongsLayoutType) switch
					{
						(ArtistItemType.Albums, LibraryLayoutTypes.GridSmall, _) => ArtistItems.LibraryItemsLayoutManager.SpanCount / 4,
						(ArtistItemType.Albums, LibraryLayoutTypes.GridMedium, _) => ArtistItems.LibraryItemsLayoutManager.SpanCount / 3,
						(ArtistItemType.Albums, LibraryLayoutTypes.GridLarge, _) => ArtistItems.LibraryItemsLayoutManager.SpanCount / 2,

						(ArtistItemType.Songs, _, LibraryLayoutTypes.GridSmall) => ArtistItems.LibraryItemsLayoutManager.SpanCount / 4,
						(ArtistItemType.Songs, _, LibraryLayoutTypes.GridMedium) => ArtistItems.LibraryItemsLayoutManager.SpanCount / 3,
						(ArtistItemType.Songs, _, LibraryLayoutTypes.GridLarge) => ArtistItems.LibraryItemsLayoutManager.SpanCount / 2,

						_ => ArtistItems.LibraryItemsLayoutManager.SpanCount,
					}
				}
			});

			PropertyChanged(nameof(Showing));
		}
		protected override void Init(Context context, IAttributeSet? attrs)
		{
			base.Init(context, attrs);

			Inflate(context, Ids.Layout, this);
		}
		protected override void ProcessMenuVariables(MenuOptionsUtils.VariableContainer menuvariables, MenuOptions? menuoption)
		{
			base.ProcessMenuVariables(menuvariables, menuoption);

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(ArtistItems.LibraryItemsAdapter);

			switch (Showing, ArtistItems.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case (ArtistItemType.Albums, RecyclerViewAdapterStates.Select):
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Songs = Enumerable.Empty<ISong>();
					MenuVariables.Albums = ArtistItems.LibraryItemsAdapter.SelectedLibraryItems.OfType<IAlbum>();
					break;
				case (ArtistItemType.Songs, RecyclerViewAdapterStates.Select):
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Albums = Enumerable.Empty<IAlbum>();
					MenuVariables.Songs = ArtistItems.LibraryItemsAdapter.SelectedLibraryItems.OfType<ISong>();
					break;		

				case (ArtistItemType.Albums, _):
					MenuVariables.Index = ArtistItems.LibraryItemsAdapter.LibraryItems.Index(ArtistItems.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Songs = Enumerable.Empty<ISong>();
					MenuVariables.Albums = ArtistItems.LibraryItemsAdapter.LibraryItems.OfType<IAlbum>();
					break;
				case (ArtistItemType.Songs, _):
					MenuVariables.Index = ArtistItems.LibraryItemsAdapter.LibraryItems.Index(ArtistItems.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Albums = Enumerable.Empty<IAlbum>();
					MenuVariables.Songs = ArtistItems.LibraryItemsAdapter.LibraryItems.OfType<ISong>();
					break;
			}			
		}
		protected override void PropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.PropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Artist):
					SetArtist(Artist);
					break;

				case nameof(Showing) when Showing == ArtistItemType.Albums:
					ArtistHeader.ArtistItems.LibraryItemsOptions.IsReversed = Settings.AlbumsIsReversed;
					ArtistHeader.ArtistItems.LibraryItemsOptions.LayoutTypeSelected = Settings.AlbumsLayoutType;
					ArtistHeader.ArtistItems.LibraryItemsOptions.LayoutTypes = IArtistSettings.Options.AlbumsLayoutTypes.AsEnumerable().NeatlyOrdered();
					ArtistHeader.ArtistItems.LibraryItemsOptions.SortKeySelected = Settings.AlbumsSortKey;
					ArtistHeader.ArtistItems.LibraryItemsOptions.SortKeys = IArtistSettings.Options.AlbumsSortKeys.AsEnumerable().OrderBy(_ => _);
					ArtistHeader.ArtistItems.LibraryItemsOptions.OnOptionsIsReversedClicked = isreversed => Settings.AlbumsIsReversed = isreversed;
					ArtistHeader.ArtistItems.LibraryItemsOptions.OnOptionsLayoutTypeItemSelected = layouttype => Settings.AlbumsLayoutType = layouttype;
					ArtistHeader.ArtistItems.LibraryItemsOptions.OnOptionsSortKeyItemSelected = sortkey => Settings.AlbumsSortKey = sortkey;
					ArtistHeader.ArtistItems.PlayClick = (sender, args) =>
					{
						ProcessMenuVariables(MenuVariables, null);

						if (ArtistItems.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
							MenuOptionsUtils.PlayAlbums(MenuVariables, null);

						else MenuOptionsUtils.PlayArtists(MenuVariables, Artist);
					};

					ArtistItems.LibraryItemsLayoutManager.LibraryLayoutType = Settings.AlbumsLayoutType;

					ArtistItems.LibraryItemsAdapter.LibraryLayoutType = Settings.AlbumsLayoutType;
					ArtistItems.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
					{
						viewholder.LibraryItem.SetAlbum(ArtistItems.LibraryItemsAdapter.LibraryItems[position] as IAlbum, LibraryItemView.Defaults.Album(Context));
						viewholder.LibraryItem.SetPlaying(Player, QueueId);
					};
					break;
				case nameof(Showing) when Showing == ArtistItemType.Songs:
					ArtistHeader.ArtistItems.LibraryItemsOptions.IsReversed = Settings.SongsIsReversed;
					ArtistHeader.ArtistItems.LibraryItemsOptions.LayoutTypeSelected = Settings.SongsLayoutType;
					ArtistHeader.ArtistItems.LibraryItemsOptions.LayoutTypes = IArtistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered();
					ArtistHeader.ArtistItems.LibraryItemsOptions.SortKeySelected = Settings.SongsSortKey;
					ArtistHeader.ArtistItems.LibraryItemsOptions.SortKeys = IArtistSettings.Options.SongsSortKeys.AsEnumerable().OrderBy(_ => _);
					ArtistHeader.ArtistItems.LibraryItemsOptions.OnOptionsIsReversedClicked = isreversed => Settings.SongsIsReversed = isreversed;
					ArtistHeader.ArtistItems.LibraryItemsOptions.OnOptionsLayoutTypeItemSelected = layouttype => Settings.SongsLayoutType = layouttype;
					ArtistHeader.ArtistItems.LibraryItemsOptions.OnOptionsSortKeyItemSelected = sortkey => Settings.SongsSortKey = sortkey;
					ArtistHeader.ArtistItems.PlayClick = (sender, args) =>
					{
						ProcessMenuVariables(MenuVariables, null);

						if (ArtistItems.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
							MenuOptionsUtils.PlaySongs(MenuVariables, null);

						else MenuOptionsUtils.PlayArtists(MenuVariables, Artist);
					};

					ArtistItems.LibraryItemsLayoutManager.LibraryLayoutType = Settings.SongsLayoutType;

					ArtistItems.LibraryItemsAdapter.LibraryLayoutType = Settings.SongsLayoutType;
					ArtistItems.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
					{
						viewholder.LibraryItem.SetSong(ArtistItems.LibraryItemsAdapter.LibraryItems[position] as ISong, LibraryItemView.Defaults.Song(Context));
						viewholder.LibraryItem.SetPlaying(Player, QueueId);
					};
					break;

				default: break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(IArtistSettings.AlbumsLayoutType) when Showing == ArtistItemType.Albums:
					ArtistItems.ReloadLayout(Settings.AlbumsLayoutType);
					break;
				case nameof(IArtistSettings.SongsLayoutType) when Showing == ArtistItemType.Songs:
					ArtistItems.ReloadLayout(Settings.SongsLayoutType);
					break;

				case nameof(IArtistSettings.AlbumsIsReversed) when Showing == ArtistItemType.Albums:
				case nameof(IArtistSettings.SongsIsReversed) when Showing == ArtistItemType.Songs:
					ArtistItems.LibraryItemsAdapter.LibraryItems.Reverse();
					break;

				case nameof(IArtistSettings.AlbumsSortKey):
					ArtistItems.LibraryItemsAdapter.SetLibraryItems(ArtistItems.LibraryItemsAdapter.LibraryItems.OfType<IAlbum>().Sort(Settings.AlbumsSortKey, Settings.AlbumsIsReversed));
					break;
				case nameof(IArtistSettings.SongsSortKey):
					ArtistItems.LibraryItemsAdapter.SetLibraryItems(ArtistItems.LibraryItemsAdapter.LibraryItems.OfType<ISong>().Sort(Settings.SongsSortKey, Settings.SongsIsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryArtist(Settings)?
					.Apply();
		}

		private IArtist? _Artist;
		private ArtistItemType _Showing;
		private IArtistSettings? _Settings;
		private ILibraryIdentifiers? _Identifiers;
		private ModelsRecyclerView? _ArtistItems;

		protected override string? QueueId
		{
			get => Showing switch
			{
				ArtistItemType.Albums => MenuOptionsUtils.QueueIdArtistAlbums(Artist?.Id, Settings),
				ArtistItemType.Songs => MenuOptionsUtils.QueueIdArtistSongs(Artist?.Id, Settings),

				_ => throw new ArgumentException()
			};
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(ArtistItems.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => ArtistItems.LibraryItemsAdapter;
		}

		public IArtist? Artist
		{
			get => _Artist;
			set
			{
				_Artist = value;
				_Identifiers = null;

				PropertyChanged();
			}
		}
		public IEnumerable<IArtist> Artists
		{
			get
			{
				IEnumerable<IArtist> artists = Enumerable.Empty<IArtist>();

				if (Artist != null)
					artists = artists.Append(Artist);

				return artists;
			}
		}
		private ArtistItemType Showing
		{
			get => _Showing;
			set
			{
				if (_Showing == value)
					return;

				_Showing = value;

				PropertyChanged();
			}
		}
		public IArtistSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryArtist();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IArtistSettings.Defaults.ArtistSettings;
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
			get => _Identifiers ??= new ILibraryIdentifiers.Default().WithArtist(Artist);
		}
		public HeaderArtistView ArtistHeader
		{
			get => (HeaderArtistView)(ArtistItems.LibraryItemsAdapter.Header ??= new HeaderArtistView(Context!)
			{
				Images = Images
			});
		}
		public ModelsRecyclerView ArtistItems
		{
			get => _ArtistItems ??= FindViewById(Ids.ArtistItems_ModelsRecyclerView) as ModelsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || Artist is null || (force is false && ArtistItems.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			if (force)
				ArtistHeader.Artist = null;

			await Library.Artists.PopulateArtist(Artist, Cancellationtoken);

			(IAsyncEnumerable<IAlbum>? albums, IAsyncEnumerable<ISong>? songs) = Showing switch
			{
				ArtistItemType.Albums => (Library.Albums
					.GetAlbums(
						identifiers: Identifiers,
						cancellationToken: Cancellationtoken,
						retriever: LibraryItemView.Retrievers.GenerateAlbumRetriever(
							modelsortkey: Settings.AlbumsSortKey,
							librarylayouttype: Settings.AlbumsLayoutType,
							albums: ArtistItems.LibraryItemsAdapter.LibraryItems.OfType<IAlbum>()))
					.Sort(Settings.AlbumsSortKey, Settings.AlbumsIsReversed), null as IAsyncEnumerable<ISong>),

				ArtistItemType.Songs => (null as IAsyncEnumerable<IAlbum>, Library.Songs
					.GetSongs(
						identifiers: Identifiers,
						cancellationToken: Cancellationtoken,
						retriever: LibraryItemView.Retrievers.GenerateSongRetriever(
							modelsortkey: Settings.SongsSortKey,
							librarylayouttype: Settings.SongsLayoutType,
							songs: ArtistItems.LibraryItemsAdapter.LibraryItems.OfType<ISong>()))
					.Sort(Settings.SongsSortKey, Settings.SongsIsReversed)),

				_ => throw new ArgumentException(), 
			};

			await SetArtist(Artist, albums, songs);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog)
			{
				if (ArtistItems.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
					ArtistItems.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;	
			}

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			bool result = base.OnMenuOptionClick(menuoption);

			return Showing switch
			{
				ArtistItemType.Albums => OnAlbumMenuOptionClick(menuoption),
				ArtistItemType.Songs => OnSongMenuOptionClick(menuoption),

				_ => result
			};
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			ArtistItems.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}

		public void SetArtist(IArtist? artist)
		{
			if (artist is null)
				ArtistHeader.Artist = null;
			else ArtistHeader.Artist ??= artist;

			(Drawable? drawable, string? text) = Showing switch
			{
				ArtistItemType.Albums => 
				(
					Context?.Resources?.GetDrawable(Resource.Drawable.icon_library_albums, Context.Theme),
					string.Format("{0} ({1})", Context?.Resources?.GetString(Resource.String.library_albums) ?? string.Empty, ArtistItems.LibraryItemsAdapter.LibraryItems.Count)
						.Trim()
				),							

				ArtistItemType.Songs => 
				(
					Context?.Resources?.GetDrawable(Resource.Drawable.icon_library_songs, Context.Theme),
					string.Format("{0} ({1})", Context?.Resources?.GetString(Resource.String.library_songs) ?? string.Empty, ArtistItems.LibraryItemsAdapter.LibraryItems.Count)
						.Trim()
				),

				_ => throw new ArgumentException(),
			};

			ArtistHeader.ArtistItems.Text.SetText(text, null);
			ArtistHeader.ArtistItems.Text.SetCompoundDrawablesRelativeWithIntrinsicBounds(drawable, null, null, null);
		}
		public void SetArtist(IArtist? artist, IEnumerable<IAlbum>? artistalbums, IEnumerable<ISong>? artistsongs)
		{
			ArtistItems.LibraryItemsAdapter.LibraryItems.Clear();
			ArtistItems.LibraryItemsAdapter.LibraryItems.AddRange(artistalbums);
			ArtistItems.LibraryItemsAdapter.LibraryItems.AddRange(artistsongs);

			SetArtist(artist);
		}
		public async Task SetArtist(IArtist? artist, IAsyncEnumerable<IAlbum>? artistalbums, IAsyncEnumerable<ISong>? artistsongs)
		{
			ArtistItems.LibraryItemsAdapter.LibraryItems.Clear();

			await Task.WhenAll(new Task[]
			{
				ArtistItems.LibraryItemsAdapter.LibraryItems.AddRange(artistalbums, Cancellationtoken),
				ArtistItems.LibraryItemsAdapter.LibraryItems.AddRange(artistsongs, Cancellationtoken),
			});

			SetArtist(artist);
		}

		public bool OnAlbumMenuOptionClick(MenuOptions menuoption)
		{
			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.ArtistAlbum.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistAlbums(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.ArtistAlbum.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueAlbums(MenuVariables);
					return true;

				case Menus.LibraryItem.ArtistAlbum.Delete:
					MenuOptionsUtils.DeleteAlbums(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.ArtistAlbum.EditInfo:
					MenuOptionsUtils.EditInfoAlbum(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.ArtistAlbum.GoToAlbum:
					DismissMenuOptions();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;

				case Menus.LibraryItem.ArtistAlbum.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlayAlbums(MenuVariables, null);
					return true;

				case Menus.LibraryItem.ArtistAlbum.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.ArtistAlbum.ViewInfo:
					MenuOptionsUtils.ViewInfoAlbum(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public bool OnSongMenuOptionClick(MenuOptions menuoption)
		{
			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.ArtistSong.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.ArtistSong.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueSongs(MenuVariables);
					return true;

				case Menus.LibraryItem.ArtistSong.Delete:
					MenuOptionsUtils.DeleteSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.ArtistSong.EditInfo:
					MenuOptionsUtils.EditInfoSong(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.ArtistSong.GoToAlbum:
					DismissMenuOptions();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;

				case Menus.LibraryItem.ArtistSong.GoToGenre:
					DismissMenuOptions();
					MenuOptionsUtils.GoToGenre(MenuVariables);
					return true;

				case Menus.LibraryItem.ArtistSong.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlaySongs(MenuVariables, null);
					return true;

				case Menus.LibraryItem.ArtistSong.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.ArtistSong.ViewInfo:
					MenuOptionsUtils.ViewInfoSong(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public async Task OnSortArtistAlbums()
		{
			if (Library is null)
				return;

			IEnumerable<IAlbum> albums = ArtistItems.LibraryItemsAdapter.LibraryItems.Cast<IAlbum>();

			IAsyncEnumerable<IAlbum> libraryitems = Library.Albums.GetAlbums(
				identifiers: Identifiers,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GenerateAlbumRetriever(
					albums: albums,
					modelsortkey: Settings.AlbumsSortKey,
					librarylayouttype: Settings.AlbumsLayoutType))
				.Sort(Settings.AlbumsSortKey, Settings.AlbumsIsReversed);

			if ((Settings.AlbumsSortKey).CanSort(albums) is false)
				await foreach (IAlbum libraryitem in libraryitems)
				{
					IAlbum? match = albums.FirstOrDefault(item => string.Equals(item.Id, libraryitem.Id));

					if (match != null && albums.Index(match) is int position && position >= 0)
						(Settings.AlbumsSortKey)
							.UpdateForSort((IAlbum)ArtistItems.LibraryItemsAdapter.LibraryItems[position], libraryitem);
				}

			SetArtist(Artist, ArtistItems.LibraryItemsAdapter.LibraryItems.Cast<IAlbum>()
				.Sort(Settings.AlbumsSortKey, Settings.AlbumsIsReversed)
				.ToList(), null);
		}
		public async Task OnSortArtistSongs()
		{
			if (Library is null)
				return;

			IEnumerable<ISong> songs = ArtistItems.LibraryItemsAdapter.LibraryItems.Cast<ISong>();

			IAsyncEnumerable<ISong> libraryitems = Library.Songs.GetSongs(
				identifiers: Identifiers,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GenerateSongRetriever(
					songs: songs,
					modelsortkey: Settings.SongsSortKey,
					librarylayouttype: Settings.SongsLayoutType))
				.Sort(Settings.SongsSortKey, Settings.SongsIsReversed);

			if ((Settings.SongsSortKey).CanSort(songs) is false)
				await foreach (ISong libraryitem in libraryitems)
				{
					ISong? match = songs.FirstOrDefault(item => string.Equals(item.Id, libraryitem.Id));

					if (match != null && songs.Index(match) is int position && position >= 0)
						(Settings.SongsSortKey)
							.UpdateForSort((ISong)ArtistItems.LibraryItemsAdapter.LibraryItems[position], libraryitem);
				}

			SetArtist(Artist, null, ArtistItems.LibraryItemsAdapter.LibraryItems.Cast<ISong>()
				.Sort(Settings.SongsSortKey, Settings.SongsIsReversed)
				.ToList());
		}
	}
}