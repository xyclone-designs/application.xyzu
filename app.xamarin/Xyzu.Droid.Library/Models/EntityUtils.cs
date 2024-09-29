using System.Collections.Generic;

namespace Xyzu.Library.Models
{
	public class EntityUtils
	{
		private const string Delim = ";";

		public static string IdsToString(IEnumerable<string> ids)
		{
			return string.Join(Delim, ids);
		}
		public static IEnumerable<string> StringToIds(string ids)
		{
			return ids.Split(Delim);
		}
	}
}