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
			public const string Playlist = "settings_userinterface_playlist_dropdownpreference_key";
			public const string PlaylistSongsIsReversed = "settings_userinterface_playlist_songs_isreversed_switchpreference_key";
			public const string PlaylistSongsLayoutType = "settings_userinterface_playlist_songs_layouttype_listpreference_key";
			public const string PlaylistSongsSortKey = "settings_userinterface_playlist_songs_sortkey_listpreference_key";
		}

		public IPlaylistSettings? _Playlist;

		public IPlaylistSettings Playlist
		{
			get => _Playlist ??= XyzuSettings.Instance.GetUserInterfacePlaylist();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfacePlaylist(_Playlist = value)?
				.Apply();
		}
		public bool PlaylistSongsIsReversed
		{
			get => Playlist.SongsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IPlaylistSettings.Keys.SongsIsReversed, Playlist.SongsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes PlaylistSongsLayoutType
		{
			get => Playlist.SongsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IPlaylistSettings.Keys.SongsLayoutType, Playlist.SongsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys PlaylistSongsSortKey
		{
			get => Playlist.SongsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IPlaylistSettings.Keys.SongsSortKey, Playlist.SongsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? PlaylistSongsIsReversedPreference { get; set; }
		public XyzuListPreference? PlaylistSongsLayoutTypePreference { get; set; }
		public XyzuListPreference? PlaylistSongsSortKeyPreference { get; set; }

		public void PlaylistOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_playlist_title);

			AddPreferenceChangeHandler(
				PlaylistSongsIsReversedPreference,
				PlaylistSongsLayoutTypePreference,
				PlaylistSongsSortKeyPreference);

			OnPropertyChanged(nameof(PlaylistSongsIsReversed));
			OnPropertyChanged(nameof(PlaylistSongsLayoutType));
			OnPropertyChanged(nameof(PlaylistSongsSortKey));
		}
		public void PlaylistOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				PlaylistSongsIsReversedPreference,
				PlaylistSongsLayoutTypePreference,
				PlaylistSongsSortKeyPreference);
		}
		public void PlaylistOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				PlaylistSongsIsReversedPreference = FindPreference(Keys.SongsIsReversed) as XyzuSwitchPreference,
				PlaylistSongsLayoutTypePreference = FindPreference(Keys.SongsLayoutType) as XyzuListPreference,
				PlaylistSongsSortKeyPreference = FindPreference(Keys.SongsSortKey) as XyzuListPreference);

			PlaylistSongsLayoutTypePreference?.SetEntries(
				entries: IPlaylistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			PlaylistSongsLayoutTypePreference?.SetEntryValues(
				entryValues: IPlaylistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());
			;
			PlaylistSongsSortKeyPreference?.SetEntryValues(
				entryValues: IPlaylistSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			PlaylistSongsSortKeyPreference?.SetEntries(
				entries: IPlaylistSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void PlaylistOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(PlaylistSongsIsReversed) when
				PlaylistSongsIsReversedPreference != null &&
				PlaylistSongsIsReversedPreference.Checked != PlaylistSongsIsReversed:
					PlaylistSongsIsReversedPreference.Checked = PlaylistSongsIsReversed;
					break;

				case nameof(PlaylistSongsLayoutType) when
				PlaylistSongsLayoutTypePreference != null:
					if (IPlaylistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered().Index(PlaylistSongsLayoutType) is int layouttypeindex)
						PlaylistSongsLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(PlaylistSongsSortKey) when
				PlaylistSongsSortKeyPreference != null:
					if (IPlaylistSettings.Options.SongsSortKeys.AsEnumerable().Index(PlaylistSongsSortKey) is int sortkeyindex)
						PlaylistSongsSortKeyPreference.SetValueIndex(sortkeyindex);
					break;

				default: break;
			}
		}
		public bool PlaylistOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == PlaylistSongsIsReversedPreference:
					PlaylistSongsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == PlaylistSongsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				PlaylistSongsLayoutType != librarylayouttype:
					PlaylistSongsLayoutType = librarylayouttype;
					return true;

				case true when
				preference == PlaylistSongsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				PlaylistSongsSortKey != modelsortkey:
					PlaylistSongsSortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}