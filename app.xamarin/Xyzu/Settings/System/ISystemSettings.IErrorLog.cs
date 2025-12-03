using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xyzu.Settings.System
{
	public partial interface ISystemSettings : ISettings
	{
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
					return stringbuilder.AppendLine("Exception: null").ToString();

				Exception? exception = Exception;

				while (exception is not null)
				{
					stringbuilder
						.AppendFormat("Exception.Message: {0}", Exception.Message).AppendLine()
						.AppendFormat("Exception.Source: {0}", Exception.Source).AppendLine()
						.AppendFormat("Exception.StackTrace: {0}", Exception.StackTrace).AppendLine()
						.AppendFormat("Exception.TargetSite: {0}", Exception.TargetSite?.Name).AppendLine()
						.AppendLine("---------------------");

					exception = exception.InnerException;
				}

				return stringbuilder.ToString();
			}

			public class Default : IErrorLog
			{
				public Default(string id) : this(id, DateTime.Now) { }
				public Default(string id, DateTime date)
				{
					Id = id;
					Date = date;
				}
				
				public string Id { get; }
				public DateTime Date { get; }

				public string? Path { get; set; }
				public Exception? Exception { get; set; }
			}

			public static Default FromText(string text)
			{
				using StringReader stringreader = new (text);

				string id = stringreader.ReadLine()?[(IdPrefix.Length - 1) ..] ?? string.Empty;
				string date = stringreader.ReadLine()?[(DatePrefix.Length - 1) ..] ?? string.Empty;
				DateTime datetime = DateTime.TryParse(date, out DateTime _datetime) ? _datetime : DateTime.Now;

				return new Default(id, datetime);
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
