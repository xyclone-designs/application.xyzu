#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.LibraryItem.Grid
{
	public class GridLargeView : GridView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_libraryitem_grid_large;

			public const int ArtworkAppCompatImageView = Resource.Id.xyzu_view_libraryitem_grid_large_artwork_appcompatimageview;
			public const int EqualiserAppCompatImageView = Resource.Id.xyzu_view_libraryitem_grid_large_equaliser_appcompatimageview;
			public const int LineOneAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_large_lineone_appcompattextview;
			public const int LineTwoAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_large_linetwo_appcompattextview;
			public const int LineThreeAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_large_linethree_appcompattextview;
			public const int LineFourAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_large_linefour_appcompattextview;
		}

		public GridLargeView(Context context) : base(context) { }
		public GridLargeView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public GridLargeView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);

			Artwork = FindViewById<AppCompatImageView>(Ids.ArtworkAppCompatImageView);
			Equaliser = FindViewById<AppCompatImageView>(Ids.EqualiserAppCompatImageView);
			LineOne = FindViewById<AppCompatTextView>(Ids.LineOneAppCompatTextView);
			LineTwo = FindViewById<AppCompatTextView>(Ids.LineTwoAppCompatTextView);
			LineThree = FindViewById<AppCompatTextView>(Ids.LineThreeAppCompatTextView);
			LineFour = FindViewById<AppCompatTextView>(Ids.LineFourAppCompatTextView);
		}
	}
}