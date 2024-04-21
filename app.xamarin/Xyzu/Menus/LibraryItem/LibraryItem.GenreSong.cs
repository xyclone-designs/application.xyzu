using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Menus
{
	public partial class LibraryItem
	{
		public class GenreSong
		{
			public const MenuOptions AddToQueue = Song.AddToQueue;
			public const MenuOptions AddToPlaylist = Song.AddToPlaylist;
			public const MenuOptions Delete = Song.Delete;
			public const MenuOptions EditInfo = Song.EditInfo;
			public const MenuOptions GoToAlbum = Song.GoToAlbum;
			public const MenuOptions GoToArtist = Song.GoToArtist;
			public const MenuOptions Play = Song.Play;
			public const MenuOptions Share = Song.Share;
			public const MenuOptions ViewInfo = Song.ViewInfo;

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
					.Append(GoToAlbum)
					.Append(GoToArtist);
			}
		}
	}
}