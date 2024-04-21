using System;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public partial interface ISong : IModel
	{
		public void Populate(ISong song)
		{
			Album ??= song.Album;
			AlbumArtist ??= song.AlbumArtist;
			Artist ??= song.Artist;
			Bitrate ??= song.Bitrate;
			Channels ??= song.Channels;
			DateAdded ??= song.DateAdded;
			DateModified ??= song.DateModified;
			DiscNumber ??= song.DiscNumber;
			Duration ??= song.Duration;
			Filepath ??= song.Filepath;
			Genre ??= song.Genre;
			Lyrics ??= song.Lyrics;
			MimeType ??= song.MimeType;
			ReleaseDate ??= song.ReleaseDate;
			Size ??= song.Size;
			Title ??= song.Title;
			TrackNumber ??= song.TrackNumber;
			Uri ??= song.Uri;
		}
		public ISong<bool>? Distinct(ISong compareto, ISong<bool>? fieldtocompare = null)
		{
			bool distinctalbum = fieldtocompare?.Album ?? true && (Album is not null || compareto.Album is not null) && !string.Equals(Album, compareto.Album);
			bool distinctalbumartist = fieldtocompare?.AlbumArtist ?? true && (AlbumArtist is not null || compareto.AlbumArtist is not null) && !string.Equals(AlbumArtist, compareto.AlbumArtist);
			bool distinctartist = fieldtocompare?.Artist ?? true && (Artist is not null || compareto.Artist is not null) && !string.Equals(Artist, compareto.Artist);
			bool distinctcopyright = fieldtocompare?.Copyright ?? true && (Copyright is not null || compareto.Copyright is not null) && !string.Equals(Copyright, compareto.Copyright);
			bool distinctdiscnumber = fieldtocompare?.DiscNumber ?? true && (DiscNumber.HasValue || compareto.DiscNumber.HasValue) && DiscNumber != compareto.DiscNumber;
			bool distinctduration = fieldtocompare?.Duration ?? true && (Duration.HasValue || compareto.Duration.HasValue) && Duration != compareto.Duration;
			bool distinctgenre = fieldtocompare?.Genre ?? true && (Genre is not null || compareto.Genre is not null) && !string.Equals(Genre, compareto.Genre);
			bool distinctlyrics = fieldtocompare?.Lyrics ?? true && (Lyrics is not null || compareto.Lyrics is not null) && !string.Equals(Lyrics, compareto.Lyrics);
			bool distinctrating = fieldtocompare?.Rating ?? true && Rating != compareto.Rating;
			bool distinctreleasedate = fieldtocompare?.ReleaseDate ?? true && (ReleaseDate.HasValue || compareto.ReleaseDate.HasValue) && ReleaseDate != compareto.ReleaseDate;
			bool distincttitle = fieldtocompare?.Title ?? true && (Title is not null || compareto.Title is not null) && !string.Equals(Title, compareto.Title);
			bool distincttracknumber = fieldtocompare?.TrackNumber ?? true && (TrackNumber.HasValue || compareto.TrackNumber.HasValue) && TrackNumber != compareto.TrackNumber;

			bool distinct =
				distinctalbum ||
				distinctalbumartist ||
				distinctartist ||
				distinctcopyright ||
				distinctdiscnumber ||
				distinctduration ||
				distinctgenre ||
				distinctlyrics ||
				distinctrating ||
				distinctreleasedate ||
				distincttitle ||
				distincttracknumber;

			if (distinct is false)
				return null;

			return new Default<bool>(false)
			{
				Album = distinctalbum,
				AlbumArtist = distinctalbumartist,
				Artist = distinctartist,
				Copyright = distinctcopyright,
				DiscNumber = distinctdiscnumber,
				Duration = distinctduration,
				Genre = distinctgenre,
				Lyrics = distinctlyrics,
				Rating = distinctrating,
				ReleaseDate = distinctreleasedate,
				Title = distincttitle,
				TrackNumber = distincttracknumber,
			};
		}
	}
}
