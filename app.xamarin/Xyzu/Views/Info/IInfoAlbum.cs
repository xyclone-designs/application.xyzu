using Xyzu.Library.Models;

namespace Xyzu.Views.Info
{
	public interface IInfoAlbum : IInfo
	{
		IAlbum? Album { get; set; }

		void SetAlbum(IAlbum? album) { Album = album; }
	}
}
