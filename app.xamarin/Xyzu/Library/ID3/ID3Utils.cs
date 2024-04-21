using Xyzu.Library.Models;

namespace Xyzu.Library.ID3
{
	public partial class ID3Utils
	{
		public static bool WouldBenefit(IAlbum? retrieved, IAlbum<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return
				(retrieved.Artist is null && (retriever?.Artist ?? true)) ||
				(retriever?.Duration ?? true) ||
				(retrieved.ReleaseDate is null && (retriever?.ReleaseDate ?? true)) ||
				(retrieved.Title is null && (retriever?.Title ?? true));
		}
		public static bool WouldBenefit(IArtist? retrieved, IArtist<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return
				(retrieved.Image is null && (retriever?.Image?.Buffer ?? true) || (retriever?.Image?.BufferHash ?? true)) ||
				(retrieved.Name is null && (retriever?.Name ?? true));
		}
		public static bool WouldBenefit(IGenre? retrieved, IGenre<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return
				retrieved.Name is null && (retriever?.Name ?? true);
		}
		public static bool WouldBenefit(IPlaylist? retrieved, IPlaylist<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return false;
		}
		public static bool WouldBenefit(ISong? retrieved, ISong<bool>? retriever) 
		{
			if (retrieved is null)
				return false;

			return
				(retrieved.Album is null && (retriever?.Album ?? true)) ||
				(retrieved.Artist is null && (retriever?.Artist ?? true)) ||
				(retrieved.Duration is null && (retriever?.Duration ?? true)) ||
				(retrieved.Genre is null && (retriever?.Genre ?? true)) ||
				(retrieved.Lyrics is null && (retriever?.Lyrics ?? true)) ||
				(retrieved.Bitrate is null && (retriever?.Bitrate ?? false)) ||
				(retrieved.Filepath is null && (retriever?.Filepath ?? false)) ||
				(retrieved.MimeType is null && (retriever?.MimeType ?? false)) ||
				(retrieved.ReleaseDate is null && (retriever?.ReleaseDate ?? true)) ||
				(retrieved.Title is null && (retriever?.Title?? true)) ||
				(retrieved.TrackNumber is null && (retriever?.TrackNumber ?? true));
		}
	}
}
