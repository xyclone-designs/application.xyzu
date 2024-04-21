#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Xyzu.Settings.Enums;

namespace Xyzu.Settings.Notification
{
	public interface INotificationSettingsDroid<T> : INotificationSettings<T>  
	{
		T BadgeIconType { get; set; }
		T CustomColour { get; set; }
		T UseCustomColour { get; set; }
		T IsColourised { get; set; }
		T Priority { get; set; }
	}
	public interface INotificationSettingsDroid : INotificationSettings 
	{
		public new static class Defaults
		{
			public static INotificationSettingsDroid Default(INotificationSettingsDroid? defaults = null)
			{
				return new Default
				{
					BadgeIconType = defaults?.BadgeIconType ?? BadgeIconType,
					CustomColour = defaults?.CustomColour ?? CustomColour,
					UseCustomColour = defaults?.UseCustomColour ?? UseCustomColour,
					IsColourised = defaults?.IsColourised ?? IsColourised,
					Priority = defaults?.Priority ?? Priority,
				};
			}

			public const BadgeIconTypes BadgeIconType = BadgeIconTypes.Small;
			public static readonly Color CustomColour = Color.Transparent;
			public const bool UseCustomColour = false;
			public const bool IsColourised = true;
			public const Priorities Priority = Priorities.Medium;
		}
		public new class Keys : INotificationSettings.Keys
		{
			public const string BadgeIconType = Base + "." + nameof(BadgeIconType); 
			public const string CustomColour = Base + "." + nameof(CustomColour); 
			public const string UseCustomColour = Base + "." + nameof(UseCustomColour); 
			public const string IsColourised = Base + "." + nameof(IsColourised); 
			public const string Priority = Base + "." + nameof(Priority); 
		}
		public new static class Options
		{
			public class BadgeTypes
			{
				public const BadgeIconTypes None = BadgeIconTypes.None;
				public const BadgeIconTypes Small = BadgeIconTypes.Small;

				public static IEnumerable<BadgeIconTypes> AsEnumerable()
				{
					return Enum
						.GetValues(typeof(BadgeIconTypes))
						.Cast<BadgeIconTypes>();
				}
			}
			public class Priority
			{
				public const Priorities Lowest = Priorities.Lowest;
				public const Priorities Low = Priorities.Low;
				public const Priorities Medium = Priorities.Medium;
				public const Priorities High = Priorities.High;
				public const Priorities Highest = Priorities.Highest;

				public static IEnumerable<Priorities> AsEnumerable()
				{
					return Enum
						.GetValues(typeof(Priorities))
						.Cast<Priorities>();
				}
			}
		}
		BadgeIconTypes BadgeIconType { get; set; }
		Color CustomColour { get; set; }
		bool UseCustomColour { get; set; }
		bool IsColourised { get; set; }
		Priorities Priority { get; set; }

		public new class Default : INotificationSettings.Default, INotificationSettingsDroid
		{
			public Default()
			{
				_BadgeIconType = Defaults.BadgeIconType;
				_CustomColour = Defaults.CustomColour;
				_UseCustomColour = Defaults.UseCustomColour;
				_IsColourised = Defaults.IsColourised;
				_Priority = Defaults.Priority;
			}

			private BadgeIconTypes _BadgeIconType;
			private Color _CustomColour;
			private bool _UseCustomColour;
			private bool _IsColourised;
			private Priorities _Priority;

			public BadgeIconTypes BadgeIconType 
			{
				get => _BadgeIconType;
				set
				{
					_BadgeIconType = value;

					OnPropertyChanged();
				}
			}
			public Color CustomColour 
			{
				get => _CustomColour;
				set
				{
					_CustomColour = value;

					OnPropertyChanged();
				}
			}
			public bool UseCustomColour 
			{
				get => _UseCustomColour;
				set
				{
					_UseCustomColour = value;

					OnPropertyChanged();
				}
			}
			public bool IsColourised 
			{
				get => _IsColourised;
				set
				{
					_IsColourised = value;

					OnPropertyChanged();
				}
			}
			public Priorities Priority 
			{
				get => _Priority;
				set
				{
					_Priority = value;

					OnPropertyChanged();
				}
			}

			public override void SetFromKey(string key, object? value)
			{
				base.SetFromKey(key, value);

				switch (key)
				{	
					case Keys.BadgeIconType when value is BadgeIconTypes badgeicontype:
						BadgeIconType = badgeicontype;
						break;
					
					case Keys.CustomColour when value is Color customcolour:
						CustomColour = customcolour;
						break;
					
					case Keys.UseCustomColour when value is bool usecustomcolour:
						UseCustomColour = usecustomcolour;
						break;
					
					case Keys.IsColourised when value is bool iscolourised:
						IsColourised = iscolourised;
						break;
					
					case Keys.Priority when value is Priorities priority:
						Priority = priority;
						break;

					default: break;
				}
			}
		}
		public new class Default<T> : INotificationSettings.Default<T>, INotificationSettingsDroid<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) 
			{
				BadgeIconType = defaultvalue;
				CustomColour = defaultvalue;
				UseCustomColour = defaultvalue;
				IsColourised = defaultvalue;
				Priority = defaultvalue;								  
			}

			public T BadgeIconType { get; set; }
			public T CustomColour { get; set; }
			public T UseCustomColour { get; set; }
			public T IsColourised { get; set; }
			public T Priority { get; set; }
		}
	}
}
