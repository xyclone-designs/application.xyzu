
namespace Xyzu.Settings.LockScreen
{
	public interface ILockScreenSettings<T> : ISettings<T> { }
	public interface ILockScreenSettings : ISettings
	{
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(ILockScreenSettings);
		}

		public new class Default : ISettings.Default, ILockScreenSettings { }
		public new class Default<T> : ISettings.Default<T>, ILockScreenSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
