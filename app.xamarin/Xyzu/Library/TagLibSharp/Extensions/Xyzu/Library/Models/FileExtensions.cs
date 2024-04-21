using TagLib;

using System;
using System.Linq;

using Xyzu.Images;
using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public static class FileExtensions
	{
		public static IAlbum Retrieve(this IAlbum retrieved, File file, IAlbum<bool>? retriever, bool overwrite = false)
		{
			if ((retriever?.Artist ?? true) && (retrieved.Artist is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedAlbumArtists) is false)
				retrieved.Artist = file.Tag.JoinedAlbumArtists.Trim();

			if ((retriever?.ReleaseDate ?? true) && (retrieved.ReleaseDate is null || overwrite) && int.TryParse(file.Tag.Year.ToString(), out int year) && year is not 0) 
				retrieved.ReleaseDate = new DateTime(year, 1, 1);  							

			if ((retriever?.DiscCount ?? true) && (retrieved.DiscCount is null || overwrite) && int.TryParse(file.Tag.DiscCount.ToString(), out int disccount)) 
				retrieved.DiscCount = disccount;  
																   
			if ((retriever?.Title ?? true) && (retrieved.Title is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Album) is false) 
				retrieved.Title = file.Tag.Album;	

			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, File file, IArtist<bool>? retriever, bool overwrite = false)
		{
			if ((retriever?.Name ?? true) && (retrieved.Name is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedPerformers) is false)
				retrieved.Name = file.Tag.JoinedPerformers.Trim();

			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, File file, IGenre<bool>? retriever, bool overwrite = false)
		{
			if ((retriever?.Name ?? true) && (retrieved.Name is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedGenres) is false) 
				retrieved.Name = file.Tag.JoinedGenres.Trim();

			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, File file, IPlaylist<bool>? retriever, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, File file, ISong<bool>? retriever, bool overwrite = false)
		{
			if ((retriever?.Album ?? true) && (retrieved.Album is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Album) is false)
				retrieved.Album = file.Tag.Album;   

			if ((retriever?.AlbumArtist ?? true) && (retrieved.AlbumArtist is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedAlbumArtists) is false)
				retrieved.AlbumArtist = file.Tag.JoinedAlbumArtists.Trim();					

			if ((retriever?.Artist ?? true) && (retrieved.Artist is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedPerformers) is false)
				retrieved.Artist = file.Tag.JoinedPerformers.Trim();																		 

			if ((retriever?.Bitrate ?? true) && (retrieved.Bitrate is null || overwrite) && file.Properties.AudioBitrate is not 0)
				retrieved.Bitrate = file.Properties.AudioBitrate;

			if ((retriever?.Channels ?? true) && (retrieved.Channels is null || overwrite) && file.Properties.AudioChannels is not 0)
				retrieved.Channels = default(AudioChannels).FromChannelCount(file.Properties.AudioChannels);

			if ((retriever?.Copyright ?? true) && (retrieved.Copyright is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Copyright) is false)
				retrieved.Copyright = file.Tag.Copyright.Trim();

			if ((retriever?.DiscNumber ?? true) && (retrieved.DiscNumber is null || overwrite) && int.TryParse(file.Tag.Disc.ToString(), out int discnumber) && discnumber is not 0)
				retrieved.DiscNumber = discnumber;

			if ((retriever?.Duration ?? true) && (retrieved.Duration is null || overwrite) && file.Properties.Duration != TimeSpan.Zero)
				retrieved.Duration = file.Properties.Duration;
																																					  
			if ((retriever?.Genre ?? true) && (retrieved.Genre is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.JoinedGenres) is false)
				retrieved.Genre = file.Tag.JoinedGenres.Trim();

			if ((retriever?.Lyrics ?? true) && (retrieved.Lyrics is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Lyrics) is false)
				retrieved.Lyrics = file.Tag.Lyrics;

			if ((retriever?.ReleaseDate ?? true) && (retrieved.ReleaseDate is null || overwrite) && int.TryParse(file.Tag.Year.ToString(), out int year) && year is not 0)
				retrieved.ReleaseDate = new DateTime(year, 1, 1);

			if ((retriever?.Title ?? true) && (retrieved.Title is null || overwrite) && string.IsNullOrWhiteSpace(file.Tag.Title) is false)
				retrieved.Title = file.Tag.Title;

			if ((retriever?.TrackNumber ?? true) && (retrieved.TrackNumber is null || overwrite) && int.TryParse(file.Tag.Track.ToString(), out int tracknumber) && tracknumber is not 0)
				retrieved.TrackNumber = tracknumber;		

			if ((retriever?.Size ?? true) && (retrieved.Size is null || overwrite) && file.Length is not 0)
				retrieved.Size = file.Length;

			return retrieved;
		}
		public static IImage Retrieve(this IImage retrieved, File file, IImage<bool>? retriever, PictureTypeComparer? picturetypecomparer)
		{
			if (retriever is null || (retriever.Buffer && retrieved.Buffer is null) || (retriever.BufferHash && retrieved.BufferHash is null))
			{
				IPicture? picture = file.Tag.Pictures
					.OrderBy(picture => picture.Type, picturetypecomparer ?? new PictureTypeComparer())
					.FirstOrDefault();

				if (picture?.Data.Data is byte[] picturedata)
				{
					retrieved.Buffer = retriever?.Buffer ?? true ? picturedata : null;
					retrieved.BufferHash = retriever?.BufferHash ?? true ? IImage.Utils.BufferToHash(picturedata) : null;
				}
			}
			return retrieved;
		}
	}
}
