using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xyzu.Images;

namespace Xyzu.Library.Models
{
	public interface IArtist<T> : IModel<T>
	{
		T AlbumIds { get; set; }
		IImage<T>? Image { get; set; }
		T Name { get; set; }
		T SongIds { get; set; }
	}
	public partial interface IArtist : IModel
{
		IEnumerable<string> AlbumIds { get; set; }
		IImage? Image { get; set; }
		string? Name { get; set; }
		IEnumerable<string> SongIds { get; set; }

		public new class Default : IModel.Default, IArtist 
		{
			public Default(string id) : base(id) { }
			public Default(IArtist artist) : base(artist)
			{
				AlbumIds = artist.AlbumIds;
				Image = artist.Image;
				Name = artist.Name;
				SongIds = artist.SongIds;
			}

			public IEnumerable<string> AlbumIds { get; set; } = Enumerable.Empty<string>();
			public IImage? Image { get; set; }
			public string? Name { get; set; }
			public IEnumerable<string> SongIds { get; set; } = Enumerable.Empty<string>();

			public override string ToString()
			{
				string basestring = base.ToString();

				_StringBuilder ??= new StringBuilder();

				return _StringBuilder.Clear()
					.Append(basestring)
					.AppendFormat("{0}: {1} \n", nameof(AlbumIds), string.Join(",", AlbumIds))
					.AppendFormat("{0}: {1} \n", nameof(Name), Name)
					.AppendFormat("{0}: {1} \n", nameof(SongIds), string.Join("," ,SongIds))
					.ToString();
			}
		}
		public new class Default<T> : IModel.Default<T>, IArtist<T>
		{
			public Default(T defaultvalue) : base(defaultvalue) 
			{
				AlbumIds = defaultvalue;
				Name = defaultvalue;
				SongIds = defaultvalue;
			}

			public T AlbumIds { get; set; }
			public IImage<T>? Image { get; set; }
			public T Name { get; set; }
			public T SongIds { get; set; }
		}
	}
}
