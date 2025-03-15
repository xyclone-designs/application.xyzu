using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library;
using Xyzu.Library.Models;
using Xyzu.Settings.Enums;
using Xyzu.Views.LibraryItem;

namespace Xyzu.Widgets.RecyclerViews.LibraryItems
{
	public abstract class LibraryItemsRecyclerView : RecyclerView
	{
		public LibraryItemsRecyclerView(Context context) : this(context, null!) { }
		public LibraryItemsRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_LibraryItems) { }
		public LibraryItemsRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetLayoutManager(_LibraryItemsLayoutManager = new LayoutManager(context, LibraryLayoutTypes.ListMedium));
			AddItemDecoration(_LibraryItemsItemDecoration = new MarginItemDecoration
			{
				MarginRes = Resource.Dimension.dp4,

			}, 0);
		}

		private LayoutManager _LibraryItemsLayoutManager;
		private MarginItemDecoration _LibraryItemsItemDecoration;

		public LayoutManager LibraryItemsLayoutManager
		{
			set => SetLayoutManager(_LibraryItemsLayoutManager = value);
			get => GetLayoutManager() as LayoutManager ?? _LibraryItemsLayoutManager;
		}
		public MarginItemDecoration LibraryItemsItemDecoration
		{
			set => AddItemDecoration(_LibraryItemsItemDecoration = value, 0);
			get => GetItemDecorationAt(0) as MarginItemDecoration ?? _LibraryItemsItemDecoration;
		}

		public IEnumerable<ViewHolder> FindViewHolders(int itemcount)
		{
			bool foundany = false;

			for (int position = 0; position < itemcount; position++)
				if (FindViewHolderForAdapterPosition(position) is ViewHolder viewholder)
				{
					foundany = true;
					yield return viewholder;
				}
				else if (foundany) break;
		}

		public new abstract class Adapter : RecyclerView.Adapter
		{
			public const int ItemViewType_Normal = 0;
			public const int ItemViewType_Header = 1;
			public const int ItemViewType_Footer = 2;

			public Adapter(LibraryItemsRecyclerView parent)
			{
				Parent = parent;
				Parent.LibraryItemsLayoutManager.SetSpanSizeLookup(new NewSpanSizeLookup
				{
					GetSpanSizeAction = position => GetItemViewType(position) switch
					{
						ItemViewType_Normal => 1,
						ItemViewType_Header => Parent.LibraryItemsLayoutManager.SpanCount,
						ItemViewType_Footer => Parent.LibraryItemsLayoutManager.SpanCount,

						_ => 1
					}
				});

				_RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;
			}

			private LibraryLayoutTypes _LibraryLayoutType;
			private RecyclerViewAdapterStates _RecyclerViewAdapterState;
			private RecyclerViewAdapterStates _RecyclerViewAdapterStatePrevious;
			private ViewHolder? _FocusedViewHolder;
			private IEnumerable<int>? _SelectedPositions;

			public LibraryItemsRecyclerView Parent { get; set; }
			public LibraryItemView? Header { get; set; }
			public LibraryItemView? Footer { get; set; }

			public IImages? Images { get; set; }
			public ILibrary? Library { get; set; }

			public LibraryLayoutTypes LibraryLayoutType
			{
				get => _LibraryLayoutType;
				set
				{
					_LibraryLayoutType = value;

					OnPropertyChanged();
				}
			}
			public RecyclerViewAdapterStates RecyclerViewAdapterState
			{
				get => _RecyclerViewAdapterState;
				set
				{
					_RecyclerViewAdapterStatePrevious = _RecyclerViewAdapterState;
					_RecyclerViewAdapterState = value;

					OnPropertyChanged();
				}
			}
			public ViewHolder? FocusedViewHolder
			{
				get => _FocusedViewHolder;
				set
				{
					_FocusedViewHolder = value;

					OnPropertyChanged();
				}
			}
			public IEnumerable<int> SelectedPositions
			{
				get => _SelectedPositions ??= Enumerable.Empty<int>();
				set
				{
					_SelectedPositions = value;

					OnPropertyChanged();
				}
			}

			public Action<object, PropertyChangedEventArgs>? PropertyChangedAction { get; set; }

			public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;

			public new void NotifyItemChanged(int position)
			{
				if (Header is not null)
					base.NotifyItemChanged(position + 1);
				else base.NotifyItemChanged(position);
			}

			public override int GetItemViewType(int position)
			{
				return true switch
				{
					true when
					ItemCount == 0
						=> ItemViewType_Normal,

					true when
					Header != null &&
					position == 0
						=> ItemViewType_Header,

					true when
					Footer != null &&
					position == ItemCount - 1
						=> ItemViewType_Footer,

					_ => ItemViewType_Normal,
				};
			}

			public void SelectLibraryItems(params int[] positions)
			{
				if (RecyclerViewAdapterState != RecyclerViewAdapterStates.Select)
					RecyclerViewAdapterState = RecyclerViewAdapterStates.Select;
				foreach (int selectedposition in SelectedPositions)
					if
					(
						positions.Contains(selectedposition) is false &&
						Parent.FindViewHolderForAdapterPosition(selectedposition) is ViewHolder viewholder &&
						viewholder.ItemView.Selected

					) viewholder.ItemView.Selected = false;

				SelectedPositions = positions.Distinct();
			}
			public void SelectLibraryItemsAll(bool deselectable = true)
			{
				if (RecyclerViewAdapterState != RecyclerViewAdapterStates.Select)
					RecyclerViewAdapterState = RecyclerViewAdapterStates.Select;

				switch (SelectedPositions.Count() == ItemCount, deselectable)
				{
					case (true, true):
						SelectedPositions = Enumerable.Empty<int>();
						break;

					case (true, false):
						break;

					case (false, true):
					case (false, false):
					default:
						SelectedPositions = Enumerable.Range(0, ItemCount);
						break;
				}
			}
			public void SelectLibraryItemsFocused(bool deselectable = true)
			{
				if (FocusedViewHolder is null)
					return;

				if (RecyclerViewAdapterState != RecyclerViewAdapterStates.Select)
					RecyclerViewAdapterState = RecyclerViewAdapterStates.Select;

				FocusedViewHolder.ItemView.Selected = FocusedViewHolder.ItemView.Selected is false;

				switch (SelectedPositions.Contains(FocusedViewHolder.LayoutPosition), deselectable)
				{
					case (true, true):
						FocusedViewHolder.ItemView.Selected = false;
						SelectedPositions = SelectedPositions.Except(Enumerable.Empty<int>().Append(FocusedViewHolder.LayoutPosition));
						break;

					case (true, false):
						break;

					case (false, true):
					case (false, false):
					default:
						FocusedViewHolder.ItemView.Selected = true;
						SelectedPositions = SelectedPositions.Append(FocusedViewHolder.LayoutPosition);
						break;
				}
			}

			protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null)
			{
				switch (propertyname)
				{
					case nameof(RecyclerViewAdapterState) when
					_RecyclerViewAdapterState == RecyclerViewAdapterStates.Normal &&
					_RecyclerViewAdapterStatePrevious == RecyclerViewAdapterStates.Select:
						if (Parent != null)
							foreach (ViewHolder viewholder in Parent.FindViewHolders(ItemCount))
								viewholder.ItemView.Selected = false;
						SelectedPositions = Enumerable.Empty<int>();
						break;

					case nameof(SelectedPositions):
						foreach (int selectedposition in SelectedPositions)
							if (Parent?.FindViewHolderForAdapterPosition(selectedposition) is ViewHolder viewholder && viewholder.ItemView.Selected is false)
								viewholder.ItemView.Selected = true;
						break;

					default: break;
				}

				PropertyChangedEventArgs propertychangedeventargs = new (propertyname ?? string.Empty);

				PropertyChanged?.Invoke(this, propertychangedeventargs);
				PropertyChangedAction?.Invoke(this, propertychangedeventargs);
			}

			public class NewSpanSizeLookup : GridLayoutManager.SpanSizeLookup
			{
				public Func<int, int>? GetSpanSizeAction { get; set; }

				public override int GetSpanSize(int position)
				{
					return GetSpanSizeAction?.Invoke(position) ?? 1;
				}
			}
		}
		public class Adapter<TModel> : Adapter where TModel : class, IModel
		{
			public Adapter(LibraryItemsRecyclerView parent) : base(parent) { }


			private ObservableList<TModel>? _LibraryItems;
			private IEnumerable<ISong> AsSongs(IEnumerable<TModel> models)
			{
				foreach (IModel model in models)
					switch (true)
					{
						case true when
						model is IAlbum album &&
						album.SongIds != null:
							{
								foreach (string songid in album.SongIds)
									yield return new ISong.Default(songid)
									{
										Album = album.Title,
										AlbumArtist = album.Artist,
									};
							}
							break;

						case true when
						model is IArtist artist &&
						artist.SongIds != null:
							{
								foreach (string songid in artist.SongIds)
									yield return new ISong.Default(songid)
									{
										Artist = artist.Name,
									};
							}
							break;

						case true when
						model is IGenre genre &&
						genre.SongIds != null:
							{
								foreach (string songid in genre.SongIds)
									yield return new ISong.Default(songid)
									{
										Genre = genre.Name,
									};
							}
							break;

						case true when
						model is IPlaylist:
							break;

						case true when
						model is ISong song:
							yield return song;
							break;

						default: throw new ArgumentException("Could not cast model");
					}
			}

			public ObservableList<TModel> LibraryItems
			{
				get
				{
					if (_LibraryItems is null)
					{
						_LibraryItems = new ObservableList<TModel>();
						_LibraryItems.ListChanged += LibraryItemsListChanged;
					}

					return _LibraryItems;
				}
				set
				{
					if (_LibraryItems != null)
						_LibraryItems.ListChanged -= LibraryItemsListChanged;

					_LibraryItems = value;

					if (_LibraryItems != null)
						_LibraryItems.ListChanged += LibraryItemsListChanged;
				}
			}

			public TModel? FocusedLibraryItem
			{
				get
				{
					if (FocusedViewHolder?.LibraryItem?.LibraryItemId != null)
						if (LibraryItems.FirstOrDefault(libraryitem => libraryitem.Id == FocusedViewHolder.LibraryItem.LibraryItemId) is TModel tmodel)
							return tmodel;

					return null;
				}
			}
			public IEnumerable<TModel> SelectedLibraryItems
			{
				get
				{
					foreach (int position in SelectedPositions)
						if (LibraryItems.ElementAtOrDefault(Header is null ? position : position - 1) is TModel tmodel)
							yield return tmodel;
				}
			}
			public IEnumerable<ISong> LibraryItemsAsSongs
			{
				get => AsSongs(LibraryItems);
			}
			public IEnumerable<ISong> SelectedLibraryItemsAsSongs
			{
				get => AsSongs(SelectedLibraryItems);
			}

			public Func<ViewGroup, int, RecyclerView.ViewHolder?>? ViewHolderOnCreate { get; set; }
			public Action<ViewHolder, int>? ViewHolderOnBind { get; set; }
			public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnEvent { get; set; }

			public void SetLibraryItems(IEnumerable<TModel> libraryitems, bool notifydatasetchanged = true)
			{
				LibraryItems = new ObservableList<TModel>(libraryitems);

				if (notifydatasetchanged)
					NotifyDataSetChanged();
			}

			public void NotifyDataSetChanged(LibraryLayoutTypes librarylayouttype)
			{
				LibraryLayoutType = librarylayouttype;

				NotifyDataSetChanged();
			}

			public new void NotifyItemChanged(int position)
			{
				NotifyItemRemoved(position);
				NotifyItemInserted(position);

				base.NotifyItemChanged(position);
			}
			public new void NotifyItemChanged(int position, Java.Lang.Object payload)
			{
				NotifyItemRemoved(position);
				NotifyItemInserted(position);

				base.NotifyItemChanged(position, payload);
			}

			public override int ItemCount
			{
				get
				{
					int itemcount = LibraryItems.Count;

					if (itemcount == 0)
						return itemcount;

					if (Header != null)
						itemcount += 1;

					if (Footer != null)
						itemcount += 1;

					return itemcount;
				}
			}

			public override void OnViewAttachedToWindow(Java.Lang.Object holder)
			{
				base.OnViewAttachedToWindow(holder);

				if (holder is ViewHolder viewholder && viewholder.ItemView != Header && viewholder.ItemView != Footer)
				{
					viewholder.OnClick += ViewHolder_OnClick;
					viewholder.OnLongClick += ViewHolder_OnLongClick;
				}
			}
			public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
			{
				base.OnViewAttachedToWindow(holder);

				if (holder is ViewHolder viewholder && viewholder.ItemView != Header && viewholder.ItemView != Footer)
				{
					viewholder.OnClick -= ViewHolder_OnClick;
					viewholder.OnLongClick -= ViewHolder_OnLongClick;
				}
			}
			public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
			{
				switch (true)
				{
					case true when Header != null && position == 0:
					case true when Footer != null && position == ItemCount - 1:
						break;

					case true when holder is ViewHolder viewholder:

						if (Header != null) position -= 1;

						ViewHolderOnBind?.Invoke(viewholder, position);

						viewholder.ItemView.Selected = SelectedPositions.Contains(position);

						break;

					default: break;
				}
			}

			public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
			{
				if (viewType == ItemViewType_Header && Header is not null)
				{
					int index = Parent.IndexOfChild(Header);

					if (index is not -1) Parent.DetachViewFromParent(Header);

					return new ViewHolder(Header);
				}
				if (viewType == ItemViewType_Footer && Footer is not null)
				{
					int index = Parent.IndexOfChild(Footer);

					if (index is not -1) Parent.DetachViewFromParent(Footer);

					return new ViewHolder(Footer);
				}
				
				RecyclerView.ViewHolder viewholder = true switch
				{
					true when parent.Context is null => throw new Exception("Could Not Create ViewHolder"),
					true when ViewHolderOnCreate?.Invoke(parent, viewType) is RecyclerView.ViewHolder viewholderoncreateviewholder => viewholderoncreateviewholder,
					true when LibraryLayoutType switch
					{
						LibraryLayoutTypes.GridSmall => new Views.LibraryItem.Grid.GridSmallView(parent.Context)
						{
							Images = Images,
							Library = Library,
						},
						LibraryLayoutTypes.GridMedium => new Views.LibraryItem.Grid.GridMediumView(parent.Context)
						{
							Images = Images,
							Library = Library,
						},
						LibraryLayoutTypes.GridLarge => new Views.LibraryItem.Grid.GridLargeView(parent.Context)
						{
							Images = Images,
							Library = Library,
						},
						LibraryLayoutTypes.ListSmall => new Views.LibraryItem.List.ListSmallView(parent.Context)
						{
							Images = Images,
							Library = Library,
						},
						LibraryLayoutTypes.ListMedium => new Views.LibraryItem.List.ListMediumView(parent.Context)
						{
							Images = Images,
							Library = Library,
						},
						LibraryLayoutTypes.ListLarge => new Views.LibraryItem.List.ListLargeView(parent.Context)
						{
							Images = Images,
							Library = Library,
						},

						_ => null as LibraryItemView,

					} is LibraryItemView libraryitemview => new ViewHolder(libraryitemview),

					_ => throw new Exception("Could Not Create ViewHolder"),
				};

				return viewholder;
			}

			protected override void OnPropertyChanged([CallerMemberName] string? propertyname = null)
			{
				base.OnPropertyChanged(propertyname);

				switch (propertyname)
				{
					case nameof(FocusedViewHolder):
						base.OnPropertyChanged(nameof(FocusedLibraryItem));
						break;

					case nameof(SelectedPositions):
						base.OnPropertyChanged(nameof(SelectedLibraryItems));
						break;

					default: break;
				}
			}

			protected virtual void LibraryItemsListChanged(object? sender, NotifyListChangedEventArgs args)
			{
				switch (args.Action)
				{
					case NotifyListChangedAction.Add:
						NotifyItemInserted(args.NewStartingIndex);
						break;

					case NotifyListChangedAction.AddRange:
						NotifyItemRangeInserted(args.NewStartingIndex, args.RangeCount);
						break;

					case NotifyListChangedAction.Move:
						NotifyItemMoved(args.OldStartingIndex, args.NewStartingIndex);
						break;

					case NotifyListChangedAction.MoveRange:
						NotifyItemRangeRemoved(args.OldStartingIndex, args.RangeCount);
						NotifyItemRangeInserted(args.NewStartingIndex, args.RangeCount);
						break;

					case NotifyListChangedAction.Remove:
						NotifyItemRemoved(args.NewStartingIndex);
						break;

					case NotifyListChangedAction.RemoveRange:
						NotifyItemRangeRemoved(args.NewStartingIndex, args.RangeCount);
						break;

					case NotifyListChangedAction.Replace:
						NotifyItemChanged(args.NewStartingIndex, false);
						break;

					case NotifyListChangedAction.ReplaceRange:
						NotifyItemRangeChanged(args.NewStartingIndex, args.RangeCount);
						break;

					case NotifyListChangedAction.Reverse:
					case NotifyListChangedAction.Refresh:
					case NotifyListChangedAction.Reset:
						NotifyDataSetChanged();
						break;

					default: break;
				}

				Parent?.InvalidateItemDecorations();
			}
			protected virtual void ViewHolder_OnLongClick(object? sender, RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
			{
				FocusedViewHolder = args.ViewHolder as ViewHolder;

				ViewHolderOnEvent?.Invoke(args);
			}
			protected virtual void ViewHolder_OnClick(object? sender, RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
			{
				FocusedViewHolder = args.ViewHolder as ViewHolder;

				ViewHolderOnEvent?.Invoke(args);
			}
		}
		public new class LayoutManager : GridLayoutManager
		{
			public LayoutManager(Context context, LibraryLayoutTypes librarylayouttype) : this(context, librarylayouttype, Vertical, false) { }
			public LayoutManager(Context context, LibraryLayoutTypes librarylayouttype, int orientation, bool reverseLayout) :
				base(context, librarylayouttype.ToGridLayoutManagerSpan(context), orientation, reverseLayout)
			{
				Context = context;
			}
			public LayoutManager(Context context, int spancount, int orientation, bool reverseLayout) : base(context, spancount, orientation, reverseLayout)
			{
				Context = context;
			}

			private LibraryLayoutTypes _LibraryLayoutType;

			public Context Context { get; }
			public LibraryLayoutTypes LibraryLayoutType
			{
				get => _LibraryLayoutType;
				set => SpanCount = (_LibraryLayoutType = value).ToGridLayoutManagerSpan(Context);
			}

			public void RequestLayout(int spancount)
			{
				SpanCount = spancount;

				RequestLayout();
			}
			public void RequestLayout(LibraryLayoutTypes librarylayouttype)
			{
				RequestLayout((LibraryLayoutType = librarylayouttype).ToGridLayoutManagerSpan(Context));
			}

			public override RecyclerView.LayoutParams GenerateDefaultLayoutParams()
			{
				return new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			}
			public override void OnLayoutChildren(Recycler? recycler, State? state)
			{
				try { base.OnLayoutChildren(recycler, state); }
				catch (Java.Lang.IndexOutOfBoundsException)
				{
					// IndexOutOfBoundsException Exception on sucessive list refreshes 
					// https://stackoverflow.com/questions/31759171/recyclerview-and-java-lang-indexoutofboundsexception-inconsistency-detected-in
				}
				catch (Exception) { }
			}
		}
		public new class ViewHolder : RecyclerViewViewHolderDefault
		{
			public ViewHolder(View itemView) : base(itemView) { }
			public ViewHolder(LibraryItemView itemView) : base(itemView) { }

			public ILibraryItem LibraryItem
			{
				get => ItemView;
			}
			public new LibraryItemView ItemView
			{
				set => base.ItemView = value;
				get => (LibraryItemView)base.ItemView;
			}

			public ViewStates Visibility
			{
				set
				{
					ItemView.Artwork?.SetVisibility(value);
					ItemView.LineOne?.SetVisibility(value);
					ItemView.LineTwo?.SetVisibility(value);
					ItemView.LineThree?.SetVisibility(value);
					ItemView.LineFour?.SetVisibility(value);
					ItemView.SetVisibility(value);
				}
			}
		}
	}
	public abstract class LibraryItemsRecyclerView<TModel> : LibraryItemsRecyclerView where TModel : class, IModel
	{
		public LibraryItemsRecyclerView(Context context) : this(context, null!)
		{ }
		public LibraryItemsRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_LibraryItems)
		{ }
		public LibraryItemsRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		public abstract Adapter<TModel> LibraryItemsAdapter { get; set; }

		public void ReloadLayout()
		{
			Adapter<TModel> libraryitemsadapter = LibraryItemsAdapter;
			LayoutManager libraryitemslayoutmanager = LibraryItemsLayoutManager;

			SetAdapter(null);
			SetLayoutManager(null);

			libraryitemslayoutmanager.RequestLayout();
			libraryitemsadapter.NotifyDataSetChanged();

			LibraryItemsAdapter = libraryitemsadapter;
			LibraryItemsLayoutManager = libraryitemslayoutmanager;
		}
		public void ReloadLayout(LibraryLayoutTypes librarylayouttype)
		{
			Adapter<TModel> libraryitemsadapter = LibraryItemsAdapter;
			LayoutManager libraryitemslayoutmanager = LibraryItemsLayoutManager;

			SetAdapter(null);
			SetLayoutManager(null);

			libraryitemslayoutmanager.RequestLayout(librarylayouttype);
			libraryitemsadapter.NotifyDataSetChanged(librarylayouttype);

			LibraryItemsAdapter = libraryitemsadapter;
			LibraryItemsLayoutManager = libraryitemslayoutmanager;
		}
	}
}