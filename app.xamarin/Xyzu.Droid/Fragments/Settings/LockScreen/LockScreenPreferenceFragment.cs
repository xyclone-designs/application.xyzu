#nullable enable 

using Android.OS;
using Android.Runtime;

using Xyzu.Droid;
using Xyzu.Settings.LockScreen;

namespace Xyzu.Fragments.Settings.LockScreen
{
	[Register(FragmentName)]
	public class LockScreenPreferenceFragment : BasePreferenceFragment, ILockScreenSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.LockScreen.LockScreenPreferenceFragment";

		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_lockscreen, rootKey);

			AppCompatActivity?.SetTitle(Resource.String.settings_lockscreen_title);
		}
	}
}