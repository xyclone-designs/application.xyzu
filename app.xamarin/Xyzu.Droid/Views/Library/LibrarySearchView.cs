#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Menus;
using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Views.LibraryItem;
using Xyzu.Widgets.RecyclerViews;

using IXyzuLibrary = Xyzu.Library.ILibrary;

namespace Xyzu.Views.Library
{
	public class LibrarySearchView : LibraryView, ILibrarySearch
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_search;

			public const int SearchResults_ModelsRecyclerView = Resource.Id.xyzu_view_library_search_searchresults_modelsrecyclerview;
		}

		public LibrarySearchView(Context context) : base(context) { }
		public LibrarySearchView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Enum.GetValues(typeof(MenuOptions))
				.Cast<MenuOptions>()
				.Where(menuoption => menuoption switch
				{
					MenuOptions.EditInfo => true,
					MenuOptions.ViewInfo => true,

					MenuOptions.AddToPlaylist => true,
					MenuOptions.AddToQueue => true,
					MenuOptions.Delete => true,
					MenuOptions.Play => true,
					MenuOptions.Share => true,

					_ => false,
				})
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						MenuOptions.EditInfo => MenuOptionEnabledArray_Single,
						MenuOptions.ViewInfo => MenuOptionEnabledArray_Single,

						MenuOptions.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
						MenuOptions.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						MenuOptions.Delete => MenuOptionEnabledArray_SingleMultiple,
						MenuOptions.Play => MenuOptionEnabledArray_SingleMultiple,
						MenuOptions.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			SearchResults.LibraryItemsLayoutManager.LibraryLayoutType = Settings.LayoutType;

			SearchResults.LibraryItemsAdapter.Images = Images;
			SearchResults.LibraryItemsAdapter.Library = Library;
			SearchResults.LibraryItemsAdapter.LibraryLayoutType = Settings.LayoutType;
			SearchResults.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			SearchResults.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				IModel? model = SearchResults.LibraryItemsAdapter.LibraryItems.ElementAtOrDefault(position);

				viewholder.LibraryItem.SetModel(model, null);
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
				viewholder.Visibility =
					(Searcher.SearchAlbums is false && model is IAlbum) ||
					(Searcher.SearchArtists is false && model is IArtist) ||
					(Searcher.SearchGenres is false && model is IGenre) ||
					(Searcher.SearchPlaylists is false && model is IPlaylist) ||
					(Searcher.SearchSongs is false && model is ISong)
						? ViewStates.Gone
						: ViewStates.Visible;
			};
			SearchResults.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, SearchResults.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						if (SearchResults.LibraryItemsAdapter.FocusedLibraryItem is ISong)
							OnMenuOptionClick(MenuOptions.Play);
						Navigatable?.NavigateModel(SearchResults.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						SearchResults.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					SearchResults.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						SearchResults.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):	 
						SearchResults.LibraryItemsAdapter.SelectLibraryItemsFocused();
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

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(SearchResults.LibraryItemsAdapter);

			switch (SearchResults.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Albums = SearchResults.LibraryItemsAdapter.SelectedLibraryItems.OfType<IAlbum>();
					MenuVariables.Artists = SearchResults.LibraryItemsAdapter.SelectedLibraryItems.OfType<IArtist>();
					MenuVariables.Genres = SearchResults.LibraryItemsAdapter.SelectedLibraryItems.OfType<IGenre>();
					MenuVariables.Playlists = SearchResults.LibraryItemsAdapter.SelectedLibraryItems.OfType<IPlaylist>();
					MenuVariables.Songs = SearchResults.LibraryItemsAdapter.SelectedLibraryItems.OfType<ISong>();
					break;

				default:
					MenuVariables.Index = SearchResults.LibraryItemsAdapter.LibraryItems.Index(SearchResults.LibraryItemsAdapter.FocusedLibraryItem);

					MenuVariables.Albums = SearchResults.LibraryItemsAdapter.LibraryItems.OfType<IAlbum>();
					MenuVariables.Artists = SearchResults.LibraryItemsAdapter.LibraryItems.OfType<IArtist>();
					MenuVariables.Genres = SearchResults.LibraryItemsAdapter.LibraryItems.OfType<IGenre>();
					MenuVariables.Playlists = SearchResults.LibraryItemsAdapter.LibraryItems.OfType<IPlaylist>();
					MenuVariables.Songs = SearchResults.LibraryItemsAdapter.LibraryItems.OfType<ISong>();
					break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(ISongsSettings.LayoutType):
					SearchResults.ReloadLayout(Settings.LayoutType);
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibrarySearch(Settings)?
					.Apply();
		}

		private ISearchSettings? _Settings;
		private IXyzuLibrary.ISearcher? _Searcher;
		private ModelsRecyclerView? _SearchResults;

		protected override string? QueueId
		{
			get => MenuOptionsUtils.QueueIdSearch(Searcher);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(SearchResults.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => SearchResults.LibraryItemsAdapter;
		}

		public ISearchSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibrarySearch();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? ISearchSettings.Defaults.SearchSettings;
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
		public IXyzuLibrary.ISearcher Searcher
		{
			get => _Searcher ??= new IXyzuLibrary.ISearcher.Default();
			set
			{
				_Searcher = value;

				PropertyChanged();
			}
		}

		public ModelsRecyclerView SearchResults
		{
			get => _SearchResults ??= FindViewById(Ids.SearchResults_ModelsRecyclerView) as ModelsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || (force is false && SearchResults.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			SearchResults.LibraryItemsAdapter.LibraryItems.Clear();

			IList<IXyzuLibrary.ISearchResult> searchresults = await Library.Misc
				.SearchAsync(Searcher, Cancellationtoken)
				.ToListAsync();

			IXyzuLibrary.IIdentifiers identifiers = new IXyzuLibrary.IIdentifiers.Default
			{
				AlbumIds = searchresults
					.Where(searchresult => searchresult.ModelType == ModelTypes.Album)
					.Select(searchresult => searchresult.Id),
				
				ArtistIds = searchresults
					.Where(searchresult => searchresult.ModelType == ModelTypes.Artist)
					.Select(searchresult => searchresult.Id),
				
				GenreIds = searchresults
					.Where(searchresult => searchresult.ModelType == ModelTypes.Genre)
					.Select(searchresult => searchresult.Id),
				
				PlaylistIds = searchresults
					.Where(searchresult => searchresult.ModelType == ModelTypes.Playlist)
					.Select(searchresult => searchresult.Id),

				SongIds = searchresults
					.Where(searchresult => searchresult.ModelType == ModelTypes.Song)
					.Select(searchresult => searchresult.Id),
			};

			await foreach (ISong song in Library.Songs.GetSongs(identifiers, null, Cancellationtoken))
				SearchResults.LibraryItemsAdapter.LibraryItems.Add(song);			   

			await foreach (IAlbum album in Library.Albums.GetAlbums(identifiers, null, Cancellationtoken))
				SearchResults.LibraryItemsAdapter.LibraryItems.Add(album);

			await foreach (IArtist artist in Library.Artists.GetArtists(identifiers, null, Cancellationtoken))
				SearchResults.LibraryItemsAdapter.LibraryItems.Add(artist);

			await foreach (IGenre genre in Library.Genres.GetGenres(identifiers, null, Cancellationtoken))
				SearchResults.LibraryItemsAdapter.LibraryItems.Add(genre);			   

			await foreach (IPlaylist playlist in Library.Playlists.GetPlaylists(identifiers, null, Cancellationtoken))
				SearchResults.LibraryItemsAdapter.LibraryItems.Add(playlist);			   

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && SearchResults.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				SearchResults.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case MenuOptions.AddToPlaylist:
					// Album, Artist, Genre, Song
					MenuOptionsUtils.AddToPlaylist(MenuVariables)?.Show();
					return true;

				case MenuOptions.AddToQueue:
					// All 								  	  
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueue(MenuVariables);
					return true;

				case MenuOptions.Delete:
					// All 
					MenuOptionsUtils.Delete(MenuVariables)?
						.Show();
					return true;

				case MenuOptions.EditInfo when
				SearchResults.LibraryItemsAdapter.SelectedLibraryItems.FirstOrDefault() is IAlbum:
					MenuOptionsUtils.EditInfoAlbum(MenuVariables)?
						.Show();
					return true;
				case MenuOptions.EditInfo when
				SearchResults.LibraryItemsAdapter.SelectedLibraryItems.FirstOrDefault() is IArtist:
					MenuOptionsUtils.EditInfoArtist(MenuVariables)?
						.Show();
					return true;
				case MenuOptions.EditInfo when
				SearchResults.LibraryItemsAdapter.SelectedLibraryItems.FirstOrDefault() is IGenre:
					MenuOptionsUtils.EditInfoGenre(MenuVariables)?
						.Show();
					return true;
				case MenuOptions.EditInfo when
				SearchResults.LibraryItemsAdapter.SelectedLibraryItems.FirstOrDefault() is IPlaylist:
					MenuOptionsUtils.EditInfoPlaylist(MenuVariables)?
						.Show();
					return true;
				case MenuOptions.EditInfo when
				SearchResults.LibraryItemsAdapter.SelectedLibraryItems.FirstOrDefault() is ISong:
					MenuOptionsUtils.EditInfoSong(MenuVariables)?
						.Show();
					return true;

				case MenuOptions.GoToAlbum:
					// Album, Song								  
					DismissMenuOptions();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;

				case MenuOptions.GoToArtist:
					// Album, Artist, Song				  
					DismissMenuOptions();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;

				case MenuOptions.GoToGenre:
					// Genre, Song						
					DismissMenuOptions();
					MenuOptionsUtils.GoToGenre(MenuVariables);
					return true;

				case MenuOptions.GoToPlaylist:
					// Playlist							
					DismissMenuOptions();
					MenuOptionsUtils.GoToPlaylist(MenuVariables);
					return true;

				case MenuOptions.Play:
					switch(true)
					{
						case true when MenuVariables.Album != null:
							MenuOptionsUtils.PlayAlbums(MenuVariables, null);
							break;							   
						case true when MenuVariables.Artist != null:
							MenuOptionsUtils.PlayArtists(MenuVariables, null);
							break;							   
						case true when MenuVariables.Genre != null:
							MenuOptionsUtils.PlayGenres(MenuVariables, null);
							break;							   
						case true when MenuVariables.Playlist != null:
							MenuOptionsUtils.PlayPlaylists(MenuVariables, null);
							break;							   
						case true when MenuVariables.Song != null:
							MenuOptionsUtils.PlaySongs(MenuVariables, null);
							break;

						default:
							MenuOptionsUtils.Play(MenuVariables); 
							break;
					}
					DismissMenuOptions();
					return true;

				case MenuOptions.Select:
					SearchResults.LibraryItemsAdapter.SelectLibraryItemsFocused();
					return true;

				case MenuOptions.Share:
					// All 
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case MenuOptions.ViewInfo when MenuVariables.Album != null:
					MenuOptionsUtils.ViewInfoAlbum(MenuVariables)?
						.Show();
					return true;
				case MenuOptions.ViewInfo when MenuVariables.Artist != null:
					MenuOptionsUtils.ViewInfoArtist(MenuVariables)?
						.Show();
					return true;
				case MenuOptions.ViewInfo when MenuVariables.Genre != null:
					MenuOptionsUtils.ViewInfoGenre(MenuVariables)?
						.Show();
					return true;
				case MenuOptions.ViewInfo when MenuVariables.Playlist != null:
					MenuOptionsUtils.ViewInfoPlaylist(MenuVariables)?
						.Show();
					return true;
				case MenuOptions.ViewInfo when MenuVariables.Song != null:
					MenuOptionsUtils.ViewInfoSong(MenuVariables)?
						.Show();
					return true;

				default: return base.OnMenuOptionClick(menuoption);
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			SearchResults.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}
	}
}