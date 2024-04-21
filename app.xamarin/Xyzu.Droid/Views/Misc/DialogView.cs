#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Palette.Graphics;

using Google.Android.Material.BottomSheet;

using System;

using Xyzu.Droid;
using Xyzu.Views.Insets;

namespace Xyzu.Views.Misc
{
	public class DialogView : ConstraintLayout
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_misc_dialog;

			public const int StatusBarInset_StatusBarInsetView = Resource.Id.xyzu_view_misc_dialog_statusbarinsetview;
			public const int Title_AppCompatTextView= Resource.Id.xyzu_view_misc_dialog_title_appcompattextview;
			public const int Description_AppCompatTextView = Resource.Id.xyzu_view_misc_dialog_description_appcompattextview;
			public const int ContentView_LinearLayoutCompat = Resource.Id.xyzu_view_misc_dialog_contentview_linearlayoutcompat;
			public const int ContentView_NestedScrollView = Resource.Id.xyzu_view_misc_dialog_contentview_nestedscrollview;
			public const int Buttons_Barrier_Top = Resource.Id.xyzu_view_misc_dialog_buttons_barrier_top;
			public const int Buttons_Neutral_AppCompatButton = Resource.Id.xyzu_view_misc_dialog_buttons_neutral_appcompatbutton;
			public const int Buttons_Positive_AppCompatButton = Resource.Id.xyzu_view_misc_dialog_buttons_positive_appcompatbutton;
			public const int Buttons_Negative_AppCompatButton = Resource.Id.xyzu_view_misc_dialog_buttons_negative_appcompatbutton;
			public const int NavigationBarInset_NavigationBarInsetView = Resource.Id.xyzu_view_misc_dialog_navigationbarinsetview;
		}

		public DialogView(Context context) : this(context, null!) 
		{ }
		public DialogView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Misc_Dialog) 
		{ }
		public DialogView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_Misc_Dialog)
		{ }
		public DialogView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(_Context = context, attrs);
		}

		protected virtual void Init(Context context, IAttributeSet? attrs) 
		{
			Inflate(context, Ids.Layout, this);

			_StatusBarInset = FindViewById(Ids.StatusBarInset_StatusBarInsetView) as StatusBarInsetView;
			_Title = FindViewById(Ids.Title_AppCompatTextView) as AppCompatTextView;
			_Description = FindViewById(Ids.Description_AppCompatTextView) as AppCompatTextView;
			_Contentlinearlayoutcompat = FindViewById(Ids.ContentView_LinearLayoutCompat) as LinearLayoutCompat;
			_Contentnestedscrollview = FindViewById(Ids.ContentView_NestedScrollView) as NestedScrollView;
			_ButtonNeutral = FindViewById(Ids.Buttons_Neutral_AppCompatButton) as AppCompatButton;
			_ButtonPositive = FindViewById(Ids.Buttons_Positive_AppCompatButton) as AppCompatButton;
			_ButtonNegative = FindViewById(Ids.Buttons_Negative_AppCompatButton) as AppCompatButton;
			_NavigationBarInset = FindViewById(Ids.NavigationBarInset_NavigationBarInsetView) as NavigationBarInsetView;
		}

		protected override void OnAttachedToWindow() 
		{
			base.OnAttachedToWindow();

			if (ButtonNeutral != null) ButtonNeutral.Click += ButtonsNeutralAppCompatButton_Click;
			if (ButtonPositive != null) ButtonPositive.Click += ButtonsPositiveAppCompatButton_Click;
			if (ButtonNegative != null) ButtonNegative.Click += ButtonsNegativeAppCompatButton_Click;
		}
		protected override void OnDetachedFromWindow() 
		{
			base.OnDetachedFromWindow();

			if (ButtonNeutral != null) ButtonNeutral.Click -= ButtonsNeutralAppCompatButton_Click;
			if (ButtonPositive != null) ButtonPositive.Click -= ButtonsPositiveAppCompatButton_Click;
			if (ButtonNegative != null) ButtonNegative.Click -= ButtonsNegativeAppCompatButton_Click;
		}

		private Context _Context;

		protected AppCompatDialog? _Dialog;
		protected StatusBarInsetView? _StatusBarInset;
		protected AppCompatTextView? _Title;
		protected AppCompatTextView? _Description;
		protected LinearLayoutCompat? _Contentlinearlayoutcompat;
		protected NestedScrollView? _Contentnestedscrollview;
		protected AppCompatButton? _ButtonNeutral;
		protected AppCompatButton? _ButtonPositive;
		protected AppCompatButton? _ButtonNegative;
		protected NavigationBarInsetView? _NavigationBarInset;

		protected Action<IDialogInterface?>? _OnClickNeutral;
		protected Action<IDialogInterface?>? _OnClickPositive;
		protected Action<IDialogInterface?>? _OnClickNegative;

		public new Context Context
		{
			get => base.Context ?? _Context;
		}

		public AppCompatDialog? Dialog
		{
			get => _Dialog;
			set
			{
				_Dialog = value;

				SetBackgroundResource(true switch
				{
					true when _Dialog is BottomSheetDialog => Resource.Drawable.shape_cornered_top,

					_ => Resource.Drawable.shape_cornered_all,
				});
			}
		}

		public StatusBarInsetView StatusBarInset
		{
			get => _StatusBarInset ?? throw new InflateException();
		}
		public AppCompatTextView Title
		{
			get => _Title ?? throw new InflateException();
		}	 					 
		public AppCompatTextView Description
		{
			get => _Description ?? throw new InflateException();
		}	 			 
		public LinearLayoutCompat Contentlinearlayoutcompat
		{
			get => _Contentlinearlayoutcompat ?? throw new InflateException();
		}	 					 
		public NestedScrollView Contentnestedscrollview
		{
			get => _Contentnestedscrollview ?? throw new InflateException();
		}	 
		public AppCompatButton ButtonNeutral 
		{
			get => _ButtonNeutral ?? throw new InflateException(); 
		}
		public AppCompatButton ButtonPositive 
		{
			get => _ButtonPositive ?? throw new InflateException(); 
		}
		public AppCompatButton ButtonNegative 
		{
			get => _ButtonNegative ?? throw new InflateException(); 
		}
		public NavigationBarInsetView NavigationBarInset
		{
			get => _NavigationBarInset ?? throw new InflateException();
		}

		public string? TitleText
		{
			get => Title.Text;
			set => Title.Visibility = (Title.Text = value) is null
				? ViewStates.Gone
				: ViewStates.Visible;
		}	 
		public string? DescriptionText
		{
			get => Description.Text;
			set => Description.Visibility = (Description.Text = value) is null
				? ViewStates.Gone
				: ViewStates.Visible;
		}
		public View? ContentView 
		{
			get => Contentnestedscrollview?.GetChildAt(0);
			set
			{
				Contentnestedscrollview?.RemoveAllViews();
				Contentnestedscrollview?.AddView(value, 0);
			}
		}
		public int ContentViewMaxHeight
		{
			set
			{
				if (Contentlinearlayoutcompat.LayoutParameters is LayoutParams layoutparams)
				{
					layoutparams.MatchConstraintMaxHeight = value;

					Contentlinearlayoutcompat.LayoutParameters = layoutparams;
				}
			}
		}
		public bool WithStatusBarInset
		{
			get => StatusBarInset.Visibility == ViewStates.Visible;
			set => StatusBarInset.Visibility = value ? ViewStates.Visible: ViewStates.Gone;
		}																				  
		public bool WithNavigationBarInset
		{
			get => NavigationBarInset.Visibility == ViewStates.Visible;
			set => NavigationBarInset.Visibility = value ? ViewStates.Visible: ViewStates.Gone;
		}
		public int ButtonsNeutralText
		{
			set	=> ButtonNeutral?.SetText(value);
		}
		public int ButtonsPositiveText
		{
			set	=> ButtonPositive?.SetText(value);
		}
		public int ButtonsNegativeText
		{
			set	=> ButtonNegative?.SetText(value);
		}
		public Palette? Palette
		{
			set
			{
				if (value?.GetColorForBackground(Context, Resource.Color.ColorSurface) is Color color)
				{
					ButtonNegative?.SetTextColor(color);
					ButtonNeutral?.SetTextColor(color);
					ButtonPositive?.SetTextColor(color);
				}
			}
		}

		public Action<IDialogInterface?>? OnClickNeutral 
		{ 
			get => _OnClickNeutral; 
			set => ButtonNeutral.Visibility = (_OnClickNeutral = value) is null
				? ViewStates.Gone
				: ViewStates.Visible;
		}
		public Action<IDialogInterface?>? OnClickPositive 
		{ 
			get => _OnClickPositive; 
			set => ButtonPositive.Visibility = (_OnClickPositive = value) is null
				? ViewStates.Gone
				: ViewStates.Visible;
		}
		public Action<IDialogInterface?>? OnClickNegative
		{
			get => _OnClickNegative;
			set => ButtonNegative.Visibility = (_OnClickNegative = value) is null
				? ViewStates.Gone
				: ViewStates.Visible;
		}

		private void ButtonsNeutralAppCompatButton_Click(object sender, EventArgs args) 
		{
			OnClickNeutral?.Invoke(Dialog);
		}
		private void ButtonsPositiveAppCompatButton_Click(object sender, EventArgs args) 
		{
			OnClickPositive?.Invoke(Dialog);
		}
		private void ButtonsNegativeAppCompatButton_Click(object sender, EventArgs args) 
		{
			OnClickNegative?.Invoke(Dialog);
		}
	}
}