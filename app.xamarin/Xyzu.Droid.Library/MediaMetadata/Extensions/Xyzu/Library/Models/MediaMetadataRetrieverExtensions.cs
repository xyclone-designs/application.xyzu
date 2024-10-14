using Android.Media;

using System;

using Xyzu.Images;
using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public static class MediaMetadataRetrieverExtensions
	{
		public static IAlbum Retrieve(this IAlbum retrieved, MediaMetadataRetriever mediametadataretriever, bool overwrite = false)
		{
			if ((retrieved.Artist is null || overwrite))
				retrieved.Artist = mediametadataretriever.ExtractMetadata(MetadataKey.Albumartist);

			if ((retrieved.ReleaseDate is null || overwrite))
				retrieved.ReleaseDate = mediametadataretriever.ExtractMetadata<DateTime?>(MetadataKey.Year, null);

			if ((retrieved.Title is null || overwrite)) 
				retrieved.Title = mediametadataretriever.ExtractMetadata(MetadataKey.Album);

			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, MediaMetadataRetriever mediametadataretriever, bool overwrite = false)
		{
			if ((retrieved.Name is null || overwrite)) 
				retrieved.Name = mediametadataretriever.ExtractMetadata(MetadataKey.Artist);

			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, MediaMetadataRetriever mediametadataretriever, bool overwrite = false)
		{
			if ((retrieved.Name is null || overwrite)) 
				retrieved.Name = mediametadataretriever.ExtractMetadata(MetadataKey.Genre);

			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, MediaMetadataRetriever mediametadataretriever, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, MediaMetadataRetriever mediametadataretriever, bool overwrite = false)
		{
			if ((retrieved.Album is null || overwrite)) 
				retrieved.Album = mediametadataretriever.ExtractMetadata(MetadataKey.Album);
			
			if ((retrieved.AlbumArtist is null || overwrite)) 
				retrieved.AlbumArtist = mediametadataretriever.ExtractMetadata(MetadataKey.Albumartist);
			
			if ((retrieved.Artist is null || overwrite)) 
				retrieved.Artist = mediametadataretriever.ExtractMetadata(MetadataKey.Artist);
			
			if ((retrieved.Bitrate is null || overwrite))
				retrieved.Bitrate = mediametadataretriever.ExtractMetadata<int?>(MetadataKey.Bitrate, null) * 1024; // (bits/s => kbits/s)
			
			if ((retrieved.DateModified is null || overwrite))
				retrieved.DateModified = mediametadataretriever.ExtractMetadata<DateTime?>(MetadataKey.Date, null);

			if ((retrieved.DiscNumber is null || overwrite))
				retrieved.DiscNumber = mediametadataretriever.ExtractMetadata(
					defaultvalue: null,
					metadatakey: MetadataKey.DiscNumber,
					onconvertmetadata: discnumber =>
					{
						if (discnumber.Contains('/'))
							discnumber = discnumber.Split("/")[0];

						if (int.TryParse(discnumber, out int outdiscnumber))
							return outdiscnumber;

						return new int?();
					});

			if ((retrieved.Duration is null || overwrite))
				retrieved.Duration = mediametadataretriever.ExtractMetadata(
					defaultvalue: null,
					metadatakey: MetadataKey.Duration,
					onconvertmetadata: duration =>
					{
						if (int.TryParse(duration, out int outduration))
							return TimeSpan.FromMilliseconds(outduration);

						return new TimeSpan?();
					});
			
			if ((retrieved.Genre is null || overwrite)) 
				retrieved.Genre = mediametadataretriever.ExtractMetadata(MetadataKey.Genre);
			
			if ((retrieved.MimeType is null || overwrite))
				retrieved.MimeType = default(MimeTypes).FromString(mediametadataretriever.ExtractMetadata(MetadataKey.Mimetype));
			
			if ((retrieved.ReleaseDate is null || overwrite)) 
				retrieved.ReleaseDate = mediametadataretriever.ExtractMetadata<DateTime?>(MetadataKey.Year, null);
			
			if ((retrieved.Title is null || overwrite)) 
				retrieved.Title = mediametadataretriever.ExtractMetadata(MetadataKey.Title);

			if ((retrieved.TrackNumber is null || overwrite)) 
				retrieved.TrackNumber = mediametadataretriever.ExtractMetadata<int?>(MetadataKey.CdTrackNumber, null);

			return retrieved;
		}
		public static IImage Retrieve(this IImage retrieved, MediaMetadataRetriever? mediametadataretriever)
		{
			if (retrieved.Buffer is null && retrieved.BufferKey is null && mediametadataretriever?.GetEmbeddedPicture() is byte[] buffer)
			{
				retrieved.Buffer = buffer;
				retrieved.BufferKey = IImage.Utils.BufferToHashString(buffer);
			}

			return retrieved;
		}
	}
}
