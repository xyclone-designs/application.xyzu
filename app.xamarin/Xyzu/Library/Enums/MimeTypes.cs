using System;

namespace Xyzu.Library.Enums
{
	public enum MimeTypes
	{
		aac,
		flac,
		m4a,
		mp3,
		wav,
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
