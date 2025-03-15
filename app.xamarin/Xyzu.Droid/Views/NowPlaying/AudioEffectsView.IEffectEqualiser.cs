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
		public interface IEffectEqualiser : IEffect<IEqualiserSettings.IPreset>
		{
			public class Default : Default<IEqualiserSettings.IPreset>, IEffectEqualiser
			{
				public Default(AudioEffectsView effectsview) : base(effectsview) { }

				protected override IEqualiserSettings.IPreset PresetDefault
				{
					get => IEqualiserSettings.IPresetable.Defaults.Presets.Default;
				}

				public override bool IsEnabled
				{
					get => XyzuPlayer.Instance.SettingsEqualiser.IsEnabled;
					set => XyzuPlayer.Instance.SettingsEqualiser.IsEnabled = value;
				}
				public override IEnumerable<IEqualiserSettings.IPreset> PresetAll
				{
					get => XyzuPlayer.Instance.SettingsEqualiser.AllPresets;
					set => XyzuPlayer.Instance.SettingsEqualiser.AllPresets = value;
				}
				public override IEqualiserSettings.IPreset PresetCurrent
				{
					get => XyzuPlayer.Instance.SettingsEqualiser.CurrentPreset;
					set => XyzuPlayer.Instance.SettingsEqualiser.CurrentPreset = value;
				}
				public override IEqualiserSettings.IPreset? PresetEdited
				{
					get => EffectsView.EffectsEqualiserView.PresetEdited;
					set => EffectsView.EffectsEqualiserView.PresetEdited = value;
				}

				public override void OnTabSelected(TabLayout.TabSelectedEventArgs args)
				{
					EffectsView.EffectsEqualiserView.Visibility = ViewStates.Visible;

					args.Tab?.SetText(Resource.String.player_audioeffects_equaliser_title);

					EffectsView.TitleAppCompatTextView.SetText(Resource.String.xyzu_view_audioeffects_equaliser_title);

					EffectsView.PresetsRecyclerView.SimpleAdapter.FuncGetItemCount = PresetsGetItemCount;
					EffectsView.PresetsRecyclerView.SimpleAdapter.ViewHolderOnBind = PresetsViewHolderOnBind;
					EffectsView.PresetsRecyclerView.SimpleAdapter.ViewHolderOnCreate = PresetsViewHolderOnCreate;
					EffectsView.PresetsRecyclerView.SimpleAdapter.ViewHolderOnCheckChange = PresetsViewHolderOnCheckChange;

					base.OnTabSelected(args);
				}
				public override void OnTabUnselected(TabLayout.TabUnselectedEventArgs args)
				{
					ButtonCancelClicked();

					EffectsView.EffectsEqualiserView.Visibility = ViewStates.Gone;

					XyzuSettings.Instance
						.Edit()?
						.PutAudioEqualiser(XyzuPlayer.Instance.SettingsEqualiser)
						.Commit();

					base.OnTabUnselected(args);
				}

				public override void ButtonCancelClicked()
				{
					if (PresetEdited is not null)
						EffectsView.EffectsEqualiserView.Preset = PresetCurrent;

					base.ButtonCancelClicked();
				}
				public override void ButtonDeleteClicked()
				{
					if (XyzuPlayer.Instance.SettingsEqualiser.CurrentPreset.IsDefault)
						return;

					XyzuSettings.Instance.RemoveAudioEqualiser(PresetCurrent.Name);
					XyzuPlayer.Instance.SettingsEqualiser = XyzuSettings.Instance.GetAudioEqualiser();

					base.ButtonDeleteClicked();
				}

				public override bool OptionsOnCreate(OptionsCreateAndViewView view, string? text)
				{
					if (text is null || text == string.Empty)
						return false;

					PresetEdited ??= new IEqualiserSettings.IPreset.Default(text);
					PresetEdited.Name = text;

					PresetCurrent = PresetEdited;
					PresetAll = XyzuPlayer.Instance.SettingsEqualiser.AllPresets
						.Append(PresetEdited);

					XyzuSettings.Instance
						.Edit()?
						.PutAudioEqualiser(XyzuPlayer.Instance.SettingsEqualiser)
						.Commit();

					XyzuPlayer.Instance.SettingsEqualiser = XyzuSettings.Instance.GetAudioEqualiser();

					base.OptionsOnCreate(view, text);

					return true;
				}
			}
		}
	}
}