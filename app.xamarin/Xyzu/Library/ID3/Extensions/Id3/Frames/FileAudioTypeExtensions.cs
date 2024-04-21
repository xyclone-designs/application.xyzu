using Xyzu.Library.Enums;

namespace Id3.Frames
{
	public static class FileAudioTypeExtensions
	{
		public static MimeTypes? ToMimeType(this FileAudioType fileaudiotype)
		{
			return fileaudiotype switch
			{
				FileAudioType.Mpeg or
				FileAudioType.Mpeg_1_2_Layer1 or
				FileAudioType.Mpeg_1_2_Layer2 or
				FileAudioType.Mpeg_1_2_Layer3 or
				FileAudioType.Mpeg_2_5 => MimeTypes.MP3,

				FileAudioType.Mpeg_Aac => MimeTypes.AAC,

				_ => null
			};
		}
	}
}
