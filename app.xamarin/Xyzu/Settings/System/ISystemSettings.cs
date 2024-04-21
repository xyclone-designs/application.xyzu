using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xyzu.Settings.System
{
	public interface ISystemSettings<T> : ISettings<T>
	{
		T ErrorLogs { get; set; }
	}
	public interface ISystemSettings : ISettings
	{
		IEnumerable<IErrorLog> ErrorLogs { get; set; }

		public new class Keys : ISettings.Keys
		{
			public new const string Base = ISettings.Keys.Base + "." + nameof(ISystemSettings);
		}

		public new class Default : ISettings.Default, ISystemSettings
		{
			public Default() : base()
			{
				ErrorLogs = Enumerable.Empty<IErrorLog>();
			}

			public IEnumerable<IErrorLog> ErrorLogs { get; set; }
		}
		public new class Default<T> : ISettings.Default<T>, ISystemSettings<T>
		{
			public Default(T defaultvalue) : base(defaultvalue)
			{
				ErrorLogs = defaultvalue;
			}

			public T ErrorLogs { get; set; }
		}

		public interface IErrorLog
		{
			private const string IdPrefix = "Id: ";
			private const string DatePrefix = "Date: ";

			string Id { get; }
			DateTime Date { get; }
			string? Path { get; set; }
			Exception? Exception { get; set; }

			string FileName
			{
				get => string.Format("{0}, {1}.txt", Date.ToString("yyyy-MM-dd, HH-mm"), Id);
			}

			public string AsText()
			{
				StringBuilder stringbuilder = new StringBuilder()
					.AppendFormat("{0}{1}", IdPrefix, Id).AppendLine()
					.AppendFormat("{0}{1}", DatePrefix, Date).AppendLine();

				if (Exception is null)
					stringbuilder.AppendLine("Exception: null");
				else
					stringbuilder
						.AppendFormat("Exception.Message: {0}", Exception.Message).AppendLine()
						.AppendFormat("Exception.Source: {0}", Exception.Source).AppendLine()
						.AppendFormat("Exception.StackTrace: {0}", Exception.StackTrace).AppendLine()
						.AppendFormat("Exception.TargetSite: {0}", Exception.TargetSite.Name).AppendLine();

				return stringbuilder.ToString();
			}

			public class Default : IErrorLog
			{
				public Default(string id) : this(id, DateTime.Now) { }
				public Default(string id, DateTime date)
				{
					Id = id;
					Date = DateTime.Now;
				}
				
				public string Id { get; }
				public DateTime Date { get; }

				public string? Path { get; set; }
				public Exception? Exception { get; set; }
			}

			public static Default FromText(string text)
			{
				StringReader stringreader = new StringReader(text);

				string id = stringreader.ReadLine() [(IdPrefix.Length - 1) ..];
				string date = stringreader.ReadLine() [(DatePrefix.Length - 1) ..];

				return new Default(id, DateTime.Parse(date));
			}
			public static Default FromException(Exception exception)
			{
				return new Default(Guid.NewGuid().ToString())
				{
					Exception = exception
				};
			}
		}
	}
}
