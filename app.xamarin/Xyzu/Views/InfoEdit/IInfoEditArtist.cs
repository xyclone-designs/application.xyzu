using Xyzu.Library.Models;

namespace Xyzu.Views.InfoEdit
{
	public interface IInfoEditArtist : IInfoEdit
	{
		IArtist? Artist { get; set; }

		void SetArtist(IArtist? artist) { Artist = artist; }
	}
}
