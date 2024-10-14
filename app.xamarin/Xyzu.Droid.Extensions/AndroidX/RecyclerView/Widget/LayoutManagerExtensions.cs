using Android.Views;

namespace AndroidX.RecyclerView.Widget
{
	public static class LayoutManagerExtensions
	{
		public static int? GetPosition(this RecyclerView.LayoutManager layoutmanager, PagerSnapHelper pagersnaphelper)
		{
			View? snapview = pagersnaphelper.FindSnapView(layoutmanager);
			
			return snapview is null ? new int?() : layoutmanager.GetPosition(snapview);
		}
	}
}