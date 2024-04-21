using System;
using System.Text;

using Xyzu.Images;
using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public interface ISong<T> : IModel<T>
	{
		T Album { get; set; }
		T AlbumArtist { get; set; }
		T Artist { get; set; }
		IImage<T>? Artwork { get; set; }
		T Bitrate { get; set; }
		T Channels { get; set; }
		T Copyright { get; set; }
		T DateAdded { get; set; }
		T DateModified { get; set; }
		T DiscNumber { get; set; }
		T Duration { get; set; }
		T Filepath { get; set; }
		T Genre { get; set; }
		T Lyrics { get; set; }
		T MimeType { get; set; }
		T ReleaseDate { get; set; }
		T Size { get; set; }
		T Title { get; set; }
		T TrackNumber { get; set; }
		T Uri { get; set; }
	}
	public partial interface ISong : IModel
	{									   
		string? Album { get; set; }
		string? AlbumArtist { get; set; }
		string? Artist { get; set; }
		IImage? Artwork { get; set; }
		int? Bitrate { get; set; }
		AudioChannels? Channels { get; set; }
		string? Copyright { get; set; }
		DateTime? DateAdded { get; set; }
		DateTime? DateModified { get; set; }
		int? DiscNumber { get; set; }
		TimeSpan? Duration { get; set; }
		string? Filepath { get; set; }
		string? Genre { get; set; }
		string? Lyrics { get; set; }
		MimeTypes? MimeType { get; set; }
		DateTime? ReleaseDate { get; set; }
		long? Size { get; set; }
		string? Title { get; set; }
		int? TrackNumber { get; set; }
		Uri? Uri { get; set; }

		public new class Default : IModel.Default, ISong 
		{
			public Default(string id) : base(id) { }
			public Default(ISong song) : base(song)
			{
				Album = song.Album;
				AlbumArtist = song.AlbumArtist;
				Artist = song.Artist;
				Artwork = song.Artwork;
				Bitrate = song.Bitrate;
				Channels = song.Channels;
				Copyright = song.Copyright;
				DateAdded = song.DateAdded;
				DateModified = song.DateModified;
				DiscNumber = song.DiscNumber;
				Duration = song.Duration;
				Filepath = song.Filepath;
				Genre = song.Genre;
				Lyrics = song.Lyrics;
				MimeType = song.MimeType;
				ReleaseDate = song.ReleaseDate;
				Size = song.Size;
				Title = song.Title;
				TrackNumber = song.TrackNumber;
				Uri = song.Uri;
			}

			public string? Album { get; set; }
			public string? AlbumArtist { get; set; }
			public string? Artist { get; set; }
			public IImage? Artwork { get; set; }
			public int? Bitrate { get; set; }
			public AudioChannels? Channels { get; set; }
			public string? Copyright { get; set; }
			public DateTime? DateAdded { get; set; }
			public DateTime? DateModified { get; set; }
			public int? DiscNumber { get; set; }
			public TimeSpan? Duration { get; set; }
			public string? Filepath { get; set; }
			public string? Genre { get; set; }
			public string? Lyrics { get; set; }
			public MimeTypes? MimeType { get; set; }
			public DateTime? ReleaseDate { get; set; }
			public long? Size { get; set; }
			public string? Title { get; set; }
			public int? TrackNumber { get; set; }
			public Uri? Uri { get; set; }

			public override string ToString()
			{
				string basestring = base.ToString();

				_StringBuilder ??= new StringBuilder();

				return _StringBuilder.Clear()
					.Append(basestring)
					.AppendFormat("{0}: {1} \n", nameof(Album), Album)
					.AppendFormat("{0}: {1} \n", nameof(AlbumArtist), AlbumArtist)
					.AppendFormat("{0}: {1} \n", nameof(Artist), Artist)
					.AppendFormat("{0}: {1} \n", nameof(Artwork), Artwork)
					.AppendFormat("{0}: {1} \n", nameof(Bitrate), Bitrate)
					.AppendFormat("{0}: {1} \n", nameof(Channels), Channels)
					.AppendFormat("{0}: {1} \n", nameof(Copyright), Copyright)
					.AppendFormat("{0}: {1} \n", nameof(DateAdded), DateAdded)
					.AppendFormat("{0}: {1} \n", nameof(DateModified), DateModified)
					.AppendFormat("{0}: {1} \n", nameof(DiscNumber), DiscNumber)
					.AppendFormat("{0}: {1} \n", nameof(Duration), Duration)
					.AppendFormat("{0}: {1} \n", nameof(Filepath), Filepath)
					.AppendFormat("{0}: {1} \n", nameof(Genre), Genre)
					.AppendFormat("{0}: {1} \n", nameof(Lyrics), Lyrics)
					.AppendFormat("{0}: {1} \n", nameof(MimeType), MimeType)
					.AppendFormat("{0}: {1} \n", nameof(ReleaseDate), ReleaseDate)
					.AppendFormat("{0}: {1} \n", nameof(Size), Size)
					.AppendFormat("{0}: {1} \n", nameof(Title), Title)
					.AppendFormat("{0}: {1} \n", nameof(TrackNumber), TrackNumber)
					.AppendFormat("{0}: {1} \n", nameof(Uri), Uri)
					.ToString();
			}
		}
		public new class Default<T> : IModel.Default<T>, ISong<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) 
			{
				Album = defaultvalue;
				AlbumArtist = defaultvalue;
				Artist = defaultvalue;
				Bitrate = defaultvalue;
				Channels = defaultvalue;
				Copyright = defaultvalue;
				DateAdded = defaultvalue;
				DateModified = defaultvalue;
				DiscNumber = defaultvalue;
				Duration = defaultvalue;
				Filepath = defaultvalue;
				Genre = defaultvalue;
				Lyrics = defaultvalue;
				MimeType = defaultvalue;
				ReleaseDate = defaultvalue;
				Size = defaultvalue;
				Title = defaultvalue;
				TrackNumber = defaultvalue;
				Uri = defaultvalue;
			}

			public T Album { get; set; }
			public T AlbumArtist { get; set; }
			public T Artist { get; set; }
			public IImage<T>? Artwork { get; set; }
			public T Bitrate { get; set; }
			public T Channels { get; set; }
			public T Copyright { get; set; }
			public T DateAdded { get; set; }
			public T DateModified { get; set; }
			public T DiscNumber { get; set; }
			public T Duration { get; set; }
			public T Filepath { get; set; }
			public T Genre { get; set; }
			public T Lyrics { get; set; }
			public T MimeType { get; set; }
			public T ReleaseDate { get; set; }
			public T Size { get; set; }
			public T Title { get; set; }
			public T TrackNumber { get; set; }
			public T Uri { get; set; }
		}
	}
}
