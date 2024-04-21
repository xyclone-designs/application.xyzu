using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Menus
{
	public partial class LibraryItem
	{
		public class ArtistAlbum
		{
			public const MenuOptions AddToQueue = Album.AddToQueue;
			public const MenuOptions AddToPlaylist = Album.AddToPlaylist;
			public const MenuOptions Delete = Album.Delete;
			public const MenuOptions EditInfo = Album.EditInfo;
			public const MenuOptions GoToAlbum = Album.GoToAlbum;
			public const MenuOptions Play = Album.Play;
			public const MenuOptions Share = Album.Share;
			public const MenuOptions ViewInfo = Album.ViewInfo;

			public static IEnumerable<MenuOptions> AsEnumerable()
			{
				return Enumerable.Empty<MenuOptions>()
					.Append(Play)
					.Append(ViewInfo)
					.Append(EditInfo)
					.Append(Share)
					.Append(Delete)
					.Append(AddToQueue)
					.Append(AddToPlaylist)
					.Append(GoToAlbum);
			}
		}
	}
}