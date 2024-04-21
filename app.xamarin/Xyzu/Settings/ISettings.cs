using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Xyzu.Settings
{
	public partial interface ISettings<T> { }//: INotifyPropertyChanged { }
	public partial interface ISettingsTyped<T> : ISettings<T> { }
	public partial interface ISettings : INotifyPropertyChanged
	{
		void SetFromKey(string key, object? value);

		public class Defaults { }
		public class Keys
		{
			public const string Base = nameof(ISettings);
		}
		public class Options { }

		public class Default : ISettings
		{
			public event PropertyChangedEventHandler? PropertyChanged;

			public virtual void SetFromKey(string key, object? value) { }
			public virtual void OnPropertyChanged([CallerMemberName]string? propertyname = null) 
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname ?? string.Empty));
			}
		}
		public class Default<T> : ISettings<T>
		{
			public Default(T defaultvalue) { }

			//public event PropertyChangedEventHandler? PropertyChanged;
			//public virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null)
			//{
			//	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname ?? string.Empty));
			//}
		}			
		public class DefaultTyped<T> : Default<T>, ISettingsTyped<T>
		{
			public DefaultTyped(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
