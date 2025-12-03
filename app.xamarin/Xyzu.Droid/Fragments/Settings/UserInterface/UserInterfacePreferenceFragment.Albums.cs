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
			public const string Albums = "settings_userinterface_albums_dropdownpreference_key";
			public const string AlbumsIsReversed = "settings_userinterface_albums_isreversed_switchpreference_key";
			public const string AlbumsLayoutType = "settings_userinterface_albums_layouttype_listpreference_key";
			public const string AlbumsSortKey = "settings_userinterface_albums_sortkey_listpreference_key";
		}

		public IAlbumsSettings? _Albums;

		public IAlbumsSettings Albums
		{
			get => _Albums ??= XyzuSettings.Instance.GetUserInterfaceAlbums();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceAlbums(_Albums = value)?
				.Apply();
		}
		public bool AlbumsIsReversed
		{
			get => Albums.AlbumsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IAlbumsSettings.Keys.IsReversed, Albums.AlbumsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes AlbumsLayoutType
		{
			get => Albums.AlbumsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IAlbumsSettings.Keys.LayoutType, Albums.AlbumsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys AlbumsSortKey
		{
			get => Albums.AlbumsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IAlbumsSettings.Keys.SortKey, Albums.AlbumsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? AlbumsIsReversedPreference { get; set; }
		public XyzuListPreference? AlbumsLayoutTypePreference { get; set; }
		public XyzuListPreference? AlbumsSortKeyPreference { get; set; }

		public void AlbumsOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_albums_title);

			AddPreferenceChangeHandler(
				AlbumsIsReversedPreference,
				AlbumsLayoutTypePreference,
				AlbumsSortKeyPreference);

			OnPropertyChanged(nameof(AlbumsIsReversed));
			OnPropertyChanged(nameof(AlbumsLayoutType));
			OnPropertyChanged(nameof(AlbumsSortKey));
		}							   
		public void AlbumsOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				AlbumsIsReversedPreference,
				AlbumsLayoutTypePreference,
				AlbumsSortKeyPreference);
		}
		public void AlbumsOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				AlbumsIsReversedPreference = FindPreference(Keys.AlbumsIsReversed) as XyzuSwitchPreference,
				AlbumsLayoutTypePreference = FindPreference(Keys.AlbumsLayoutType) as XyzuListPreference,
				AlbumsSortKeyPreference = FindPreference(Keys.AlbumsSortKey) as XyzuListPreference);

			AlbumsLayoutTypePreference?.SetEntries(
				entries: IAlbumsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			AlbumsLayoutTypePreference?.SetEntryValues(
				entryValues: IAlbumsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			AlbumsSortKeyPreference?.SetEntryValues(
				entryValues: IAlbumsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			AlbumsSortKeyPreference?.SetEntries(
				entries: IAlbumsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void AlbumsOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch(propertyName)
			{
				case nameof(AlbumsIsReversed) when
				AlbumsIsReversedPreference != null &&
				AlbumsIsReversedPreference.Checked != AlbumsIsReversed:
					AlbumsIsReversedPreference.Checked = AlbumsIsReversed; 
					break;
											  
				case nameof(AlbumsLayoutType) when
				AlbumsLayoutTypePreference != null:
					if (IAlbumsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(AlbumsLayoutType) is int layouttypeindex)
						AlbumsLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;
											  
				case nameof(AlbumsSortKey) when
				AlbumsSortKeyPreference != null:
					if (IAlbumsSettings.Options.SortKeys.AsEnumerable().Index(AlbumsSortKey) is int sortkeyindex)
						AlbumsSortKeyPreference.SetValueIndex(sortkeyindex);
					break;

				default: break;
			}
		}
		public bool AlbumsOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
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
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				AlbumsLayoutType != librarylayouttype:
					AlbumsLayoutType = librarylayouttype;
					return true;	   

				case true when
				preference == AlbumsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				AlbumsSortKey != modelsortkey:
					AlbumsSortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}