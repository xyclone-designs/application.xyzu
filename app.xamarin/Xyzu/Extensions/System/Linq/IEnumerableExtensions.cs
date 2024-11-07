using System.Collections.Generic;

namespace System.Linq
{
	public static class IEnumerableExtensions
	{
		public static int? Index<T>(this IEnumerable<T> enumerable, T? value)
		{
			if (value is null)
				return null;

			int index = 0;

			foreach (T t in enumerable)
				if (Equals(t, value))
					return index;
				else index++;

			return null;
		}
		public static int? Index<T>(this IEnumerable<T> enumerable, Func<T, bool> comparer)
		{
			IEnumerator<T> enumerator = enumerable.GetEnumerator();

			for (int index = 0; enumerator.MoveNext(); index++)
				if (comparer.Invoke(enumerator.Current))
					return index;

			return null;
		}	
		public static int? Index<T>(this IEnumerable<T> enumerable, T? value, IEqualityComparer<T> comparer)
		{
			if (value is null)
				return null;

			IEnumerator<T> enumerator = enumerable.GetEnumerator();

			for (int index = 0; enumerator.MoveNext(); index++)
				if (comparer.Equals(enumerator.Current, value))
					return index;

			return null;
		}

		public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T value)
		{
			switch(true)
			{
				case true when index <= 0:
					return enumerable.Prepend(value);

				default:
					IEnumerable<T> before = enumerable.TakeWhile((detail, position) => position > index);
					IEnumerable<T> after = enumerable.SkipWhile((detail, position) => position <= index);
					return before
						.Append(value)
						.Concat(after);
			}
		}

		public static T? Random<T>(this IEnumerable<T> enumerable, Random? random = null, params int[] notvalues)
		{
			int maxindex = enumerable.Count() - 1;

			random ??= new Random();

			while (random.Next(0, maxindex) is int randomnumber)
				if (notvalues.Contains(randomnumber) is false)
					return enumerable.ElementAt(randomnumber);
				else break;

			return default;
		}

		public static TimeSpan Sum(this IEnumerable<TimeSpan> enumerable)
		{
			double milliseconds = 0;
			TimeSpan sum = TimeSpan.Zero;

			foreach (TimeSpan timespan in enumerable)
				try { milliseconds += timespan.TotalMilliseconds; }
				catch (Exception) 
				{
					sum += TimeSpan.FromMilliseconds(milliseconds);
					milliseconds = timespan.TotalMilliseconds;
				}

			sum += TimeSpan.FromMilliseconds(milliseconds);

			return sum;
		}
		public static TimeSpan Sum<T>(this IEnumerable<T> enumerable, Func<T, TimeSpan> selector)
		{
			return enumerable
				.Select(selector)
				.Sum();
		}
	}
}
