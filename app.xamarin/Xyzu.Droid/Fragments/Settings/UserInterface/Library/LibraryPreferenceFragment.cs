#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface.Library;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;
using XyzuMultiSelectListPreference = Xyzu.Preference.MultiSelectListPreference;

namespace Xyzu.Fragments.Settings.UserInterface.Library
{
	[Register(FragmentName)]
	public class LibraryPreferenceFragment : BasePreferenceFragment, ILibrarySettingsDroid
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.LibraryPreferenceFragment";

		public static class Keys										
		{
			public const string NavigationType = "settings_userinterface_library_navigationtype_listpreference_key";
			public const string HeaderScrollType = "settings_userinterface_library_headerscrolltype_listpreference_key";
			public const string PageDefault = "settings_userinterface_library_pagedefault_listpreference_key";
			public const string PagesOrdered = "settings_userinterface_library_pagesordered_multiselectlistpreference_key";

			public const string Album = "settings_userinterface_library_album_preferencescreen_key";
			public const string Albums = "settings_userinterface_library_albums_preferencescreen_key";
			public const string Artist = "settings_userinterface_library_artist_preferencescreen_key";
			public const string Artists = "settings_userinterface_library_artists_preferencescreen_key";
			public const string Genre = "settings_userinterface_library_genre_preferencescreen_key";
			public const string Genres = "settings_userinterface_library_genres_preferencescreen_key";
			public const string Playlist = "settings_userinterface_library_playlist_preferencescreen_key";
			public const string Playlists = "settings_userinterface_library_playlists_preferencescreen_key";
			public const string Queue = "settings_userinterface_library_queue_preferencescreen_key";
			public const string Search = "settings_userinterface_library_search_preferencescreen_key";
			public const string Songs = "settings_userinterface_library_songs_preferencescreen_key";
		}

		private LibraryNavigationTypes _NavigationType;
		private LibraryHeaderScrollTypes _HeaderScrollType;
		private LibraryPages _PageDefault;
		private IEnumerable<LibraryPages>? _PagesOrdered;
		   
		public LibraryNavigationTypes NavigationType
		{
			get => _NavigationType;
			set
			{
				_NavigationType = value;

				OnPropertyChanged();
			}
		}
		public LibraryHeaderScrollTypes HeaderScrollType
		{
			get => _HeaderScrollType;
			set
			{
				_HeaderScrollType = value;

				OnPropertyChanged();
			}
		}
		public LibraryPages PageDefault
		{
			get => _PageDefault;
			set
			{
				_PageDefault = value;

				OnPropertyChanged();
			}
		}
		public IEnumerable<LibraryPages> PagesOrdered
		{
			get => _PagesOrdered ?? Enumerable.Empty<LibraryPages>();
			set
			{
				_PagesOrdered = value;

				OnPropertyChanged();
			}
		}

		public XyzuListPreference? HeaderScrollTypePreference { get; set; }
		public XyzuListPreference? NavigationTypePreference { get; set; }
		public XyzuListPreference? PageDefaultPreference { get; set; }
		public XyzuMultiSelectListPreference? PagesOrderedPreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_library_title);

			AddPreferenceChangeHandler(
				NavigationTypePreference,
				HeaderScrollTypePreference,
				PageDefaultPreference,
				PagesOrderedPreference);

			ILibrarySettingsDroid settings = XyzuSettings.Instance.GetUserInterfaceLibraryDroid();

			HeaderScrollType = settings.HeaderScrollType;
			NavigationType = settings.NavigationType;
			PageDefault = settings.PageDefault;
			PagesOrdered = settings.PagesOrdered;
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				HeaderScrollTypePreference,
				NavigationTypePreference,
				PageDefaultPreference,
				PagesOrderedPreference);

			XyzuSettings.Instance.SharedPreferences?
				.Edit()?
				.PutUserInterfaceLibraryDroid(this)?
				.Apply();
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface_library, rootKey);
			InitPreferences(
				NavigationTypePreference = FindPreference(Keys.NavigationType) as XyzuListPreference,
				HeaderScrollTypePreference = FindPreference(Keys.HeaderScrollType) as XyzuListPreference,
				PageDefaultPreference = FindPreference(Keys.PageDefault) as XyzuListPreference,
				PagesOrderedPreference = FindPreference(Keys.PagesOrdered) as XyzuMultiSelectListPreference,
				FindPreference(Keys.Album),
				FindPreference(Keys.Albums),
				FindPreference(Keys.Artist),
				FindPreference(Keys.Artists),
				FindPreference(Keys.Genre),
				FindPreference(Keys.Genres),
				FindPreference(Keys.Playlist),
				FindPreference(Keys.Playlists),
				FindPreference(Keys.Queue),
				FindPreference(Keys.Search),
				FindPreference(Keys.Songs));

			NavigationTypePreference?.SetEntries(
				entries: ILibrarySettingsDroid.Options.NavigationTypes.AsEnumerable()
					.Select(type => type.ToString())
					.ToArray());
			NavigationTypePreference?.SetEntryValues(
				entryValues: ILibrarySettingsDroid.Options.NavigationTypes.AsEnumerable()
					.Select(type => type.AsStringTitle(Context) ?? type.ToString())
					.ToArray());

			HeaderScrollTypePreference?.SetEntries(
				entries: ILibrarySettingsDroid.Options.HeaderScrollTypes.AsEnumerable()
					.Select(type => type.ToString())
					.ToArray());
			HeaderScrollTypePreference?.SetEntryValues(
				entryValues: ILibrarySettingsDroid.Options.HeaderScrollTypes.AsEnumerable()
					.Select(type => type.AsStringTitle(Context) ?? type.ToString())
					.ToArray());

			PageDefaultPreference?.SetEntries(
				entries: ILibrarySettingsDroid.Options.Pages.AsEnumerable()
					.Select(page => page.ToString())
					.ToArray());
			PageDefaultPreference?.SetEntryValues(
				entryValues: ILibrarySettingsDroid.Options.Pages.AsEnumerable()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			PagesOrderedPreference?.SetEntries(
				entries: ILibrarySettingsDroid.Options.Pages.AsEnumerable()
					.Select(page => page.ToString())
					.ToArray());
			PagesOrderedPreference?.SetEntryValues(
				entryValues: ILibrarySettingsDroid.Options.Pages.AsEnumerable()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(NavigationType) when
				NavigationTypePreference != null:
					if (ILibrarySettingsDroid.Options.NavigationTypes.AsEnumerable().Index(NavigationType) is int navigationtypeindex)
						NavigationTypePreference.SetValueIndex(navigationtypeindex);
					break;
							
				case nameof(HeaderScrollType) when
				HeaderScrollTypePreference != null:
					if (ILibrarySettingsDroid.Options.HeaderScrollTypes.AsEnumerable().Index(HeaderScrollType) is int headerscrolltypeindex)
						HeaderScrollTypePreference.SetValueIndex(headerscrolltypeindex);
					break;
										
				case nameof(PageDefault) when
				PageDefaultPreference != null:
					if (ILibrarySettingsDroid.Options.Pages.AsEnumerable().Index(PageDefault) is int pagedefaultindex)
						PageDefaultPreference.SetValueIndex(pagedefaultindex);
					break;

				case nameof(PagesOrdered) when
				PagesOrderedPreference != null:
					PagesOrderedPreference.Values = PagesOrdered
						.Select(page => page.ToString())
						.ToList();
					break;

				default: break;
			}
		}
		public override bool OnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			switch (true)
			{
				case true when
				preference == NavigationTypePreference &&
				newvalue?.ToString() is string newvaluestring &&
				Enum.TryParse(newvaluestring, out LibraryNavigationTypes navigationtype) &&
				NavigationType != navigationtype:
					NavigationType = navigationtype;
					return true;
													
				case true when
				preference == HeaderScrollTypePreference &&
				newvalue?.ToString() is string newvaluestring &&
				Enum.TryParse(newvaluestring, out LibraryHeaderScrollTypes headerscrolltype) &&
				HeaderScrollType != headerscrolltype:
					HeaderScrollType = headerscrolltype;
					return true;

				case true when
				preference == PageDefaultPreference &&
				newvalue?.ToString() is string newvaluestring &&
				Enum.TryParse(newvaluestring, out LibraryPages librarypage) &&
				PageDefault != librarypage:
					PageDefault = librarypage;
					return true;

				case true when
				preference == PagesOrderedPreference:
					PagesOrdered = PagesOrderedPreference.Values
						.Select(value =>
						{
							if (Enum.TryParse(value, out LibraryPages librarypage))
								return librarypage;

							return new LibraryPages?();

						}).OfType<LibraryPages>().ToList();
					return true;

				default: return result;
			}
		}
	}
}