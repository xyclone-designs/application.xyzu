using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public partial interface IPlaylist
	{
		public new class SortKeys : IModel.SortKeys
		{
			public const ModelSortKeys DateAdded = ModelSortKeys.DateAdded;
			public const ModelSortKeys DateCreated = ModelSortKeys.DateCreated;
			public const ModelSortKeys DateModified = ModelSortKeys.DateModified;
			public const ModelSortKeys Duration = ModelSortKeys.Duration;
			public const ModelSortKeys Songs = ModelSortKeys.Songs;
			public const ModelSortKeys Title = ModelSortKeys.Title;

			public new static IEnumerable<ModelSortKeys> AsEnumerable()
			{
				return IModel.SortKeys.AsEnumerable()
					.Append(DateAdded)
					.Append(DateCreated)
					.Append(DateModified)
					.Append(Duration)
					.Append(Songs)
					.Append(Title);
			}

			public class SongSortKeys : ISong.SortKeys
			{
				public const ModelSortKeys Position = ModelSortKeys.Position;

				public new static IEnumerable<ModelSortKeys> AsEnumerable()
				{
					return ISong.SortKeys.AsEnumerable()
						.Append(Position);
				}
			}
		}
	}
}
