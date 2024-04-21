#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.LibraryItem.Grid
{
	public class GridView : LibraryItemView
	{
		public GridView(Context context) : this(context, null!) { }
		public GridView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_LibraryItem_Grid) { }
		public GridView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_LibraryItem_Grid) { }
		public GridView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }
	}
}