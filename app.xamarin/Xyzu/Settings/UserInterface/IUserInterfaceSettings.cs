using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Settings.UserInterface
{
	public interface IUserInterfaceSettings<T> : ISettings<T>
	{
		T PageDefault { get; set; }
		T PagesOrdered { get; set; }
		T Album { get; set; }
		T Albums { get; set; }
		T Artist { get; set; }
		T Artists { get; set; }
		T Genre { get; set; }
		T Genres { get; set; }
		T Playlist { get; set; }
		T Playlists { get; set; }
		T Queue { get; set; }
		T Search { get; set; }
		T Songs { get; set; }
	}
	public partial interface IUserInterfaceSettings : ISettings
	{
		LibraryPages PageDefault { get; set; }
		IEnumerable<LibraryPages> PagesOrdered { get; set; }
		IAlbumSettings Album { get; set; }
		IAlbumsSettings Albums { get; set; }
		IArtistSettings Artist { get; set; }
		IArtistsSettings Artists { get; set; }
		IGenreSettings Genre { get; set; }
		IGenresSettings Genres { get; set; }
		IPlaylistSettings Playlist { get; set; }
		IPlaylistsSettings Playlists { get; set; }
		IQueueSettings Queue { get; set; }
		ISearchSettings Search { get; set; }
		ISongsSettings Songs { get; set; }

		public new class Defaults : ISettings.Defaults
		{
			public const LibraryPages PageDefault = Options.Pages.Songs;

			public const int PagesOrderedAlbums = 2;
			public const int PagesOrderedArtists = 3;
			public const int PagesOrderedGenres = 4;
			public const int PagesOrderedPlaylists = 5;
			public const int PagesOrderedQueue = 0;
			public const int PagesOrderedSongs = 1;

			public static readonly IUserInterfaceSettings UserInterfaceSettings = new Default
			{
				PageDefault = PageDefault,
				PagesOrdered = PagesOrdered(),
			};

			public static IEnumerable<LibraryPages> PagesOrdered()
			{
				return Enumerable.Empty<LibraryPages>()
					.Append(Options.Pages.Queue)
					.Append(Options.Pages.Songs)
					.Append(Options.Pages.Albums)
					.Append(Options.Pages.Artists)
					.Append(Options.Pages.Genres)
					.Append(Options.Pages.Playlists);
			}
		}
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(IUserInterfaceSettings);

			public const string PageDefault = Base + ".PageDefault";

			public const string PagesOrderedAlbums = Base + ".PagesOrderedAlbums";
			public const string PagesOrderedArtists = Base + ".PagesOrderedArtists";
			public const string PagesOrderedGenres = Base + ".PagesOrderedGenres";
			public const string PagesOrderedPlaylists = Base + ".PagesOrderedPlaylists";
			public const string PagesOrderedQueue = Base + ".PagesOrderedQueue";
			public const string PagesOrderedSongs = Base + ".PagesOrderedSongs";
		}
		public new class Options : ISettings.Options
		{
			public class Pages
			{
				public const LibraryPages Albums = LibraryPages.Albums;
				public const LibraryPages Artists = LibraryPages.Artists;
				public const LibraryPages Genres = LibraryPages.Genres;
				public const LibraryPages Playlists = LibraryPages.Playlists;
				public const LibraryPages Queue = LibraryPages.Queue;
				public const LibraryPages Songs = LibraryPages.Songs;

				public static IEnumerable<LibraryPages> AsEnumerable()
				{
					return Enumerable.Empty<LibraryPages>()
						.Append(Albums)
						.Append(Artists)
						.Append(Genres)
						.Append(Playlists)
						.Append(Queue)
						.Append(Songs);
				}
			}
		}

		public new class Default : ISettings.Default, IUserInterfaceSettings
		{
			public Default()
			{
				_PageDefault = Defaults.PageDefault;
				_PagesOrdered = Options.Pages.AsEnumerable()
					.OrderBy(librarypage => librarypage switch
					{
						Options.Pages.Albums => Defaults.PagesOrderedAlbums,
						Options.Pages.Artists => Defaults.PagesOrderedArtists,
						Options.Pages.Genres => Defaults.PagesOrderedGenres,
						Options.Pages.Playlists => Defaults.PagesOrderedPlaylists,
						Options.Pages.Queue => Defaults.PagesOrderedQueue,
						Options.Pages.Songs => Defaults.PagesOrderedSongs,

						_ => throw new ArgumentException("Invalid LibraryPage"),
					});
			}

			private LibraryPages _PageDefault;
			private IEnumerable<LibraryPages> _PagesOrdered;
			private IAlbumSettings _Album;
			private IAlbumsSettings _Albums;
			private IArtistSettings _Artist;
			private IArtistsSettings _Artists;
			private IGenreSettings _Genre;
			private IGenresSettings _Genres;
			private IPlaylistSettings _Playlist;
			private IPlaylistsSettings _Playlists;
			private IQueueSettings _Queue;
			private ISearchSettings _Search;
			private ISongsSettings _Songs;

			public LibraryPages PageDefault
			{
				get => _PageDefault;
				set
				{
					_PageDefault = value;

					OnPropertyChanged();
				}
			}
			public IEnumerable<LibraryPages> PagesOrdered
			{
				get => _PagesOrdered;
				set
				{
					_PagesOrdered = value;

					OnPropertyChanged();
				}
			}
			public IAlbumSettings Album
			{
				get => _Album;
				set
				{
					_Album = value;

					OnPropertyChanged();
				}
			}
			public IAlbumsSettings Albums
			{
				get => _Albums;
				set
				{
					_Albums = value;

					OnPropertyChanged();
				}
			}
			public IArtistSettings Artist
			{
				get => _Artist;
				set
				{
					_Artist = value;

					OnPropertyChanged();
				}
			}
			public IArtistsSettings Artists
			{
				get => _Artists;
				set
				{
					_Artists = value;

					OnPropertyChanged();
				}
			}
			public IGenreSettings Genre
			{
				get => _Genre;
				set
				{
					_Genre = value;

					OnPropertyChanged();
				}
			}
			public IGenresSettings Genres
			{
				get => _Genres;
				set
				{
					_Genres = value;

					OnPropertyChanged();
				}
			}
			public IPlaylistSettings Playlist
			{
				get => _Playlist;
				set
				{
					_Playlist = value;

					OnPropertyChanged();
				}
			}
			public IPlaylistsSettings Playlists
			{
				get => _Playlists;
				set
				{
					_Playlists = value;

					OnPropertyChanged();
				}
			}
			public IQueueSettings Queue
			{
				get => _Queue;
				set
				{
					_Queue = value;

					OnPropertyChanged();
				}
			}
			public ISearchSettings Search
			{
				get => _Search;
				set
				{
					_Search = value;

					OnPropertyChanged();
				}
			}
			public ISongsSettings Songs
			{
				get => _Songs;
				set
				{
					_Songs = value;

					OnPropertyChanged();
				}
			}

			public override void SetFromKey(string key, object? value)
			{
				base.SetFromKey(key, value);

				switch (key)
				{
					case Keys.PageDefault when value is LibraryPages pagedefault:
						PageDefault = pagedefault;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : ISettings.Default<T>, IUserInterfaceSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				PageDefault = defaultvalue;
				PagesOrdered = defaultvalue;
				Album = defaultvalue;
				Albums = defaultvalue;
				Artist = defaultvalue;
				Artists = defaultvalue;
				Genre = defaultvalue;
				Genres = defaultvalue;
				Playlist = defaultvalue;
				Playlists = defaultvalue;
				Queue = defaultvalue;
				Search = defaultvalue;
				Songs = defaultvalue;
			}

			public T PageDefault { get; set; }
			public T PagesOrdered { get; set; }
			public T Album { get; set; }
			public T Albums { get; set; }
			public T Artist { get; set; }
			public T Artists { get; set; }
			public T Genre { get; set; }
			public T Genres { get; set; }
			public T Playlist { get; set; }
			public T Playlists { get; set; }
			public T Queue { get; set; }
			public T Search { get; set; }
			public T Songs { get; set; }
		}
	}
}
