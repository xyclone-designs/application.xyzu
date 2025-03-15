using Android.Content;
using Android.Media;
using Android.Runtime;
using Android.Util;
using Android.Views;

using System;
using System.ComponentModel;
using System.Collections.Generic;

using Xyzu.Droid;
using Xyzu.Settings.Audio;
using Xyzu.Widgets.Controls;

namespace Xyzu.Views.AudioEffects
{
	[Register("xyzu/views/audioeffects/VolumeControlView")]
	public class VolumeControlView : AudioEffectsView
	{
		public const Stream StreamType = Stream.Music;

		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_audioeffects_volumecontrol;

			public const int Volume_Value_RotaryKnob = Resource.Id.xyzu_view_audioeffects_volumecontrol_volume_value_rotaryknob;
			public const int Balance_Value_RotaryKnob = Resource.Id.xyzu_view_audioeffects_volumecontrol_balance_value_rotaryknob;
			public const int Bassboost_Strength_RotaryKnob = Resource.Id.xyzu_view_audioeffects_volumecontrol_bassboost_strength_rotaryknob;
			public const int LoudnessEnhancer_TargetGain_RotaryKnob = Resource.Id.xyzu_view_audioeffects_volumecontrol_loudnessenhancer_targetgain_rotaryknob;
			public const int PlaybackPitch_RotaryKnob = Resource.Id.xyzu_view_audioeffects_volumecontrol_playbackpitch_rotaryknob;
			public const int PlaybackSpeed_RotaryKnob = Resource.Id.xyzu_view_audioeffects_volumecontrol_playbackspeed_rotaryknob;
		}

		public VolumeControlView(Context context) : base(context) { }
		public VolumeControlView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public VolumeControlView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
		public VolumeControlView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);

			Preset = _Preset;
		}

		protected AudioManager? _Audiomanager;
		protected RotaryKnob? _Volume_Value_RotaryKnob;
		protected RotaryKnob? _Balance_Value_RotaryKnob;
		protected RotaryKnob? _Bassboost_Strength_RotaryKnob;
		protected RotaryKnob? _LoudnessEnhancer_TargetGain_RotaryKnob;
		protected RotaryKnob? _PlaybackPitch_RotaryKnob;
		protected RotaryKnob? _PlaybackSpeed_RotaryKnob;
		protected IVolumeControlSettings.IPreset _Preset = new IVolumeControlSettings.IPreset.Default(string.Empty);
		protected IVolumeControlSettings.IPreset? _PresetEdited = null;

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			XyzuBroadcast.Instance.ReceiveActions.AddOrReplace("Xyzu.Views.AudioEffects.VolumeControlView.VolumeChanged", (sender, args) =>
			{
				switch (args.Intent?.Action)
				{
					case XyzuBroadcast.Intents.Actions.VolumeChanged:
						Volume_Value_RotaryKnob.Progress = Audiomanager?.GetStreamVolume(StreamType) ?? Volume_Value_RotaryKnob.Progress;
						break;
					default:
						break;
				}
			});
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			XyzuBroadcast.Instance.ReceiveActions.Remove("Xyzu.Views.AudioEffects.VolumeControlView.VolumeChanged");
		}

		public AudioManager? Audiomanager
		{
			get => _Audiomanager ??= Context is null ? null : AudioManager.FromContext(Context);
		}
		public RotaryKnob Volume_Value_RotaryKnob
		{
			get
			{
				if (_Volume_Value_RotaryKnob is null)
				{
					_Volume_Value_RotaryKnob = FindViewById(Ids.Volume_Value_RotaryKnob) as RotaryKnob ?? throw new InflateException("Volume_Value_RotaryKnob");
					_Volume_Value_RotaryKnob.ProgressStart = 
					_Volume_Value_RotaryKnob.Min = Audiomanager?.GetStreamMinVolume(StreamType) ?? 000;
					_Volume_Value_RotaryKnob.Max = Audiomanager?.GetStreamMaxVolume(StreamType) ?? 100;
					_Volume_Value_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							Audiomanager?.SetStreamVolume(StreamType, args.Progress, default);

						return string.Format("{0:00} %", (double)args.Progress / (double)Volume_Value_RotaryKnob.Max * 100);
					};
				}

				return _Volume_Value_RotaryKnob;
			}
		}
		public RotaryKnob Balance_Value_RotaryKnob
		{
			get
			{
				if (_Balance_Value_RotaryKnob is null)
				{
					_Balance_Value_RotaryKnob = FindViewById(Ids.Balance_Value_RotaryKnob) as RotaryKnob ?? throw new InflateException("Balance_Value_RotaryKnob");
					_Balance_Value_RotaryKnob.Min = IVolumeControlSettings.IPreset.Ranges.BalancePositionLower;
					_Balance_Value_RotaryKnob.Max = IVolumeControlSettings.IPreset.Ranges.BalancePositionUpper;
					_Balance_Value_RotaryKnob.ProgressStart = _Balance_Value_RotaryKnob.Min + _Balance_Value_RotaryKnob.Max;
					_Balance_Value_RotaryKnob.OnProgress = args => 
					{
						if (args.FromUser)
							(PresetEdited ??= new IVolumeControlSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.BalancePosition = (short)args.Progress;
					};
					_Balance_Value_RotaryKnob.SetValue(null);
				}

				return _Balance_Value_RotaryKnob;
			}
		}
		public RotaryKnob Bassboost_Strength_RotaryKnob
		{
			get
			{
				if (_Bassboost_Strength_RotaryKnob is null)
				{
					_Bassboost_Strength_RotaryKnob = FindViewById(Ids.Bassboost_Strength_RotaryKnob) as RotaryKnob ?? throw new InflateException("Bassboost_Strength_RotaryKnob");
					_Bassboost_Strength_RotaryKnob.ProgressStart = 
					_Bassboost_Strength_RotaryKnob.Min = IVolumeControlSettings.IPreset.Ranges.BassBoostStrengthLower;
					_Bassboost_Strength_RotaryKnob.Max = IVolumeControlSettings.IPreset.Ranges.BassBoostStrengthUpper;
					_Bassboost_Strength_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IVolumeControlSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.BassBoostStrength = (short)args.Progress;

						return (PresetEdited ?? Preset).BassBoostStrength.ToString();
					};
				}

				return _Bassboost_Strength_RotaryKnob;
			}
		}
		public RotaryKnob LoudnessEnhancer_TargetGain_RotaryKnob
		{
			get
			{
				if (_LoudnessEnhancer_TargetGain_RotaryKnob is null)
				{
					_LoudnessEnhancer_TargetGain_RotaryKnob = FindViewById(Ids.LoudnessEnhancer_TargetGain_RotaryKnob) as RotaryKnob ?? throw new InflateException("LoudnessEnhancer_TargetGain_RotaryKnob");
					_LoudnessEnhancer_TargetGain_RotaryKnob.ProgressStart = 
					_LoudnessEnhancer_TargetGain_RotaryKnob.Min = IVolumeControlSettings.IPreset.Ranges.LoudnessEnhancerTargetGainLower;
					_LoudnessEnhancer_TargetGain_RotaryKnob.Max = IVolumeControlSettings.IPreset.Ranges.LoudnessEnhancerTargetGainUpper;
					_LoudnessEnhancer_TargetGain_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IVolumeControlSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.LoudnessEnhancerTargetGain = (short)args.Progress;

						return (PresetEdited ?? Preset).LoudnessEnhancerTargetGain.ToString();
					};
				}

				return _LoudnessEnhancer_TargetGain_RotaryKnob;
			}
		}
		public RotaryKnob PlaybackPitch_RotaryKnob
		{
			get
			{
				if (_PlaybackPitch_RotaryKnob is null)
				{
					_PlaybackPitch_RotaryKnob = FindViewById(Ids.PlaybackPitch_RotaryKnob) as RotaryKnob ?? throw new InflateException("PlaybackPitch_RotaryKnob");
					_PlaybackPitch_RotaryKnob.Min = (int)(IVolumeControlSettings.IPreset.Ranges.PlaybackSpeedLower * 100);
					_PlaybackPitch_RotaryKnob.Max = (int)(IVolumeControlSettings.IPreset.Ranges.PlaybackSpeedUpper * 100);
					_PlaybackPitch_RotaryKnob.ProgressStart = (_PlaybackPitch_RotaryKnob.Min + _PlaybackPitch_RotaryKnob.Max) / 2;
					_PlaybackPitch_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IVolumeControlSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.PlaybackPitch = (float)args.Progress / 100;

						return string.Format("{0:00.00X}", (PresetEdited ?? Preset).PlaybackPitch);
					};
				}

				return _PlaybackPitch_RotaryKnob;
			}
		}
		public RotaryKnob PlaybackSpeed_RotaryKnob
		{
			get
			{
				if (_PlaybackSpeed_RotaryKnob is null)
				{
					_PlaybackSpeed_RotaryKnob = FindViewById(Ids.PlaybackSpeed_RotaryKnob) as RotaryKnob ?? throw new InflateException("PlaybackSpeed_RotaryKnob");
					_PlaybackSpeed_RotaryKnob.Min = (int)(IVolumeControlSettings.IPreset.Ranges.PlaybackSpeedLower * 100);
					_PlaybackSpeed_RotaryKnob.Max = (int)(IVolumeControlSettings.IPreset.Ranges.PlaybackSpeedUpper * 100);
					_PlaybackSpeed_RotaryKnob.ProgressStart = (_PlaybackSpeed_RotaryKnob.Min + _PlaybackSpeed_RotaryKnob.Max) / 2;
					_PlaybackSpeed_RotaryKnob.OnProgressValue = args =>
					{
						if (args.FromUser)
							(PresetEdited ??= new IVolumeControlSettings.IPreset.Default(Preset.IsDefault ? string.Empty : Preset.Name, Preset) ?? Preset)
								.PlaybackSpeed = (float)args.Progress / 100;

						return string.Format("{0:00.00X}", (PresetEdited ?? Preset).PlaybackSpeed);
					};
				}

				return _PlaybackSpeed_RotaryKnob;
			}
		}
		public IVolumeControlSettings.IPreset Preset
		{
			get => _Preset;
			set
			{
				_Preset.PropertyChanged -= PresetPropertyChanged;
				_Preset = value;
				_PresetEdited = null;
				_Preset.PropertyChanged += PresetPropertyChanged;

				Volume_Value_RotaryKnob.SetProgress(Audiomanager?.GetStreamVolume(StreamType) ?? 0);
				Balance_Value_RotaryKnob.SetProgress(_Preset.BalancePosition);
				Bassboost_Strength_RotaryKnob.SetProgress(_Preset.BassBoostStrength);
				LoudnessEnhancer_TargetGain_RotaryKnob.SetProgress(_Preset.LoudnessEnhancerTargetGain);
				PlaybackPitch_RotaryKnob.SetProgress((int)_Preset.PlaybackPitch * 100);
				PlaybackSpeed_RotaryKnob.SetProgress((int)_Preset.PlaybackSpeed * 100);
			}
		}
		public IVolumeControlSettings.IPreset? PresetEdited
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