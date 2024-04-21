#nullable enable

using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Preference;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Activities;
using Xyzu.Settings;

using AndroidXPreference = AndroidX.Preference.Preference;

namespace Xyzu.Fragments.Settings
{
	public abstract class BasePreferenceFragment : PreferenceFragmentCompat, ISettings, AndroidXPreference.IOnPreferenceChangeListener
	{
		public SettingsActivity? AppCompatActivity
		{
			get => Activity as SettingsActivity;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
		{
			View? view = base.OnCreateView(inflater, container, savedInstanceState);

			if (Context?.Resources?.GetColor(Resource.Color.ColorBackground, Context.Theme) is Color bakgroundcolor)
				view?.SetBackgroundColor(bakgroundcolor);

			return view;
		}

		public virtual void SetFromKey(string key, object? value) 
		{ }
		public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		public virtual bool OnPreferenceChange(AndroidXPreference preference, Java.Lang.Object? newValue) 
		{ 
			return false; 
		}

		protected virtual void InitPreferences(params AndroidXPreference?[] preferences)
		{
			foreach (AndroidXPreference? preference in preferences)
				if (preference != null)
					switch(true)
					{
						case true when
						preference is PreferenceScreen preferencescreen:
							preferencescreen.LayoutResource = Resource.Layout.xyzu_preference_preferencescreen;
							break;

						default: break;
					}
		}
		protected virtual void AddPreferenceChangeHandler(params AndroidXPreference?[] preferences)
		{
			foreach (AndroidXPreference? preference in preferences)
				if (preference != null)
					preference.OnPreferenceChangeListener = this;
		}			
		protected virtual void RemovePreferenceChangeHandler(params AndroidXPreference?[] preferences)
		{
			foreach (AndroidXPreference? preference in preferences)
				if (preference != null)
					preference.OnPreferenceChangeListener = null;
		}
	}
}