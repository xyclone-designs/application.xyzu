using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public class LibraryDefaultSettings
	{
		public class Options 
		{
			public class LayoutTypes
			{
				public const LibraryLayoutTypes GridLarge = LibraryLayoutTypes.GridLarge;
				public const LibraryLayoutTypes GridMedium = LibraryLayoutTypes.GridMedium;
				public const LibraryLayoutTypes GridSmall = LibraryLayoutTypes.GridSmall;
				public const LibraryLayoutTypes ListLarge = LibraryLayoutTypes.ListLarge;
				public const LibraryLayoutTypes ListMedium = LibraryLayoutTypes.ListMedium;
				public const LibraryLayoutTypes ListSmall = LibraryLayoutTypes.ListSmall;

				public static IEnumerable<LibraryLayoutTypes> AsEnumerable()
				{
					return Enum
						.GetValues(typeof(LibraryLayoutTypes))
						.Cast<LibraryLayoutTypes>();
				}
			}
		}
	}
}
