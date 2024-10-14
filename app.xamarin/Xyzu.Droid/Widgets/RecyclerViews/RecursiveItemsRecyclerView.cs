using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using System;

using Xyzu.Droid;

namespace Xyzu.Widgets.RecyclerViews
{
	[Register("xyzu/widgets/recyclerviews/RecursiveItemsRecyclerView")]
	public class RecursiveItemsRecyclerView : RecyclerView
	{
		public RecursiveItemsRecyclerView(Context context) : this(context, null!) { }
		public RecursiveItemsRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_RecursiveItems) { }
		public RecursiveItemsRecyclerView(Context context, IAttributeSet attrs, int defStyleRef) : base(context, attrs, defStyleRef)
		{
			SetAdapter(_RecursiveAdapter = new Adapter(context));
			SetLayoutManager(_RecursiveLayoutManager = new LayoutManager(context));
		}

		private Adapter _RecursiveAdapter;
		private LayoutManager _RecursiveLayoutManager;

		public Adapter RecursiveAdapter
		{
			set => SetAdapter(_RecursiveAdapter = value);
			get => GetAdapter() as Adapter ?? _RecursiveAdapter;
		}
		public LayoutManager RecursiveLayoutManager
		{
			set => SetLayoutManager(_RecursiveLayoutManager = value);
			get => GetLayoutManager() as LayoutManager ?? _RecursiveLayoutManager;
		}

		public new class LayoutManager : GridLayoutManager
		{
			public LayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
			{ }
			public LayoutManager(Context context, int spanCount = 1, int orientation = Vertical, bool reverseLayout = false) : base(context, spanCount, orientation, reverseLayout)
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
		public new class Adapter : RecyclerView.Adapter
		{
			public Adapter(Context context)
			{
				Context = context;
			}

			public Context Context { get; set; }
			public RecyclerView? Parent { get; set; }
			public Func<int>? GetItemCount { get; set; }

			public Action<ViewHolder, int>? ViewHolderOnBind { get; set; }
			public Func<ViewGroup, int, ViewHolder>? ViewHolderOnCreate { get; set; }
			
			public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnCheckChange { get; set; }
			public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnClick { get; set; }
			public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnLongClick { get; set; }

			public override int ItemCount
			{
				get => GetItemCount?.Invoke() ?? 0;
			}
			public override void OnViewAttachedToWindow(Java.Lang.Object holder)
			{
				base.OnViewAttachedToWindow(holder);

				ViewHolder viewholder = (ViewHolder)holder;

				viewholder.OnCheckedChange += ViewHolder_OnCheckChange;
				viewholder.OnClick += ViewHolder_OnClick;
				viewholder.OnLongClick += ViewHolder_OnLongClick;
			}
			public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
			{
				base.OnViewDetachedFromWindow(holder);

				ViewHolder viewholder = (ViewHolder)holder;

				viewholder.OnCheckedChange -= ViewHolder_OnCheckChange;
				viewholder.OnClick -= ViewHolder_OnClick;
				viewholder.OnLongClick -= ViewHolder_OnLongClick;
			}
			public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
			{
				ViewHolderOnBind?.Invoke((ViewHolder)holder, position);
			}
			public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
			{
				return ViewHolderOnCreate?.Invoke(parent, viewType) ?? new ViewHolder(new View(Context));
			}

			private void ViewHolder_OnCheckChange(object? sender, RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
			{
				ViewHolderOnCheckChange?.Invoke(args);
			}																									 
			private void ViewHolder_OnClick(object? sender, RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
			{
				ViewHolderOnClick?.Invoke(args);
			}							
			private void ViewHolder_OnLongClick(object? sender, RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
			{
				ViewHolderOnLongClick?.Invoke(args);
			}						   
		}
		public new class ViewHolder : RecyclerViewViewHolderDefault
		{
			public ViewHolder(View itemView) : base(itemView) { }

			public RecursiveItemsRecyclerView? ItemChildren { get; set; }
		}
	}
}