
namespace Xyzu.Settings
{
	public partial interface ISettings
	{
		public interface IPreset<T> : ISettings<T>
		{
			T IsDefault { get; set; }
			T Name { get; set; }
		}
		public interface IPreset : ISettings
		{
			bool IsDefault { get; }
			string Name { get; set; }

			public new class Default : ISettings.Default, IPreset 
			{
				public Default(string name)
				{					
					_Name = name;
				}
				public Default(string name, IPreset preset)
				{					
					_Name = name;
				}

				protected string _Name;
				protected bool _IsDefault;

				public bool IsDefault
				{
					get => _IsDefault;
					internal set => _IsDefault = value;
				}
				public string Name
				{
					get => _Name;
					set
					{
						_Name = value;

						OnPropertyChanged();
					}
				}
			}
			public new class Default<T> : ISettings.Default<T>, IPreset<T>
			{
				public Default(T defaultvalue) : base(defaultvalue)
				{
					IsDefault = defaultvalue;
					Name = defaultvalue;
				}

				public T IsDefault { get; set; }
				public T Name { get; set; }
			}
		}
	}
}
