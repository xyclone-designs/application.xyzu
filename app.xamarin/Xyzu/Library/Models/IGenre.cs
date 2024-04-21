using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xyzu.Library.Models
{
	public interface IGenre<T> : IModel<T>
	{
		T Duration { get; set; }
		T Name { get; set; }
		T SongIds { get; set; }
	}
	public partial interface IGenre : IModel
	{
		TimeSpan Duration { get; set; }
		string? Name { get; set; }
		IEnumerable<string> SongIds { get; set; }

		public new class Default : IModel.Default, IGenre 
		{
			public Default(string id) : base(id) { }
			public Default(IGenre genre) : base(genre)
			{
				Duration = genre.Duration;
				Name = genre.Name;
				SongIds = genre.SongIds;
			}

			public TimeSpan Duration { get; set; }
			public string? Name { get; set; }
			public IEnumerable<string> SongIds { get; set; } = Enumerable.Empty<string>();

			public override string ToString()
			{
				string basestring = base.ToString();

				_StringBuilder ??= new StringBuilder();

				return _StringBuilder.Clear()
					.Append(basestring)
					.AppendFormat("{0}: {1} \n", nameof(Duration), Duration)
					.AppendFormat("{0}: {1} \n", nameof(Name), Name)
					.AppendFormat("{0}: {1} \n", nameof(SongIds), string.Join(",", SongIds))
					.ToString();
			}
		}
		public new class Default<T> : IModel.Default<T>, IGenre<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) 
			{
				Duration = defaultvalue;
				Name = defaultvalue;
				SongIds = defaultvalue;
			}

			public T Duration { get; set; }
			public T Name { get; set; }
			public T SongIds { get; set; }
		}
	}
}
