#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;

using System;

namespace Xyzu.Views.Insets
{
	[Register("xyzu/views/insets/NavigationBarInsetView")]
	public class NavigationBarInsetView : InsetView
	{
		public NavigationBarInsetView(Context context) : base(context) { }
		public NavigationBarInsetView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public NavigationBarInsetView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (Context?.Resources?.GetNavigationBarHeight() is int navigationbarheight)
				AddInset("NavigationBarHeight", navigationbarheight);
		}
	}
}