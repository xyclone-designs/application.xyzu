using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryGenres : ILibrary
	{
		IGenresSettings Settings { get; set; }
	}
}
