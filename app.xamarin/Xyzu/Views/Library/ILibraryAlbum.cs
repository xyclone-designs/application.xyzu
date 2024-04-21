using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryAlbum : ILibrary
	{
		IAlbum? Album { get; set; }
		IAlbumSettings Settings { get; set; }
	}
}
