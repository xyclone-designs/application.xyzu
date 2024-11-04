using SQLite;

using System;

using Xyzu.Images;

namespace Xyzu.Library.Models
{
	public class SongEntity : ISong.Default
	{
		public SongEntity() : this(Guid.NewGuid().ToString()) { }
		public SongEntity(string id) : base(id) { }
		public SongEntity(ISong song) : base(song) { }

		[PrimaryKey]
		public new string Id
		{
			get => base.Id;
			set => base.Id = value;
		}

		public new int Malformed
		{
			get => base.Malformed ? 1 : 0;
			set => base.Malformed = value is 1;
		}
		[Ignore]
		public new IImage? Artwork
		{
			get => base.Artwork;
			set => base.Artwork = value;
		}

		public Uri? ArtworkUri
		{
			get => base.Artwork?.Uri;
			set => (base.Artwork ??= new IImage.Default()).Uri = value;
		}
		public string? ArtworkFilepath
		{
			get => base.Artwork?.Filepath;
			set => (base.Artwork ??= new IImage.Default()).Filepath = value;
		}
		public string? ArtworkBufferKey
		{
			get => base.Artwork?.BufferKey;
			set => (base.Artwork ??= new IImage.Default()).BufferKey = value;
		}
	}
}