using Android.Content;
using Android.OS;
using Android.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Settings.Enums;
using Xyzu.Settings.UserInterface;
using Xyzu.Settings.UserInterface.Library;

using AndroidXPreference = AndroidX.Preference.Preference;
using XyzuListPreference = Xyzu.Preference.ListPreference;
using XyzuMultiSelectListPreference = Xyzu.Preference.MultiSelectListPreference;
using XyzuSwitchPreference = Xyzu.Preference.SwitchPreference;

namespace Xyzu.Fragments.Settings.UserInterface
{
	[Register(FragmentName)]
	public partial class UserInterfacePreferenceFragment : BasePreferenceFragment, IUserInterfaceSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.UserInterface.UserInterfacePreferenceFragment";

		public static partial class Keys
		{
			public const string NavigationType = "settings_userinterface_navigationtype_listpreference_key";
			public const string HeaderScrollType = "settings_userinterface_headerscrolltype_listpreference_key";
			public const string PageDefault = "settings_userinterface_pagedefault_listpreference_key";
			public const string PagesOrdered = "settings_userinterface_pagesordered_multiselectlistpreference_key";
			public const string NowPlayingForceShow = "settings_userinterface_nowplayingforceshow_switchpreference_key";
		}

		private LibraryNavigationTypes _NavigationType;
		private LibraryHeaderScrollTypes _HeaderScrollType;
		private LibraryPages _PageDefault;
		private IEnumerable<LibraryPages>? _PagesOrdered;
		private bool _NowPlayingForceShow;

		public LibraryNavigationTypes NavigationType
		{
			get => _NavigationType;
			set
			{
				_NavigationType = value;

				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IUserInterfaceSettingsDroid.Keys.NavigationType, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryHeaderScrollTypes HeaderScrollType
		{
			get => _HeaderScrollType;
			set
			{
				_HeaderScrollType = value;

				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IUserInterfaceSettingsDroid.Keys.HeaderScrollType, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryPages PageDefault
		{
			get => _PageDefault;
			set
			{
				_PageDefault = value;

				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IUserInterfaceSettingsDroid.Keys.PageDefault, value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public IEnumerable<LibraryPages> PagesOrdered
		{
			get => _PagesOrdered ?? Enumerable.Empty<LibraryPages>();
			set
			{
				_PagesOrdered = value;

				XyzuSettings.Instance
					.Edit()?
					//.PutUserInterface(this)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public bool NowPlayingForceShow
		{
			get => _NowPlayingForceShow;
			set
			{

				_NowPlayingForceShow = value;

				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IUserInterfaceSettingsDroid.Keys.NowPlayingForceShow, value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuListPreference? HeaderScrollTypePreference { get; set; }
		public XyzuListPreference? NavigationTypePreference { get; set; }
		public XyzuListPreference? PageDefaultPreference { get; set; }
		public XyzuMultiSelectListPreference? PagesOrderedPreference { get; set; }
		public XyzuSwitchPreference? NowPlayingForceShowPreference { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_title);

			AddPreferenceChangeHandler(
				NavigationTypePreference,
				HeaderScrollTypePreference,
				PageDefaultPreference,
				PagesOrderedPreference,
				NowPlayingForceShowPreference);

			IUserInterfaceSettingsDroid settings = XyzuSettings.Instance.GetUserInterfaceDroid();

			_HeaderScrollType = settings.HeaderScrollType; OnPropertyChanged(nameof(HeaderScrollType));
			_NavigationType = settings.NavigationType; OnPropertyChanged(nameof(NavigationType));
			_PageDefault = settings.PageDefault; OnPropertyChanged(nameof(PageDefault));
			_PagesOrdered = settings.PagesOrdered; OnPropertyChanged(nameof(PagesOrdered));
			_NowPlayingForceShow = settings.NowPlayingForceShow; OnPropertyChanged(nameof(NowPlayingForceShow));

			AlbumOnResume();
			AlbumsOnResume();
			ArtistOnResume();
			ArtistsOnResume();
			GenreOnResume();
			GenresOnResume();
			PlaylistOnResume();
			PlaylistsOnResume();
			QueueOnResume();
			SearchOnResume();
			SongsOnResume();
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				HeaderScrollTypePreference,
				NavigationTypePreference,
				PageDefaultPreference,
				PagesOrderedPreference,
				NowPlayingForceShowPreference);

			AlbumOnPause();
			AlbumsOnPause();
			ArtistOnPause();
			ArtistsOnPause();
			GenreOnPause();
			GenresOnPause();
			PlaylistOnPause();
			PlaylistsOnPause();
			QueueOnPause();
			SearchOnPause();
			SongsOnPause();
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_userinterface, rootKey);
			InitPreferences(
				NavigationTypePreference = FindPreference(Keys.NavigationType) as XyzuListPreference,
				HeaderScrollTypePreference = FindPreference(Keys.HeaderScrollType) as XyzuListPreference,
				PageDefaultPreference = FindPreference(Keys.PageDefault) as XyzuListPreference,
				PagesOrderedPreference = FindPreference(Keys.PagesOrdered) as XyzuMultiSelectListPreference,
				NowPlayingForceShowPreference = FindPreference(Keys.NowPlayingForceShow) as XyzuSwitchPreference,
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
				entries: IUserInterfaceSettingsDroid.Options.NavigationTypes.AsEnumerable()
					.Select(type => type.ToString())
					.ToArray());
			NavigationTypePreference?.SetEntryValues(
				entryValues: IUserInterfaceSettingsDroid.Options.NavigationTypes.AsEnumerable()
					.Select(type => type.AsStringTitle(Context) ?? type.ToString())
					.ToArray());

			HeaderScrollTypePreference?.SetEntries(
				entries: IUserInterfaceSettingsDroid.Options.HeaderScrollTypes.AsEnumerable()
					.Select(type => type.ToString())
					.ToArray());
			HeaderScrollTypePreference?.SetEntryValues(
				entryValues: IUserInterfaceSettingsDroid.Options.HeaderScrollTypes.AsEnumerable()
					.Select(type => type.AsStringTitle(Context) ?? type.ToString())
					.ToArray());

			PageDefaultPreference?.SetEntries(
				entries: IUserInterfaceSettingsDroid.Options.Pages.AsEnumerable()
					.Select(page => page.ToString())
					.ToArray());
			PageDefaultPreference?.SetEntryValues(
				entryValues: IUserInterfaceSettingsDroid.Options.Pages.AsEnumerable()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			PagesOrderedPreference?.SetEntries(
				entries: IUserInterfaceSettingsDroid.Options.Pages.AsEnumerable()
					.Select(page => page.ToString())
					.ToArray());
			PagesOrderedPreference?.SetEntryValues(
				entryValues: IUserInterfaceSettingsDroid.Options.Pages.AsEnumerable()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			AlbumOnCreatePreferences(savedInstanceState, rootKey);
			AlbumsOnCreatePreferences(savedInstanceState, rootKey);
			ArtistOnCreatePreferences(savedInstanceState, rootKey);
			ArtistsOnCreatePreferences(savedInstanceState, rootKey);
			GenreOnCreatePreferences(savedInstanceState, rootKey);
			GenresOnCreatePreferences(savedInstanceState, rootKey);
			PlaylistOnCreatePreferences(savedInstanceState, rootKey);
			PlaylistsOnCreatePreferences(savedInstanceState, rootKey);
			QueueOnCreatePreferences(savedInstanceState, rootKey);
			SearchOnCreatePreferences(savedInstanceState, rootKey);
			SongsOnCreatePreferences(savedInstanceState, rootKey);
		}
		public override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			base.OnPropertyChanged(propertyname);

			switch (propertyname)
			{
				case nameof(NavigationType) when
				NavigationTypePreference != null:
					if (IUserInterfaceSettingsDroid.Options.NavigationTypes.AsEnumerable().Index(NavigationType) is int navigationtypeindex)
						NavigationTypePreference.SetValueIndex(navigationtypeindex);
					break;

				case nameof(HeaderScrollType) when
				HeaderScrollTypePreference != null:
					if (IUserInterfaceSettingsDroid.Options.HeaderScrollTypes.AsEnumerable().Index(HeaderScrollType) is int headerscrolltypeindex)
						HeaderScrollTypePreference.SetValueIndex(headerscrolltypeindex);
					break;

				case nameof(PageDefault) when
				PageDefaultPreference != null:
					if (IUserInterfaceSettingsDroid.Options.Pages.AsEnumerable().Index(PageDefault) is int pagedefaultindex)
						PageDefaultPreference.SetValueIndex(pagedefaultindex);
					break;

				case nameof(PagesOrdered) when
				PagesOrderedPreference != null:
					PagesOrderedPreference.Values = PagesOrdered
						.Select(page => page.ToString())
						.ToList();
					break;

				case nameof(NowPlayingForceShow) when
				NowPlayingForceShowPreference != null &&
				NowPlayingForceShowPreference.Checked != NowPlayingForceShow:
					NowPlayingForceShowPreference.Checked = NowPlayingForceShow;
					break;

				default:
					AlbumOnPropertyChanged(propertyname);
					AlbumsOnPropertyChanged(propertyname);
					ArtistOnPropertyChanged(propertyname);
					ArtistsOnPropertyChanged(propertyname);
					GenreOnPropertyChanged(propertyname);
					GenresOnPropertyChanged(propertyname);
					PlaylistOnPropertyChanged(propertyname);
					PlaylistsOnPropertyChanged(propertyname);
					QueueOnPropertyChanged(propertyname);
					SearchOnPropertyChanged(propertyname);
					SongsOnPropertyChanged(propertyname);
					break;
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
					PagesOrdered = PagesOrderedPreference.Values?
						.Select(value =>
						{
							if (Enum.TryParse(value, out LibraryPages librarypage))
								return librarypage;

							return new LibraryPages?();

						}).OfType<LibraryPages>().ToList() ?? new List<LibraryPages> { };
					return true;

				case true when
				preference == NowPlayingForceShowPreference:
					NowPlayingForceShow = newvalue?.JavaCast<Java.Lang.Boolean>().BooleanValue() ?? false;
					return true;

				default: return
					AlbumOnPreferenceChange(preference, newvalue) ||
					AlbumsOnPreferenceChange(preference, newvalue) ||
					ArtistOnPreferenceChange(preference, newvalue) ||
					ArtistsOnPreferenceChange(preference, newvalue) ||
					GenreOnPreferenceChange(preference, newvalue) ||
					GenresOnPreferenceChange(preference, newvalue) ||
					PlaylistOnPreferenceChange(preference, newvalue) ||
					PlaylistsOnPreferenceChange(preference, newvalue) ||
					QueueOnPreferenceChange(preference, newvalue) ||
					SearchOnPreferenceChange(preference, newvalue) ||
					SongsOnPreferenceChange(preference, newvalue) || 
					result;
			}
		}
	}
}