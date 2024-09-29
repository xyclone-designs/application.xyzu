#nullable enable

using Android.Content;
using Android.Content.Res;

using Xyzu.Settings.Enums;

namespace AndroidX.RecyclerView.Widget
{
	public static class LibraryLayoutTypesExtensions
	{
		public static int ToGridLayoutManagerSpan(this LibraryLayoutTypes librarylayouttype, Context context)
		{
			return (librarylayouttype, context?.Resources?.Configuration?.Orientation == Orientation.Landscape) switch
			{
				(LibraryLayoutTypes.GridSmall, true) => 5,
				(LibraryLayoutTypes.GridSmall, _) => 4,

				(LibraryLayoutTypes.GridMedium, true) => 4,
				(LibraryLayoutTypes.GridMedium, _) => 3,

				(LibraryLayoutTypes.GridLarge, true) => 3,
				(LibraryLayoutTypes.GridLarge, _) => 2,

				(_, _) => 1
			};
		}
	}
}