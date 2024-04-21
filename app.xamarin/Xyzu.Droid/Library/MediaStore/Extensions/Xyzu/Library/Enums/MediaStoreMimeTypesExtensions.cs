
namespace Xyzu.Library.Enums
{
	public static class MediaStoreMimeTypesExtensions
	{
		public static string ToMediaStoreMimeType(this MimeTypes mimetype)
		{
			return mimetype switch
			{
				MimeTypes.AAC => "audio/aac",
				MimeTypes.FLAC => "audio/flac",
				MimeTypes.M4A => "audio/m4a",
				MimeTypes.MP3 => "audio/mp3",
				MimeTypes.MP4 => "video/mp4",
				MimeTypes.WAV => "audio/wav",

				_ => mimetype.ToString()
			};
		}
	}
}
