#nullable enable

using Android.OS;
using Android.Runtime;

using Xyzu.Droid;
using Xyzu.Settings.About;

namespace Xyzu.Fragments.Settings.About
{
	[Register(FragmentName)]
	public class AboutPreferenceFragment : BasePreferenceFragment, IAboutSettings
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.About.AboutPreferenceFragment";

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_about_title);
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_about, rootKey);
		}
	}
}