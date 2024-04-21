
namespace Xyzu.Settings.About
{
	public interface IAboutSettings<T> : ISettings<T> { }
	public interface IAboutSettings : ISettings
	{
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(IAboutSettings);
		}

		public new class Default : ISettings.Default, IAboutSettings { }
		public new class Default<T> : ISettings.Default<T>, IAboutSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
