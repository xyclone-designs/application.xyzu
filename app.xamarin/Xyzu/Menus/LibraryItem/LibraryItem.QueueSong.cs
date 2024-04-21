using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Menus
{
	public partial class LibraryItem
	{
		public class QueueSong
		{
			public const MenuOptions AddToPlaylist = Song.AddToPlaylist;
			public const MenuOptions Delete = Song.Delete;
			public const MenuOptions EditInfo = Song.EditInfo;
			public const MenuOptions GoToAlbum = Song.GoToAlbum;
			public const MenuOptions GoToArtist = Song.GoToArtist;
			public const MenuOptions GoToGenre = Song.GoToGenre;
			public const MenuOptions MoveDown = MenuOptions.MoveDown;
			public const MenuOptions MoveToBottom = MenuOptions.MoveToBottom;
			public const MenuOptions MoveToTop = MenuOptions.MoveToTop;
			public const MenuOptions MoveUp = MenuOptions.MoveUp;
			public const MenuOptions Play = Song.Play;
			public const MenuOptions Remove = MenuOptions.Remove;
			public const MenuOptions Share = Song.Share;
			public const MenuOptions ViewInfo = Song.ViewInfo;

			public static IEnumerable<MenuOptions> AsEnumerable()
			{
				return Enumerable.Empty<MenuOptions>()
					.Append(Play)
					.Append(MoveDown)
					.Append(MoveToBottom)
					.Append(MoveToTop)
					.Append(Remove)
					.Append(ViewInfo)
					.Append(EditInfo)
					.Append(Share)
					.Append(Delete)
					.Append(AddToPlaylist)
					.Append(GoToAlbum)
					.Append(GoToArtist)
					.Append(GoToGenre);
			}
		}
	}
}