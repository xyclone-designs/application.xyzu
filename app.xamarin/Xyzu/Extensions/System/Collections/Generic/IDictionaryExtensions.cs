
namespace System.Collections.Generic
{
	public static class IDictionaryExtensions
	{
		public static void AddOrReplace<TKey, TClassValue>(this IDictionary<TKey, TClassValue> dictionary, TKey key, TClassValue? value) where TClassValue : class
		{
			if (value is null)
				return;

			if (dictionary.ContainsKey(key))
				dictionary.Remove(key);

			dictionary.Add(key, value);
		}											   
		public static void AddOrReplace<TKey, TStructValue>(this IDictionary<TKey, TStructValue> dictionary, TKey key, TStructValue? value) where TStructValue : struct
		{
			if (value is null)
				return;

			if (dictionary.ContainsKey(key))
				dictionary.Remove(key);

			dictionary.Add(key, value.Value);
		}										      
		public static void AddReplaceOrRemove<TKey, TClassValue>(this IDictionary<TKey, TClassValue> dictionary, TKey key, TClassValue? value) where TClassValue : class
		{
			if (dictionary.ContainsKey(key))
				dictionary.Remove(key);

			if (value != null)
				dictionary.Add(key, value);
		}											   
		public static void AddReplaceOrRemove<TKey, TStructValue>(this IDictionary<TKey, TStructValue> dictionary, TKey key, TStructValue? value) where TStructValue : struct
		{
			if (dictionary.ContainsKey(key))
				dictionary.Remove(key);

			if (value != null)
				dictionary.Add(key, value.Value);
		}
		public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary.TryGetValue(key, out TValue _out))
				return _out;

			return default;
		}
	}
}
