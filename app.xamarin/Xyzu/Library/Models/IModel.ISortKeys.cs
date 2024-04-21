using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public partial interface IModel
	{
		public class SortKeys
		{
			public const ModelSortKeys Rating = ModelSortKeys.Rating;

			public static IEnumerable<ModelSortKeys> AsEnumerable()
			{
				return Enumerable.Empty<ModelSortKeys>()
					.Append(ModelSortKeys.Rating);
			}
		}
	}
}
