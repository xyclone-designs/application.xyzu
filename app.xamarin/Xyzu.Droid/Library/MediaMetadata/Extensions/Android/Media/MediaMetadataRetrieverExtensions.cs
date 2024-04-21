#nullable enable

using System;

using Xyzu.Library.Models;

namespace Android.Media
{
	public static class MediaMetadataRetrieverExtensions
	{
		public static T ExtractMetadata<T>(this MediaMetadataRetriever mediametadataretriever, MetadataKey metadatakey, T defaultvalue, Func<string, T>? onconvertmetadata = null)
		{
			string? metadata = mediametadataretriever.ExtractMetadata(metadatakey);

			if (metadata is null)
				return defaultvalue;

			if (onconvertmetadata != null)
				return onconvertmetadata.Invoke(metadata) ?? defaultvalue;

			Type type = typeof(T);

			object? obj = true switch
			{
				true when type == typeof(byte) || type == typeof(byte?) => byte.TryParse(metadata, out byte outoutint) ? outoutint : new byte?(),
				true when type == typeof(float) || type == typeof(float?) => float.TryParse(metadata, out float outfloat) ? outfloat : new float?(),
				true when type == typeof(int) || type == typeof(int?) => int.TryParse(metadata, out int outint) ? outint : new int?(),
				true when type == typeof(long) || type == typeof(long?) => long.TryParse(metadata, out long outlong) ? outlong : new long?(),
				true when type == typeof(short) || type == typeof(short?) => short.TryParse(metadata, out short outshort) ? outshort : new short?(),
				true when type == typeof(string) => metadata,
				true when type == typeof(DateTime) || type == typeof(DateTime?) => true switch
				{
					true when int.TryParse(metadata, out int outdatetimeint) => outdatetimeint <= 12 ? new DateTime(outdatetimeint, 1, 1) : new DateTime(outdatetimeint),

					_ => DateTime.TryParse(metadata, out DateTime datetime) ? datetime : new DateTime?()
				},
				true when type == typeof(Uri) || type == typeof(Uri) => Uri.TryCreate(metadata, UriKind.RelativeOrAbsolute, out Uri outuri)
					? outuri
					: null,
				true when type == typeof(TimeSpan) || type == typeof(TimeSpan?) => TimeSpan.TryParse(metadata, out TimeSpan outtimespan)
					? outtimespan
					: new TimeSpan?(),

				_ => null,				
			};

			if (obj is null)
				return defaultvalue;

			return (T)obj;
		}

		public static bool ShouldRetrieveAlbum(this MediaMetadataRetriever mediametadataretriever, IAlbum album)
		{
			return string.Equals(album?.Title, mediametadataretriever.ExtractMetadata(MetadataKey.Album));
		}
		public static bool ShouldRetrieveArtist(this MediaMetadataRetriever mediametadataretriever, IArtist artist)
		{
			return string.Equals(artist?.Name, mediametadataretriever.ExtractMetadata(MetadataKey.Artist));
		}
		public static bool ShouldRetrieveGenre(this MediaMetadataRetriever mediametadataretriever, IGenre genre)
		{
			return string.Equals(genre?.Name, mediametadataretriever.ExtractMetadata(MetadataKey.Genre));
		}
		public static bool ShouldRetrievePlaylist(this MediaMetadataRetriever mediametadataretriever, IPlaylist playlist)
		{
			return false;
		}
		public static bool ShouldRetrieveSong(this MediaMetadataRetriever mediametadataretriever, ISong song)
		{
			return string.Equals(song?.Title, mediametadataretriever.ExtractMetadata(MetadataKey.Title));
		}
	}
}
