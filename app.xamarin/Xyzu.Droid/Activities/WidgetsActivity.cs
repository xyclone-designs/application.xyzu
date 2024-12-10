using Android.App;
using Android.Content.PM;
using Android.OS;

using Xyzu.Droid;

namespace Xyzu.Activities
{
    [Activity(
		Theme = "@style/WidgetsTheme",
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
	public class WidgetsActivity : BaseActivity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.xyzu_layout_widgets);
		}
	}
}