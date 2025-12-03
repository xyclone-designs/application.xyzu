using Android.Content;
using Android.OS;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Library.Enums;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;

namespace Xyzu.Fragments.Settings.UserInterface
{
	public partial class UserInterfacePreferenceFragment
	{
		public static partial class Keys
		{
			public const string Queue = "settings_userinterface_queue_dropdownpreference_key";
			public const string QueueLayoutType = "settings_userinterface_queue_layouttype_listpreference_key";
		}

		public IQueueSettings? _Queue;

		public IQueueSettings Queue
		{
			get => _Queue ??= XyzuSettings.Instance.GetUserInterfaceQueue();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceQueue(_Queue = value)?
				.Apply();
		}
		public LibraryLayoutTypes QueueLayoutType
		{
			get => Queue.QueueLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IQueueSettings.Keys.LayoutType, Queue.QueueLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuListPreference? QueueLayoutTypePreference { get; set; }

		public void QueueOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_queue_title);

			AddPreferenceChangeHandler(
				QueueLayoutTypePreference);

			OnPropertyChanged(nameof(QueueLayoutType));
		}
		public void QueueOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				QueueLayoutTypePreference);
		}
		public void QueueOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				QueueLayoutTypePreference = FindPreference(Keys.QueueLayoutType) as XyzuListPreference);

			QueueLayoutTypePreference?.SetEntries(
				entries: IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			QueueLayoutTypePreference?.SetEntryValues(
				entryValues: IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());
		}
		public void QueueOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(QueueLayoutType) when
				QueueLayoutTypePreference != null:
					if (IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(QueueLayoutType) is int layouttypeindex)
						QueueLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				default: break;
			}
		}
		public bool QueueOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == QueueLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				QueueLayoutType != librarylayouttype:
					QueueLayoutType = librarylayouttype;
					return true;

				default: return result;
			}
		}
	}
}