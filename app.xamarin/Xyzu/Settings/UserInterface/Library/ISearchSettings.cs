using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface.Library
{
	public interface ISearchSettings<T> : ISettings<T> 
	{
		T LayoutType { get; set; }
	}
	public interface ISearchSettings : ISettings
	{
		public new class Defaults : ISettings.Defaults
		{
			public static readonly LibraryLayoutTypes LayoutType = LibraryLayoutTypes.ListMedium;
			
			public static readonly ISearchSettings SearchSettings = new Default
			{
				LayoutType = LayoutType,
			};
		}
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ILibrarySettings.Keys.Base + "." + nameof(ISearchSettings);

			public const string LayoutType = Base + "." + nameof(LayoutType);
		}
		public new class Options : ISettings.Options
		{
			public class LayoutTypes : LibraryDefaultSettings.Options.LayoutTypes { }
		}

		LibraryLayoutTypes LayoutType { get; set; }

		public new class Default : ISettings.Default, ISearchSettings
		{
			public Default()
			{
				_LayoutType = Defaults.LayoutType;
			}

			private LibraryLayoutTypes _LayoutType;

			public LibraryLayoutTypes LayoutType
			{
				get => _LayoutType;
				set
				{
					_LayoutType = value;

					OnPropertyChanged();
				}
			}

			public override void SetFromKey(string key, object? value)
			{
				base.SetFromKey(key, value);

				switch (key)
				{
					case Keys.LayoutType when value is LibraryLayoutTypes layouttype:
						LayoutType = layouttype;
						break; 

					default: break;
				}
			}
		}
		public new class Default<T> : ISettings.Default<T>, ISearchSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) 
			{
				LayoutType = defaultvalue;
			}

			public T LayoutType { get; set; }
		}
	}
}
