#nullable enable

using Android.Content;

using System;
using System.Collections.Generic;

namespace Xyzu
{
	public sealed partial class XyzuSettings : Java.Lang.Object, ISharedPreferences
	{
		private XyzuSettings(Context context) 
		{
			Context = context;
			SharedPreferences = Context.GetSharedPreferences(Context.PackageName, FileCreationMode.Append) ?? 
				throw new ArgumentException("Could not create shared preerences");
		}

		private static XyzuSettings? _Instance;
		public static XyzuSettings Instance
		{
			get => _Instance ?? throw new Exception("Instance is null. Init AppSettings before use");
		}

		public static void Init(Context context, Action<XyzuSettings>? oninit)
		{
			_Instance = new XyzuSettings(context) { };
			oninit?.Invoke(_Instance);
		}

		private Context Context { get; }
		private ISharedPreferences SharedPreferences { get; }

		public IDictionary<string, object>? All => SharedPreferences.All;
		public bool Contains(string? key)
		{
			return SharedPreferences.Contains(key);
		}
		public ISharedPreferencesEditor? Edit()
		{
			return SharedPreferences.Edit();
		}
		public bool GetBoolean(string? key, bool defValue)
		{
			return SharedPreferences.GetBoolean(key, defValue);
		}
		public float GetFloat(string? key, float defValue)
		{
			return SharedPreferences.GetFloat(key, defValue);
		}
		public int GetInt(string? key, int defValue)
		{
			return SharedPreferences.GetInt(key, defValue);
		}
		public long GetLong(string? key, long defValue)
		{
			return SharedPreferences.GetLong(key, defValue);
		}
		public string? GetString(string? key, string? defValue)
		{
			return SharedPreferences.GetString(key, defValue);
		}
		public ICollection<string>? GetStringSet(string? key, ICollection<string>? defValues)
		{
			return SharedPreferences.GetStringSet(key, defValues);
		}
		public void RegisterOnSharedPreferenceChangeListener(ISharedPreferencesOnSharedPreferenceChangeListener? listener)
		{
			SharedPreferences.RegisterOnSharedPreferenceChangeListener(listener);
		}
		public void UnregisterOnSharedPreferenceChangeListener(ISharedPreferencesOnSharedPreferenceChangeListener? listener)
		{
			SharedPreferences.UnregisterOnSharedPreferenceChangeListener(listener);
		}


		public class OnSharedPreferenceChangeEventArgs : EventArgs
		{
			public OnSharedPreferenceChangeEventArgs(string key, object? value)
			{
				Key = key;
				Value = value;
			}

			public string Key { get; }
			public object? Value { get; }
		}
	}
}