#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.LibraryItem.Grid
{
	public class GridMediumView : GridView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_libraryitem_grid_medium;

			public const int ArtworkAppCompatImageView = Resource.Id.xyzu_view_libraryitem_grid_medium_artwork_appcompatimageview;
			public const int EqualiserAppCompatImageView = Resource.Id.xyzu_view_libraryitem_grid_medium_equaliser_appcompatimageview;
			public const int LineOneAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_medium_lineone_appcompattextview;
			public const int LineTwoAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_medium_linetwo_appcompattextview;
			public const int LineThreeAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_medium_linethree_appcompattextview;
		}

		public GridMediumView(Context context) : base(context) { }
		public GridMediumView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public GridMediumView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);

			Artwork = FindViewById<AppCompatImageView>(Ids.ArtworkAppCompatImageView);
			Equaliser = FindViewById<AppCompatImageView>(Ids.EqualiserAppCompatImageView);
			LineOne = FindViewById<AppCompatTextView>(Ids.LineOneAppCompatTextView);
			LineTwo = FindViewById<AppCompatTextView>(Ids.LineTwoAppCompatTextView);
			LineThree = FindViewById<AppCompatTextView>(Ids.LineThreeAppCompatTextView);
		}
	}
}