#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;
using Xyzu.Views.Option;

namespace Xyzu.Views.LibraryItem.Header
{
	public class HeaderLibraryItemsView : HeaderView
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_libraryitem_header_libraryitems;

			public const int Play_AppCompatImageButton = Resource.Id.xyzu_view_libraryitem_header_libraryitems_play_appcompatimagebutton;
			public const int Text_AppCompatTextView = Resource.Id.xyzu_view_libraryitem_header_libraryitems_text_appcompattextview;
			public const int LibraryItemsOptionsExpanded_AppCompatImageButton = Resource.Id.xyzu_view_libraryitem_header_libraryitems_libraryitemsoptionsexpanded_appcompatimagebutton;
			public const int Options_AppCompatImageButton = Resource.Id.xyzu_view_libraryitem_header_libraryitems_options_appcompatimagebutton;
			public const int LibraryItemsOptions_OptionsLibraryListView = Resource.Id.xyzu_view_libraryitem_header_libraryitems_libraryitemsoptions_optionslibrarylistview;
		}

		public HeaderLibraryItemsView(Context context) : base(context) { }
		public HeaderLibraryItemsView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public HeaderLibraryItemsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);
		}
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			Text.Click += Text_Click;
			Play.Click += Play_Click;
			LibraryItemsOptionsExpanded.Click += LibraryItemsOptionsExpanded_Click;
			Options.Click += Options_Click;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			Text.Click -= Text_Click;
			Play.Click -= Play_Click;
			LibraryItemsOptionsExpanded.Click -= LibraryItemsOptionsExpanded_Click;
			Options.Click -= Options_Click;
		}

		protected AppCompatTextView? _Text;
		protected AppCompatImageButton? _Play;
		protected AppCompatImageButton? _LibraryItemsOptionsExpanded;
		protected AppCompatImageButton? _Options;
		protected OptionsLibraryListView? _LibraryItemsOptions;

		public AppCompatTextView Text
		{
			get => _Text ??= FindViewById(Ids.Text_AppCompatTextView) as AppCompatTextView ?? throw new InflateException();
		}
		public AppCompatImageButton Play
		{
			get => _Play ??= FindViewById(Ids.Play_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}
		public AppCompatImageButton LibraryItemsOptionsExpanded
		{
			get => _LibraryItemsOptionsExpanded ??= FindViewById(Ids.LibraryItemsOptionsExpanded_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}
		public AppCompatImageButton Options
		{
			get => _Options ??= FindViewById(Ids.Options_AppCompatImageButton) as AppCompatImageButton ?? throw new InflateException();
		}
		public OptionsLibraryListView LibraryItemsOptions
		{
			get => _LibraryItemsOptions ??= FindViewById(Ids.LibraryItemsOptions_OptionsLibraryListView) as OptionsLibraryListView ?? throw new InflateException();
		}

		public Action<object, EventArgs>? TextClick { get; set; }
		public Action<object, EventArgs>? PlayClick { get; set; }
		public Action<object, EventArgs>? LibraryItemsOptionsExpandedClick { get; set; }
		public Action<object, EventArgs>? OptionsClick { get; set; }

		private void Text_Click(object sender, EventArgs args)
		{
			TextClick?.Invoke(sender, args);
		}		
		private void Play_Click(object sender, EventArgs args)
		{
			PlayClick?.Invoke(sender, args);
		}						 
		private void LibraryItemsOptionsExpanded_Click(object sender, EventArgs args)
		{
			LibraryItemsOptions.Visibility = LibraryItemsOptions.Visibility != ViewStates.Gone
				? ViewStates.Gone
				: ViewStates.Visible;

			LibraryItemsOptionsExpandedClick?.Invoke(sender, args);
		}
		private void Options_Click(object sender, EventArgs args)
		{
			OptionsClick?.Invoke(sender, args);
		}
	}
}