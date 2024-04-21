
namespace Xyzu.Settings.Notification
{
	public interface INotificationSettings<T> : ISettings<T> { }
	public interface INotificationSettings : ISettings
	{
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(INotificationSettings);
		}

		public new class Default : ISettings.Default, INotificationSettings { }
		public new class Default<T> : ISettings.Default<T>, INotificationSettings<T> 
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
