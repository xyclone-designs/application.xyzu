using SQLite;

using System;

namespace Xyzu.Library.Models
{
	public class PlaylistEntity : IPlaylist.Default
	{
		public PlaylistEntity() : this(Guid.NewGuid().ToString()) { }
		public PlaylistEntity(string id) : base(id) { }
		public PlaylistEntity(IPlaylist playlist) : base(playlist) { }

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