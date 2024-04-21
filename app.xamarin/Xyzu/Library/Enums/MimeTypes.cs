using System;

namespace Xyzu.Library.Enums
{
	public enum MimeTypes
	{
		AAC,
		FLAC,
		M4A,
		MP3,
		MP4,
		WAV,
	}

	public static class MimeTypesExtensions
	{
		public static MimeTypes? FromString(this MimeTypes _, string? str)
		{
			if (str is null)
				return null;

			if (Enum.TryParse(str, true, out MimeTypes outmimetype))
				return outmimetype;

			return null;
		}
	}
}
