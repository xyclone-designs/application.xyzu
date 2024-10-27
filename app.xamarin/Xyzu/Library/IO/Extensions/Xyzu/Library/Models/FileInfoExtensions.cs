using System.IO;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public static class FileInfoExtensions
	{
		public static IAlbum Retrieve(this IAlbum retrieved, FileInfo fileinfo, bool overwrite = false)
		{
			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, FileInfo fileinfo, bool overwrite = false)
		{
			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, FileInfo fileinfo, bool overwrite = false)
		{
			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, FileInfo fileinfo, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, FileInfo fileinfo, bool overwrite = false)
		{
			if ((retrieved.Filepath is null || overwrite)) 
				retrieved.Filepath = fileinfo.FullName;

			if ((retrieved.MimeType is null || overwrite)) 
				retrieved.MimeType = default(MimeTypes).FromString(fileinfo.Extension);
			
			if ((retrieved.Size is null || overwrite)) 
				retrieved.Size = fileinfo.Length;

			return retrieved;
		}
	}
}
