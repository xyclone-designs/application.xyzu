#nullable enable

using Android.Content.Res;
using Android.Graphics;
using Android.Views;

using System;

namespace AndroidX.RecyclerView.Widget
{
	public class MarginItemDecoration : RecyclerView.ItemDecoration
	{
		public View? HeaderView { get; set; }
		public View? FooterView { get; set; }

		public int Margin
		{
			set => (MarginVertical, MarginHorizontal) = (value, value);
		}
		public int MarginVertical
		{
			set => (MarginTop, MarginBottom) = (value, value);
		}
		public int MarginHorizontal
		{
			set => (MarginLeft, MarginRight) = (value, value);
		}
		public int MarginTop { get; set; }
		public int MarginLeft { get; set; }
		public int MarginRight { get; set; }
		public int MarginBottom { get; set; }

		public int MarginRes
		{
			set => (MarginResVertical, MarginResHorizontal) = (value, value);
		}
		public int? MarginResVertical
		{	 
			set => (MarginResTop, MarginResBottom) = (value, value);
		}											  
		public int? MarginResHorizontal
		{
			set => (MarginResLeft, MarginResRight) = (value, value);
		}

		public int? MarginResTop { get; set; }
		public int? MarginResLeft { get; set; }
		public int? MarginResRight { get; set; }
		public int? MarginResBottom { get; set; }

		public Func<MarginItemDecoration, bool>? ApplyTopInset { get; set; }
		public Func<MarginItemDecoration, bool>? ApplyLeftInset { get; set; }
		public Func<MarginItemDecoration, bool>? ApplyRightInset { get; set; }
		public Func<MarginItemDecoration, bool>? ApplyBottomInset { get; set; }		   

		public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
		{
			base.GetItemOffsets(outRect, view, parent, state);

			RecyclerView.Adapter? adapter = parent.GetAdapter();
			RecyclerView.LayoutManager? layoutmanager = parent.GetLayoutManager();

			if (view.Visibility == ViewStates.Gone || adapter is null || layoutmanager is null)
				return;

		int orientation, spancount;

			switch (true)
			{
				case true when layoutmanager is GridLayoutManager gridlayoutmanager:
					orientation = gridlayoutmanager.Orientation;
					spancount = gridlayoutmanager.SpanCount;
					break;

				case true when layoutmanager is LinearLayoutManager linearlayoutmanager:
					orientation = linearlayoutmanager.Orientation;
					spancount = 1;
					break;
				
				case true when layoutmanager is StaggeredGridLayoutManager staggeredgridlayoutmanager:
					orientation = staggeredgridlayoutmanager.Orientation;
					spancount = staggeredgridlayoutmanager.SpanCount;
					break;

				default: return;
			}

			bool
				isRtL = false,
				row_top = false,
				row_bottom = false,
				column_first = false,
				column_last = false;

			int position = parent.GetChildAdapterPosition(view) + 1;

			switch (orientation)
			{
				case LinearLayoutManager.Vertical:
					row_top = position <= spancount;
					row_bottom = position >= adapter.ItemCount - spancount;
					column_first = position % spancount == 1;
					column_last = position % spancount == 0;
					break;

				case LinearLayoutManager.Horizontal:
					row_top = position % spancount != 0;
					row_bottom = position % spancount == 0;
					column_first = position <= spancount;
					column_last = position >= adapter.ItemCount - spancount;
					break;

				default: break;
			}

			Resources? resources =
				view.Resources ??
				view.Context?.Resources ??
				parent.Resources ??
				parent.Context?.Resources;

			if (MarginTop == 0 && MarginResTop.HasValue) MarginTop = resources?.GetDimensionPixelSize(MarginResTop.Value) ?? 0;
			if (MarginLeft == 0 && MarginResLeft.HasValue) MarginLeft = resources?.GetDimensionPixelSize(MarginResLeft.Value) ?? 0;
			if (MarginRight == 0 && MarginResRight.HasValue) MarginRight= resources?.GetDimensionPixelSize(MarginResRight.Value) ?? 0;
			if (MarginBottom == 0 && MarginResBottom.HasValue) MarginBottom = resources?.GetDimensionPixelSize(MarginResBottom.Value) ?? 0;

			bool isnotheader = (HeaderView is null || HeaderView != view);
			bool isnotfooter = (FooterView is null || FooterView != view);

			if (ApplyTopInset?.Invoke(this) ?? true && isnotheader)
				outRect.Top = row_top ? MarginTop : MarginTop / 2;

			if (ApplyLeftInset?.Invoke(this) ?? true && isnotheader && isnotfooter)
				outRect.Left = spancount == 1 ? MarginLeft : (column_first && !isRtL) ? MarginLeft : MarginLeft / 2;

			if (ApplyRightInset?.Invoke(this) ?? true && isnotheader && isnotfooter)
				outRect.Right = spancount == 1 ? MarginRight : (column_last && !isRtL) ? MarginRight : MarginRight / 2;
			
			if (ApplyBottomInset?.Invoke(this) ?? true && isnotfooter)
				outRect.Bottom = row_bottom ? MarginBottom : MarginBottom / 2;
		}
	}
}