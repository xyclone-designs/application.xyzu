
namespace Xyzu.Settings.UserInterface
{
	public interface INowPlayingSettingsDroid<T> : INowPlayingSettings<T>
	{
		T ForceShowNowPlaying { get; set; }
	}
	public interface INowPlayingSettingsDroid : INowPlayingSettings
	{
		public new class Defaults : INowPlayingSettings.Defaults
		{
			public const bool ForceShowNowPlaying = true;

			public static readonly INowPlayingSettingsDroid NowPlayingSettingsDroid = new Default
			{
				ForceShowNowPlaying = ForceShowNowPlaying,
			};
		}
		public new class Keys : INowPlayingSettings.Keys
		{
			public const string ForceShowNowPlaying = Base + "." + nameof(ForceShowNowPlaying);
		}
		public new class Options : INowPlayingSettings.Options
		{
		}

		bool ForceShowNowPlaying { get; set; }

		public new class Default : INowPlayingSettings.Default, INowPlayingSettingsDroid
		{
			public bool ForceShowNowPlaying { get; set; }
		}
		public new class Default<T> : INowPlayingSettings.Default<T>, INowPlayingSettingsDroid<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				ForceShowNowPlaying = defaultvalue;
			}

			public T ForceShowNowPlaying { get; set; }
		}

	}
}
