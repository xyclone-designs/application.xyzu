using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface IPlaylistsSettings<T> : ISettings<T>
	{
		T IsReversed { get; set; }
		T LayoutType { get; set; }
		T SortKey { get; set; }
	}
	public interface IPlaylistsSettings : ISettings
	{
		bool PlaylistsIsReversed { get; set; }
		LibraryLayoutTypes PlaylistsLayoutType { get; set; }
		ModelSortKeys PlaylistsSortKey { get; set; }

		public new class Defaults : ISettings.Defaults
		{
			public static readonly bool IsReversed = false;
			public static readonly LibraryLayoutTypes LayoutType = LibraryLayoutTypes.ListMedium;
			public static readonly ModelSortKeys SortKey = ModelSortKeys.Title;

			public static readonly IPlaylistsSettings PlaylistsSettings = new Default
			{
				PlaylistsIsReversed = IsReversed,
				PlaylistsLayoutType = LayoutType,
				PlaylistsSortKey = SortKey,
			};
		}
		public new class Keys : ISettings.Keys
		{
			public new const string Base = IUserInterfaceSettings.Keys.Base + "." + nameof(IPlaylistsSettings);

			public const string IsReversed = Base + "." + nameof(IsReversed);
			public const string LayoutType = Base + "." + nameof(LayoutType);
			public const string SortKey = Base + "." + nameof(SortKey);
		}
		public new class Options : ISettings.Options
		{
			public class LayoutTypes : LibraryDefaultSettings.Options.LayoutTypes { }
			public class SortKeys : IPlaylist.SortKeys { }
		}

		public new class Default : ISettings.Default, IPlaylistsSettings
		{
			public Default()
			{
				_IsReversed = Defaults.IsReversed;
				_LayoutType = Defaults.LayoutType;
				_SortKey = Defaults.SortKey;
			}

			protected bool _IsReversed;
			protected LibraryLayoutTypes _LayoutType;
			protected ModelSortKeys _SortKey;

			public bool PlaylistsIsReversed
			{
				get => _IsReversed;
				set
				{
					_IsReversed = value;

					OnPropertyChanged();
				}
			}
			public LibraryLayoutTypes PlaylistsLayoutType
			{
				get => _LayoutType;
				set
				{
					_LayoutType = value;

					OnPropertyChanged();
				}
			}
			public ModelSortKeys PlaylistsSortKey
			{
				get => _SortKey;
				set
				{
					_SortKey = value;

					OnPropertyChanged();
				}
			}

			public override void SetFromKey(string key, object? value)
			{
				base.SetFromKey(key, value);

				switch (key)
				{
					case Keys.IsReversed when value is bool isreversed:
						PlaylistsIsReversed = isreversed;
						break;

					case Keys.LayoutType when value is LibraryLayoutTypes layouttype:
						PlaylistsLayoutType = layouttype;
						break;

					case Keys.SortKey when value is ModelSortKeys sortkey:
						PlaylistsSortKey = sortkey;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : ISettings.Default<T>, IPlaylistsSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				IsReversed = defaultvalue;
				LayoutType = defaultvalue;
				SortKey = defaultvalue;
			}

			public T IsReversed { get; set; }
			public T LayoutType { get; set; }
			public T SortKey { get; set; }
		}
	}
}
