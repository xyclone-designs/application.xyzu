using Xyzu.Library.Models;

namespace Xyzu.Library.IO
{
	public partial class IOUtils 
	{
		public static bool WouldBenefit(IAlbum? retrieved, IAlbum<bool>? retriever)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(IArtist? retrieved, IArtist<bool>? retriever)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(IGenre? retrieved, IGenre<bool>? retriever)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(IPlaylist? retrieved, IPlaylist<bool>? retriever)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(ISong? retrieved, ISong<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return
				retrieved.DateAdded is null && (retriever?.DateAdded ?? false) ||
				retrieved.DateModified is null && (retriever?.DateModified ?? false) ||
				retrieved.Filepath is null && (retriever?.Filepath ?? false) ||
				retrieved.MimeType is null && (retriever?.MimeType ?? false) ||
				retrieved.Size is null && (retriever?.Size ?? false);
		}
	}
}
