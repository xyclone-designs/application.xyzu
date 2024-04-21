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
	public class ArtistPreferenceFragment : BasePreferenceFragment, IArtistSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.Library.ArtistPreferenceFragment";

		public static class Keys										
		{
			public const string AlbumsIsReversed = "settings_userinterface_library_artist_albums_isreversed_switchpreference_key";
			public const string AlbumsLayoutType = "settings_userinterface_library_artist_albums_layouttype_listpreference_key";
			public const string AlbumsSortKey = "settings_userinterface_library_artist_albums_sortkey_listpreference_key";				
			public const string SongsIsReversed = "settings_userinterface_library_artist_songs_isreversed_switchpreference_key";
			public const string SongsLayoutType = "settings_userinterface_library_artist_songs_layouttype_listpreference_key";
			public const string SongsSortKey = "settings_userinterface_library_artist_songs_sortkey_listpreference_key";
		}

		private bool _AlbumsIsReversed;
		private LibraryLayoutTypes _AlbumsLayoutType;
		private ModelSortKeys _AlbumsSortKey;						  
		private bool _SongsIsReversed;
		private LibraryLayoutTypes _SongsLayoutType;
		private ModelSortKeys _SongsSortKey;

		public bool AlbumsIsReversed
		{
			get => _AlbumsIsReversed;
			set
			{

				_AlbumsIsReversed = value;

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes AlbumsLayoutType
		{
			get => _AlbumsLayoutType;
			set
			{

				_AlbumsLayoutType = value;

				OnPropertyChanged();
			}
		}
		public ModelSortKeys AlbumsSortKey
		{
			get => _AlbumsSortKey;
			set
			{

				_AlbumsSortKey = value;

				OnPropertyChanged();
			}
		}
									
		public bool SongsIsReversed
		{
			get => _SongsIsReversed;
			set
			{

				_SongsIsReversed = value;

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes SongsLayoutType
		{
			get => _SongsLayoutType;
			set
			{

				_SongsLayoutType = value;

				OnPropertyChanged();
			}
		}
		public ModelSortKeys SongsSortKey
		{
			get => _SongsSortKey;
			set
			{

				_SongsSortKey = value;

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? AlbumsIsReversedPreference { get; set; }
		public XyzuListPreference? AlbumsLayoutTypePreference { get; set; }
		public XyzuListPreference? AlbumsSortKeyPreference { get; set; }					 
		public XyzuSwitchPreference? SongsIsReversedPreference { get; set; }
		public XyzuListPreference? SongsLayoutTypePreference { get; set; }
		public XyzuListPreference? SongsSortKeyPreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_library_artist_title);

			AddPreferenceChangeHandler(
				AlbumsIsReversedPreference,
				AlbumsLayoutTypePreference,
				AlbumsSortKeyPreference,
				SongsIsReversedPreference,
				SongsLayoutTypePreference,
				SongsSortKeyPreference);

			IArtistSettings settings = XyzuSettings.Instance.GetUserInterfaceLibraryArtist();

			AlbumsIsReversed = settings.AlbumsIsReversed;
			AlbumsLayoutType = settings.AlbumsLayoutType;
			AlbumsSortKey = settings.AlbumsSortKey;
			SongsIsReversed = settings.SongsIsReversed;
			SongsLayoutType = settings.SongsLayoutType;
			SongsSortKey = settings.SongsSortKey;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				AlbumsIsReversedPreference,
				AlbumsLayoutTypePreference,
				AlbumsSortKeyPreference,
				SongsIsReversedPreference,
				SongsLayoutTypePreference,
				SongsSortKeyPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutUserInterfaceLibraryArtist(this)
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface_library_artist, rootKey);
			InitPreferences(
				AlbumsIsReversedPreference = FindPreference(Keys.AlbumsIsReversed) as XyzuSwitchPreference,
				AlbumsLayoutTypePreference = FindPreference(Keys.AlbumsLayoutType) as XyzuListPreference,
				AlbumsSortKeyPreference = FindPreference(Keys.AlbumsSortKey) as XyzuListPreference,
				SongsIsReversedPreference = FindPreference(Keys.SongsIsReversed) as XyzuSwitchPreference,
				SongsLayoutTypePreference = FindPreference(Keys.SongsLayoutType) as XyzuListPreference,
				SongsSortKeyPreference = FindPreference(Keys.SongsSortKey) as XyzuListPreference);

			AlbumsLayoutTypePreference?.SetEntries(
				entries: IArtistSettings.Options.AlbumsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			AlbumsLayoutTypePreference?.SetEntryValues(
				entryValues: IArtistSettings.Options.AlbumsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			AlbumsSortKeyPreference?.SetEntryValues(
				entryValues: IArtistSettings.Options.AlbumsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			AlbumsSortKeyPreference?.SetEntries(
				entries: IArtistSettings.Options.AlbumsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());														

			SongsLayoutTypePreference?.SetEntries(
				entries: IArtistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			SongsLayoutTypePreference?.SetEntryValues(
				entryValues: IArtistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			SongsSortKeyPreference?.SetEntryValues(
				entryValues: IArtistSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			SongsSortKeyPreference?.SetEntries(
				entries: IArtistSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(AlbumsIsReversed) when
				AlbumsIsReversedPreference != null &&
				AlbumsIsReversedPreference.Checked != AlbumsIsReversed:
					AlbumsIsReversedPreference.Checked = AlbumsIsReversed;
					break;

				case nameof(AlbumsLayoutType) when
				AlbumsLayoutTypePreference != null:
					if (IArtistSettings.Options.AlbumsLayoutTypes.AsEnumerable().NeatlyOrdered().Index(AlbumsLayoutType) is int albumslayouttypeindex)
						AlbumsLayoutTypePreference.SetValueIndex(albumslayouttypeindex);
					break;

				case nameof(AlbumsSortKey) when
				AlbumsSortKeyPreference != null:
					if (IArtistSettings.Options.AlbumsSortKeys.AsEnumerable().Index(AlbumsSortKey) is int albumssortkeyindex)
						AlbumsSortKeyPreference.SetValueIndex(albumssortkeyindex);
					break;
									   
				case nameof(SongsIsReversed) when
				SongsIsReversedPreference != null &&
				SongsIsReversedPreference.Checked != SongsIsReversed:
					SongsIsReversedPreference.Checked = SongsIsReversed;
					break;

				case nameof(SongsLayoutType) when
				SongsLayoutTypePreference != null:
					if (IArtistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered().Index(SongsLayoutType) is int songslayouttypeindex)
						SongsLayoutTypePreference.SetValueIndex(songslayouttypeindex);
					break;

				case nameof(SongsSortKey) when
				SongsSortKeyPreference != null:
					if (IArtistSettings.Options.SongsSortKeys.AsEnumerable().Index(SongsSortKey) is int songssortkeyindex)
						SongsSortKeyPreference.SetValueIndex(songssortkeyindex);
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
				preference == AlbumsIsReversedPreference:
					AlbumsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == AlbumsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes albumslibrarylayouttype) &&
				AlbumsLayoutType != albumslibrarylayouttype:
					AlbumsLayoutType = albumslibrarylayouttype;
					return true;

				case true when
				preference == AlbumsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys albumsmodelsortkey) &&
				AlbumsSortKey != albumsmodelsortkey:
					AlbumsSortKey = albumsmodelsortkey;
					return true;
											 
				case true when
				preference == SongsIsReversedPreference:
					SongsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == SongsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes songslibrarylayouttype) &&
				SongsLayoutType != songslibrarylayouttype:
					SongsLayoutType = songslibrarylayouttype;
					return true;

				case true when
				preference == SongsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys songsmodelsortkey) &&
				SongsSortKey != songsmodelsortkey:
					SongsSortKey = songsmodelsortkey;
					return true;

				default: return result;
			}
		}
	}
}