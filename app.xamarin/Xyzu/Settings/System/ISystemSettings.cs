using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Xyzu.Settings.Enums;

namespace Xyzu.Settings.System
{
	public interface ISystemSettings<T> : ISettings<T>
	{
		T ErrorLogs { get; set; }
		T LanguageCurrent { get; set; }
		T LanguageMode { get; set; }
		T ThemeMode { get; set; }
	}
	public partial interface ISystemSettings : ISettings
	{
		IEnumerable<IErrorLog> ErrorLogs { get; set; }
		CultureInfo LanguageCurrent { get; set; }
		LanguageModes LanguageMode { get; set; }
		ThemeModes ThemeMode { get; set; }

		public new class Defaults : ISettings.Defaults
		{
			public static readonly IEnumerable<IErrorLog> ErrorLogs = Enumerable.Empty<IErrorLog>();
			public static readonly CultureInfo LanguageCurrent = Options.LanguageCurrent.English;
			public static readonly LanguageModes LanguageMode = Options.LanguageMode.FollowSystem;
			public static readonly ThemeModes ThemeMode = Options.ThemeMode.FollowSystem;
		}
		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(ISystemSettings);

			public const string ThemeMode = Base + "." + nameof(ThemeMode);
			public const string LanguageCurrent = Base + "." + nameof(LanguageCurrent);
			public const string LanguageMode = Base + "." + nameof(LanguageMode);
		}
		public new class Options : ISettings.Options
		{
			public class LanguageCurrent
			{
				public static readonly CultureInfo English = CultureInfo.GetCultureInfo("en");

				public static IEnumerable<CultureInfo> AsEnumerable()
				{
					yield return English;
				}
			}
			public class LanguageMode
			{
				public const LanguageModes FollowSystem = LanguageModes.FollowSystem;
				public const LanguageModes ForceChosen = LanguageModes.ForceChosen;

				public static IEnumerable<LanguageModes> AsEnumerable()
				{
					yield return FollowSystem;
					yield return ForceChosen;
				}
			}
			public class ThemeMode
			{
				public const ThemeModes FollowSystem = ThemeModes.FollowSystem;
				public const ThemeModes ForceDark = ThemeModes.ForceDark;
				public const ThemeModes ForceLight = ThemeModes.ForceLight;

				public static IEnumerable<ThemeModes> AsEnumerable()
				{
					yield return FollowSystem;
					yield return ForceDark;
					yield return ForceLight;
				}
			}
		}

		public new class Default : ISettings.Default, ISystemSettings
		{
			public Default() : base()
			{
				ErrorLogs = Defaults.ErrorLogs;
				LanguageCurrent = Defaults.LanguageCurrent;
				LanguageMode = Defaults.LanguageMode;
				ThemeMode = Defaults.ThemeMode;
			}

			public IEnumerable<IErrorLog> ErrorLogs { get; set; }
			public CultureInfo LanguageCurrent { get; set; }
			public LanguageModes LanguageMode { get; set; }
			public ThemeModes ThemeMode { get; set; }

			public override void SetFromKey(string key, object? value)
			{
				base.SetFromKey(key, value);

				switch (key)
				{
					case Keys.LanguageCurrent when value is CultureInfo languagecurrent:
						LanguageCurrent = languagecurrent;
						break;

					case Keys.LanguageMode when value is LanguageModes languagemode:
						LanguageMode = languagemode;
						break;

					case Keys.ThemeMode when value is ThemeModes thememode:
						ThemeMode = thememode;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : ISettings.Default<T>, ISystemSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				ErrorLogs = defaultvalue;
				LanguageCurrent = defaultvalue;
				LanguageMode = defaultvalue;
				ThemeMode = defaultvalue;
			}

			public T ErrorLogs { get; set; }
			public T LanguageCurrent { get; set; }
			public T LanguageMode { get; set; }
			public T ThemeMode { get; set; }
		}
	}
}
