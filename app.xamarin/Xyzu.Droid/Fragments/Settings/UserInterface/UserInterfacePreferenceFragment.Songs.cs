using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Library.Enums;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;
using XyzuSwitchPreference = Xyzu.Preference.SwitchPreference;

namespace Xyzu.Fragments.Settings.UserInterface
{
	public partial class UserInterfacePreferenceFragment
	{
		public static partial class Keys
		{
			public const string Songs = "settings_userinterface_songs_dropdownpreference_key";
			public const string SongsIsReversed = "settings_userinterface_songs_isreversed_switchpreference_key";
			public const string SongsLayoutType = "settings_userinterface_songs_layouttype_listpreference_key";
			public const string SongsSortKey = "settings_userinterface_songs_sortkey_listpreference_key";
		}

		public ISongsSettings? _Songs;

		public ISongsSettings Songs
		{
			get => _Songs ??= XyzuSettings.Instance.GetUserInterfaceSongs();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceSongs(_Songs = value)?
				.Apply();
		}
		public bool SongsIsReversed
		{
			get => Songs.SongsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(ISongsSettings.Keys.IsReversed, Songs.SongsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes SongsLayoutType
		{
			get => Songs.SongsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(ISongsSettings.Keys.LayoutType, Songs.SongsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys SongsSortKey
		{
			get => Songs.SongsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(ISongsSettings.Keys.SortKey, Songs.SongsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? SongsIsReversedPreference { get; set; }
		public XyzuListPreference? SongsLayoutTypePreference { get; set; }
		public XyzuListPreference? SongsSortKeyPreference { get; set; }

		public void SongsOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_songs_title);

			AddPreferenceChangeHandler(
				SongsIsReversedPreference,
				SongsLayoutTypePreference,
				SongsSortKeyPreference);

			OnPropertyChanged(nameof(SongsIsReversed));
			OnPropertyChanged(nameof(SongsLayoutType));
			OnPropertyChanged(nameof(SongsSortKey));
		}
		public void SongsOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				SongsIsReversedPreference,
				SongsLayoutTypePreference,
				SongsSortKeyPreference);
		}
		public void SongsOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				SongsIsReversedPreference = FindPreference(Keys.SongsIsReversed) as XyzuSwitchPreference,
				SongsLayoutTypePreference = FindPreference(Keys.SongsLayoutType) as XyzuListPreference,
				SongsSortKeyPreference = FindPreference(Keys.SongsSortKey) as XyzuListPreference);

			SongsLayoutTypePreference?.SetEntries(
				entries: ISongsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			SongsLayoutTypePreference?.SetEntryValues(
				entryValues: ISongsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			SongsSortKeyPreference?.SetEntryValues(
				entryValues: ISongsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			SongsSortKeyPreference?.SetEntries(
				entries: ISongsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void SongsOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(SongsIsReversed) when
				SongsIsReversedPreference != null &&
				SongsIsReversedPreference.Checked != SongsIsReversed:
					SongsIsReversedPreference.Checked = SongsIsReversed;
					break;

				case nameof(SongsLayoutType) when
				SongsLayoutTypePreference != null:
					if (ISongsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(SongsLayoutType) is int layouttypeindex)
						SongsLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(SongsSortKey) when
				SongsSortKeyPreference != null:
					if (ISongsSettings.Options.SortKeys.AsEnumerable().Index(SongsSortKey) is int sortkeyindex)
						SongsSortKeyPreference.SetValueIndex(sortkeyindex);
					break;

				default: break;
			}
		}
		public bool SongsOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == SongsIsReversedPreference:
					SongsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == SongsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				SongsLayoutType != librarylayouttype:
					SongsLayoutType = librarylayouttype;
					return true;

				case true when
				preference == SongsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				SongsSortKey != modelsortkey:
					SongsSortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}