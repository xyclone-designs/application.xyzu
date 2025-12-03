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
			public const string Genre = "settings_userinterface_genre_dropdownpreference_key";
			public const string GenreSongsIsReversed = "settings_userinterface_genre_songs_isreversed_switchpreference_key";
			public const string GenreSongsLayoutType = "settings_userinterface_genre_songs_layouttype_listpreference_key";
			public const string GenreSongsSortKey = "settings_userinterface_genre_songs_sortkey_listpreference_key";
		}

		public IGenreSettings? _Genre;

		public IGenreSettings Genre
		{
			get => _Genre ??= XyzuSettings.Instance.GetUserInterfaceGenre();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceGenre(_Genre = value)?
				.Apply();
		}
		public bool GenreSongsIsReversed
		{
			get => Genre.SongsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IGenreSettings.Keys.SongsIsReversed, Genre.SongsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes GenreSongsLayoutType
		{
			get => Genre.SongsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IGenreSettings.Keys.SongsLayoutType, Genre.SongsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys GenreSongsSortKey
		{
			get => Genre.SongsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IGenreSettings.Keys.SongsSortKey, Genre.SongsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? GenreSongsIsReversedPreference { get; set; }
		public XyzuListPreference? GenreSongsLayoutTypePreference { get; set; }
		public XyzuListPreference? GenreSongsSortKeyPreference { get; set; }

		public void GenreOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_genre_title);

			AddPreferenceChangeHandler(
				GenreSongsIsReversedPreference,
				GenreSongsLayoutTypePreference,
				GenreSongsSortKeyPreference);

			OnPropertyChanged(nameof(GenreSongsIsReversed));
			OnPropertyChanged(nameof(GenreSongsLayoutType));
			OnPropertyChanged(nameof(GenreSongsSortKey));
		}
		public void GenreOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				GenreSongsIsReversedPreference,
				GenreSongsLayoutTypePreference,
				GenreSongsSortKeyPreference);
		}
		public void GenreOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				GenreSongsIsReversedPreference = FindPreference(Keys.SongsIsReversed) as XyzuSwitchPreference,
				GenreSongsLayoutTypePreference = FindPreference(Keys.SongsLayoutType) as XyzuListPreference,
				GenreSongsSortKeyPreference = FindPreference(Keys.SongsSortKey) as XyzuListPreference);

			GenreSongsLayoutTypePreference?.SetEntries(
				entries: IGenreSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			GenreSongsLayoutTypePreference?.SetEntryValues(
				entryValues: IGenreSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());
			;
			GenreSongsSortKeyPreference?.SetEntryValues(
				entryValues: IGenreSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			GenreSongsSortKeyPreference?.SetEntries(
				entries: IGenreSettings.Options.SongsSortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void GenreOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(GenreSongsIsReversed) when
				GenreSongsIsReversedPreference != null &&
				GenreSongsIsReversedPreference.Checked != GenreSongsIsReversed:
					GenreSongsIsReversedPreference.Checked = GenreSongsIsReversed;
					break;

				case nameof(GenreSongsLayoutType) when
				GenreSongsLayoutTypePreference != null:
					if (IGenreSettings.Options.SongsLayoutTypes.AsEnumerable().NeatlyOrdered().Index(GenreSongsLayoutType) is int layouttypeindex)
						GenreSongsLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(GenreSongsSortKey) when
				GenreSongsSortKeyPreference != null:
					if (IGenreSettings.Options.SongsSortKeys.AsEnumerable().Index(GenreSongsSortKey) is int sortkeyindex)
						GenreSongsSortKeyPreference.SetValueIndex(sortkeyindex);
					break;

				default: break;
			}
		}
		public bool GenreOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == GenreSongsIsReversedPreference:
					GenreSongsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == GenreSongsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				GenreSongsLayoutType != librarylayouttype:
					GenreSongsLayoutType = librarylayouttype;
					return true;

				case true when
				preference == GenreSongsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				GenreSongsSortKey != modelsortkey:
					GenreSongsSortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}