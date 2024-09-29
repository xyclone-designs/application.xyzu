#nullable enable

namespace AndroidX.RecyclerView.Widget
{
	public static class RecyclerViewExtensions
	{
		public static RecyclerView.ViewHolder? FindViewHolderForAdapterPosition(this RecyclerView recyclerview, int? position)
		{
			if (position.HasValue && recyclerview.FindViewHolderForAdapterPosition(position.Value) is RecyclerView.ViewHolder viewholder)
				return viewholder;

			return null;
		}
	}
}