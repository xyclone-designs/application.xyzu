using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Settings.Audio
{
	public interface ILoudnessEnhancerSettings<T> : IAudioSettings<T> { }
	public interface ILoudnessEnhancerSettings : IAudioSettings
	{
		public new interface IPreset<T> : ISettings.IPreset<T> 
		{
			T TargetGain { get; set; }
		}
		public new interface IPreset : ISettings.IPreset 
		{
			short TargetGain { get; set; }

			public new class Keys : IPreset.Keys
			{
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(ILoudnessEnhancerSettings) + "." + nameof(IPreset);

				public static string PresetName(string name) => string.Format("{0}.{1}", Base, name);

				public static string TargetGain(string name) => string.Format("{0}.{1}", PresetName(name), nameof(TargetGain));

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
				public static readonly short TargetGainLower = 0;
				public static readonly short TargetGainUpper = 1_000;
			}

			public new class Default : IPreset.Default, ILoudnessEnhancerSettings.IPreset
			{
				public Default(string name) : base(name) { }

				public short _TargetGain;

				public short TargetGain
				{
					get => _TargetGain;
					set
					{
						_TargetGain = value;

						OnPropertyChanged();
					}
				}

				public override void SetFromKey(string key, object? value)
				{
					base.SetFromKey(key, value);

					switch (true)
					{
						case true when key == Keys.TargetGain(Name) && value is short targetgain:
							TargetGain = targetgain;
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
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(ILoudnessEnhancerSettings) + "." + nameof(IPresetable);

				public const string IsEnabled = Base + "." + nameof(IsEnabled);
				public const string CurrentPreset = Base + "." + nameof(CurrentPreset);
			}
			public new static class Defaults
			{
				public const bool IsEnabled = false;

				public static class Presets
				{
					public static readonly ILoudnessEnhancerSettings.IPreset Min = new ILoudnessEnhancerSettings.IPreset.Default(nameof(Min))
					{
						TargetGain = ILoudnessEnhancerSettings.IPreset.Ranges.TargetGainLower
					};
					public static readonly ILoudnessEnhancerSettings.IPreset Moderate = new ILoudnessEnhancerSettings.IPreset.Default(nameof(Moderate))
					{
						TargetGain = 200
					};
					public static readonly ILoudnessEnhancerSettings.IPreset Max = new ILoudnessEnhancerSettings.IPreset.Default(nameof(Max))
					{
						TargetGain = ILoudnessEnhancerSettings.IPreset.Ranges.TargetGainUpper,
					};

					public static readonly ILoudnessEnhancerSettings.IPreset Default = Min;

					public static IEnumerable<ILoudnessEnhancerSettings.IPreset> AsEnumerable()
					{
						return Enumerable.Empty<ILoudnessEnhancerSettings.IPreset>()
							.Append(Min)
							.Append(Moderate)
							.Append(Max);
					}
				}

				public static ILoudnessEnhancerSettings.IPresetable FromPreset(ILoudnessEnhancerSettings.IPreset? preset, bool withallpresets = false)
				{
					ILoudnessEnhancerSettings.IPresetable presetable = new Default(preset ?? Presets.Default, null)
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

			public new class Default : IPresetable.Default<ILoudnessEnhancerSettings.IPreset>, ILoudnessEnhancerSettings.IPresetable
			{
				public Default(ILoudnessEnhancerSettings.IPreset currentpreset, IEnumerable<ILoudnessEnhancerSettings.IPreset>? allpresets) : base(currentpreset, allpresets) { }
			}
		}
	}
}
