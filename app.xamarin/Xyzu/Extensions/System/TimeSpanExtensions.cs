using System.Text;

namespace System
{
	public static class TimespanExtensions
	{
		private readonly static StringBuilder Builder = new();

		public static string ToMicrowaveFormat(this TimeSpan timespan, bool includemilliseconds = false, bool forceincludehours = false)
		{
			Builder.Clear();

			if (forceincludehours || timespan.TotalHours >= 1) Builder.AppendFormat("{0}:", timespan.Hours.ToString("00"));
			Builder.AppendFormat("{0}", timespan.Minutes.ToString("00"));
			Builder.Append(':');
			Builder.AppendFormat("{0}", timespan.Seconds.ToString("00"));

			if (includemilliseconds) Builder.AppendFormat(":{0}", timespan.Milliseconds.ToString("0000"));

			return Builder.ToString();
		}
	}
}
