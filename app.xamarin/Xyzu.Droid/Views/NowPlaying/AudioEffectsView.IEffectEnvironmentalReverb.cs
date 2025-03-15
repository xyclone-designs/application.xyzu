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
		public interface IEffectEnvironmentalReverb : IEffect<IEnvironmentalReverbSettings.IPreset>
		{
			public class Default : Default<IEnvironmentalReverbSettings.IPreset>, IEffectEnvironmentalReverb
			{
				public Default(AudioEffectsView effectsview) : base(effectsview) { }

				protected override IEnvironmentalReverbSettings.IPreset PresetDefault
				{
					get => IEnvironmentalReverbSettings.IPresetable.Defaults.Presets.Default;
				}

				public override bool IsEnabled
				{
					get => XyzuPlayer.Instance.SettingsEnvironmentalReverb.IsEnabled;
					set => XyzuPlayer.Instance.SettingsEnvironmentalReverb.IsEnabled = value;
				}
				public override IEnumerable<IEnvironmentalReverbSettings.IPreset> PresetAll
				{
					get => XyzuPlayer.Instance.SettingsEnvironmentalReverb.AllPresets;
					set => XyzuPlayer.Instance.SettingsEnvironmentalReverb.AllPresets = value;
				}
				public override IEnvironmentalReverbSettings.IPreset PresetCurrent
				{
					get => XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset;
					set => XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset = value;
				}
				public override IEnvironmentalReverbSettings.IPreset? PresetEdited
				{
					get => EffectsView.EffectsEnvironmentalReverbView.PresetEdited;
					set => EffectsView.EffectsEnvironmentalReverbView.PresetEdited = value;
				}

				public override void OnTabSelected(TabLayout.TabSelectedEventArgs args)
				{
					EffectsView.EffectsEnvironmentalReverbView.Visibility = ViewStates.Visible;
					EffectsView.EffectsEnvironmentalReverbContainer.Visibility = ViewStates.Visible;

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

					EffectsView.EffectsEnvironmentalReverbView.Visibility = ViewStates.Gone;
					EffectsView.EffectsEnvironmentalReverbContainer.Visibility = ViewStates.Gone;

					XyzuSettings.Instance
						.Edit()?
						.PutAudioEnvironmentalReverb(XyzuPlayer.Instance.SettingsEnvironmentalReverb)
						.Commit();

					base.OnTabUnselected(args);
				}

				public override void ButtonCancelClicked()
				{
					if (PresetEdited is not null)
						EffectsView.EffectsEnvironmentalReverbView.Preset = PresetCurrent;

					base.ButtonCancelClicked();
				}
				public override void ButtonDeleteClicked()
				{
					if (XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset.IsDefault)
						return;

					XyzuSettings.Instance.RemoveAudioEnvironmentalReverb(PresetCurrent.Name);
					XyzuPlayer.Instance.SettingsEnvironmentalReverb = XyzuSettings.Instance.GetAudioEnvironmentalReverb();

					base.ButtonDeleteClicked();
				}

				public override bool OptionsOnCreate(OptionsCreateAndViewView view, string? text)
				{
					if (text is null || text == string.Empty)
						return false;

					PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(text);
					PresetEdited.Name = text;

					PresetCurrent = PresetEdited;
					PresetAll = XyzuPlayer.Instance.SettingsEnvironmentalReverb.AllPresets
						.Append(PresetEdited);

					XyzuSettings.Instance
						.Edit()?
						.PutAudioEnvironmentalReverb(XyzuPlayer.Instance.SettingsEnvironmentalReverb)
						.Commit();

					XyzuPlayer.Instance.SettingsEnvironmentalReverb = XyzuSettings.Instance.GetAudioEnvironmentalReverb();

					base.OptionsOnCreate(view, text);

					return true;
				}
			}
		}
	}
}