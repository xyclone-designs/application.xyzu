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

		public Uri? ArtworkUri
		{
			get => base.Artwork?.Uri;
			set => (base.Artwork ??= new IImage.Default()).Uri = value;
		}
		public byte[]? ArtworkBufferHash
		{
			get => base.Artwork?.BufferHash;
			set => (base.Artwork ??= new IImage.Default()).BufferHash = value;
		}

		[Ignore]
		public new IImage? Artwork
		{
			get => base.Artwork;
			set => base.Artwork = value;
		}
	}
}