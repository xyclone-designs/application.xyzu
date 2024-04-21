using Xyzu.Library.Models;

namespace Xyzu.Views.InfoEdit
{
	public interface IInfoEditPlaylist : IInfoEdit
	{
		IPlaylist? Playlist { get; set; }

		void SetPlaylist(IPlaylist? playlist) { Playlist = playlist; }
	}
}
