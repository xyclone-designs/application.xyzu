using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface
{
	public interface IThemesSettings<T> : IUserInterfaceSettings<T>
	{
		T Mode { get; set; }
	}
	public interface IThemesSettings : IUserInterfaceSettings
	{
		ThemeModes Mode { get; set; }

		public new class Defaults : IUserInterfaceSettings.Defaults
		{
			public static readonly ThemeModes Mode = ThemeModes.FollowSystem;
		}
		public new class Keys : IUserInterfaceSettings.Keys
		{
			public new const string Base = IUserInterfaceSettings.Keys.Base + "." + nameof(IThemesSettings);

			public const string Mode = Base + "." + nameof(Mode);
		}
		public new class Options : IUserInterfaceSettings.Options
		{
			public class Modes
			{
				public const ThemeModes FollowSystem = ThemeModes.FollowSystem;
				public const ThemeModes ForceDark = ThemeModes.ForceDark;
				public const ThemeModes ForceLight = ThemeModes.ForceLight;

				public static IEnumerable<ThemeModes> AsEnumerable()
				{
					return Enum
						.GetValues(typeof(ThemeModes))
						.Cast<ThemeModes>();
				}
			}
		}

		public new class Default : IUserInterfaceSettings.Default, IThemesSettings 
		{
			public Default()
			{
				_Mode = Defaults.Mode;
			}

			private ThemeModes _Mode;
			public ThemeModes Mode
			{
				get => _Mode;
				set
				{
					_Mode = value;

					OnPropertyChanged();
				}
			}

			public override void SetFromKey(string key, object? value)
			{
				base.SetFromKey(key, value);

				switch (key)
				{
					case Keys.Mode when value is ThemeModes mode:
						Mode = mode;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : IUserInterfaceSettings.Default<T>, IThemesSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				Mode = defaultvalue;
			}

			public T Mode { get; set; }
		}
	}
}
