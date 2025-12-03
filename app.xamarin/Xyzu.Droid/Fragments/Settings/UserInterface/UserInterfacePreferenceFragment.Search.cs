using Android.Content;
using Android.OS;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Library.Enums;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;

namespace Xyzu.Fragments.Settings.UserInterface
{
	public partial class UserInterfacePreferenceFragment
	{
		public static partial class Keys
		{
			public const string Search = "settings_userinterface_search_dropdownpreference_key";
			public const string SearchLayoutType = "settings_userinterface_search_layouttype_listpreference_key";
		}

		public ISearchSettings? _Search;

		public ISearchSettings Search
		{
			get => _Search ??= XyzuSettings.Instance.GetUserInterfaceSearch();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceSearch(_Search = value)?
				.Apply();
		}
		public LibraryLayoutTypes SearchLayoutType
		{
			get => Search.SearchLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(ISearchSettings.Keys.LayoutType, Search.SearchLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuListPreference? SearchLayoutTypePreference { get; set; }

		public void SearchOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_search_title);

			AddPreferenceChangeHandler(
				SearchLayoutTypePreference);

			OnPropertyChanged(nameof(SearchLayoutType));
		}
		public void SearchOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				SearchLayoutTypePreference);
		}
		public void SearchOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				SearchLayoutTypePreference = FindPreference(Keys.SearchLayoutType) as XyzuListPreference);

			SearchLayoutTypePreference?.SetEntries(
				entries: IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			SearchLayoutTypePreference?.SetEntryValues(
				entryValues: IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());
		}
		public void SearchOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(SearchLayoutType) when
				SearchLayoutTypePreference != null:
					if (IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(SearchLayoutType) is int layouttypeindex)
						SearchLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				default: break;
			}
		}
		public bool SearchOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == SearchLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				SearchLayoutType != librarylayouttype:
					SearchLayoutType = librarylayouttype;
					return true;

				default: return result;
			}
		}
	}
}