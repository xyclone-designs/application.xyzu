#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;

using System;

using Xyzu.Droid;
using Xyzu.Widgets.RecyclerViews;

namespace Xyzu.Views.Option
{
	public class OptionsCreateAndViewView : OptionsView, TextView.IOnEditorActionListener
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_option_createandview;

			public const int CreateName_AppCompatEditText = Resource.Id.xyzu_view_option_createandview_createname_appcompatedittext;
			public const int CreateButton_AppCompatImageButton = Resource.Id.xyzu_view_option_createandview_createbutton_appcompatimagebutton;
			public const int CreateMessage_AppCompatTextView = Resource.Id.xyzu_view_option_createandview_createmessage_appcompattextview;
			public const int View_SimpleVerticalRecyclerView = Resource.Id.xyzu_view_option_createandview_view_simpleverticalrecyclerview;
		}

		public OptionsCreateAndViewView(Context context) : this(context, null!)
		{ }
		public OptionsCreateAndViewView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Option_CreateAndView)
		{ }
		public OptionsCreateAndViewView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_Option_CreateAndView)
		{ }
		public OptionsCreateAndViewView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{ }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			base.Init(context, attrs);

			Inflate(context, Ids.Layout, this);

			View.SimpleMarginItemDecoration.MarginResVertical = Resource.Dimension.dp0;
			View.SimpleMarginItemDecoration.MarginResHorizontal = Resource.Dimension.dp16;
			View.SimpleAdapter.GetItemCount = ViewGetItemCount;
			View.SimpleAdapter.ViewHolderOnBind = ViewViewHolderOnBind;
			View.SimpleAdapter.ViewHolderOnCreate = ViewViewHolderOnCreate;
			View.SimpleAdapter.ViewHolderOnClick = ViewViewHolderOnClick;
		}
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			CreateName.SetOnEditorActionListener(this);

			CreateButton.Click += CreateButton_Click;
			CreateName.TextChanged += CreateName_TextChanged;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			CreateName.SetOnEditorActionListener(null);

			CreateButton.Click -= CreateButton_Click;
			CreateName.TextChanged -= CreateName_TextChanged;
		}

		private AppCompatEditText? _CreateName;
		private AppCompatImageButton? _CreateButton;
		private AppCompatTextView? _CreateMessage;
		private SimpleVerticalRecyclerView? _View;

		public AppCompatEditText CreateName
		{
			get => _CreateName ??= FindViewById(Ids.CreateName_AppCompatEditText) as AppCompatEditText ?? throw new InflateException();
		}		
		public AppCompatImageButton CreateButton
		{
			get => _CreateButton ??= FindViewById(Ids.CreateButton_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}					
		public AppCompatTextView CreateMessage
		{
			get => _CreateMessage ??= FindViewById(Ids.CreateMessage_AppCompatTextView) as AppCompatTextView ?? throw new InflateException();
		}
		public SimpleVerticalRecyclerView View
		{
			get => _View ??= FindViewById(Ids.View_SimpleVerticalRecyclerView) as SimpleVerticalRecyclerView ?? throw new InflateException();
		}

		public bool WithCreate
		{
			get =>
				CreateName.Visibility == ViewStates.Visible &&
				CreateButton.Visibility == ViewStates.Visible;

			set =>
				(CreateName.Visibility, CreateButton.Visibility) = value
					? (ViewStates.Visible, ViewStates.Visible)
					: (ViewStates.Gone, ViewStates.Gone);
		}		 
		public bool WithView
		{
			get => View.Visibility == ViewStates.Visible;
			set => View.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
		}

		public Func<OptionsCreateAndViewView, string?, bool>? OnCreate { get; set; }
		public Func<OptionsCreateAndViewView, string?, bool>? OnCreateTextChanged { get; set; }

		public Func<int>? ViewGetItemCountFunc { get; set; }
		public Action<RecyclerViewViewHolderDefault, int>? ViewViewHolderOnBindAction { get; set; }
		public Func<ViewGroup?, int, RecyclerViewViewHolderDefault>? ViewViewHolderOnCreateFunc { get; set; }
		public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewViewHolderOnClickAction { get; set; }

		private void _SetMessage(string? text)
		{
			if (text is null)
				CreateMessage.Visibility = ViewStates.Gone;
			else
				CreateMessage.Visibility = ViewStates.Visible;

			CreateMessage.SetText(text, null);
		}		   	
		public void SetMessageInfo(string? text)
		{
			_SetMessage(text);

			if (Context?.Resources?.GetColor(Resource.Color.ColorOnSurface, Context.Theme) is Color color)
				CreateMessage.SetTextColor(color);
		}				   
		public void SetMessageError(string? text)
		{
			_SetMessage(text);

			if (Context?.Resources?.GetColor(Resource.Color.ColorError, Context.Theme) is Color color)
				CreateMessage.SetTextColor(color);
		}				    
		public void SetMessageSuccess(string? text)
		{
			_SetMessage(text);

			if (Context?.Resources?.GetColor(Resource.Color.ColorSuccess, Context.Theme) is Color color)
				CreateMessage.SetTextColor(color);
		}

		public bool OnEditorAction(TextView? v, [GeneratedEnum] ImeAction actionId, KeyEvent? e)
		{
			switch (actionId)
			{
				case ImeAction.Done when OnCreate != null:
					if (OnCreate.Invoke(this, CreateName.Text))
						CreateName.Text = null;
					break;

				default: break;
			}

			return false;
		}

		public int ViewGetItemCount()
		{
			return ViewGetItemCountFunc?.Invoke() ?? 0;
		}
		public void ViewViewHolderOnBind(RecyclerViewViewHolderDefault recyclerviewviewholderdefault, int position)
		{
			ViewViewHolderOnBindAction?.Invoke(recyclerviewviewholderdefault, position);
		}
		public RecyclerViewViewHolderDefault ViewViewHolderOnCreate(ViewGroup? parent, int viewtype)
		{
			return 
				ViewViewHolderOnCreateFunc?.Invoke(parent, viewtype) ??
				new ViewHolder(Context ?? throw new ArgumentException("No Context"))
				{
					ItemParent = View,
				};
		}
		public void ViewViewHolderOnClick(RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
		{
			ViewViewHolderOnClickAction?.Invoke(args);
		}

		private void CreateButton_Click(object sender, EventArgs args)
		{
			CreateName.OnEditorAction(ImeAction.Done);
		}
		private void CreateName_TextChanged(object sender, TextChangedEventArgs args)
		{
			OnCreateTextChanged?.Invoke(this, args.Text is null ? null : string.Join("", args.Text));
		}

		public class ViewHolder : RecyclerViewViewHolderDefault
		{
			private static AppCompatButton ItemViewDefault(Context context)
			{
				ContextThemeWrapper contextthemewrapper = new ContextThemeWrapper(context, Resource.Style.Xyzu_View_Option_CreateAndView_RecyclerView_ItemView);
				AppCompatButton itemview = new AppCompatButton(contextthemewrapper, null!, Resource.Style.Xyzu_View_Option_CreateAndView_RecyclerView_ItemView)
				{
					LayoutParameters = new RecyclerView.LayoutParams(RecyclerView.LayoutParams.MatchParent, RecyclerView.LayoutParams.WrapContent)
				};

				return itemview;
			}

			public ViewHolder(Context context) : this(ItemViewDefault(context)) { }
			public ViewHolder(AppCompatButton itemView) : base(itemView) { }

			public new AppCompatButton ItemView
			{
				set => base.ItemView = value;
				get => (AppCompatButton)base.ItemView;
			}
			public SimpleVerticalRecyclerView? ItemParent 
			{
				get; set;
			}
		}
	}
}