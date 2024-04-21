
namespace Xyzu.Library.Models
{
	public partial interface IArtist : IModel
{
		public void Populate(IArtist artist)
		{
			AlbumIds ??= artist.AlbumIds;
			Name ??= artist.Name;
			SongIds ??= artist.SongIds;
		}
		public IArtist<bool>? Distinct(IArtist compareto, IArtist<bool>? fieldtocompare = null)
		{
			bool distinctname = (Name is not null || compareto.Name is not null) && !string.Equals(Name, compareto.Name);
			bool distinctrating = (Rating != compareto.Rating);

			bool distinct =
				distinctname ||
				distinctrating;

			if (distinct is false)
				return null;

			return new Default<bool>(false)
			{
				Name = distinctname,
				Rating = distinctrating,
			};
		}
	}
}
