using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

using System;
using System.ComponentModel;

using Xyzu.Droid;
using Xyzu.Settings.Audio;
using Xyzu.Widgets.Controls;

namespace Xyzu.Views.AudioEffects
{
	[Register("xyzu/views/audioeffects/EnvironmentalReverbView")]
	public class EnvironmentalReverbView : AudioEffectsView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_audioeffects_environmentalreverb;

			public const int Density_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_density_rotaryknob;
			public const int Diffusion_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_diffusion_rotaryknob;
			public const int Decay_Time_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_decay_time_rotaryknob;
			public const int Decay_HfRatio_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_decay_hfratio_rotaryknob;
			public const int Reflections_Level_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_reflections_level_rotaryknob;
			public const int Reflections_Delay_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_reflections_delay_rotaryknob;
			public const int Reverb_Level_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_reverb_level_rotaryknob;
			public const int Reverb_Delay_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_reverb_delay_rotaryknob;
			public const int Room_Level_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_room_level_rotaryknob;
			public const int Room_HfLevel_RotaryKnob = Resource.Id.xyzu_view_audioeffects_environmentalreverb_room_hflevel_rotaryknob;
		}

		public EnvironmentalReverbView(Context context) : base(context) { }
		public EnvironmentalReverbView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public EnvironmentalReverbView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
		public EnvironmentalReverbView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);

			Preset = _Preset;
		}

		protected RotaryKnob? _Density_RotaryKnob;
		protected RotaryKnob? _Diffusion_RotaryKnob;
		protected RotaryKnob? _Decay_Time_RotaryKnob;
		protected RotaryKnob? _Decay_HfRatio_RotaryKnob;
		protected RotaryKnob? _Reflections_Level_RotaryKnob;
		protected RotaryKnob? _Reflections_Delay_RotaryKnob;
		protected RotaryKnob? _Reverb_Level_RotaryKnob;
		protected RotaryKnob? _Reverb_Delay_RotaryKnob;
		protected RotaryKnob? _Room_Level_RotaryKnob;
		protected RotaryKnob? _Room_HfLevel_RotaryKnob;
		protected IEnvironmentalReverbSettings.IPreset _Preset = new IEnvironmentalReverbSettings.IPreset.Default(string.Empty);
		protected IEnvironmentalReverbSettings.IPreset? _PresetEdited = null;

		public RotaryKnob Density_RotaryKnob
		{
			get
			{
				if (_Density_RotaryKnob is null)
				{
					_Density_RotaryKnob = FindViewById(Ids.Density_RotaryKnob) as RotaryKnob ?? throw new InflateException("Density_RotaryKnob");
					_Density_RotaryKnob.ProgressStart =  
					_Density_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.DensityLower;
					_Density_RotaryKnob.Max = IEnvironmentalReverbSettings.IPreset.Ranges.DensityUpper;
					_Density_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.Density = (short)args.Progress;

						return (PresetEdited ?? Preset).Density.ToString();
					};
				}

				return _Density_RotaryKnob;
			}
		}
		public RotaryKnob Diffusion_RotaryKnob
		{
			get
			{
				if (_Diffusion_RotaryKnob is null)
				{
					_Diffusion_RotaryKnob = FindViewById(Ids.Diffusion_RotaryKnob) as RotaryKnob ?? throw new InflateException("Diffusion_RotaryKnob");
					_Diffusion_RotaryKnob.ProgressStart = 
					_Diffusion_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.DiffusionLower;
					_Diffusion_RotaryKnob.Max = IEnvironmentalReverbSettings.IPreset.Ranges.DiffusionUpper;
					_Diffusion_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.Diffusion = (short)args.Progress;

						return (PresetEdited ?? Preset).Diffusion.ToString();
					};
				}

				return _Diffusion_RotaryKnob;
			}
		}
		public RotaryKnob Decay_Time_RotaryKnob
		{
			get
			{
				if (_Decay_Time_RotaryKnob is null)
				{
					_Decay_Time_RotaryKnob = FindViewById(Ids.Decay_Time_RotaryKnob) as RotaryKnob ?? throw new InflateException("Decay_Time_RotaryKnob");
					_Decay_Time_RotaryKnob.ProgressStart = 
					_Decay_Time_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.DecayTimeLower;
					_Decay_Time_RotaryKnob.Max = IEnvironmentalReverbSettings.IPreset.Ranges.DecayTimeUpper;
					_Decay_Time_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.DecayTime = args.Progress;

						return string.Format("{0:0.00}s", (float)(PresetEdited ?? Preset).DecayTime / 1_000);
					};
				}

				return _Decay_Time_RotaryKnob;
			}
		}
		public RotaryKnob Decay_HfRatio_RotaryKnob
		{
			get
			{
				if (_Decay_HfRatio_RotaryKnob is null)
				{
					_Decay_HfRatio_RotaryKnob = FindViewById(Ids.Decay_HfRatio_RotaryKnob) as RotaryKnob ?? throw new InflateException("Decay_HfRatio_RotaryKnob");
					_Decay_HfRatio_RotaryKnob.ProgressStart = 
					_Decay_HfRatio_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.DecayHFRatioLower;
					_Decay_HfRatio_RotaryKnob.Max = IEnvironmentalReverbSettings.IPreset.Ranges.DecayHFRatioUpper;
					_Decay_HfRatio_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.DecayHFRatio = (short)args.Progress;

						return (PresetEdited ?? Preset).DecayHFRatio.ToString();
					};
				}

				return _Decay_HfRatio_RotaryKnob;
			}
		}
		public RotaryKnob Reflections_Level_RotaryKnob
		{
			get
			{
				if (_Reflections_Level_RotaryKnob is null)
				{
					_Reflections_Level_RotaryKnob = FindViewById(Ids.Reflections_Level_RotaryKnob) as RotaryKnob ?? throw new InflateException("Reflections_Level_RotaryKnob");
					_Reflections_Level_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.ReflectionsLevelLower;
					_Reflections_Level_RotaryKnob.Max = IEnvironmentalReverbSettings.IPreset.Ranges.ReflectionsLevelUpper;
					_Reflections_Level_RotaryKnob.ProgressStart = 0;
					_Reflections_Level_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.RoomHFLevel = (short)args.Progress;

						return (PresetEdited ?? Preset).RoomHFLevel.ToString();
					};
				}

				return _Reflections_Level_RotaryKnob;
			}
		}
		public RotaryKnob Reflections_Delay_RotaryKnob
		{
			get
			{
				if (_Reflections_Delay_RotaryKnob is null)
				{
					_Reflections_Delay_RotaryKnob = FindViewById(Ids.Reflections_Delay_RotaryKnob) as RotaryKnob ?? throw new InflateException("Reflections_Delay_RotaryKnob");
					_Reflections_Delay_RotaryKnob.ProgressStart = 
					_Reflections_Delay_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.ReflectionsDelayLower;
					_Reflections_Delay_RotaryKnob.Max = IEnvironmentalReverbSettings.IPreset.Ranges.ReflectionsDelayUpper;
					_Reflections_Delay_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.ReflectionsDelay = args.Progress;

						return (PresetEdited ?? Preset).ReflectionsDelay.ToString();
					};
				}

				return _Reflections_Delay_RotaryKnob;
			}
		}
		public RotaryKnob Reverb_Level_RotaryKnob
		{
			get
			{
				if (_Reverb_Level_RotaryKnob is null)
				{
					_Reverb_Level_RotaryKnob = FindViewById(Ids.Reverb_Level_RotaryKnob) as RotaryKnob ?? throw new InflateException("Reverb_Level_RotaryKnob");
					_Reverb_Level_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.ReverbLevelLower;
					_Reverb_Level_RotaryKnob.Max = 
					_Reverb_Level_RotaryKnob.ProgressStart = IEnvironmentalReverbSettings.IPreset.Ranges.ReverbLevelUpper;
					_Reverb_Level_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.ReverbLevel = (short)args.Progress;

						return (PresetEdited ?? Preset).ReverbLevel.ToString();
					};
				}

				return _Reverb_Level_RotaryKnob;
			}
		}
		public RotaryKnob Reverb_Delay_RotaryKnob
		{
			get
			{
				if (_Reverb_Delay_RotaryKnob is null)
				{
					_Reverb_Delay_RotaryKnob = FindViewById(Ids.Reverb_Delay_RotaryKnob) as RotaryKnob ?? throw new InflateException("Reverb_Delay_RotaryKnob");
					_Reverb_Delay_RotaryKnob.ProgressStart = 
					_Reverb_Delay_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.ReverbDelayLower;
					_Reverb_Delay_RotaryKnob.Max = IEnvironmentalReverbSettings.IPreset.Ranges.ReverbDelayUpper;
					_Reverb_Delay_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.ReverbDelay = args.Progress;

						return (PresetEdited ?? Preset).ReverbDelay.ToString();
					};
				}

				return _Reverb_Delay_RotaryKnob;
			}
		}
		public RotaryKnob Room_Level_RotaryKnob
		{
			get
			{
				if (_Room_Level_RotaryKnob is null)
				{
					_Room_Level_RotaryKnob = FindViewById(Ids.Room_Level_RotaryKnob) as RotaryKnob ?? throw new InflateException("Room_Level_RotaryKnob");
					_Room_Level_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.RoomLevelLower;
					_Room_Level_RotaryKnob.Max = 
					_Room_Level_RotaryKnob.ProgressStart = IEnvironmentalReverbSettings.IPreset.Ranges.RoomLevelUpper;
					_Room_Level_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.RoomLevel = (short)args.Progress;

						return (PresetEdited ?? Preset).RoomLevel.ToString();
					};
				}

				return _Room_Level_RotaryKnob;
			}
		}
		public RotaryKnob Room_HfLevel_RotaryKnob
		{
			get
			{
				if (_Room_HfLevel_RotaryKnob is null)
				{
					_Room_HfLevel_RotaryKnob = FindViewById(Ids.Room_HfLevel_RotaryKnob) as RotaryKnob ?? throw new InflateException("Room_HfLevel_RotaryKnob");
					_Room_HfLevel_RotaryKnob.Min = IEnvironmentalReverbSettings.IPreset.Ranges.RoomHFLevelLower;
					_Room_HfLevel_RotaryKnob.Max = 
					_Room_HfLevel_RotaryKnob.ProgressStart = IEnvironmentalReverbSettings.IPreset.Ranges.RoomHFLevelUpper;
					_Room_HfLevel_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IEnvironmentalReverbSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.RoomHFLevel = (short)args.Progress;

						return (PresetEdited ?? Preset).RoomHFLevel.ToString();
					};
				}

				return _Room_HfLevel_RotaryKnob;
			}
		}
		public IEnvironmentalReverbSettings.IPreset Preset
		{
			get => _Preset;
			set
			{
				_Preset.PropertyChanged -= PresetPropertyChanged;
				_Preset = value;
				_PresetEdited = null;
				_Preset.PropertyChanged += PresetPropertyChanged;

				Density_RotaryKnob.SetProgress(_Preset.Density);
				Diffusion_RotaryKnob.SetProgress(_Preset.Diffusion);
				Decay_Time_RotaryKnob.SetProgress(_Preset.DecayTime);
				Decay_HfRatio_RotaryKnob.SetProgress(_Preset.DecayHFRatio);
				Reflections_Level_RotaryKnob.SetProgress(_Preset.ReflectionsLevel);
				Reflections_Delay_RotaryKnob.SetProgress(_Preset.ReflectionsDelay);
				Reverb_Level_RotaryKnob.SetProgress(_Preset.ReverbLevel);
				Reverb_Delay_RotaryKnob.SetProgress(_Preset.ReverbDelay);
				Room_Level_RotaryKnob.SetProgress(_Preset.RoomLevel);
				Room_HfLevel_RotaryKnob.SetProgress(_Preset.RoomHFLevel);
			}
		}
		public IEnvironmentalReverbSettings.IPreset? PresetEdited
		{
			get => _PresetEdited;
			set
			{
				if (_PresetEdited is not null)
					_PresetEdited.PropertyChanged -= PresetPropertyChanged;

				_PresetEdited = value;

				if (_PresetEdited is not null)
					_PresetEdited.PropertyChanged += PresetPropertyChanged;

				PresetPropertyChanged(this, new PropertyChangedEventArgs(nameof(PresetEdited)));
			}
		}

		public Action<object?, PropertyChangedEventArgs>? OnPresetPropertyChanged { get; set; }

		private void PresetPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			OnPresetPropertyChanged?.Invoke(sender, args);
		}
	}
}