using System.Text;

using Xyzu.Library.Models;

namespace TagLib
{
	public static class FileExtensions
	{
		public static bool ShouldRetrieveAlbum(this File file, IAlbum album)
		{
			return string.Equals(album.Title, file.Tag.Album);
		}
		public static bool ShouldRetrieveArtist(this File file, IArtist artist)
		{
			foreach (string p in file.Tag.Performers)
				if (string.Equals(artist.Name, p))
					return true;

			return false;
		}
		public static bool ShouldRetrieveGenre(this File file, IGenre genre)
		{
			foreach (string g in file.Tag.Genres)
				if (string.Equals(genre.Name, g))
					return true;

			return false;
		}									   
		public static bool ShouldRetrievePlaylist(this File file, IPlaylist playlist)
		{
			return false;
		}
		public static bool ShouldRetrieveSong(this File file, ISong song)
		{
			return string.Equals(song.Title, file.Tag.Title);
		}

		public static string TagsToString(this File file, StringBuilder? stringbuilder = null)
		{
			(stringbuilder ??= new StringBuilder())

				.Append("MusicBrainzReleaseGroupId: ").AppendLine(file.Tag.MusicBrainzReleaseGroupId)
				.Append("RemixedBy: ").AppendLine(file.Tag.RemixedBy)
				.Append("InitialKey: ").AppendLine(file.Tag.InitialKey)
				.Append("ReplayGainAlbumPeak: ").AppendLine(file.Tag.ReplayGainAlbumPeak.ToString())
				.Append("ReplayGainAlbumGain: ").AppendLine(file.Tag.ReplayGainAlbumGain.ToString())
				.Append("ReplayGainTrackPeak: ").AppendLine(file.Tag.ReplayGainTrackPeak.ToString())
				.Append("ReplayGainTrackGain: ").AppendLine(file.Tag.ReplayGainTrackGain.ToString())
				.Append("MusicBrainzReleaseCountry: ").AppendLine(file.Tag.MusicBrainzReleaseCountry)
				.Append("MusicBrainzReleaseType: ").AppendLine(file.Tag.MusicBrainzReleaseType)
				.Append("MusicBrainzReleaseStatus: ").AppendLine(file.Tag.MusicBrainzReleaseStatus)
				.Append("AmazonId: ").AppendLine(file.Tag.AmazonId)
				.Append("MusicIpId: ").AppendLine(file.Tag.MusicIpId)
				.Append("MusicBrainzDiscId: ").AppendLine(file.Tag.MusicBrainzDiscId)
				.Append("MusicBrainzTrackId: ").AppendLine(file.Tag.MusicBrainzTrackId)
				.Append("Publisher: ").AppendLine(file.Tag.Publisher)
				.Append("ISRC: ").AppendLine(file.Tag.ISRC)
				.Append("FirstAlbumArtist: ").AppendLine(file.Tag.FirstAlbumArtist)
				.Append("FirstAlbumArtistSort: ").AppendLine(file.Tag.FirstAlbumArtistSort)
				.Append("FirstPerformer: ").AppendLine(file.Tag.FirstPerformer)
				.Append("FirstPerformerSort: ").AppendLine(file.Tag.FirstPerformerSort)
				.Append("FirstComposerSort: ").AppendLine(file.Tag.FirstComposerSort)
				.Append("FirstComposer: ").AppendLine(file.Tag.FirstComposer)
				.Append("FirstGenre: ").AppendLine(file.Tag.FirstGenre)
				.Append("JoinedAlbumArtists: ").AppendLine(file.Tag.JoinedAlbumArtists)
				.Append("JoinedPerformers: ").AppendLine(file.Tag.JoinedPerformers)
				.Append("JoinedPerformersSort: ").AppendLine(file.Tag.JoinedPerformersSort)
				.Append("JoinedComposers: ").AppendLine(file.Tag.JoinedComposers)
				.Append("MusicBrainzReleaseArtistId: ").AppendLine(file.Tag.MusicBrainzReleaseArtistId)
				.Append("MusicBrainzReleaseId: ").AppendLine(file.Tag.MusicBrainzReleaseId)
				.Append("IsEmpty: ").AppendLine(file.Tag.IsEmpty.ToString())
				.Append("MusicBrainzArtistId: ").AppendLine(file.Tag.MusicBrainzArtistId)
				.Append("TagTypes: ").AppendLine(file.Tag.TagTypes.ToString())
				.Append("Title: ").AppendLine(file.Tag.Title)
				.Append("TitleSort: ").AppendLine(file.Tag.TitleSort)
				.Append("Subtitle: ").AppendLine(file.Tag.Subtitle)
				.Append("Description: ").AppendLine(file.Tag.Description)
				.Append("Performers: ").AppendLine(string.Join("; ", file.Tag.Performers))
				.Append("PerformersSort: ").AppendLine(string.Join("; ", file.Tag.PerformersSort))
				.Append("PerformersRole: ").AppendLine(string.Join("; ", file.Tag.PerformersRole))
				.Append("AlbumArtists: ").AppendLine(string.Join("; ", file.Tag.AlbumArtists))
				.Append("AlbumArtistsSort: ").AppendLine(string.Join("; ", file.Tag.AlbumArtistsSort))
				.Append("Composers: ").AppendLine(string.Join("; ", file.Tag.Composers))
				.Append("JoinedGenres: ").AppendLine(file.Tag.JoinedGenres)
				.Append("Album: ").AppendLine(file.Tag.Album)
				.Append("ComposersSort: ").AppendLine(string.Join("; ", file.Tag.ComposersSort))
				.Append("Comment: ").AppendLine(file.Tag.Comment)
				.Append("DateTagged: ").AppendLine(file.Tag.DateTagged?.ToString("yyyy/MMM/dd"))
				.Append("Copyright: ").AppendLine(file.Tag.Copyright)
				.Append("Conductor: ").AppendLine(file.Tag.Conductor)
				.Append("BeatsPerMinute: ").AppendLine(file.Tag.BeatsPerMinute.ToString())
				.Append("AlbumSort: ").AppendLine(file.Tag.AlbumSort)
				.Append("Lyrics: ").AppendLine(file.Tag.Lyrics)
				.Append("Grouping: ").AppendLine(file.Tag.Grouping)
				.Append("Disc: ").AppendLine(file.Tag.Disc.ToString())
				.Append("TrackCount: ").AppendLine(file.Tag.TrackCount.ToString())
				.Append("Track: ").AppendLine(file.Tag.Track.ToString())
				.Append("Year: ").AppendLine(file.Tag.Year.ToString())
				.Append("Genres: ").AppendLine(string.Join("; ", file.Tag.Genres))
				.Append("DiscCount: ").AppendLine(file.Tag.DiscCount.ToString());

			return stringbuilder.ToString();
		}
	}
}
