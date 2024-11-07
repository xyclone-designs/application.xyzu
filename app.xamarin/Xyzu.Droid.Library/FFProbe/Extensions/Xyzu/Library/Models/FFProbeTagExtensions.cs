using Laerdal.FFmpeg.Android;

using System;
using System.Linq;

using Xyzu.Images;
using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public static class MediaInformationExtensions 
	{
		public static IAlbum Retrieve(this IAlbum retrieved, MediaInformation mediainformation, bool overwrite = false)
		{
			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, MediaInformation mediainformation, bool overwrite = false)
		{
			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, MediaInformation mediainformation, bool overwrite = false)
		{
			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, MediaInformation mediainformation, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, MediaInformation mediainformation, bool overwrite = false)
		{
			if (overwrite)
			{
				retrieved.Duration = null;
				retrieved.Malformed = false;
				retrieved.MimeType = null;
				retrieved.Size = null;
			}

			if (retrieved.Filepath is not null)
				retrieved.Malformed = mediainformation.Format.Contains(retrieved.Filepath.Split('.').Last()) is false;

			if (mediainformation.Streams.FirstOrDefault(_ => _.Type == "audio") is StreamInformation audiostream)
			{
				retrieved.Bitrate ??= int.TryParse(audiostream.Bitrate, out int _audiobitrate) ? _audiobitrate : new int?();
				retrieved.MimeType ??= 
					Enum.TryParse(audiostream.Codec, true, out MimeTypes _codec) ? _codec : 
					Enum.TryParse(retrieved.Filepath?.Split('.').Last(), true, out MimeTypes _extension) ? _extension : 
					new MimeTypes?();
			}

			retrieved.Bitrate ??= int.TryParse(mediainformation.Bitrate, out int _bitrate) ? _bitrate : new int?();
			retrieved.Duration ??= double.TryParse(mediainformation.Duration, out double _duration) ? TimeSpan.FromSeconds(_duration) : new TimeSpan?();
			retrieved.Size ??= long.TryParse(mediainformation.Size, out long _size) ? _size : new long?();

			retrieved = retrieved.Retrieve(mediainformation.Tags);

			return retrieved;
		}
		public static IImage Retrieve(this IImage retrieved, MediaInformation mediainformation, bool overwrite = false)
		{
			return retrieved;
		}
	}
}
