using System.Linq;
using System.Text;

using Xyzu.Library.Models;

namespace Id3
{
	public static class Mp3Extensions
	{
		public static bool ShouldRetrieveAlbum(this Mp3 mp3, IAlbum album)
		{
			return mp3.GetAllTags()
				.Any(tag => string.Equals(album?.Title, tag.Album.Value));
		}
		public static bool ShouldRetrieveArtist(this Mp3 mp3, IArtist artist)
		{
			return mp3.GetAllTags()
				.Any(tag => tag.Artists.Value.Any(value => string.Equals(artist?.Name, value)));
		}
		public static bool ShouldRetrieveGenre(this Mp3 mp3, IGenre genre)
		{
			return mp3.GetAllTags()
				.Any(tag => string.Equals(genre?.Name, tag.Genre.Value));
		}
		public static bool ShouldRetrievePlaylist(this Mp3 mp3, IPlaylist playlist)
		{
			return false;
		}
		public static bool ShouldRetrieveSong(this Mp3 mp3, ISong song)
		{
			return mp3.GetAllTags()
				.Any(tag => string.Equals(song?.Title, tag.Title.Value));
		}

		public static string TagsToString(this Mp3 mp3, StringBuilder? stringbuilder = null)
		{
			stringbuilder ??= new StringBuilder();

			foreach (Id3Tag id3tag in mp3.GetAllTags()) stringbuilder
				 .Append("Id3Version: ").AppendLine(id3tag.Version.ToString())
				 .Append("CopyrightFrame: ").AppendLine(id3tag.Copyright.ToString())
				 .Append("CopyrightUrlFrame: ").AppendLine(id3tag.CopyrightUrl.ToString())
				 .Append("CustomTextFrameList: ").AppendLine(id3tag.CustomTexts.ToString())
				 .Append("EncoderFrame: ").AppendLine(id3tag.Encoder.ToString())
				 .Append("EncodingSettingsFrame: ").AppendLine(id3tag.EncodingSettings.ToString())
				 .Append("FileOwnerFrame: ").AppendLine(id3tag.FileOwner.ToString())
				 .Append("FileTypeFrame: ").AppendLine(id3tag.FileType.ToString())
				 .Append("GenreFrame: ").AppendLine(id3tag.Genre.ToString())
				 .Append("ContentGroupDescriptionFrame: ").AppendLine(id3tag.ContentGroupDescription.ToString())
				 .Append("LengthFrame: ").AppendLine(id3tag.Length.ToString())
				 .Append("LyricsFrameList: ").AppendLine(id3tag.Lyrics.ToString())
				 .Append("PaymentUrlFrame: ").AppendLine(id3tag.PaymentUrl.ToString())
				 .Append("PublisherFrame: ").AppendLine(id3tag.Publisher.ToString())
				 .Append("PictureFrameList: ").AppendLine(id3tag.Pictures.ToString())
				 .Append("PrivateFrameList: ").AppendLine(id3tag.PrivateData.ToString())
				 .Append("RecordingDateFrame: ").AppendLine(id3tag.RecordingDate.ToString())
				 .Append("SubtitleFrame: ").AppendLine(id3tag.Subtitle.ToString())
				 .Append("TitleFrame: ").AppendLine(id3tag.Title.ToString())
				 .Append("LyricistsFrame: ").AppendLine(id3tag.Lyricists.ToString())
				 .Append("ConductorFrame: ").AppendLine(id3tag.Conductor.ToString())
				 .Append("ComposersFrame: ").AppendLine(id3tag.Composers.ToString())
				 .Append("CommercialUrlFrameList: ").AppendLine(id3tag.CommercialUrls.ToString())
				 .Append("TrackFrame: ").AppendLine(id3tag.Track.ToString())
				 .Append("AlbumFrame: ").AppendLine(id3tag.Album.ToString())
				 .Append("ArtistsFrame: ").AppendLine(id3tag.Artists.ToString())
				 .Append("ArtistUrlFrameList: ").AppendLine(id3tag.ArtistUrls.ToString())
				 .Append("Id3TagFamily: ").AppendLine(id3tag.Family.ToString())
				 .Append("AudioSourceUrlFrame: ").AppendLine(id3tag.AudioSourceUrl.ToString())
				 .Append("BandFrame: ").AppendLine(id3tag.Band.ToString())
				 .Append("BeatsPerMinuteFrame: ").AppendLine(id3tag.BeatsPerMinute.ToString())
				 .Append("CommentFrameList: ").AppendLine(id3tag.Comments.ToString())
				 .Append("AudioFileUrlFrame: ").AppendLine(id3tag.AudioFileUrl.ToString())
				 .Append("YearFrame: ").AppendLine(id3tag.Year.ToString());

			return stringbuilder.ToString();
		}
	}
}
