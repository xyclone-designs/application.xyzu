using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface ILibrarySettings<T> : ISettings<T>
	{
		T PageDefault { get; set; }
		T PagesOrdered { get; set; }
	}
	public partial interface ILibrarySettings : ISettings
	{
		LibraryPages PageDefault { get; set; }
		IEnumerable<LibraryPages> PagesOrdered { get; set; }

		public new class Defaults : ISettings.Defaults 
		{
			public const LibraryPages PageDefault = Options.Pages.Songs;

			public const int PagesOrderedAlbums = 2;
			public const int PagesOrderedArtists = 3;
			public const int PagesOrderedGenres = 4;
			public const int PagesOrderedPlaylists = 5;
			public const int PagesOrderedQueue = 0;
			public const int PagesOrderedSongs = 1;

			public static readonly ILibrarySettings LibrarySettings = new Default
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
			public new const string Base = ISettings.Keys.Base + "." + nameof(ILibrarySettings);

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

		public new class Default : ISettings.Default, ILibrarySettings
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
		public new class Default<T> : ISettings.Default<T>, ILibrarySettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				PageDefault = defaultvalue;
				PagesOrdered = defaultvalue;
			}

			public T PageDefault { get; set; }
			public T PagesOrdered { get; set; }
		}
	}
}
