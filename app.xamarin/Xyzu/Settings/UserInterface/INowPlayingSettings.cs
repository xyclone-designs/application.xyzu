
namespace Xyzu.Settings.UserInterface
{
	public interface INowPlayingSettings<T> : IUserInterfaceSettings<T> { }
	public interface INowPlayingSettings : IUserInterfaceSettings
	{
		public new class Defaults : ISettings.Defaults
		{
			public static readonly INowPlayingSettings NowPlayingSettings = new Default();
		}

		public new class Keys : ISettings.Keys
		{
			public new const string Base = IUserInterfaceSettings.Keys.Base + "." + nameof(INowPlayingSettings);
		}

		public new class Default : IUserInterfaceSettings.Default, INowPlayingSettings { }
		public new class Default<T> : IUserInterfaceSettings.Default<T>, INowPlayingSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
