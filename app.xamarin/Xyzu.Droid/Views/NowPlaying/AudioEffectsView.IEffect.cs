using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;

using Google.Android.Material.Dialog;
using Google.Android.Material.Tabs;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Settings;
using Xyzu.Settings.Audio;
using Xyzu.Views.AudioEffects;
using Xyzu.Views.Option;

using AndroidXAlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Xyzu.Views.NowPlaying
{
	public partial class AudioEffectsView
	{
		public interface IEffect
		{
			bool IsEnabled { get; set; }
			AudioEffectsView EffectsView { get; set; }

			void OnTabSelected(TabLayout.TabSelectedEventArgs args);
			void OnTabUnselected(TabLayout.TabUnselectedEventArgs args);
			void OnPresetPropertyChanged(object? sender, PropertyChangedEventArgs args);

			void ButtonCancelClicked();
			void ButtonDeleteClicked();
			void ButtonSaveNewClicked();
			void ButtonSaveEditedClicked();
			void ButtonAddClicked();
			void ButtonsRefresh();

			int PresetsGetItemCount();
			void PresetsRefresh(int? position);
			void PresetsViewHolderOnCheckChange(RecyclerViewViewHolderDefault.ViewHolderEventArgs args);
			void PresetsViewHolderOnBind(RecyclerViewViewHolderDefault recyclerviewviewholderdefault, int position);
			RecyclerViewViewHolderDefault PresetsViewHolderOnCreate(ViewGroup? parent, int viewtype);

			public abstract class Default<TPreset> : IEffect<TPreset> where TPreset : ISettings.IPreset
			{
				protected abstract TPreset PresetDefault { get; }

				public Default(AudioEffectsView effectsview)
				{
					EffectsView = effectsview;
					PresetCurrent = PresetDefault;
				}

				private IEnumerable<TPreset> _PresetAll = Enumerable.Empty<TPreset>();

				public AudioEffectsView EffectsView { get; set; }
				public AndroidXAlertDialog? EffectsAlertDialog { get; set; }

				public virtual bool IsEnabled { get; set; }
				public virtual TPreset PresetCurrent { get; set; }
				public virtual TPreset? PresetEdited { get; set; }
				public virtual IEnumerable<TPreset> PresetAll
				{
					get => _PresetAll;
					set => _PresetAll = value;
				}

				private Action<MaterialAlertDialogBuilder?, AndroidXAlertDialog?> AlertDialogAdd(Action<AndroidXAlertDialog, OptionsCreateAndViewView>? oncreate)
				{
					return (alertdialogbuilder, alertdialog) =>
					{
						if (alertdialogbuilder is not null)
						{
							alertdialogbuilder.SetTitle(Resource.String.player_audioeffects_presets_new_title);
							alertdialogbuilder.SetMessage(Resource.String.player_audioeffects_presets_new_description);
							alertdialogbuilder.SetNegativeButton(Resource.String.cancel, new DialogInterfaceOnClickListener((sender, args) => ButtonCancelClicked()));
							alertdialogbuilder.SetOnDismissListener(new DialogInterfaceOnDismissListener((dialog) => ButtonCancelClicked()));
						}

						if (alertdialog is not null && EffectsView.Context is not null)
						{
							OptionsCreateAndViewView optioncreateandviewview = new(EffectsView.Context);

							oncreate?.Invoke(alertdialog, optioncreateandviewview);

							optioncreateandviewview.OnCreate?.Invoke(optioncreateandviewview, null);
							optioncreateandviewview.OnCreateTextChanged?.Invoke(optioncreateandviewview, null);

							alertdialog.SetView(optioncreateandviewview);
							alertdialog.SetOnShowListener(new DialogInterfaceOnShowListener
							{
								OnShowAction = dialoginterface =>
								{
									optioncreateandviewview.View.SimpleAdapter.NotifyDataSetChanged();
								}
							});
						}
					};
				}

				public virtual void OnTabSelected(TabLayout.TabSelectedEventArgs args)
				{
					EffectsView.State = States.Default;
					EffectsView.EnabledSwitchCompatCheckedChange(this, new CompoundButton.CheckedChangeEventArgs(EffectsView.EnabledSwitchCompat.Checked));					
					EffectsView.PresetsRecyclerView.SimpleAdapter.NotifyDataSetChanged();
				}
				public virtual void OnTabUnselected(TabLayout.TabUnselectedEventArgs args)
				{
					args.Tab?.SetText(null as string);
				}
				public virtual void OnPresetPropertyChanged(object? sender, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == nameof(PresetEdited))
						EffectsView.State = PresetEdited is null ? States.Default : States.Edit;
				}

				public virtual void ButtonCancelClicked() 
				{
					PresetEdited = default;

					PresetsRefresh(null);

					EffectsView.State = States.Default;
				}
				public virtual void ButtonDeleteClicked() 
				{
					PresetEdited = default;
					EffectsView.PresetsRecyclerView.SimpleAdapter.NotifyDataSetChanged();
					PresetsRefresh(null);
				}
				public virtual void ButtonSaveNewClicked() 
				{
					if (EffectsView.Context is null)
						return;

					EffectsAlertDialog = XyzuUtils.Dialogs.Alert(EffectsView.Context, AlertDialogAdd((alertdialog, optionsview) =>
					{
						optionsview.OnCreate = OptionsOnCreate;
						optionsview.OnCreateTextChanged = OptionsOnCreateTextChanged;

					}), null); EffectsAlertDialog.Show();
				}
				public virtual void ButtonSaveEditedClicked() 
				{
					PresetEdited = default;
					EffectsView.PresetsRecyclerView.SimpleAdapter.NotifyDataSetChanged();
					PresetsRefresh(null);

					EffectsView.State = States.Default;
				}
				public virtual void ButtonAddClicked() 
				{
					if (EffectsView.Context is null)
						return;

					EffectsAlertDialog = XyzuUtils.Dialogs.Alert(EffectsView.Context, AlertDialogAdd((alertdialog, optionsview) =>
					{
						optionsview.OnCreate = OptionsOnCreate;
						optionsview.OnCreateTextChanged = OptionsOnCreateTextChanged;

					}), null); EffectsAlertDialog.Show();
				}
				public virtual void ButtonsRefresh() 
				{
					switch (EffectsView.State, PresetCurrent.IsDefault)
					{
						case (States.Edit, true):
							EffectsView.ButtonCancel.Visibility = ViewStates.Visible;
							EffectsView.ButtonDelete.Visibility = ViewStates.Gone;
							EffectsView.ButtonSaveEdited.Visibility = ViewStates.Gone;
							EffectsView.ButtonSaveNew.Visibility = ViewStates.Visible;
							EffectsView.ButtonAdd.Visibility = ViewStates.Gone;
							break;
						case (States.Edit, false):
							EffectsView.ButtonCancel.Visibility = ViewStates.Visible;
							EffectsView.ButtonDelete.Visibility = ViewStates.Gone;
							EffectsView.ButtonSaveEdited.Visibility = ViewStates.Visible;
							EffectsView.ButtonSaveNew.Visibility = ViewStates.Visible;
							EffectsView.ButtonAdd.Visibility = ViewStates.Gone;
							break;

						case (States.Default, true):
							EffectsView.ButtonCancel.Visibility = ViewStates.Gone;
							EffectsView.ButtonDelete.Visibility = ViewStates.Gone;
							EffectsView.ButtonSaveEdited.Visibility = ViewStates.Gone;
							EffectsView.ButtonSaveNew.Visibility = ViewStates.Gone;
							EffectsView.ButtonAdd.Visibility = ViewStates.Visible;
							break;

						case (States.Default, false):
							EffectsView.ButtonCancel.Visibility = ViewStates.Gone;
							EffectsView.ButtonDelete.Visibility = ViewStates.Visible;
							EffectsView.ButtonSaveEdited.Visibility = ViewStates.Gone;
							EffectsView.ButtonSaveNew.Visibility = ViewStates.Gone;
							EffectsView.ButtonAdd.Visibility = ViewStates.Visible;
							break;
					}
				}

				public virtual int PresetsGetItemCount()
				{
					return PresetAll.Count();
				}
				public virtual void PresetsRefresh(int? position)
				{
					position ??= PresetAll.Index(PresetCurrent);

					PresetCurrent = true switch
					{
						true when PresetEdited is not null => PresetEdited,
						true when position is null => PresetDefault,
						_ => PresetAll.ElementAt(position.Value),
					};

					ButtonsRefresh();
				}
				public virtual void PresetsViewHolderOnCheckChange(RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
				{
					ButtonCancelClicked();
					PresetsRefresh(args.ViewHolder.AbsoluteAdapterPosition);
					EffectsView.PresetsRecyclerView.SimpleAdapter.NotifyDataSetChanged();
				}
				public virtual void PresetsViewHolderOnBind(RecyclerViewViewHolderDefault recyclerviewviewholderdefault, int position)
				{
					if (PresetAll.ElementAtOrDefault(position) is not TPreset preset)
						return;

					if (recyclerviewviewholderdefault.ItemView is not AppCompatCheckBox itemview)
						return;

					bool waschecked = itemview.Checked;
					bool nowchecked = PresetEdited?.Name == preset.Name || PresetCurrent.Name == preset.Name;

					itemview.SetText(preset.Name, null);
					itemview.SetChecked(nowchecked);

					if (waschecked is false && nowchecked is true)
						PresetsRefresh(position);
				}
				public virtual RecyclerViewViewHolderDefault PresetsViewHolderOnCreate(ViewGroup? parent, int viewtype)
				{
					return new PresetViewHolder(parent?.Context ?? throw new InflateException("EffectPresetViewHolder Context"));
				}

				public virtual bool OptionsOnCreate(OptionsCreateAndViewView view, string? text)
				{
					if (text is null || text == string.Empty)
						return false;

					EffectsView.State = States.Default;
					EffectsView.PresetsRecyclerView.SimpleAdapter.NotifyDataSetChanged();
					EffectsAlertDialog?.Dismiss();
					PresetEdited = default;					
					PresetsRefresh(null);

					return true;
				}
				public virtual bool OptionsOnCreateTextChanged(OptionsCreateAndViewView view, string? text)
				{
					switch (true)
					{
						case true when text is null:
						case true when string.IsNullOrWhiteSpace(text):
							view.CreateButton.Enabled = false;
							break;

						case true when PresetAll.Any(_ => string.Equals(text, _.Name, StringComparison.OrdinalIgnoreCase)):
							view.CreateButton.Enabled = false;
							view.SetMessageError(string.Format(
								"'{0}' {1}",
								text,
								view.Context?.Resources?.GetString(Resource.String.extra_alreadyexists) ?? string.Empty));
							break;

						default:
							view.CreateButton.Enabled = true;
							view.SetMessageInfo(null);
							break;
					}

					return true;
				}
			}
		}
		public interface IEffect<TPreset> : IEffect where TPreset : ISettings.IPreset
		{
			TPreset PresetCurrent { get; set; }
			TPreset? PresetEdited { get; set; }
			IEnumerable<TPreset> PresetAll { get; set; }
		}
	}
}