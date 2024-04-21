using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public partial interface IGenre
	{
		public new class SortKeys : IModel.SortKeys
		{
			public const ModelSortKeys Duration = ModelSortKeys.Duration;
			public const ModelSortKeys Name = ModelSortKeys.Name;
			public const ModelSortKeys Songs = ModelSortKeys.Songs;

			public new static IEnumerable<ModelSortKeys> AsEnumerable()
			{
				return IModel.SortKeys.AsEnumerable()
					.Append(Duration)
					.Append(Name)
					.Append(Songs);
			}

			public class SongSortKeys : ISong.SortKeys { }
		}
	}
}
