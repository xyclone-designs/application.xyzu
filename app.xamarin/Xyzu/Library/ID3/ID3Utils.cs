using Xyzu.Library.Models;

namespace Xyzu.Library.ID3
{
	public partial class ID3Utils
	{
		public static bool WouldBenefit(IAlbum? retrieved)
		{
			if (retrieved is null)
				return false;

			return
				retrieved.Artist is null ||
				retrieved.ReleaseDate is null ||
				retrieved.Title is null;
		}
		public static bool WouldBenefit(IArtist? retrieved)
		{
			if (retrieved is null)
				return false;

			return
				retrieved.Image is null ||
				retrieved.Name is null;
		}
		public static bool WouldBenefit(IGenre? retrieved)
		{
			if (retrieved is null)
				return false;

			return
				retrieved.Name is null;
		}
		public static bool WouldBenefit(IPlaylist? retrieved)
		{
			if (retrieved is null)
				return false;

			return false;
		}
		public static bool WouldBenefit(ISong? retrieved) 
		{
			if (retrieved is null)
				return false;

			return
				retrieved.Album is null ||
				retrieved.Artist is null ||
				retrieved.Duration is null ||
				retrieved.Genre is null ||
				retrieved.Lyrics is null ||
				retrieved.Bitrate is null ||
				retrieved.Filepath is null ||
				retrieved.MimeType is null ||
				retrieved.ReleaseDate is null ||
				retrieved.Title is null ||
				retrieved.TrackNumber is null;
		}
	}
}
