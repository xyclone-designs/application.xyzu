using Android.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;

using Google.Android.Material.Tabs;

using System;
using System.ComponentModel;

using Xyzu.Droid;
using Xyzu.Views.AudioEffects;
using Xyzu.Widgets.RecyclerViews.Simple;

namespace Xyzu.Views.NowPlaying
{
	public partial class AudioEffectsView : ConstraintLayout
	{
		public enum States
		{
			Default,
			Edit
		}

		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_nowplaying_audioeffects;

			public const int Title_AppCompatTextView = Resource.Id.xyzu_view_nowplaying_audioeffects_title_appcompattextview;
			public const int Enabled_SwitchCompat = Resource.Id.xyzu_view_nowplaying_audioeffects_enabled_switchcompat;
			public const int Effects_RelativeLayout = Resource.Id.xyzu_view_nowplaying_audioeffects_effects_relativelayout;
			public const int Effects_VolumeControl_NestedScrollView = Resource.Id.xyzu_view_nowplaying_audioeffects_effects_volumecontrol_nestedscrollview;
			public const int Effects_VolumeControl_VolumeControlView = Resource.Id.xyzu_view_nowplaying_audioeffects_effects_volumecontrol_volumecontrolview;
			public const int Effects_Equaliser_EqualiserView = Resource.Id.xyzu_view_nowplaying_audioeffects_effects_equaliser_equaliserview;
			public const int Effects_EnvironmentalReverb_NestedScrollView = Resource.Id.xyzu_view_nowplaying_audioeffects_effects_environmentalreverb_nestedscrollview;
			public const int Effects_EnvironmentalReverb_EnvironmentalReverbView = Resource.Id.xyzu_view_nowplaying_audioeffects_effects_environmentalreverb_environmentalreverbview;
			public const int Presets_SimpleHorizontalRecyclerView = Resource.Id.xyzu_view_nowplaying_audioeffects_presets_simplehorizontalrecyclerview;
			public const int ButtonCancel_AppCompatButton = Resource.Id.xyzu_view_nowplaying_audioeffects_buttoncancel_appcompatbutton;
			public const int ButtonDelete_AppCompatButton = Resource.Id.xyzu_view_nowplaying_audioeffects_buttondelete_appcompatbutton;
			public const int ButtonSaveEdited_AppCompatButton = Resource.Id.xyzu_view_nowplaying_audioeffects_buttonsaveedited_appcompatbutton;
			public const int ButtonSaveNew_AppCompatButton = Resource.Id.xyzu_view_nowplaying_audioeffects_buttonsavenew_appcompatbutton;
			public const int ButtonAdd_AppCompatButton = Resource.Id.xyzu_view_nowplaying_audioeffects_buttonadd_appcompatbutton;
			public const int Effects_TabLayout = Resource.Id.xyzu_view_nowplaying_audioeffects_effects_tablayout;
		}

		public AudioEffectsView(Context context) : this(context, null!)
		{ }
		public AudioEffectsView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_NowPlaying_AudioEffects)
		{ }
		public AudioEffectsView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_NowPlaying_AudioEffects)
		{ }
		public AudioEffectsView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Inflate(context, Ids.Layout, this);

			TitleAppCompatTextView = FindViewById(Ids.Title_AppCompatTextView) as AppCompatTextView ?? throw new InflateException("Title_AppCompatTextView");
			EnabledSwitchCompat = FindViewById(Ids.Enabled_SwitchCompat) as SwitchCompat ?? throw new InflateException("Enabled_SwitchCompat");

			PresetsRecyclerView = FindViewById<SimpleHorizontalRecyclerView>(Ids.Presets_SimpleHorizontalRecyclerView) ?? throw new InflateException("Presets_SimpleHorizontalRecyclerView");
		
			ButtonCancel = FindViewById<AppCompatButton>(Ids.ButtonCancel_AppCompatButton) ?? throw new InflateException("ButtonCancel_AppCompatButton");
			ButtonDelete = FindViewById<AppCompatButton>(Ids.ButtonDelete_AppCompatButton) ?? throw new InflateException("ButtonDelete_AppCompatButton");
			ButtonSaveEdited = FindViewById<AppCompatButton>(Ids.ButtonSaveEdited_AppCompatButton) ?? throw new InflateException("ButtonSaveEdited_AppCompatButton");
			ButtonSaveNew = FindViewById<AppCompatButton>(Ids.ButtonSaveNew_AppCompatButton) ?? throw new InflateException("ButtonSaveNew_AppCompatButton");
			ButtonAdd = FindViewById<AppCompatButton>(Ids.ButtonAdd_AppCompatButton) ?? throw new InflateException("ButtonAdd_AppCompatButton");

			if (string.Format(
				"{0}  <i><small>*{1}*</small></i>",
				context.Resources?.GetString(Resource.String.save),
				context.Resources?.GetString(Resource.String.edit).ToLower()) is string buttonsaveedited) 
				ButtonSaveEdited.SetText(Html.FromHtml(buttonsaveedited), TextView.BufferType.Spannable);

			if (string.Format(
				"{0}  <i><small>*{1}*</small></i>",
				context.Resources?.GetString(Resource.String.save),
				context.Resources?.GetString(Resource.String.new_).ToLower()) is string buttonsavenew) 
				ButtonSaveNew.SetText(Html.FromHtml(buttonsavenew), TextView.BufferType.Spannable);

			EffectsRelativeLayout = FindViewById<RelativeLayout>(Ids.Effects_RelativeLayout) ?? throw new InflateException("Effects_RelativeLayout");
			EffectsVolumeControlContainer = FindViewById<NestedScrollView>(Ids.Effects_VolumeControl_NestedScrollView) ?? throw new InflateException("Effects_VolumeControl_NestedScrollView");
			EffectsVolumeControlView = FindViewById<VolumeControlView>(Ids.Effects_VolumeControl_VolumeControlView) ?? throw new InflateException("Effects_VolumeControl_VolumeControlView");
			EffectsEqualiserView = FindViewById<EqualiserView>(Ids.Effects_Equaliser_EqualiserView) ?? throw new InflateException("Effects_Equaliser_EqualiserView");
			EffectsEnvironmentalReverbContainer = FindViewById<NestedScrollView>(Ids.Effects_EnvironmentalReverb_NestedScrollView) ?? throw new InflateException("Effects_EnvironmentalReverb_NestedScrollView");
			EffectsEnvironmentalReverbView = FindViewById<EnvironmentalReverbView>(Ids.Effects_EnvironmentalReverb_EnvironmentalReverbView) ?? throw new InflateException("Effects_EnvironmentalReverb_EnvironmentalReverbView");

			EffectsTablayout = FindViewById<TabLayout>(Ids.Effects_TabLayout) ?? throw new InflateException("Effects_TabLayout");
			EffectsTabVolumeControl = EffectsTablayout.NewTab().SetId(0).SetIcon(Resource.Drawable.icon_player_audioeffects_volumecontrol);
			EffectsTabEqualiser = EffectsTablayout.NewTab().SetId(1).SetIcon(Resource.Drawable.icon_player_audioeffects_equaliser);
			EffectsTabEnvironmentalReverb = EffectsTablayout.NewTab().SetId(2).SetIcon(Resource.Drawable.icon_player_audioeffects_environmentalreverb);

			_Effects = new IEffect[]
			{
				_EffectVolumeControl = new IEffectVolumeControl.Default(this),
				_EffectEqualiser = new IEffectEqualiser.Default(this),
				_EffectEnvironmentalReverb = new IEffectEnvironmentalReverb.Default(this),
			};

			PresetsRecyclerView.SimpleLayoutManager.SpanCount = 1;
			PresetsRecyclerView.SimpleMarginItemDecoration.MarginResTop = Resource.Dimension.dp16;
			PresetsRecyclerView.SimpleMarginItemDecoration.MarginResHorizontal = Resource.Dimension.dp8;

			EffectsRelativeLayout.LayoutChange += EffectsRelativeLayoutLayoutChange;
			EffectsVolumeControlView.OnPresetPropertyChanged = _Effects[0].OnPresetPropertyChanged;
			EffectsEqualiserView.OnPresetPropertyChanged = _Effects[1].OnPresetPropertyChanged;
			EffectsEnvironmentalReverbView.OnPresetPropertyChanged = _Effects[2].OnPresetPropertyChanged;

			EffectsTablayout.AddTab(EffectsTabVolumeControl, 0, true);
			EffectsTablayout.AddTab(EffectsTabEqualiser, 1, false);
			EffectsTablayout.AddTab(EffectsTabEnvironmentalReverb, 2, false);

			OnTabSelected(this, new TabLayout.TabSelectedEventArgs(EffectsTabVolumeControl));
			OnTabUnselected(this, new TabLayout.TabUnselectedEventArgs(EffectsTabEqualiser));
			OnTabUnselected(this, new TabLayout.TabUnselectedEventArgs(EffectsTabEnvironmentalReverb));
		}

		private States _State;
		private readonly IEffect[] _Effects;
		private IEffectVolumeControl _EffectVolumeControl;
		private IEffectEqualiser _EffectEqualiser;
		private IEffectEnvironmentalReverb _EffectEnvironmentalReverb;

		protected AppCompatTextView TitleAppCompatTextView { get; set; }
		protected SwitchCompat EnabledSwitchCompat { get; set; }

		protected SimpleHorizontalRecyclerView PresetsRecyclerView { get; set; }

		protected AppCompatButton ButtonCancel { get; set; }
		protected AppCompatButton ButtonDelete { get; set; }
		protected AppCompatButton ButtonSaveEdited { get; set; }
		protected AppCompatButton ButtonSaveNew { get; set; }
		protected AppCompatButton ButtonAdd { get; set; }

		protected RelativeLayout EffectsRelativeLayout { get; set; }
		protected NestedScrollView EffectsEnvironmentalReverbContainer { get; set; }
		protected NestedScrollView EffectsVolumeControlContainer { get; set; }
		protected EnvironmentalReverbView EffectsEnvironmentalReverbView { get; set; }
		protected VolumeControlView EffectsVolumeControlView { get; set; }
		protected EqualiserView EffectsEqualiserView { get; set; }

		protected TabLayout EffectsTablayout { get; set; }
		protected TabLayout.Tab EffectsTabVolumeControl { get; set; }
		protected TabLayout.Tab EffectsTabEqualiser { get; set; }
		protected TabLayout.Tab EffectsTabEnvironmentalReverb { get; set; }

		public States State
		{
			get => _State;
			set
			{
				_State = value;
				_Effects[TabPosition].ButtonsRefresh();
			}
		}
		public int TabPosition
		{
			get => Math.Max(EffectsTablayout.SelectedTabPosition, 0);
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			EffectsTablayout.TabSelected += OnTabSelected;
			EffectsTablayout.TabUnselected += OnTabUnselected;
			ButtonCancel.Click += ButtonCancelClicked;
			ButtonDelete.Click += ButtonDeleteClicked;
			ButtonSaveEdited.Click += ButtonSaveEditedClicked;
			ButtonSaveNew.Click += ButtonSaveNewClicked;
			ButtonAdd.Click += ButtonAddClicked;
			EnabledSwitchCompat.CheckedChange += EnabledSwitchCompatCheckedChange;

			XyzuPlayer.Instance.SettingsVolumeControl.PropertyChanged += SettingsVolumeControlPropertyChanged;
			XyzuPlayer.Instance.SettingsEqualiser.PropertyChanged += SettingsEqualiserPropertyChanged;
			XyzuPlayer.Instance.SettingsEnvironmentalReverb.PropertyChanged += SettingsEnvironmentalReverbPropertyChanged;
		}

		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			EffectsTablayout.TabSelected -= OnTabSelected;
			EffectsTablayout.TabUnselected -= OnTabUnselected;
			ButtonCancel.Click -= ButtonCancelClicked;
			ButtonDelete.Click -= ButtonDeleteClicked;
			ButtonSaveEdited.Click -= ButtonSaveEditedClicked;
			ButtonSaveNew.Click -= ButtonSaveNewClicked;
			ButtonAdd.Click -= ButtonAddClicked;
			EnabledSwitchCompat.CheckedChange -= EnabledSwitchCompatCheckedChange;

			XyzuPlayer.Instance.SettingsVolumeControl.PropertyChanged -= SettingsVolumeControlPropertyChanged;
			XyzuPlayer.Instance.SettingsEqualiser.PropertyChanged -= SettingsEqualiserPropertyChanged;
			XyzuPlayer.Instance.SettingsEnvironmentalReverb.PropertyChanged -= SettingsEnvironmentalReverbPropertyChanged;

			OnTabUnselected(this, new TabLayout.TabUnselectedEventArgs(EffectsTabVolumeControl));
			OnTabUnselected(this, new TabLayout.TabUnselectedEventArgs(EffectsTabEqualiser));
			OnTabUnselected(this, new TabLayout.TabUnselectedEventArgs(EffectsTabEnvironmentalReverb));
		}

		protected void OnTabSelected(object? sender, TabLayout.TabSelectedEventArgs args)
		{
			if (args.Tab is not null)
				_Effects[args.Tab.Id].OnTabSelected(args);
		}
		protected void OnTabUnselected(object? sender, TabLayout.TabUnselectedEventArgs args)
		{
			if (args.Tab is not null)
				_Effects[args.Tab.Id].OnTabUnselected(args);
		}

		protected void SettingsVolumeControlPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			EffectsVolumeControlView.Preset = XyzuPlayer.Instance.SettingsVolumeControl.CurrentPreset;
		}
		protected void SettingsEqualiserPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			EffectsEqualiserView.Preset = XyzuPlayer.Instance.SettingsEqualiser.CurrentPreset;
		}
		protected void SettingsEnvironmentalReverbPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			EffectsEnvironmentalReverbView.Preset = XyzuPlayer.Instance.SettingsEnvironmentalReverb.CurrentPreset; 
		}

		private void EffectsRelativeLayoutLayoutChange(object? sender, LayoutChangeEventArgs args)
		{
			if (args.AllZeroOld() && args.AllZero() is false)
			{
				EffectsRelativeLayout.LayoutChange -= EffectsRelativeLayoutLayoutChange;

				EffectsRelativeLayout.LayoutParameters ??= new ViewGroup.LayoutParams(args.Right - args.Left, args.Bottom - args.Top);
				EffectsVolumeControlContainer.LayoutParameters ??= new ViewGroup.LayoutParams(args.Right - args.Left, args.Bottom - args.Top);
				EffectsEqualiserView.LayoutParameters ??= new ViewGroup.LayoutParams(args.Right - args.Left, args.Bottom - args.Top);
				EffectsEnvironmentalReverbContainer.LayoutParameters ??= new ViewGroup.LayoutParams(args.Right - args.Left, args.Bottom - args.Top);

				EffectsRelativeLayout.LayoutParameters.Height =
				EffectsVolumeControlContainer.LayoutParameters.Height =
				EffectsEqualiserView.LayoutParameters.Height =
				EffectsEnvironmentalReverbContainer.LayoutParameters.Height = args.Bottom - args.Top;
			}
		}
		private void EnabledSwitchCompatCheckedChange(object? sender, CompoundButton.CheckedChangeEventArgs args)
		{
			_Effects[TabPosition].IsEnabled = EnabledSwitchCompat.Checked;
		}

		private void ButtonCancelClicked(object? sender, EventArgs args)
		{
			_Effects[TabPosition].ButtonCancelClicked();
		}
		private void ButtonDeleteClicked(object? sender, EventArgs args)
		{
			_Effects[TabPosition].ButtonDeleteClicked();
		}
		private void ButtonSaveEditedClicked(object? sender, EventArgs args)
		{
			_Effects[TabPosition].ButtonSaveEditedClicked();
		}
		private void ButtonSaveNewClicked(object? sender, EventArgs args)
		{
			_Effects[TabPosition].ButtonSaveNewClicked();
		}
		private void ButtonAddClicked(object? sender, EventArgs args)
		{
			_Effects[TabPosition].ButtonAddClicked();
		}

		public class PresetViewHolder : RecyclerViewViewHolderDefault
		{
			public PresetViewHolder(Context context) : base(new AppCompatCheckBox(context)) { }
		}
	}
}