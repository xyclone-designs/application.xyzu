﻿using Xyzu.Library.Enums;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface ISongsSettings<T> : IUserInterfaceSettings<T>
	{
		T IsReversed { get; set; }
		T LayoutType { get; set; }
		T SortKey { get; set; }
	}
	public interface ISongsSettings : IUserInterfaceSettings
	{
		bool IsReversed { get; set; }
		LibraryLayoutTypes LayoutType { get; set; }
		ModelSortKeys SortKey { get; set; }

		public new class Defaults : IUserInterfaceSettings.Defaults
		{
			public static readonly bool IsReversed = false;
			public static readonly LibraryLayoutTypes LayoutType = LibraryLayoutTypes.ListMedium;
			public static readonly ModelSortKeys SortKey = ModelSortKeys.Title;

			public static readonly ISongsSettings SongsSettings = new Default
			{
				IsReversed = IsReversed,
				LayoutType = LayoutType,
				SortKey = SortKey,
			};
		}
		public new class Keys : IUserInterfaceSettings.Keys
		{
			public new const string Base = ILibrarySettings.Keys.Base + "." + nameof(ISongsSettings);

			public const string IsReversed = Base + "." + nameof(IsReversed);
			public const string LayoutType = Base + "." + nameof(LayoutType);
			public const string SortKey = Base + "." + nameof(SortKey);
		}
		public new class Options : IUserInterfaceSettings.Options
		{
			public class LayoutTypes : LibraryDefaultSettings.Options.LayoutTypes { }
			public class SortKeys : ISong.SortKeys { }
		}

		public new class Default : IUserInterfaceSettings.Default, ISongsSettings
		{
			public Default()
			{
				IsReversed = Defaults.IsReversed;
				LayoutType = Defaults.LayoutType;
				SortKey = Defaults.SortKey;
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
		public new class Default<T> : IUserInterfaceSettings.Default<T>, ISongsSettings<T>
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