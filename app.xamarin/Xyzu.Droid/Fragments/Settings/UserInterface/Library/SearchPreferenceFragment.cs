#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;

namespace Xyzu.Fragments.Settings.UserInterface.Library
{
	[Register(FragmentName)]
	public class SearchPreferenceFragment : BasePreferenceFragment, ISearchSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.Library.SearchPreferenceFragment";

		public static class Keys										
		{
			public const string LayoutType = "settings_userinterface_library_search_layouttype_listpreference_key";
		}

		private LibraryLayoutTypes _LayoutType;

		public LibraryLayoutTypes LayoutType
		{
			get => _LayoutType;
			set
			{
				_LayoutType = value;

				OnPropertyChanged();
			}
		}
		
		public XyzuListPreference? LayoutTypePreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_library_search_title);

			AddPreferenceChangeHandler(
				LayoutTypePreference);

			ISearchSettings settings = XyzuSettings.Instance.GetUserInterfaceLibrarySearch();

			LayoutType = settings.LayoutType;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				LayoutTypePreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutUserInterfaceLibrarySearch(this)
				.Apply();
		}

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface_library_search, rootKey);	   
			InitPreferences(
				LayoutTypePreference = FindPreference(Keys.LayoutType) as XyzuListPreference);

			LayoutTypePreference?.SetEntries(
				entries: IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			LayoutTypePreference?.SetEntryValues(
				entryValues: IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(LayoutType) when
				LayoutTypePreference != null:
					if (IQueueSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(LayoutType) is int layouttypeindex)
						LayoutTypePreference.SetValueIndex(layouttypeindex);
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
				preference == LayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				LayoutType != librarylayouttype:
					LayoutType = librarylayouttype;
					return true;

				default: return result;
			}
		}
	}
}