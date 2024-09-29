using SQLite;

using System;

namespace Xyzu.Library.Models
{
	public class GenreEntity : IGenre.Default
	{
		public GenreEntity() : this(Guid.NewGuid().ToString()) { }
		public GenreEntity(string id) : base(id) { }
		public GenreEntity(IGenre genre) : base(genre) { }

		[PrimaryKey]
		public new string Id
		{
			get => base.Id;
			set => base.Id = value;
		}

		public new string SongIds
		{
			get => EntityUtils.IdsToString(base.SongIds);
			set => base.SongIds = EntityUtils.StringToIds(value);
		}
	}
}