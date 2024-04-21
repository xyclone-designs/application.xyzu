using Xyzu.Library.Enums;

namespace Id3
{
	public static class AudioModeExtensions
	{
		public static AudioChannels? ToAudioChannel(this AudioMode audiomode)
		{
			return audiomode switch
			{
				AudioMode.DualChannel => AudioChannels.DualChannel,
				AudioMode.JointStereo => AudioChannels.JointStereo,
				AudioMode.SingleChannel => AudioChannels.SingleChannel,
				AudioMode.Stereo => AudioChannels.Stereo,

				_ => new AudioChannels?(),
			};
		}
	}
}
