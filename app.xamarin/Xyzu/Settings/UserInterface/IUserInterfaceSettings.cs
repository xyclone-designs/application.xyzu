
namespace Xyzu.Settings.UserInterface
{
	public interface IUserInterfaceSettings<T> : ISettings<T> { }
	public interface IUserInterfaceSettings : ISettings
	{
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(IUserInterfaceSettings);
		}

		public new class Default : ISettings.Default, IUserInterfaceSettings { }
		public new class Default<T> : ISettings.Default<T>, IUserInterfaceSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
