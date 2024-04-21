using Xyzu.Library.Models;

namespace Xyzu.Views.InfoEdit
{
	public interface IInfoEditGenre : IInfoEdit
	{
		IGenre? Genre { get; set; }

		void SetGenre(IGenre? genre) { Genre = genre; }
	}
}
