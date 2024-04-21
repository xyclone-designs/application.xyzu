#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Settings.Audio;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;
using XyzuSeekBarPreference = Xyzu.Preference.SeekBarPreference;
using XyzuSwitchPreference = Xyzu.Preference.SwitchPreference;

namespace Xyzu.Fragments.Settings.Audio
{
	[Register(FragmentName)]
	public class EnvironmentalReverbPreferenceFragment : BasePreferenceFragment, IEnvironmentalReverbSettings.IPresetable
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.Audio.EnvironmentalReverbPreferenceFragment";

		public static class Keys
		{
			public const string IsEnabled = "settings_audio_environmentalreverb_isenabled_switchpreference_key";
			public const string CurrentPreset = "settings_audio_environmentalreverb_currentpreset_listpreference_key";
			public const string DecayHFRatio = "settings_audio_environmentalreverb_decayhfratio_seekbarpreference_key";
			public const string DecayTime = "settings_audio_environmentalreverb_decaytime_seekbarpreference_key";
			public const string Density = "settings_audio_environmentalreverb_density_seekbarpreference_key";
			public const string Diffusion = "settings_audio_environmentalreverb_diffusion_seekbarpreference_key";
			public const string ReflectionsDelay = "settings_audio_environmentalreverb_reflectionsdelay_seekbarpreference_key";
			public const string ReflectionsLevel = "settings_audio_environmentalreverb_reflectionslevel_seekbarpreference_key";
			public const string ReverbDelay = "settings_audio_environmentalreverb_reverbdelay_seekbarpreference_key";
			public const string ReverbLevel = "settings_audio_environmentalreverb_reverblevel_seekbarpreference_key";
			public const string RoomHFLevel = "settings_audio_environmentalreverb_roomhflevel_seekbarpreference_key";
			public const string RoomLevel = "settings_audio_environmentalreverb_roomlevel_seekbarpreference_key";
		}

		private bool _IsEnabled;
		private IEnvironmentalReverbSettings.IPreset? _CurrentPreset;
		private IEnumerable<IEnvironmentalReverbSettings.IPreset>? _AllPresets;
		private short _DecayHFRatio;
		private int _DecayTime;
		private short _Density;
		private short _Diffusion;
		private int _ReflectionsDelay;
		private short _ReflectionsLevel;
		private int _ReverbDelay;
		private short _ReverbLevel;
		private short _RoomHFLevel;
		private short _RoomLevel;

		public bool IsEnabled
		{
			get => _IsEnabled;
			set
			{
				_IsEnabled = value;

				OnPropertyChanged();
			}
		}
		public IEnvironmentalReverbSettings.IPreset CurrentPreset
		{
			get => _CurrentPreset ??= IEnvironmentalReverbSettings.IPresetable.Defaults.Presets.Default;
			set
			{
				if (_CurrentPreset != null)
					_CurrentPreset.PropertyChanged -= CurrentPresetPropertyChanged;

				_CurrentPreset = value;

				_CurrentPreset.PropertyChanged += CurrentPresetPropertyChanged;

				OnPropertyChanged();
			}
		}
		public IEnumerable<IEnvironmentalReverbSettings.IPreset> AllPresets
		{
			get => _AllPresets ??= IEnvironmentalReverbSettings.IPresetable.Defaults.Presets.AsEnumerable();
			set
			{
				_AllPresets = value;

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
		public int DecayTime
		{
			get => _DecayTime;
			set
			{
				_DecayTime = value;

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
		public int ReflectionsDelay
		{
			get => _ReflectionsDelay;
			set
			{
				_ReflectionsDelay = value;

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
		public int ReverbDelay
		{
			get => _ReverbDelay;
			set
			{
				_ReverbDelay = value;

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
		public short RoomHFLevel
		{
			get => _RoomHFLevel;
			set
			{
				_RoomHFLevel = value;

				OnPropertyChanged();
			}
		}
		public short RoomLevel
		{
			get => _RoomLevel;
			set
			{
				_RoomLevel = value;

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? IsEnabledPreference { get; set; }
		public XyzuListPreference? CurrentPresetPreference { get; set; }
		public XyzuSeekBarPreference? DecayHFRatioPreference { get; set; }
		public XyzuSeekBarPreference? DecayTimePreference { get; set; }
		public XyzuSeekBarPreference? DensityPreference { get; set; }
		public XyzuSeekBarPreference? DiffusionPreference { get; set; }
		public XyzuSeekBarPreference? ReflectionsDelayPreference { get; set; }
		public XyzuSeekBarPreference? ReflectionsLevelPreference { get; set; }
		public XyzuSeekBarPreference? ReverbDelayPreference { get; set; }
		public XyzuSeekBarPreference? ReverbLevelPreference { get; set; }
		public XyzuSeekBarPreference? RoomHFLevelPreference { get; set; }
		public XyzuSeekBarPreference? RoomLevelPreference { get; set; }

		public void CurrentPresetPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(IEnvironmentalReverbSettings.IPreset.DecayHFRatio) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null && 
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.DecayHFRatio != CurrentPreset.DecayHFRatio:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.DecayHFRatio = CurrentPreset.DecayHFRatio;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.DecayTime) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.DecayTime != CurrentPreset.DecayTime:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.DecayTime = CurrentPreset.DecayTime;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.Density) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.Density != CurrentPreset.Density:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.Density = CurrentPreset.Density;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.Diffusion) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.Diffusion != CurrentPreset.Diffusion:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.Diffusion = CurrentPreset.Diffusion;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.ReflectionsDelay) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.ReflectionsDelay != CurrentPreset.ReflectionsDelay:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.ReflectionsDelay = CurrentPreset.ReflectionsDelay;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.ReflectionsLevel) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.ReflectionsLevel != CurrentPreset.ReflectionsLevel:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.ReflectionsLevel = CurrentPreset.ReflectionsLevel;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.ReverbDelay) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.ReverbDelay != CurrentPreset.ReverbDelay:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.ReverbDelay = CurrentPreset.ReverbDelay;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.ReverbLevel) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.ReverbLevel != CurrentPreset.ReverbLevel:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.ReverbLevel = CurrentPreset.ReverbLevel;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.RoomHFLevel) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.RoomHFLevel != CurrentPreset.RoomHFLevel:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.RoomHFLevel = CurrentPreset.RoomHFLevel;
					break;

				case nameof(IEnvironmentalReverbSettings.IPreset.RoomLevel) when
				XyzuPlayer.Instance.SettingsEnvironmentalReverb != null &&
				XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.RoomLevel != CurrentPreset.RoomLevel:
					XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.RoomLevel = CurrentPreset.RoomLevel;
					break;

				default: break;
			}
		}

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_audio_environmentalreverb_title);

			AddPreferenceChangeHandler(
				IsEnabledPreference,
				CurrentPresetPreference,
				DecayHFRatioPreference,
				DecayTimePreference,
				DensityPreference,
				DiffusionPreference,
				ReflectionsDelayPreference,
				ReflectionsLevelPreference,
				ReverbDelayPreference,
				ReverbLevelPreference,
				RoomHFLevelPreference,
				RoomLevelPreference);

			IEnvironmentalReverbSettings.IPresetable environmentalreverbsettings = XyzuSettings.Instance.GetAudioEnvironmentalReverb();

			IsEnabled = environmentalreverbsettings.IsEnabled;
			CurrentPreset = environmentalreverbsettings.CurrentPreset;
			AllPresets = environmentalreverbsettings.AllPresets;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				IsEnabledPreference,
				CurrentPresetPreference,
				DecayHFRatioPreference,
				DecayTimePreference,
				DensityPreference,
				DiffusionPreference,
				ReflectionsDelayPreference,
				ReflectionsLevelPreference,
				ReverbDelayPreference,
				ReverbLevelPreference,
				RoomHFLevelPreference,
				RoomLevelPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutAudioEnvironmentalReverb(this)
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_audio_environmentalreverb, rootKey);
			InitPreferences(
				IsEnabledPreference ??= FindPreference(Keys.IsEnabled) as XyzuSwitchPreference,
				CurrentPresetPreference ??= FindPreference(Keys.CurrentPreset) as XyzuListPreference,
				DecayHFRatioPreference ??= FindPreference(Keys.DecayHFRatio) as XyzuSeekBarPreference,
				DecayTimePreference ??= FindPreference(Keys.DecayTime) as XyzuSeekBarPreference,
				DensityPreference ??= FindPreference(Keys.Density) as XyzuSeekBarPreference,
				DiffusionPreference ??= FindPreference(Keys.Diffusion) as XyzuSeekBarPreference,
				ReflectionsDelayPreference ??= FindPreference(Keys.ReflectionsDelay) as XyzuSeekBarPreference,
				ReflectionsLevelPreference ??= FindPreference(Keys.ReflectionsLevel) as XyzuSeekBarPreference,
				ReverbDelayPreference ??= FindPreference(Keys.ReverbDelay) as XyzuSeekBarPreference,
				ReverbLevelPreference ??= FindPreference(Keys.ReverbLevel) as XyzuSeekBarPreference,
				RoomHFLevelPreference ??= FindPreference(Keys.RoomHFLevel) as XyzuSeekBarPreference,
				RoomLevelPreference ??= FindPreference(Keys.RoomLevel) as XyzuSeekBarPreference);

			if (IsEnabledPreference != null)
			{
				IsEnabledPreference.OnPreferenceChangeListener = this;
			}

			if (CurrentPresetPreference != null)
			{
				CurrentPresetPreference.OnPreferenceChangeListener = this;
			}

			if (DecayHFRatioPreference != null)
			{
				DecayHFRatioPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.DecayHFRatioLower;
				DecayHFRatioPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.DecayHFRatioUpper;
			}

			if (DecayTimePreference != null)
			{
				DecayTimePreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.DecayTimeLower;
				DecayTimePreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.DecayTimeUpper;
			}

			if (DensityPreference != null)
			{
				DensityPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.DensityLower;
				DensityPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.DensityUpper;
			}

			if (DiffusionPreference != null)
			{
				DiffusionPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.DiffusionLower;
				DiffusionPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.DiffusionUpper;
			}

			if (ReflectionsDelayPreference != null)
			{
				ReflectionsDelayPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.ReflectionsDelayLower;
				ReflectionsDelayPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.ReflectionsDelayUpper;
			}

			if (ReflectionsLevelPreference != null)
			{
				ReflectionsLevelPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.ReflectionsLevelLower;
				ReflectionsLevelPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.ReflectionsLevelUpper;
			}
			
			if (ReverbDelayPreference != null)
			{
				ReverbDelayPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.ReverbDelayLower;
				ReverbDelayPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.ReverbDelayUpper;
			}

			if (ReverbLevelPreference != null)
			{
				ReverbLevelPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.ReverbLevelLower;
				ReverbLevelPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.ReverbDelayUpper;
			}
			
			if (RoomHFLevelPreference != null)
			{
				RoomHFLevelPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.RoomHFLevelLower;
				RoomHFLevelPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.RoomHFLevelUpper;
			}

			if (RoomLevelPreference != null)
			{
				RoomLevelPreference.Min = IEnvironmentalReverbSettings.IPreset.Ranges.RoomLevelLower;
				RoomLevelPreference.Max = IEnvironmentalReverbSettings.IPreset.Ranges.RoomLevelUpper;
			}
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(IsEnabled):
					{
						if (IsEnabledPreference != null && IsEnabledPreference.Checked != IsEnabled)
							IsEnabledPreference.Checked = IsEnabled;

						if (XyzuPlayer.Instance.SettingsEnvironmentalReverb != null && XyzuPlayer.Instance.SettingsEnvironmentalReverb.IsEnabled != IsEnabled)
							XyzuPlayer.Instance.SettingsEnvironmentalReverb.IsEnabled = IsEnabled;

					} break;

				case nameof(AllPresets):
				case nameof(CurrentPreset):
					{
						if (XyzuPlayer.Instance.SettingsEnvironmentalReverb != null && XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.Name != CurrentPreset.Name)
							XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset = CurrentPreset;

						if (AllPresets.Index(preset => string.Equals(preset.Name, CurrentPreset?.Name, StringComparison.OrdinalIgnoreCase)) is int currentpresetindex)
						{
							string[] presetnames = AllPresets.Select(preset => preset.Name).ToArray();

							CurrentPresetPreference?.SetEntries(presetnames);
							CurrentPresetPreference?.SetEntryValues(presetnames);
							CurrentPresetPreference?.SetValueIndex(currentpresetindex);
						}

						if (propertyName == nameof(CurrentPreset))
						{
							if (DecayHFRatioPreference != null && DecayHFRatioPreference.Value != CurrentPreset.DecayHFRatio)
								DecayHFRatioPreference.Value = CurrentPreset.DecayHFRatio;

							if (DecayTimePreference != null && DecayTimePreference.Value != CurrentPreset.DecayTime) 
								DecayTimePreference.Value = CurrentPreset.DecayTime;

							if (DensityPreference != null && DensityPreference.Value != CurrentPreset.Density) 
								DensityPreference.Value = CurrentPreset.Density;

							if (DiffusionPreference != null && DiffusionPreference.Value != CurrentPreset.Diffusion) 
								DiffusionPreference.Value = CurrentPreset.Diffusion;

							if (ReflectionsDelayPreference != null && ReflectionsDelayPreference.Value != CurrentPreset.ReflectionsDelay) 
								ReflectionsDelayPreference.Value = CurrentPreset.ReflectionsDelay;

							if (ReflectionsLevelPreference != null && ReflectionsLevelPreference.Value != CurrentPreset.ReflectionsLevel) 
								ReflectionsLevelPreference.Value = CurrentPreset.ReflectionsLevel;

							if (ReverbDelayPreference != null && ReverbDelayPreference.Value != CurrentPreset.ReverbDelay) 
								ReverbDelayPreference.Value = CurrentPreset.ReverbDelay;

							if (ReverbLevelPreference != null && ReverbLevelPreference.Value != CurrentPreset.ReverbLevel) 
								ReverbLevelPreference.Value = CurrentPreset.ReverbLevel;

							if (RoomHFLevelPreference != null && RoomHFLevelPreference.Value != CurrentPreset.RoomHFLevel) 
								RoomHFLevelPreference.Value = CurrentPreset.RoomHFLevel;

							if (RoomLevelPreference != null && RoomLevelPreference.Value != CurrentPreset.RoomLevel) 
								RoomLevelPreference.Value = CurrentPreset.RoomLevel;
						}

					}
					break;

				default: break;
			}
		}
		public override bool OnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == IsEnabledPreference:
					IsEnabled = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == CurrentPresetPreference:
					if (AllPresets.FirstOrDefault(preset => string.Equals(preset.Name, newvalue.ToString(), StringComparison.OrdinalIgnoreCase)) is IEnvironmentalReverbSettings.IPreset currentpreset)
						CurrentPreset = currentpreset;
					return true;

				case true when
				preference == DecayHFRatioPreference:
					CurrentPreset.DecayHFRatio = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == DecayTimePreference:
					CurrentPreset.DecayTime = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == DensityPreference:
					CurrentPreset.Density = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == DiffusionPreference:
					CurrentPreset.Diffusion = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == ReflectionsDelayPreference:
					CurrentPreset.ReflectionsDelay = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == ReflectionsLevelPreference:
					CurrentPreset.ReflectionsLevel = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == ReverbDelayPreference:
					CurrentPreset.ReverbDelay = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == ReverbLevelPreference:
					CurrentPreset.ReverbLevel = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == RoomHFLevelPreference:
					CurrentPreset.RoomHFLevel = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;
				
				case true when
				preference == RoomLevelPreference:
					CurrentPreset.RoomLevel = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;

				default: return base.OnPreferenceChange(preference, newvalue);
			}
		}
	}
}