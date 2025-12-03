using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface
{
	public interface IUserInterfaceSettingsDroid<T> : IUserInterfaceSettings<T>
	{
		T HeaderScrollType { get; set; }
		T NavigationType { get; set; }
		T NowPlayingForceShow { get; set; }
	}
	public interface IUserInterfaceSettingsDroid : IUserInterfaceSettings
	{
		public new class Defaults : IUserInterfaceSettings.Defaults
		{
			public const LibraryHeaderScrollTypes HeaderScrollType = LibraryHeaderScrollTypes.Scroll;
			public const LibraryNavigationTypes NavigationType = LibraryNavigationTypes.Tabs;
			public const bool NowPlayingForceShow = false;

			public static readonly IUserInterfaceSettingsDroid LibrarySettingsDroid = new Default
			{
				HeaderScrollType = HeaderScrollType,
				NavigationType = NavigationType,
				PageDefault = PageDefault,
				PagesOrdered = PagesOrdered(),
			};
		}
		public new class Keys : IUserInterfaceSettings.Keys
		{
			public const string HeaderScrollType = Base + "." + nameof(HeaderScrollType);
			public const string NavigationType = Base + "." + nameof(NavigationType);
			public const string NowPlayingForceShow = Base + "." + nameof(NowPlayingForceShow);
		}
		public new class Options : IUserInterfaceSettings.Options
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
		bool NowPlayingForceShow { get; set; }

		public new class Default : IUserInterfaceSettings.Default, IUserInterfaceSettingsDroid
		{
			public LibraryHeaderScrollTypes HeaderScrollType { get; set; }
			public LibraryNavigationTypes NavigationType { get; set; }
			public bool NowPlayingForceShow { get; set; }
		}
		public new class Default<T> : IUserInterfaceSettings.Default<T>, IUserInterfaceSettingsDroid<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				HeaderScrollType = defaultvalue;
				NavigationType = defaultvalue;
				NowPlayingForceShow = defaultvalue;
			}

			public T HeaderScrollType { get; set; }
			public T NavigationType { get; set; }
			public T NowPlayingForceShow { get; set; }
		}
	}
}
