#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.RecyclerView.Widget;

using Xyzu.Droid;

namespace Xyzu.Widgets.RecyclerViews.Simple
{
	[Register("xyzu/widgets/recyclerviews/simple/SimpleHorizontalRecyclerView")]
	public class SimpleHorizontalRecyclerView : SimpleRecyclerView
	{
		public SimpleHorizontalRecyclerView(Context context) : this(context, null!)
		{ }
		public SimpleHorizontalRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_Simple_Horizontal)
		{ }
		public SimpleHorizontalRecyclerView(Context context, IAttributeSet attrs, int defStyleRef) : base(context, attrs, defStyleRef)
		{
			HasFixedSize = true;
			SetLayoutManager(_SimpleLayoutManager = new LayoutManager(context));
		}

		private LayoutManager _SimpleLayoutManager;

		public LayoutManager SimpleLayoutManager
		{
			set => SetLayoutManager(_SimpleLayoutManager = value);
			get => GetLayoutManager() as LayoutManager ?? _SimpleLayoutManager;
		}

		public new class LayoutManager : GridLayoutManager
		{
			public LayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
			{ }
			public LayoutManager(Context context, int spanCount = 1, int orientation = Horizontal, bool reverseLayout = false) : base(context, spanCount, orientation, reverseLayout)
			{ }

			public override void OnLayoutChildren(Recycler? recycler, State? state)
			{
				try { base.OnLayoutChildren(recycler, state); }
				catch (Java.Lang.IndexOutOfBoundsException)
				{
					// IndexOutOfBoundsException Exception on sucessive list refreshes 
					// https://stackoverflow.com/questions/31759171/recyclerview-and-java-lang-indexoutofboundsexception-inconsistency-detected-in
				}
			}
		}
	}
}