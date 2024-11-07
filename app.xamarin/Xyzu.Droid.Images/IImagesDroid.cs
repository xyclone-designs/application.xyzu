using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xyzu.Images
{
	public partial interface IImagesDroid : IImages
	{
		public new class Parameters : IImages.Parameters
		{
			public Parameters() : this(Array.Empty<object?>()) { }
			public Parameters(params object?[] sources) : base()
			{
				Sources = sources;
			}

			public object?[] Sources { get; set; }
			public View? View { get; set; }
			public ImageView? ImageView { get; set; }

			public Action<bool>? OnComplete { get; set; }
			public Action<Bitmap?>? OnBitmap { get; set; }
			public Action<Drawable?>? OnDrawable { get; set; }
			public Action<Palette?>? OnPalette { get; set; }

			public CancellationToken CancellationToken { get; set; }
			public BitmapFactory.Options? BitmapOptions { get; set; }
		}

		Task Operate(Parameters parameters);
		Task SetToImageView(Parameters parameters);
		Task SetToViewBackground(Parameters parameters);
		Task<Bitmap?> GetBitmap(Parameters parameters);
		Task<Drawable?> GetDrawable(Parameters parameters);
	}

	public static class IImagesDroidExtensions
	{
		public static Task Operate(this IImages images, IImagesDroid.Parameters parameters)
		{
			return (images as IImagesDroid)?.Operate(parameters) ?? Task.CompletedTask;
		}
		public static Task SetToImageView(this IImages images, IImagesDroid.Parameters parameters)
		{
			return (images as IImagesDroid)?.SetToImageView(parameters) ?? Task.CompletedTask;
		}
		public static Task SetToViewBackground(this IImages images, IImagesDroid.Parameters parameters)
		{
			return (images as IImagesDroid)?.SetToViewBackground(parameters) ?? Task.CompletedTask;
		}
		public static Task<Bitmap?> GetBitmap(this IImages images, IImagesDroid.Parameters parameters)
		{
			return (images as IImagesDroid)?.GetBitmap(parameters) ?? Task.FromResult<Bitmap?>(null);
		}
		public static Task<Drawable?> GetDrawable(this IImages images, IImagesDroid.Parameters parameters)
		{
			return (images as IImagesDroid)?.GetDrawable(parameters) ?? Task.FromResult<Drawable?>(null);
		}
	}
}