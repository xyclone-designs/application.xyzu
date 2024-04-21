using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public partial interface IArtist
	{
		public new class SortKeys : IModel.SortKeys
		{
			public const ModelSortKeys Albums = ModelSortKeys.Albums;
			public const ModelSortKeys Duration = ModelSortKeys.Duration;
			public const ModelSortKeys Name = ModelSortKeys.Name;
			public const ModelSortKeys Songs = ModelSortKeys.Songs;

			public new static IEnumerable<ModelSortKeys> AsEnumerable()
			{
				return IModel.SortKeys.AsEnumerable()
					.Append(Albums)
					.Append(Duration)
					.Append(Name)
					.Append(Rating)
					.Append(Songs);
			}

			public class AlbumSortKeys : IAlbum.SortKeys { }
			public class SongSortKeys : ISong.SortKeys { }
		}
	}
}
