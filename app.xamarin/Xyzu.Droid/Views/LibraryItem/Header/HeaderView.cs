#nullable enable

using Android.Content;
using Android.Util;

using Xyzu.Droid;

namespace Xyzu.Views.LibraryItem.Header
{
	public class HeaderView : LibraryItemView
	{
		public HeaderView(Context context) : this(context, null!) { }
		public HeaderView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_LibraryItem_Header) { }
		public HeaderView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_LibraryItem_Header) { }
		public HeaderView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }
	}
}