using System;

namespace Android.Content
{
	public class SharedPreferencesOnSharedPreferenceChangeListener : Java.Lang.Object, ISharedPreferencesOnSharedPreferenceChangeListener
	{
		public SharedPreferencesOnSharedPreferenceChangeListener() { }
		public SharedPreferencesOnSharedPreferenceChangeListener(Action<ISharedPreferences?, string?> onsharedpreferencechangedaction)
		{
			OnSharedPreferenceChangedAction = onsharedpreferencechangedaction;
		}

		public Action<ISharedPreferences?, string?>? OnSharedPreferenceChangedAction { get; set; }

		public void OnSharedPreferenceChanged(ISharedPreferences? sharedPreferences, string? key)
		{
			OnSharedPreferenceChangedAction?.Invoke(sharedPreferences, key);
		}
	}
}