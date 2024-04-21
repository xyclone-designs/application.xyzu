using Xyzu.Player;
using Xyzu.Settings.UserInterface;

namespace Xyzu.Views.NowPlaying
{
	public interface INowPlaying
	{
		IPlayer? Player { get; set; }
		INowPlayingSettings Settings { get; set; }
	}
}
