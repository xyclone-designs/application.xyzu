using SQLite;

using System;

using Xyzu.Images;

namespace Xyzu.Library.Models
{
	public class AlbumEntity : IAlbum.Default
	{
		public AlbumEntity() : this(Guid.NewGuid().ToString()) { }
		public AlbumEntity(string id) : base(id) { }
		public AlbumEntity(IAlbum album) : base(album) { }

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

		public new string SongIds
		{
			get => EntityUtils.IdsToString(base.SongIds);
			set => base.SongIds = EntityUtils.StringToIds(value);
		}

		[Ignore]
		public new IImage? Artwork
		{
			get => base.Artwork;
			set => base.Artwork = value;
		}
	}
}