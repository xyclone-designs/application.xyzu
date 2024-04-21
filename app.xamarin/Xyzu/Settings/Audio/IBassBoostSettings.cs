using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Settings.Audio
{
	public interface IBassBoostSettings<T> : IAudioSettings<T> { }
	public interface IBassBoostSettings : IAudioSettings
	{
		public new interface IPreset<T> : ISettings.IPreset<T> { }
		public new interface IPreset : ISettings.IPreset 
		{
			short Strength { get; set; }

			public new class Keys : IPreset.Keys
			{
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IBassBoostSettings) + "." + nameof(IPreset);

				public static string PresetName(string name) => string.Format("{0}.{1}", Base, name);

				public static string Strength(string name) => string.Format("{0}.{1}", PresetName(name), nameof(Strength));

				public static string? GetNameFromKey(string key)
				{
					key = key.Replace(Base, string.Empty);

					if (key[0] != '.')
						return null;

					return key.Split(".", StringSplitOptions.RemoveEmptyEntries)
						.FirstOrDefault();
				}
			}
			public static class Ranges
			{
				public static readonly short StrengthLower = 0;
				public static readonly short StrengthUpper = 1_000;
			}

			public new class Default : IPreset.Default, IBassBoostSettings.IPreset
			{
				public Default(string name) : base(name) { }

				public short _Strength;

				public short Strength
				{
					get => _Strength;
					set
					{
						_Strength = value;

						OnPropertyChanged();
					}
				}

				public override void SetFromKey(string key, object? value)
				{
					base.SetFromKey(key, value);

					switch (true)
					{
						case true when key == Keys.Strength(Name) && value is short strength:
							Strength = strength;
							break;

						default: break;
					}
				}
			}
		}

		public new interface IPresetable : IPresetable<IPreset>
		{
			public new class Keys : IPresetable<IPreset>.Keys
			{
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IBassBoostSettings) + "." + nameof(IPresetable);
				
				public const string IsEnabled = Base + "." + nameof(IsEnabled);
				public const string CurrentPreset = Base + "." + nameof(CurrentPreset);
			}
			public new static class Defaults
			{
				public const bool IsEnabled = false;

				public static class Presets
				{
					public static readonly IBassBoostSettings.IPreset Min = new IBassBoostSettings.IPreset.Default(nameof(Min))
					{
						Strength = IBassBoostSettings.IPreset.Ranges.StrengthLower
					};
					public static readonly IBassBoostSettings.IPreset Moderate = new IBassBoostSettings.IPreset.Default(nameof(Moderate))
					{
						Strength = 200
					};
					public static readonly IBassBoostSettings.IPreset Max = new IBassBoostSettings.IPreset.Default(nameof(Max))
					{
						Strength = IBassBoostSettings.IPreset.Ranges.StrengthUpper,
					};

					public static readonly IBassBoostSettings.IPreset Default = Min;

					public static IEnumerable<IBassBoostSettings.IPreset> AsEnumerable()
					{
						return Enumerable.Empty<IBassBoostSettings.IPreset>()
							.Append(Min)
							.Append(Moderate)
							.Append(Max);
					}
				}

				public static IBassBoostSettings.IPresetable FromPreset(IBassBoostSettings.IPreset? preset, bool withallpresets = false)
				{
					IBassBoostSettings.IPresetable presetable = new Default(preset ?? Presets.Default, null)
					{
						IsEnabled = IsEnabled,
					};

					if (withallpresets)
						presetable.AllPresets = presetable.AllPresets
							.Concat(Presets.AsEnumerable())
							.Append(presetable.CurrentPreset)
							.Distinct();

					return presetable;
				}
			}

			public new class Default : IPresetable.Default<IBassBoostSettings.IPreset>, IBassBoostSettings.IPresetable 
			{
				public Default(IBassBoostSettings.IPreset currentpreset, IEnumerable<IBassBoostSettings.IPreset>? allpresets) : base(currentpreset, allpresets) { }
			}
		}
	}
}
