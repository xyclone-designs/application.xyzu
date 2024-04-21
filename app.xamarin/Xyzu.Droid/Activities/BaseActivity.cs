#nullable enable

using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Settings.System;

using AndroidEnvironment = Android.OS.Environment;

namespace Xyzu.Activities
{
	public partial class BaseActivity : AppCompatActivity, Toolbar.IOnMenuItemClickListener, IMenuItemOnMenuItemClickListener
	{
		public BaseActivity() : base() { }

		public View? RootView { get; protected set; }
		public Toolbar? ActivityToolbar { get; protected set; }
		public CancellationTokenSource? ActivityCancellationTokenSource { get; protected set; }

		protected Configuration? NewConfiguration { get; set; }
		protected Configuration? OldConfiguration { get; set; }

		protected override void OnCreate(Bundle? savedInstaneState)
		{
			Window?.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

			base.OnCreate(savedInstaneState);

			AppDomain.CurrentDomain.UnhandledException += ISystemSettingsDroid.OnUnhandledException;
			TaskScheduler.UnobservedTaskException += ISystemSettingsDroid.OnUnobservedTaskException;

			ResultLauncher = RegisterForActivityResult(ResultContract, ResultCallback);
		}
		protected override void OnStart()
		{
			base.OnStart();

			XyzuPlayer.Instance.BindPlayer();
		}
		protected override void OnResume()
		{
			base.OnResume();

			SupportFragmentManager.BackStackChanged += SupportFragmentManagerBackStackChanged;
			SupportFragmentManager.FragmentOnAttach += SupportFragmentManagerFragmentOnAttach;

			XyzuLibrary.Instance.OnServiceConnectionChanged += XyzuLibraryOnServiceConnectionChanged;
			XyzuPlayer.Instance.OnServiceConnectionChanged += XyzuPlayerOnServiceConnectionChanged;

			XyzuLibraryOnServiceConnectionChanged(this, new ServiceConnectionChangedEventArgs(XyzuPlayer.Instance.ServiceConnectionState));
			XyzuPlayerOnServiceConnectionChanged(this, new ServiceConnectionChangedEventArgs(XyzuPlayer.Instance.ServiceConnectionState));
		}
		protected override void OnPause()
		{
			base.OnPause();

			SupportFragmentManager.BackStackChanged -= SupportFragmentManagerBackStackChanged;
			SupportFragmentManager.FragmentOnAttach -= SupportFragmentManagerFragmentOnAttach;

			XyzuLibrary.Instance.OnServiceConnectionChanged -= XyzuLibraryOnServiceConnectionChanged;
			XyzuPlayer.Instance.OnServiceConnectionChanged -= XyzuPlayerOnServiceConnectionChanged;
		}
		protected override void OnStop()
		{
			base.OnStop();

			XyzuPlayer.Instance.UnbindPlayer();
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			AppDomain.CurrentDomain.UnhandledException -= ISystemSettingsDroid.OnUnhandledException;
			TaskScheduler.UnobservedTaskException -= ISystemSettingsDroid.OnUnobservedTaskException; 
		}

		public override void OnConfigurationChanged(Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);

			if (OldConfiguration is null)
			{
				OnLocaleChanged(newConfig);
				OnOrientationChanged(newConfig);
				OnScreenSizeChanged(newConfig);
				OnUiModeChanged(newConfig);
			}
			else
			{
				if (newConfig.Locales != OldConfiguration.Locales)
					OnLocaleChanged(newConfig);
								   
				if (newConfig.Orientation != OldConfiguration.Orientation)
					OnOrientationChanged(newConfig);
								   
				if (newConfig.ScreenWidthDp != OldConfiguration.ScreenWidthDp || newConfig.ScreenHeightDp != OldConfiguration.ScreenHeightDp)
					OnScreenSizeChanged(newConfig);

				if (newConfig.UiMode != OldConfiguration.UiMode)
					OnUiModeChanged(newConfig);
			}

			OldConfiguration = NewConfiguration;
			NewConfiguration = newConfig;
		}
		public override bool OnOptionsItemSelected(IMenuItem menuitem)
		{
			switch (menuitem.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					return true;

				default: return base.OnOptionsItemSelected(menuitem);
			}
		}

		public virtual bool OnMenuItemClick(IMenuItem? menuitem) 
		{
			if (menuitem is null)
				return false;

			return OnOptionsItemSelected(menuitem);
		}

		protected virtual void OnLocaleChanged(Configuration newConfig) 
		{ 
			OnContentChanged();
		}
		protected virtual void OnOrientationChanged(Configuration newConfig)
		{
			OnContentChanged();
		}
		protected virtual void OnScreenSizeChanged(Configuration newConfig)
		{
			OnContentChanged();
		}
		protected virtual void OnUiModeChanged(Configuration newConfig)
		{
			OnContentChanged();

			Recreate();
		}

		protected virtual void ActivityCancellationTokenSourceCancel()
		{
			// TODO
			// ActivityCancellationTokenSource?.Cancel();
		}
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname ?? string.Empty));
		}
		protected virtual void SupportFragmentManagerBackStackChanged(object sender, EventArgs args) { }
		protected virtual void SupportFragmentManagerFragmentOnAttach(object sender, FragmentOnAttachEventArgs args) { }
		protected virtual void XyzuLibraryOnServiceConnectionChanged(object sender, ServiceConnectionChangedEventArgs args) { }
		protected virtual void XyzuPlayerOnServiceConnectionChanged(object sender, ServiceConnectionChangedEventArgs args) { }
	}
}