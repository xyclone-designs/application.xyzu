using Android.Content;
using Android.Util;
using AndroidX.AppCompat.Widget;

using System;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library.Models;

namespace Xyzu.Views.LibraryItem.Header
{
	public class HeaderView : LibraryItemView
	{
		public HeaderView(Context context) : this(context, null!) { }
		public HeaderView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_LibraryItem_Header) { }
		public HeaderView(Context context, IAttributeSet attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, Resource.Style.Xyzu_View_LibraryItem_Header) { }
		public HeaderView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }
	
		public AppCompatImageButton? Back { get; set; }

		public Action<object?, EventArgs>? OnBackClick { get; set; }

		protected void BackOnClick(object? sender, EventArgs args)
		{
			OnBackClick?.Invoke(sender, args);
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (Back != null) Back.Click += BackOnClick;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			if (Back != null) Back.Click -= BackOnClick;
		}

		public override async void SetArtwork(IImage? image)
		{
			if (Images != null)
				await Images.SetToViewBackground(new IImagesDroid.Parameters(image)
				{
					View = this,
					Operations = IImages.DefaultOperations.BlurDownsample,
				});

			base.SetArtwork(image);
		}
		public override async void SetArtwork(IModel? model)
		{
			if (Images != null)
				await Images.SetToViewBackground(new IImagesDroid.Parameters(model)
				{
					View = this,
					Operations = IImages.DefaultOperations.BlurDownsample,
				});

			base.SetArtwork(model);
		}
	}
}