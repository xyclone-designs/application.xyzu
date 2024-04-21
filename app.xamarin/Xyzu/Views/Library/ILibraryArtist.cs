using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryArtist : ILibrary
	{
		IArtist? Artist { get; set; }
		IArtistSettings Settings { get; set; }
	}
}
