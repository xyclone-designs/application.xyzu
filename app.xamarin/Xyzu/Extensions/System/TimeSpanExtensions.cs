using System.Text;

namespace System
{
	public static class TimespanExtensions
	{
		public static string ToMicrowaveFormat(this TimeSpan timespan, bool includemilliseconds = false, bool forceincludehours = false)
		{
			StringBuilder builder = new();

			if (forceincludehours || timespan.TotalHours >= 1) builder.AppendFormat("{0}:", timespan.Hours.ToString("00"));

			builder.AppendFormat("{0}", timespan.Minutes.ToString("00"));
			builder.Append(':');
			builder.AppendFormat("{0}", timespan.Seconds.ToString("00"));

			if (includemilliseconds) builder.AppendFormat(":{0}", timespan.Milliseconds.ToString("0000"));

			return builder.ToString();
		}
	}
}
