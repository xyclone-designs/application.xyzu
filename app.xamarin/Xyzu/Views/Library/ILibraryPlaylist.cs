using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryPlaylist : ILibrary
	{
		IPlaylist? Playlist { get; set; }
		IPlaylistSettings Settings { get; set; }
	}
}
