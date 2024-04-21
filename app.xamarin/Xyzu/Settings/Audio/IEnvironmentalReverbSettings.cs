using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyzu.Settings.Audio
{
	public interface IEnvironmentalReverbSettings<T> : IAudioSettings<T> { }
	public interface IEnvironmentalReverbSettings : IAudioSettings
	{
		public new interface IPreset<T> : ISettings.IPreset<T> 
		{
			T RoomLevel { get; set; }
			T RoomHFLevel { get; set; }
			T DecayTime { get; set; }
			T DecayHFRatio { get; set; }
			T ReflectionsLevel { get; set; }
			T ReflectionsDelay { get; set; }
			T ReverbLevel { get; set; }
			T ReverbDelay { get; set; }
			T Diffusion { get; set; }
			T Density { get; set; }
		}
		public new interface IPreset : ISettings.IPreset 
		{
			short RoomLevel { get; set; }
			short RoomHFLevel { get; set; }
			int DecayTime { get; set; }
			short DecayHFRatio { get; set; }
			short Density { get; set; }
			short Diffusion { get; set; }
			short ReflectionsLevel { get; set; }
			int ReflectionsDelay { get; set; }
			short ReverbLevel { get; set; }
			int ReverbDelay { get; set; }
																					   
			public new class Keys : IPreset.Keys
			{
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IEnvironmentalReverbSettings) + "." + nameof(IPreset);

				public static string PresetName(string name) => string.Format("{0}.{1}", Base, name);						   

				public static string RoomLevel(string name) => string.Format("{0}.{1}", PresetName(name), nameof(RoomLevel));
				public static string RoomHFLevel(string name) => string.Format("{0}.{1}", PresetName(name), nameof(RoomHFLevel));
				public static string DecayTime(string name) => string.Format("{0}.{1}", PresetName(name), nameof(DecayTime));
				public static string DecayHFRatio(string name) => string.Format("{0}.{1}", PresetName(name), nameof(DecayHFRatio));
				public static string ReflectionsLevel(string name) => string.Format("{0}.{1}", PresetName(name), nameof(ReflectionsLevel));
				public static string ReflectionsDelay(string name) => string.Format("{0}.{1}", PresetName(name), nameof(ReflectionsDelay));
				public static string ReverbLevel(string name) => string.Format("{0}.{1}", PresetName(name), nameof(ReverbLevel));
				public static string ReverbDelay(string name) => string.Format("{0}.{1}", PresetName(name), nameof(ReverbDelay));
				public static string Diffusion(string name) => string.Format("{0}.{1}", PresetName(name), nameof(Diffusion));
				public static string Density(string name) => string.Format("{0}.{1}", PresetName(name), nameof(Density));

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
				public const short DiffusionLower = 0;
				public const short DiffusionUpper = 1_000;
				public const int DecayTimeLower = 100;
				public const int DecayTimeUpper = 20_000;
				public const short DecayHFRatioLower = 100;
				public const short DecayHFRatioUpper = 2_000;
				public const short DensityLower = 0;
				public const short DensityUpper = 1_000;
				public const short ReflectionsLevelLower = -9_000;
				public const short ReflectionsLevelUpper = 1_000;
				public const int ReflectionsDelayLower = 0;
				public const int ReflectionsDelayUpper = 300;
				public const int ReverbDelayLower = 0;
				public const int ReverbDelayUpper = 100;
				public const short ReverbLevelLower = -9_000;
				public const short ReverbLevelUpper = -2_000;
				public const short RoomHFLevelLower = -9_000;
				public const short RoomHFLevelUpper = 0;
				public const short RoomLevelLower = -9_000;
				public const short RoomLevelUpper = 0;
			}

			public new class Default : IPreset.Default, IEnvironmentalReverbSettings.IPreset
			{
				public Default(string name) : base(name) { }

				private short _RoomLevel;
				private short _RoomHFLevel;
				private int _DecayTime;
				private short _DecayHFRatio;
				private short _ReflectionsLevel;
				private int _ReflectionsDelay;
				private short _ReverbLevel;
				private int _ReverbDelay;
				private short _Diffusion;
				private short _Density;

				public short RoomLevel
				{
					get => _RoomLevel;
					set
					{
						_RoomLevel = value;

						OnPropertyChanged();
					}
				}
				public short RoomHFLevel
				{
					get => _RoomHFLevel;
					set
					{
						_RoomHFLevel = value;

						OnPropertyChanged();
					}
				}
				public int DecayTime
				{
					get => _DecayTime;
					set
					{
						_DecayTime = value;

						OnPropertyChanged();
					}
				}
				public short DecayHFRatio
				{
					get => _DecayHFRatio;
					set
					{
						_DecayHFRatio = value;

						OnPropertyChanged();
					}
				}
				public short Density
				{
					get => _Density;
					set
					{
						_Density = value;

						OnPropertyChanged();
					}
				}
				public short Diffusion
				{
					get => _Diffusion;
					set
					{
						_Diffusion = value;

						OnPropertyChanged();
					}
				}
				public short ReflectionsLevel
				{
					get => _ReflectionsLevel;
					set
					{
						_ReflectionsLevel = value;

						OnPropertyChanged();
					}
				}
				public int ReflectionsDelay
				{
					get => _ReflectionsDelay;
					set
					{
						_ReflectionsDelay = value;

						OnPropertyChanged();
					}
				}
				public short ReverbLevel
				{
					get => _ReverbLevel;
					set
					{
						_ReverbLevel = value;

						OnPropertyChanged();
					}
				}
				public int ReverbDelay
				{
					get => _ReverbDelay;
					set
					{
						_ReverbDelay = value;

						OnPropertyChanged();
					}
				}

				public override void SetFromKey(string key, object? value)
				{
					base.SetFromKey(key, value);

					switch (true)
					{
						case true when key == Keys.RoomLevel(Name) && value is short roomlevel:
							RoomLevel = roomlevel;
							break;

						case true when key == Keys.DecayTime(Name) && value is short decaytime:
							DecayTime = decaytime;
							break;

						case true when key == Keys.DecayHFRatio(Name) && value is short decayhfratio:
							DecayHFRatio = decayhfratio;
							break;

						case true when key == Keys.ReflectionsLevel(Name) && value is short reflectionslevel:
							ReflectionsLevel = reflectionslevel;
							break;

						case true when key == Keys.ReflectionsDelay(Name) && value is int reflectionsdelay:
							ReflectionsDelay = reflectionsdelay;
							break;

						case true when key == Keys.ReverbDelay(Name) && value is int reverbdelay:
							ReverbDelay = reverbdelay;
							break;

						case true when key == Keys.RoomLevel(Name) && value is short roomlevel:
							RoomLevel = roomlevel;
							break;

						case true when key == Keys.Diffusion(Name) && value is short diffusion:
							Diffusion = diffusion;
							break;

						case true when key == Keys.Density(Name) && value is short density:
							Density = density;
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
				public new const string Base = IAudioSettings.Keys.Base + "." + nameof(IEnvironmentalReverbSettings) + "." + nameof(IPresetable);

				public const string IsEnabled = Base + "." + nameof(IsEnabled);
				public const string CurrentPreset = Base + "." + nameof(CurrentPreset);
			}
			public new static class Defaults
			{
				public const bool IsEnabled = false;

				public static class Presets
				{
					public static readonly IEnvironmentalReverbSettings.IPreset Basic = new IEnvironmentalReverbSettings.IPreset.Default(nameof(Basic))
					{
						Diffusion = 750,
						DecayTime = 50,
						DecayHFRatio = 30,
						Density = 400,
						ReflectionsLevel = -2_000,
						ReflectionsDelay = 20,
						ReverbDelay = 15,
						ReverbLevel = -2_000,
						RoomHFLevel = -2_000,
						RoomLevel = 0,
					};

					public static readonly IEnvironmentalReverbSettings.IPreset Default = Basic;

					public static IEnumerable<IEnvironmentalReverbSettings.IPreset> AsEnumerable()
					{
						return Enumerable.Empty<IEnvironmentalReverbSettings.IPreset>()
							.Append(Basic);
					}
				}

				public static IEnvironmentalReverbSettings.IPresetable FromPreset(IEnvironmentalReverbSettings.IPreset? preset, bool withallpresets = false)
				{
					IEnvironmentalReverbSettings.IPresetable presetable = new Default(preset ?? Presets.Default, null)
					{
						IsEnabled = IsEnabled,
						CurrentPreset = preset ?? Presets.Default,
					};

					if (withallpresets)
						presetable.AllPresets = presetable.AllPresets
							.Concat(Presets.AsEnumerable())
							.Append(presetable.CurrentPreset)
							.Distinct();

					return presetable;
				}
			}
			public new class Default : IPresetable.Default<IEnvironmentalReverbSettings.IPreset>, IEnvironmentalReverbSettings.IPresetable
			{
				public Default(IEnvironmentalReverbSettings.IPreset currentpreset, IEnumerable<IEnvironmentalReverbSettings.IPreset>? allpresets) : base(currentpreset, allpresets) { }
			}
		}
	}
}
