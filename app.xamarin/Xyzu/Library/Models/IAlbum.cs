using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xyzu.Images;

namespace Xyzu.Library.Models
{
	public interface IAlbum<T> : IModel<T>
	{
		T ArtistId { get; set; }
		T Artist { get; set; }
		IImage<T>? Artwork { get; set; }
		T DiscCount { get; set; }
		T Duration { get; set; }
		T ReleaseDate { get; set; }
		T SongIds { get; set; }
		T Title { get; set; }
	}
	public partial interface IAlbum : IModel
	{
		string? ArtistId { get; set; }
		string? Artist { get; set; }
		IImage? Artwork { get; set; }
		int? DiscCount { get; set; }
		TimeSpan Duration { get; set; }
		DateTime? ReleaseDate { get; set; }
		IEnumerable<string> SongIds { get; set; }
		string? Title { get; set; }

		public new class Default : IModel.Default, IAlbum 
		{
			public Default(string id) : base(id) { }
			public Default(IAlbum album) : base(album)
			{
				ArtistId = album.ArtistId;
				Artist = album.Artist;
				Artwork = album.Artwork;
				DiscCount = album.DiscCount;
				Duration = album.Duration;
				ReleaseDate = album.ReleaseDate;
				SongIds = album.SongIds;
				Title = album.Title;
			}

			public string? ArtistId { get; set; }
			public string? Artist { get; set; }
			public IImage? Artwork { get; set; }
			public int? DiscCount { get; set; }
			public TimeSpan Duration { get; set; }
			public DateTime? ReleaseDate { get; set; }
			public IEnumerable<string> SongIds { get; set; } = Enumerable.Empty<string>();
			public string? Title { get; set; }

			public override string ToString()
			{
				string basestring = base.ToString();

				_StringBuilder ??= new StringBuilder();

				return _StringBuilder.Clear()
					.Append(basestring)
					.AppendFormat("{0}: {1} \n", nameof(ArtistId), ArtistId)
					.AppendFormat("{0}: {1} \n", nameof(Artist), Artist)
					.AppendFormat("{0}: {1} \n", nameof(DiscCount), DiscCount)
					.AppendFormat("{0}: {1} \n", nameof(Duration), Duration)
					.AppendFormat("{0}: {1} \n", nameof(ReleaseDate), ReleaseDate)
					.AppendFormat("{0}: {1} \n", nameof(SongIds), string.Join(",", SongIds))
					.AppendFormat("{0}: {1} \n", nameof(Title), Title)
					.ToString();
			}
		}
		public new class Default<T> : IModel.Default<T>, IAlbum<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) 
			{
				ArtistId = defaultvalue;
				Artist = defaultvalue;
				DiscCount = defaultvalue;
				Duration = defaultvalue;
				ReleaseDate = defaultvalue;
				SongIds = defaultvalue;
				Title = defaultvalue;
			}

			public T ArtistId { get; set; }
			public T Artist { get; set; }
			public IImage<T>? Artwork { get; set; }
			public T DiscCount { get; set; }
			public T Duration { get; set; }
			public T ReleaseDate { get; set; }
			public T SongIds { get; set; }
			public T Title { get; set; }
		}
	}
}
