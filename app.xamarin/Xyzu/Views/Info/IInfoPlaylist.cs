using Xyzu.Library.Models;

namespace Xyzu.Views.Info
{
	public interface IInfoPlaylist : IInfo
	{
		IPlaylist? Playlist { get; set; }

		void SetPlaylist(IPlaylist? playlist) { Playlist = playlist; }
	}
}
