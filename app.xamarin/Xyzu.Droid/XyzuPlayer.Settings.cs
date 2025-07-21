using Android.Content;
using Android.Media.Audiofx;
using AndroidX.Core.App;

using System.Collections.Generic;
using System.ComponentModel;

using Xyzu.Player.Exoplayer;
using Xyzu.Settings.Audio;
using Xyzu.Settings.Enums;
using Xyzu.Settings.Notification;

namespace Xyzu
{
	public sealed partial class XyzuPlayer 
	{
		private const string _AllPropertyName = "_all";

		private IVolumeControlSettings.IPresetable? _SettingsVolumeControl;
		private IEnvironmentalReverbSettings.IPresetable? _SettingsEnvironmentalReverb;
		private IEqualiserSettings.IPresetable? _SettingsEqualiser;
		private INotificationSettingsDroid? _SettingsNotification;

		private BassBoost.Settings? _EffectsBassBoost;
		private EnvironmentalReverb.Settings? _EffectsEnvironmentalReverb;
		private Equalizer.Settings? _EffectsEqualizer;
		private LoudnessEnhancerFX.Settings? _EffectsLoudnessEnhancer;

		public IVolumeControlSettings.IPresetable SettingsVolumeControl
		{
			get
			{
				if (_SettingsVolumeControl is null)
				{
					_SettingsVolumeControl = XyzuSettings.Instance.GetAudioVolumeControl();
					_SettingsVolumeControl.PropertyChanged += SettingsVolumeControlPropertyChanged;
				}

				return _SettingsVolumeControl;
			}
			set
			{
				if (_SettingsVolumeControl != null)
					_SettingsVolumeControl.PropertyChanged -= SettingsVolumeControlPropertyChanged;

				_SettingsVolumeControl = value;

				if (_SettingsVolumeControl != null)
					_SettingsVolumeControl.PropertyChanged += SettingsVolumeControlPropertyChanged;

				SettingsVolumeControlPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			}
		}
		public IEnvironmentalReverbSettings.IPresetable SettingsEnvironmentalReverb
		{
			get
			{
				if (_SettingsEnvironmentalReverb is null)
				{
					_SettingsEnvironmentalReverb = XyzuSettings.Instance.GetAudioEnvironmentalReverb();
					_SettingsEnvironmentalReverb.PropertyChanged += SettingsEnvironmentalReverbPropertyChanged;
				}

				return _SettingsEnvironmentalReverb;
			}
			set
			{
				if (_SettingsEnvironmentalReverb != null)
					_SettingsEnvironmentalReverb.PropertyChanged -= SettingsEnvironmentalReverbPropertyChanged;

				_SettingsEnvironmentalReverb = value;

				if (_SettingsEnvironmentalReverb != null)
					_SettingsEnvironmentalReverb.PropertyChanged += SettingsEnvironmentalReverbPropertyChanged;

				SettingsEnvironmentalReverbPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			}
		}
		public IEqualiserSettings.IPresetable SettingsEqualiser
		{
			get
			{
				if (_SettingsEqualiser is null)
				{
					_SettingsEqualiser = XyzuSettings.Instance.GetAudioEqualiser();
					_SettingsEqualiser.PropertyChanged += SettingsEqualiserPropertyChanged;
				}

				return _SettingsEqualiser;
			}
			set
			{
				if (_SettingsEqualiser != null)
					_SettingsEqualiser.PropertyChanged -= SettingsEqualiserPropertyChanged;

				_SettingsEqualiser = value;
				
				if (_SettingsEqualiser != null)
					_SettingsEqualiser.PropertyChanged += SettingsEqualiserPropertyChanged;

				SettingsEqualiserPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			}
		}
		public INotificationSettingsDroid? SettingsNotification
		{
			get => _SettingsNotification;
			set
			{
				if (_SettingsNotification != null)
					_SettingsNotification.PropertyChanged += SettingsNotificationPropertyChanged;

				_SettingsNotification = value;

				if (_SettingsNotification != null)
					_SettingsNotification.PropertyChanged += SettingsNotificationPropertyChanged;

				SettingsNotificationPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			}
		}

		public BassBoost.Settings EffectsBassBoost
		{
			get => _EffectsBassBoost ??= new BassBoost.Settings
			{
				Strength = SettingsVolumeControl.CurrentPreset.BassBoostStrength
			};
		}		  
		public EnvironmentalReverb.Settings EffectsEnvironmentalReverb
		{
			get => _EffectsEnvironmentalReverb ??= new EnvironmentalReverb.Settings
			{
				DecayHFRatio = SettingsEnvironmentalReverb.CurrentPreset.DecayHFRatio,
				DecayTime = SettingsEnvironmentalReverb.CurrentPreset.DecayTime,
				Density = SettingsEnvironmentalReverb.CurrentPreset.Density,
				Diffusion = SettingsEnvironmentalReverb.CurrentPreset.Diffusion,
				ReflectionsDelay = SettingsEnvironmentalReverb.CurrentPreset.ReflectionsDelay,
				ReflectionsLevel = SettingsEnvironmentalReverb.CurrentPreset.ReflectionsLevel,
				ReverbDelay = SettingsEnvironmentalReverb.CurrentPreset.ReverbDelay,
				ReverbLevel = SettingsEnvironmentalReverb.CurrentPreset.ReverbLevel,
				RoomHFLevel = SettingsEnvironmentalReverb.CurrentPreset.RoomHFLevel,
				RoomLevel = SettingsEnvironmentalReverb.CurrentPreset.RoomLevel,
			};
		}		  
		public Equalizer.Settings EffectsEqualizer
		{
			get => _EffectsEqualizer ??= new Equalizer.Settings
			{
				NumBands = (short)SettingsEqualiser.CurrentPreset.FrequencyLevels.Length,
				BandLevels = new List<short>(SettingsEqualiser.CurrentPreset.FrequencyLevels),
			};
		}		  
		public LoudnessEnhancerFX.Settings EffectsLoudnessEnhancer
		{
			get => _EffectsLoudnessEnhancer ??= new LoudnessEnhancerFX.Settings
			{
				TargetGain = SettingsVolumeControl.CurrentPreset.LoudnessEnhancerTargetGain
			};
		}

		private void SettingsVolumeControlPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			if (ServiceBinder is null)
				return;

			switch (args.PropertyName)
			{
				case _AllPropertyName:
				case nameof(IVolumeControlSettings.IPresetable.IsEnabled) when SettingsVolumeControl.IsEnabled:
				case nameof(IVolumeControlSettings.IPresetable.CurrentPreset):
				case nameof(IVolumeControlSettings.IPreset.Name):
					ServiceBinder.PlayerService.EffectsPlaybackPitch = SettingsVolumeControl.CurrentPreset.PlaybackPitch;
					ServiceBinder.PlayerService.EffectsPlaybackSpeed = SettingsVolumeControl.CurrentPreset.PlaybackSpeed;
					ServiceBinder.PlayerService.EffectsPlaybackBalance = SettingsVolumeControl.CurrentPreset.BalancePosition;

					if (ServiceBinder.PlayerService.EffectsBassBoost is not null) ServiceBinder.PlayerService.EffectsBassBoost.Properties = EffectsBassBoost;
					if (ServiceBinder.PlayerService.EffectsLoudnessEnhancer is not null) ServiceBinder.PlayerService.EffectsLoudnessEnhancer.Properties = EffectsLoudnessEnhancer;
					return;

				case nameof(IVolumeControlSettings.IPresetable.IsEnabled) when SettingsVolumeControl.IsEnabled is false:
					ServiceBinder.PlayerService.EffectsPlaybackPitch = 1F;
					ServiceBinder.PlayerService.EffectsPlaybackSpeed = 1F;
					ServiceBinder.PlayerService.EffectsPlaybackBalance = 0F;
					ServiceBinder.PlayerService.EffectsBassBoost = null;
					ServiceBinder.PlayerService.EffectsLoudnessEnhancer = null;
					return;

				default: break;
			}

			if (SettingsVolumeControl.IsEnabled)
				switch (args.PropertyName)
				{
					case nameof(IVolumeControlSettings.IPreset.LoudnessEnhancerTargetGain):
						ServiceBinder.PlayerService.EffectsLoudnessEnhancer?.SetTargetGain(SettingsVolumeControl.CurrentPreset.LoudnessEnhancerTargetGain);
						break;

					case nameof(IVolumeControlSettings.IPreset.BassBoostStrength):
						ServiceBinder.PlayerService.EffectsBassBoost?.SetStrength(SettingsVolumeControl.CurrentPreset.BassBoostStrength);
						break;

					case nameof(IVolumeControlSettings.IPreset.BalancePosition):
						ServiceBinder.PlayerService.EffectsPlaybackBalance = SettingsVolumeControl.CurrentPreset.BalancePosition;
						break;

					case nameof(IVolumeControlSettings.IPreset.PlaybackSpeed):
						ServiceBinder.PlayerService.EffectsPlaybackSpeed = SettingsVolumeControl.CurrentPreset.PlaybackSpeed;
						break;

					case nameof(IVolumeControlSettings.IPreset.PlaybackPitch):
						ServiceBinder.PlayerService.EffectsPlaybackPitch = SettingsVolumeControl.CurrentPreset.PlaybackPitch;
						break;

					default: break;
				}
		}
		private void SettingsEnvironmentalReverbPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			if (ServiceBinder is null)
				return;

			switch (args.PropertyName)
			{
				case _AllPropertyName:
				case nameof(IEnvironmentalReverbSettings.IPresetable.IsEnabled) when SettingsEnvironmentalReverb.IsEnabled:
				case nameof(IEnvironmentalReverbSettings.IPresetable.CurrentPreset):
				case nameof(IEnvironmentalReverbSettings.IPreset.Name):
					if (ServiceBinder.PlayerService.EffectsEnvironmentalReverb is not null)
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.Properties = EffectsEnvironmentalReverb;
					return;

				case nameof(IEnvironmentalReverbSettings.IPresetable.IsEnabled) when SettingsEnvironmentalReverb.IsEnabled is false:
					ServiceBinder.PlayerService.EffectsEnvironmentalReverb = null;
					return;

				default: break;
			}

			if (SettingsEnvironmentalReverb.IsEnabled && ServiceBinder.PlayerService.EffectsEnvironmentalReverb is not null) 
				switch (args.PropertyName)
				{
					case nameof(IEnvironmentalReverbSettings.IPreset.DecayHFRatio):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.DecayHFRatio = SettingsEnvironmentalReverb.CurrentPreset.DecayHFRatio;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.DecayTime):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.DecayTime = SettingsEnvironmentalReverb.CurrentPreset.DecayTime;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.Density):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.Density = SettingsEnvironmentalReverb.CurrentPreset.Density;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.Diffusion):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.Diffusion = SettingsEnvironmentalReverb.CurrentPreset.Diffusion;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.ReflectionsDelay):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.ReflectionsDelay = SettingsEnvironmentalReverb.CurrentPreset.ReflectionsDelay;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.ReflectionsLevel):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.ReflectionsLevel = SettingsEnvironmentalReverb.CurrentPreset.ReflectionsLevel;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.ReverbDelay):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.ReverbDelay = SettingsEnvironmentalReverb.CurrentPreset.ReverbDelay;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.ReverbLevel):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.ReverbLevel = SettingsEnvironmentalReverb.CurrentPreset.ReverbLevel;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.RoomHFLevel):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.RoomHFLevel = SettingsEnvironmentalReverb.CurrentPreset.RoomHFLevel;
						break;
					case nameof(IEnvironmentalReverbSettings.IPreset.RoomLevel):
						ServiceBinder.PlayerService.EffectsEnvironmentalReverb.RoomLevel = SettingsEnvironmentalReverb.CurrentPreset.RoomLevel;
						break;

					default: break;
				}
		}
		private void SettingsEqualiserPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			if (ServiceBinder is null)
				return;

			switch (args.PropertyName)
			{
				case _AllPropertyName:
				case nameof(IVolumeControlSettings.IPresetable.IsEnabled) when SettingsEqualiser.IsEnabled:
				case nameof(IVolumeControlSettings.IPresetable.CurrentPreset):
				case nameof(IVolumeControlSettings.IPreset.Name):
					if (ServiceBinder.PlayerService.EffectsEqualizer is not null) { }
					//ServiceBinder.PlayerService.EffectsEqualizer.Properties = EffectsEqualizer;
					return;

				case nameof(IVolumeControlSettings.IPresetable.IsEnabled) when SettingsEqualiser.IsEnabled is false:
					ServiceBinder.PlayerService.EffectsEqualizer = null;
					return;

				default: break;
			}

			if (SettingsEqualiser.IsEnabled && ServiceBinder.PlayerService.EffectsEqualizer is not null) 
				switch (args.PropertyName)
				{
					case nameof(IEqualiserSettings.IPreset.FrequencyLevels):
					//ServiceBinder.PlayerService.EffectsEqualizer.Properties = new Equalizer.Settings
					//{
					//	NumBands = (short)SettingsEqualiser.CurrentPreset.FrequencyLevels.Length,
					//	BandLevels = new List<short>(SettingsEqualiser.CurrentPreset.FrequencyLevels),
					//};

					default: break;
				}
		}						
		private void SettingsNotificationPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			if (ServiceBinder?.PlayerService is ExoPlayerService exoplayerservice)
				switch (args.PropertyName)
				{
					case _AllPropertyName:
						exoplayerservice.NotificationManager.SetBadgeIconType(
							badgeIconType: (SettingsNotification?.BadgeIconType ?? INotificationSettingsDroid.Defaults.BadgeIconType).ToNotifiationCompatInt(NotificationCompat.BadgeIconLarge));

						exoplayerservice.NotificationManager.SetColor(
							color: (SettingsNotification?.UseCustomColour ?? INotificationSettingsDroid.Defaults.UseCustomColour)
								? (SettingsNotification?.CustomColour ?? INotificationSettingsDroid.Defaults.CustomColour).ToArgb()
								: NotificationCompat.ColorDefault);

						exoplayerservice.NotificationManager.SetColorized(
							colorized: SettingsNotification?.IsColourised ?? INotificationSettingsDroid.Defaults.IsColourised);

						exoplayerservice.NotificationManager.SetPriority(
							priority: (SettingsNotification?.Priority ?? INotificationSettingsDroid.Defaults.Priority).ToNotificationPriority(NotificationCompat.PriorityDefault));
						break;

					case nameof(INotificationSettingsDroid.BadgeIconType):
						exoplayerservice.NotificationManager.SetBadgeIconType(
							badgeIconType: (SettingsNotification?.BadgeIconType ?? INotificationSettingsDroid.Defaults.BadgeIconType).ToNotifiationCompatInt(NotificationCompat.BadgeIconLarge));
						break;

					case nameof(INotificationSettingsDroid.UseCustomColour):
					case nameof(INotificationSettingsDroid.CustomColour):
						exoplayerservice.NotificationManager.SetColor(
							color: (SettingsNotification?.UseCustomColour ?? INotificationSettingsDroid.Defaults.UseCustomColour)
								? (SettingsNotification?.CustomColour ?? INotificationSettingsDroid.Defaults.CustomColour).ToArgb()
								: NotificationCompat.ColorDefault);
						break;

					case nameof(INotificationSettingsDroid.IsColourised):
						exoplayerservice.NotificationManager.SetColorized(
							colorized: SettingsNotification?.IsColourised ?? INotificationSettingsDroid.Defaults.IsColourised);
						break;

					case nameof(INotificationSettingsDroid.Priority):
						exoplayerservice.NotificationManager.SetPriority(
							priority: (SettingsNotification?.Priority ?? INotificationSettingsDroid.Defaults.Priority).ToNotificationPriority(NotificationCompat.PriorityDefault));
						break;

					default: break;
				}
		}
	}
}