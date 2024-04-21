using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryAlbums : ILibrary
	{
		IAlbumsSettings Settings { get; set; }
	}
}
