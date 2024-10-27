using System;

namespace Org.Json
{
	public static class JSONObjectExtensions
	{
		public static DateTime? OptDateTime(this JSONObject jsonobject, string? name)
		{
			if (DateTime.TryParse(jsonobject.OptString(name), out DateTime result))
				return result;

			return new DateTime?();
		}
		public static int? OptIntDenominator(this JSONObject jsonobject, string? name)
		{
			if (jsonobject.OptString(name)?.Split('/') is not string[] fraction || fraction.Length is not 2)
				return new int?();

			if (int.TryParse(fraction[0], out int denominator))
				return denominator;

			return new int?();
		}
		public static int? OptIntNumerator(this JSONObject jsonobject, string? name)
		{
			if (jsonobject.OptString(name)?.Split('/') is not string[] fraction || fraction.Length is not 2)
				return new int?();

			if (int.TryParse(fraction[0], out int numerator))
				return numerator;

			return new int?();
		}
	}
}
