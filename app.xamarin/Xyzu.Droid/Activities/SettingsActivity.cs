using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

using Xyzu.Droid;
using Xyzu.Views.Toolbar;

namespace Xyzu.Activities
{
    [Activity(
		Theme = "@style/SettingsTheme",
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
	public class SettingsActivity : BaseActivity
	{
		public static class Intents
		{
			public static class ExtraKeys
			{
				public const string FragmentName = "SettingsActivity.Intents.Keys.FragmentName";
			}
		}

		protected ToolbarSettingsView? _ToolbarSettings;

		public ToolbarSettingsView ToolbarSettings
		{
			get => _ToolbarSettings ??=
				FindViewById(Resource.Id.xyzu_layout_settings_toolbarsettingsview) as ToolbarSettingsView ??
				throw new InflateException();
		}

		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.xyzu_layout_settings);			
			SetSupportActionBar(ActivityToolbar = ToolbarSettings.Toolbar);
			SupportActionBar?.SetHomeButtonEnabled(true);
			SupportActionBar?.SetDisplayShowHomeEnabled(true);
			SupportActionBar?.SetDisplayHomeAsUpEnabled(true);
		}
		protected override void OnResume()
		{
			base.OnResume();

			SupportFragmentManager?
				.BeginTransaction()
				.Replace(Resource.Id.xyzu_layout_settings_framelayout, Intent?.GetStringExtra(Intents.ExtraKeys.FragmentName) switch
				{
					Fragments.Settings.About.AboutPreferenceFragment.FragmentName => new Fragments.Settings.About.AboutPreferenceFragment { },

					Fragments.Settings.Files.FilesPreferenceFragment.FragmentName => new Fragments.Settings.Files.FilesPreferenceFragment { },

					Fragments.Settings.LockScreen.LockScreenPreferenceFragment.FragmentName => new Fragments.Settings.LockScreen.LockScreenPreferenceFragment { },

					Fragments.Settings.Notification.NotificationPreferenceFragment.FragmentName => new Fragments.Settings.Notification.NotificationPreferenceFragment { },

					Fragments.Settings.System.SystemPreferenceFragment.FragmentName => new Fragments.Settings.System.SystemPreferenceFragment{ },

					Fragments.Settings.UserInterface.LanguagesPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.LanguagesPreferenceFragment { },
					Fragments.Settings.UserInterface.NowPlayingPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.NowPlayingPreferenceFragment { },
					Fragments.Settings.UserInterface.ThemesPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.ThemesPreferenceFragment { },
					Fragments.Settings.UserInterface.UserInterfacePreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.UserInterfacePreferenceFragment { },

					Fragments.Settings.UserInterface.Library.AlbumPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.AlbumPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.AlbumsPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.AlbumsPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.ArtistPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.ArtistPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.ArtistsPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.ArtistsPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.GenrePreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.GenrePreferenceFragment { },
					Fragments.Settings.UserInterface.Library.GenresPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.GenresPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.LibraryPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.LibraryPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.PlaylistPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.PlaylistPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.PlaylistsPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.PlaylistsPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.QueuePreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.QueuePreferenceFragment { },
					Fragments.Settings.UserInterface.Library.SearchPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.SearchPreferenceFragment { },
					Fragments.Settings.UserInterface.Library.SongsPreferenceFragment.FragmentName => new Fragments.Settings.UserInterface.Library.SongsPreferenceFragment { },

					_ => new Fragments.Settings.SettingsPreferenceFragment { }

				}).Commit();
		}

		public override void OnBackPressed()
		{
			if (SupportFragmentManager.BackStackEntryCount == 0)
				StartActivity(XyzuSettings.Utils.MainActivityIntent(this, null));
			else 
				base.OnBackPressed();
		}
	}
}