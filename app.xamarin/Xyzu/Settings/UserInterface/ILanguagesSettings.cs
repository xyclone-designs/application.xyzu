using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Xyzu.Settings.Enums;

namespace Xyzu.Settings.UserInterface
{
	public interface ILanguagesSettings<T> : IUserInterfaceSettings<T> { }
	public interface ILanguagesSettings : IUserInterfaceSettings
	{
		public new class Defaults
		{
			public const LanguageModes Mode = Options.Modes.FollowSystem;
			public static readonly CultureInfo CurrentLanguage = Options.CurrentLanguage.English;
		}
		public new class Keys : IUserInterfaceSettings.Keys
		{
			public new const string Base = IUserInterfaceSettings.Keys.Base + "." + nameof(ILanguagesSettings);

			public const string Mode = Base + "." + nameof(Mode);
			public const string CurrentLanguage = Base + "." + nameof(CurrentLanguage);
		}
		public new class Options : ISettings.Options
		{
			public class Modes
			{
				public const LanguageModes FollowSystem = LanguageModes.FollowSystem;
				public const LanguageModes ForceChosen = LanguageModes.ForceChosen;

				public static IEnumerable<LanguageModes> AsEnumerable()
				{
					return Enumerable.Empty<LanguageModes>()
						.Append(LanguageModes.FollowSystem)
						.Append(LanguageModes.ForceChosen);
				}
			}					 
			public class CurrentLanguage
			{
				public static readonly CultureInfo English = CultureInfo.GetCultureInfo("en");

				public static IEnumerable<CultureInfo> AsEnumerable()
				{
					return Enumerable.Empty<CultureInfo>()
						.Append(English);
				}
			}
		}

		LanguageModes Mode { get; set; }
		CultureInfo? CurrentLanguage { get; set; }

		public new class Default : IUserInterfaceSettings.Default, ILanguagesSettings 
		{
			public LanguageModes Mode { get; set; }
			public CultureInfo? CurrentLanguage { get; set; }

			public override void SetFromKey(string key, object? value)
			{
				base.SetFromKey(key, value);

				switch (key)
				{
					case Keys.Mode when value is LanguageModes mode:
						Mode = mode;
						break;
										   
					case Keys.CurrentLanguage when value is CultureInfo currentlanguage:
						CurrentLanguage = currentlanguage;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : IUserInterfaceSettings.Default<T>, ILanguagesSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) { }
		}
	}
}
