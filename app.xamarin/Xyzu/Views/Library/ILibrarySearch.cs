using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibrarySearch : ILibrary
	{
		ISearchSettings Settings { get; set; }
	}
}
