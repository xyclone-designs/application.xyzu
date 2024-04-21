
namespace Xyzu.Library.Models
{
	public partial interface IAlbum : IModel
	{
		public void Populate(IAlbum album)
		{
			ArtistId ??= album.ArtistId;
			Artist ??= album.Artist;
			DiscCount ??= album.DiscCount;
			Duration = album.Duration;
			ReleaseDate ??= album.ReleaseDate;
			SongIds ??= album.SongIds;
			Title ??= album.Title;
		}
		public IAlbum<bool>? Distinct(IAlbum compareto, IAlbum<bool>? fieldtocompare = null)
		{
			bool distinctartist = (Artist is not null || compareto.Artist is not null) && !Equals(Artist, compareto.Artist);
			bool distinctdisccount = (DiscCount is not null || compareto.DiscCount is not null) && !Equals(DiscCount, compareto.DiscCount);
			bool distinctduration = !Equals(Duration, compareto.Duration);
			bool distinctrating = !(Rating != compareto.Rating);
			bool distinctreleasedate = (ReleaseDate is not null || compareto.ReleaseDate is not null) && !(ReleaseDate != compareto.ReleaseDate);
			bool distincttitle = (Title is not null || compareto.Title is not null) && !string.Equals(Title, compareto.Title);

			bool distinct =
				distinctartist ||
				distinctdisccount ||
				distinctduration ||
				distinctreleasedate ||
				distincttitle;

			if (distinct is false)
				return null;

			return new Default<bool>(false)
			{
				Artist = distinctartist,
				DiscCount = distinctdisccount,
				Duration = distinctduration,
				ReleaseDate = distinctreleasedate,
				Rating = distinctrating,
				Title = distincttitle,
			};
		}
	}
}
