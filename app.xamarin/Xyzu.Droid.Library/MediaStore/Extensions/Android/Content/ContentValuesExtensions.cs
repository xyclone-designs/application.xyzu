#nullable enable

using System;

using Xyzu.Library.Models;
using Xyzu.Library.MediaStore;

namespace Android.Content
{
	public static class ContentValuesExtensions
	{
		public static ContentValues UpdateAlbum(this ContentValues contentvalues, IAlbum old, IAlbum updated)
		{
			if (string.Equals(old.Artist, updated.Artist, StringComparison.OrdinalIgnoreCase) is false)
				contentvalues.Put(MediaStoreUtils.ColumnNames.AlbumArtist, updated.Artist);

			if ((old.ReleaseDate == updated.ReleaseDate) is false)
			{
				if (updated.ReleaseDate.HasValue)
					contentvalues.Put(MediaStoreUtils.ColumnNames.Year, updated.ReleaseDate.Value.Year);
				else contentvalues.PutNull(MediaStoreUtils.ColumnNames.Year);
			}

			if (string.Equals(old.Title, updated.Title, StringComparison.OrdinalIgnoreCase) is false)
				contentvalues.Put(MediaStoreUtils.ColumnNames.Album, updated.Title);

			return contentvalues;
		}
		public static ContentValues UpdateArtist(this ContentValues contentvalues, IArtist old, IArtist updated)
		{
			if (string.Equals(old.Name, updated.Name, StringComparison.OrdinalIgnoreCase) is false)
				contentvalues.Put(MediaStoreUtils.ColumnNames.Artist, updated.Name);

			return contentvalues;
		}
		public static ContentValues UpdateGenre(this ContentValues contentvalues, IGenre old, IGenre updated)
		{
			return contentvalues;
		}
		public static ContentValues UpdatePlaylist(this ContentValues contentvalues, IPlaylist old, IPlaylist updated)
		{
			if ((old.DateModified == updated.DateModified) is false)
				contentvalues.Put(MediaStoreUtils.ColumnNames.DateModified, updated.DateModified?.ToString());
			
			return contentvalues;
		}
		public static ContentValues UpdateSong(this ContentValues contentvalues, ISong old, ISong updated)
		{
			if (string.Equals(old.Album, updated.Album, StringComparison.OrdinalIgnoreCase) is false)
				contentvalues.Put(MediaStoreUtils.ColumnNames.Album, updated.Album);

			if (string.Equals(old.AlbumArtist, updated.AlbumArtist, StringComparison.OrdinalIgnoreCase) is false)
				contentvalues.Put(MediaStoreUtils.ColumnNames.AlbumArtist, updated.AlbumArtist);

			if (string.Equals(old.Artist, updated.Artist, StringComparison.OrdinalIgnoreCase) is false)
				contentvalues.Put(MediaStoreUtils.ColumnNames.Artist, updated.Artist);

			if ((old.ReleaseDate == updated.ReleaseDate) is false)
			{
				if (updated.ReleaseDate.HasValue)
					contentvalues.Put(MediaStoreUtils.ColumnNames.Year, updated.ReleaseDate.Value.Year);
				else contentvalues.PutNull(MediaStoreUtils.ColumnNames.Year);
			}

			if (string.Equals(old.Title, updated.Title, StringComparison.OrdinalIgnoreCase) is false)
				contentvalues.Put(MediaStoreUtils.ColumnNames.Title, updated.Title);

			return contentvalues;
		}
	}
}