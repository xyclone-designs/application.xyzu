#nullable enable

using Android.Content;
using Android.Util;
using AndroidX.AppCompat.Widget;

using Xyzu.Droid;

namespace Xyzu.Views.LibraryItem.Grid
{
	public class GridSmallView : GridView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_libraryitem_grid_small;

			public const int ArtworkAppCompatImageView = Resource.Id.xyzu_view_libraryitem_grid_small_artwork_appcompatimageview;
			public const int EqualiserAppCompatImageView = Resource.Id.xyzu_view_libraryitem_grid_small_equaliser_appcompatimageview;
			public const int CorruptAppCompatImageView = Resource.Id.xyzu_view_libraryitem_grid_small_corrupt_appcompatimageview;
			public const int LineOneAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_small_lineone_appcompattextview;
			public const int LineTwoAppCompatTextView = Resource.Id.xyzu_view_libraryitem_grid_small_linetwo_appcompattextview;
		}

		public GridSmallView(Context context) : base(context) { }
		public GridSmallView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public GridSmallView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);

			Artwork = FindViewById<AppCompatImageView>(Ids.ArtworkAppCompatImageView);
			Equaliser = FindViewById<AppCompatImageView>(Ids.EqualiserAppCompatImageView);
			Corrupt = FindViewById<AppCompatImageView>(Ids.CorruptAppCompatImageView);
			LineOne = FindViewById<AppCompatTextView>(Ids.LineOneAppCompatTextView);
			LineTwo = FindViewById<AppCompatTextView>(Ids.LineTwoAppCompatTextView);
		}
	}
}