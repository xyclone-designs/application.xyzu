using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Menus
{
	public class LibraryAlbum : LibraryItem.Album
	{
		public const MenuOptions Rescan = MenuOptions.Rescan;

		public new static IEnumerable<MenuOptions> AsEnumerable()
		{
			return LibraryItem.Genre.AsEnumerable()
				.Append(Rescan);
		}
	}
}