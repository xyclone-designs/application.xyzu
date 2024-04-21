using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface IAlbumsSettings<T> : IUserInterfaceSettings<T> 
	{
		T IsReversed { get; set; }
		T LayoutType { get; set; }
		T SortKey { get; set; }
	}
	public interface IAlbumsSettings : IUserInterfaceSettings
	{
		bool IsReversed { get; set; }
		LibraryLayoutTypes LayoutType { get; set; }
		ModelSortKeys SortKey { get; set; }

		public new class Defaults : IUserInterfaceSettings.Defaults
		{
			public static readonly bool IsReversed = false;
			public static readonly LibraryLayoutTypes LayoutType = LibraryLayoutTypes.GridMedium;
			public static readonly ModelSortKeys SortKey = ModelSortKeys.Title;

			public static readonly IAlbumsSettings AlbumsSettings = new Default
			{
				IsReversed = IsReversed,
				LayoutType = LayoutType,
				SortKey = SortKey,
			};
		}
		public new class Keys : IUserInterfaceSettings.Keys
		{
			public new const string Base = IUserInterfaceSettings.Keys.Base + "." + nameof(IAlbumsSettings);

			public const string IsReversed = Base + "." + nameof(IsReversed);
			public const string LayoutType = Base + "." + nameof(LayoutType);
			public const string SortKey = Base + "." + nameof(SortKey);
		}
		public new class Options : IUserInterfaceSettings.Options
		{
			public class LayoutTypes : LibraryDefaultSettings.Options.LayoutTypes { }
			public class SortKeys : IAlbum.SortKeys { }
		}

		public new class Default : IUserInterfaceSettings.Default, IAlbumsSettings
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

			public bool IsReversed
			{
				get => _IsReversed;
				set
				{
					_IsReversed = value;

					OnPropertyChanged();
				}
			}
			public LibraryLayoutTypes LayoutType
			{
				get => _LayoutType;
				set
				{
					_LayoutType = value;

					OnPropertyChanged();
				}
			}
			public ModelSortKeys SortKey
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
						IsReversed = isreversed;
						break;

					case Keys.LayoutType when value is LibraryLayoutTypes layouttype:
						LayoutType = layouttype;
						break;

					case Keys.SortKey when value is ModelSortKeys sortkey:
						SortKey = sortkey;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : IUserInterfaceSettings.Default<T>, IAlbumsSettings<T>
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
