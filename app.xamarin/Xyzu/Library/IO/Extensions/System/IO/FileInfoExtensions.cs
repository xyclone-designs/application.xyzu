using Xyzu.Library.Models;

namespace System.IO
{
	// TODO FileInfoExtensions

	public static class FileInfoExtensions
	{
		public static bool ShouldRetrieveAlbum(this FileInfo fileInfo, IAlbum album)
		{
			return false;
		}				 
		public static bool ShouldRetrieveArtist(this FileInfo fileInfo, IArtist artist)
		{
			return false;
		}				 
		public static bool ShouldRetrieveGenre(this FileInfo fileInfo, IGenre genre)
		{
			return false;
		}				 
		public static bool ShouldRetrievePlaylist(this FileInfo fileInfo, IPlaylist playlist)
		{
			return false;
		}				 
		public static bool ShouldRetrieveSong(this FileInfo fileInfo, ISong song)
		{
            return false;
		}
	}
}
