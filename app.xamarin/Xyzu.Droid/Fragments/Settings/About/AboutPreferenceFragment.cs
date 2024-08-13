#nullable enable

using Android.Content;
using Android.OS;
using Android.Runtime;

using Xyzu.Droid;
using Xyzu.Preference;
using Xyzu.Settings.About;

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
		public PlainPreference? Licenses_ExoPlayerPreference { get; set; }
		public PlainPreference? Licenses_SqlnetpclPreference { get; set; }
		public PlainPreference? Licenses_Id3Preference { get; set; }
		public PlainPreference? Licenses_TagLibSharpPreference { get; set; }
		public PlainPreference? Licenses_MusicBarPreference { get; set; }
		public PlainPreference? Licenses_GlidePreference { get; set; }
		public PlainPreference? Licenses_PicassoPreference { get; set; }
		public PlainPreference? Licenses_ColorPickerPreference { get; set; }

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
				true when preference == Licenses_ExoPlayerPreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_licenses_exoplayer_url)),
				true when preference == Licenses_SqlnetpclPreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_licenses_sqlnetpcl_url)),
				true when preference == Licenses_Id3Preference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_licenses_id3_url)),
				true when preference == Licenses_TagLibSharpPreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_licenses_taglibsharp_url)),
				true when preference == Licenses_MusicBarPreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_licenses_musicbar_url)),
				true when preference == Licenses_GlidePreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_licenses_glide_url)),
				true when preference == Licenses_PicassoPreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_licenses_picasso_url)),
				true when preference == Licenses_ColorPickerPreference
					=> XyzuUtils.Intents.App_Browser(Context, Resources?.GetString(Resource.String.settings_about_licenses_colorpicker_url)),

				_ => null,

			} is Intent intent) { Activity?.StartActivity(intent); return true; }

			return OnPreferenceClick(preference);
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
				Licenses_ExoPlayerPreference = FindPreference(Keys.Licenses_ExoPlayer) as PlainPreference,
				Licenses_SqlnetpclPreference = FindPreference(Keys.Licenses_Sqlnetpcl) as PlainPreference,
				Licenses_Id3Preference = FindPreference(Keys.Licenses_Id3) as PlainPreference,
				Licenses_TagLibSharpPreference = FindPreference(Keys.Licenses_TagLibSharp) as PlainPreference,
				Licenses_MusicBarPreference = FindPreference(Keys.Licenses_MusicBar) as PlainPreference,
				Licenses_GlidePreference = FindPreference(Keys.Licenses_Glide) as PlainPreference,
				Licenses_PicassoPreference = FindPreference(Keys.Licenses_Picasso) as PlainPreference,
				Licenses_ColorPickerPreference = FindPreference(Keys.Licenses_ColorPicker) as PlainPreference);
		}
	}
}