﻿#nullable enable

using Android.Content;
using Android.Media.Audiofx;
using Android.OS;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Settings.Audio;
using Xyzu.Settings.UserInterface;
using Xyzu.Views.Setting;
using Xyzu.Widgets.RecyclerViews.Simple;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;
using XyzuSwitchPreference = Xyzu.Preference.SwitchPreference;

namespace Xyzu.Fragments.Settings.Audio
{
	[Register(FragmentName)]
	public class EqualiserPreferenceFragment : BasePreferenceFragment, IEqualiserSettings.IPresetable
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.Audio.EqualiserPreferenceFragment";

		public static class Keys
		{
			public const string IsEnabled = "settings_audio_equaliser_isenabled_switchpreference_key";
			public const string CurrentPreset = "settings_audio_equaliser_currentpreset_listpreference_key";
			public const string CurrentPresetBands = "settings_audio_equaliser_currentpreset_bands_listpreference_key";
		}

		private bool _IsEnabled;
		private IEqualiserSettings.IPreset? _CurrentPreset;
		private IEnumerable<IEqualiserSettings.IPreset>? _AllPresets;

		public bool IsEnabled
		{
			get => _IsEnabled;
			set
			{
				_IsEnabled = value;

				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IEqualiserSettings.IPresetable.Keys.IsEnabled, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public IEqualiserSettings.IPreset CurrentPreset
		{
			get => _CurrentPreset ??= IEqualiserSettings.IPresetable.Defaults.Presets.TenBand.Default;
			set
			{
				if (_CurrentPreset != null)
					_CurrentPreset.PropertyChanged -= CurrentPresetPropertyChanged;

				_CurrentPreset = value;
				_CurrentPreset.PropertyChanged += CurrentPresetPropertyChanged;

				XyzuSettings.Instance
					.Edit()?
					.PutString(IEqualiserSettings.IPresetable.Keys.CurrentPreset, value?.Name)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public IEnumerable<IEqualiserSettings.IPreset> AllPresets
		{
			get => _AllPresets ??= IEqualiserSettings.IPresetable.Defaults.Presets.TenBand.AsEnumerable();
			set
			{
				_AllPresets = value;

				XyzuSettings.Instance
					.Edit()?
					.PutAudioEqualiser(value.ToArray())?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? IsEnabledPreference { get; set; }
		public XyzuListPreference? CurrentPresetPreference { get; set; }
		public SimpleHorizontalRecyclerView? CurrentPresetBandsView { get; set; }

		public void CurrentPresetPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				default: break;
			}
		}

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_audio_equaliser_title);

			//AddPreferenceClickHandler(CurrentPresetBandsPreference);
			AddPreferenceChangeHandler(
				IsEnabledPreference,
				CurrentPresetPreference);

			IEqualiserSettings.IPresetable settings = XyzuSettings.Instance.GetAudioEqualiser();

			_IsEnabled = settings.IsEnabled; OnPropertyChanged(nameof(IsEnabled));
			_AllPresets = settings.AllPresets; OnPropertyChanged(nameof(AllPresets));
			_CurrentPreset = settings.CurrentPreset; OnPropertyChanged(nameof(CurrentPreset));
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				IsEnabledPreference,
				CurrentPresetPreference);
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_audio_equaliser, rootKey);
			InitPreferences(
				IsEnabledPreference = FindPreference(Keys.IsEnabled) as XyzuSwitchPreference,
				CurrentPresetPreference = FindPreference(Keys.CurrentPreset) as XyzuListPreference);

			if (CurrentPresetPreference?.View != null && Context != null)
			{
				CurrentPresetPreference.View.Expanded = true;
				CurrentPresetPreference.View.ViewAdditionalContentView = CurrentPresetBandsView = new SimpleHorizontalRecyclerView(Context);

				CurrentPresetBandsView.SimpleLayoutManager.SpanCount = 1;
				CurrentPresetBandsView.SimpleAdapter.GetItemCount = () => CurrentPreset.FrequencyLevels.Length;
				CurrentPresetBandsView.SimpleAdapter.ViewHolderOnCreate = (parent, viewtype) => new AudioBandViewHolder(Context);
				CurrentPresetBandsView.SimpleAdapter.ViewHolderOnBind = (viewholderdefault, position) =>
				{
					AudioBandViewHolder viewholder = (AudioBandViewHolder)viewholderdefault;

					viewholder.ItemView.Title.Text = CurrentPreset.FrequencyLevels[position].ToString();
					viewholder.ItemView.Max.Text = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper.ToString();
					viewholder.ItemView.Value.Max = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsUpper;
					viewholder.ItemView.Value.Progress = CurrentPreset.FrequencyLevels[position];
					viewholder.ItemView.ValueProgressChanged = (sender, args) =>
					{
						CurrentPreset.FrequencyLevels[position] = (short)args.Progress;
						viewholder.ItemView.Title.Text = CurrentPreset.FrequencyLevels[position].ToString();
					};
					viewholder.ItemView.Value.Min = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower;
					viewholder.ItemView.Min.Text = IEqualiserSettings.IPreset.Ranges.FrequencyLevelsLower.ToString();
					viewholder.ItemView.Footer.Text = string.Format("{0} Hz", CurrentPreset.FrequencyLevels.Length switch
					{
						06 => IEqualiserSettings.IPreset.BandCuttoffs.SixBand
							.AsEnumerable()
							.ElementAt(position)
							.ToString(),

						08 => IEqualiserSettings.IPreset.BandCuttoffs.EightBand
							.AsEnumerable()
							.ElementAt(position)
							.ToString(),

						10 => IEqualiserSettings.IPreset.BandCuttoffs.TenBand
							.AsEnumerable()
							.ElementAt(position)
							.ToString(),

						_ => string.Empty
					});
				};
			}
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(IsEnabled):
						if (IsEnabledPreference != null && IsEnabledPreference.Checked != IsEnabled)
							IsEnabledPreference.Checked = IsEnabled;

						if (XyzuPlayer.Instance.SettingsEqualiser != null && XyzuPlayer.Instance.SettingsEqualiser.IsEnabled != IsEnabled)
							XyzuPlayer.Instance.SettingsEqualiser.IsEnabled = IsEnabled;
					break;

				case nameof(AllPresets) when CurrentPresetPreference != null:
						string[] presetnames = AllPresets.Select(preset => preset.Name).ToArray();

						CurrentPresetPreference.SetEntries(presetnames);
						CurrentPresetPreference.SetEntryValues(presetnames);
					break;

				case nameof(CurrentPreset):
					if (XyzuPlayer.Instance.SettingsEqualiser != null && XyzuPlayer.Instance.SettingsEqualiser.CurrentPreset.Name != CurrentPreset.Name)
						XyzuPlayer.Instance.SettingsEqualiser.CurrentPreset = CurrentPreset;

					if (AllPresets.Index(preset => string.Equals(preset.Name, CurrentPreset?.Name, StringComparison.OrdinalIgnoreCase)) is int currentpresetindex)
						CurrentPresetPreference?.SetValueIndex(currentpresetindex);

					CurrentPresetBandsView?.SimpleAdapter.NotifyDataSetChanged();
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
					if (AllPresets.FirstOrDefault(preset => string.Equals(preset.Name, newvalue.ToString(), StringComparison.OrdinalIgnoreCase)) is IEqualiserSettings.IPreset currentpreset)
						CurrentPreset = currentpreset;
					return true;

				default: return base.OnPreferenceChange(preference, newvalue);
			}
		}

		public class AudioBandViewHolder : RecyclerViewViewHolderDefault
		{
			private static AudioBand ItemViewDefault(Context context)
			{
				return new AudioBand(context);
			}

			public AudioBandViewHolder(Context context) : this(ItemViewDefault(context)) { }
			public AudioBandViewHolder(AudioBand itemView) : base(itemView) { }

			public new AudioBand ItemView
			{
				set => base.ItemView = value;
				get => (AudioBand)base.ItemView;
			}
		}
	}
}