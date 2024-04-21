
#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library.Enums;
using Xyzu.Settings.Enums;
using Xyzu.Widgets.RecyclerViews;

namespace Xyzu.Views.Option
{
	public class OptionsLibraryListView : OptionsView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_option_librarylist;

			public const int Options_IsReversed_AppCompatCheckBox = Resource.Id.xyzu_view_option_librarylist_options_isreversed_appcompatcheckbox;
			public const int Options_SortKey_AppCompatTextView = Resource.Id.xyzu_view_option_librarylist_options_sortkey_appcompattextview;
			public const int Options_LayoutType_AppCompatTextView = Resource.Id.xyzu_view_option_librarylist_options_layouttype_appcompattextview;
			public const int Options_SimpleHorizontalRecyclerView = Resource.Id.xyzu_view_option_librarylist_options_simplehorizontalrecyclerview;
		}

		public OptionsLibraryListView(Context context) : this(context, null!)
		{ }
		public OptionsLibraryListView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Option_LibraryList)
		{ }
		public OptionsLibraryListView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_Option_LibraryList)
		{ }
		public OptionsLibraryListView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{ }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			base.Init(context, attrs);

			Inflate(context, Ids.Layout, this);

			_OptionsIsReversed = FindViewById(Ids.Options_IsReversed_AppCompatCheckBox) as AppCompatCheckBox;
			_OptionsLayoutType = FindViewById(Ids.Options_LayoutType_AppCompatTextView) as AppCompatTextView;
			_OptionsSortKey = FindViewById(Ids.Options_SortKey_AppCompatTextView) as AppCompatTextView;
			_OptionsRecyclerView = FindViewById(Ids.Options_SimpleHorizontalRecyclerView) as SimpleHorizontalRecyclerView;

			OptionsRecyclerView.SimpleLayoutManager.SpanCount = 1;	 
			OptionsRecyclerView.SimpleMarginItemDecoration.MarginResTop = Resource.Dimension.dp16;	 
			OptionsRecyclerView.SimpleMarginItemDecoration.MarginResHorizontal = Resource.Dimension.dp8;
			OptionsRecyclerView.SimpleAdapter.GetItemCount = OptionsGetItemCount;
			OptionsRecyclerView.SimpleAdapter.ViewHolderOnCreate = OptionsViewHolderOnCreate;
			OptionsRecyclerView.SimpleAdapter.ViewHolderOnBind = OptionsViewHolderOnBind;
			OptionsRecyclerView.SimpleAdapter.ViewHolderOnCheckChange = OptionsViewHolderOnCheckChange;
		}
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			OptionsIsReversed.CheckedChange += OptionsIsReversed_CheckedChange;
			OptionsLayoutType.Click += OptionsLayoutType_Click;
			OptionsSortKey.Click += OptionsSortKey_Click;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			OptionsIsReversed.CheckedChange -= OptionsIsReversed_CheckedChange;
			OptionsLayoutType.Click -= OptionsLayoutType_Click;
			OptionsSortKey.Click -= OptionsSortKey_Click;
		}

		private AppCompatCheckBox? _OptionsIsReversed;
		private AppCompatTextView? _OptionsLayoutType;
		private AppCompatTextView? _OptionsSortKey;
		private SimpleHorizontalRecyclerView? _OptionsRecyclerView;

		private bool _OptionsLayoutTypesExpanded = false;
		private bool _OptionsSortKeysExpanded = false;
		private bool _IsReversed = false;
		private LibraryLayoutTypes _LayoutTypeSelected = default;
		private ModelSortKeys _SortKeySelected = default;
		private IEnumerable<LibraryLayoutTypes> _LayoutTypes = Enumerable.Empty<LibraryLayoutTypes>();
		private IEnumerable<ModelSortKeys> _SortKeys = Enumerable.Empty<ModelSortKeys>();

		public AppCompatCheckBox OptionsIsReversed 
		{
			get => _OptionsIsReversed ?? throw new InflateException();
		}
		public AppCompatTextView OptionsLayoutType 
		{
			get => _OptionsLayoutType ?? throw new InflateException();
		}
		public AppCompatTextView OptionsSortKey 
		{
			get => _OptionsSortKey ?? throw new InflateException();
		}								  
		public SimpleHorizontalRecyclerView OptionsRecyclerView 
		{
			get => _OptionsRecyclerView ?? throw new InflateException();
		}

		public bool OptionsLayoutTypesExpanded 
		{
			get => _OptionsLayoutTypesExpanded;
			set
			{
				if (value == _OptionsLayoutTypesExpanded)
					return;
				
				_OptionsLayoutTypesExpanded = value;

				OptionsRecyclerView.SimpleAdapter.NotifyDataSetChanged();

				OnPropertyChanged();
			}
		}
		public bool OptionsSortKeysExpanded 
		{
			get => _OptionsSortKeysExpanded;
			set
			{
				if (value == _OptionsSortKeysExpanded)
					return;

				_OptionsSortKeysExpanded = value;

				OptionsRecyclerView.SimpleAdapter.NotifyDataSetChanged();

				OnPropertyChanged();
			}
		}
		public bool IsReversed 
		{
			get => _IsReversed;
			set
			{
				OptionsIsReversed.Selected = _IsReversed = value;

				OnPropertyChanged();
			}
		}
		public IEnumerable<LibraryLayoutTypes> LayoutTypes 
		{
			get => _LayoutTypes;
			set
			{
				_LayoutTypes = value;

				OnPropertyChanged();
			}
		}
		public LibraryLayoutTypes LayoutTypeSelected 
		{
			get => _LayoutTypeSelected;
			set
			{
				_LayoutTypeSelected = value;

				OptionsLayoutType.Text = _LayoutTypeSelected.AsStringTitle(Context) ?? _LayoutTypeSelected.ToString();

				OnPropertyChanged();
			}
		}
		public int LayoutTypeSelectedIndex 
		{
			get => LayoutTypes.Index(LayoutTypeSelected) ?? throw new Exception("'LayoutTypeSelected' was not found in 'LayoutTypes'");
		}
		public IEnumerable<ModelSortKeys> SortKeys 
		{
			get => _SortKeys;
			set
			{
				_SortKeys = value;

				OnPropertyChanged();
			}
		}
		public ModelSortKeys SortKeySelected 
		{
			get => _SortKeySelected;
			set
			{
				_SortKeySelected = value;

				OptionsSortKey.Text =
					Context?.Resources?.GetString(Resource.String.sort) is string sort
						? string.Format("{0}: {1}", sort, _SortKeySelected.AsStringTitle(Context) ?? _SortKeySelected.ToString())
						: _SortKeySelected.AsStringTitle(Context) ?? _SortKeySelected.ToString();

				OnPropertyChanged();
			}
		}
		public int SortKeySelectedIndex 
		{
			get => SortKeys.Index(SortKeySelected) ?? throw new Exception("'SortKeySelected' was not found in 'SortKeys'");
		}

		public Action<bool>? OnOptionsIsReversedClicked { get; set; }
		public Action<LibraryLayoutTypes>? OnOptionsLayoutTypeItemSelected { get; set; }
		public Action<ModelSortKeys>? OnOptionsSortKeyItemSelected { get; set; }

		public int OptionsGetItemCount()
		{
			int itemcount = true switch
			{
				true when OptionsLayoutTypesExpanded => LayoutTypes.Count(),
				true when OptionsSortKeysExpanded => SortKeys.Count(),

				_ => 0
			};

			return itemcount;
		}
		public void OptionsViewHolderOnBind(RecyclerViewViewHolderDefault recyclerviewviewholderdefault, int position)
		{
			ViewHolder viewholder = (ViewHolder)recyclerviewviewholderdefault;

			switch (true)
			{
				case true when OptionsLayoutTypesExpanded:
					LibraryLayoutTypes layouttype = LayoutTypes.ElementAt(position);

					viewholder.EnumValue = (int)layouttype;
					viewholder.ItemView.Checked = layouttype == LayoutTypeSelected;
					viewholder.ItemView.Text = layouttype.AsStringTitle(Context) ?? layouttype.ToString();
					break;

				case true when OptionsSortKeysExpanded:
					ModelSortKeys modelsortkey = SortKeys.ElementAt(position);

					viewholder.EnumValue = (int)modelsortkey;
					viewholder.ItemView.Checked = modelsortkey == SortKeySelected;
					viewholder.ItemView.Text = modelsortkey.AsStringTitle(Context) ?? modelsortkey.ToString();
					break;

				default: break;
			}
		}
		public RecyclerViewViewHolderDefault OptionsViewHolderOnCreate(ViewGroup? parent, int viewtype)
		{
			return new ViewHolder(Context ?? throw new ArgumentException("No Context"));
		}
		public void OptionsViewHolderOnCheckChange(RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
		{
			ViewHolder viewholder = (ViewHolder)args.ViewHolder;

			switch (true)
			{
				case true when OptionsLayoutTypesExpanded && (LibraryLayoutTypes)viewholder.EnumValue == LayoutTypeSelected:
					viewholder.ItemView.Checked = true;
					break;

				case true when OptionsLayoutTypesExpanded:

					int oldlayouttypeselectedindex = LayoutTypeSelectedIndex;

					LayoutTypeSelected = (LibraryLayoutTypes)viewholder.EnumValue;
					OnOptionsLayoutTypeItemSelected?.Invoke(LayoutTypeSelected);
					OptionsRecyclerView.SimpleAdapter.NotifyItemChanged(oldlayouttypeselectedindex);
					break;

				case true when OptionsSortKeysExpanded && (ModelSortKeys)viewholder.EnumValue == SortKeySelected:
					viewholder.ItemView.Checked = true;
					break;

				case true when OptionsSortKeysExpanded:
					int oldsortkeyselectedindex = SortKeySelectedIndex;

					SortKeySelected = (ModelSortKeys)viewholder.EnumValue;
					OnOptionsSortKeyItemSelected?.Invoke(SortKeySelected);
					OptionsRecyclerView.SimpleAdapter.NotifyItemChanged(oldsortkeyselectedindex);
					break;

				default: break;
			}
		}

		private void OptionsIsReversed_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs args)
		{
			OnOptionsIsReversedClicked?.Invoke(IsReversed);
		}
		private void OptionsIsReversed_Click(object sender, EventArgs args)
		{
			IsReversed = !IsReversed;

			OnOptionsIsReversedClicked?.Invoke(IsReversed);

			OptionsSortKeysExpanded = false;
			OptionsLayoutTypesExpanded = OptionsLayoutTypesExpanded is false;
		}								
		private void OptionsLayoutType_Click(object sender, EventArgs args)
		{
			OptionsSortKeysExpanded = false;
			OptionsLayoutType.Selected = OptionsLayoutTypesExpanded = OptionsLayoutTypesExpanded is false;
			OptionsRecyclerView.Visibility = OptionsLayoutTypesExpanded is false
				? ViewStates.Gone
				: ViewStates.Visible;
		}
		private void OptionsSortKey_Click(object sender, EventArgs args)
		{
			OptionsLayoutTypesExpanded = false;
			OptionsSortKey.Selected = OptionsSortKeysExpanded = OptionsSortKeysExpanded is false; 
			OptionsRecyclerView.Visibility = OptionsSortKeysExpanded is false
				? ViewStates.Gone
				: ViewStates.Visible;
		}

		public class ViewHolder : RecyclerViewViewHolderDefault
		{
			private static AppCompatCheckBox ItemViewDefault(Context context)
			{
				ContextThemeWrapper contextthemewrapper = new ContextThemeWrapper(context, Resource.Style.Xyzu_View_Option_LibraryList_RecyclerView_ItemView);
				AppCompatCheckBox itemview = new AppCompatCheckBox(contextthemewrapper);

				itemview.SetMinimumWidth(0);
				itemview.SetMinimumHeight(0);
				itemview.SetBackgroundResource(Resource.Drawable.xyzu_view_option_button_background);
				itemview.SetButtonDrawable(context.Resources?.GetDrawable(Resource.Drawable.xyzu_view_option_button_checkbox_drawable, context.Theme));

				return itemview;
			}

			public ViewHolder(Context context) : this(ItemViewDefault(context)) { }
			public ViewHolder(AppCompatCheckBox itemView) : base(itemView)
			{ }

			public int EnumValue { get; set; }

			public new AppCompatCheckBox ItemView
			{
				set => base.ItemView = value;
				get => (AppCompatCheckBox)base.ItemView;
			}
		}
	}
}