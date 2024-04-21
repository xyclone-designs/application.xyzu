#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;
using Xyzu.Library;

namespace Xyzu.Views.Toolbar
{
	public class ToolbarSearchView : ToolbarView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_toolbar_search;

			public const int StatusBarInset = Resource.Id.xyzu_view_toolbar_search_statusbarinsetview;
			public const int SearchView = Resource.Id.xyzu_view_toolbar_search_searchview;
			public const int Searcher_HorizontalScrollView = Resource.Id.xyzu_view_toolbar_search_searcher_horizontalscrollview;
			public const int Searcher_AlbumsToggle_AppCompatImageButton = Resource.Id.xyzu_view_toolbar_search_searcher_albumstoggle_appcompatimagebutton;
			public const int Searcher_ArtistsToggle_AppCompatImageButton = Resource.Id.xyzu_view_toolbar_search_searcher_artiststoggle_appcompatimagebutton;
			public const int Searcher_GenresToggle_AppCompatImageButton = Resource.Id.xyzu_view_toolbar_search_searcher_genrestoggle_appcompatimagebutton;
			public const int Searcher_PlaylistsToggle_AppCompatImageButton = Resource.Id.xyzu_view_toolbar_search_searcher_playliststoggle_appcompatimagebutton;
			public const int Searcher_SongsToggle_AppCompatImageButton = Resource.Id.xyzu_view_toolbar_search_searcher_songstoggle_appcompatimagebutton;
			public const int SearchResults_OptionsToggle_AppCompatImageButton = Resource.Id.xyzu_view_toolbar_search_searchresults_optionstoggle_appcompatimagebutton;

		}

		public ToolbarSearchView(Context context) : base(context)
		{ }
		public ToolbarSearchView(Context context, IAttributeSet attrs) : base(context, attrs)
		{ }
		public ToolbarSearchView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			base.Init(context, attrs);

			Inflate(context, Ids.Layout, this);
		}
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			Search.QueryTextChange += SearchQueryTextChange;
			Search.QueryTextSubmit += SearchQueryTextSubmit;

			SearcherAlbumsToggle.Click += SearcherAlbumsToggleClick;
			SearcherArtistsToggle.Click += SearcherArtistsToggleClick;
			SearcherGenresToggle.Click += SearcherGenresToggleClick;
			SearcherPlaylistsToggle.Click += SearcherPlaylistsToggleClick;
			SearcherSongsToggle.Click += SearcherSongsToggleClick;
			SearchResultsOptionsToggle.Click += SearchResultsOptionsToggleClick;

		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			Search.QueryTextChange -= SearchQueryTextChange;
			Search.QueryTextSubmit -= SearchQueryTextSubmit;

			SearcherAlbumsToggle.Click -= SearcherAlbumsToggleClick;
			SearcherArtistsToggle.Click -= SearcherArtistsToggleClick;
			SearcherGenresToggle.Click -= SearcherGenresToggleClick;
			SearcherPlaylistsToggle.Click -= SearcherPlaylistsToggleClick;
			SearcherSongsToggle.Click -= SearcherSongsToggleClick;
			SearchResultsOptionsToggle.Click -= SearchResultsOptionsToggleClick;
		}

		private ILibrary.ISearcher? _Searcher;
		private SearchView? _Search;
		private AppCompatImageButton? _SearcherAlbumsToggle;
		private AppCompatImageButton? _SearcherArtistsToggle;
		private AppCompatImageButton? _SearcherGenresToggle;
		private AppCompatImageButton? _SearcherPlaylistsToggle;
		private AppCompatImageButton? _SearcherSongsToggle;
		private AppCompatImageButton? _SearchResultsOptionsToggle;

		public Action? OnOptions { get; set; }
		public Action? OnSubmit { get; set; }
		public Action<string>? OnSearcherPropertyChanged { get; set; }

		public ILibrary.ISearcher Searcher
		{
			get => _Searcher ??= new ILibrary.ISearcher.Default { };
			set
			{
				_Searcher = value;

				Search.SetQuery(_Searcher.String, false);

				SearcherAlbumsToggle.Selected = _Searcher.SearchAlbums;
				SearcherArtistsToggle.Selected = _Searcher.SearchArtists;
				SearcherGenresToggle.Selected = _Searcher.SearchGenres;
				SearcherPlaylistsToggle.Selected = _Searcher.SearchPlaylists;
				SearcherSongsToggle.Selected = _Searcher.SearchSongs;
			}
		}

		public SearchView Search
		{
			get => _Search ??= FindViewById(Ids.SearchView) as SearchView ?? throw new InflateException();
		}
		public AppCompatImageButton SearcherAlbumsToggle
		{
			get => _SearcherAlbumsToggle ??= FindViewById(Ids.Searcher_AlbumsToggle_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}
		public AppCompatImageButton SearcherArtistsToggle
		{
			get => _SearcherArtistsToggle ??= FindViewById(Ids.Searcher_ArtistsToggle_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}
		public AppCompatImageButton SearcherGenresToggle
		{
			get => _SearcherGenresToggle ??= FindViewById(Ids.Searcher_GenresToggle_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}
		public AppCompatImageButton SearcherPlaylistsToggle
		{
			get => _SearcherPlaylistsToggle ??= FindViewById(Ids.Searcher_PlaylistsToggle_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}
		public AppCompatImageButton SearcherSongsToggle
		{
			get => _SearcherSongsToggle ??= FindViewById(Ids.Searcher_SongsToggle_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}
		public AppCompatImageButton SearchResultsOptionsToggle
		{
			get => _SearchResultsOptionsToggle ??= FindViewById(Ids.SearchResults_OptionsToggle_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}

		void SearcherAlbumsToggleClick(object sender, EventArgs args)
		{
			SearcherAlbumsToggle.Selected = Searcher.SearchAlbums = !SearcherAlbumsToggle.Selected;

			OnSearcherPropertyChanged?.Invoke(nameof(Searcher.SearchAlbums));
		}
		void SearcherArtistsToggleClick(object sender, EventArgs args)
		{
			SearcherArtistsToggle.Selected = Searcher.SearchArtists = !SearcherArtistsToggle.Selected;

			OnSearcherPropertyChanged?.Invoke(nameof(Searcher.SearchArtists));
		}
		void SearcherGenresToggleClick(object sender, EventArgs args)
		{
			SearcherGenresToggle.Selected = Searcher.SearchGenres = !SearcherGenresToggle.Selected;

			OnSearcherPropertyChanged?.Invoke(nameof(Searcher.SearchGenres));
		}
		void SearcherPlaylistsToggleClick(object sender, EventArgs args)
		{
			SearcherPlaylistsToggle.Selected = Searcher.SearchPlaylists = !SearcherPlaylistsToggle.Selected;

			OnSearcherPropertyChanged?.Invoke(nameof(Searcher.SearchPlaylists));
		}
		void SearcherSongsToggleClick(object sender, EventArgs args)
		{
			SearcherSongsToggle.Selected = Searcher.SearchSongs = !SearcherSongsToggle.Selected;

			OnSearcherPropertyChanged?.Invoke(nameof(Searcher.SearchSongs));
		}
		void SearchResultsOptionsToggleClick(object sender, EventArgs args)
		{
			OnOptions?.Invoke();
		}

		private void SearchQueryTextChange(object sender, SearchView.QueryTextChangeEventArgs args)
		{
			Searcher.String = args.NewText ?? string.Empty;

			OnSearcherPropertyChanged?.Invoke(nameof(Searcher.String));
		}
		private void SearchQueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs args)
		{
			Search.ClearFocus();

			OnSubmit?.Invoke();
		}
	}
}