using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface IPlaylistSettings<T> : ISettings<T>
	{
		T SongsIsReversed { get; set; }
		T SongsLayoutType { get; set; }
		T SongsSortKey { get; set; }
	}
	public interface IPlaylistSettings : ISettings
	{
		public new class Defaults : ISettings.Defaults
		{
			public static readonly bool SongsIsReversed = false;
			public static readonly LibraryLayoutTypes SongsLayoutType = LibraryLayoutTypes.ListMedium;
			public static readonly ModelSortKeys SongsSortKey = ModelSortKeys.Position;

			public static readonly IPlaylistSettings PlaylistSettings = new Default
			{
				SongsIsReversed = SongsIsReversed,
				SongsLayoutType = SongsLayoutType,
				SongsSortKey = SongsSortKey,
			};
		}
		public new class Keys : ISettings.Keys
		{
			public new const string Base = IUserInterfaceSettings.Keys.Base + "." + nameof(IPlaylistSettings);

			public const string SongsIsReversed = Base + "." + nameof(SongsIsReversed);
			public const string SongsLayoutType = Base + "." + nameof(SongsLayoutType);
			public const string SongsSortKey = Base + "." + nameof(SongsSortKey);
		}
		public new class Options : ISettings.Options
		{
			public class SongsLayoutTypes : LibraryDefaultSettings.Options.LayoutTypes { }
			public class SongsSortKeys : IPlaylist.SortKeys.SongSortKeys { }
		}

		bool SongsIsReversed { get; set; }
		LibraryLayoutTypes SongsLayoutType { get; set; }
		ModelSortKeys SongsSortKey { get; set; }

		public new class Default : ISettings.Default, IPlaylistSettings
		{
			public Default()
			{
				_SongsIsReversed = Defaults.SongsIsReversed;
				_SongsLayoutType = Defaults.SongsLayoutType;
				_SongsSortKey = Defaults.SongsSortKey;
			}

			private bool _SongsIsReversed;
			private LibraryLayoutTypes _SongsLayoutType;
			private ModelSortKeys _SongsSortKey;

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
		public new class Default<T> : ISettings.Default<T>, IPlaylistSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				SongsIsReversed = defaultvalue;
				SongsLayoutType = defaultvalue;
				SongsSortKey = defaultvalue;
			}

			public T SongsIsReversed { get; set; }
			public T SongsLayoutType { get; set; }
			public T SongsSortKey { get; set; }
		}
	}
}
