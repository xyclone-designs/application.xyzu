﻿#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Library.Enums;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface;
using Xyzu.Settings.UserInterface.Library;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;
using XyzuSwitchPreference = Xyzu.Preference.SwitchPreference;

namespace Xyzu.Fragments.Settings.UserInterface.Library
{
	[Register(FragmentName)]
	public class GenrePreferenceFragment : BasePreferenceFragment, IGenreSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.Library.GenrePreferenceFragment";

		public static class Keys										
		{
			public const string SongsIsReversed = "settings_userinterface_library_genre_songs_isreversed_switchpreference_key";
			public const string SongsLayoutType = "settings_userinterface_library_genre_songs_layouttype_listpreference_key";
			public const string SongsSortKey = "settings_userinterface_library_genre_songs_sortkey_listpreference_key";
		}

		private bool _SongsIsReversed;
		private LibraryLayoutTypes _SongsLayoutType;
		private ModelSortKeys _SongsSortKey;

		public bool SongsIsReversed
		{
			get => _SongsIsReversed;
			set
			{
				_SongsIsReversed = value;

				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IAlbumSettings.Keys.SongsIsReversed, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes SongsLayoutType
		{
			get => _SongsLayoutType;
			set
			{
				_SongsLayoutType = value;

				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IAlbumSettings.Keys.SongsLayoutType, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys SongsSortKey
		{
			get => _SongsSortKey;
			set
			{
				_SongsSortKey = value;

				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IGenreSettings.Keys.SongsSortKey, value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? SongsIsReversedPreference { get; set; }
		public XyzuListPreference? SongsLayoutTypePreference { get; set; }
		public XyzuListPreference? SongsSortKeyPreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_library_genre_title);

			AddPreferenceChangeHandler(
				SongsIsReversedPreference,
				SongsLayoutTypePreference,
				SongsSortKeyPreference);

			IGenreSettings settings = XyzuSettings.Instance.GetUserInterfaceLibraryGenre();

			_SongsIsReversed = settings.SongsIsReversed; OnPropertyChanged(nameof(SongsIsReversed));
			_SongsLayoutType = settings.SongsLayoutType; OnPropertyChanged(nameof(SongsLayoutType));
			_SongsSortKey = settings.SongsSortKey; OnPropertyChanged(nameof(SongsSortKey));
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				SongsIsReversedPreference,
				SongsLayoutTypePreference,
				SongsSortKeyPreference);
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface_library_genre, rootKey);
			InitPreferences(
				SongsIsReversedPreference = FindPreference(Keys.SongsIsReversed) as XyzuSwitchPreference,
				SongsLayoutTypePreference = FindPreference(Keys.SongsLayoutType) as XyzuListPreference,
				SongsSortKeyPreference = FindPreference(Keys.SongsSortKey) as XyzuListPreference);

			SongsLayoutTypePreference?.SetEntries(
				entries: IGenreSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			SongsLayoutTypePreference?.SetEntryValues(
				entryValues: IGenreSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			SongsSortKeyPreference?.SetEntryValues(
				entryValues: IGenreSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			SongsSortKeyPreference?.SetEntries(
				entries: IGenreSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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
					if (IGenreSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered().Index(SongsLayoutType) is int layouttypeindex)
						SongsLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(SongsSortKey) when
				SongsSortKeyPreference != null:
					if (IGenreSettings.Options.SongsSortKeys.AsEnumerable().Index(SongsSortKey) is int sortkeyindex)
						SongsSortKeyPreference.SetValueIndex(sortkeyindex);
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