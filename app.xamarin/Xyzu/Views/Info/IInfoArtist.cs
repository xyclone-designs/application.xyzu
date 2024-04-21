using Xyzu.Library.Models;

namespace Xyzu.Views.Info
{
	public interface IInfoArtist : IInfo
	{
		IArtist? Artist { get; set; }

		void SetArtist(IArtist? artist) { Artist = artist; }
	}
}
