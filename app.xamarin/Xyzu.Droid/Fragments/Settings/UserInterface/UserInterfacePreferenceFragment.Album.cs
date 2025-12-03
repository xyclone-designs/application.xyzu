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
			public const string Album = "settings_userinterface_album_dropdownpreference_key";
			public const string AlbumSongsIsReversed = "settings_userinterface_album_songs_isreversed_switchpreference_key";
			public const string AlbumSongsLayoutType = "settings_userinterface_album_songs_layouttype_listpreference_key";
			public const string AlbumSongsSortKey = "settings_userinterface_album_songs_sortkey_listpreference_key";
		}

		public IAlbumSettings? _Album;
		
		public IAlbumSettings Album
		{
			get => _Album ??= XyzuSettings.Instance.GetUserInterfaceAlbum();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceAlbum(_Album = value)?
				.Apply();
		}
		public bool AlbumSongsIsReversed
		{
			get => Album.SongsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IAlbumSettings.Keys.SongsIsReversed, Album.SongsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes AlbumSongsLayoutType
		{
			get => Album.SongsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IAlbumSettings.Keys.SongsLayoutType, Album.SongsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys AlbumSongsSortKey
		{
			get => Album.SongsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IAlbumSettings.Keys.SongsSortKey, Album.SongsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? AlbumSongsIsReversedPreference { get; set; }
		public XyzuListPreference? AlbumSongsLayoutTypePreference { get; set; }
		public XyzuListPreference? AlbumSongsSortKeyPreference { get; set; }

		public void AlbumOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_album_title);

			AddPreferenceChangeHandler(
				AlbumSongsIsReversedPreference,
				AlbumSongsLayoutTypePreference,
				AlbumSongsSortKeyPreference);

			OnPropertyChanged(nameof(AlbumSongsIsReversed));
			OnPropertyChanged(nameof(AlbumSongsLayoutType));
			OnPropertyChanged(nameof(AlbumSongsSortKey));
		}
		public void AlbumOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				AlbumSongsIsReversedPreference,
				AlbumSongsLayoutTypePreference,
				AlbumSongsSortKeyPreference);
		}
		public void AlbumOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				AlbumSongsIsReversedPreference = FindPreference(Keys.SongsIsReversed) as XyzuSwitchPreference,
				AlbumSongsLayoutTypePreference = FindPreference(Keys.SongsLayoutType) as XyzuListPreference,
				AlbumSongsSortKeyPreference = FindPreference(Keys.SongsSortKey) as XyzuListPreference);

			AlbumSongsLayoutTypePreference?.SetEntries(
				entries: IAlbumSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			AlbumSongsLayoutTypePreference?.SetEntryValues(
				entryValues: IAlbumSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());
;
			AlbumSongsSortKeyPreference?.SetEntryValues(
				entryValues: IAlbumSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			AlbumSongsSortKeyPreference?.SetEntries(
				entries: IAlbumSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void AlbumOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(AlbumSongsIsReversed) when
				AlbumSongsIsReversedPreference != null &&
				AlbumSongsIsReversedPreference.Checked != AlbumSongsIsReversed:
					AlbumSongsIsReversedPreference.Checked = AlbumSongsIsReversed;
					break;

				case nameof(AlbumSongsLayoutType) when
				AlbumSongsLayoutTypePreference != null:
					if (IAlbumSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered().Index(AlbumSongsLayoutType) is int layouttypeindex)
						AlbumSongsLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(AlbumSongsSortKey) when
				AlbumSongsSortKeyPreference != null:
					if (IAlbumSettings.Options.SongsSortKeys.AsEnumerable().Index(AlbumSongsSortKey) is int sortkeyindex)
						AlbumSongsSortKeyPreference.SetValueIndex(sortkeyindex);
					break;

				default: break;
			}
		}
		public bool AlbumOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == AlbumSongsIsReversedPreference:
					AlbumSongsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == AlbumSongsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				AlbumSongsLayoutType != librarylayouttype:
					AlbumSongsLayoutType = librarylayouttype;
					return true;

				case true when
				preference == AlbumSongsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				AlbumSongsSortKey != modelsortkey:
					AlbumSongsSortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}