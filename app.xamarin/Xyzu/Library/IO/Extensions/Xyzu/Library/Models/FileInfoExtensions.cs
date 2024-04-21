using System.IO;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public static class FileInfoExtensions
	{
		public static IAlbum Retrieve(this IAlbum retrieved, FileInfo fileinfo, IAlbum<bool>? retriever, bool overwrite = false)
		{
			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, FileInfo fileinfo, IArtist<bool>? retriever, bool overwrite = false)
		{
			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, FileInfo fileinfo, IGenre<bool>? retriever, bool overwrite = false)
		{
			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, FileInfo fileinfo, IPlaylist<bool>? retriever, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, FileInfo fileinfo, ISong<bool>? retriever, bool overwrite = false)
		{
			if (retriever?.DateAdded ?? true && (retrieved.DateAdded is null || overwrite)) 
				retrieved.DateAdded = fileinfo.CreationTime;
			
			if (retriever?.DateModified ?? true && (retrieved.DateModified is null || overwrite)) 
				retrieved.DateModified = fileinfo.LastWriteTime;
			
			if (retriever?.Filepath ?? true && (retrieved.Filepath is null || overwrite)) 
				retrieved.Filepath = fileinfo.FullName;
			
			if (retriever?.MimeType ?? true && (retrieved.MimeType is null || overwrite)) 
				retrieved.MimeType = default(MimeTypes).FromString(fileinfo.Extension);
			
			if (retriever?.Size ?? true && (retrieved.Size is null || overwrite)) 
				retrieved.Size = fileinfo.Length;

			return retrieved;
		}
	}
}
