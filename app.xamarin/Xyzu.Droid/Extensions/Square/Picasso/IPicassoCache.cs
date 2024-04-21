#nullable enable

using Android.Graphics;

using System.Collections.Generic;

namespace Square.Picasso
{
	public interface IPicassoCache : ICache
	{
		bool Contains(string key);

		public class Default : Java.Lang.Object, IPicassoCache
		{
			public Default()
			{
				Cache = new Dictionary<string, Bitmap>();
			}

			IDictionary<string, Bitmap> Cache { get; set; }

			public void Clear()
			{
				Cache?.Clear();
			}
			public bool Contains(string key)
			{
				return Cache.ContainsKey(key);
			}
			public void ClearKeyUri(string key)
			{
				if (Cache.ContainsKey(key))
					Cache.Remove(key);
			}
			public Bitmap? Get(string keyPrefix)
			{
				return Cache.TryGetValue(keyPrefix, out Bitmap bitmap) ? bitmap : null;
			}
			public int MaxSize()
			{
				return Cache.Count + 10;
			}
			public void Set(string key, Bitmap bitmap)
			{
				if (Cache.ContainsKey(key))
					Cache.Remove(key);

				Cache.Add(key, bitmap);
			}
			public int Size()
			{
				return Cache.Count;
			}
		}
	}
}