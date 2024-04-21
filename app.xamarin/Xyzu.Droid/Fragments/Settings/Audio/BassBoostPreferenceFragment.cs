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
	public class BassBoostPreferenceFragment : BasePreferenceFragment, IBassBoostSettings.IPresetable
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.Audio.BassBoostPreferenceFragment";

		public static class Keys 
		{
			public const string IsEnabled = "settings_audio_bassboost_isenabled_switchpreference_key";
			public const string CurrentPreset = "settings_audio_bassboost_currentpreset_listpreference_key";
			public const string Strength = "settings_audio_bassboost_strength_seekbarpreference_key";
		}

		private bool _IsEnabled;
		private IBassBoostSettings.IPreset? _CurrentPreset;
		private IEnumerable<IBassBoostSettings.IPreset>? _AllPresets;

		public bool IsEnabled
		{
			get => _IsEnabled;
			set
			{
				_IsEnabled = value;

				OnPropertyChanged();
			}
		}
		public IBassBoostSettings.IPreset CurrentPreset
		{
			get => _CurrentPreset ??= IBassBoostSettings.IPresetable.Defaults.Presets.Default;
			set
			{
				if (_CurrentPreset != null)
					_CurrentPreset.PropertyChanged -= CurrentPresetPropertyChanged;

				_CurrentPreset = value;

				_CurrentPreset.PropertyChanged += CurrentPresetPropertyChanged;

				OnPropertyChanged();
			}
		}
		public IEnumerable<IBassBoostSettings.IPreset> AllPresets
		{
			get => _AllPresets ??= IBassBoostSettings.IPresetable.Defaults.Presets.AsEnumerable();
			set
			{
				_AllPresets = value;

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? IsEnabledPreference { get; set; }
		public XyzuListPreference? CurrentPresetPreference { get; set; }
		public XyzuSeekBarPreference? StrengthPreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_audio_bassboost_title);

			AddPreferenceChangeHandler(
				IsEnabledPreference,
				CurrentPresetPreference,
				StrengthPreference);

			IBassBoostSettings.IPresetable bassboostsettings = XyzuSettings.Instance.GetAudioBassBoost();

			IsEnabled = bassboostsettings.IsEnabled;
			CurrentPreset = bassboostsettings.CurrentPreset;
			AllPresets = bassboostsettings.AllPresets;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				IsEnabledPreference,
				CurrentPresetPreference,
				StrengthPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutAudioBassBoost(this)
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_audio_bassboost, rootKey);
			InitPreferences(
				IsEnabledPreference = FindPreference(Keys.IsEnabled) as XyzuSwitchPreference,
				CurrentPresetPreference = FindPreference(Keys.CurrentPreset) as XyzuListPreference,
				StrengthPreference = FindPreference(Keys.Strength) as XyzuSeekBarPreference);

			if (StrengthPreference != null)
			{
				StrengthPreference.Min = IBassBoostSettings.IPreset.Ranges.StrengthLower;
				StrengthPreference.Max = IBassBoostSettings.IPreset.Ranges.StrengthUpper;
			}
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch(propertyName)
			{
				case nameof(IsEnabled):
					{
						if (IsEnabledPreference != null && IsEnabledPreference.Checked != IsEnabled)
							IsEnabledPreference.Checked = IsEnabled;

						if (XyzuPlayer.Instance.SettingsBassBoost != null && XyzuPlayer.Instance.SettingsBassBoost.IsEnabled != IsEnabled)
							XyzuPlayer.Instance.SettingsBassBoost.IsEnabled = IsEnabled;
					
					} break;

				case nameof(AllPresets):
				case nameof(CurrentPreset):
					{
						if (XyzuPlayer.Instance.SettingsBassBoost != null && XyzuPlayer.Instance.SettingsBassBoost.CurrentPreset.Name != CurrentPreset.Name)
							XyzuPlayer.Instance.SettingsBassBoost.CurrentPreset = CurrentPreset;

						if (CurrentPresetPreference != null)
						{
							string[] presetnames = AllPresets.Select(preset => preset.Name).ToArray();

							CurrentPresetPreference.SetEntries(presetnames);
							CurrentPresetPreference.SetEntryValues(presetnames);

							if (AllPresets.Index(preset => string.Equals(preset.Name, CurrentPreset.Name, StringComparison.OrdinalIgnoreCase)) is int currentpresetindex)
								CurrentPresetPreference.SetValueIndex(currentpresetindex);
						}

						if (propertyName == nameof(CurrentPreset))
						{
							if (StrengthPreference != null && StrengthPreference.Value != CurrentPreset.Strength)
								StrengthPreference.Value = CurrentPreset.Strength;
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
					if (AllPresets.FirstOrDefault(preset => string.Equals(preset.Name, newvalue.ToString(), StringComparison.OrdinalIgnoreCase)) is IBassBoostSettings.IPreset currentpreset)
						CurrentPreset = currentpreset;
					return true;

				case true when
				preference == StrengthPreference:
					CurrentPreset.Strength = newvalue.JavaCast<Java.Lang.Integer>().ShortValue();
					return true;

				default: return base.OnPreferenceChange(preference, newvalue);
			}
		}
		public void CurrentPresetPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(IBassBoostSettings.IPreset.Strength) when
				XyzuPlayer.Instance.SettingsBassBoost != null && 
				XyzuPlayer.Instance.SettingsBassBoost.CurrentPreset.Strength != CurrentPreset.Strength:
					XyzuPlayer.Instance.SettingsBassBoost.CurrentPreset.Strength = CurrentPreset.Strength;
					break;

				default: break;
			}
		}
	}
}