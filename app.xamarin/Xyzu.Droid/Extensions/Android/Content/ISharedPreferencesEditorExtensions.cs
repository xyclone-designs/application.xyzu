﻿#nullable enable

using Android.Media.Audiofx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

using Xyzu.Settings;
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
	public static class ISharedPreferencesEditorExtensions
	{
		public static ISharedPreferencesEditor? PutColor(this ISharedPreferencesEditor sharedpreferenceseditor, string? key, Color value)
		{
			return sharedpreferenceseditor.PutInt(key, value.ToArgb());
		}			  
		public static ISharedPreferencesEditor? PutCultureInfo(this ISharedPreferencesEditor sharedpreferenceseditor, string? key, CultureInfo? cultureInfo)
		{
			return sharedpreferenceseditor.PutString(key, cultureInfo?.IetfLanguageTag);
		}
		public static ISharedPreferencesEditor? PutEnum<TEnum>(this ISharedPreferencesEditor sharedpreferenceseditor, string? key, TEnum value) where TEnum : struct
		{
			return sharedpreferenceseditor.PutString(key, value.ToString());
		}				 				   
		public static ISharedPreferencesEditor? PutEnumSet<TEnum>(this ISharedPreferencesEditor sharedpreferenceseditor, string? key, IEnumerable<TEnum> value) where TEnum : struct
		{
			return sharedpreferenceseditor.PutString(key, string.Join('.', value.Select(val => val.ToString())));
		}				 
		public static ISharedPreferencesEditor? PutShort(this ISharedPreferencesEditor sharedpreferenceseditor, string? key, short value)
		{
			return sharedpreferenceseditor.PutInt(key, value); 
		}			  
		public static ISharedPreferencesEditor? PutShortArray(this ISharedPreferencesEditor sharedpreferenceseditor, string? key, short[] value)
		{
			return sharedpreferenceseditor.PutStringSet(key, value.Select(val => value.ToString()).OfType<string>().ToList()); 
		}

		public static ISharedPreferencesEditor PutSettings(this ISharedPreferencesEditor sharedpreferenceseditor, ISettings settings)
		{
			return sharedpreferenceseditor;
		}			  
		public static ISharedPreferencesEditor PutAbout(this ISharedPreferencesEditor sharedpreferenceseditor, IAboutSettings about)
		{
			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutAudio(this ISharedPreferencesEditor sharedpreferenceseditor, IAudioSettings audio)
		{
			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutAudioVolumeControl(this ISharedPreferencesEditor sharedpreferenceseditor, IVolumeControlSettings.IPresetable volumecontrol)
		{
			sharedpreferenceseditor.PutBoolean(IVolumeControlSettings.IPresetable.Keys.IsEnabled, volumecontrol.IsEnabled);
			sharedpreferenceseditor.PutString(IVolumeControlSettings.IPresetable.Keys.CurrentPreset, volumecontrol.CurrentPreset.Name);

			foreach (IVolumeControlSettings.IPreset preset in volumecontrol.AllPresets)
				sharedpreferenceseditor.PutAudioVolumeControl(preset);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutAudioVolumeControl(this ISharedPreferencesEditor sharedpreferenceseditor, params IVolumeControlSettings.IPreset[] presets)
		{
			foreach (IVolumeControlSettings.IPreset preset in presets.Where(_preset =>
			{
				return IVolumeControlSettings.IPresetable.Defaults.Presets
					.AsEnumerable()
					.Any(_presetdefault => string.Equals(_presetdefault.Name, _preset.Name, StringComparison.OrdinalIgnoreCase)) is false;
			}))
			{
				sharedpreferenceseditor.PutShort(IVolumeControlSettings.IPreset.Keys.BalancePosition(preset.Name), preset.BalancePosition);
				sharedpreferenceseditor.PutShort(IVolumeControlSettings.IPreset.Keys.BassBoostStrength(preset.Name), preset.BassBoostStrength);
				sharedpreferenceseditor.PutShort(IVolumeControlSettings.IPreset.Keys.LoudnessEnhancerTargetGain(preset.Name), preset.LoudnessEnhancerTargetGain);
				sharedpreferenceseditor.PutFloat(IVolumeControlSettings.IPreset.Keys.PlaybackPitch(preset.Name), preset.PlaybackPitch);
				sharedpreferenceseditor.PutFloat(IVolumeControlSettings.IPreset.Keys.PlaybackSpeed(preset.Name), preset.PlaybackSpeed);
			}

			return sharedpreferenceseditor;
		}											 									 
		public static ISharedPreferencesEditor PutAudioEnvironmentalReverb(this ISharedPreferencesEditor sharedpreferenceseditor, IEnvironmentalReverbSettings.IPresetable environmentalreverb)
		{
			sharedpreferenceseditor.PutBoolean(IEnvironmentalReverbSettings.IPresetable.Keys.IsEnabled, environmentalreverb.IsEnabled);
			sharedpreferenceseditor.PutString(IEnvironmentalReverbSettings.IPresetable.Keys.CurrentPreset, environmentalreverb.CurrentPreset.Name);

			foreach (IEnvironmentalReverbSettings.IPreset preset in environmentalreverb.AllPresets)
				sharedpreferenceseditor.PutAudioEnvironmentalReverb(preset);

			return sharedpreferenceseditor;
		}	 
		public static ISharedPreferencesEditor PutAudioEnvironmentalReverb(this ISharedPreferencesEditor sharedpreferenceseditor, params IEnvironmentalReverbSettings.IPreset[] presets)
		{
			foreach (IEnvironmentalReverbSettings.IPreset preset in presets.Where(_preset =>
			{
				return IEnvironmentalReverbSettings.IPresetable.Defaults.Presets
					.AsEnumerable()
					.Any(_presetdefault => string.Equals(_presetdefault.Name, _preset.Name, StringComparison.OrdinalIgnoreCase)) is false;
			}))
			{
				sharedpreferenceseditor.PutShort(IEnvironmentalReverbSettings.IPreset.Keys.DecayHFRatio(preset.Name), preset.DecayHFRatio);
				sharedpreferenceseditor.PutInt(IEnvironmentalReverbSettings.IPreset.Keys.DecayTime(preset.Name), preset.DecayTime);
				sharedpreferenceseditor.PutShort(IEnvironmentalReverbSettings.IPreset.Keys.Density(preset.Name), preset.Density);
				sharedpreferenceseditor.PutShort(IEnvironmentalReverbSettings.IPreset.Keys.Diffusion(preset.Name), preset.Diffusion);
				sharedpreferenceseditor.PutInt(IEnvironmentalReverbSettings.IPreset.Keys.ReflectionsDelay(preset.Name), preset.ReflectionsDelay);
				sharedpreferenceseditor.PutShort(IEnvironmentalReverbSettings.IPreset.Keys.ReflectionsLevel(preset.Name), preset.ReflectionsLevel);
				sharedpreferenceseditor.PutInt(IEnvironmentalReverbSettings.IPreset.Keys.ReverbDelay(preset.Name), preset.ReverbDelay);
				sharedpreferenceseditor.PutShort(IEnvironmentalReverbSettings.IPreset.Keys.ReverbLevel(preset.Name), preset.ReverbLevel);
				sharedpreferenceseditor.PutShort(IEnvironmentalReverbSettings.IPreset.Keys.RoomHFLevel(preset.Name), preset.RoomHFLevel);
				sharedpreferenceseditor.PutShort(IEnvironmentalReverbSettings.IPreset.Keys.RoomLevel(preset.Name), preset.RoomLevel);
			}

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutAudioEqualiser(this ISharedPreferencesEditor sharedpreferenceseditor, IEqualiserSettings.IPresetable equaliser)
		{
			sharedpreferenceseditor.PutBoolean(IEqualiserSettings.IPresetable.Keys.IsEnabled, equaliser.IsEnabled);
			sharedpreferenceseditor.PutString(IEqualiserSettings.IPresetable.Keys.CurrentPreset, equaliser.CurrentPreset.Name);

			foreach (IEqualiserSettings.IPreset preset in equaliser.AllPresets)
				sharedpreferenceseditor.PutAudioEqualiser(preset);

			return sharedpreferenceseditor;
		}									
		public static ISharedPreferencesEditor PutAudioEqualiser(this ISharedPreferencesEditor sharedpreferenceseditor, params IEqualiserSettings.IPreset[] presets)
		{
			foreach (IEqualiserSettings.IPreset preset in presets.Where(_preset =>
			{
				return IEqualiserSettings.IPresetable.Defaults.Presets
					.AsEnumerable()
					.Any(_presetdefault => string.Equals(_presetdefault.Name, _preset.Name, StringComparison.OrdinalIgnoreCase)) is false;
			}))
			{
				sharedpreferenceseditor.PutShortArray(IEqualiserSettings.IPreset.Keys.FrequencyLevels(preset.Name), preset.FrequencyLevels);
			}

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutFiles(this ISharedPreferencesEditor sharedpreferenceseditor, IFilesSettings files)
		{
			sharedpreferenceseditor.PutStringSet(IFilesSettings.Keys.Directories, files.Directories.ToList());
			sharedpreferenceseditor.PutStringSet(IFilesSettings.Keys.DirectoriesExclude, files.DirectoriesExclude.ToList());
			sharedpreferenceseditor.PutEnumSet(IFilesSettings.Keys.Mimetypes, files.Mimetypes);
			sharedpreferenceseditor.PutInt(IFilesSettings.Keys.TrackLengthIgnore, files.TrackLengthIgnore);

			return sharedpreferenceseditor;
		}																	  
		public static ISharedPreferencesEditor PutFilesDroid(this ISharedPreferencesEditor sharedpreferenceseditor, IFilesSettingsDroid files)
		{
			sharedpreferenceseditor.PutFiles(files);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutLockScreen(this ISharedPreferencesEditor sharedpreferenceseditor, ILockScreenSettings lockscreen)
		{
			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutNotification(this ISharedPreferencesEditor sharedpreferenceseditor, INotificationSettings notification)
		{
			return sharedpreferenceseditor;
		}	
		public static ISharedPreferencesEditor PutNotificationDroid(this ISharedPreferencesEditor sharedpreferenceseditor, INotificationSettingsDroid notification)
		{
			sharedpreferenceseditor.PutEnum(INotificationSettingsDroid.Keys.BadgeIconType, notification.BadgeIconType);
			sharedpreferenceseditor.PutColor(INotificationSettingsDroid.Keys.CustomColour, notification.CustomColour);
			sharedpreferenceseditor.PutBoolean(INotificationSettingsDroid.Keys.IsColourised, notification.IsColourised);
			sharedpreferenceseditor.PutEnum(INotificationSettingsDroid.Keys.Priority, notification.Priority);
			sharedpreferenceseditor.PutBoolean(INotificationSettingsDroid.Keys.UseCustomColour, notification.UseCustomColour);

			sharedpreferenceseditor.PutNotification(notification);

			return sharedpreferenceseditor;
		}											   
		public static ISharedPreferencesEditor PutSystem(this ISharedPreferencesEditor sharedpreferenceseditor, ISystemSettings system)
		{
			return sharedpreferenceseditor;
		}						   				   
		public static ISharedPreferencesEditor PutSystemDroid(this ISharedPreferencesEditor sharedpreferenceseditor, ISystemSettingsDroid system)
		{
			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterface(this ISharedPreferencesEditor sharedpreferenceseditor, IUserInterfaceSettings userinterface)
		{
			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLanguages(this ISharedPreferencesEditor sharedpreferenceseditor, ILanguagesSettings languages)
		{
			sharedpreferenceseditor.PutCultureInfo(ILanguagesSettings.Keys.CurrentLanguage, languages.CurrentLanguage);
			sharedpreferenceseditor.PutEnum(ILanguagesSettings.Keys.Mode, languages.Mode);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibrary(this ISharedPreferencesEditor sharedpreferenceseditor, ILibrarySettings library)
		{
			sharedpreferenceseditor.PutEnum(ILibrarySettings.Keys.PageDefault, library.PageDefault);

			sharedpreferenceseditor.PutInt(ILibrarySettings.Keys.PagesOrderedAlbums, library.PagesOrdered?.Index(ILibrarySettings.Options.Pages.Albums) ?? -1);
			sharedpreferenceseditor.PutInt(ILibrarySettings.Keys.PagesOrderedArtists, library.PagesOrdered?.Index(ILibrarySettings.Options.Pages.Artists) ?? -1);
			sharedpreferenceseditor.PutInt(ILibrarySettings.Keys.PagesOrderedGenres, library.PagesOrdered?.Index(ILibrarySettings.Options.Pages.Genres) ?? -1);
			sharedpreferenceseditor.PutInt(ILibrarySettings.Keys.PagesOrderedPlaylists, library.PagesOrdered?.Index(ILibrarySettings.Options.Pages.Playlists) ?? -1);
			sharedpreferenceseditor.PutInt(ILibrarySettings.Keys.PagesOrderedQueue, library.PagesOrdered?.Index(ILibrarySettings.Options.Pages.Queue) ?? -1);
			sharedpreferenceseditor.PutInt(ILibrarySettings.Keys.PagesOrderedSongs, library.PagesOrdered?.Index(ILibrarySettings.Options.Pages.Songs) ?? -1);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryDroid(this ISharedPreferencesEditor sharedpreferenceseditor, ILibrarySettingsDroid librarydroid)
		{
			sharedpreferenceseditor.PutEnum(ILibrarySettingsDroid.Keys.HeaderScrollType, librarydroid.HeaderScrollType);
			sharedpreferenceseditor.PutEnum(ILibrarySettingsDroid.Keys.NavigationType, librarydroid.NavigationType);

			sharedpreferenceseditor.PutUserInterfaceLibrary(librarydroid);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryAlbum(this ISharedPreferencesEditor sharedpreferenceseditor, IAlbumSettings album)
		{
			sharedpreferenceseditor.PutBoolean(IAlbumSettings.Keys.SongsIsReversed, album.SongsIsReversed);
			sharedpreferenceseditor.PutEnum(IAlbumSettings.Keys.SongsLayoutType, album.SongsLayoutType);
			sharedpreferenceseditor.PutEnum(IAlbumSettings.Keys.SongsSortKey, album.SongsSortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryAlbums(this ISharedPreferencesEditor sharedpreferenceseditor, IAlbumsSettings albums)
		{
			sharedpreferenceseditor.PutBoolean(IAlbumsSettings.Keys.IsReversed, albums.IsReversed);
			sharedpreferenceseditor.PutEnum(IAlbumsSettings.Keys.LayoutType, albums.LayoutType);
			sharedpreferenceseditor.PutEnum(IAlbumsSettings.Keys.SortKey, albums.SortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryArtist(this ISharedPreferencesEditor sharedpreferenceseditor, IArtistSettings artist)
		{
			sharedpreferenceseditor.PutBoolean(IArtistSettings.Keys.AlbumsIsReversed, artist.AlbumsIsReversed);
			sharedpreferenceseditor.PutEnum(IArtistSettings.Keys.AlbumsLayoutType, artist.AlbumsLayoutType);
			sharedpreferenceseditor.PutEnum(IArtistSettings.Keys.AlbumsSortKey, artist.AlbumsSortKey);
			sharedpreferenceseditor.PutBoolean(IArtistSettings.Keys.SongsIsReversed, artist.SongsIsReversed);
			sharedpreferenceseditor.PutEnum(IArtistSettings.Keys.SongsLayoutType, artist.SongsLayoutType);
			sharedpreferenceseditor.PutEnum(IArtistSettings.Keys.SongsSortKey, artist.SongsSortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryArtists(this ISharedPreferencesEditor sharedpreferenceseditor, IArtistsSettings artists)
		{
			sharedpreferenceseditor.PutBoolean(IArtistsSettings.Keys.IsReversed, artists.IsReversed);
			sharedpreferenceseditor.PutEnum(IArtistsSettings.Keys.LayoutType, artists.LayoutType);
			sharedpreferenceseditor.PutEnum(IArtistsSettings.Keys.SortKey, artists.SortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryGenre(this ISharedPreferencesEditor sharedpreferenceseditor, IGenreSettings genre)
		{
			sharedpreferenceseditor.PutBoolean(IGenreSettings.Keys.SongsIsReversed, genre.SongsIsReversed);
			sharedpreferenceseditor.PutEnum(IGenreSettings.Keys.SongsLayoutType, genre.SongsLayoutType);
			sharedpreferenceseditor.PutEnum(IGenreSettings.Keys.SongsSortKey, genre.SongsSortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryGenres(this ISharedPreferencesEditor sharedpreferenceseditor, IGenresSettings genres)
		{
			sharedpreferenceseditor.PutBoolean(IGenresSettings.Keys.IsReversed, genres.IsReversed);
			sharedpreferenceseditor.PutEnum(IGenresSettings.Keys.LayoutType, genres.LayoutType);
			sharedpreferenceseditor.PutEnum(IGenresSettings.Keys.SortKey, genres.SortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryPlaylist(this ISharedPreferencesEditor sharedpreferenceseditor, IPlaylistSettings playlist)
		{
			sharedpreferenceseditor.PutBoolean(IPlaylistSettings.Keys.SongsIsReversed, playlist.SongsIsReversed);
			sharedpreferenceseditor.PutEnum(IPlaylistSettings.Keys.SongsLayoutType, playlist.SongsLayoutType);
			sharedpreferenceseditor.PutEnum(IPlaylistSettings.Keys.SongsSortKey, playlist.SongsSortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryPlaylists(this ISharedPreferencesEditor sharedpreferenceseditor, IPlaylistsSettings playlists)
		{
			sharedpreferenceseditor.PutBoolean(IPlaylistsSettings.Keys.IsReversed, playlists.IsReversed);
			sharedpreferenceseditor.PutEnum(IPlaylistsSettings.Keys.LayoutType, playlists.LayoutType);
			sharedpreferenceseditor.PutEnum(IPlaylistsSettings.Keys.SortKey, playlists.SortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibraryQueue(this ISharedPreferencesEditor sharedpreferenceseditor, IQueueSettings queue)
		{
			sharedpreferenceseditor.PutEnum(IQueueSettings.Keys.LayoutType, queue.LayoutType);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibrarySearch(this ISharedPreferencesEditor sharedpreferenceseditor, ISearchSettings search)
		{
			sharedpreferenceseditor.PutEnum(ISearchSettings.Keys.LayoutType, search.LayoutType);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceLibrarySongs(this ISharedPreferencesEditor sharedpreferenceseditor, ISongsSettings songs)
		{
			sharedpreferenceseditor.PutBoolean(ISongsSettings.Keys.IsReversed, songs.IsReversed);
			sharedpreferenceseditor.PutEnum(ISongsSettings.Keys.LayoutType, songs.LayoutType);
			sharedpreferenceseditor.PutEnum(ISongsSettings.Keys.SortKey, songs.SortKey);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceNowPlaying(this ISharedPreferencesEditor sharedpreferenceseditor, INowPlayingSettings nowplaying)
		{
			return sharedpreferenceseditor;
		}		  
		public static ISharedPreferencesEditor PutUserInterfaceNowPlayingDroid(this ISharedPreferencesEditor sharedpreferenceseditor, INowPlayingSettingsDroid nowplayingdroid)
		{
			sharedpreferenceseditor.PutBoolean(INowPlayingSettingsDroid.Keys.ForceShowNowPlaying, nowplayingdroid.ForceShowNowPlaying);

			return sharedpreferenceseditor;
		}
		public static ISharedPreferencesEditor PutUserInterfaceThemes(this ISharedPreferencesEditor sharedpreferenceseditor, IThemesSettings themes)
		{
			sharedpreferenceseditor.PutEnum(IThemesSettings.Keys.Mode, themes.Mode);

			return sharedpreferenceseditor;
		}
	}
}