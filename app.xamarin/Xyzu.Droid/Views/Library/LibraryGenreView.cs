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
	public partial class LibraryGenreView : LibraryView, ILibraryGenre
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_genre;

			public const int GenreSongs_SongsRecyclerView = Resource.Id.xyzu_view_library_genre_genresongs_songsrecyclerview;
		}

		public LibraryGenreView(Context context) : base(context) { }
		public LibraryGenreView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.GenreSong
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.GenreSong.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.GenreSong.GoToAlbum => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.GenreSong.GoToArtist => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.GenreSong.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.GenreSong.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.GenreSong.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.GenreSong.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.GenreSong.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.GenreSong.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			GenreHeader.Images = Images;
			GenreHeader.Library = Library;
			GenreHeader.OnClickOptions = (sender, args) =>
			{
				CreateOptionsMenuAlertDialog((optionsmenuview, alertdialog) =>
				{
					optionsmenuview.Text.Text = Genre?.Name;
					optionsmenuview.MenuOptions = Menus.LibraryItem.Genre.AsEnumerable();
					optionsmenuview.OnMenuOptionClicked = menuoption =>
					{
						MenuOptionsUtils.VariableContainer menuvariables = new MenuOptionsUtils.VariableContainer
						{
							Genres = Genres, 
							AnchorView = sender as View,
							AnchorViewGroup = sender as ViewGroup,
						};

						base.ProcessMenuVariables(menuvariables, null);

						switch (menuoption)
						{

							case Menus.LibraryItem.Genre.AddToPlaylist:
								MenuOptionsUtils.AddToPlaylistGenres(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Genre.AddToQueue:
								alertdialog.Dismiss();
								MenuOptionsUtils.AddToQueueGenres(menuvariables);
								return true;

							case Menus.LibraryItem.Genre.Delete:
								MenuOptionsUtils.DeleteGenres(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Genre.EditInfo:
								MenuOptionsUtils.EditInfoGenre(menuvariables)?
									.Show();
								return true;

							case Menus.LibraryItem.Genre.GoToGenre:
								alertdialog.Dismiss();
								MenuOptionsUtils.GoToGenre(menuvariables);
								return true;

							case Menus.LibraryItem.Genre.Play:
								alertdialog.Dismiss();
								MenuOptionsUtils.PlayGenres(menuvariables, Genre);
								return true;

							case Menus.LibraryItem.Genre.Share:
								MenuOptionsUtils.Share(menuvariables);
								return true;

							case Menus.LibraryItem.Genre.ViewInfo:
								MenuOptionsUtils.ViewInfoGenre(menuvariables)?
									.Show();
								return true;

							default: return false;
						}
					};

				}).Show();
			};
			GenreHeader.OnClickPlay = (sender, args) =>
			{
				ProcessMenuVariables(MenuVariables, null);

				if (GenreSongs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
					MenuOptionsUtils.PlaySongs(MenuVariables, null);

				else MenuOptionsUtils.PlayGenres(MenuVariables, Genre);
			};

			GenreHeader.GenreSongs.LibraryItemsOptions.IsReversed = Settings.SongsIsReversed;
			GenreHeader.GenreSongs.LibraryItemsOptions.LayoutTypes = IGenreSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered();
			GenreHeader.GenreSongs.LibraryItemsOptions.LayoutTypeSelected = Settings.SongsLayoutType;
			GenreHeader.GenreSongs.LibraryItemsOptions.SortKeys = IGenreSettings.Options.SongsSortKeys.AsEnumerable().OrderBy(_ => _);
			GenreHeader.GenreSongs.LibraryItemsOptions.SortKeySelected = Settings.SongsSortKey;
			GenreHeader.GenreSongs.LibraryItemsOptions.OnOptionsIsReversedClicked = isreversed => Settings.SongsIsReversed = isreversed;
			GenreHeader.GenreSongs.LibraryItemsOptions.OnOptionsLayoutTypeItemSelected = layouttype => Settings.SongsLayoutType = layouttype;
			GenreHeader.GenreSongs.LibraryItemsOptions.OnOptionsSortKeyItemSelected = sortkey => Settings.SongsSortKey = sortkey;

			GenreSongs.LibraryItemsItemDecoration.HeaderView = GenreHeader;
			GenreSongs.LibraryItemsItemDecoration.FooterView = InsetBottomView;

			GenreSongs.LibraryItemsLayoutManager.LibraryLayoutType = Settings.SongsLayoutType;

			GenreSongs.LibraryItemsAdapter.Images = Images;
			GenreSongs.LibraryItemsAdapter.Library = Library;
			GenreSongs.LibraryItemsAdapter.LibraryLayoutType = Settings.SongsLayoutType;
			GenreSongs.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			GenreSongs.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				viewholder.LibraryItem.SetSong(GenreSongs.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Song(Context));
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
			};
			GenreSongs.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, GenreSongs.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						OnMenuOptionClick(MenuOptions.Play);
						Navigatable?.NavigateSong(GenreSongs.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						GenreSongs.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					GenreSongs.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						GenreSongs.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						GenreSongs.LibraryItemsAdapter.SelectLibraryItemsFocused();
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

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(GenreSongs.LibraryItemsAdapter);

			switch (GenreSongs.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Songs = GenreSongs.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = GenreSongs.LibraryItemsAdapter.LibraryItems.Index(GenreSongs.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Songs = GenreSongs.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.PropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(Genre):
					SetGenre(Genre);
					break;

				default: break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(Images):
					if (GenreSongs.LibraryItemsAdapter != null)
						GenreSongs.LibraryItemsAdapter.Images = Images;
					break;

				case nameof(IGenreSettings.SongsLayoutType):
					GenreSongs.ReloadLayout(Settings.SongsLayoutType);
					break;
				case nameof(IGenreSettings.SongsIsReversed):
					GenreSongs.LibraryItemsAdapter.LibraryItems.Reverse();
					break;
				case nameof(IGenreSettings.SongsSortKey):
					GenreSongs.LibraryItemsAdapter.SetLibraryItems(GenreSongs.LibraryItemsAdapter.LibraryItems.Sort(Settings.SongsSortKey, Settings.SongsIsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryGenre(Settings)?
					.Apply();
		}

		private IGenre? _Genre;
		private IGenreSettings? _Settings;
		private ILibraryIdentifiers? _Identifiers;
		private SongsRecyclerView? _GenreSongs;

		protected override string? QueueId
		{
			get => MenuOptionsUtils.QueueIdGenre(Genre?.Id, Settings);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(GenreSongs.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => GenreSongs.LibraryItemsAdapter;
		}

		public IGenre? Genre
		{
			get => _Genre;
			set
			{
				_Genre = value;
				_Identifiers = null;

				PropertyChanged();
			}
		}
		public IEnumerable<IGenre> Genres
		{
			get
			{
				IEnumerable<IGenre> genres = Enumerable.Empty<IGenre>();

				if (Genre != null)
					genres = genres.Append(Genre);

				return genres;
			}
		}
		public IGenreSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryGenre();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IGenreSettings.Defaults.GenreSettings;
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
			get => _Identifiers ??= new ILibraryIdentifiers.Default().WithGenre(Genre);
		}
		public HeaderGenreView GenreHeader
		{
			get => (HeaderGenreView)(GenreSongs.LibraryItemsAdapter.Header ??= new HeaderGenreView(Context!)
			{
				Images = Images
			});
		}
		public SongsRecyclerView GenreSongs
		{
			get => _GenreSongs ??= FindViewById(Ids.GenreSongs_SongsRecyclerView) as SongsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || Genre is null || (force is false && GenreSongs.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			if (force)
				GenreHeader.Genre = null;

			await Library.Genres.PopulateGenre(Genre, Cancellationtoken);

			IAsyncEnumerable<ISong> libraryitems = Library.Songs.GetSongs(
				identifiers: Identifiers,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GenerateSongRetriever(
					songs: GenreSongs.LibraryItemsAdapter.LibraryItems,
					modelsortkey: Settings.SongsSortKey,
					librarylayouttype: Settings.SongsLayoutType))
				.Sort(Settings.SongsSortKey, Settings.SongsIsReversed);

			await SetGenre(Genre, libraryitems);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && GenreSongs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				GenreSongs.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.GenreSong.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.GenreSong.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueSongs(MenuVariables);
					return true;

				case Menus.LibraryItem.GenreSong.Delete:
					MenuOptionsUtils.DeleteSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.GenreSong.EditInfo:
					MenuOptionsUtils.ViewInfoGenre(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.GenreSong.GoToAlbum:
					DismissMenuOptions();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;										  

				case Menus.LibraryItem.GenreSong.GoToArtist:
					DismissMenuOptions();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;	  

				case Menus.LibraryItem.GenreSong.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlaySongs(MenuVariables, null);
					return true;

				case Menus.LibraryItem.GenreSong.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.GenreSong.ViewInfo:
					MenuOptionsUtils.ViewInfoSong(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			GenreSongs.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}

		public void SetGenre(IGenre? genre)
		{
			if (genre is null)
				GenreHeader.Genre = null;
			else GenreHeader.Genre ??= genre;

			GenreHeader.GenreSongs.Text.SetCompoundDrawablesRelativeWithIntrinsicBounds(Context?.Resources?.GetDrawable(Resource.Drawable.icon_library_songs, Context.Theme), null, null, null);
			GenreHeader.GenreSongs.Text.Text = string.Format("{0} ({1})", Context?.Resources?.GetString(Resource.String.library_songs) ?? string.Empty, GenreSongs.LibraryItemsAdapter.LibraryItems.Count);
		}
		public void SetGenre(IGenre? genre, IEnumerable<ISong>? genresongs)
		{
			GenreSongs.LibraryItemsAdapter.LibraryItems.Clear();
			GenreSongs.LibraryItemsAdapter.LibraryItems.AddRange(genresongs);

			SetGenre(genre);
		}
		public async Task SetGenre(IGenre? genre, IAsyncEnumerable<ISong>? genresongs)
		{
			GenreSongs.LibraryItemsAdapter.LibraryItems.Clear();

			await GenreSongs.LibraryItemsAdapter.LibraryItems.AddRange(genresongs, Cancellationtoken);

			SetGenre(genre);
		}
	}
}