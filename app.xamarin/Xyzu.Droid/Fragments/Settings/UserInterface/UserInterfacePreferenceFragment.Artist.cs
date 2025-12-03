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

namespace Xyzu.Fragments.Settings.UserInterface
{
	public partial class UserInterfacePreferenceFragment
	{
		public static partial class Keys
		{
			public const string Artist = "settings_userinterface_artist_dropdownpreference_key";
			public const string ArtistAlbumsIsReversed = "settings_userinterface_artist_albums_isreversed_switchpreference_key";
			public const string ArtistAlbumsLayoutType = "settings_userinterface_artist_albums_layouttype_listpreference_key";
			public const string ArtistAlbumsSortKey = "settings_userinterface_artist_albums_sortkey_listpreference_key";				
			public const string ArtistSongsIsReversed = "settings_userinterface_artist_songs_isreversed_switchpreference_key";
			public const string ArtistSongsLayoutType = "settings_userinterface_artist_songs_layouttype_listpreference_key";
			public const string ArtistSongsSortKey = "settings_userinterface_artist_songs_sortkey_listpreference_key";
		}

		public IArtistSettings? _Artist;

		public IArtistSettings Artist
		{
			get => _Artist ??= XyzuSettings.Instance.GetUserInterfaceArtist();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceArtist(_Artist = value)?
				.Apply();
		}
		public bool ArtistAlbumsIsReversed
		{
			get => Artist.AlbumsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IArtistSettings.Keys.AlbumsIsReversed, Artist.AlbumsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes ArtistAlbumsLayoutType
		{
			get => Artist.AlbumsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IArtistSettings.Keys.AlbumsLayoutType, Artist.AlbumsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys ArtistAlbumsSortKey
		{
			get => Artist.AlbumsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IArtistSettings.Keys.AlbumsSortKey, Artist.AlbumsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public bool ArtistSongsIsReversed
		{
			get => Artist.SongsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IArtistSettings.Keys.SongsIsReversed, Artist.SongsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes ArtistSongsLayoutType
		{
			get => Artist.SongsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IArtistSettings.Keys.SongsLayoutType, Artist.SongsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys ArtistSongsSortKey
		{
			get => Artist.SongsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IArtistSettings.Keys.SongsSortKey, Artist.SongsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? ArtistAlbumsIsReversedPreference { get; set; }
		public XyzuListPreference? ArtistAlbumsLayoutTypePreference { get; set; }
		public XyzuListPreference? ArtistAlbumsSortKeyPreference { get; set; }					 
		public XyzuSwitchPreference? ArtistSongsIsReversedPreference { get; set; }
		public XyzuListPreference? ArtistSongsLayoutTypePreference { get; set; }
		public XyzuListPreference? ArtistSongsSortKeyPreference { get; set; }

		public void ArtistOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_artist_title);

			AddPreferenceChangeHandler(
				ArtistAlbumsIsReversedPreference,
				ArtistAlbumsLayoutTypePreference,
				ArtistAlbumsSortKeyPreference,
				ArtistSongsIsReversedPreference,
				ArtistSongsLayoutTypePreference,
				ArtistSongsSortKeyPreference);

			OnPropertyChanged(nameof(ArtistAlbumsIsReversed));
			OnPropertyChanged(nameof(ArtistAlbumsLayoutType));
			OnPropertyChanged(nameof(ArtistAlbumsSortKey));
			OnPropertyChanged(nameof(ArtistSongsIsReversed));
			OnPropertyChanged(nameof(ArtistSongsLayoutType));
			OnPropertyChanged(nameof(ArtistSongsSortKey));
		}
		public void ArtistOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				ArtistAlbumsIsReversedPreference,
				ArtistAlbumsLayoutTypePreference,
				ArtistAlbumsSortKeyPreference,
				ArtistSongsIsReversedPreference,
				ArtistSongsLayoutTypePreference,
				ArtistSongsSortKeyPreference);
		}
		public void ArtistOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				ArtistAlbumsIsReversedPreference = FindPreference(Keys.AlbumsIsReversed) as XyzuSwitchPreference,
				ArtistAlbumsLayoutTypePreference = FindPreference(Keys.AlbumsLayoutType) as XyzuListPreference,
				ArtistAlbumsSortKeyPreference = FindPreference(Keys.AlbumsSortKey) as XyzuListPreference,
				ArtistSongsIsReversedPreference = FindPreference(Keys.SongsIsReversed) as XyzuSwitchPreference,
				ArtistSongsLayoutTypePreference = FindPreference(Keys.SongsLayoutType) as XyzuListPreference,
				ArtistSongsSortKeyPreference = FindPreference(Keys.SongsSortKey) as XyzuListPreference);

			ArtistAlbumsLayoutTypePreference?.SetEntries(
				entries: IArtistSettings.Options.AlbumsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			ArtistAlbumsLayoutTypePreference?.SetEntryValues(
				entryValues: IArtistSettings.Options.AlbumsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			ArtistAlbumsSortKeyPreference?.SetEntryValues(
				entryValues: IArtistSettings.Options.AlbumsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			ArtistAlbumsSortKeyPreference?.SetEntries(
				entries: IArtistSettings.Options.AlbumsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());														

			ArtistSongsLayoutTypePreference?.SetEntries(
				entries: IArtistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			ArtistSongsLayoutTypePreference?.SetEntryValues(
				entryValues: IArtistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			ArtistSongsSortKeyPreference?.SetEntryValues(
				entryValues: IArtistSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			ArtistSongsSortKeyPreference?.SetEntries(
				entries: IArtistSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void ArtistOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(ArtistAlbumsIsReversed) when
				ArtistAlbumsIsReversedPreference != null &&
				ArtistAlbumsIsReversedPreference.Checked != ArtistAlbumsIsReversed:
					ArtistAlbumsIsReversedPreference.Checked = ArtistAlbumsIsReversed;
					break;

				case nameof(ArtistAlbumsLayoutType) when
				ArtistAlbumsLayoutTypePreference != null:
					if (IArtistSettings.Options.AlbumsLayoutTypes.AsEnumerable().NeatlyOrdered().Index(ArtistAlbumsLayoutType) is int albumslayouttypeindex)
						ArtistAlbumsLayoutTypePreference.SetValueIndex(albumslayouttypeindex);
					break;

				case nameof(ArtistAlbumsSortKey) when
				ArtistAlbumsSortKeyPreference != null:
					if (IArtistSettings.Options.AlbumsSortKeys.AsEnumerable().Index(ArtistAlbumsSortKey) is int albumssortkeyindex)
						ArtistAlbumsSortKeyPreference.SetValueIndex(albumssortkeyindex);
					break;
									   
				case nameof(ArtistSongsIsReversed) when
				ArtistSongsIsReversedPreference != null &&
				ArtistSongsIsReversedPreference.Checked != ArtistSongsIsReversed:
					ArtistSongsIsReversedPreference.Checked = ArtistSongsIsReversed;
					break;

				case nameof(ArtistSongsLayoutType) when
				ArtistSongsLayoutTypePreference != null:
					if (IArtistSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered().Index(ArtistSongsLayoutType) is int songslayouttypeindex)
						ArtistSongsLayoutTypePreference.SetValueIndex(songslayouttypeindex);
					break;

				case nameof(ArtistSongsSortKey) when
				ArtistSongsSortKeyPreference != null:
					if (IArtistSettings.Options.SongsSortKeys.AsEnumerable().Index(ArtistSongsSortKey) is int songssortkeyindex)
						ArtistSongsSortKeyPreference.SetValueIndex(songssortkeyindex);
					break;

				default: break;
			}
		}
		public bool ArtistOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == ArtistAlbumsIsReversedPreference:
					ArtistAlbumsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == ArtistAlbumsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes albumslibrarylayouttype) &&
				ArtistAlbumsLayoutType != albumslibrarylayouttype:
					ArtistAlbumsLayoutType = albumslibrarylayouttype;
					return true;

				case true when
				preference == ArtistAlbumsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys albumsmodelsortkey) &&
				ArtistAlbumsSortKey != albumsmodelsortkey:
					ArtistAlbumsSortKey = albumsmodelsortkey;
					return true;
											 
				case true when
				preference == ArtistSongsIsReversedPreference:
					ArtistSongsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == ArtistSongsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes songslibrarylayouttype) &&
				ArtistSongsLayoutType != songslibrarylayouttype:
					ArtistSongsLayoutType = songslibrarylayouttype;
					return true;

				case true when
				preference == ArtistSongsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys songsmodelsortkey) &&
				ArtistSongsSortKey != songsmodelsortkey:
					ArtistSongsSortKey = songsmodelsortkey;
					return true;

				default: return result;
			}
		}
	}
}