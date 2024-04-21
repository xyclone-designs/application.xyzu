using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface ILibrarySettingsDroid<T> : ILibrarySettings<T>
	{
		T HeaderScrollType { get; set; }
		T NavigationType { get; set; }
	}
	public interface ILibrarySettingsDroid : ILibrarySettings
	{
		public new class Defaults : ILibrarySettings.Defaults
		{
			public const LibraryHeaderScrollTypes HeaderScrollType = LibraryHeaderScrollTypes.Scroll;
			public const LibraryNavigationTypes NavigationType = LibraryNavigationTypes.Tabs;

			public static readonly ILibrarySettingsDroid LibrarySettingsDroid = new Default
			{
				HeaderScrollType = HeaderScrollType,
				NavigationType = NavigationType,
				PageDefault = LibrarySettings.PageDefault,
				PagesOrdered = LibrarySettings.PagesOrdered,
			};
		}
		public new class Keys : ILibrarySettings.Keys
		{
			public const string HeaderScrollType = Base + "." + nameof(HeaderScrollType);
			public const string NavigationType = Base + "." + nameof(NavigationType);
		}
		public new class Options : ILibrarySettings.Options
		{
			public class HeaderScrollTypes
			{
				public const LibraryHeaderScrollTypes PinToTop = LibraryHeaderScrollTypes.PinToTop;
				public const LibraryHeaderScrollTypes PinToTopScroll = LibraryHeaderScrollTypes.PinToTopScroll;
				public const LibraryHeaderScrollTypes Scroll = LibraryHeaderScrollTypes.Scroll;

				public static IEnumerable<LibraryHeaderScrollTypes> AsEnumerable()
				{
					return Enum
						.GetValues(typeof(LibraryHeaderScrollTypes))
						.Cast<LibraryHeaderScrollTypes>();
				}
			}			  
			public class NavigationTypes
			{
				public const LibraryNavigationTypes Drawer = LibraryNavigationTypes.Drawer;
				public const LibraryNavigationTypes Tabs = LibraryNavigationTypes.Tabs;

				public static IEnumerable<LibraryNavigationTypes> AsEnumerable()
				{
					return Enum
						.GetValues(typeof(LibraryNavigationTypes))
						.Cast<LibraryNavigationTypes>();
				}
			}
		}

		LibraryHeaderScrollTypes HeaderScrollType { get; set; }
		LibraryNavigationTypes NavigationType { get; set; }

		public new class Default : ILibrarySettings.Default, ILibrarySettingsDroid
		{
			public LibraryHeaderScrollTypes HeaderScrollType { get; set; }
			public LibraryNavigationTypes NavigationType { get; set; }
		}
		public new class Default<T> : ILibrarySettings.Default<T>, ILibrarySettingsDroid<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				HeaderScrollType = defaultvalue;
				NavigationType = defaultvalue;
			}

			public T HeaderScrollType { get; set; }
			public T NavigationType { get; set; }
		}
	}
}
