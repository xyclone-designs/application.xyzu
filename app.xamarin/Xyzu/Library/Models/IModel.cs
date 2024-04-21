using System.Text;

using Xyzu.Library.Enums;

namespace Xyzu.Library.Models
{
	public interface IModel<T> 
	{
		T Id { get; set; }
		T Rating { get; set; }
	}
	public interface IModelTyped<T> : IModel<T> { }
	public partial interface IModel
	{
		string Id { get; }
		Ratings Rating { get; set; }

		public class Default : IModel
		{
			public Default(string id)
			{
				Id = id;
			}
			public Default(IModel model) : this(model.Id)
			{ }

			public string Id { get; protected set; }
			public Ratings Rating { get; set; }

			protected StringBuilder? _StringBuilder { get; set; }

			public override string ToString()
			{
				string basestring = base.ToString();

				_StringBuilder ??= new StringBuilder();

				return _StringBuilder.Clear()
					.Append(basestring)
					.AppendFormat("{0}: {1} \n", nameof(Id), Id)
					.AppendFormat("{0}: {1} \n", nameof(Rating), Rating)
					.ToString();
			}
		}
		public class Default<T> : IModel<T>
		{
			public Default(T defaultvalue) 
			{
				Id = defaultvalue;
				Rating = defaultvalue;
			}

			public T Id { get; set; }
			public T Rating { get; set; }
		}
	}
}
