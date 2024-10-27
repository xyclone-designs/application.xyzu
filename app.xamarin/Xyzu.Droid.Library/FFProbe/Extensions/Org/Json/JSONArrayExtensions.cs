using System;
using System.Collections.Generic;

namespace Org.Json
{
	public static class JSONArrayExtensions
	{
		public static IEnumerable<T> AsEnumerable<T>(this JSONArray jsonarray, Func<JSONArray, int, T> action)
		{
			for (int index = 0, length = jsonarray.Length(); index < length; index++)
				yield return action.Invoke(jsonarray, index);
		}
	}
}
