using Xyzu.Settings.UserInterface;

namespace Xyzu.Views.NowPlaying
{
	public interface INowPlayingDroid : INowPlaying
	{
		new INowPlayingSettingsDroid Settings { get; set; }
	}
}
