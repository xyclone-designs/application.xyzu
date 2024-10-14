using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.Views;

using Bumptech.Glide;
using Bumptech.Glide.Load.Resource.Bitmap;
using Bumptech.Glide.Request;
using Bumptech.Glide.Request.Target;

using JP.Wasabeef.Glide.Transformations;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
		private RequestBuilder? RequestBuilder(Func<RequestManager, RequestBuilder> requestbuilderaction, params object?[] sources)
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
							true when Buffer(model) is byte[] artworkbuffer =>
								requestbuilderaction.Invoke(requestmanager).Load(artworkbuffer),

							true when Image(model) is IImage image && image.Uri?.ToAndroidUri() is AndroidUri imageuri =>
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

		public async override Task<Bitmap?> GetBitmap(Operations[] operations, BitmapFactory.Options? options, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			Bitmap? bitmap = null;

			using RequestBuilder? requestbuilder = RequestBuilder(requestmanager => requestmanager.AsBitmap(), sources);
			using IFutureTarget? futuretarget = requestbuilder?.RunOperations(operations, Operate).Submit();

			try
			{
				bitmap = futuretarget?.Get() as Bitmap;
			}
			catch (Java.Util.Concurrent.ExecutionException) { }
			catch (Java.Lang.InterruptedException) { }

			return bitmap ?? await base.GetBitmap(operations, options, cancellationtoken, sources);
		}
		public async override Task<Drawable?> GetDrawable(Operations[] operations, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			BitmapDrawable? bitmapdrawable = null;

			using RequestBuilder? requestbuilder = RequestBuilder(requestmanager => requestmanager.AsDrawable(), sources);
			using IFutureTarget? futuretarget = requestbuilder?.RunOperations(operations, Operate).Submit();

			try
			{
				bitmapdrawable = futuretarget?.Get() as BitmapDrawable;
			}
			catch (Java.Util.Concurrent.ExecutionException) { }
			catch (Java.Lang.InterruptedException) { }

			return bitmapdrawable ?? await base.GetDrawable(operations, cancellationtoken, sources);
		}
		public async override Task SetToImageView(Operations[] operations, ImageView? imageview, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			await Task.CompletedTask;

			if (imageview is null || RequestBuilder(
				sources: sources,
				requestbuilderaction: requestmanager =>
				{
					if (imageview.Width == 0 && imageview.Height == 0 && imageview.Parent is ViewGroup viewgroup)
					{
						int widthMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Width, MeasureSpecMode.AtMost);
						int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Height, MeasureSpecMode.AtMost);

						imageview.Measure(widthMeasureSpec, heightMeasureSpec);
					}

					imageview.SetImageBitmap(null);
					imageview.SetImageDrawable(null);
					requestmanager.Clear(imageview);

					return requestmanager.AsBitmap();

				}) is not RequestBuilder requestbuilder) oncomplete?.Invoke(false);
			else
			{
				int width = imageview.Width == 0 ? imageview.MeasuredWidth : -1;
				int height = imageview.Height == 0 ? imageview.MeasuredHeight : -1;

				requestbuilder
					.Override(width, height)
					.RunOperations(operations, Operate)
					.Into(new ImageResourceTarget(imageview)
					{
						OnLoadFailedAction = drawable => oncomplete?.Invoke(false),
						OnResourceReadyAction = (resource, transition) => oncomplete?.Invoke(true),
					});
			}
		}
		public async override Task SetToViewBackground(Operations[] operations, View? view, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			await Task.CompletedTask;

			if (view is null || RequestBuilder(
				sources: sources,
				requestbuilderaction: requestmanager =>
				{
					view.Background = null;

					return requestmanager.AsDrawable();

				}) is not RequestBuilder requestbuilder) oncomplete?.Invoke(false);

			else requestbuilder
					.RunOperations(operations, Operate)
					.Into(new ViewBackgroundTarget(view)
					{
						OnLoadFailedAction = drawable => oncomplete?.Invoke(false),
						OnResourceReadyAction = (resource, transition) => oncomplete?.Invoke(true),
					});
		}
	}
}