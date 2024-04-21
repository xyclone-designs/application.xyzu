using Xyzu.Library.Models;

namespace Xyzu.Views.InfoEdit
{
	public interface IInfoEditAlbum : IInfoEdit
	{
		IAlbum? Album { get; set; }

		void SetAlbum(IAlbum? album) { Album = album; }
	}
}
