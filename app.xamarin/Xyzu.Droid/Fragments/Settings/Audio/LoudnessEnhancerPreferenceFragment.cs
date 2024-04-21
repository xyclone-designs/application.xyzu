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
	public class LoudnessEnhancerPreferenceFragment : BasePreferenceFragment, ILoudnessEnhancerSettings.IPresetable
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.Audio.LoudnessEnhancerPreferenceFragment";

		public static class Keys
		{
			public const string IsEnabled = "settings_audio_loudnessenhancer_isenabled_switchpreference_key";
			public const string CurrentPreset = "settings_audio_loudnessenhancer_currentpreset_listpreference_key";
			public const string TargetGain = "settings_audio_loudnessenhancer_targetgain_seekbarpreference_key";
		}

		private bool _IsEnabled;
		private ILoudnessEnhancerSettings.IPreset? _CurrentPreset;
		private IEnumerable<ILoudnessEnhancerSettings.IPreset>? _AllPresets;

		public bool IsEnabled
		{
			get => _IsEnabled;
			set
			{
				_IsEnabled = value;

				OnPropertyChanged();
			}
		}
		public ILoudnessEnhancerSettings.IPreset CurrentPreset
		{
			get => _CurrentPreset ??= ILoudnessEnhancerSettings.IPresetable.Defaults.Presets.Default;
			set
			{
				if (_CurrentPreset != null) _CurrentPreset.PropertyChanged -= CurrentPresetPropertyChanged;

				_CurrentPreset = value;

				_CurrentPreset.PropertyChanged += CurrentPresetPropertyChanged;

				OnPropertyChanged();
			}
		}
		public IEnumerable<ILoudnessEnhancerSettings.IPreset> AllPresets
		{
			get => _AllPresets ?? ILoudnessEnhancerSettings.IPresetable.Defaults.Presets.AsEnumerable();
			set
			{
				_AllPresets = value;

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? IsEnabledPreference { get; set; }
		public XyzuListPreference? CurrentPresetPreference { get; set; }
		public XyzuSeekBarPreference? TargetGainPreference { get; set; }

		public void CurrentPresetPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(ILoudnessEnhancerSettings.IPreset.TargetGain) when
				XyzuPlayer.Instance.SettingsLoudnessEnhancer != null &&
				XyzuPlayer.Instance.SettingsLoudnessEnhancer.CurrentPreset.TargetGain != CurrentPreset.TargetGain:
					XyzuPlayer.Instance.SettingsLoudnessEnhancer.CurrentPreset.TargetGain = CurrentPreset.TargetGain;
					break;

				default: break;
			}
		}

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_audio_loudnessenhancer_title);

			AddPreferenceChangeHandler(
				IsEnabledPreference, 
				CurrentPresetPreference, 
				TargetGainPreference);

			ILoudnessEnhancerSettings.IPresetable loudnessenhancersettings = XyzuSettings.Instance.GetAudioLoudnessEnhancer();

			IsEnabled = loudnessenhancersettings.IsEnabled;
			CurrentPreset = loudnessenhancersettings.CurrentPreset;
			AllPresets = loudnessenhancersettings.AllPresets;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				IsEnabledPreference,
				CurrentPresetPreference,
				TargetGainPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutAudioLoudnessEnhancer(this)
				.Apply();
		}
		
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_audio_loudnessenhancer, rootKey);
			InitPreferences(
				IsEnabledPreference = FindPreference(Keys.IsEnabled) as XyzuSwitchPreference,
				CurrentPresetPreference = FindPreference(Keys.CurrentPreset) as XyzuListPreference,
				TargetGainPreference = FindPreference(Keys.TargetGain) as XyzuSeekBarPreference);

			if (TargetGainPreference != null)
			{
				TargetGainPreference.Min = ILoudnessEnhancerSettings.IPreset.Ranges.TargetGainLower;
				TargetGainPreference.Max = ILoudnessEnhancerSettings.IPreset.Ranges.TargetGainUpper;
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

						if (XyzuPlayer.Instance.SettingsLoudnessEnhancer != null && XyzuPlayer.Instance.SettingsLoudnessEnhancer.IsEnabled != IsEnabled)
							XyzuPlayer.Instance.SettingsLoudnessEnhancer.IsEnabled = IsEnabled;

					} break;

				case nameof(AllPresets):
				case nameof(CurrentPreset):
					{
						if (XyzuPlayer.Instance.SettingsLoudnessEnhancer != null && string.Equals(XyzuPlayer.Instance.SettingsLoudnessEnhancer.CurrentPreset.Name, CurrentPreset.Name, StringComparison.OrdinalIgnoreCase) is false)
							XyzuPlayer.Instance.SettingsLoudnessEnhancer.CurrentPreset = CurrentPreset;

						if (AllPresets.Index(preset => string.Equals(preset.Name, CurrentPreset?.Name, StringComparison.OrdinalIgnoreCase)) is int currentpresetindex)
						{
							string[] presetnames = AllPresets.Select(preset => preset.Name).ToArray();

							CurrentPresetPreference?.SetEntries(presetnames);
							CurrentPresetPreference?.SetEntryValues(presetnames);
							CurrentPresetPreference?.SetValueIndex(currentpresetindex);
						}

						if (propertyName == nameof(CurrentPreset))
						{
							if (TargetGainPreference != null && TargetGainPreference.Value != CurrentPreset.TargetGain)
								TargetGainPreference.Value = CurrentPreset.TargetGain;
						}
					
					} break;

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
					if (AllPresets.FirstOrDefault(preset => string.Equals(preset.Name, newvalue.ToString(), StringComparison.OrdinalIgnoreCase)) is ILoudnessEnhancerSettings.IPreset currentpreset)
						CurrentPreset = currentpreset;
					return true;

				case true when
				preference == TargetGainPreference:
					CurrentPreset.TargetGain = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;

				default: return base.OnPreferenceChange(preference, newvalue);
			}
		}
	}
}