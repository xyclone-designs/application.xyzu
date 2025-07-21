using XyzuResource = Xyzu.Droid.Resource;

namespace Android.Content
{
	public static class IntentTypesExtensions
	{
		public static int? AsResoureIdIntentChooserTitle(this IntentTypes intenttype)
		{
			return intenttype switch
			{
				IntentTypes.All => XyzuResource.String.intentchooser_select_all,
				IntentTypes.Application => XyzuResource.String.intentchooser_select_application,
				IntentTypes.Application_PDF => XyzuResource.String.intentchooser_select_application_pdf,
				IntentTypes.Audio => XyzuResource.String.intentchooser_select_audio,
				IntentTypes.Audio_FLAC => XyzuResource.String.intentchooser_select_audio_flac,
				IntentTypes.Audio_M4A => XyzuResource.String.intentchooser_select_audio_m4a,
				IntentTypes.Audio_MP3 => XyzuResource.String.intentchooser_select_audio_mp3,
				IntentTypes.Audio_WAV => XyzuResource.String.intentchooser_select_audio_wav,
				IntentTypes.Image => XyzuResource.String.intentchooser_select_image,
				IntentTypes.Image_GIF => XyzuResource.String.intentchooser_select_image_gif,
				IntentTypes.Image_JPG => XyzuResource.String.intentchooser_select_image_jpg,
				IntentTypes.Image_JPEG => XyzuResource.String.intentchooser_select_image_jpeg,
				IntentTypes.Image_PNG => XyzuResource.String.intentchooser_select_image_png,
				IntentTypes.Text => XyzuResource.String.intentchooser_select_text,
				IntentTypes.Text_HTML => XyzuResource.String.intentchooser_select_text_html,
				IntentTypes.Text_JSON => XyzuResource.String.intentchooser_select_text_json,
				IntentTypes.Text_Plain => XyzuResource.String.intentchooser_select_text_plain,
				IntentTypes.Text_RTF => XyzuResource.String.intentchooser_select_text_rtf,
				IntentTypes.Video => XyzuResource.String.intentchooser_select_video,
				IntentTypes.Video_MP4 => XyzuResource.String.intentchooser_select_video_mp4,
				IntentTypes.Video_3GP => XyzuResource.String.intentchooser_select_video_3gp,

				_ => new int?()
			};
		}
		public static string? AsStringIntentChooserTitle(this IntentTypes intenttype, Context? context)
		{
			if (intenttype.AsResoureIdIntentChooserTitle() is int resoureidintentchoosertitle)
				return context?.Resources?.GetString(resoureidintentchoosertitle);

			return null;
		}
	}
}