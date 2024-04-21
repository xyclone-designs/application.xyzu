using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Settings.Audio
{
	public interface IEqualiserSettings<T> : IAudioSettings<T> { }
	public interface IEqualiserSettings : IAudioSettings
	{
		public enum BandType
		{
			Six = 6,
			Eight = 8,
			Ten = 10,
		}

		public new interface IPreset<T> : ISettings.IPreset<T> 
		{
			T FrequencyLevels { get; set; }
		}
		public new interface IPreset : ISettings.IPreset 
		{
			public new class Keys : IPreset.Keys
			{
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IEqualiserSettings) + "." + nameof(IPreset);

				public static string PresetName(string name) => string.Format("{0}.{1}", Base, name);

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
				public class SixBand
				{
					public const int Band_01 = 128;
					public const int Band_02 = 512;
					public const int Band_03 = 4_096;
					public const int Band_04 = 8_192;
					public const int Band_05 = 16_256;
					public const int Band_06 = 24_000;

					public static IEnumerable<int> AsEnumerable()
					{
						return Enumerable.Empty<int>()
							.Append(Band_01)
							.Append(Band_02)
							.Append(Band_03)
							.Append(Band_04)
							.Append(Band_05)
							.Append(Band_06);
					}
				}
				public class EightBand
				{
					public const int Band_01 = 128;
					public const int Band_02 = 512;
					public const int Band_03 = 1_024;
					public const int Band_04 = 2_048;
					public const int Band_05 = 4_096;
					public const int Band_06 = 8_192;
					public const int Band_07 = 16_256;
					public const int Band_08 = 24_000;

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
							.Append(Band_08);
					}
				}
				public class TenBand
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
			}
			public class Ranges
			{
				public const int FrequencyLevelsLower = -100;
				public const int FrequencyLevelsUpper = 100;
			}

			short[] FrequencyLevels { get; set; }

			public new class Default : IPreset.Default, IEqualiserSettings.IPreset
			{
				public Default(string name) : base(name) { }

				public short[] _FrequencyLevels = Array.Empty<short>();

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
					public static class SixBand
					{
						public static readonly IEqualiserSettings.IPreset Bass = new IEqualiserSettings.IPreset.Default(nameof(Bass))
						{
							FrequencyLevels = new short[] { 50, 20, 0, 0, 0, 0 },
						};
						public static readonly IEqualiserSettings.IPreset Flat = new IEqualiserSettings.IPreset.Default(nameof(Flat))
						{
							FrequencyLevels = new short[] { 0, 0, 0, 0, 0, 0 },
						};
						public static readonly IEqualiserSettings.IPreset Treble = new IEqualiserSettings.IPreset.Default(nameof(Treble))
						{
							FrequencyLevels = new short[] { 0, 0, 0, 0, 20, 50 },
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
					public static class EightBand
					{
						public static readonly IEqualiserSettings.IPreset Bass = new IEqualiserSettings.IPreset.Default(nameof(Bass))
						{
							FrequencyLevels = new short[] { 50, 20, 10, 0, 0, 0, 0, 0 },
						};
						public static readonly IEqualiserSettings.IPreset Flat = new IEqualiserSettings.IPreset.Default(nameof(Flat))
						{
							FrequencyLevels = new short[] { 0, 0, 0, 0, 0, 0, 0, 0 },
						};
						public static readonly IEqualiserSettings.IPreset Treble = new IEqualiserSettings.IPreset.Default(nameof(Treble))
						{
							FrequencyLevels = new short[] { 0, 0, 0, 0, 0, 10, 20, 50 },
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
					public static class TenBand
					{
						public static readonly IEqualiserSettings.IPreset Bass = new IEqualiserSettings.IPreset.Default(nameof(Bass))
						{
							FrequencyLevels = new short[] { 50, 20, 10, 0, 0, 0, 0, 0, 0, 0 },
						};
						public static readonly IEqualiserSettings.IPreset Flat = new IEqualiserSettings.IPreset.Default(nameof(Flat))
						{
							FrequencyLevels = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
						};
						public static readonly IEqualiserSettings.IPreset Treble = new IEqualiserSettings.IPreset.Default(nameof(Treble))
						{
							FrequencyLevels = new short[] { 0, 0, 0, 0, 0, 0, 0, 10, 20, 50 },
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
				}

				public static IEqualiserSettings.IPresetable FromPreset(IEqualiserSettings.IPreset? preset, BandType bandtype, bool withallpresets = false)
				{
					preset ??= bandtype switch
					{
						BandType.Six => Presets.SixBand.Default,
						BandType.Eight => Presets.EightBand.Default,
						BandType.Ten => Presets.TenBand.Default,

						_ => throw new ArgumentException(string.Format("Invalid Band Type '{0}'", bandtype)),
					};

					IEqualiserSettings.IPresetable presetable = new Default(preset, null)
					{
						IsEnabled = IsEnabled
					};

					if (withallpresets)
						presetable.AllPresets = presetable.AllPresets
							.Append(presetable.CurrentPreset)
							.Concat(bandtype switch
							{
								BandType.Six => Presets.SixBand.AsEnumerable(),
								BandType.Eight => Presets.EightBand.AsEnumerable(),
								BandType.Ten => Presets.TenBand.AsEnumerable(),

								_ => throw new ArgumentException(string.Format("Invalid Band Type '{0}'", bandtype)),
						
							}).Distinct();

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
