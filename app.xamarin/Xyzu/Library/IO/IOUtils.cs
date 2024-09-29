using Xyzu.Library.Models;

namespace Xyzu.Library.IO
{
	public partial class IOUtils 
	{
		public static bool WouldBenefit(IAlbum? retrieved)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(IArtist? retrieved)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(IGenre? retrieved)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(IPlaylist? retrieved)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(ISong? retrieved)
		{
			if (retrieved is null)
				return false;

			return
				retrieved.DateAdded is null ||
				retrieved.DateModified is null ||
				retrieved.Filepath is null ||
				retrieved.MimeType is null ||
				retrieved.Size is null;
		}
	}
}
