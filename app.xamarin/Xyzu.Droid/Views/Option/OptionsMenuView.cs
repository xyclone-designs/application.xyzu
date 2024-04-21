
#nullable enable

using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library.Enums;
using Xyzu.Menus;
using Xyzu.Settings.Enums;
using Xyzu.Widgets.RecyclerViews;

namespace Xyzu.Views.Option
{
	public class OptionsMenuView : OptionsView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_option_menu;

			public const int All_AppCompatCheckBox = Resource.Id.xyzu_view_option_menu_all_appcompatcheckbox;
			public const int Text_AppCompatTextView = Resource.Id.xyzu_view_option_menu_text_appcompattextview;
			public const int Cancel_AppCompatImageButton = Resource.Id.xyzu_view_option_menu_cancel_appcompatimagebutton;
			public const int MenuOptions_SimpleHorizontalRecyclerView = Resource.Id.xyzu_view_option_menu_menu_simplehorizontalrecyclerview;
		}

		public OptionsMenuView(Context context) : this(context, null!)
		{ }
		public OptionsMenuView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Option_Menu)
		{ }
		public OptionsMenuView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_Option_Menu)
		{ }
		public OptionsMenuView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{ }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			base.Init(context, attrs);

			Inflate(context, Ids.Layout, this);

			_All = FindViewById(Ids.All_AppCompatCheckBox) as AppCompatCheckBox;
			_Text = FindViewById(Ids.Text_AppCompatTextView) as AppCompatTextView;
			_Cancel = FindViewById(Ids.Cancel_AppCompatImageButton) as AppCompatImageButton;
			_MenuOptionsRecyclerView = FindViewById(Ids.MenuOptions_SimpleHorizontalRecyclerView) as SimpleHorizontalRecyclerView;

			MenuOptionsRecyclerView.SimpleLayoutManager.SpanCount = 2;
			MenuOptionsRecyclerView.SimpleMarginItemDecoration.MarginResVertical = Resource.Dimension.dp8;
			MenuOptionsRecyclerView.SimpleMarginItemDecoration.MarginResHorizontal = Resource.Dimension.dp16;
			MenuOptionsRecyclerView.SimpleAdapter.GetItemCount = MenuOptionsGetItemCount;
			MenuOptionsRecyclerView.SimpleAdapter.ViewHolderOnBind = MenuOptionsViewHolderOnBind;
			MenuOptionsRecyclerView.SimpleAdapter.ViewHolderOnCreate = MenuOptionsViewHolderOnCreate;
			MenuOptionsRecyclerView.SimpleAdapter.ViewHolderOnClick = MenuOptionsViewHolderOnClick;
		}
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			All.Click += All_Click;
			Cancel.Click += Cancel_Click;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			All.Click -= All_Click;
			Cancel.Click -= Cancel_Click;
		}

		private AppCompatCheckBox? _All;
		private AppCompatTextView? _Text;
		private AppCompatImageButton? _Cancel;
		private SimpleHorizontalRecyclerView? _MenuOptionsRecyclerView;
		private IEnumerable<MenuOptions> _MenuOptions = Enumerable.Empty<MenuOptions>();

		private bool _WithHeaderButtons = true;
		private bool _WithHeaderText = true;
		public bool WithHeaderButtons
		{
			get => _WithHeaderButtons;
			set => (All.Visibility, Cancel.Visibility, Text.TextAlignment) = (_WithHeaderButtons = value)
				? (ViewStates.Visible, ViewStates.Visible, TextAlignment.ViewStart)
				: (ViewStates.Gone, ViewStates.Gone, TextAlignment.Center);
		}
		public bool WithHeaderText
		{
			get => _WithHeaderText;
			set => Text.Visibility = (_WithHeaderText = value)
				? ViewStates.Visible
				: ViewStates.Gone;
		}

		public AppCompatCheckBox All 
		{
			get => _All ?? throw new InflateException();
		}
		public AppCompatTextView Text 
		{
			get => _Text ?? throw new InflateException();
		}				
		public AppCompatImageButton Cancel 
		{
			get => _Cancel ?? throw new InflateException();
		}
		public SimpleHorizontalRecyclerView MenuOptionsRecyclerView 
		{
			get => _MenuOptionsRecyclerView ?? throw new InflateException();
		}

		public IEnumerable<MenuOptions> MenuOptions 
		{
			get => _MenuOptions;
			set
			{
				_MenuOptions = value;

				OnPropertyChanged();
			}
		}

		public Action<object, EventArgs>? OnAllClicked { get; set; }
		public Action<object, EventArgs>? OnCancelClicked { get; set; }
		public Func<MenuOptions, bool>? OnMenuOptionClicked { get; set; }
		
		public int MenuOptionsGetItemCount()
		{
			return MenuOptions.Count();
		}
		public void MenuOptionsViewHolderOnBind(RecyclerViewViewHolderDefault recyclerviewviewholderdefault, int position)
		{
			ViewHolder viewholder = (ViewHolder)recyclerviewviewholderdefault;

			MenuOptions menuoption = MenuOptions.ElementAt(position);
			string? menuoptiontitle = menuoption.AsStringTitle(Context);
			Drawable? menuoptiondrawable = menuoption.AsDrawable(Context);

			viewholder.MenuOption = menuoption;
			viewholder.ItemView.Activated = true;
			viewholder.ItemView.SetText(menuoptiontitle, null);
			viewholder.ItemView.SetCompoundDrawablesRelativeWithIntrinsicBounds(null, menuoptiondrawable, null, null);
		}
		public RecyclerViewViewHolderDefault MenuOptionsViewHolderOnCreate(ViewGroup? parent, int viewtype)
		{
			return new ViewHolder(Context ?? throw new ArgumentException("No Context"))
			{
				ItemParent = MenuOptionsRecyclerView,
			};
		}
		public void MenuOptionsViewHolderOnClick(RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
		{
			ViewHolder viewholder = (ViewHolder)args.ViewHolder;

			OnMenuOptionClicked?.Invoke(viewholder.MenuOption);
		}

		private void All_Click(object sender, EventArgs args)
		{
			OnAllClicked?.Invoke(sender, args);
		}
		private void Cancel_Click(object sender, EventArgs args)
		{
			OnCancelClicked?.Invoke(sender, args);
		}

		public class ViewHolder : RecyclerViewViewHolderDefault
		{
			private static AppCompatTextView ItemViewDefault(Context context)
			{
				ContextThemeWrapper contextthemewrapper = new ContextThemeWrapper(context, Resource.Style.Xyzu_View_Option_Menu_RecyclerView_ItemView);
				AppCompatTextView itemview = new AppCompatTextView(contextthemewrapper, null!, Resource.Style.Xyzu_View_Option_Menu_RecyclerView_ItemView);

				itemview.SetSingleLine(true);

				return itemview;
			}

			public ViewHolder(Context context) : this(ItemViewDefault(context)) { }
			public ViewHolder(AppCompatTextView itemView) : base(itemView) { }

			public MenuOptions MenuOption { get; set; }

			public new AppCompatTextView ItemView
			{
				set => base.ItemView = value;
				get => (AppCompatTextView)base.ItemView;
			}
			public SimpleHorizontalRecyclerView? ItemParent
			{
				get; set; 
			}
		}
	}
}