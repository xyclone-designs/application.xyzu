#nullable enable

using Android.App;
using Android.Content;
using Java.Interop;
using System;
using System.Collections.Generic;

namespace Xyzu
{
	public sealed partial class XyzuSettings : Java.Lang.Object, ISharedPreferences
	{
		private XyzuSettings(Context context) 
		{
			Context = context;
		}

		private static XyzuSettings? _Instance;
		public static XyzuSettings Instance
		{
			get => _Instance ?? throw new Exception("Instance is null. Init AppSettings before use");
		}

		public static void Init(Context context)
		{
			_Instance = new XyzuSettings(context) { };
		}

		public Context Context { get; set; }

		public const string SharedPreferencesName = "co.za.xyclonedesigns.xyzu";

		private ISharedPreferences? _SharedPreferences;
		public ISharedPreferences? SharedPreferences
		{
			get => _SharedPreferences ??= Context.GetSharedPreferences(Context.PackageName, FileCreationMode.Append);
		}

		public IDictionary<string, object>? All => SharedPreferences?.All;

		public ISharedPreferencesEditor? Edit()
		{
			return SharedPreferences?.Edit();
		}

		public bool Contains(string? key)
		{
			return SharedPreferences?.Contains(key) ?? false;
		}
		public bool GetBoolean(string? key, bool defValue)
		{
			return SharedPreferences?.GetBoolean(key, defValue) ?? defValue;
		}
		public float GetFloat(string? key, float defValue)
		{
			return SharedPreferences?.GetFloat(key, defValue) ?? defValue;
		}
		public int GetInt(string? key, int defValue)
		{
			return SharedPreferences?.GetInt(key, defValue) ?? defValue;
		}
		public long GetLong(string? key, long defValue)
		{
			return SharedPreferences?.GetLong(key, defValue) ?? defValue;
		}
		public string? GetString(string? key, string? defValue)
		{
			return SharedPreferences?.GetString(key, defValue) ?? defValue;
		}
		public ICollection<string>? GetStringSet(string? key, ICollection<string>? defValues)
		{
			return SharedPreferences?.GetStringSet(key, defValues) ?? defValues;
		}
		public void RegisterOnSharedPreferenceChangeListener(ISharedPreferencesOnSharedPreferenceChangeListener? listener)
		{
			SharedPreferences?.RegisterOnSharedPreferenceChangeListener(listener);
		}
		public void UnregisterOnSharedPreferenceChangeListener(ISharedPreferencesOnSharedPreferenceChangeListener? listener)
		{
			SharedPreferences?.UnregisterOnSharedPreferenceChangeListener(listener);
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