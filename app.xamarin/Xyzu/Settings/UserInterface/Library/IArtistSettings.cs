using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface IArtistSettings<T> : ISettings<T>
	{
		T AlbumsIsReversed { get; set; }
		T AlbumsLayoutType { get; set; }
		T AlbumsSortKey { get; set; }
		T SongsIsReversed { get; set; }
		T SongsLayoutType { get; set; }
		T SongsSortKey { get; set; }
	}
	public interface IArtistSettings : ISettings
	{
		public new class Defaults : ISettings.Defaults
		{
			public static readonly bool AlbumsIsReversed = false;
			public static readonly LibraryLayoutTypes AlbumsLayoutType = LibraryLayoutTypes.GridMedium;
			public static readonly ModelSortKeys AlbumsSortKey = ModelSortKeys.Title;	
			public static readonly bool SongsIsReversed = false;
			public static readonly LibraryLayoutTypes SongsLayoutType = LibraryLayoutTypes.ListMedium;
			public static readonly ModelSortKeys SongsSortKey = ModelSortKeys.Title;

			public static readonly IArtistSettings ArtistSettings = new Default
			{
				AlbumsIsReversed = AlbumsIsReversed,
				AlbumsLayoutType = AlbumsLayoutType,
				AlbumsSortKey = AlbumsSortKey,	   
				SongsIsReversed = SongsIsReversed,
				SongsLayoutType = SongsLayoutType,
				SongsSortKey = SongsSortKey,
			};
		}
		public new class Keys : ISettings.Keys
		{
			public new const string Base = IUserInterfaceSettings.Keys.Base + "." + nameof(IArtistSettings);

			public const string AlbumsIsReversed = Base + "." + nameof(AlbumsIsReversed);
			public const string AlbumsLayoutType = Base + "." + nameof(AlbumsLayoutType);
			public const string AlbumsSortKey = Base + "." + nameof(AlbumsSortKey);		 
			public const string SongsIsReversed = Base + "." + nameof(SongsIsReversed);
			public const string SongsLayoutType = Base + "." + nameof(SongsLayoutType);
			public const string SongsSortKey = Base + "." + nameof(SongsSortKey);
		}
		public new class Options : ISettings.Options
		{
			public class AlbumsLayoutTypes : LibraryDefaultSettings.Options.LayoutTypes { }
			public class AlbumsSortKeys : IArtist.SortKeys.AlbumSortKeys { }

			public class SongsLayoutTypes : LibraryDefaultSettings.Options.LayoutTypes { }
			public class SongsSortKeys : IArtist.SortKeys.SongSortKeys { }
		}

		bool AlbumsIsReversed { get; set; }
		LibraryLayoutTypes AlbumsLayoutType { get; set; }
		ModelSortKeys AlbumsSortKey { get; set; }
		bool SongsIsReversed { get; set; }
		LibraryLayoutTypes SongsLayoutType { get; set; }
		ModelSortKeys SongsSortKey { get; set; }

		public new class Default : ISettings.Default, IArtistSettings
		{
			public Default()
			{
				_AlbumsIsReversed = Defaults.AlbumsIsReversed;
				_AlbumsLayoutType = Defaults.AlbumsLayoutType;
				_AlbumsSortKey = Defaults.AlbumsSortKey;			
				_SongsIsReversed = Defaults.SongsIsReversed;
				_SongsLayoutType = Defaults.SongsLayoutType;
				_SongsSortKey = Defaults.SongsSortKey;
			}

			private bool _AlbumsIsReversed;
			private LibraryLayoutTypes _AlbumsLayoutType;
			private ModelSortKeys _AlbumsSortKey;
			private bool _SongsIsReversed;
			private LibraryLayoutTypes _SongsLayoutType;
			private ModelSortKeys _SongsSortKey;	  

			public bool AlbumsIsReversed
			{
				get => _AlbumsIsReversed;
				set
				{
					_AlbumsIsReversed = value;

					OnPropertyChanged();
				}
			}
			public LibraryLayoutTypes AlbumsLayoutType
			{
				get => _AlbumsLayoutType;
				set
				{
					_AlbumsLayoutType = value;

					OnPropertyChanged();
				}
			}
			public ModelSortKeys AlbumsSortKey
			{
				get => _AlbumsSortKey;
				set
				{
					_AlbumsSortKey = value;

					OnPropertyChanged();
				}
			}													
			public bool SongsIsReversed
			{
				get => _SongsIsReversed;
				set
				{
					_SongsIsReversed = value;

					OnPropertyChanged();
				}
			}
			public LibraryLayoutTypes SongsLayoutType
			{
				get => _SongsLayoutType;
				set
				{
					_SongsLayoutType = value;

					OnPropertyChanged();
				}
			}
			public ModelSortKeys SongsSortKey
			{
				get => _SongsSortKey;
				set
				{
					_SongsSortKey = value;

					OnPropertyChanged();
				}
			}

			public override void SetFromKey(string key, object? value)
			{
				base.SetFromKey(key, value);

				switch (key)
				{
					case Keys.AlbumsIsReversed when value is bool albumsisreversed:
						AlbumsIsReversed = albumsisreversed;
						break;

					case Keys.AlbumsLayoutType when value is LibraryLayoutTypes albumslayouttype:
						AlbumsLayoutType = albumslayouttype;
						break;

					case Keys.AlbumsSortKey when value is ModelSortKeys albumssortkey:
						AlbumsSortKey = albumssortkey;
						break;
										 
					case Keys.SongsIsReversed when value is bool songsisreversed:
						SongsIsReversed = songsisreversed;
						break;

					case Keys.SongsLayoutType when value is LibraryLayoutTypes songslayouttype:
						SongsLayoutType = songslayouttype;
						break;

					case Keys.SongsSortKey when value is ModelSortKeys songssortkey:
						SongsSortKey = songssortkey;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : ISettings.Default<T>, IArtistSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				AlbumsIsReversed = defaultvalue;
				AlbumsLayoutType = defaultvalue;
				AlbumsSortKey = defaultvalue; 
				SongsIsReversed = defaultvalue;
				SongsLayoutType = defaultvalue;
				SongsSortKey = defaultvalue;
			}

			public T AlbumsIsReversed { get; set; }
			public T AlbumsLayoutType { get; set; }
			public T AlbumsSortKey { get; set; }	
			public T SongsIsReversed { get; set; }
			public T SongsLayoutType { get; set; }
			public T SongsSortKey { get; set; }
		}
	}
}
