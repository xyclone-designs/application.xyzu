using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.RecyclerView.Widget;

using Xyzu.Droid;

namespace Xyzu.Widgets.RecyclerViews.Simple
{
	[Register("xyzu/widgets/recyclerviews/simple/SimpleVerticalRecyclerView")]
	public class SimpleVerticalRecyclerView : SimpleRecyclerView
	{
		public SimpleVerticalRecyclerView(Context context) : this(context, null!)
		{ }
		public SimpleVerticalRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_Simple_Vertical)
		{ }
		public SimpleVerticalRecyclerView(Context context, IAttributeSet attrs, int defStyleRef) : base(context, attrs, defStyleRef)
		{
			SetLayoutManager(_SimpleLayoutManager = new LayoutManager(context));
		}

		private LayoutManager _SimpleLayoutManager;

		public LayoutManager SimpleLayoutManager
		{
			set => SetLayoutManager(_SimpleLayoutManager = value);
			get => GetLayoutManager() as LayoutManager ?? _SimpleLayoutManager;
		}

		public new class LayoutManager : LinearLayoutManager
		{
			public LayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
			{ }
			public LayoutManager(Context context, int orientation = Vertical, bool reverseLayout = false) : base(context, orientation, reverseLayout)
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