using SQLite;

using System;

using Xyzu.Images;

namespace Xyzu.Library.Models
{
	public class ArtistEntity : IArtist.Default
	{
		public ArtistEntity() : this(Guid.NewGuid().ToString()) { }
		public ArtistEntity(string id) : base(id) { }
		public ArtistEntity(IArtist artist) : base(artist) { }

		[PrimaryKey]
		public new string Id
		{
			get => base.Id;
			set => base.Id = value;
		}

		[Ignore]
		public new IImage? Image
		{
			get => base.Image;
			set => base.Image = value;
		}
		public Uri? ImageUri
		{
			get => base.Image?.Uri;
			set => (base.Image ??= new IImage.Default()).Uri = value;
		}
		public byte[]? ImageBufferHash
		{
			get => base.Image?.BufferHash;
			set => (base.Image ??= new IImage.Default()).BufferHash = value;
		}

		public new string AlbumIds
		{
			get => EntityUtils.IdsToString(base.AlbumIds);
			set => base.AlbumIds = EntityUtils.StringToIds(value);
		}
		public new string SongIds
		{
			get => EntityUtils.IdsToString(base.SongIds);
			set => base.SongIds = EntityUtils.StringToIds(value);
		}
	}
}