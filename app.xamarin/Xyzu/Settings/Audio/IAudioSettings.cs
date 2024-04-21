
namespace Xyzu.Settings.Audio
{
	public interface IAudioSettings<T> : ISettings<T> { }
	public interface IAudioSettings : ISettings
	{
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(IAudioSettings);
		}

		public new class Default : ISettings.Default, IAudioSettings { }
		public new class Default<T> : ISettings.Default<T>, IAudioSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
