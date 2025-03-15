using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

using System.IO;

using Xyzu.Droid;
using Xyzu.Preference;
using Xyzu.Settings.About;
using Xyzu.Views.Preference;

using AndroidXPreference = AndroidX.Preference.Preference;

namespace Xyzu.Fragments.Settings.About
{
	[Register(FragmentName)]
	public class AboutPreferenceFragment : BasePreferenceFragment, IAboutSettings, AndroidXPreference.IOnPreferenceClickListener
	{
		public const string FragmentName = "Xyzu.Fragments.Settings.About.AboutPreferenceFragment";

		public static class Keys
		{
			public const string Version = "settings_about_version_preference_key";
			public const string Copyright = "settings_about_copyright_preference_key";
			public const string Legal = "settings_about_legal_preference_key";
			public const string Information = "settings_about_information_preferencegroup_key";
			public const string Information_Email_Developers = "settings_about_information_email_developers_preference_key";
			public const string Information_Website_XycloneDesigns = "settings_about_information_website_xyclonedesigns_preference_key";
			public const string Information_Website_Github = "settings_about_information_website_github_preference_key";
			public const string Licenses = "settings_about_licenses_preferencegroup_key";
			public const string Licenses_SqlNetPcl = "settings_about_licenses_sqlnetpcl_preference_key";
			public const string Licenses_Id3 = "settings_about_licenses_id3_preference_key";
			public const string Licenses_TagLibSharp = "settings_about_licenses_taglibsharp_preference_key";
			public const string Licenses_Musicbar = "settings_about_licenses_musicbar_preference_key";
			public const string Licenses_Laerdalffmpeg = "settings_about_licenses_laerdalffmpeg_preference_key";
			public const string Licenses_Glide = "settings_about_licenses_glide_preference_key";
			public const string Licenses_Picasso = "settings_about_licenses_picasso_preference_key";
			public const string Licenses_Colorpicker = "settings_about_licenses_colorpicker_preference_key";
			public const string Licenses_Oxyplot = "settings_about_licenses_oxyplot_preference_key";
			public const string Licenses_Newtonsoft = "settings_about_licenses_newtonsoft_preference_key";
			public const string Licenses_SeekArc = "settings_about_licenses_seekarc_preference_key";
			public const string Licenses_SlidingUpPanel = "settings_about_licenses_slidinguppanel_preference_key";
			public const string Licenses_BoxedVerticalSeekbar = "settings_about_licenses_boxedverticalseekbar_preference_key";
		}

		public PlainPreference? VersionPreference { get; set; }
		public PlainPreference? CopyrightPreference { get; set; }
		public PlainPreference? LegalPreference { get; set; }
		public PlainPreference? InformationPreference { get; set; }
		public PlainPreference? Information_Email_DevelopersPreference { get; set; }
		public PlainPreference? Information_Website_XycloneDesignsPreference { get; set; }
		public PlainPreference? Information_Website_GithubPreference { get; set; }
		public PlainPreference? LicensesPreference { get; set; }
		public LicensePreference? Licenses_SqlNetPcl { get; set; }
		public LicensePreference? Licenses_Id3 { get; set; }
		public LicensePreference? Licenses_TagLibSharp { get; set; }
		public LicensePreference? Licenses_Musicbar { get; set; }
		public LicensePreference? Licenses_Laerdalffmpeg { get; set; }
		public LicensePreference? Licenses_Glide { get; set; }
		public LicensePreference? Licenses_Picasso { get; set; }
		public LicensePreference? Licenses_Colorpicker { get; set; }
		public LicensePreference? Licenses_Oxyplot { get; set; }
		public LicensePreference? Licenses_Newtonsoft { get; set; }
		public LicensePreference? Licenses_SeekArc { get; set; }
		public LicensePreference? Licenses_SlidingUpPanel { get; set; }
		public LicensePreference? Licenses_BoxedVerticalSeekbar { get; set; }

		public override void OnResume()
		{
			base.OnResume();

			AppCompatActivity?.SetTitle(Resource.String.settings_about_title);

			AddPreferenceClickHandler(
				VersionPreference,
				CopyrightPreference,
				LegalPreference,
				InformationPreference,
				Information_Email_DevelopersPreference,
				Information_Website_XycloneDesignsPreference,
				Information_Website_GithubPreference,
				LicensesPreference,
				Licenses_SqlNetPcl,
				Licenses_Id3,
				Licenses_TagLibSharp,
				Licenses_Musicbar,
				Licenses_Laerdalffmpeg,
				Licenses_Glide,
				Licenses_Picasso,
				Licenses_Colorpicker,
				Licenses_Oxyplot,
				Licenses_Newtonsoft,
				Licenses_SeekArc,
				Licenses_SlidingUpPanel,
				Licenses_BoxedVerticalSeekbar);
		}
		public override void OnPause()
		{
			base.OnPause();

			RemovePreferenceClickHandler(
				VersionPreference,
				CopyrightPreference,
				LegalPreference,
				InformationPreference,
				Information_Email_DevelopersPreference,
				Information_Website_XycloneDesignsPreference,
				Information_Website_GithubPreference,
				LicensesPreference,
				Licenses_SqlNetPcl,
				Licenses_Id3,
				Licenses_TagLibSharp,
				Licenses_Musicbar,
				Licenses_Laerdalffmpeg,
				Licenses_Glide,
				Licenses_Picasso,
				Licenses_Colorpicker,
				Licenses_Oxyplot,
				Licenses_Newtonsoft,
				Licenses_SeekArc,
				Licenses_SlidingUpPanel,
				Licenses_BoxedVerticalSeekbar);
		}
		public override bool OnPreferenceClick(AndroidXPreference preference)
		{
			if (true switch
			{
				true when preference == Information_Email_DevelopersPreference
					=> XyzuUtils.Intents.App_Email(Context, Resources?.GetString(Resource.String.settings_about_information_email_developers_url)),
				true when preference == Information_Website_XycloneDesignsPreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_information_website_xyclonedesigns_url)),
				true when preference == Information_Website_GithubPreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_information_website_github_url)),

				_ => null,

			} is Intent intent) { Activity?.StartActivity(intent); return true; }

			if (preference is DropdownPreference dropdownpreference && dropdownpreference.View != null)
				dropdownpreference.View.Expanded = !dropdownpreference.View.Expanded;

			return base.OnPreferenceClick(preference);
		}
		public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
		{
			SetPreferencesFromResource(Resource.Xml.settings_about, rootKey);
			InitPreferences(
				VersionPreference = FindPreference(Keys.Version) as PlainPreference,
				CopyrightPreference = FindPreference(Keys.Copyright) as PlainPreference,
				LegalPreference = FindPreference(Keys.Legal) as PlainPreference,
				InformationPreference = FindPreference(Keys.Information) as PlainPreference,
				Information_Email_DevelopersPreference = FindPreference(Keys.Information_Email_Developers) as PlainPreference,
				Information_Website_XycloneDesignsPreference = FindPreference(Keys.Information_Website_XycloneDesigns) as PlainPreference,
				Information_Website_GithubPreference = FindPreference(Keys.Information_Website_Github) as PlainPreference,
				LicensesPreference = FindPreference(Keys.Licenses) as PlainPreference,
				Licenses_SqlNetPcl = FindPreference(Keys.Licenses_SqlNetPcl) as LicensePreference,
				Licenses_Id3 = FindPreference(Keys.Licenses_Id3) as LicensePreference,
				Licenses_TagLibSharp = FindPreference(Keys.Licenses_TagLibSharp) as LicensePreference,
				Licenses_Musicbar = FindPreference(Keys.Licenses_Musicbar) as LicensePreference,
				Licenses_Laerdalffmpeg = FindPreference(Keys.Licenses_Laerdalffmpeg) as LicensePreference,
				Licenses_Glide = FindPreference(Keys.Licenses_Glide) as LicensePreference,
				Licenses_Picasso = FindPreference(Keys.Licenses_Picasso) as LicensePreference,
				Licenses_Colorpicker = FindPreference(Keys.Licenses_Colorpicker) as LicensePreference,
				Licenses_Oxyplot = FindPreference(Keys.Licenses_Oxyplot) as LicensePreference,
				Licenses_Newtonsoft = FindPreference(Keys.Licenses_Newtonsoft) as LicensePreference,
				Licenses_SeekArc = FindPreference(Keys.Licenses_SeekArc) as LicensePreference,
				Licenses_SlidingUpPanel = FindPreference(Keys.Licenses_SlidingUpPanel) as LicensePreference,
				Licenses_BoxedVerticalSeekbar = FindPreference(Keys.Licenses_BoxedVerticalSeekbar) as LicensePreference);

			if (Context?.Assets is null)
				return;

			using Stream sqlnetpclstream = Context.Assets.Open("licenses/package_sqlnetpcl.txt");
			using Stream id3stream = Context.Assets.Open("licenses/package_id3.txt");
			using Stream taglibsharpstream = Context.Assets.Open("licenses/package_taglibsharp.txt");
			using Stream musicbarstream = Context.Assets.Open("licenses/package_musicbar.txt");
			using Stream laerdalffmpegstream = Context.Assets.Open("licenses/package_laerdalffmpeg.txt");
			using Stream glidestream = Context.Assets.Open("licenses/package_glide.txt");
			using Stream picassostream = Context.Assets.Open("licenses/package_picasso.txt");
			using Stream colorpickerstream = Context.Assets.Open("licenses/package_colorpicker.txt");
			using Stream oxyplotstream = Context.Assets.Open("licenses/package_oxyplot.txt");
			using Stream newtonsoftstream = Context.Assets.Open("licenses/package_newtonsoft.txt");
			using Stream seekarcstream = Context.Assets.Open("licenses/package_seekarc.txt");
			using Stream slidinguppanelstream = Context.Assets.Open("licenses/package_slidinguppanel.txt");
			using Stream boxedverticalseekbarstream = Context.Assets.Open("licenses/package_boxedverticalseekbar.txt");

			using StreamReader sqlnetpclstreamreader = new(sqlnetpclstream);
			using StreamReader id3streamreader = new(id3stream);
			using StreamReader taglibsharpstreamreader = new(taglibsharpstream);
			using StreamReader musicbarstreamreader = new(musicbarstream);
			using StreamReader laerdalffmpegstreamreader = new(laerdalffmpegstream);
			using StreamReader glidestreamreader = new(glidestream);
			using StreamReader picassostreamreader = new(picassostream);
			using StreamReader colorpickerstreamreader = new(colorpickerstream);
			using StreamReader oxyplotstreamreader = new(oxyplotstream);
			using StreamReader newtonsoftstreamreader = new(newtonsoftstream);
			using StreamReader seekarcstreamreader = new(seekarcstream);
			using StreamReader slidinguppanelstreamreader = new(slidinguppanelstream);
			using StreamReader boxedverticalseekbarstreamreader = new(boxedverticalseekbarstream);

			Licenses_SqlNetPcl?.SetLibraryItem(this, sqlnetpclstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_sqlnetpcl_url);
			Licenses_Id3?.SetLibraryItem(this, id3streamreader.ReadToEnd(), Resource.String.settings_about_licenses_id3_url);
			Licenses_TagLibSharp?.SetLibraryItem(this, taglibsharpstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_taglibsharp_url);
			Licenses_Musicbar?.SetLibraryItem(this, musicbarstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_musicbar_url);
			Licenses_Laerdalffmpeg?.SetLibraryItem(this, laerdalffmpegstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_laerdalffmpeg_url);
			Licenses_Glide?.SetLibraryItem(this, glidestreamreader.ReadToEnd(), Resource.String.settings_about_licenses_glide_url);
			Licenses_Picasso?.SetLibraryItem(this, picassostreamreader.ReadToEnd(), Resource.String.settings_about_licenses_picasso_url);
			Licenses_Colorpicker?.SetLibraryItem(this, colorpickerstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_colorpicker_url);
			Licenses_Oxyplot?.SetLibraryItem(this, oxyplotstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_oxyplot_url);
			Licenses_Newtonsoft?.SetLibraryItem(this, newtonsoftstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_newtonsoft_url);
			Licenses_SeekArc?.SetLibraryItem(this, seekarcstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_seekarc_url);
			Licenses_SlidingUpPanel?.SetLibraryItem(this, slidinguppanelstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_slidinguppanel_url);
			Licenses_BoxedVerticalSeekbar?.SetLibraryItem(this, boxedverticalseekbarstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_boxedverticalseekbar_url);
		}
	}

	public static class AboutPreferenceFragmentExtensions
	{
		public static void SetLibraryItem(this DropdownPreference dropdownpreference, AboutPreferenceFragment fragment, string text, int licenseurl)
		{
			if (dropdownpreference.View is null)
				return;

			dropdownpreference.View.ViewAdditionalContentView = LicenseView.ForDropdownPreference(dropdownpreference.Context, text, new OnClickListener(_ =>
			{
				fragment.Activity?.StartActivity(XyzuUtils.Intents.App_Browser(dropdownpreference.Context, dropdownpreference.Context.Resources?.GetString(licenseurl)));
			}));
		}
	}
}