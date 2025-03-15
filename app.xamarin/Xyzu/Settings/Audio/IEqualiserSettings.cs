using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Settings.Audio
{
	public interface IEqualiserSettings<T> : IAudioSettings<T> { }
	public interface IEqualiserSettings : IAudioSettings
	{
		public new interface IPreset<T> : ISettings.IPreset<T> 
		{
			T PreAmp { get; set; }
			T FrequencyLevels { get; set; }
		}
		public new interface IPreset : ISettings.IPreset 
		{
			public new class Keys : IPreset.Keys
			{
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IEqualiserSettings) + "." + nameof(IPreset);

				public static string PresetName(string name) => string.Format("{0}.{1}", Base, name);

				public static string IsDefault(string name) => string.Format("{0}.{1}", PresetName(name), nameof(IsDefault));
				public static string PreAmp(string name) => string.Format("{0}.{1}", PresetName(name), nameof(PreAmp));
				public static string FrequencyLevels(string name) => string.Format("{0}.{1}", PresetName(name), nameof(FrequencyLevels));

				public static string? GetNameFromKey(string key)
				{
					key = key.Replace(Base, string.Empty);

					if (key[0] != '.')
						return null;

					return key.Split(".", StringSplitOptions.RemoveEmptyEntries)
						.FirstOrDefault();
				}
			}
			public class BandCuttoffs
			{
				public const int Band_01 = 32;
				public const int Band_02 = 128;
				public const int Band_03 = 256;
				public const int Band_04 = 512;
				public const int Band_05 = 1_024;
				public const int Band_06 = 2_048;
				public const int Band_07 = 4_096;
				public const int Band_08 = 8_192;
				public const int Band_09 = 16_256;
				public const int Band_10 = 24_000;

				public static IEnumerable<int> AsEnumerable()
				{
					return Enumerable.Empty<int>()
						.Append(Band_01)
						.Append(Band_02)
						.Append(Band_03)
						.Append(Band_04)
						.Append(Band_05)
						.Append(Band_06)
						.Append(Band_07)
						.Append(Band_08)
						.Append(Band_09)
						.Append(Band_10);
				}
			}
			public class Ranges
			{
				public const float PreAmpLower = -1;
				public const float PreAmpUpper = 1;
				public const short FrequencyLevelsLower = -100;
				public const short FrequencyLevelsUpper = 100;
			}

			float PreAmp { get; set; }
			short[] FrequencyLevels { get; set; }

			public new class Default : IPreset.Default, IEqualiserSettings.IPreset
			{
				public Default(string name) : base(name) { }
				public Default(string name, IEqualiserSettings.IPreset preset) : base(name, preset)
				{
					_PreAmp = preset.PreAmp;
					_FrequencyLevels = preset.FrequencyLevels;
				}

				private float _PreAmp = 0F;
				private short[] _FrequencyLevels = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

				public float PreAmp
				{
					get => _PreAmp;
					set
					{
						_PreAmp = value;

						OnPropertyChanged();
					}
				}
				public short[] FrequencyLevels
				{
					get => _FrequencyLevels;
					set
					{
						_FrequencyLevels = value;

						OnPropertyChanged();
					}
				}

				public override void SetFromKey(string key, object? value)
				{
					base.SetFromKey(key, value);

					switch (true)
					{
						case true when key == Keys.IsDefault(Name) && value is bool isdefault:
							IsDefault = isdefault;
							break;
							
						case true when key == Keys.PreAmp(Name) && value is float preamp:
							PreAmp = preamp;
							break;

						case true when key == Keys.FrequencyLevels(Name) && value is short[] frequencylevels:
							FrequencyLevels = frequencylevels;
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
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IEqualiserSettings) + "." + nameof(IPresetable);

				public const string IsEnabled = Base + "." + nameof(IsEnabled);
				public const string CurrentPreset = Base + "." + nameof(CurrentPreset);
			}

			public new static class Defaults
			{
				public const bool IsEnabled = false;

				public static class Presets
				{
					public static readonly IEqualiserSettings.IPreset Bass = new IEqualiserSettings.IPreset.Default(nameof(Bass))
					{
						IsDefault = true,
						PreAmp = 0,
						FrequencyLevels = new short[] { 50, 20, 10, 0, 0, 0, 0, 0, 0, 0 },
					};
					public static readonly IEqualiserSettings.IPreset Flat = new IEqualiserSettings.IPreset.Default(nameof(Flat))
					{
						IsDefault = true,
						PreAmp = 0,
						FrequencyLevels = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					};
					public static readonly IEqualiserSettings.IPreset Treble = new IEqualiserSettings.IPreset.Default(nameof(Treble))
					{
						IsDefault = true,
						PreAmp = 0,
						FrequencyLevels = new short[] { 0, 0, 0, 0, 0, 10, 20, 30, 40, 50 },
					};

					public static readonly IEqualiserSettings.IPreset Default = Flat;

					public static IEnumerable<IEqualiserSettings.IPreset> AsEnumerable()
					{
						return Enumerable.Empty<IEqualiserSettings.IPreset>()
							.Append(Bass)
							.Append(Flat)
							.Append(Treble);
					}
				}

				public static IEqualiserSettings.IPresetable FromPreset(IEqualiserSettings.IPreset? preset, bool withallpresets = false)
				{
					preset ??= Presets.Default;

					IEqualiserSettings.IPresetable presetable = new Default(preset, null)
					{
						IsEnabled = IsEnabled
					};

					if (withallpresets)
						presetable.AllPresets = presetable.AllPresets
							.Append(presetable.CurrentPreset)
							.Concat(Presets.AsEnumerable())
							.Distinct();

					return presetable;
				}
			}

			public new class Default : IPresetable.Default<IEqualiserSettings.IPreset>, IEqualiserSettings.IPresetable
			{
				public Default(IEqualiserSettings.IPreset currentpreset, IEnumerable<IEqualiserSettings.IPreset>? allpresets) : base(currentpreset, allpresets) { }
			}
		}
	}
}
