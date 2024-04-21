using System;
using System.Collections.Generic;
using System.Linq;

namespace Id3.Frames
{
	public class PictureTypeComparer : IComparer<PictureType>
	{
		public PictureTypeComparer()
		{
			Strict = false;
		}

		public bool Strict { get; set; }
		public IEnumerable<PictureType>? Order { get; set; }

		public int Compare(PictureType x, PictureType y)
		{
			IEnumerable<PictureType>? picturetypesorder = Order;

			if (picturetypesorder is null)
				return Comparer<PictureType>.Default.Compare(x, y);
			else if (!Strict)
			{
				IEnumerable<PictureType> picturetypes = Enum
					.GetValues(typeof(PictureType))
					.Cast<PictureType>()
					.Where(picturetype => !Order.Contains(picturetype));

				picturetypesorder = picturetypesorder.Concat(picturetypes);
			}

			int? xposition = picturetypesorder.Index(x);	
			int? yposition = picturetypesorder.Index(y);

			return Comparer<int?>.Default.Compare(xposition, xposition);
		}

		public static PictureTypeComparer ForAlbumArtwork(bool strict = false)
		{
			return new PictureTypeComparer
			{
				Strict = strict,
				Order = Enumerable.Empty<PictureType>()
					.Append(PictureType.FrontCover)
					.Append(PictureType.Illustration)
					.Append(PictureType.BackCover),
			};
		}
		public static PictureTypeComparer ForArtistImage(bool strict = false)
		{
			return new PictureTypeComparer
			{
				Strict = strict,
				Order = Enumerable.Empty<PictureType>()
					.Append(PictureType.ArtistOrPerformer)
					.Append(PictureType.BandOrOrchestra)
					.Append(PictureType.BandOrArtistLogotype)
					.Append(PictureType.LeadArtistPerformerSoloist),
			};
		}
		public static PictureTypeComparer ForSongArtwork(bool strict = false)
		{
			return new PictureTypeComparer
			{
				Strict = strict,
				Order = Enumerable.Empty<PictureType>()
					.Append(PictureType.FrontCover)
					.Append(PictureType.Illustration)
					.Append(PictureType.BackCover)
					.Append(PictureType.Media)
					.Append(PictureType.ArtistOrPerformer),
			};
		}
	}
}
