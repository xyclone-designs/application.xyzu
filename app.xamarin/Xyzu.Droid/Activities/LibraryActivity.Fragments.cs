#nullable enable

using Android.Content;

using System;
using System.Threading;

using Xyzu.Fragments.Library;
using Xyzu.Library.Models;
using Xyzu.Views.Library;

namespace Xyzu.Activities
{
	public partial class LibraryActivity 
	{
		private LibraryAlbumFragment? _FragmentLibraryAlbum;
		private LibraryAlbumsFragment? _FragmentLibraryAlbums;
		private LibraryArtistFragment? _FragmentLibraryArtist;
		private LibraryArtistsFragment? _FragmentLibraryArtists;
		private LibraryGenreFragment? _FragmentLibraryGenre;
		private LibraryGenresFragment? _FragmentLibraryGenres;
		private LibraryPlaylistFragment? _FragmentLibraryPlaylist;
		private LibraryPlaylistsFragment? _FragmentLibraryPlaylists;
		private LibraryQueueFragment? _FragmentLibraryQueue;
		private LibrarySearchFragment? _FragmentLibrarySearch;
		private LibrarySongsFragment? _FragmentLibrarySongs;

		protected LibraryFragment? CurrentLibraryFragment { get; set; }
		protected virtual LibraryAlbumFragment FragmentLibraryAlbum
		{
			set => _FragmentLibraryAlbum = value;
			get => _FragmentLibraryAlbum ??= new LibraryAlbumFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}		
		protected virtual LibraryAlbumsFragment FragmentLibraryAlbums
		{
			set => _FragmentLibraryAlbums = value;
			get => _FragmentLibraryAlbums ??= new LibraryAlbumsFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}
		protected virtual LibraryArtistFragment FragmentLibraryArtist
		{
			set => _FragmentLibraryArtist = value;
			get => _FragmentLibraryArtist ??= new LibraryArtistFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}	  
		protected virtual LibraryArtistsFragment FragmentLibraryArtists
		{
			set => _FragmentLibraryArtists = value;
			get => _FragmentLibraryArtists ??= new LibraryArtistsFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}
		protected virtual LibraryGenreFragment FragmentLibraryGenre
		{
			set => _FragmentLibraryGenre = value;
			get => _FragmentLibraryGenre ??= new LibraryGenreFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}	   
		protected virtual LibraryGenresFragment FragmentLibraryGenres
		{
			set => _FragmentLibraryGenres = value;
			get => _FragmentLibraryGenres ??= new LibraryGenresFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}
		protected virtual LibraryPlaylistFragment FragmentLibraryPlaylist
		{
			set => _FragmentLibraryPlaylist = value;
			get => _FragmentLibraryPlaylist ??= new LibraryPlaylistFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		} 
		protected virtual LibraryPlaylistsFragment FragmentLibraryPlaylists
		{
			set => _FragmentLibraryPlaylists = value;
			get => _FragmentLibraryPlaylists ??= new LibraryPlaylistsFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}
		protected virtual LibraryQueueFragment FragmentLibraryQueue
		{
			set => _FragmentLibraryQueue = value;
			get => _FragmentLibraryQueue ??= new LibraryQueueFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}
		protected virtual LibrarySearchFragment FragmentLibrarySearch
		{
			set => _FragmentLibrarySearch = value;
			get => _FragmentLibrarySearch ??= new LibrarySearchFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView(libraryview =>
				{
					if (libraryview is LibrarySearchView librarysearchview)
					{
						ToolbarSearch.Searcher = librarysearchview.Searcher;
						ToolbarSearch.OnSubmit = async () => await librarysearchview.OnRefresh(true);
						ToolbarSearch.OnOptions = () => FragmentLibrarySearch.OnMenuOptionClick(Menus.Library.ListOptions);
						ToolbarSearch.OnSearcherPropertyChanged = propertyname =>
						{
							switch(propertyname)
							{
								case nameof(ToolbarSearch.Searcher.SearchAlbums):
								case nameof(ToolbarSearch.Searcher.SearchArtists):
								case nameof(ToolbarSearch.Searcher.SearchGenres):
								case nameof(ToolbarSearch.Searcher.SearchPlaylists):
								case nameof(ToolbarSearch.Searcher.SearchSongs):
									librarysearchview.SearchResults.LibraryItemsAdapter.NotifyDataSetChanged();
									break;

								default: break;
							}
						};
					}
				})
			};
		}
		protected virtual LibrarySongsFragment FragmentLibrarySongs
		{
			set => _FragmentLibrarySongs = value;
			get => _FragmentLibrarySongs ??= new LibrarySongsFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView
			};
		}

		protected LibraryAlbumFragment GenerateFragmentLibraryAlbum(IAlbum? album, bool refreshoncreate = true)
		{
			return FragmentLibraryAlbum = new LibraryAlbumFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView(async libraryview =>
				{
					if (libraryview is LibraryAlbumView libraryalbumview)
					{
						libraryalbumview.Album = album;

						if (refreshoncreate)
							await libraryalbumview.OnRefresh();
					}
				}),
			};
		}
		protected LibraryArtistFragment GenerateFragmentLibraryArtist(IArtist? artist, bool refreshoncreate = true)
		{
			return FragmentLibraryArtist = new LibraryArtistFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView(async libraryview =>
				{
					if (libraryview is LibraryArtistView libraryartistview)
					{
						libraryartistview.Artist = artist;
						
						if (refreshoncreate) 
							await libraryartistview.OnRefresh();
					}
				}),
			};
		}
		protected LibraryGenreFragment GenerateFragmentLibraryGenre(IGenre? genre, bool refreshoncreate = true)
		{
			return FragmentLibraryGenre = new LibraryGenreFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView(async libraryview =>
				{
					if (libraryview is LibraryGenreView librarygenreview)
					{
						librarygenreview.Genre = genre;

						if (refreshoncreate)
							await librarygenreview.OnRefresh();
					}
				}),
			};
		}
		protected LibraryPlaylistFragment GenerateFragmentLibraryPlaylist(IPlaylist? playlist, bool refreshoncreate = true)
		{
			return FragmentLibraryPlaylist = new LibraryPlaylistFragment
			{
				Activity = this,
				OnCreateViewAction = OnCreateLibraryView(async libraryview =>
				{
					if (libraryview is LibraryPlaylistView libraryplaylistview)
					{
						libraryplaylistview.Playlist = playlist;

						if (refreshoncreate)
							await libraryplaylistview.OnRefresh();
					}
				}),
			};
		}

		protected async void OnCreateLibraryView(LibraryView libraryview)
		{
			libraryview.Cancellationtoken = (ActivityCancellationTokenSource ??= new CancellationTokenSource()).Token;
			libraryview.Images = XyzuImages.Instance;
			libraryview.Library = XyzuLibrary.Instance;
			libraryview.Navigatable = this;
			libraryview.Player = XyzuPlayer.Instance.Player;
			libraryview.SetOnRefreshListener(this);
			libraryview.SharedPreferences = XyzuSettings.Instance;
			libraryview.AddInsets("NavigationBarHeight", null, null, null, Resources?.GetNavigationBarHeight());

			ConfigurePanelInsets(libraryview);

			await libraryview.OnRefresh();
		}
		protected Action<LibraryView> OnCreateLibraryView(Action<LibraryView> action)
		{
			return _libraryview =>
			{
				OnCreateLibraryView(_libraryview);

				action.Invoke(_libraryview);
			};
		}

		protected void FragmentActions(Action<LibraryFragment> action)
		{
			if (_FragmentLibraryAlbum != null) 
				action.Invoke(FragmentLibraryAlbum);

			if (_FragmentLibraryAlbums != null) 
				action.Invoke(FragmentLibraryAlbums);

			if (_FragmentLibraryArtist != null) 
				action.Invoke(FragmentLibraryArtist);

			if (_FragmentLibraryArtists != null) 
				action.Invoke(FragmentLibraryArtists);

			if (_FragmentLibraryGenre != null) 
				action.Invoke(FragmentLibraryGenre);

			if (_FragmentLibraryGenres != null) 
				action.Invoke(FragmentLibraryGenres);

			if (_FragmentLibraryPlaylist != null) 
				action.Invoke(FragmentLibraryPlaylist);

			if (_FragmentLibraryPlaylists != null) 
				action.Invoke(FragmentLibraryPlaylists);

			if (_FragmentLibraryQueue != null) 
				action.Invoke(FragmentLibraryQueue);

			if (_FragmentLibrarySearch != null) 
				action.Invoke(FragmentLibrarySearch);

			if (_FragmentLibrarySongs != null) 
				action.Invoke(FragmentLibrarySongs);
		}
	}
}