
namespace Android.Views
{
	public static class ViewExtensions
	{
		public static void SetMarginAll(this View view, int margin, bool addtoexisting = false)
		{
			switch (true)
			{
				case true when
				view.LayoutParameters is ViewGroup.MarginLayoutParams marginlayoutparams:
					if (addtoexisting)
					{
						marginlayoutparams.TopMargin += margin;
						marginlayoutparams.LeftMargin += margin;
						marginlayoutparams.RightMargin += margin;
						marginlayoutparams.BottomMargin += margin;
					}
					else
					{
						marginlayoutparams.TopMargin = margin;
						marginlayoutparams.LeftMargin = margin;
						marginlayoutparams.RightMargin = margin;
						marginlayoutparams.BottomMargin = margin;
					}
					break;

				default: break;
			}
		}
		public static void SetMarginVertical(this View view, int margin, bool addtoexisting = false)
		{
			if (view.LayoutParameters is not ViewGroup.MarginLayoutParams marginlayoutparams)
				return;

			if (addtoexisting)
			{
				marginlayoutparams.LeftMargin += margin;
				marginlayoutparams.RightMargin += margin;
			}
			else
			{
				marginlayoutparams.LeftMargin = margin;
				marginlayoutparams.RightMargin = margin;
			}
		}
		public static void SetMarginHorizontal(this View view, int margin, bool addtoexisting = false)
		{
			if (view.LayoutParameters is not ViewGroup.MarginLayoutParams marginlayoutparams)
				return;

			if (addtoexisting)
			{
				marginlayoutparams.TopMargin += margin;
				marginlayoutparams.BottomMargin += margin;
			}
			else
			{
				marginlayoutparams.TopMargin = margin;
				marginlayoutparams.BottomMargin = margin;
			}
		}
		public static void SetMarginBottom(this View view, int margin, bool addtoexisting = false)
		{
			switch (true)
			{
				case true when
				view.LayoutParameters is ViewGroup.MarginLayoutParams marginlayoutparams:
					if (addtoexisting)
						marginlayoutparams.BottomMargin += margin;
					else marginlayoutparams.BottomMargin = margin;
					break;

				default: break;
			}
		}
		public static void SetMarginEnd(this View view, int margin, bool addtoexisting = false)
		{
			switch (true)
			{
				case true when
				view.LayoutParameters is ViewGroup.MarginLayoutParams marginlayoutparams:
					if (addtoexisting)
						marginlayoutparams.MarginEnd += margin;
					else marginlayoutparams.MarginEnd = margin;
					break;

				default: break;
			}
		}
		public static void SetMarginLeft(this View view, int margin, bool addtoexisting = false)
		{
			switch (true)
			{
				case true when
				view.LayoutParameters is ViewGroup.MarginLayoutParams marginlayoutparams:
					if (addtoexisting)
						marginlayoutparams.LeftMargin += margin;
					else marginlayoutparams.LeftMargin = margin;
					break;

				default: break;
			}
		}
		public static void SetMarginRight(this View view, int margin, bool addtoexisting = false)
		{
			switch (true)
			{
				case true when
				view.LayoutParameters is ViewGroup.MarginLayoutParams marginlayoutparams:
					if (addtoexisting)
						marginlayoutparams.RightMargin += margin;
					else marginlayoutparams.RightMargin = margin;
					break;

				default: break;
			}
		}
		public static void SetMarginStart(this View view, int margin, bool addtoexisting = false)
		{
			switch (true)
			{						
				case true when
				view.LayoutParameters is ViewGroup.MarginLayoutParams marginlayoutparams:
					if (addtoexisting)
						marginlayoutparams.MarginStart += margin;
					else marginlayoutparams.MarginStart = margin;
					break;

				default: break;
			}
		}
		public static void SetMarginTop(this View view, int margin, bool addtoexisting = false)
		{
			switch (true)
			{
				case true when
				view.LayoutParameters is ViewGroup.MarginLayoutParams marginlayoutparams:
					if (addtoexisting)
						marginlayoutparams.TopMargin += margin;
					else marginlayoutparams.TopMargin = margin;
					break;

				default: break;
			}
		}

		public static void SetPaddingAll(this View view, int padding, bool addtoexisting = false)
		{
			if (addtoexisting)
				view.SetPaddingRelative(
					end: view.PaddingEnd + padding,
					top: view.PaddingTop + padding,
					start: view.PaddingStart + padding,
					bottom: view.PaddingBottom + padding);
			else view.SetPaddingRelative(
				end: padding,
				top: padding,
				start: padding,
				bottom: padding);
		}
		public static void SetPaddingVertical(this View view, int padding, bool addtoexisting = false)
		{
			view.SetPaddingRelative(
				top: view.PaddingTop,
				bottom: view.PaddingBottom,
				end: addtoexisting ? view.PaddingEnd + padding : padding,
				start: addtoexisting ? view.PaddingStart + padding : padding);
		}
		public static void SetPaddingHorizontal(this View view, int padding, bool addtoexisting = false)
		{
			view.SetPaddingRelative(
				end: view.PaddingEnd,
				start: view.PaddingStart,
				top: addtoexisting ? view.PaddingTop + padding : padding,
				bottom: addtoexisting ? view.PaddingBottom + padding : padding);
		}
		public static void SetPaddingBottom(this View view, int padding, bool addtoexisting = false)
		{
			view.SetPaddingRelative(
				end: view.PaddingEnd,
				top: view.PaddingTop,
				start: view.PaddingStart,
				bottom: addtoexisting ? view.PaddingBottom + padding : padding);
		}
		public static void SetPaddingEnd(this View view, int padding, bool addtoexisting = false)
		{
			view.SetPaddingRelative(
				top: view.PaddingTop,
				start: view.PaddingStart,
				bottom: view.PaddingBottom,
				end: addtoexisting ? view.PaddingEnd + padding : padding);
		}
		public static void SetPaddingLeft(this View view, int padding, bool addtoexisting = false)
		{
			view.SetPadding(
				top: view.PaddingTop,
				right: view.PaddingRight,
				bottom: view.PaddingBottom,
				left: addtoexisting ? view.PaddingLeft + padding : padding);
		}
		public static void SetPaddingRight(this View view, int padding, bool addtoexisting = false)
		{
			view.SetPadding(
				top: view.PaddingTop,
				left: view.PaddingLeft,
				bottom: view.PaddingBottom,
				right: addtoexisting ? view.PaddingRight + padding : padding);
		}
		public static void SetPaddingStart(this View view, int padding, bool addtoexisting = false)
		{
			view.SetPaddingRelative(
				end: view.PaddingEnd,
				top: view.PaddingTop,
				bottom: view.PaddingBottom,
				start: addtoexisting ? view.PaddingStart + padding : padding);
		}
		public static void SetPaddingTop(this View view, int padding, bool addtoexisting = false)
		{
			view.SetPaddingRelative(
				end: view.PaddingEnd,
				start: view.PaddingStart,
				bottom: view.PaddingBottom,
				top: addtoexisting ? view.PaddingTop + padding : padding);
		}

		public static void SetVisibility(this View view, ViewStates viewstate)
		{
			view.Visibility = viewstate;
		}

		private static bool AllZero(int top, int left, int right, int bottom)
		{
			return
				top == 0 &&
				left == 0 &&
				right == 0 &&
				bottom == 0;
		}
		public static bool AllZero(this View.LayoutChangeEventArgs layoutchangeeventargs)
		{
			return AllZero(layoutchangeeventargs.Top, layoutchangeeventargs.Left, layoutchangeeventargs.Right, layoutchangeeventargs.Bottom);
		}
		public static bool AllZeroOld(this View.LayoutChangeEventArgs layoutchangeeventargs)
		{
			return AllZero(layoutchangeeventargs.OldTop, layoutchangeeventargs.OldLeft, layoutchangeeventargs.OldRight, layoutchangeeventargs.OldBottom);
		}
	}
}