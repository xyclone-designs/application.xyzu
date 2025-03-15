#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;

using Xyzu.Droid;

namespace Xyzu.Views.Preference
{
	public partial class LicenseView : ConstraintLayout
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_preference_license;

			public const int ViewMore_AppCompatTextView = Resource.Id.xyzu_view_preference_license_viewmore_appcompattextview;
			public const int License_AppCompatTextView = Resource.Id.xyzu_view_preference_license_license_appcompattextview;
		}

		public LicenseView(Context context) : this(context, null!)
		{ }
		public LicenseView(Context context, IAttributeSet? attrs) : this(context, attrs, Resource.Style.Xyzu_Preference)
		{ }
		public LicenseView(Context context, IAttributeSet? attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_Preference)
		{ }
		public LicenseView(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs); 
		}

		protected virtual void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			Background = context.Resources?.GetDrawable(Resource.Drawable.background_view_preference_license, null);
		}

		private AppCompatTextView? _ViewViewMore;
		private AppCompatTextView? _ViewLicense;

		public AppCompatTextView ViewViewMore
		{
			get => _ViewViewMore
				??= FindViewById<AppCompatTextView>(Ids.ViewMore_AppCompatTextView) ??
				throw new InflateException("ViewMore_AppCompatTextView");
		}
		public AppCompatTextView ViewLicense
		{
			get => _ViewLicense
				??= FindViewById<AppCompatTextView>(Ids.License_AppCompatTextView) ??
				throw new InflateException("License_AppCompatTextView");
		}

		public static LicenseView ForDropdownPreference(Context context, string license, IOnClickListener listener)
		{
			LicenseView licenseview = new (context, null, Resource.Style.Xyzu_Preference);
			licenseview.ViewLicense.SetText(license, null);
			licenseview.SetOnClickListener(listener);

			return licenseview;
		}
	}
}