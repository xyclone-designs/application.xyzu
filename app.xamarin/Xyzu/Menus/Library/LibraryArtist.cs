using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Menus
{
	public class LibraryArtist : LibraryItem.Artist
	{
		public const MenuOptions Rescan = MenuOptions.Rescan;

		public new static IEnumerable<MenuOptions> AsEnumerable()
		{
			return LibraryItem.Genre.AsEnumerable()
				.Append(Rescan);
		}
	}
}