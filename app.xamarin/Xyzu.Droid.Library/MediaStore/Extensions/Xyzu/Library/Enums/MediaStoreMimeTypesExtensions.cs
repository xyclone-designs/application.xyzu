
namespace Xyzu.Library.Enums
{
	public static class MediaStoreMimeTypesExtensions
	{
		public static string ToMediaStoreMimeType(this MimeTypes mimetype)
		{
			return mimetype switch
			{
				MimeTypes.aac => "audio/aac",
				MimeTypes.flac => "audio/flac",
				MimeTypes.m4a => "audio/m4a",
				MimeTypes.mp3 => "audio/mp3",
				MimeTypes.wav => "audio/wav",

				_ => mimetype.ToString()
			};
		}
	}
}
