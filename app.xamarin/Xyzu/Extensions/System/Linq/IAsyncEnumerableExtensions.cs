using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
	public static class IAsyncEnumerableExtensions
	{
		public static async Task<int?> IndexAsync<T>(this IAsyncEnumerable<T> asyncenumerable, T? value, CancellationToken cancellationToken = default)
		{
			if (value is null)
				return null;

			int index = 0;

			await foreach (T t in asyncenumerable.WithCancellation(cancellationToken))
				if (Equals(t, value))
					return index;
				else index++;

			return null;
		}
		public static async Task<int?> IndexAsync<T>(this IAsyncEnumerable<T> asyncenumerable, Func<T, bool> comparer, CancellationToken cancellationToken = default)
		{
			int index = 0;

			await foreach (T t in asyncenumerable.WithCancellation(cancellationToken))
				if (comparer.Invoke(t))
					return index;
				else index++;

			return null;
		}	
		public static async Task<int?> IndexAsync<T>(this IAsyncEnumerable<T> asyncenumerable, T? value, IEqualityComparer<T> comparer, CancellationToken cancellationToken = default)
		{
			if (value is null)
				return null;

			int index = 0;

			await foreach (T t in asyncenumerable.WithCancellation(cancellationToken))
				if (comparer.Equals(t, value))
					return index;
				else index++;

			return null;
		}

		public static IAsyncEnumerable<T> Insert<T>(this IAsyncEnumerable<T> asyncenumerable, int index, T value)
		{
			switch(true)
			{
				case true when index <= 0:
					return asyncenumerable.Prepend(value);

				default:
					IAsyncEnumerable<T> before = asyncenumerable.TakeWhile((detail, position) => position > index);
					IAsyncEnumerable<T> after = asyncenumerable.SkipWhile((detail, position) => position <= index);
					return before
						.Append(value)
						.Concat(after);
			}
		}

		public static async Task<T?> Random<T>(this IAsyncEnumerable<T> asyncenumerable, Random? random = null, CancellationToken cancellationToken = default, params int[] notvalues)
		{
			int maxindex = await asyncenumerable.CountAsync(cancellationToken) - 1;

			random ??= new Random();

			while (random.Next(0, maxindex) is int randomnumber)
				if (notvalues.Contains(randomnumber) is false)
					return await asyncenumerable.ElementAtAsync(randomnumber, cancellationToken);
				else break;

			return default;
		}

		public static async ValueTask<ObservableList<T>> ToObservableList<T>(this IAsyncEnumerable<T> asyncenumerable, CancellationToken cancellationToken = default)
		{
			ObservableList<T> observablelist = new ();

			await foreach (T t in asyncenumerable.WithCancellation(cancellationToken))
				observablelist.Add(t);

			return observablelist;
		}		   

		public static async Task<TimeSpan?> Sum(this IAsyncEnumerable<TimeSpan?> asyncenumerable, CancellationToken cancellationToken = default)
		{
			double timespansum = 0;

			await foreach (TimeSpan? timespan in asyncenumerable.WithCancellation(cancellationToken))
				if (timespan.HasValue)
					timespansum += timespan.Value.TotalMilliseconds;

			return TimeSpan.FromMilliseconds(timespansum);
		}
	}
}
