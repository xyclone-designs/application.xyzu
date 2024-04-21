#nullable enable

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

		private IBassBoostSettings.IPresetable? _SettingsBassBoost;
		private IEnvironmentalReverbSettings.IPresetable? _SettingsEnvironmentalReverb;
		private IEqualiserSettings.IPresetable? _SettingsEqualiser;
		private ILoudnessEnhancerSettings.IPresetable? _SettingsLoudnessEnhancer;
		private INotificationSettingsDroid? _SettingsNotification;

		private BassBoost.Settings? _EffectsBassBoost;
		private EnvironmentalReverb.Settings? _EffectsEnvironmentalReverb;
		private Equalizer.Settings? _EffectsEqualizer;
		private LoudnessEnhancerEffect.Settings? _EffectsLoudnessEnhancer;

		public IBassBoostSettings.IPresetable? SettingsBassBoost
		{
			get => _SettingsBassBoost;
			set
			{
				if (_SettingsBassBoost != null)
					_SettingsBassBoost.PropertyChanged -= SettingsBassBoostPropertyChanged;

				_SettingsBassBoost = value;

				if (_SettingsBassBoost != null)
					_SettingsBassBoost.PropertyChanged += SettingsBassBoostPropertyChanged;

				SettingsBassBoostPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			}
		}
		public IEnvironmentalReverbSettings.IPresetable? SettingsEnvironmentalReverb
		{
			get => _SettingsEnvironmentalReverb;
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
		public IEqualiserSettings.IPresetable? SettingsEqualiser
		{
			get => _SettingsEqualiser;
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
		public ILoudnessEnhancerSettings.IPresetable? SettingsLoudnessEnhancer
		{
			get => _SettingsLoudnessEnhancer;
			set
			{
				if (_SettingsLoudnessEnhancer != null)
					_SettingsLoudnessEnhancer.PropertyChanged -= SettingsLoudnessEnhancerPropertyChanged;

				_SettingsLoudnessEnhancer = value;

				if (_SettingsLoudnessEnhancer != null)
					_SettingsLoudnessEnhancer.PropertyChanged += SettingsLoudnessEnhancerPropertyChanged;

				SettingsLoudnessEnhancerPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			}
		}		  
		public INotificationSettingsDroid? SettingsNotification
		{
			get => _SettingsNotification;
			set
			{
				if (_SettingsNotification != null)
					_SettingsNotification.PropertyChanged -= SettingsNotificationPropertyChanged;

				_SettingsNotification = value;

				if (_SettingsNotification != null)
					_SettingsNotification.PropertyChanged += SettingsNotificationPropertyChanged;

				SettingsNotificationPropertyChanged(this, new PropertyChangedEventArgs(_AllPropertyName));
			}
		}

		public BassBoost.Settings EffectsBassBoost
		{
			get
			{
				if (_EffectsBassBoost is null)
				{
					_EffectsBassBoost = new BassBoost.Settings();

					if (SettingsBassBoost?.CurrentPreset != null)
					{
						_EffectsBassBoost.Strength = SettingsBassBoost.CurrentPreset.Strength;
					}
				}	

				return _EffectsBassBoost;
			}
		}		  
		public EnvironmentalReverb.Settings EffectsEnvironmentalReverb
		{
			get
			{
				if (_EffectsEnvironmentalReverb is null)
				{
					_EffectsEnvironmentalReverb ??= new EnvironmentalReverb.Settings();

					if (SettingsEnvironmentalReverb?.CurrentPreset != null)
					{
						_EffectsEnvironmentalReverb.DecayHFRatio = SettingsEnvironmentalReverb.CurrentPreset.DecayHFRatio;
						_EffectsEnvironmentalReverb.DecayTime = SettingsEnvironmentalReverb.CurrentPreset.DecayTime;
						_EffectsEnvironmentalReverb.Density = SettingsEnvironmentalReverb.CurrentPreset.Density;
						_EffectsEnvironmentalReverb.Diffusion = SettingsEnvironmentalReverb.CurrentPreset.Diffusion;
						_EffectsEnvironmentalReverb.ReflectionsDelay = SettingsEnvironmentalReverb.CurrentPreset.ReflectionsDelay;
						_EffectsEnvironmentalReverb.ReflectionsLevel = SettingsEnvironmentalReverb.CurrentPreset.ReflectionsLevel;
						_EffectsEnvironmentalReverb.ReverbDelay = SettingsEnvironmentalReverb.CurrentPreset.ReverbDelay;
						_EffectsEnvironmentalReverb.ReverbLevel = SettingsEnvironmentalReverb.CurrentPreset.ReverbLevel;
						_EffectsEnvironmentalReverb.RoomHFLevel = SettingsEnvironmentalReverb.CurrentPreset.RoomHFLevel;
						_EffectsEnvironmentalReverb.RoomLevel = SettingsEnvironmentalReverb.CurrentPreset.RoomLevel;
					}
				}

				return _EffectsEnvironmentalReverb;
			}
		}		  
		public Equalizer.Settings EffectsEqualizer
		{
			get
			{
				if (_EffectsEqualizer is null)
				{
					_EffectsEqualizer ??= new Equalizer.Settings();

					if (SettingsEqualiser?.CurrentPreset != null)
					{
						_EffectsEqualizer.NumBands = (short)SettingsEqualiser.CurrentPreset.FrequencyLevels.Length;
						_EffectsEqualizer.BandLevels = new List<short>(SettingsEqualiser.CurrentPreset.FrequencyLevels);
					}
				}

				return _EffectsEqualizer;
			}
		}		  
		public LoudnessEnhancerEffect.Settings EffectsLoudnessEnhancer
		{
			get
			{
				if (_EffectsLoudnessEnhancer is null)
				{
					_EffectsLoudnessEnhancer ??= new LoudnessEnhancerEffect.Settings();

					if (SettingsLoudnessEnhancer?.CurrentPreset != null)
					{
						_EffectsLoudnessEnhancer.TargetGain = SettingsLoudnessEnhancer.CurrentPreset.TargetGain;
					}
				}

				return _EffectsLoudnessEnhancer;
			}
		}

		private void SettingsBassBoostPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (ServiceBinder?.PlayerService?.EffectsBassBoost != null && SettingsBassBoost?.CurrentPreset != null) switch (args.PropertyName)
			{
				case _AllPropertyName:
				case nameof(IBassBoostSettings.IPresetable.CurrentPreset):
				case nameof(IBassBoostSettings.IPreset.Name):
					ServiceBinder.PlayerService.EffectsBassBoost.Properties = new BassBoost.Settings
					{
						Strength = SettingsBassBoost.CurrentPreset.Strength
					};
					break;

					case nameof(IBassBoostSettings.IPreset.Strength):
					ServiceBinder.PlayerService.EffectsBassBoost.SetStrength(SettingsBassBoost.CurrentPreset.Strength);
					break;

				default: break;
			}
		}
		private void SettingsEnvironmentalReverbPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (ServiceBinder?.PlayerService?.EffectsEnvironmentalReverb != null && SettingsEnvironmentalReverb?.CurrentPreset != null) switch (args.PropertyName)
			{
				case _AllPropertyName:
				case nameof(IEnvironmentalReverbSettings.IPresetable.CurrentPreset):
				case nameof(IEnvironmentalReverbSettings.IPreset.Name):
					ServiceBinder.PlayerService.EffectsEnvironmentalReverb.Properties = new EnvironmentalReverb.Settings
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
					break;

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
		private void SettingsEqualiserPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (ServiceBinder?.PlayerService?.EffectsEqualizer != null && SettingsEqualiser?.CurrentPreset != null) switch (args.PropertyName)
			{
				case _AllPropertyName:
				case nameof(IEqualiserSettings.IPresetable.CurrentPreset):
				case nameof(IEqualiserSettings.IPreset.Name):
				case nameof(IEqualiserSettings.IPreset.FrequencyLevels):
					ServiceBinder.PlayerService.EffectsEqualizer.Properties = new Equalizer.Settings
					{
						NumBands = (short)SettingsEqualiser.CurrentPreset.FrequencyLevels.Length,
						BandLevels = new List<short>(SettingsEqualiser.CurrentPreset.FrequencyLevels),
					};
					break;

				default: break;
			}
		}
		private void SettingsLoudnessEnhancerPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (ServiceBinder?.PlayerService?.EffectsLoudnessEnhancer != null && SettingsLoudnessEnhancer?.CurrentPreset != null) switch (args.PropertyName)
			{
				case _AllPropertyName:
				case nameof(ILoudnessEnhancerSettings.IPresetable.CurrentPreset):
				case nameof(ILoudnessEnhancerSettings.IPreset.Name):
				case nameof(ILoudnessEnhancerSettings.IPreset.TargetGain):
					ServiceBinder.PlayerService.EffectsLoudnessEnhancer.SetTargetGain(SettingsLoudnessEnhancer.CurrentPreset.TargetGain);
					break;

				default: break;
			}
		}									
		private void SettingsNotificationPropertyChanged(object sender, PropertyChangedEventArgs args)
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