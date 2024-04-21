using System;
using System.Collections.Generic;

namespace Xyzu.Library.Models
{
	public partial interface IGenre : IModel
	{
		public void Populate(IGenre genre)
		{
			Duration = genre.Duration;
			Name ??= genre.Name;
			SongIds ??= genre.SongIds;
		}
		public IGenre<bool>? Distinct(IGenre compareto, IGenre<bool>? fieldtocompare = null)
		{
			bool distinctname = (Name is not null || compareto.Name is not null) && !string.Equals(Name, compareto.Name);
			bool distinctrating = (Rating != compareto.Rating);

			bool distinct = 
				distinctname ||
				distinctrating;

			if (distinct is false)
				return null;

			return new Default<bool>(false) 
			{
				Name = distinctname,
				Rating = distinctrating,
			};
		}
	}
}
