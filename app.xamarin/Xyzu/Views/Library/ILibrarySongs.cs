using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibrarySongs : ILibrary
	{	
		ISongsSettings Settings { get; set; }
	}
}
