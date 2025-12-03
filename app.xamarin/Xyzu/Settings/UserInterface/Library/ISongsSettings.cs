using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface ISongsSettings<T> : ISettings<T>
	{
		T IsReversed { get; set; }
		T LayoutType { get; set; }
		T SortKey { get; set; }
	}
	public interface ISongsSettings : ISettings
	{
		bool SongsIsReversed { get; set; }
		LibraryLayoutTypes SongsLayoutType { get; set; }
		ModelSortKeys SongsSortKey { get; set; }

		public new class Defaults : ISettings.Defaults
		{
			public static readonly bool IsReversed = false;
			public static readonly LibraryLayoutTypes LayoutType = LibraryLayoutTypes.ListMedium;
			public static readonly ModelSortKeys SortKey = ModelSortKeys.Title;

			public static readonly ISongsSettings SongsSettings = new Default
			{
				SongsIsReversed = IsReversed,
				SongsLayoutType = LayoutType,
				SongsSortKey = SortKey,
			};
		}
		public new class Keys : ISettings.Keys
		{
			public new const string Base = IUserInterfaceSettings.Keys.Base + "." + nameof(ISongsSettings);

			public const string IsReversed = Base + "." + nameof(IsReversed);
			public const string LayoutType = Base + "." + nameof(LayoutType);
			public const string SortKey = Base + "." + nameof(SortKey);
		}
		public new class Options : ISettings.Options
		{
			public class LayoutTypes : LibraryDefaultSettings.Options.LayoutTypes { }
			public class SortKeys : ISong.SortKeys { }
		}

		public new class Default : ISettings.Default, ISongsSettings
		{
			public Default()
			{
				SongsIsReversed = Defaults.IsReversed;
				SongsLayoutType = Defaults.LayoutType;
				SongsSortKey = Defaults.SortKey;
			}

			protected bool _IsReversed;
			protected LibraryLayoutTypes _LayoutType;
			protected ModelSortKeys _SortKey;

			public bool SongsIsReversed
			{
				get => _IsReversed;
				set
				{
					_IsReversed = value;

					OnPropertyChanged();
				}
			}
			public LibraryLayoutTypes SongsLayoutType
			{
				get => _LayoutType;
				set
				{
					_LayoutType = value;

					OnPropertyChanged();
				}
			}
			public ModelSortKeys SongsSortKey
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
						SongsIsReversed = isreversed;
						break;
								  
					case Keys.LayoutType when value is LibraryLayoutTypes layouttype:
						SongsLayoutType = layouttype;
						break;
								  
					case Keys.SortKey when value is ModelSortKeys sortkey:
						SongsSortKey = sortkey;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : ISettings.Default<T>, ISongsSettings<T>
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
