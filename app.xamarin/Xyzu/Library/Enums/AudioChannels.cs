
namespace Xyzu.Library.Enums
{
	public enum AudioChannels
	{
		Stereo,
		JointStereo,
		DualChannel,
		SingleChannel 
	}

	public static class AudioChannelsExtensions
	{
		public static AudioChannels? FromChannelCount(this AudioChannels _, int channelcount)
		{
			return channelcount switch
			{
				0 => AudioChannels.Stereo,
				1 => AudioChannels.SingleChannel,
				2 => AudioChannels.DualChannel,

				_ => new AudioChannels?()
			};
		}
	}
}
