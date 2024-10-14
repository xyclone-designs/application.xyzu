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
		public string? ImageFilepath
		{
			get => base.Image?.Filepath;
			set => (base.Image ??= new IImage.Default()).Filepath = value;
		}
		public Uri? ImageUri
		{
			get => base.Image?.Uri;
			set => (base.Image ??= new IImage.Default()).Uri = value;
		}
		public string? ImageBufferKey
		{
			get => base.Image?.BufferKey;
			set => (base.Image ??= new IImage.Default()).BufferKey = value;
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