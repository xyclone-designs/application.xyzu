#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

using Xyzu.Library;
using Xyzu.Library.Enums;
using Xyzu.Settings;
using Xyzu.Settings.Enums;
using Xyzu.Settings.About;
using Xyzu.Settings.Audio;
using Xyzu.Settings.Files;
using Xyzu.Settings.LockScreen;
using Xyzu.Settings.Notification;
using Xyzu.Settings.System;
using Xyzu.Settings.UserInterface;
using Xyzu.Settings.UserInterface.Library;

namespace Android.Content
{
	public static class ISharedPreferencesExtensions
	{
		public static Color GetColor(this ISharedPreferences sharedpreferences, string? key, Color defaultvalue)
		{
			int defaultArgb = defaultvalue.ToArgb();
			int valueArgb = sharedpreferences.GetInt(key, defaultArgb);

			if (valueArgb == defaultArgb)
				return defaultvalue;

			return Color.FromArgb(valueArgb);
		}		   				  
		public static CultureInfo GetCultureInfo(this ISharedPreferences sharedpreferences, string? key, CultureInfo defaultvalue)
		{
			string defaultietflanguagetag = defaultvalue.IetfLanguageTag;
			string? valueietflanguagetag = sharedpreferences.GetString(key, defaultietflanguagetag);

			if (valueietflanguagetag == defaultietflanguagetag)
				return defaultvalue;

			return CultureInfo.GetCultureInfoByIetfLanguageTag(valueietflanguagetag);
		}		   
		public static TEnum GetEnum<TEnum>(this ISharedPreferences sharedpreferences, string? key, TEnum defaultvalue) where TEnum : struct
		{
			string defaultstring = defaultvalue.ToString();
			string? valuestring = sharedpreferences.GetString(key, defaultstring);

			if (string.Equals(defaultstring, valuestring, StringComparison.OrdinalIgnoreCase))
				return defaultvalue;

			if (Enum.TryParse(valuestring, true, out TEnum value))
				return value;

			return defaultvalue;
		}				 	 					
		public static IEnumerable<TEnum>? GetEnumSet<TEnum>(this ISharedPreferences sharedpreferences, string? key, IEnumerable<TEnum>? defaultvalue) where TEnum : struct
		{
			if (sharedpreferences.GetString(key, null)?.Split(".") is IEnumerable<string> stringset)
			{
				foreach (string str in stringset)
					if (Enum.TryParse(str, true, out TEnum value))
						yield return value;
			}
			else if (defaultvalue != null)
				foreach (TEnum @enum in defaultvalue)
					yield return @enum;
		}				 	 
		public static short GetShort(this ISharedPreferences sharedpreferences, string? key, short defaultvalue)
		{
			int value = sharedpreferences.GetInt(key, defaultvalue);

			if (value == defaultvalue)
				return defaultvalue;

			if (value >= short.MinValue && value <= short.MaxValue)
				return short.Parse(value.ToString());

			return defaultvalue;
		}			   				   
		public static short[] GetShortArray(this ISharedPreferences sharedpreferences, string? key, short[] defaultvalue)
		{
			short[] value = sharedpreferences.GetStringSet(key, null)?
				.Select(str => short.TryParse(str, out short shortvalue) ? shortvalue : new short?())
				.OfType<short>()
				.ToArray() ?? defaultvalue;

			return value;
		}			   

		public static ISettings GetSettings(this ISharedPreferences sharedpreferences)
		{
			return new ISettings.Default { };
		}			  
		public static IAboutSettings GetAbout(this ISharedPreferences sharedpreferences)
		{
			return new IAboutSettings.Default { };
		}
		public static IAudioSettings GetAudio(this ISharedPreferences sharedpreferences)
		{
			return new IAudioSettings.Default { };
		}											 
		public static IBassBoostSettings.IPresetable GetAudioBassBoost(this ISharedPreferences sharedpreferences)
		{
			IBassBoostSettings.IPresetable presetable = IBassBoostSettings.IPresetable.Defaults.FromPreset(null, false);

			presetable.IsEnabled = sharedpreferences.GetBoolean(IBassBoostSettings.IPresetable.Keys.IsEnabled, IBassBoostSettings.IPresetable.Defaults.IsEnabled);
			presetable.AllPresets = sharedpreferences.All is null
				? IBassBoostSettings.IPresetable.Defaults.Presets.AsEnumerable()
				: sharedpreferences.All.Keys
					.Where(key => key.StartsWith(IBassBoostSettings.IPreset.Keys.Base, StringComparison.OrdinalIgnoreCase))
					.Where(key => key.StartsWith(IBassBoostSettings.IPresetable.Keys.Base, StringComparison.OrdinalIgnoreCase) is false)
					.Select(key => IBassBoostSettings.IPreset.Keys.GetNameFromKey(key))
					.OfType<string>()
					.Distinct()
					.Select(presetname => sharedpreferences.GetAudioBassBoost(presetname))
					.OfType<IBassBoostSettings.IPreset>()
					.Concat(IBassBoostSettings.IPresetable.Defaults.Presets.AsEnumerable())
					.Distinct();
			presetable.CurrentPreset = sharedpreferences.GetString(IBassBoostSettings.IPresetable.Keys.CurrentPreset, null) is string currentpresetname
				? presetable.AllPresets.FirstOrDefault(preset => string.Equals(preset.Name, currentpresetname, StringComparison.OrdinalIgnoreCase))
				: IBassBoostSettings.IPresetable.Defaults.Presets.Default;

			return presetable;
		}											 									 						 
		public static IBassBoostSettings.IPreset? GetAudioBassBoost(this ISharedPreferences sharedpreferences, string presetname)
		{
			if (sharedpreferences.All is null)
				return null;

			IEnumerable<string> keys = sharedpreferences.All.Keys
				.Where(key => key.Contains(IBassBoostSettings.IPreset.Keys.PresetName(presetname), StringComparison.OrdinalIgnoreCase));

			if (keys.Any() is false)
				return null;

			IBassBoostSettings.IPreset preset = new IBassBoostSettings.IPreset.Default(presetname);

			foreach (string key in keys)
				if (sharedpreferences.All.TryGetValue(key, out object value))
					(preset ??= new IBassBoostSettings.IPreset.Default(presetname))
						.SetFromKey(key, value);

			return preset;
		}											 									 
		public static IEnvironmentalReverbSettings.IPresetable GetAudioEnvironmentalReverb(this ISharedPreferences sharedpreferences)
		{
			IEnvironmentalReverbSettings.IPresetable presetable = IEnvironmentalReverbSettings.IPresetable.Defaults.FromPreset(null, false);

			presetable.IsEnabled = sharedpreferences.GetBoolean(IEnvironmentalReverbSettings.IPresetable.Keys.IsEnabled, IEnvironmentalReverbSettings.IPresetable.Defaults.IsEnabled);
			presetable.AllPresets = sharedpreferences.All is null
				? IEnvironmentalReverbSettings.IPresetable.Defaults.Presets.AsEnumerable()
				: sharedpreferences.All.Keys
					.Where(key => key.StartsWith(IEnvironmentalReverbSettings.IPreset.Keys.Base, StringComparison.OrdinalIgnoreCase))
					.Where(key => key.StartsWith(IEnvironmentalReverbSettings.IPresetable.Keys.Base, StringComparison.OrdinalIgnoreCase) is false)
					.Select(key => IEnvironmentalReverbSettings.IPreset.Keys.GetNameFromKey(key))
					.OfType<string>()
					.Distinct()
					.Select(presetname => sharedpreferences.GetAudioEnvironmentalReverb(presetname))
					.OfType<IEnvironmentalReverbSettings.IPreset>()
					.Concat(IEnvironmentalReverbSettings.IPresetable.Defaults.Presets.AsEnumerable())
					.Distinct();
			presetable.CurrentPreset = sharedpreferences.GetString(IEnvironmentalReverbSettings.IPresetable.Keys.CurrentPreset, null) is string currentpresetname
				? presetable.AllPresets.FirstOrDefault(preset => string.Equals(preset.Name, currentpresetname, StringComparison.OrdinalIgnoreCase))
				: IEnvironmentalReverbSettings.IPresetable.Defaults.Presets.Default;

			return presetable;
		}
		public static IEnvironmentalReverbSettings.IPreset? GetAudioEnvironmentalReverb(this ISharedPreferences sharedpreferences, string presetname)
		{
			if (sharedpreferences.All is null)
				return null;

			IEnumerable<string> keys = sharedpreferences.All.Keys
					.Where(key => key.Contains(IEnvironmentalReverbSettings.IPreset.Keys.PresetName(presetname), StringComparison.OrdinalIgnoreCase));

			if (keys.Any() is false)
				return null;

			IEnvironmentalReverbSettings.IPreset preset = new IEnvironmentalReverbSettings.IPreset.Default(presetname);

			foreach (string key in keys)
				if (sharedpreferences.All.TryGetValue(key, out object value))
					(preset ??= new IEnvironmentalReverbSettings.IPreset.Default(presetname))
						.SetFromKey(key, value);

			return preset;
		}
		public static IEqualiserSettings.IPresetable GetAudioEqualiser(this ISharedPreferences sharedpreferences)
		{
			IEqualiserSettings.IPresetable presetable = IEqualiserSettings.IPresetable.Defaults.FromPreset(null, IEqualiserSettings.BandType.Ten, false);

			presetable.IsEnabled = sharedpreferences.GetBoolean(IEqualiserSettings.IPresetable.Keys.IsEnabled, IEqualiserSettings.IPresetable.Defaults.IsEnabled);
			presetable.AllPresets = sharedpreferences.All is null
				? IEqualiserSettings.IPresetable.Defaults.Presets.TenBand.AsEnumerable()
				: sharedpreferences.All.Keys
					.Where(key => key.StartsWith(IEqualiserSettings.IPreset.Keys.Base, StringComparison.OrdinalIgnoreCase))
					.Where(key => key.StartsWith(IEqualiserSettings.IPresetable.Keys.Base, StringComparison.OrdinalIgnoreCase) is false)
					.Select(key => IEqualiserSettings.IPreset.Keys.GetNameFromKey(key))
					.OfType<string>()
					.Distinct()
					.Select(presetname => sharedpreferences.GetAudioEqualiser(presetname))
					.OfType<IEqualiserSettings.IPreset>()
					.Concat(IEqualiserSettings.IPresetable.Defaults.Presets.TenBand.AsEnumerable())
					.Distinct();
			presetable.CurrentPreset = sharedpreferences.GetString(IEqualiserSettings.IPresetable.Keys.CurrentPreset, null) is string currentpresetname
				? presetable.AllPresets.FirstOrDefault(preset => string.Equals(preset.Name, currentpresetname, StringComparison.OrdinalIgnoreCase))
				: IEqualiserSettings.IPresetable.Defaults.Presets.TenBand.Default;

			return presetable;
		}
		public static IEqualiserSettings.IPreset? GetAudioEqualiser(this ISharedPreferences sharedpreferences, string presetname)
		{
			if (sharedpreferences.All is null)
				return null;

			IEnumerable<string> keys = sharedpreferences.All.Keys
					.Where(key => key.Contains(IEqualiserSettings.IPreset.Keys.PresetName(presetname), StringComparison.OrdinalIgnoreCase));

			if (keys.Any() is false)
				return null;

			IEqualiserSettings.IPreset preset = new IEqualiserSettings.IPreset.Default(presetname);

			foreach (string key in keys)
				if (sharedpreferences.All.TryGetValue(key, out object value))
					(preset ??= new IEqualiserSettings.IPreset.Default(presetname))
						.SetFromKey(key, value);

			return preset;
		}
		public static ILoudnessEnhancerSettings.IPresetable GetAudioLoudnessEnhancer(this ISharedPreferences sharedpreferences)
		{
			ILoudnessEnhancerSettings.IPresetable presetable = ILoudnessEnhancerSettings.IPresetable.Defaults.FromPreset(null, false);

			presetable.IsEnabled = sharedpreferences.GetBoolean(ILoudnessEnhancerSettings.IPresetable.Keys.IsEnabled, ILoudnessEnhancerSettings.IPresetable.Defaults.IsEnabled);
			presetable.AllPresets = sharedpreferences.All is null
				? ILoudnessEnhancerSettings.IPresetable.Defaults.Presets.AsEnumerable()
				: sharedpreferences.All.Keys
					.Where(key => key.StartsWith(ILoudnessEnhancerSettings.IPreset.Keys.Base, StringComparison.OrdinalIgnoreCase))
					.Where(key => key.StartsWith(ILoudnessEnhancerSettings.IPresetable.Keys.Base, StringComparison.OrdinalIgnoreCase) is false)
					.Select(key => ILoudnessEnhancerSettings.IPreset.Keys.GetNameFromKey(key))
					.OfType<string>()
					.Distinct()
					.Select(presetname => sharedpreferences.GetAudioLoudnessEnhancer(presetname))
					.OfType<ILoudnessEnhancerSettings.IPreset>()
					.Concat(ILoudnessEnhancerSettings.IPresetable.Defaults.Presets.AsEnumerable())
					.Distinct();
			presetable.CurrentPreset = sharedpreferences.GetString(ILoudnessEnhancerSettings.IPresetable.Keys.CurrentPreset, null) is string currentpresetname
				? presetable.AllPresets.FirstOrDefault(preset => string.Equals(preset.Name, currentpresetname, StringComparison.OrdinalIgnoreCase))
				: ILoudnessEnhancerSettings.IPresetable.Defaults.Presets.Default;

			return presetable;
		}
		public static ILoudnessEnhancerSettings.IPreset? GetAudioLoudnessEnhancer(this ISharedPreferences sharedpreferences, string presetname)
		{
			if (sharedpreferences.All is null)
				return null;

			IEnumerable<string> keys = sharedpreferences.All.Keys
					.Where(key => key.Contains(ILoudnessEnhancerSettings.IPreset.Keys.PresetName(presetname), StringComparison.OrdinalIgnoreCase));

			if (keys.Any() is false)
				return null;

			ILoudnessEnhancerSettings.IPreset preset = new ILoudnessEnhancerSettings.IPreset.Default(presetname);

			foreach (string key in keys)
				if (sharedpreferences.All.TryGetValue(key, out object value))
					(preset ??= new ILoudnessEnhancerSettings.IPreset.Default(presetname))
						.SetFromKey(key, value);

			return preset;
		}
		public static IFilesSettings GetFiles(this ISharedPreferences sharedpreferences)
		{
			return new IFilesSettings.Default
			{
				Directories = sharedpreferences.GetStringSet(IFilesSettings.Keys.Directories, null) ?? IFilesSettings.Defaults.Directories,
				Mimetypes = sharedpreferences.GetEnumSet<MimeTypes>(IFilesSettings.Keys.Mimetypes, IFilesSettings.Defaults.Mimetypes) ?? IFilesSettings.Defaults.Mimetypes,
				TrackLengthIgnore = sharedpreferences.GetInt(IFilesSettings.Keys.TrackLengthIgnore, IFilesSettings.Defaults.TrackLengthIgnore),
			};
		}																  
		public static IFilesSettingsDroid GetFilesDroid(this ISharedPreferences sharedpreferences)
		{
			return new IFilesSettingsDroid.Default
			{
				Directories = sharedpreferences.GetStringSet(IFilesSettingsDroid.Keys.Directories, null) ?? IFilesSettingsDroid.Defaults.Directories,
				Mimetypes = sharedpreferences.GetEnumSet<MimeTypes>(IFilesSettingsDroid.Keys.Mimetypes, IFilesSettingsDroid.Defaults.Mimetypes) ?? IFilesSettingsDroid.Defaults.Mimetypes,
				TrackLengthIgnore = sharedpreferences.GetInt(IFilesSettingsDroid.Keys.TrackLengthIgnore, IFilesSettingsDroid.Defaults.TrackLengthIgnore),
			};
		}
		public static ILockScreenSettings GetLockScreen(this ISharedPreferences sharedpreferences)
		{
			return new ILockScreenSettings.Default { };
		}
		public static INotificationSettings GetNotification(this ISharedPreferences sharedpreferences)
		{
			return new INotificationSettings.Default { };
		}											   			 					
		public static INotificationSettingsDroid GetNotificationDroid(this ISharedPreferences sharedpreferences)
		{
			return new INotificationSettingsDroid.Default
			{
				BadgeIconType = sharedpreferences.GetEnum(INotificationSettingsDroid.Keys.BadgeIconType, INotificationSettingsDroid.Defaults.BadgeIconType),
				CustomColour = sharedpreferences.GetColor(INotificationSettingsDroid.Keys.CustomColour, INotificationSettingsDroid.Defaults.CustomColour),
				IsColourised = sharedpreferences.GetBoolean(INotificationSettingsDroid.Keys.IsColourised, INotificationSettingsDroid.Defaults.IsColourised),
				Priority = sharedpreferences.GetEnum(INotificationSettingsDroid.Keys.Priority, INotificationSettingsDroid.Defaults.Priority),
				UseCustomColour = sharedpreferences.GetBoolean(INotificationSettingsDroid.Keys.UseCustomColour, INotificationSettingsDroid.Defaults.UseCustomColour),
			};
		}											   			 
		public static ISystemSettings GetSystem(this ISharedPreferences sharedpreferences)
		{
			return sharedpreferences.GetSystemDroid();
		}												 
		public static ISystemSettingsDroid GetSystemDroid(this ISharedPreferences sharedpreferences)
		{
			return new ISystemSettingsDroid.Default { };
		}				
		public static IUserInterfaceSettings GetUserInterface(this ISharedPreferences sharedpreferences)
		{
			return new IUserInterfaceSettings.Default { };
		}	 
		public static ILanguagesSettings GetUserInterfaceLanguages(this ISharedPreferences sharedpreferences)
		{
			return new ILanguagesSettings.Default 
			{
				Mode = sharedpreferences.GetEnum(ILanguagesSettings.Keys.Mode, ILanguagesSettings.Defaults.Mode),
				CurrentLanguage = sharedpreferences.GetCultureInfo(ILanguagesSettings.Keys.CurrentLanguage, ILanguagesSettings.Defaults.CurrentLanguage),
			};
		}
		public static ILibrarySettings GetUserInterfaceLibrary(this ISharedPreferences sharedpreferences)
		{
			IDictionary<LibraryPages, int> pagesordered = new Dictionary<LibraryPages, int>
			{
				{ ILibrarySettings.Options.Pages.Albums, sharedpreferences.GetInt(ILibrarySettings.Keys.PagesOrderedAlbums, ILibrarySettings.Defaults.PagesOrderedAlbums) },
				{ ILibrarySettings.Options.Pages.Artists, sharedpreferences.GetInt(ILibrarySettings.Keys.PagesOrderedArtists, ILibrarySettings.Defaults.PagesOrderedArtists) },
				{ ILibrarySettings.Options.Pages.Genres, sharedpreferences.GetInt(ILibrarySettings.Keys.PagesOrderedGenres, ILibrarySettings.Defaults.PagesOrderedGenres) },
				{ ILibrarySettings.Options.Pages.Playlists, sharedpreferences.GetInt(ILibrarySettings.Keys.PagesOrderedPlaylists, ILibrarySettings.Defaults.PagesOrderedPlaylists) },
				{ ILibrarySettings.Options.Pages.Queue, sharedpreferences.GetInt(ILibrarySettings.Keys.PagesOrderedQueue, ILibrarySettings.Defaults.PagesOrderedQueue) },
				{ ILibrarySettings.Options.Pages.Songs, sharedpreferences.GetInt(ILibrarySettings.Keys.PagesOrderedSongs, ILibrarySettings.Defaults.PagesOrderedSongs) },
			};

			return new ILibrarySettings.Default
			{
				PageDefault = sharedpreferences.GetEnum(ILibrarySettings.Keys.PageDefault, ILibrarySettings.Defaults.PageDefault),
				PagesOrdered = pagesordered
					.Where(pageordered => pageordered.Value != -1)
					.OrderBy(pageordered => pageordered.Value)
					.Select(pageordered => pageordered.Key),
			};
		}
		public static ILibrarySettingsDroid GetUserInterfaceLibraryDroid(this ISharedPreferences sharedpreferences)
		{
			ILibrarySettings librarynavigationsettings = sharedpreferences.GetUserInterfaceLibrary();

			return new ILibrarySettingsDroid.Default
			{
				HeaderScrollType = sharedpreferences.GetEnum(ILibrarySettingsDroid.Keys.HeaderScrollType, ILibrarySettingsDroid.Defaults.HeaderScrollType),
				NavigationType = sharedpreferences.GetEnum(ILibrarySettingsDroid.Keys.NavigationType, ILibrarySettingsDroid.Defaults.NavigationType),

				PageDefault = librarynavigationsettings.PageDefault,
				PagesOrdered = librarynavigationsettings.PagesOrdered,
			};
		}  
		public static IAlbumSettings GetUserInterfaceLibraryAlbum(this ISharedPreferences sharedpreferences)
		{
			return new IAlbumSettings.Default
			{
				SongsIsReversed = sharedpreferences.GetBoolean(IAlbumSettings.Keys.SongsIsReversed, IAlbumSettings.Defaults.SongsIsReversed),
				SongsLayoutType = sharedpreferences.GetEnum(IAlbumSettings.Keys.SongsLayoutType, IAlbumSettings.Defaults.SongsLayoutType),
				SongsSortKey = sharedpreferences.GetEnum(IAlbumSettings.Keys.SongsSortKey, IAlbumSettings.Defaults.SongsSortKey),
			};
		}
		public static IAlbumsSettings GetUserInterfaceLibraryAlbums(this ISharedPreferences sharedpreferences)
		{
			return new IAlbumsSettings.Default
			{
				IsReversed = sharedpreferences.GetBoolean(IAlbumsSettings.Keys.IsReversed, IAlbumsSettings.Defaults.IsReversed),
				LayoutType = sharedpreferences.GetEnum(IAlbumsSettings.Keys.LayoutType, IAlbumsSettings.Defaults.LayoutType),
				SortKey = sharedpreferences.GetEnum(IAlbumsSettings.Keys.SortKey, IAlbumsSettings.Defaults.SortKey),
			};
		}
		public static IArtistSettings GetUserInterfaceLibraryArtist(this ISharedPreferences sharedpreferences)
		{
			return new IArtistSettings.Default
			{
				AlbumsIsReversed = sharedpreferences.GetBoolean(IArtistSettings.Keys.AlbumsIsReversed, IArtistSettings.Defaults.AlbumsIsReversed),
				AlbumsLayoutType = sharedpreferences.GetEnum(IArtistSettings.Keys.AlbumsLayoutType, IArtistSettings.Defaults.AlbumsLayoutType),
				AlbumsSortKey = sharedpreferences.GetEnum(IArtistSettings.Keys.AlbumsSortKey, IArtistSettings.Defaults.AlbumsSortKey),
				SongsIsReversed = sharedpreferences.GetBoolean(IArtistSettings.Keys.SongsIsReversed, IArtistSettings.Defaults.SongsIsReversed),
				SongsLayoutType = sharedpreferences.GetEnum(IArtistSettings.Keys.SongsLayoutType, IArtistSettings.Defaults.SongsLayoutType),
				SongsSortKey = sharedpreferences.GetEnum(IArtistSettings.Keys.SongsSortKey, IArtistSettings.Defaults.SongsSortKey),
			};
		}
		public static IArtistsSettings GetUserInterfaceLibraryArtists(this ISharedPreferences sharedpreferences)
		{
			return new IArtistsSettings.Default
			{
				IsReversed = sharedpreferences.GetBoolean(IArtistsSettings.Keys.IsReversed, IArtistsSettings.Defaults.IsReversed),
				LayoutType = sharedpreferences.GetEnum(IArtistsSettings.Keys.LayoutType, IArtistsSettings.Defaults.LayoutType),
				SortKey = sharedpreferences.GetEnum(IArtistsSettings.Keys.SortKey, IArtistsSettings.Defaults.SortKey),
			};
		}
		public static IGenreSettings GetUserInterfaceLibraryGenre(this ISharedPreferences sharedpreferences)
		{
			return new IGenreSettings.Default
			{
				SongsIsReversed = sharedpreferences.GetBoolean(IGenreSettings.Keys.SongsIsReversed, IGenreSettings.Defaults.SongsIsReversed),
				SongsLayoutType = sharedpreferences.GetEnum(IGenreSettings.Keys.SongsLayoutType, IGenreSettings.Defaults.SongsLayoutType),
				SongsSortKey = sharedpreferences.GetEnum(IGenreSettings.Keys.SongsSortKey, IGenreSettings.Defaults.SongsSortKey),
			};
		}
		public static IGenresSettings GetUserInterfaceLibraryGenres(this ISharedPreferences sharedpreferences)
		{
			return new IGenresSettings.Default
			{
				IsReversed = sharedpreferences.GetBoolean(IGenresSettings.Keys.IsReversed, IGenresSettings.Defaults.IsReversed),
				LayoutType = sharedpreferences.GetEnum(IGenresSettings.Keys.LayoutType, IGenresSettings.Defaults.LayoutType),
				SortKey = sharedpreferences.GetEnum(IGenresSettings.Keys.SortKey, IGenresSettings.Defaults.SortKey),
			};
		}
		public static IPlaylistSettings GetUserInterfaceLibraryPlaylist(this ISharedPreferences sharedpreferences)
		{
			return new IPlaylistSettings.Default
			{
				SongsIsReversed = sharedpreferences.GetBoolean(IPlaylistSettings.Keys.SongsIsReversed, IPlaylistSettings.Defaults.SongsIsReversed),
				SongsLayoutType = sharedpreferences.GetEnum(IPlaylistSettings.Keys.SongsLayoutType, IPlaylistSettings.Defaults.SongsLayoutType),
				SongsSortKey = sharedpreferences.GetEnum(IPlaylistSettings.Keys.SongsSortKey, IPlaylistSettings.Defaults.SongsSortKey),
			};
		}
		public static IPlaylistsSettings GetUserInterfaceLibraryPlaylists(this ISharedPreferences sharedpreferences)
		{
			return new IPlaylistsSettings.Default
			{
				IsReversed = sharedpreferences.GetBoolean(IPlaylistsSettings.Keys.IsReversed, IPlaylistsSettings.Defaults.IsReversed),
				LayoutType = sharedpreferences.GetEnum(IPlaylistsSettings.Keys.LayoutType, IPlaylistsSettings.Defaults.LayoutType),
				SortKey = sharedpreferences.GetEnum(IPlaylistsSettings.Keys.SortKey, IPlaylistsSettings.Defaults.SortKey),
			};
		}
		public static IQueueSettings GetUserInterfaceLibraryQueue(this ISharedPreferences sharedpreferences)
		{
			return new IQueueSettings.Default
			{
				LayoutType = sharedpreferences.GetEnum(IQueueSettings.Keys.LayoutType, IQueueSettings.Defaults.LayoutType),
			};
		}
		public static ISearchSettings GetUserInterfaceLibrarySearch(this ISharedPreferences sharedpreferences)
		{
			return new ISearchSettings.Default
			{
				LayoutType = sharedpreferences.GetEnum(ISearchSettings.Keys.LayoutType, ISearchSettings.Defaults.LayoutType),
			};
		}
		public static ISongsSettings GetUserInterfaceLibrarySongs(this ISharedPreferences sharedpreferences)
		{
			return new ISongsSettings.Default
			{
				IsReversed = sharedpreferences.GetBoolean(ISongsSettings.Keys.IsReversed, ISongsSettings.Defaults.IsReversed),
				LayoutType = sharedpreferences.GetEnum(ISongsSettings.Keys.LayoutType, ISongsSettings.Defaults.LayoutType),
				SortKey = sharedpreferences.GetEnum(ISongsSettings.Keys.SortKey, ISongsSettings.Defaults.SortKey),
			};
		}
		public static INowPlayingSettings GetUserInterfaceNowPlaying(this ISharedPreferences sharedpreferences)
		{
			return new INowPlayingSettings.Default { };
		}	 				
		public static INowPlayingSettingsDroid GetUserInterfaceNowPlayingDroid(this ISharedPreferences sharedpreferences)
		{
			return new INowPlayingSettingsDroid.Default 
			{
				ForceShowNowPlaying = sharedpreferences.GetBoolean(INowPlayingSettingsDroid.Keys.ForceShowNowPlaying, INowPlayingSettingsDroid.Defaults.ForceShowNowPlaying),
			};
		}	 
		public static IThemesSettings GetUserInterfaceThemes(this ISharedPreferences sharedpreferences)
		{
			return new IThemesSettings.Default 
			{
				Mode = sharedpreferences.GetEnum(IThemesSettings.Keys.Mode, IThemesSettings.Defaults.Mode),
			};
		}
	}
}