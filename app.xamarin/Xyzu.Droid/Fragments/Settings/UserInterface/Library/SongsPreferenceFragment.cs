#nullable enable

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

namespace Xyzu.Fragments.Settings.UserInterface.Library
{
	[Register(FragmentName)]
	public class SongsPreferenceFragment : BasePreferenceFragment, ISongsSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.Library.SongsPreferenceFragment";

		public static class Keys										
		{
			public const string IsReversed = "settings_userinterface_library_songs_isreversed_switchpreference_key";
			public const string LayoutType = "settings_userinterface_library_songs_layouttype_listpreference_key";
			public const string SortKey = "settings_userinterface_library_songs_sortkey_listpreference_key";
		}

		private bool _IsReversed;
		private LibraryLayoutTypes _LayoutType;
		private ModelSortKeys _SortKey;

		public bool IsReversed
		{
			get => _IsReversed;
			set
			{

				_IsReversed = value;

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes LayoutType
		{
			get => _LayoutType;
			set
			{

				_LayoutType = value;

				OnPropertyChanged();
			}
		}
		public ModelSortKeys SortKey
		{
			get => _SortKey;
			set
			{

				_SortKey = value;

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? IsReversedPreference { get; set; }
		public XyzuListPreference? LayoutTypePreference { get; set; }
		public XyzuListPreference? SortKeyPreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_library_songs_title);

			AddPreferenceChangeHandler(
				IsReversedPreference,
				LayoutTypePreference,
				SortKeyPreference);

			ISongsSettings settings = XyzuSettings.Instance.GetUserInterfaceLibrarySongs();

			IsReversed = settings.IsReversed;
			LayoutType = settings.LayoutType;
			SortKey = settings.SortKey;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				IsReversedPreference,
				LayoutTypePreference,
				SortKeyPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutUserInterfaceLibrarySongs(this)
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface_library_songs, rootKey);
			InitPreferences(
				IsReversedPreference = FindPreference(Keys.IsReversed) as XyzuSwitchPreference,
				LayoutTypePreference = FindPreference(Keys.LayoutType) as XyzuListPreference,
				SortKeyPreference = FindPreference(Keys.SortKey) as XyzuListPreference);

			LayoutTypePreference?.SetEntries(
				entries: ISongsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			LayoutTypePreference?.SetEntryValues(
				entryValues: ISongsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			SortKeyPreference?.SetEntryValues(
				entryValues: ISongsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			SortKeyPreference?.SetEntries(
				entries: ISongsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(IsReversed) when
				IsReversedPreference != null &&
				IsReversedPreference.Checked != IsReversed:
					IsReversedPreference.Checked = IsReversed;
					break;

				case nameof(LayoutType) when
				LayoutTypePreference != null:
					if (ISongsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(LayoutType) is int layouttypeindex)
						LayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(SortKey) when
				SortKeyPreference != null:
					if (ISongsSettings.Options.SortKeys.AsEnumerable().Index(SortKey) is int sortkeyindex)
						SortKeyPreference.SetValueIndex(sortkeyindex);
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
				preference == IsReversedPreference:
					IsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == LayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				LayoutType != librarylayouttype:
					LayoutType = librarylayouttype;
					return true;

				case true when
				preference == SortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				SortKey != modelsortkey:
					SortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}