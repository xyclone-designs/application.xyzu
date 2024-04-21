#nullable enable

using Android.Content;

using System;

using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;
using AndroidStore = Android.Provider.MediaStore;

namespace Xyzu.Library.MediaStore
{
	public partial class MediaStoreUtils
	{
		public static class ColumnNames
		{
			// Errors regarding cursor creation with convconventional InterfaceConsts (Bad MediaStore/Bad Me). Use these for no? errors

			private const string Obsolete_Deprecated = "deprecated";

			public const string Album = AndroidStore.Audio.Media.InterfaceConsts.Album;
			public const string AlbumId = AndroidStore.Audio.Media.InterfaceConsts.AlbumId;
			public const string AlbumArt = AndroidStore.Audio.Media.InterfaceConsts.AlbumArt;
			public const string AlbumArtist = AndroidStore.Audio.Media.InterfaceConsts.AlbumArtist;
			[Obsolete(Obsolete_Deprecated)]
			public const string AlbumKey = AndroidStore.Audio.Media.InterfaceConsts.AlbumKey;
			public const string Artist = AndroidStore.Audio.Media.InterfaceConsts.Artist;
			public const string ArtistId = AndroidStore.Audio.Media.InterfaceConsts.ArtistId;
			[Obsolete(Obsolete_Deprecated)]
			public const string ArtistKey = AndroidStore.Audio.Media.InterfaceConsts.ArtistKey;
			public const string Bookmark = AndroidStore.Audio.Media.InterfaceConsts.Bookmark;
			public const string BucketDisplayName = AndroidStore.Audio.Media.InterfaceConsts.BucketDisplayName;
			public const string BucketId = AndroidStore.Audio.Media.InterfaceConsts.BucketId;
			public const string Composer = AndroidStore.Audio.Media.InterfaceConsts.Composer;
			[Obsolete(Obsolete_Deprecated)]
			public const string Data = AndroidStore.Audio.Media.InterfaceConsts.Data;
			public const string DateAdded = AndroidStore.Audio.Media.InterfaceConsts.DateAdded;
			public const string DateExpires = AndroidStore.Audio.Media.InterfaceConsts.DateExpires;
			public const string DateModified = AndroidStore.Audio.Media.InterfaceConsts.DateModified;
			public const string DateTaken = AndroidStore.Audio.Media.InterfaceConsts.DateTaken;
			public const string DisplayName = AndroidStore.Audio.Media.InterfaceConsts.DisplayName;
			public const string DocumentId = AndroidStore.Audio.Media.InterfaceConsts.DocumentId;
			public const string Duration = AndroidStore.Audio.Media.InterfaceConsts.Duration;
			public const string Height = AndroidStore.Audio.Media.InterfaceConsts.Height;
			public const string Id = AndroidStore.Audio.Media.InterfaceConsts.Id;
			public const string IsAudiobook = AndroidStore.Audio.Media.InterfaceConsts.IsAudiobook;
			public const string IsAlarm = AndroidStore.Audio.Media.InterfaceConsts.IsAlarm;
			public const string IsDrm = AndroidStore.Audio.Media.InterfaceConsts.IsDrm;
			public const string IsMusic = AndroidStore.Audio.Media.InterfaceConsts.IsMusic;
			public const string IsNotification = AndroidStore.Audio.Media.InterfaceConsts.IsNotification;
			public const string IsPending = AndroidStore.Audio.Media.InterfaceConsts.IsPending;
			public const string IsPodcast = AndroidStore.Audio.Media.InterfaceConsts.IsPodcast;
			public const string IsRingtone = AndroidStore.Audio.Media.InterfaceConsts.IsRingtone;
			public const string IsTrashed = AndroidStore.Audio.Media.InterfaceConsts.IsTrashed;
			public const string InstanceId = AndroidStore.Audio.Media.InterfaceConsts.InstanceId;
			public const string MimeType = AndroidStore.Audio.Media.InterfaceConsts.MimeType;
			public const string Orientation = AndroidStore.Audio.Media.InterfaceConsts.Orientation;
			public const string OwnerPackageName = AndroidStore.Audio.Media.InterfaceConsts.OwnerPackageName;
			public const string OriginalDocumentId = AndroidStore.Audio.Media.InterfaceConsts.OriginalDocumentId;
			public const string RelativePath = AndroidStore.Audio.Media.InterfaceConsts.RelativePath;
			public const string Size = AndroidStore.Audio.Media.InterfaceConsts.Size;
			public const string Title = AndroidStore.Audio.Media.InterfaceConsts.Title;
			[Obsolete(Obsolete_Deprecated)]
			public const string TitleKey = AndroidStore.Audio.Media.InterfaceConsts.TitleKey;
			public const string TitleResourceUri = AndroidStore.Audio.Media.InterfaceConsts.TitleResourceUri;
			public const string Track = AndroidStore.Audio.Media.InterfaceConsts.Track;
			public const string VolumeName = AndroidStore.Audio.Media.InterfaceConsts.VolumeName;
			public const string Width = AndroidStore.Audio.Media.InterfaceConsts.Width;
			public const string Year = AndroidStore.Audio.Media.InterfaceConsts.Year;
		}
		public static class Uris
		{
			public const string AlbumArtworkContentUriString = "content://media/external/audio/albumart";

			public static AndroidUri? AlbumArtworkContentUri = AndroidUri.Parse(AlbumArtworkContentUriString);

			public static AndroidUri? AlbumArtwork(long albumid)
			{
				if (AlbumArtworkContentUri is null)
					return null;

				return ContentUris.WithAppendedId(AlbumArtworkContentUri, albumid);
			}
			public static AndroidUri? ArtistImage(long artistid)
			{
				if (AlbumArtworkContentUri is null)
					return null;

				return ContentUris.WithAppendedId(AlbumArtworkContentUri, artistid);
			}
			public static AndroidUri? SongArtwork(long albumid)
			{
				if (AlbumArtworkContentUri is null)
					return null;

				return ContentUris.WithAppendedId(AlbumArtworkContentUri, albumid);
			}
		}

		public static bool WouldBenefit(IAlbum? retrieved, IAlbum<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return
				(retrieved.Artist is null && (retriever?.Artist ?? true)) ||
				(retrieved.ReleaseDate is null && (retriever?.ReleaseDate ?? true)) ||
				(retrieved.Title is null && (retriever?.Title ?? true));
		}
		public static bool WouldBenefit(IArtist? retrieved, IArtist<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return retrieved.Name is null && (retriever?.Name ?? true);
		}
		public static bool WouldBenefit(IGenre? retrieved, IGenre<bool>? retriever)
		{
			//if (retrieved is null)
			//	return false;

			return false;
		}
		public static bool WouldBenefit(IPlaylist? retrieved, IPlaylist<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return
				(retrieved.DateCreated == default && (retriever?.DateCreated ?? true)) ||
				(retrieved.DateModified is null && (retriever?.DateModified ?? true)) ||
				(retriever?.Duration ?? true);
		}
		public static bool WouldBenefit(ISong? retrieved, ISong<bool>? retriever)
		{
			if (retrieved is null)
				return false;

			return
				(retrieved.Album is null && (retriever?.Album ?? true)) ||
				(retrieved.AlbumArtist is null && (retriever?.AlbumArtist ?? true)) ||
				(retrieved.Artist is null && (retriever?.Artist ?? true)) ||
				(retrieved.Duration is null && (retriever?.Duration ?? true)) ||
				(retrieved.DateAdded is null && (retriever is null || (retriever?.DateAdded ?? false))) ||
				(retrieved.DateModified is null && (retriever is null || (retriever?.DateModified ?? false))) ||
				(retrieved.MimeType is null && (retriever is null || (retriever?.MimeType ?? false))) ||
				(retrieved.Size is null && (retriever is null || (retriever?.Size ?? false))) ||
				(retrieved.ReleaseDate is null && (retriever?.ReleaseDate ?? true)) ||
				(retrieved.Title is null && (retriever?.Title ?? true)) ||
				(retrieved.TrackNumber is null && (retriever?.TrackNumber ?? true));
		}
	}
}