using System.Collections.Generic;
using System.Linq;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public partial interface ISong : IModel
	{
		public new class SortKeys : IModel.SortKeys
		{
			public const ModelSortKeys Album = ModelSortKeys.Album;
			public const ModelSortKeys AlbumArtist = ModelSortKeys.AlbumArtist;
			public const ModelSortKeys Artist = ModelSortKeys.Artist;
			public const ModelSortKeys Bitrate = ModelSortKeys.Bitrate;
			public const ModelSortKeys DateAdded = ModelSortKeys.DateAdded;
			public const ModelSortKeys DateModified = ModelSortKeys.DateModified;
			public const ModelSortKeys DiscNumber = ModelSortKeys.DiscNumber;
			public const ModelSortKeys Duration = ModelSortKeys.Duration;
			public const ModelSortKeys Genre = ModelSortKeys.Genre;
			public const ModelSortKeys ReleaseDate = ModelSortKeys.ReleaseDate;
			public const ModelSortKeys Size = ModelSortKeys.Size;
			public const ModelSortKeys Title = ModelSortKeys.Title;
			public const ModelSortKeys TrackNumber = ModelSortKeys.TrackNumber;

			public new static IEnumerable<ModelSortKeys> AsEnumerable()
			{
				return IModel.SortKeys.AsEnumerable()
					.Append(Album)
					.Append(AlbumArtist)
					.Append(Artist)
					.Append(Bitrate)
					.Append(DateAdded)
					.Append(DateModified)
					.Append(DiscNumber)
					.Append(Duration)
					.Append(Genre)
					.Append(ReleaseDate)
					.Append(Size)
					.Append(Title)
					.Append(TrackNumber);
			}
		}
	}
}
