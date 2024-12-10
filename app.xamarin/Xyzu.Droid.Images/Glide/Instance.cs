using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.Palette.Graphics;

using Bumptech.Glide;
using Bumptech.Glide.Load.Resource.Bitmap;
using Bumptech.Glide.Request;
using Bumptech.Glide.Request.Target;

using JP.Wasabeef.Glide.Transformations;

using System;
using System.Collections;
using System.Threading.Tasks;

using Xyzu.Images.Enums;
using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;

namespace Xyzu.Images.Glide
{
	public class Instance : IImagesDroid.Default
	{
		public Instance(Context context) : base(context)
		{
			_GlideModule = new GlideModule(context, new GlideBuilder());
		}

		private readonly GlideModule _GlideModule;
		private readonly BlurTransformation _BlurTransformation = new(100, 5);
		private readonly CropCircleWithBorderTransformation _CropCircleWithBorderTransformation = new(0, 0);
		private readonly RoundedCornersTransformation _RoundedCornersTransformation = new(24, 0);

		private void Operate(RequestBuilder requestbuilder, Operations operation)
		{
			switch (operation)
			{
				case Operations.Blur:
					requestbuilder.Transform(_BlurTransformation);
					break;

				case Operations.Circularise:
					requestbuilder.Transform(_CropCircleWithBorderTransformation);
					break;

				case Operations.Downsample:
					requestbuilder.Downsample(DownsampleStrategy.FitCenter);
					break;

				case Operations.Rounded:
					requestbuilder.Transform(_RoundedCornersTransformation);
					break;

				default: break;
			}
		}
		private async Task<RequestBuilder?> RequestBuilder(Func<RequestManager, RequestBuilder> requestbuilderaction, params object?[] sources)
		{
			RequestManager requestmanager = _GlideModule.Glide.RequestManagerRetriever.Get(Context);
			RequestBuilder? requestbuilder = null;

			IEnumerator sourceenumerator = sources.GetEnumerator();

			while (sourceenumerator.MoveNext() && requestbuilder is null)
				requestbuilder = true switch
				{
					true when sourceenumerator.Current is IModel model =>
						true switch
						{
							true when await Buffer(model) is byte[] artworkbuffer =>
								requestbuilderaction.Invoke(requestmanager).Load(artworkbuffer),

							true when (
								(model as IAlbum)?.Artwork ??
								(model as IArtist)?.Image ??
								(model as ISong)?.Artwork) is IImage image && image.Uri?.ToAndroidUri() is AndroidUri imageuri =>
								requestbuilderaction.Invoke(requestmanager).Load(imageuri),

							_ => null
						},
				
					true when sourceenumerator.Current is AndroidUri androiduri => 
						requestbuilderaction.Invoke(requestmanager).Load(androiduri),

					true when sourceenumerator.Current is Uri uri && uri.ToAndroidUri() is AndroidUri toandroiduri =>
						requestbuilderaction.Invoke(requestmanager).Load(toandroiduri),

					true when sourceenumerator.Current is Bitmap bitmap => 
						requestbuilderaction.Invoke(requestmanager).Load(bitmap),

					true when sourceenumerator.Current is Drawable drawable => 
						requestbuilderaction.Invoke(requestmanager).Load(drawable),

					_ => null
				};

			return requestbuilder;
		}

		public async override Task Operate(IImagesDroid.Parameters parameters)
		{
			await Task.CompletedTask;

			if ((parameters.OnBitmap is not null || (parameters.OnPalette is not null && parameters.OnDrawable is null)) && await RequestBuilder(
				sources: parameters.Sources,
				requestbuilderaction: requestmanager =>
				{
					return requestmanager
						.AsBitmap()
						.RunOperations(parameters.Operations, Operate);

				}) is RequestBuilder requestbuilderbitmap) requestbuilderbitmap.Into(new GlideTarget()
				{
					OnLoadFailedAction = drawable => parameters.OnComplete?.Invoke(false),
					OnResourceReadyAction = (resource, transition) =>
					{
						if (resource is Bitmap bitmap)
						{
							parameters.OnBitmap?.Invoke(bitmap);
							parameters.OnPalette?.Invoke(new Palette.Builder(bitmap).Generate());
						}

						parameters.OnComplete?.Invoke(true);
					},
				});
			
			if (parameters.OnDrawable is not null && await RequestBuilder(
				sources: parameters.Sources,
				requestbuilderaction: requestmanager =>
				{
					return requestmanager
						.AsDrawable()
						.RunOperations(parameters.Operations, Operate);

				}) is RequestBuilder requestbuilderdrawable) requestbuilderdrawable.Into(new GlideTarget()
				{
					OnLoadFailedAction = drawable => parameters.OnComplete?.Invoke(false),
					OnResourceReadyAction = (resource, transition) =>
					{
						if (resource is BitmapDrawable bitmapdrawable)
						{
							parameters.OnDrawable?.Invoke(bitmapdrawable);
							if (bitmapdrawable.Bitmap is not null)
								parameters.OnPalette?.Invoke(new Palette.Builder(bitmapdrawable.Bitmap).Generate());
						}

						parameters.OnComplete?.Invoke(true);
					},
				});
		}
		public async override Task SetToImageView(IImagesDroid.Parameters parameters)
		{
			await Task.CompletedTask;

			if (parameters.ImageView is null || await RequestBuilder(
				sources: parameters.Sources,
				requestbuilderaction: requestmanager =>
				{
					if (parameters.ImageView.Width == 0 && parameters.ImageView.Height == 0 && parameters.ImageView.Parent is ViewGroup viewgroup)
					{
						int widthMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Width, MeasureSpecMode.AtMost);
						int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Height, MeasureSpecMode.AtMost);

						parameters.ImageView.Measure(widthMeasureSpec, heightMeasureSpec);
					}

					parameters.ImageView.SetImageBitmap(null);
					parameters.ImageView.SetImageDrawable(null);
					requestmanager.Clear(parameters.ImageView);

					return requestmanager.AsBitmap();

				}) is not RequestBuilder requestbuilder) parameters.OnComplete?.Invoke(false);
			else
			{
				int width = parameters.ImageView.Width == 0 ? parameters.ImageView.MeasuredWidth : -1;
				int height = parameters.ImageView.Height == 0 ? parameters.ImageView.MeasuredHeight : -1;

				requestbuilder
					.Override(width, height)
					.RunOperations(parameters.Operations, Operate)
					.Into(new ImageBitmapTarget(parameters.ImageView)
					{
						OnLoadFailedAction = drawable => parameters.OnComplete?.Invoke(false),
						OnResourceReadyAction = (resource, transition) =>
						{
							if (resource is Bitmap bitmap)
							{
								parameters.OnBitmap?.Invoke(bitmap);
								parameters.OnPalette?.Invoke(new Palette.Builder(bitmap).Generate());
							}

							parameters.OnComplete?.Invoke(true);
						},
					});
			}
		}
		public async override Task SetToViewBackground(IImagesDroid.Parameters parameters)
		{
			await Task.CompletedTask;

			if (parameters.View is null || await RequestBuilder(
				sources: parameters.Sources,
				requestbuilderaction: requestmanager =>
				{
					parameters.View.Background = null;

					return requestmanager.AsDrawable();

				}) is not RequestBuilder requestbuilder) parameters.OnComplete?.Invoke(false);
			else
			{
				int width = parameters.View.Width == 0 ? parameters.View.MeasuredWidth : -1;
				int height = parameters.View.Height == 0 ? parameters.View.MeasuredHeight : -1;

				requestbuilder
					.RunOperations(parameters.Operations, Operate)
					.Into(new ViewBackgroundTarget(parameters.View)
					{
						OnLoadFailedAction = drawable => parameters.OnComplete?.Invoke(false),
						OnResourceReadyAction = (resource, transition) => parameters.OnComplete?.Invoke(true),
					});
			}
		}
		public async override Task<Bitmap?> GetBitmap(IImagesDroid.Parameters parameters)
		{
			Bitmap? bitmap = null;

			using RequestBuilder? requestbuilder = await RequestBuilder(requestmanager => requestmanager.AsBitmap(), parameters.Sources);
			using IFutureTarget? futuretarget = requestbuilder?.RunOperations(parameters.Operations, Operate).Submit();

			try
			{
				bitmap = futuretarget?.Get() as Bitmap;
			}
			catch (Java.Util.Concurrent.ExecutionException) { }
			catch (Java.Lang.InterruptedException) { }

			return bitmap ?? await base.GetBitmap(parameters);
		}
		public async override Task<Drawable?> GetDrawable(IImagesDroid.Parameters parameters)
		{
			BitmapDrawable? bitmapdrawable = null;

			using RequestBuilder? requestbuilder = await RequestBuilder(requestmanager => requestmanager.AsDrawable(), parameters.Sources);
			using IFutureTarget? futuretarget = requestbuilder?.RunOperations(parameters.Operations, Operate).Submit();

			try
			{
				bitmapdrawable = futuretarget?.Get() as BitmapDrawable;
			}
			catch (Java.Util.Concurrent.ExecutionException) { }
			catch (Java.Lang.InterruptedException) { }

			return bitmapdrawable ?? await base.GetDrawable(parameters);
		}
	}
}