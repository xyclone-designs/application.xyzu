using System;
using System.Collections.Generic;

namespace Xyzu.Library.Models
{
	public partial interface IPlaylist : IModel
	{
		public void Populate(IPlaylist playlist)
		{
			DateCreated = playlist.DateCreated;
			DateModified ??= playlist.DateModified;
			Duration = playlist.Duration;
			Name ??= playlist.Name;
			SongIds ??= playlist.SongIds;
		}
		public IPlaylist<bool>? Distinct(IPlaylist compareto, IPlaylist<bool>? fieldtocompare = null)
		{
			bool distinctdatecreated = (DateCreated != compareto.DateCreated);
			bool distinctdatemodified = (DateModified is not null || compareto.DateModified is not null) && (DateModified != compareto.DateModified);
			bool distinctduration = (Duration != compareto.Duration);
			bool distinctname = (Name is not null || compareto.Name is not null) && !string.Equals(Name, compareto.Name);
			bool distinctrating = (Rating != compareto.Rating);

			bool distinct =
				distinctdatecreated ||
				distinctdatemodified ||
				distinctduration ||
				distinctname ||
				distinctrating;

			if (distinct is false)
				return null;

			return new Default<bool>(false)
			{
				DateCreated = distinctdatecreated,
				DateModified = distinctdatemodified,
				Duration = distinctduration,
				Name = distinctname,
				Rating = distinctrating,
			};
		}
	}
}
