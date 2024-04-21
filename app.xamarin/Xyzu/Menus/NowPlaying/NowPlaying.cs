using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Menus
{
	public partial class NowPlaying 
	{
		public const MenuOptions AudioEffects = MenuOptions.AudioEffects;
		public const MenuOptions Delete = MenuOptions.Delete;
		public const MenuOptions EditInfo = MenuOptions.EditInfo;
		public const MenuOptions GoToAlbum = MenuOptions.GoToAlbum;
		public const MenuOptions GoToArtist = MenuOptions.GoToArtist;
		public const MenuOptions GoToGenre = MenuOptions.GoToGenre;
		public const MenuOptions GoToQueue = MenuOptions.GoToQueue;
		public const MenuOptions Settings = MenuOptions.Settings;
		public const MenuOptions Share = MenuOptions.Share;
		public const MenuOptions ViewInfo = MenuOptions.ViewInfo;

		public static IEnumerable<MenuOptions> AsEnumerable()
		{
			return Enumerable.Empty<MenuOptions>()
				.Append(AudioEffects)
				.Append(ViewInfo)
				.Append(EditInfo)
				.Append(Share)
				.Append(Delete)
				.Append(GoToAlbum)
				.Append(GoToArtist)
				.Append(GoToGenre)
				.Append(GoToQueue)
				.Append(Settings);
		}
	}
}