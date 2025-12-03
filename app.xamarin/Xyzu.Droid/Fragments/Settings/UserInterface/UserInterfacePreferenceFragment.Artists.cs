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
			public const string Artists = "settings_userinterface_artists_dropdownpreference_key";
			public const string ArtistsIsReversed = "settings_userinterface_artists_isreversed_switchpreference_key";
			public const string ArtistsLayoutType = "settings_userinterface_artists_layouttype_listpreference_key";
			public const string ArtistsSortKey = "settings_userinterface_artists_sortkey_listpreference_key";
		}

		public IArtistsSettings? _Artists;

		public IArtistsSettings Artists
		{
			get => _Artists ??= XyzuSettings.Instance.GetUserInterfaceArtists();
			set => XyzuSettings.Instance
				.Edit()?
				.PutUserInterfaceArtists(_Artists = value)?
				.Apply();
		}
		public bool ArtistsIsReversed
		{
			get => Artists.ArtistsIsReversed;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutBoolean(IArtistsSettings.Keys.IsReversed, Artists.ArtistsIsReversed = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes ArtistsLayoutType
		{
			get => Artists.ArtistsLayoutType;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IArtistsSettings.Keys.LayoutType, Artists.ArtistsLayoutType = value)?
					.Apply();

				OnPropertyChanged();
			}
		}
		public ModelSortKeys ArtistsSortKey
		{
			get => Artists.ArtistsSortKey;
			set
			{
				XyzuSettings.Instance
					.Edit()?
					.PutEnum(IArtistsSettings.Keys.SortKey, Artists.ArtistsSortKey = value)?
					.Apply();

				OnPropertyChanged();
			}
		}

		public XyzuSwitchPreference? ArtistsIsReversedPreference { get; set; }
		public XyzuListPreference? ArtistsLayoutTypePreference { get; set; }
		public XyzuListPreference? ArtistsSortKeyPreference { get; set; }

		public void ArtistsOnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_userinterface_artists_title);

			AddPreferenceChangeHandler(
				ArtistsIsReversedPreference,
				ArtistsLayoutTypePreference,
				ArtistsSortKeyPreference);

			OnPropertyChanged(nameof(ArtistsIsReversed));
			OnPropertyChanged(nameof(ArtistsLayoutType));
			OnPropertyChanged(nameof(ArtistsSortKey));
		}
		public void ArtistsOnPause()
		{
			base.OnPause();

			RemovePreferenceChangeHandler(
				ArtistsIsReversedPreference,
				ArtistsLayoutTypePreference,
				ArtistsSortKeyPreference);
		}
		public void ArtistsOnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			InitPreferences(
				ArtistsIsReversedPreference = FindPreference(Keys.ArtistsIsReversed) as XyzuSwitchPreference,
				ArtistsLayoutTypePreference = FindPreference(Keys.ArtistsLayoutType) as XyzuListPreference,
				ArtistsSortKeyPreference = FindPreference(Keys.ArtistsSortKey) as XyzuListPreference);

			ArtistsLayoutTypePreference?.SetEntries(
				entries: IArtistsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.ToString())
					.ToArray());
			ArtistsLayoutTypePreference?.SetEntryValues(
				entryValues: IArtistsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered()
					.Select(layouttype => layouttype.AsStringTitle(Context) ?? layouttype.ToString())
					.ToArray());

			ArtistsSortKeyPreference?.SetEntryValues(
				entryValues: IArtistsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.AsStringTitle(Context) ?? sortkey.ToString())
					.ToArray());
			ArtistsSortKeyPreference?.SetEntries(
				entries: IArtistsSettings.Options.SortKeys.AsEnumerable()
					.Select(sortkey => sortkey.ToString())
					.ToArray());
		}
		public void ArtistsOnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(ArtistsIsReversed) when
				ArtistsIsReversedPreference != null &&
				ArtistsIsReversedPreference.Checked != ArtistsIsReversed:
					ArtistsIsReversedPreference.Checked = ArtistsIsReversed;
					break;

				case nameof(ArtistsLayoutType) when
				ArtistsLayoutTypePreference != null:
					if (IArtistsSettings.Options.LayoutTypes.AsEnumerable().NeatlyOrdered().Index(ArtistsLayoutType) is int layouttypeindex)
						ArtistsLayoutTypePreference.SetValueIndex(layouttypeindex);
					break;

				case nameof(ArtistsSortKey) when
				ArtistsSortKeyPreference != null:
					if (IArtistsSettings.Options.SortKeys.AsEnumerable().Index(ArtistsSortKey) is int sortkeyindex)
						ArtistsSortKeyPreference.SetValueIndex(sortkeyindex);
					break;

				default: break;
			}
		}
		public bool ArtistsOnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newvalue)
		{
			bool result = base.OnPreferenceChange(preference, newvalue);

			if (newvalue is null)
				return result;

			switch (true)
			{
				case true when
				preference == ArtistsIsReversedPreference:
					ArtistsIsReversed = newvalue.JavaCast<Java.Lang.Boolean>().BooleanValue();
					return true;

				case true when
				preference == ArtistsLayoutTypePreference &&
				Enum.TryParse(newvalue.ToString(), out LibraryLayoutTypes librarylayouttype) &&
				ArtistsLayoutType != librarylayouttype:
					ArtistsLayoutType = librarylayouttype;
					return true;

				case true when
				preference == ArtistsSortKeyPreference &&
				Enum.TryParse(newvalue.ToString(), out ModelSortKeys modelsortkey) &&
				ArtistsSortKey != modelsortkey:
					ArtistsSortKey = modelsortkey;
					return true;

				default: return result;
			}
		}
	}
}