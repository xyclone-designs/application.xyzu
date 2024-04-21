#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;

using Google.Android.Material.Tabs;

using Xyzu.Droid;

using AndroidXToolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Xyzu.Views.Toolbar
{
	public class ToolbarTabLayoutView : ToolbarView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_toolbar_tablayout;

			public const int StatusBarInset= Resource.Id.xyzu_view_toolbar_tablayout_statusbarinsetview;
			public const int Toolbar = Resource.Id.xyzu_view_toolbar_tablayout_toolbar;
			public const int TabLayout = Resource.Id.xyzu_view_toolbar_tablayout_tablayout;
		}

		public ToolbarTabLayoutView(Context context) : base(context)
		{ }
		public ToolbarTabLayoutView(Context context, IAttributeSet attrs) : base(context, attrs)
		{ }
		public ToolbarTabLayoutView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			base.Init(context, attrs);

			Inflate(context, Ids.Layout, this);
		}

		private TabLayout? _Tablayout;
		private AndroidXToolbar? _Toolbar;

		public TabLayout Tablayout
		{
			get => _Tablayout ??= FindViewById(Ids.TabLayout) as TabLayout ?? throw new InflateException();
		}								   
		public AndroidXToolbar Toolbar
		{
			get => _Toolbar ??= FindViewById(Ids.Toolbar) as AndroidXToolbar ?? throw new InflateException();
		}
	}
}