using Xyzu.Settings.UserInterface.Library;

namespace Xyzu.Views.Library
{
	public interface ILibraryQueue : ILibrary
	{
		IQueueSettings Settings { get; set; }
	}
}
