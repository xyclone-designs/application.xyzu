#nullable enable

using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Library.Enums
{
	public static class MimeTypesExtensions
	{  
		public static int AsResoureIdTitle(this MimeTypes mimetype)
		{
			return mimetype switch
			{
				MimeTypes.aac => Resource.String.enums_mimetypes_aac_title,
				MimeTypes.flac => Resource.String.enums_mimetypes_flac_title,
				MimeTypes.m4a => Resource.String.enums_mimetypes_m4a_title,
				MimeTypes.mp3 => Resource.String.enums_mimetypes_mp3_title,
				MimeTypes.wav => Resource.String.enums_mimetypes_wav_title,

				_ => throw new ArgumentException(string.Format("Invalid MimeTypes '{0}'", mimetype))
			};
		}	
		public static int AsResoureIdDescription(this MimeTypes mimetype)
		{
			return mimetype switch
			{
				MimeTypes.aac => Resource.String.enums_mimetypes_aac_description,
				MimeTypes.flac => Resource.String.enums_mimetypes_flac_description,
				MimeTypes.m4a => Resource.String.enums_mimetypes_m4a_description,
				MimeTypes.mp3 => Resource.String.enums_mimetypes_mp3_description,
				MimeTypes.wav => Resource.String.enums_mimetypes_wav_description,

				_ => throw new ArgumentException(string.Format("Invalid MimeTypes '{0}'", mimetype))
			};
		}
		public static string? AsStringTitle(this MimeTypes mimetype, Context? context)
		{
			if (mimetype.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this MimeTypes mimetype, Context? context)
		{
			if (mimetype.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}