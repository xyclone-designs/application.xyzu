using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryPlaylists : ILibrary
	{
		IPlaylistsSettings Settings { get; set; }
	}
}
