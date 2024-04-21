using Xyzu.Library.Models;

namespace Xyzu.Views.Info
{
	public interface IInfoGenre : IInfo
	{
		IGenre? Genre { get; set; }

		void SetGenre(IGenre? genre) { Genre = genre; }
	}
}
