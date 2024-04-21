#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;

using Xyzu.Droid;

using AndroidXToolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Xyzu.Views.Toolbar
{
	public class ToolbarSettingsView : ToolbarView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_toolbar_settings;

			public const int StatusBarInset = Resource.Id.xyzu_view_toolbar_settings_statusbarinsetview;
			public const int Toolbar = Resource.Id.xyzu_view_toolbar_settings_toolbar;
		}

		public ToolbarSettingsView(Context context) : base(context)
		{ }
		public ToolbarSettingsView(Context context, IAttributeSet attrs) : base(context, attrs)
		{ }
		public ToolbarSettingsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);
		}

		private AndroidXToolbar? _Toolbar;

		public AndroidXToolbar Toolbar
		{
			get => _Toolbar ??= FindViewById(Ids.Toolbar) as AndroidXToolbar ?? throw new InflateException();
		}
	}
}