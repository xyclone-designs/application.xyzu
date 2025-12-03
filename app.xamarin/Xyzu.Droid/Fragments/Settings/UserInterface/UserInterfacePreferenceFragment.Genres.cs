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
			public const string Genres = "settings_userinterface_genres_dropdownpreference_key";
			public const string GenresIsReversed = "settings_userinterface_genres_isreversed_switchpreference_key";
			public const string GenresLayoutType = "settings_userinterface_genres_layouttype_listpreference_key";
			public const string GenresSortKey = "settings_userinterface_genres_sortkey_listpreference_key";
		}

		public IGenresSettings? _Genres;

		public IGenresSettings Genres
		{
			get => _Genres ??= XyzuSettings.Instance.GetUserInterfaceGenres();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceGenres(_Genres = value)?
				.Apply();
		}
		public bool GenresIsReversed
		{
			get => Genres.GenresIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IGenresSettings.Keys.IsReversed, Genres.GenresIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes GenresLayoutType
		{
			get => Genres.GenresLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IGenresSettings.Keys.LayoutType, Genres.GenresLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys GenresSortKey
		{
			get => Genres.GenresSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IGenresSettings.Keys.SortKey, Genres.GenresSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? GenresIsReversedPreference { get; set; }
		public XyzuListPreference? GenresLayoutTypePreference { get; set; }
		public XyzuListPreference? GenresSortKeyPreference { get; set; }

		public void GenresOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_genres_title);

			AddPreferenceChangeHandler(
				GenresIsReversedPreference,
				GenresLayoutTypePreference,
				GenresSortKeyPreference);

			OnPropertyChanged(nameof(GenresIsReversed));
			OnPropertyChanged(nameof(GenresLayoutType));
			OnPropertyChanged(nameof(GenresSortKey));
		}
		public void GenresOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				GenresIsReversedPreference,
				GenresLayoutTypePreference,
				GenresSortKeyPreference);
		}
		public void GenresOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				GenresIsReversedPreference = FindPreference(Keys.GenresIsReversed) as XyzuSwitchPreference,
				GenresLayoutTypePreference = FindPreference(Keys.GenresLayoutType) as XyzuListPreference,
				GenresSortKeyPreference = FindPreference(Keys.GenresSortKey) as XyzuListPreference);

			GenresLayoutTypePreference?.SetEntries(
				entries: IGenresSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			GenresLayoutTypePreference?.SetEntryValues(
				entryValues: IGenresSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			GenresSortKeyPreference?.SetEntryValues(
				entryValues: IGenresSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			GenresSortKeyPreference?.SetEntries(
				entries: IGenresSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void GenresOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(GenresIsReversed) when
				GenresIsReversedPreference != null &&
				GenresIsReversedPreference.Checked != GenresIsReversed:
					GenresIsReversedPreference.Checked = GenresIsReversed;
					break;

				case nameof(GenresLayoutType) when
				GenresLayoutTypePreference != null:
					if (IGenresSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(GenresLayoutType) is int layouttypeindex)
						GenresLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(GenresSortKey) when
				GenresSortKeyPreference != null:
					if (IGenresSettings.Options.SortKeys.AsEnumerable().Index(GenresSortKey) is int sortkeyindex)
						GenresSortKeyPreference.SetValueIndex(sortkeyindex);
					break;

				default: break;
			}
		}
		public bool GenresOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == GenresIsReversedPreference:
					GenresIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == GenresLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				GenresLayoutType != librarylayouttype:
					GenresLayoutType = librarylayouttype;
					return true;

				case true when
				preference == GenresSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				GenresSortKey != modelsortkey:
					GenresSortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}