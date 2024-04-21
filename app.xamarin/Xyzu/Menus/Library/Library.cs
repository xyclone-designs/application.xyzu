using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Menus
{
	public class Library 
	{
		public const MenuOptions ListOptions = MenuOptions.ListOptions;
		public const MenuOptions Search = MenuOptions.Search;

		public static IEnumerable<MenuOptions> AsEnumerable()
		{
			return Enumerable.Empty<MenuOptions>()
				.Append(Search)
				.Append(ListOptions);
		}
	}
}