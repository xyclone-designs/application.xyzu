#nullable enable

using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Library.Enums
{
	public static class AudioChannelsExtensions
	{  
		public static int AsResoureIdTitle(this AudioChannels audiochannel)
		{
			return audiochannel switch
			{
				AudioChannels.DualChannel => Resource.String.enums_audiochannels_dualchannel_title,
				AudioChannels.JointStereo => Resource.String.enums_audiochannels_jointstereo_title,
				AudioChannels.SingleChannel => Resource.String.enums_audiochannels_singlechannel_title,
				AudioChannels.Stereo => Resource.String.enums_audiochannels_stereo_title,

				_ => throw new ArgumentException(string.Format("Invalid AudioChannels '{0}'", audiochannel))
			};
		}	
		public static int AsResoureIdDescription(this AudioChannels audiochannel)
		{
			return audiochannel switch
			{
				AudioChannels.DualChannel => Resource.String.enums_audiochannels_dualchannel_description,
				AudioChannels.JointStereo => Resource.String.enums_audiochannels_jointstereo_description,
				AudioChannels.SingleChannel => Resource.String.enums_audiochannels_singlechannel_description,
				AudioChannels.Stereo => Resource.String.enums_audiochannels_stereo_description,

				_ => throw new ArgumentException(string.Format("Invalid AudioChannels '{0}'", audiochannel))
			};
		}
		public static string? AsStringTitle(this AudioChannels audiochannel, Context? context)
		{
			if (audiochannel.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this AudioChannels audiochannel, Context? context)
		{
			if (audiochannel.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}