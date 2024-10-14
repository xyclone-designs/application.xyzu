using System;

namespace AndroidX.RecyclerView.Widget
{
	public class RecyclerViewOnScrollListener : RecyclerView.OnScrollListener
	{
		public Action<RecyclerView, int, int>? OnScrolledAction { get; set; }
		public Action<RecyclerView, int>? OnScrollStateChangedAction { get; set; }

		public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
		{
			base.OnScrolled(recyclerView, dx, dy);

			OnScrolledAction?.Invoke(recyclerView, dx, dy);
		}
		public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
		{
			base.OnScrollStateChanged(recyclerView, newState);

			OnScrollStateChangedAction?.Invoke(recyclerView, newState);
		}
	}
}