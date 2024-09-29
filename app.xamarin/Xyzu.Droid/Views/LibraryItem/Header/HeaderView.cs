#nullable enable

using Android.Content;
using Android.Util;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.LibraryItem.Header
{
	public class HeaderView : LibraryItemView
	{
		public HeaderView(Context context) : this(context, null!) { }
		public HeaderView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_LibraryItem_Header) { }
		public HeaderView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_LibraryItem_Header) { }
		public HeaderView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }
	
		public AppCompatImageButton? Back { get; set; }

		public Action<object?, EventArgs>? OnBackClick { get; set; }

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (Back != null) Back.Click += BackOnClick;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			if (Back != null) Back.Click -= BackOnClick;
		}
		protected void BackOnClick(object? sender, EventArgs args)
		{
			OnBackClick?.Invoke(sender, args);
		}
	}
}