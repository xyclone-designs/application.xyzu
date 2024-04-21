using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public partial interface IAlbum
	{
		public new class SortKeys : IModel.SortKeys
		{
			public const ModelSortKeys AlbumArtist = ModelSortKeys.AlbumArtist;
			public const ModelSortKeys Discs = ModelSortKeys.Discs;
			public const ModelSortKeys Duration = ModelSortKeys.Duration;
			public const ModelSortKeys ReleaseDate = ModelSortKeys.ReleaseDate;
			public const ModelSortKeys Songs = ModelSortKeys.Songs;
			public const ModelSortKeys Title = ModelSortKeys.Title;

			public new static IEnumerable<ModelSortKeys> AsEnumerable()
			{
				return IModel.SortKeys.AsEnumerable()
					.Append(AlbumArtist)
					.Append(Discs)
					.Append(Duration)
					.Append(ReleaseDate)
					.Append(Songs)
					.Append(Title);
			}

			public class SongSortKeys : ISong.SortKeys { }
		}
	}
}
