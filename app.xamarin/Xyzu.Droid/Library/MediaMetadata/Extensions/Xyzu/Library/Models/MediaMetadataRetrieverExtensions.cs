#nullable enable

using Android.Media;

using System;

using Xyzu.Images;
using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public static class MediaMetadataRetrieverExtensions
	{
		public static IAlbum Retrieve(this IAlbum retrieved, MediaMetadataRetriever mediametadataretriever, IAlbum<bool>? retriever, bool overwrite = false)
		{
			if ((retriever?.Artist ?? true) && (retrieved.Artist is null || overwrite))
				retrieved.Artist = mediametadataretriever.ExtractMetadata(MetadataKey.Albumartist);

			if ((retriever?.ReleaseDate ?? true) && (retrieved.ReleaseDate is null || overwrite))
				retrieved.ReleaseDate = mediametadataretriever.ExtractMetadata<DateTime?>(MetadataKey.Year, null);

			if ((retriever?.Title ?? true) && (retrieved.Title is null || overwrite)) 
				retrieved.Title = mediametadataretriever.ExtractMetadata(MetadataKey.Album);

			return retrieved;
		}
		public static IArtist Retrieve(this IArtist retrieved, MediaMetadataRetriever mediametadataretriever, IArtist<bool>? retriever, bool overwrite = false)
		{
			if ((retriever?.Name ?? true) && (retrieved.Name is null || overwrite)) 
				retrieved.Name = mediametadataretriever.ExtractMetadata(MetadataKey.Artist);

			return retrieved;
		}
		public static IGenre Retrieve(this IGenre retrieved, MediaMetadataRetriever mediametadataretriever, IGenre<bool>? retriever, bool overwrite = false)
		{
			if ((retriever?.Name ?? true) && (retrieved.Name is null || overwrite)) 
				retrieved.Name = mediametadataretriever.ExtractMetadata(MetadataKey.Genre);

			return retrieved;
		}
		public static IPlaylist Retrieve(this IPlaylist retrieved, MediaMetadataRetriever mediametadataretriever, IPlaylist<bool>? retriever, bool overwrite = false)
		{
			return retrieved;
		}
		public static ISong Retrieve(this ISong retrieved, MediaMetadataRetriever mediametadataretriever, ISong<bool>? retriever, bool overwrite = false)
		{
			if ((retriever?.Album ?? true) && (retrieved.Album is null || overwrite)) 
				retrieved.Album = mediametadataretriever.ExtractMetadata(MetadataKey.Album);
			
			if ((retriever?.AlbumArtist ?? true) && (retrieved.AlbumArtist is null || overwrite)) 
				retrieved.AlbumArtist = mediametadataretriever.ExtractMetadata(MetadataKey.Albumartist);
			
			if ((retriever?.Artist ?? true) && (retrieved.Artist is null || overwrite)) 
				retrieved.Artist = mediametadataretriever.ExtractMetadata(MetadataKey.Artist);

			if ((retriever?.Bitrate ?? true) && (retrieved.Bitrate is null || overwrite))
				retrieved.Bitrate = mediametadataretriever.ExtractMetadata<int?>(MetadataKey.Bitrate, null) * 1024; // (bits/s => kbits/s)

			if ((retriever?.DateModified ?? true) && (retrieved.DateModified is null || overwrite))
				retrieved.DateModified = mediametadataretriever.ExtractMetadata<DateTime?>(MetadataKey.Date, null);

			if ((retriever?.DiscNumber ?? true) && (retrieved.DiscNumber is null || overwrite))
				retrieved.DiscNumber = mediametadataretriever.ExtractMetadata(
					defaultvalue: null,
					metadatakey: MetadataKey.DiscNumber,
					onconvertmetadata: discnumber =>
					{
						if (discnumber.Contains("/"))
							discnumber = discnumber.Split("/")[0];

						if (int.TryParse(discnumber, out int outdiscnumber))
							return outdiscnumber;

						return new int?();
					});

			if ((retriever?.Duration ?? true) && (retrieved.Duration is null || overwrite))
				retrieved.Duration = mediametadataretriever.ExtractMetadata(
					defaultvalue: null,
					metadatakey: MetadataKey.Duration,
					onconvertmetadata: duration =>
					{
						if (int.TryParse(duration, out int outduration))
							return TimeSpan.FromMilliseconds(outduration);

						return new TimeSpan?();
					});

			if ((retriever?.Genre ?? true) && (retrieved.Genre is null || overwrite)) 
				retrieved.Genre = mediametadataretriever.ExtractMetadata(MetadataKey.Genre);

			if ((retriever?.MimeType ?? true) && (retrieved.MimeType is null || overwrite))
				retrieved.MimeType = default(MimeTypes).FromString(mediametadataretriever.ExtractMetadata(MetadataKey.Mimetype));

			if ((retriever?.ReleaseDate ?? true) && (retrieved.ReleaseDate is null || overwrite)) 
				retrieved.ReleaseDate = mediametadataretriever.ExtractMetadata<DateTime?>(MetadataKey.Year, null);
			
			if ((retriever?.Title ?? true) && (retrieved.Title is null || overwrite)) 
				retrieved.Title = mediametadataretriever.ExtractMetadata(MetadataKey.Title);
			
			if ((retriever?.TrackNumber ?? true) && (retrieved.TrackNumber is null || overwrite)) 
				retrieved.TrackNumber = mediametadataretriever.ExtractMetadata<int?>(MetadataKey.CdTrackNumber, null);

			return retrieved;
		}
		public static IImage Retrieve(this IImage retrieved, IImage<bool>? retriever, MediaMetadataRetriever? mediametadataretriever)
		{
			if (retriever is null || (retriever.Buffer && retrieved.Buffer is null) || (retriever.BufferHash && retrieved.BufferHash is null))
				if (mediametadataretriever?.GetEmbeddedPicture() is byte[] buffer)
				{
					retrieved.Buffer = retriever?.Buffer ?? true ? buffer : null;
					retrieved.BufferHash = retriever?.BufferHash ?? true ? IImage.Utils.BufferToHash(buffer) : null;
				}

			return retrieved;
		}
	}
}
