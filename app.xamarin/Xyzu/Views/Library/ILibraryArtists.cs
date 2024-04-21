using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryArtists : ILibrary
	{
		IArtistsSettings Settings { get; set; }
	}
}
