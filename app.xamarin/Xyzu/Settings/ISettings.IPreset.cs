
namespace Xyzu.Settings
{
	public partial interface ISettings
	{
		public interface IPreset<T> : ISettings<T>
		{
			T Name { get; set; }
		}
		public interface IPreset : ISettings
		{
			string Name { get; set; }

			public new class Default : ISettings.Default, IPreset 
			{
				public Default (string name)
				{					
					_Name = name;
				}

				private string _Name;

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
					Name = defaultvalue;
				}

				public T Name { get; set; }
			}
		}
	}
}
