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
			public const string Playlists = "settings_userinterface_playlists_dropdownpreference_key";
			public const string PlaylistsIsReversed = "settings_userinterface_playlists_isreversed_switchpreference_key";
			public const string PlaylistsLayoutType = "settings_userinterface_playlists_layouttype_listpreference_key";
			public const string PlaylistsSortKey = "settings_userinterface_playlists_sortkey_listpreference_key";
		}

		public IPlaylistsSettings? _Playlists;

		public IPlaylistsSettings Playlists
		{
			get => _Playlists ??= XyzuSettings.Instance.GetUserInterfacePlaylists();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfacePlaylists(_Playlists = value)?
				.Apply();
		}
		public bool PlaylistsIsReversed
		{
			get => Playlists.PlaylistsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IPlaylistsSettings.Keys.IsReversed, Playlists.PlaylistsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes PlaylistsLayoutType
		{
			get => Playlists.PlaylistsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IPlaylistsSettings.Keys.LayoutType, Playlists.PlaylistsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys PlaylistsSortKey
		{
			get => Playlists.PlaylistsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IPlaylistsSettings.Keys.SortKey, Playlists.PlaylistsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? PlaylistsIsReversedPreference { get; set; }
		public XyzuListPreference? PlaylistsLayoutTypePreference { get; set; }
		public XyzuListPreference? PlaylistsSortKeyPreference { get; set; }

		public void PlaylistsOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_playlists_title);

			AddPreferenceChangeHandler(
				PlaylistsIsReversedPreference,
				PlaylistsLayoutTypePreference,
				PlaylistsSortKeyPreference);

			OnPropertyChanged(nameof(PlaylistsIsReversed));
			OnPropertyChanged(nameof(PlaylistsLayoutType));
			OnPropertyChanged(nameof(PlaylistsSortKey));
		}
		public void PlaylistsOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				PlaylistsIsReversedPreference,
				PlaylistsLayoutTypePreference,
				PlaylistsSortKeyPreference);
		}
		public void PlaylistsOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				PlaylistsIsReversedPreference = FindPreference(Keys.PlaylistsIsReversed) as XyzuSwitchPreference,
				PlaylistsLayoutTypePreference = FindPreference(Keys.PlaylistsLayoutType) as XyzuListPreference,
				PlaylistsSortKeyPreference = FindPreference(Keys.PlaylistsSortKey) as XyzuListPreference);

			PlaylistsLayoutTypePreference?.SetEntries(
				entries: IPlaylistsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			PlaylistsLayoutTypePreference?.SetEntryValues(
				entryValues: IPlaylistsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			PlaylistsSortKeyPreference?.SetEntryValues(
				entryValues: IPlaylistsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			PlaylistsSortKeyPreference?.SetEntries(
				entries: IPlaylistsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void PlaylistsOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(PlaylistsIsReversed) when
				PlaylistsIsReversedPreference != null &&
				PlaylistsIsReversedPreference.Checked != PlaylistsIsReversed:
					PlaylistsIsReversedPreference.Checked = PlaylistsIsReversed;
					break;

				case nameof(PlaylistsLayoutType) when
				PlaylistsLayoutTypePreference != null:
					if (IPlaylistsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(PlaylistsLayoutType) is int layouttypeindex)
						PlaylistsLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(PlaylistsSortKey) when
				PlaylistsSortKeyPreference != null:
					if (IPlaylistsSettings.Options.SortKeys.AsEnumerable().Index(PlaylistsSortKey) is int sortkeyindex)
						PlaylistsSortKeyPreference.SetValueIndex(sortkeyindex);
					break;

				default: break;
			}
		}
		public bool PlaylistsOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == PlaylistsIsReversedPreference:
					PlaylistsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == PlaylistsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				PlaylistsLayoutType != librarylayouttype:
					PlaylistsLayoutType = librarylayouttype;
					return true;

				case true when
				preference == PlaylistsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				PlaylistsSortKey != modelsortkey:
					PlaylistsSortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}