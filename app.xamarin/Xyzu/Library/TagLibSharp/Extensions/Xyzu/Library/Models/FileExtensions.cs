using TagLib;

using System;
using System.Linq;

using Xyzu.Images;
using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public static class FileExtensions
	{
		public static IAlbum Retrieve(this IAlbum retrieved, File file, bool overwrite = false)
		{
			if ((retrieved.Artist is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedAlbumArtists) is false)
				retrieved.Artist = file.Tag.JoinedAlbumArtists.Trim();

			if ((retrieved.ReleaseDate is null || overwrite) && int.TryParse(file.Tag.Year.ToString(), out int year) && year is not 0) 
				retrieved.ReleaseDate = new DateTime(year, 1, 1);  							

			if ((retrieved.DiscCount is null || overwrite) && int.TryParse(file.Tag.DiscCount.ToString(), out int disccount)) 
				retrieved.DiscCount = disccount;  
																   
			if ((retrieved.Title is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Album) is false) 
				retrieved.Title = file.Tag.Album;	

			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, File file, bool overwrite = false)
		{
			if ((retrieved.Name is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedPerformers) is false)
				retrieved.Name = file.Tag.JoinedPerformers.Trim();

			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, File file, bool overwrite = false)
		{
			if ((retrieved.Name is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedGenres) is false) 
				retrieved.Name = file.Tag.JoinedGenres.Trim();

			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, File file, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, File file, bool overwrite = false)
		{
			if ((retrieved.Album is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Album) is false)
				retrieved.Album = file.Tag.Album;   
			
			if ((retrieved.AlbumArtist is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedAlbumArtists) is false)
				retrieved.AlbumArtist = file.Tag.JoinedAlbumArtists.Trim();					
			
			if ((retrieved.Artist is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedPerformers) is false)
				retrieved.Artist = file.Tag.JoinedPerformers.Trim();																		 
			
			if ((retrieved.Copyright is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Copyright) is false)
				retrieved.Copyright = file.Tag.Copyright.Trim();
			
			if ((retrieved.DiscNumber is null || overwrite) && int.TryParse(file.Tag.Disc.ToString(), out int discnumber) && discnumber is not 0)
				retrieved.DiscNumber = discnumber;
												
			if ((retrieved.Genre is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedGenres) is false)
				retrieved.Genre = file.Tag.JoinedGenres.Trim();
			
			if ((retrieved.Lyrics is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Lyrics) is false)
				retrieved.Lyrics = file.Tag.Lyrics;
			
			if ((retrieved.ReleaseDate is null || overwrite) && int.TryParse(file.Tag.Year.ToString(), out int year) && year is not 0)
				retrieved.ReleaseDate = new DateTime(year, 1, 1);
			
			if ((retrieved.Title is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Title) is false)
				retrieved.Title = file.Tag.Title;
			
			if ((retrieved.TrackNumber is null || overwrite) && int.TryParse(file.Tag.Track.ToString(), out int tracknumber) && tracknumber is not 0)
				retrieved.TrackNumber = tracknumber;		
			
			if ((retrieved.Size is null || overwrite) && file.Length is not 0)
				retrieved.Size = file.Length;

			if (file.Properties is not null)
			{
				if ((retrieved.Bitrate is null || overwrite) && file.Properties.AudioBitrate is not 0)
					retrieved.Bitrate = file.Properties.AudioBitrate;

				if ((retrieved.Duration is null || overwrite) && file.Properties.Duration != TimeSpan.Zero)
					retrieved.Duration = file.Properties.Duration;
			}

			return retrieved;
		}
		public static IImage Retrieve(this IImage retrieved, File file, PictureTypeComparer? picturetypecomparer)
		{
			IPicture? picture = file.Tag.Pictures
				.OrderBy(picture => picture.Type, picturetypecomparer ?? new PictureTypeComparer())
				.FirstOrDefault();

			if (picture?.Data.Data is byte[] picturedata)
			{
				retrieved.Buffer = picturedata;
				retrieved.BufferKey = IImage.Utils.BufferToHashString(picturedata);
			}

			return retrieved;
		}
	}
}
