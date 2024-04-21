#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.Notification;

using AndroidColor = Android.Graphics.Color;
using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuColorPickerPreference = Xyzu.Preference.ColorPickerPreference;
using XyzuListPreference = Xyzu.Preference.ListPreference;
using XyzuSwitchPreference = Xyzu.Preference.SwitchPreference;

namespace Xyzu.Fragments.Settings.Notification
{
	[Register(FragmentName)]
	public class NotificationPreferenceFragment : BasePreferenceFragment, INotificationSettingsDroid
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.Notification.NotificationPreferenceFragment";

		public static class Keys 
		{
			public const string IsColourised = "settings_notification_iscolourised_switchpreference_key";
			public const string UseCustomColor = "settings_notification_usecustomcolour_switchpreference_key";
			public const string CustomColor = "settings_notification_customcolour_styledcolorpreferencecompat_key";
			public const string Priority = "settings_notification_priority_listpreference_key";
			public const string BadgeIconType = "settings_notification_badgeicontype_listpreference_key";
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

		public XyzuSwitchPreference? IsColourisedPreference { get; set; }
		public XyzuSwitchPreference? UseCustomColourPreference { get; set; }
		public XyzuColorPickerPreference? CustomColourPreference { get; set; }
		public XyzuListPreference? PriorityPreference { get; set; }
		public XyzuListPreference? BadgeIconTypePreference { get; set; }
		
		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_notification_title);

			AddPreferenceChangeHandler(
				IsColourisedPreference,
				UseCustomColourPreference,
				CustomColourPreference,
				PriorityPreference,
				BadgeIconTypePreference);

			INotificationSettingsDroid notification = XyzuSettings.Instance.GetNotificationDroid();

			BadgeIconType = notification.BadgeIconType;
			CustomColour = notification.CustomColour;
			UseCustomColour = notification.UseCustomColour;
			IsColourised = notification.IsColourised;
			Priority = notification.Priority;   
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				IsColourisedPreference,
				UseCustomColourPreference,
				CustomColourPreference,
				PriorityPreference,
				BadgeIconTypePreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutNotificationDroid(this)?
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_notification, rootKey);
			InitPreferences(
				UseCustomColourPreference = FindPreference(Keys.UseCustomColor) as XyzuSwitchPreference,
				CustomColourPreference = FindPreference(Keys.CustomColor) as XyzuColorPickerPreference,
				IsColourisedPreference = FindPreference(Keys.IsColourised) as XyzuSwitchPreference,
				PriorityPreference = FindPreference(Keys.Priority) as XyzuListPreference,
				BadgeIconTypePreference = FindPreference(Keys.BadgeIconType) as XyzuListPreference);

			PriorityPreference?.SetEntries(
				entries: INotificationSettingsDroid.Options.Priority.AsEnumerable()
					.Select(priority => priority.AsStringTitle(Context) ?? priority.ToString())
					.ToArray());
			PriorityPreference?.SetEntryValues(
				entryValues: INotificationSettingsDroid.Options.Priority.AsEnumerable()
					.Select(priority => priority.ToString())
					.ToArray());

			BadgeIconTypePreference?.SetEntries(
				entries: INotificationSettingsDroid.Options.BadgeTypes.AsEnumerable()
					.Select(badgeicontype => badgeicontype.AsStringTitle(Context) ?? badgeicontype.ToString())
					.ToArray());
			BadgeIconTypePreference?.SetEntryValues(
				entryValues: INotificationSettingsDroid.Options.BadgeTypes.AsEnumerable()
					.Select(badgeicontype => badgeicontype.ToString())
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(BadgeIconType):
					if (INotificationSettingsDroid.Options.BadgeTypes.AsEnumerable().Index(BadgeIconType) is int badgeicontypeindex)
						BadgeIconTypePreference?.SetValueIndex(badgeicontypeindex);
					break;

				case nameof(CustomColour):
					if (CustomColourPreference != null)
						CustomColourPreference.CurrentColor = new AndroidColor(CustomColour.ToArgb());
					break;

				case nameof(UseCustomColour):
					if (UseCustomColourPreference != null && UseCustomColourPreference.Checked != UseCustomColour)
						UseCustomColourPreference.Checked = UseCustomColour;
					break;

				case nameof(IsColourised):
					if (IsColourisedPreference != null && IsColourisedPreference.Checked != IsColourised)
						IsColourisedPreference.Checked = IsColourised;
					break;

				case nameof(Priority):
					if (INotificationSettingsDroid.Options.Priority.AsEnumerable().Index(Priority) is int priorityindex)
						PriorityPreference?.SetValueIndex(priorityindex);
					break;

				default: break;
			}
		}
		public override bool OnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == IsColourisedPreference:
					IsColourised = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == UseCustomColourPreference:
					UseCustomColour = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == CustomColourPreference:
					CustomColour = Color.FromArgb(newvalue.JavaCast<Java.Lang.Integer>().IntValue());
					return true;																				     

				case true when
				preference == BadgeIconTypePreference &&
				Enum.TryParse(newvalue.ToString(), out BadgeIconTypes badgeicontype) &&
				BadgeIconType != badgeicontype:
					BadgeIconType = badgeicontype;
					return true;		

				case true when
				preference == PriorityPreference &&
				Enum.TryParse(newvalue.ToString(), out Priorities priority) &&
				Priority != priority:
					Priority = priority;
					return true;

				default: return result;
			}
		}
	}
}