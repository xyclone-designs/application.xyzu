using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Settings.Audio
{
	public interface IVolumeControlSettings<T> : IAudioSettings<T> { }
	public interface IVolumeControlSettings : IAudioSettings
	{
		public new interface IPreset<T> : ISettings.IPreset<T> { }
		public new interface IPreset : ISettings.IPreset 
		{
			short BalancePosition { get; set; }
			short BassBoostStrength { get; set; }
			short LoudnessEnhancerTargetGain { get; set; }
			float PlaybackPitch { get; set; }
			float PlaybackSpeed { get; set; } 

			public new class Keys : IPreset.Keys
			{
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IVolumeControlSettings) + "." + nameof(IPreset);

				public static string IsDefault(string name) => string.Format("{0}.{1}", PresetName(name), nameof(IsDefault));
				public static string PresetName(string name) => string.Format("{0}.{1}", Base, name);
				public static string BalancePosition(string name) => string.Format("{0}.{1}", PresetName(name), nameof(BalancePosition));
				public static string BassBoostStrength(string name) => string.Format("{0}.{1}", PresetName(name), nameof(BassBoostStrength));
				public static string LoudnessEnhancerTargetGain(string name) => string.Format("{0}.{1}", PresetName(name), nameof(LoudnessEnhancerTargetGain));
				public static string PlaybackPitch(string name) => string.Format("{0}.{1}", PresetName(name), nameof(PlaybackPitch));
				public static string PlaybackSpeed(string name) => string.Format("{0}.{1}", PresetName(name), nameof(PlaybackSpeed));
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
				public static readonly short BalancePositionLower = -100;
				public static readonly short BalancePositionUpper = +100;
				public static readonly short BassBoostStrengthLower = 0_000;
				public static readonly short BassBoostStrengthUpper = 1_000;
				public static readonly short LoudnessEnhancerTargetGainLower = 0_000;
				public static readonly short LoudnessEnhancerTargetGainUpper = 1_000;
				public static readonly float PlaybackPitchLower = 0.0F;
				public static readonly float PlaybackPitchUpper = 2.0F;
				public static readonly float PlaybackSpeedLower = 0.0F;
				public static readonly float PlaybackSpeedUpper = 2.0F;
			}

			public new class Default : IPreset.Default, IVolumeControlSettings.IPreset
			{
				public Default(string name) : base(name) { }

				public Default(string name, IVolumeControlSettings.IPreset preset) : base(name, preset)
				{
					_BalancePosition = preset.BalancePosition;
					_BassBoostStrength = preset.BassBoostStrength;
					_LoudnessEnhancerTargetGain = preset.LoudnessEnhancerTargetGain;
					_PlaybackPitch = preset.PlaybackPitch;
					_PlaybackSpeed = preset.PlaybackSpeed;
				}

				public short _BalancePosition = 0;
				public short _BassBoostStrength = 0;
				public short _LoudnessEnhancerTargetGain = 0;
				public float _PlaybackPitch = 1F;
				public float _PlaybackSpeed = 1F;

				public short BalancePosition
				{
					get => _BalancePosition;
					set
					{
						_BalancePosition = value;

						OnPropertyChanged();
					}
				}
				public short BassBoostStrength
				{
					get => _BassBoostStrength;
					set
					{
						_BassBoostStrength = value;

						OnPropertyChanged();
					}
				}
				public short LoudnessEnhancerTargetGain
				{
					get => _LoudnessEnhancerTargetGain;
					set
					{
						_LoudnessEnhancerTargetGain = value;

						OnPropertyChanged();
					}
				}
				public float PlaybackPitch
				{
					get => _PlaybackPitch;
					set
					{
						_PlaybackPitch = value;

						OnPropertyChanged();
					}
				}
				public float PlaybackSpeed
				{
					get => _PlaybackSpeed;
					set
					{
						_PlaybackSpeed = value;

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

						case true when key == Keys.BalancePosition(Name) && value is short balance:
							BalancePosition = balance;
							break;

						case true when key == Keys.BassBoostStrength(Name) && value is short strength:
							BassBoostStrength = strength;
							break;
							
						case true when key == Keys.LoudnessEnhancerTargetGain(Name) && value is short targetgain:
							LoudnessEnhancerTargetGain = targetgain;
							break;
							
						case true when key == Keys.PlaybackPitch(Name) && value is float playbackpitch:
							PlaybackPitch = playbackpitch;
							break;
							
						case true when key == Keys.PlaybackSpeed(Name) && value is float playbackspeed:
							PlaybackSpeed = playbackspeed;
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
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IVolumeControlSettings) + "." + nameof(IPresetable);
				
				public const string IsEnabled = Base + "." + nameof(IsEnabled);
				public const string CurrentPreset = Base + "." + nameof(CurrentPreset);
			}
			public new static class Defaults
			{
				public const bool IsEnabled = false;

				public static class Presets
				{
					public static readonly IVolumeControlSettings.IPreset Min = new IVolumeControlSettings.IPreset.Default(nameof(Min))
					{
						IsDefault = true,
						BalancePosition = 0,
						BassBoostStrength = IVolumeControlSettings.IPreset.Ranges.BassBoostStrengthLower,
						LoudnessEnhancerTargetGain = IVolumeControlSettings.IPreset.Ranges.LoudnessEnhancerTargetGainLower,
						PlaybackPitch = 1F,
						PlaybackSpeed = 1F,
					};
					public static readonly IVolumeControlSettings.IPreset Moderate = new IVolumeControlSettings.IPreset.Default(nameof(Moderate))
					{
						IsDefault = true,
						BalancePosition = 0,
						BassBoostStrength = 200,
						LoudnessEnhancerTargetGain = 200,
						PlaybackPitch = 1F,
						PlaybackSpeed = 1F,
					};
					public static readonly IVolumeControlSettings.IPreset Max = new IVolumeControlSettings.IPreset.Default(nameof(Max))
					{
						IsDefault = true,
						BalancePosition = 0,
						BassBoostStrength = IVolumeControlSettings.IPreset.Ranges.BassBoostStrengthUpper,
						LoudnessEnhancerTargetGain = IVolumeControlSettings.IPreset.Ranges.LoudnessEnhancerTargetGainUpper,
						PlaybackPitch = 1F,
						PlaybackSpeed = 1F,
					};

					public static readonly IVolumeControlSettings.IPreset Default = Min;

					public static IEnumerable<IVolumeControlSettings.IPreset> AsEnumerable()
					{
						return Enumerable.Empty<IVolumeControlSettings.IPreset>()
							.Append(Min)
							.Append(Moderate)
							.Append(Max);
					}
				}

				public static IVolumeControlSettings.IPresetable FromPreset(IVolumeControlSettings.IPreset? preset, bool withallpresets = false)
				{
					IVolumeControlSettings.IPresetable presetable = new Default(preset ?? Presets.Default, null)
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

			public new class Default : IPresetable.Default<IVolumeControlSettings.IPreset>, IVolumeControlSettings.IPresetable 
			{
				public Default(IVolumeControlSettings.IPreset currentpreset, IEnumerable<IVolumeControlSettings.IPreset>? allpresets) : base(currentpreset, allpresets) { }
			}
		}
	}
}
