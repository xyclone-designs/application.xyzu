using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryGenre : ILibrary
	{
		IGenre? Genre { get; set; }
		IGenreSettings Settings { get; set; }
	}
}
