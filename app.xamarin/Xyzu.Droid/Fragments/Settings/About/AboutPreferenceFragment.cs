#nullable enable

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
			public const string Licenses_ExoPlayer = "settings_about_licenses_exoplayer_preference_key";
			public const string Licenses_Sqlnetpcl = "settings_about_licenses_sqlnetpcl_preference_key";
			public const string Licenses_Id3 = "settings_about_licenses_id3_preference_key";
			public const string Licenses_TagLibSharp = "settings_about_licenses_taglibsharp_preference_key";
			public const string Licenses_MusicBar = "settings_about_licenses_musicbar_preference_key";
			public const string Licenses_Glide = "settings_about_licenses_glide_preference_key";
			public const string Licenses_Picasso = "settings_about_licenses_picasso_preference_key";
			public const string Licenses_ColorPicker = "settings_about_licenses_colorpicker_preference_key";
		}

		public PlainPreference? VersionPreference { get; set; }
		public PlainPreference? CopyrightPreference { get; set; }
		public PlainPreference? LegalPreference { get; set; }
		public PlainPreference? InformationPreference { get; set; }
		public PlainPreference? Information_Email_DevelopersPreference { get; set; }
		public PlainPreference? Information_Website_XycloneDesignsPreference { get; set; }
		public PlainPreference? Information_Website_GithubPreference { get; set; }
		public PlainPreference? LicensesPreference { get; set; }
		public DropdownPreference? Licenses_ExoPlayerPreference { get; set; }
		public DropdownPreference? Licenses_SqlnetpclPreference { get; set; }
		public DropdownPreference? Licenses_Id3Preference { get; set; }
		public DropdownPreference? Licenses_TagLibSharpPreference { get; set; }
		public DropdownPreference? Licenses_MusicBarPreference { get; set; }
		public DropdownPreference? Licenses_GlidePreference { get; set; }
		public DropdownPreference? Licenses_PicassoPreference { get; set; }
		public DropdownPreference? Licenses_ColorPickerPreference { get; set; }

		protected override void OnBindPreferences()
		{
			base.OnBindPreferences();

			if (Context?.Assets is null)
				return;

			using Stream exoplayerstream = Context.Assets.Open("licenses/package_exoplayer.txt");
			using Stream sqlnetpclstream = Context.Assets.Open("licenses/package_sqlnetpcl.txt");
			using Stream id3stream = Context.Assets.Open("licenses/package_id3.txt");
			using Stream taglibsharpstream = Context.Assets.Open("licenses/package_taglibsharp.txt");
			using Stream musicbarstream = Context.Assets.Open("licenses/package_musicbar.txt");
			using Stream glidestream = Context.Assets.Open("licenses/package_glide.txt");
			using Stream picassostream = Context.Assets.Open("licenses/package_picasso.txt");
			using Stream colorpickerstream = Context.Assets.Open("licenses/package_colorpicker.txt");

			using StreamReader exoplayerstreamreader = new StreamReader(exoplayerstream);
			using StreamReader sqlnetpclstreamreader = new StreamReader(sqlnetpclstream);
			using StreamReader id3streamreader = new StreamReader(id3stream);
			using StreamReader taglibsharpstreamreader = new StreamReader(taglibsharpstream);
			using StreamReader musicbarstreamreader = new StreamReader(musicbarstream);
			using StreamReader glidestreamreader = new StreamReader(glidestream);
			using StreamReader picassostreamreader = new StreamReader(picassostream);
			using StreamReader colorpickerstreamreader = new StreamReader(colorpickerstream);

			Licenses_ExoPlayerPreference?.SetLibraryItem(this, exoplayerstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_exoplayer_url);
			Licenses_SqlnetpclPreference?.SetLibraryItem(this, sqlnetpclstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_sqlnetpcl_url);
			Licenses_Id3Preference?.SetLibraryItem(this, id3streamreader.ReadToEnd(), Resource.String.settings_about_licenses_id3_url);
			Licenses_TagLibSharpPreference?.SetLibraryItem(this, taglibsharpstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_taglibsharp_url);
			Licenses_MusicBarPreference?.SetLibraryItem(this, musicbarstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_musicbar_url);
			Licenses_GlidePreference?.SetLibraryItem(this, glidestreamreader.ReadToEnd(), Resource.String.settings_about_licenses_glide_url);
			Licenses_PicassoPreference?.SetLibraryItem(this, picassostreamreader.ReadToEnd(), Resource.String.settings_about_licenses_picasso_url);
			Licenses_ColorPickerPreference?.SetLibraryItem(this, colorpickerstreamreader.ReadToEnd(), Resource.String.settings_about_licenses_colorpicker_url);
		}

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
				Licenses_ExoPlayerPreference,
				Licenses_SqlnetpclPreference,
				Licenses_Id3Preference,
				Licenses_TagLibSharpPreference,
				Licenses_MusicBarPreference,
				Licenses_GlidePreference,
				Licenses_PicassoPreference,
				Licenses_ColorPickerPreference);
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
				Licenses_ExoPlayerPreference,
				Licenses_SqlnetpclPreference,
				Licenses_Id3Preference,
				Licenses_TagLibSharpPreference,
				Licenses_MusicBarPreference,
				Licenses_GlidePreference,
				Licenses_PicassoPreference,
				Licenses_ColorPickerPreference);
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
				Licenses_ExoPlayerPreference = FindPreference(Keys.Licenses_ExoPlayer) as DropdownPreference,
				Licenses_SqlnetpclPreference = FindPreference(Keys.Licenses_Sqlnetpcl) as DropdownPreference,
				Licenses_Id3Preference = FindPreference(Keys.Licenses_Id3) as DropdownPreference,
				Licenses_TagLibSharpPreference = FindPreference(Keys.Licenses_TagLibSharp) as DropdownPreference,
				Licenses_MusicBarPreference = FindPreference(Keys.Licenses_MusicBar) as DropdownPreference,
				Licenses_GlidePreference = FindPreference(Keys.Licenses_Glide) as DropdownPreference,
				Licenses_PicassoPreference = FindPreference(Keys.Licenses_Picasso) as DropdownPreference,
				Licenses_ColorPickerPreference = FindPreference(Keys.Licenses_ColorPicker) as DropdownPreference);
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