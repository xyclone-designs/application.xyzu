using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using System;

using Xyzu.Droid;

namespace Xyzu.Widgets.RecyclerViews.Simple
{
	public abstract class SimpleRecyclerView : RecyclerView
	{
		public SimpleRecyclerView(Context context) : this(context, null!) { }
		public SimpleRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_Simple) { }
		public SimpleRecyclerView(Context context, IAttributeSet attrs, int defStyleRef) : base(context, attrs, defStyleRef)
		{
			SetAdapter(_SimpleAdapter = new Adapter(context, () => -1));
			AddItemDecoration(_SimpleItemDecorationMargin = new ItemDecorationMargin
			{
				MarginRes = Resource.Dimension.dp4
			});
		}

		private Adapter _SimpleAdapter;
		public ItemDecorationMargin _SimpleItemDecorationMargin;

		public Adapter SimpleAdapter
		{
			set => SetAdapter(_SimpleAdapter = value);
			get => GetAdapter() as Adapter ?? _SimpleAdapter;
		}
		public ItemDecorationMargin SimpleItemDecorationMargin
		{
			get => _SimpleItemDecorationMargin;
			protected set => _SimpleItemDecorationMargin = value; 
		}

		public new class Adapter : RecyclerView.Adapter
		{
			public Adapter(Context context, Func<int> itemCount)
			{
				Context = context;
				FuncGetItemCount = itemCount;
			}

			public Context Context { get; set; }
			public RecyclerView? Parent { get; set; }
			public Func<int> FuncGetItemCount { get; set; }
			public Func<int, int>? FuncGetItemViewType { get; set; }
			public Action<RecyclerViewViewHolderDefault, int>? ViewHolderOnBind { get; set; }
			public Func<ViewGroup, int, RecyclerViewViewHolderDefault>? ViewHolderOnCreate { get; set; }
			public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnCheckChange { get; set; }
			public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnClick { get; set; }
			public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnLongClick { get; set; }

			public override int ItemCount
			{
				get => FuncGetItemCount.Invoke();
			}
			public override int GetItemViewType(int position)
			{
				return FuncGetItemViewType?.Invoke(position) ?? base.GetItemViewType(position);
			}
			public override void OnViewAttachedToWindow(Java.Lang.Object holder)
			{
				base.OnViewAttachedToWindow(holder);

				RecyclerViewViewHolderDefault viewholder = (RecyclerViewViewHolderDefault)holder;

				viewholder.OnCheckedChange += ViewHolder_OnCheckChange;
				viewholder.OnClick += ViewHolder_OnClick;
				viewholder.OnLongClick += ViewHolder_OnLongClick;
			}
			public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
			{
				base.OnViewDetachedFromWindow(holder);

				RecyclerViewViewHolderDefault viewholder = (RecyclerViewViewHolderDefault)holder;

				viewholder.OnCheckedChange -= ViewHolder_OnCheckChange;
				viewholder.OnClick -= ViewHolder_OnClick;
				viewholder.OnLongClick -= ViewHolder_OnLongClick;
			}
			public override void OnBindViewHolder(ViewHolder holder, int position)
			{
				ViewHolderOnBind?.Invoke((RecyclerViewViewHolderDefault)holder, position);
			}
			public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
			{
				return ViewHolderOnCreate?.Invoke(parent, viewType) ?? new RecyclerViewViewHolderDefault(new View(Context) { });
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
	}
}