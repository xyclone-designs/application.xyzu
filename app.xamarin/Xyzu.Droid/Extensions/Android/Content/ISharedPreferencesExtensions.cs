#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

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
			string? defaultietflanguagetag = defaultvalue.IetfLanguageTag;
			string? valueietflanguagetag = sharedpreferences.GetString(key, defaultietflanguagetag);

			if (valueietflanguagetag is null || valueietflanguagetag == defaultietflanguagetag)
				return defaultvalue;

			return CultureInfo.GetCultureInfoByIetfLanguageTag(valueietflanguagetag);
		}		   
		public static TEnum GetEnum<TEnum>(this ISharedPreferences sharedpreferences, string? key, TEnum defaultvalue) where TEnum : struct
		{
			string? defaultstring = defaultvalue.ToString();
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
		public static IVolumeControlSettings.IPresetable GetAudioVolumeControl(this ISharedPreferences sharedpreferences)
		{
			IVolumeControlSettings.IPresetable presetable = IVolumeControlSettings.IPresetable.Defaults.FromPreset(null, false);
			IEnumerable<IVolumeControlSettings.IPreset> allpresets = sharedpreferences.All?.Keys
				.Where(key => key.StartsWith(IVolumeControlSettings.IPreset.Keys.Base, StringComparison.OrdinalIgnoreCase))
				.Where(key => key.StartsWith(IVolumeControlSettings.IPresetable.Keys.Base, StringComparison.OrdinalIgnoreCase) is false)
				.Select(key => IVolumeControlSettings.IPreset.Keys.GetNameFromKey(key))
				.OfType<string>()
				.Distinct()
				.Select(presetname => sharedpreferences.GetAudioVolumeControl(presetname))
				.OfType<IVolumeControlSettings.IPreset>()
				.Distinct() ?? Enumerable.Empty<IVolumeControlSettings.IPreset>();

			presetable.IsEnabled = sharedpreferences.GetBoolean(IVolumeControlSettings.IPresetable.Keys.IsEnabled, IVolumeControlSettings.IPresetable.Defaults.IsEnabled);
			presetable.AllPresets = allpresets.Concat(IVolumeControlSettings.IPresetable.Defaults.Presets.AsEnumerable());
			presetable.CurrentPreset =
				sharedpreferences.GetString(IVolumeControlSettings.IPresetable.Keys.CurrentPreset, null) is string currentpresetname && presetable.AllPresets
				.FirstOrDefault(preset => string.Equals(preset.Name, currentpresetname, StringComparison.OrdinalIgnoreCase)) is IVolumeControlSettings.IPreset preset
					? preset
					: IVolumeControlSettings.IPresetable.Defaults.Presets.Default;

			return presetable;
		}											 									 						 
		public static IVolumeControlSettings.IPreset? GetAudioVolumeControl(this ISharedPreferences sharedpreferences, string presetname)
		{
			if (sharedpreferences.All is null)
				return null;

			IEnumerable<string> keys = sharedpreferences.All.Keys
				.Where(key => key.Contains(IVolumeControlSettings.IPreset.Keys.PresetName(presetname), StringComparison.OrdinalIgnoreCase));

			if (keys.Any() is false)
				return null;

			IVolumeControlSettings.IPreset preset = new IVolumeControlSettings.IPreset.Default(presetname);

			foreach (string key in keys)
				if (sharedpreferences.All.TryGetValue(key, out object? value))
					(preset ??= new IVolumeControlSettings.IPreset.Default(presetname))
						.SetFromKey(key, value);

			return preset;
		}											 									 
		public static IEnvironmentalReverbSettings.IPresetable GetAudioEnvironmentalReverb(this ISharedPreferences sharedpreferences)
		{
			IEnvironmentalReverbSettings.IPresetable presetable = IEnvironmentalReverbSettings.IPresetable.Defaults.FromPreset(null, false);
			IEnumerable<IEnvironmentalReverbSettings.IPreset> allpresets = sharedpreferences.All?.Keys
				.Where(key => key.StartsWith(IEnvironmentalReverbSettings.IPreset.Keys.Base, StringComparison.OrdinalIgnoreCase))
				.Where(key => key.StartsWith(IEnvironmentalReverbSettings.IPresetable.Keys.Base, StringComparison.OrdinalIgnoreCase) is false)
				.Select(key => IEnvironmentalReverbSettings.IPreset.Keys.GetNameFromKey(key))
				.OfType<string>()
				.Distinct()
				.Select(presetname => sharedpreferences.GetAudioEnvironmentalReverb(presetname))
				.OfType<IEnvironmentalReverbSettings.IPreset>()
				.Distinct() ?? Enumerable.Empty<IEnvironmentalReverbSettings.IPreset>();

			presetable.IsEnabled = sharedpreferences.GetBoolean(IEnvironmentalReverbSettings.IPresetable.Keys.IsEnabled, IEnvironmentalReverbSettings.IPresetable.Defaults.IsEnabled);
			presetable.AllPresets = allpresets.Concat(IEnvironmentalReverbSettings.IPresetable.Defaults.Presets.AsEnumerable());
			presetable.CurrentPreset =
				sharedpreferences.GetString(IEnvironmentalReverbSettings.IPresetable.Keys.CurrentPreset, null) is string currentpresetname && presetable.AllPresets
				.FirstOrDefault(preset => string.Equals(preset.Name, currentpresetname, StringComparison.OrdinalIgnoreCase)) is IEnvironmentalReverbSettings.IPreset preset
					? preset
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
				if (sharedpreferences.All.TryGetValue(key, out object? value))
					(preset ??= new IEnvironmentalReverbSettings.IPreset.Default(presetname))
						.SetFromKey(key, value);

			return preset;
		}
		public static IEqualiserSettings.IPresetable GetAudioEqualiser(this ISharedPreferences sharedpreferences)
		{
			IEqualiserSettings.IPresetable presetable = IEqualiserSettings.IPresetable.Defaults.FromPreset(null, false);
			IEnumerable<IEqualiserSettings.IPreset> allpresets = sharedpreferences.All?.Keys
				.Where(key => key.StartsWith(IEqualiserSettings.IPreset.Keys.Base, StringComparison.OrdinalIgnoreCase))
				.Where(key => key.StartsWith(IEqualiserSettings.IPresetable.Keys.Base, StringComparison.OrdinalIgnoreCase) is false)
				.Select(key => IEqualiserSettings.IPreset.Keys.GetNameFromKey(key))
				.OfType<string>()
				.Distinct()
				.Select(presetname => sharedpreferences.GetAudioEqualiser(presetname))
				.OfType<IEqualiserSettings.IPreset>()
				.Distinct() ?? Enumerable.Empty<IEqualiserSettings.IPreset>();

			presetable.IsEnabled = sharedpreferences.GetBoolean(IEqualiserSettings.IPresetable.Keys.IsEnabled, IEqualiserSettings.IPresetable.Defaults.IsEnabled);
			presetable.AllPresets = allpresets.Concat(IEqualiserSettings.IPresetable.Defaults.Presets.AsEnumerable());
			presetable.CurrentPreset =
				sharedpreferences.GetString(IEqualiserSettings.IPresetable.Keys.CurrentPreset, null) is string currentpresetname && presetable.AllPresets
				.FirstOrDefault(preset => string.Equals(preset.Name, currentpresetname, StringComparison.OrdinalIgnoreCase)) is IEqualiserSettings.IPreset preset
					? preset
					: IEqualiserSettings.IPresetable.Defaults.Presets.Default;

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
				if (sharedpreferences.All.TryGetValue(key, out object? value))
					(preset ??= new IEqualiserSettings.IPreset.Default(presetname))
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
				Directories = sharedpreferences.GetStringSet(IFilesSettingsDroid.Keys.Directories, IFilesSettingsDroid.Defaults.Directories.ToList()) ?? IFilesSettingsDroid.Defaults.Directories,
				DirectoriesExclude = sharedpreferences.GetStringSet(IFilesSettingsDroid.Keys.DirectoriesExclude, IFilesSettingsDroid.Defaults.DirectoriesExclude.ToList()) ?? IFilesSettingsDroid.Defaults.DirectoriesExclude,
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
			return new ISystemSettingsDroid.Default 
			{
				LanguageMode = sharedpreferences.GetEnum(ISystemSettings.Keys.LanguageMode, ISystemSettings.Defaults.LanguageMode),
				LanguageCurrent = sharedpreferences.GetCultureInfo(ISystemSettings.Keys.LanguageCurrent, ISystemSettings.Defaults.LanguageCurrent),
				ThemeMode = sharedpreferences.GetEnum(ISystemSettings.Keys.ThemeMode, ISystemSettings.Defaults.ThemeMode),
			};
		}				
		public static IUserInterfaceSettings GetUserInterface(this ISharedPreferences sharedpreferences)
		{
			IDictionary<LibraryPages, int> pagesordered = new Dictionary<LibraryPages, int>
			{
				{ IUserInterfaceSettings.Options.Pages.Albums, sharedpreferences.GetInt(IUserInterfaceSettings.Keys.PagesOrderedAlbums, IUserInterfaceSettings.Defaults.PagesOrderedAlbums) },
				{ IUserInterfaceSettings.Options.Pages.Artists, sharedpreferences.GetInt(IUserInterfaceSettings.Keys.PagesOrderedArtists, IUserInterfaceSettings.Defaults.PagesOrderedArtists) },
				{ IUserInterfaceSettings.Options.Pages.Genres, sharedpreferences.GetInt(IUserInterfaceSettings.Keys.PagesOrderedGenres, IUserInterfaceSettings.Defaults.PagesOrderedGenres) },
				{ IUserInterfaceSettings.Options.Pages.Playlists, sharedpreferences.GetInt(IUserInterfaceSettings.Keys.PagesOrderedPlaylists, IUserInterfaceSettings.Defaults.PagesOrderedPlaylists) },
				{ IUserInterfaceSettings.Options.Pages.Queue, sharedpreferences.GetInt(IUserInterfaceSettings.Keys.PagesOrderedQueue, IUserInterfaceSettings.Defaults.PagesOrderedQueue) },
				{ IUserInterfaceSettings.Options.Pages.Songs, sharedpreferences.GetInt(IUserInterfaceSettings.Keys.PagesOrderedSongs, IUserInterfaceSettings.Defaults.PagesOrderedSongs) },
			};

			return new IUserInterfaceSettings.Default
			{
				PageDefault = sharedpreferences.GetEnum(IUserInterfaceSettings.Keys.PageDefault, IUserInterfaceSettings.Defaults.PageDefault),
				PagesOrdered = pagesordered
					.Where(pageordered => pageordered.Value != -1)
					.OrderBy(pageordered => pageordered.Value)
					.Select(pageordered => pageordered.Key),

				Album = sharedpreferences.GetUserInterfaceAlbum(),
				Albums = sharedpreferences.GetUserInterfaceAlbums(),
				Artist = sharedpreferences.GetUserInterfaceArtist(),
				Artists = sharedpreferences.GetUserInterfaceArtists(),
				Genre = sharedpreferences.GetUserInterfaceGenre(),
				Genres = sharedpreferences.GetUserInterfaceGenres(),
				Playlist = sharedpreferences.GetUserInterfacePlaylist(),
				Playlists = sharedpreferences.GetUserInterfacePlaylists(),
				Queue = sharedpreferences.GetUserInterfaceQueue(),
				Search = sharedpreferences.GetUserInterfaceSearch(),
				Songs = sharedpreferences.GetUserInterfaceSongs(),
			};
		}
		public static IUserInterfaceSettingsDroid GetUserInterfaceDroid(this ISharedPreferences sharedpreferences)
		{
			IUserInterfaceSettings librarynavigationsettings = sharedpreferences.GetUserInterface();

			return new IUserInterfaceSettingsDroid.Default
			{
				HeaderScrollType = sharedpreferences.GetEnum(IUserInterfaceSettingsDroid.Keys.HeaderScrollType, IUserInterfaceSettingsDroid.Defaults.HeaderScrollType),
				NavigationType = sharedpreferences.GetEnum(IUserInterfaceSettingsDroid.Keys.NavigationType, IUserInterfaceSettingsDroid.Defaults.NavigationType),
				NowPlayingForceShow = sharedpreferences.GetBoolean(IUserInterfaceSettingsDroid.Keys.NowPlayingForceShow, IUserInterfaceSettingsDroid.Defaults.NowPlayingForceShow),

				PageDefault = librarynavigationsettings.PageDefault,
				PagesOrdered = librarynavigationsettings.PagesOrdered,

				Album = librarynavigationsettings.Album,
				Albums = librarynavigationsettings.Albums,
				Artist = librarynavigationsettings.Artist,
				Artists = librarynavigationsettings.Artists,
				Genre = librarynavigationsettings.Genre,
				Genres = librarynavigationsettings.Genres,
				Playlist = librarynavigationsettings.Playlist,
				Playlists = librarynavigationsettings.Playlists,
				Queue = librarynavigationsettings.Queue,
				Search = librarynavigationsettings.Search,
				Songs = librarynavigationsettings.Songs,
			};
		}  
		public static IAlbumSettings GetUserInterfaceAlbum(this ISharedPreferences sharedpreferences)
		{
			return new IAlbumSettings.Default
			{
				SongsIsReversed = sharedpreferences.GetBoolean(IAlbumSettings.Keys.SongsIsReversed, IAlbumSettings.Defaults.SongsIsReversed),
				SongsLayoutType = sharedpreferences.GetEnum(IAlbumSettings.Keys.SongsLayoutType, IAlbumSettings.Defaults.SongsLayoutType),
				SongsSortKey = sharedpreferences.GetEnum(IAlbumSettings.Keys.SongsSortKey, IAlbumSettings.Defaults.SongsSortKey),
			};
		}
		public static IAlbumsSettings GetUserInterfaceAlbums(this ISharedPreferences sharedpreferences)
		{
			return new IAlbumsSettings.Default
			{
				AlbumsIsReversed = sharedpreferences.GetBoolean(IAlbumsSettings.Keys.IsReversed, IAlbumsSettings.Defaults.IsReversed),
				AlbumsLayoutType = sharedpreferences.GetEnum(IAlbumsSettings.Keys.LayoutType, IAlbumsSettings.Defaults.LayoutType),
				AlbumsSortKey = sharedpreferences.GetEnum(IAlbumsSettings.Keys.SortKey, IAlbumsSettings.Defaults.SortKey),
			};
		}
		public static IArtistSettings GetUserInterfaceArtist(this ISharedPreferences sharedpreferences)
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
		public static IArtistsSettings GetUserInterfaceArtists(this ISharedPreferences sharedpreferences)
		{
			return new IArtistsSettings.Default
			{
				ArtistsIsReversed = sharedpreferences.GetBoolean(IArtistsSettings.Keys.IsReversed, IArtistsSettings.Defaults.IsReversed),
				ArtistsLayoutType = sharedpreferences.GetEnum(IArtistsSettings.Keys.LayoutType, IArtistsSettings.Defaults.LayoutType),
				ArtistsSortKey = sharedpreferences.GetEnum(IArtistsSettings.Keys.SortKey, IArtistsSettings.Defaults.SortKey),
			};
		}
		public static IGenreSettings GetUserInterfaceGenre(this ISharedPreferences sharedpreferences)
		{
			return new IGenreSettings.Default
			{
				SongsIsReversed = sharedpreferences.GetBoolean(IGenreSettings.Keys.SongsIsReversed, IGenreSettings.Defaults.SongsIsReversed),
				SongsLayoutType = sharedpreferences.GetEnum(IGenreSettings.Keys.SongsLayoutType, IGenreSettings.Defaults.SongsLayoutType),
				SongsSortKey = sharedpreferences.GetEnum(IGenreSettings.Keys.SongsSortKey, IGenreSettings.Defaults.SongsSortKey),
			};
		}
		public static IGenresSettings GetUserInterfaceGenres(this ISharedPreferences sharedpreferences)
		{
			return new IGenresSettings.Default
			{
				GenresIsReversed = sharedpreferences.GetBoolean(IGenresSettings.Keys.IsReversed, IGenresSettings.Defaults.IsReversed),
				GenresLayoutType = sharedpreferences.GetEnum(IGenresSettings.Keys.LayoutType, IGenresSettings.Defaults.LayoutType),
				GenresSortKey = sharedpreferences.GetEnum(IGenresSettings.Keys.SortKey, IGenresSettings.Defaults.SortKey),
			};
		}
		public static IPlaylistSettings GetUserInterfacePlaylist(this ISharedPreferences sharedpreferences)
		{
			return new IPlaylistSettings.Default
			{
				SongsIsReversed = sharedpreferences.GetBoolean(IPlaylistSettings.Keys.SongsIsReversed, IPlaylistSettings.Defaults.SongsIsReversed),
				SongsLayoutType = sharedpreferences.GetEnum(IPlaylistSettings.Keys.SongsLayoutType, IPlaylistSettings.Defaults.SongsLayoutType),
				SongsSortKey = sharedpreferences.GetEnum(IPlaylistSettings.Keys.SongsSortKey, IPlaylistSettings.Defaults.SongsSortKey),
			};
		}
		public static IPlaylistsSettings GetUserInterfacePlaylists(this ISharedPreferences sharedpreferences)
		{
			return new IPlaylistsSettings.Default
			{
				PlaylistsIsReversed = sharedpreferences.GetBoolean(IPlaylistsSettings.Keys.IsReversed, IPlaylistsSettings.Defaults.IsReversed),
				PlaylistsLayoutType = sharedpreferences.GetEnum(IPlaylistsSettings.Keys.LayoutType, IPlaylistsSettings.Defaults.LayoutType),
				PlaylistsSortKey = sharedpreferences.GetEnum(IPlaylistsSettings.Keys.SortKey, IPlaylistsSettings.Defaults.SortKey),
			};
		}
		public static IQueueSettings GetUserInterfaceQueue(this ISharedPreferences sharedpreferences)
		{
			return new IQueueSettings.Default
			{
				QueueLayoutType = sharedpreferences.GetEnum(IQueueSettings.Keys.LayoutType, IQueueSettings.Defaults.LayoutType),
			};
		}
		public static ISearchSettings GetUserInterfaceSearch(this ISharedPreferences sharedpreferences)
		{
			return new ISearchSettings.Default
			{
				SearchLayoutType = sharedpreferences.GetEnum(ISearchSettings.Keys.LayoutType, ISearchSettings.Defaults.LayoutType),
			};
		}
		public static ISongsSettings GetUserInterfaceSongs(this ISharedPreferences sharedpreferences)
		{
			return new ISongsSettings.Default
			{
				SongsIsReversed = sharedpreferences.GetBoolean(ISongsSettings.Keys.IsReversed, ISongsSettings.Defaults.IsReversed),
				SongsLayoutType = sharedpreferences.GetEnum(ISongsSettings.Keys.LayoutType, ISongsSettings.Defaults.LayoutType),
				SongsSortKey = sharedpreferences.GetEnum(ISongsSettings.Keys.SortKey, ISongsSettings.Defaults.SortKey),
			};
		}
		public static ISharedPreferences RemoveAudioVolumeControl(this ISharedPreferences sharedpreferences, string presetname)
		{
			if (sharedpreferences.All is null || sharedpreferences.Edit() is not ISharedPreferencesEditor sharedpreferenceseditor)
				return sharedpreferences;

			foreach (string key in sharedpreferences.All.Keys.Where(key =>
			{
				return key.Contains(IVolumeControlSettings.IPreset.Keys.PresetName(presetname), StringComparison.OrdinalIgnoreCase);

			})) sharedpreferenceseditor.Remove(key); sharedpreferenceseditor.Commit();

			return sharedpreferences;
		}
		public static ISharedPreferences RemoveAudioEnvironmentalReverb(this ISharedPreferences sharedpreferences, string presetname)
		{
			if (sharedpreferences.All is null || sharedpreferences.Edit() is not ISharedPreferencesEditor sharedpreferenceseditor)
				return sharedpreferences;

			foreach (string key in sharedpreferences.All.Keys.Where(key =>
			{
				return key.Contains(IEnvironmentalReverbSettings.IPreset.Keys.PresetName(presetname), StringComparison.OrdinalIgnoreCase);

			})) sharedpreferenceseditor.Remove(key); sharedpreferenceseditor.Commit();

			return sharedpreferences;
		}
		public static ISharedPreferences RemoveAudioEqualiser(this ISharedPreferences sharedpreferences, string presetname)
		{
			if (sharedpreferences.All is null || sharedpreferences.Edit() is not ISharedPreferencesEditor sharedpreferenceseditor)
				return sharedpreferences;

			foreach (string key in sharedpreferences.All.Keys.Where(key =>
			{
				return key.Contains(IEqualiserSettings.IPreset.Keys.PresetName(presetname), StringComparison.OrdinalIgnoreCase);

			})) sharedpreferenceseditor.Remove(key); sharedpreferenceseditor.Commit();

			return sharedpreferences;
		}
	}
}