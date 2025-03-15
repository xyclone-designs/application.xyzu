using Android.Content;
using Android.Views;

using Google.Android.Material.Tabs;

using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Settings.Audio;
using Xyzu.Views.Option;

namespace Xyzu.Views.NowPlaying
{
	public partial class AudioEffectsView
	{
		public interface IEffectVolumeControl : IEffect<IVolumeControlSettings.IPreset> 
		{
			public class Default : Default<IVolumeControlSettings.IPreset>, IEffectVolumeControl
			{
				public Default(AudioEffectsView effectsview) : base(effectsview) { }

				protected override IVolumeControlSettings.IPreset PresetDefault
				{
					get => IVolumeControlSettings.IPresetable.Defaults.Presets.Default;
				}

				public override bool IsEnabled
				{
					get => XyzuPlayer.Instance.SettingsVolumeControl.IsEnabled;
					set => XyzuPlayer.Instance.SettingsVolumeControl.IsEnabled = value;
				}
				public override IEnumerable<IVolumeControlSettings.IPreset> PresetAll
				{
					get => XyzuPlayer.Instance.SettingsVolumeControl.AllPresets;
					set => XyzuPlayer.Instance.SettingsVolumeControl.AllPresets = value;
				}
				public override IVolumeControlSettings.IPreset PresetCurrent
				{
					get => XyzuPlayer.Instance.SettingsVolumeControl.CurrentPreset;
					set => XyzuPlayer.Instance.SettingsVolumeControl.CurrentPreset = value;
				}
				public override IVolumeControlSettings.IPreset? PresetEdited
				{
					get => EffectsView.EffectsVolumeControlView.PresetEdited;
					set => EffectsView.EffectsVolumeControlView.PresetEdited = value;
				}

				public override void OnTabSelected(TabLayout.TabSelectedEventArgs args)
				{
					EffectsView.EffectsVolumeControlView.Visibility = ViewStates.Visible;
					EffectsView.EffectsVolumeControlContainer.Visibility = ViewStates.Visible;

					args.Tab?.SetText(Resource.String.player_audioeffects_volumecontrol_title);

					EffectsView.TitleAppCompatTextView.SetText(Resource.String.xyzu_view_audioeffects_volumecontrol_title);

					EffectsView.PresetsRecyclerView.SimpleAdapter.FuncGetItemCount = PresetsGetItemCount;
					EffectsView.PresetsRecyclerView.SimpleAdapter.ViewHolderOnBind = PresetsViewHolderOnBind;
					EffectsView.PresetsRecyclerView.SimpleAdapter.ViewHolderOnCreate = PresetsViewHolderOnCreate;
					EffectsView.PresetsRecyclerView.SimpleAdapter.ViewHolderOnCheckChange = PresetsViewHolderOnCheckChange;

					base.OnTabSelected(args);
				}
				public override void OnTabUnselected(TabLayout.TabUnselectedEventArgs args)
				{
					ButtonCancelClicked();

					EffectsView.EffectsVolumeControlView.Visibility = ViewStates.Gone;
					EffectsView.EffectsVolumeControlContainer.Visibility = ViewStates.Gone;

					XyzuSettings.Instance
						.Edit()?
						.PutAudioVolumeControl(XyzuPlayer.Instance.SettingsVolumeControl)
						.Commit();

					base.OnTabUnselected(args);
				}

				public override void ButtonCancelClicked()
				{
					if (PresetEdited is not null)
						EffectsView.EffectsVolumeControlView.Preset = PresetCurrent;

					base.ButtonCancelClicked();
				}
				public override void ButtonDeleteClicked()
				{
					if (XyzuPlayer.Instance.SettingsVolumeControl.CurrentPreset.IsDefault)
						return;

					XyzuSettings.Instance.RemoveAudioVolumeControl(PresetCurrent.Name);
					XyzuPlayer.Instance.SettingsVolumeControl = XyzuSettings.Instance.GetAudioVolumeControl();

					base.ButtonDeleteClicked();
				}

				public override bool OptionsOnCreate(OptionsCreateAndViewView view, string? text)
				{
					if (text is null || text == string.Empty)
						return false;

					PresetEdited ??= new IVolumeControlSettings.IPreset.Default(text);
					PresetEdited.Name = text;

					PresetCurrent = PresetEdited;
					PresetAll = XyzuPlayer.Instance.SettingsVolumeControl.AllPresets
						.Append(PresetEdited);

					XyzuSettings.Instance
						.Edit()?
						.PutAudioVolumeControl(XyzuPlayer.Instance.SettingsVolumeControl)
						.Apply();

					XyzuPlayer.Instance.SettingsVolumeControl = XyzuSettings.Instance.GetAudioVolumeControl();

					base.OptionsOnCreate(view, text);

					return true;
				}
			}
		}
	}
}