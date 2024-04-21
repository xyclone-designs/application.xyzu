using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xyzu.Library.Models
{
	public interface IPlaylist<T> : IModel<T>
	{
		T DateCreated { get; set; }
		T DateModified { get; set; }
		T Duration { get; set; }
		T Name { get; set; }
		T SongIds { get; set; }
	}
	public partial interface IPlaylist : IModel
	{
		DateTime DateCreated { get; set; }
		DateTime? DateModified { get; set; }
		TimeSpan Duration { get; set; }
		string? Name { get; set; }
		IEnumerable<string> SongIds { get; set; }

		public new class Default : IModel.Default, IPlaylist
		{
			public Default(string id) : base(id) { }
			public Default(IPlaylist playlist) : base(playlist)
			{
				DateCreated = playlist.DateCreated;
				DateModified = playlist.DateModified;
				Duration = playlist.Duration;
				Name = playlist.Name;
				SongIds = playlist.SongIds;
			}

			public DateTime DateCreated { get; set; }
			public DateTime? DateModified { get; set; }
			public TimeSpan Duration { get; set; }
			public string? Name { get; set; }
			public IEnumerable<string> SongIds { get; set; } = Enumerable.Empty<string>();

			public override string ToString()
			{
				string basestring = base.ToString();

				_StringBuilder ??= new StringBuilder();

				return _StringBuilder.Clear()
					.Append(basestring)
					.AppendFormat("{0}: {1} \n", nameof(DateCreated), DateCreated)
					.AppendFormat("{0}: {1} \n", nameof(DateModified), DateModified)
					.AppendFormat("{0}: {1} \n", nameof(Duration), Duration)
					.AppendFormat("{0}: {1} \n", nameof(Name), Name)
					.AppendFormat("{0}: {1} \n", nameof(SongIds), string.Join(",", SongIds))
					.ToString();
			}
		}
		public new class Default<T> : IModel.Default<T>, IPlaylist<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) 
			{
				DateCreated = defaultvalue;
				DateModified = defaultvalue;
				Duration = defaultvalue;
				Name = defaultvalue;
				SongIds = defaultvalue;
			}

			public T DateCreated { get; set; }
			public T DateModified { get; set; }
			public T Duration { get; set; }
			public T Name { get; set; }
			public T SongIds { get; set; }
		}
	}
}
