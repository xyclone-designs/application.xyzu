#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.LibraryItem.List
{
	public class ListView : LibraryItemView
	{
		public ListView(Context context) : this(context, null!) { }
		public ListView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_LibraryItem_List) { }
		public ListView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_LibraryItem_List) { }
		public ListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }
	}
}