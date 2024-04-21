﻿using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Menus
{
	public partial class LibraryItem
	{
		public class Genre
		{
			public const MenuOptions AddToQueue = MenuOptions.AddToQueue;
			public const MenuOptions AddToPlaylist = MenuOptions.AddToPlaylist;
			public const MenuOptions Delete = MenuOptions.Delete;
			public const MenuOptions EditInfo = MenuOptions.EditInfo;
			public const MenuOptions GoToGenre = MenuOptions.GoToGenre;
			public const MenuOptions Play = MenuOptions.Play;
			public const MenuOptions Share = MenuOptions.Share;
			public const MenuOptions ViewInfo = MenuOptions.ViewInfo;

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
					.Append(GoToGenre);
			}
		}
	}
}