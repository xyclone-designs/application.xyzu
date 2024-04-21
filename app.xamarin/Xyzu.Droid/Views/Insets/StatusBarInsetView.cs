#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;

using System;

namespace Xyzu.Views.Insets
{
	[Register("xyzu/views/insets/StatusBarInsetView")]
	public class StatusBarInsetView : InsetView
	{
		public StatusBarInsetView(Context context) : base(context) { }
		public StatusBarInsetView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public StatusBarInsetView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (Context?.Resources?.GetStatusBarHeight() is int statusbarheight)
				AddInset("StatusBarHeight", statusbarheight);
		}
	}
}