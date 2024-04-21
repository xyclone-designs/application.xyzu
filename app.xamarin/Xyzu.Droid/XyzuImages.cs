#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.Views;
using AndroidX.Palette.Graphics;

using Bumptech.Glide;
using Bumptech.Glide.Load;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Load.Engine.Cache;
using Bumptech.Glide.Load.Resource.Bitmap;
using Bumptech.Glide.Module;
using Bumptech.Glide.Request;
using Bumptech.Glide.Request.Target;

using JP.Wasabeef.Glide.Transformations;

using Kdd.Glide.AppModuleInjector;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Images.Enums;
using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;

namespace Xyzu
{
	public sealed class XyzuImages : IImages.Default, IImagesDroid
	{
		private XyzuImages(Context context)
		{
			(_XyzuGlideModule = new XyzuGlideModule(Context = context, new GlideBuilder()))
				.Init();
		}

		private static XyzuImages? _Instance;
		public static XyzuImages Instance
		{
			get => _Instance ?? throw new Exception("Instane is null. Init AppImages before use");
		}

		public static void Init(Context context, Action<XyzuImages>? action = null)
		{
			_Instance = new XyzuImages(context);

			action?.Invoke(_Instance);
		}

		private readonly XyzuGlideModule _XyzuGlideModule;
		private readonly BlurTransformation _BlurTransformation = new BlurTransformation(100, 5);
		private readonly CropCircleWithBorderTransformation _CropCircleWithBorderTransformation = new CropCircleWithBorderTransformation(0, 0);
		private readonly RoundedCornersTransformation _RoundedCornersTransformation = new RoundedCornersTransformation(24, 0);

		public Context Context { get; set; }
		public IDictionary<string, byte[]>? Buffers { get; set; }

		private byte[]? Buffer(IModel model)
		{
			if (true switch
			{
				true when model is IAlbum album => album.Artwork,
				true when model is IArtist artist => artist.Image,
				true when model is ISong song => song.Artwork,

				_ => null,

			} is IImage image) return Buffer(image);

			return null;
		}
		private byte[]? Buffer(IImage image)
		{
			Buffers ??= new Dictionary<string, byte[]>();

			image.BufferHash ??= image.Buffer is null ? null : IImage.Utils.BufferToHash(image.Buffer);

			if ((image.BufferHash is null ? null : string.Join(".", image.BufferHash)) is string imagebufferkey)
			{
				image.Buffer ??= Buffers.TryGetValue(imagebufferkey);

				if (image.Buffer != null && Buffers.ContainsKey(imagebufferkey) is false)
					Buffers.Add(imagebufferkey, image.Buffer);
			}

			return image.Buffer;
		}
		private Bitmap? GenerateBitmap(params object?[] sources)
		{
			Bitmap? bitmap = null;

			IEnumerator sourceenumerator = sources.GetEnumerator();

			while (sourceenumerator.MoveNext() && bitmap is null)
				switch (true)
				{
					case true when
					sourceenumerator.Current is IModel model:
						byte[]? modelbuffer = Buffer(model);

						if (modelbuffer is null && true switch
						{
							true when model is IAlbum album => album.Artwork?.BufferHash != null,
							true when model is IArtist artist => artist.Image?.BufferHash != null,
							true when model is ISong song => song.Artwork?.BufferHash != null,

							_ => false,

						}) LibraryMisc?.SetImage(model);

						modelbuffer ??= Buffer(model);

						if (modelbuffer != null)
							bitmap ??= BitmapFactory.DecodeByteArray(modelbuffer, 0, modelbuffer.Length);
						break;

					case true when sourceenumerator.Current is IImage image:
						switch (true)
						{
							case true when image.Buffer != null:
								bitmap ??= BitmapFactory.DecodeByteArray(image.Buffer, 0, image.Buffer.Length);
								break;

							case true when image.Uri?.ToAndroidUri() is AndroidUri androiduri:
								bitmap ??=
									BitmapFactory.DecodeFile(androiduri.Path) ??
									BitmapFactory.DecodeFileDescriptor(Context.ContentResolver?.OpenFileDescriptorSafe(androiduri, "r")?.FileDescriptor);
								break;

							default: break;
						}
						break;

					case true when sourceenumerator.Current is Bitmap sourcebitmap:
						bitmap ??= sourcebitmap;
						break;

					case true when sourceenumerator.Current is byte[] sourcebuffer:
						bitmap ??= BitmapFactory.DecodeByteArray(sourcebuffer, 0, sourcebuffer.Length);
						break;

					case true when sourceenumerator.Current is Java.Lang.Integer sourceresourceid:
						bitmap ??= BitmapFactory.DecodeResource(Context.Resources, sourceresourceid.IntValue());
						break;

					case true when sourceenumerator.Current is Drawable sourcedrawable:
						break;

					case true when (sourceenumerator.Current as AndroidUri ?? (sourceenumerator.Current as Uri)?.ToAndroidUri()) is AndroidUri sourceandroiduri:
						bitmap ??=
							BitmapFactory.DecodeFile(sourceandroiduri.Path) ??
							BitmapFactory.DecodeFileDescriptor(Context.ContentResolver?.OpenFileDescriptorSafe(sourceandroiduri, "r")?.FileDescriptor);
						break;

					default: break;
				}

			return bitmap;
		}

		private async Task<RequestBuilder?> RequestBuilder(Func<RequestManager?, RequestBuilder?>? requestbuilderaction, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			RequestBuilder? requestbuilder = null;
			RequestManager? requestmanager = _XyzuGlideModule.Glide.RequestManagerRetriever.Get(Context);

			IEnumerator sourceenumerator = sources.GetEnumerator();

			while (sourceenumerator.MoveNext() && requestbuilder is null)
				switch (true)
				{
					case true when 
					sourceenumerator.Current is IModel model:
						byte[]? artworkbuffer = Buffer(model);

						if (artworkbuffer is null && true switch
						{
							true when model is IAlbum album => album.Artwork?.BufferHash != null,
							true when model is IArtist artist => artist.Image?.BufferHash != null,
							true when model is ISong song => song.Artwork?.BufferHash != null,

							_ => false,

						} && LibraryMisc != null) await LibraryMisc.SetImage(model, cancellationtoken); 

						artworkbuffer ??= Buffer(model);
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(artworkbuffer) ?? requestmanager.Load(artworkbuffer);
						break;

					case true when sourceenumerator.Current is IImage image:
						switch (true)
						{
							case true when image.Buffer != null:
								requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(image.Buffer) ?? requestmanager.Load(image.Buffer);
								break;

							case true when image.Uri?.ToAndroidUri() is AndroidUri androiduri:
								requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(androiduri) ?? requestmanager.Load(androiduri);
								break;

							default: break;
						}
						break;

					case true when sourceenumerator.Current is Bitmap bitmap:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(bitmap) ?? requestmanager.Load(bitmap);
						break;

					case true when sourceenumerator.Current is byte[] buffer:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(buffer) ?? requestmanager.Load(buffer);
						break;

					case true when ((sourceenumerator.Current as Java.Lang.Integer)?.IntValue() ?? sourceenumerator.Current) is int resourceid:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(resourceid) ?? requestmanager.Load(resourceid);
						break;

					case true when sourceenumerator.Current is Drawable drawable:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(drawable) ?? requestmanager.Load(drawable);
						break;

					case true when (sourceenumerator.Current as AndroidUri ?? (sourceenumerator.Current as Uri)?.ToAndroidUri()) is AndroidUri androiduri:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(androiduri) ?? requestmanager.Load(androiduri);
						break;

					default: break;
				}

			return requestbuilder;
		}			  
		private RequestBuilder? RequestBuilder(Func<RequestManager?, RequestBuilder?>? requestbuilderaction, params object?[] sources)
		{
			RequestBuilder? requestbuilder = null;
			RequestManager? requestmanager = _XyzuGlideModule.Glide.RequestManagerRetriever.Get(Context);

			IEnumerator sourceenumerator = sources.GetEnumerator();

			while (sourceenumerator.MoveNext() && requestbuilder is null)
				switch (true)
				{
					case true when 
					sourceenumerator.Current is IModel model:
						byte[]? artworkbuffer = Buffer(model);

						if (artworkbuffer is null && true switch
						{
							true when model is IAlbum album => album.Artwork?.BufferHash != null,
							true when model is IArtist artist => artist.Image?.BufferHash != null,
							true when model is ISong song => song.Artwork?.BufferHash != null,

							_ => false,

						}) LibraryMisc?.SetImage(model);

						artworkbuffer ??= Buffer(model);
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(artworkbuffer) ?? requestmanager.Load(artworkbuffer);
						break;

					case true when sourceenumerator.Current is IImage image:
						switch (true)
						{
							case true when image.Buffer != null:
								requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(image.Buffer) ?? requestmanager.Load(image.Buffer);
								break;

							case true when image.Uri?.ToAndroidUri() is AndroidUri androiduri:
								requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(androiduri) ?? requestmanager.Load(androiduri);
								break;

							default: break;
						}
						break;

					case true when sourceenumerator.Current is Bitmap bitmap:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(bitmap) ?? requestmanager.Load(bitmap);
						break;

					case true when sourceenumerator.Current is byte[] buffer:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(buffer) ?? requestmanager.Load(buffer);
						break;

					case true when ((sourceenumerator.Current as Java.Lang.Integer)?.IntValue() ?? sourceenumerator.Current) is int resourceid:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(resourceid) ?? requestmanager.Load(resourceid);
						break;

					case true when sourceenumerator.Current is Drawable drawable:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(drawable) ?? requestmanager.Load(drawable);
						break;

					case true when (sourceenumerator.Current as AndroidUri ?? (sourceenumerator.Current as Uri)?.ToAndroidUri()) is AndroidUri androiduri:
						requestbuilder ??= requestbuilderaction?.Invoke(requestmanager)?.Load(androiduri) ?? requestmanager.Load(androiduri);
						break;

					default: break;
				}

			return requestbuilder;
		}
		private RequestBuilder? RequestBuilderOperations(RequestBuilder? requestbuilder, params Operations[] operations)
		{
			if (requestbuilder is null)
				return requestbuilder;

			requestbuilder.SetDiskCacheStrategy(DiskCacheStrategy.All);

			foreach (Operations operation in operations)
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

			return requestbuilder;
		}

		public Bitmap? GetBitmap(Operations[] operations, BitmapFactory.Options? options, params object?[] sources)
		{
			Bitmap? bitmap = null;

			Task.Run(() =>
			{
				try
				{
					using RequestBuilder? requestbuilder = RequestBuilder(requestmanager => requestmanager?.AsBitmap(), sources)?
						.Override(options?.OutWidth ?? -1, options?.OutHeight ?? -1);
					using IFutureTarget? futuretarget = RequestBuilderOperations(requestbuilder, operations)?
						.Submit();

					bitmap = futuretarget?.Get() as Bitmap;
				}
				catch (Java.Util.Concurrent.ExecutionException) { }
				catch (Java.Lang.InterruptedException) { }
			});

			return bitmap ?? GenerateBitmap(sources);
		}
		public Drawable? GetDrawable(Operations[] operations, params object?[] sources)
		{
			BitmapDrawable? bitmapdrawable = null;

			Task.Run(() =>
			{
				using RequestBuilder? requestbuilder = RequestBuilder(requestmanager => requestmanager?.AsDrawable(), sources);
				using IFutureTarget? futuretarget = RequestBuilderOperations(requestbuilder, operations)?.Submit();

				try
				{
					bitmapdrawable = futuretarget?.Get() as BitmapDrawable;
				}
				catch (Java.Util.Concurrent.ExecutionException) { }
				catch (Java.Lang.InterruptedException) { }
			});

			return bitmapdrawable;
		}
		public Palette? GetPalette(params object?[] sources)
		{
			if (GetBitmap(Array.Empty<Operations>(), null, sources) is Bitmap bitmap)
				return Palette.From(bitmap).Generate();

			return null;

		}
		public async Task SetToImageView(Operations[] operations, ImageView? imageview, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			if (imageview is null)
			{
				oncomplete?.Invoke(false);

				return;
			}

			using RequestBuilder? requestbuilder = await RequestBuilder(
				sources: sources,
				cancellationtoken: cancellationtoken,
				requestbuilderaction: requestmanager =>
				{
					if (imageview.Width == 0 && imageview.Height == 0 && imageview.Parent is ViewGroup viewgroup)
					{
						int widthMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Width, MeasureSpecMode.AtMost);
						int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Height, MeasureSpecMode.AtMost);

						imageview.Measure(widthMeasureSpec, heightMeasureSpec);
					}

					requestmanager?.Clear(imageview);
					imageview.SetImageBitmap(null);
					imageview.SetImageDrawable(null);
					
					return null;
				});

			if (requestbuilder is null)
			{
				oncomplete?.Invoke(false);

				return;
			}

			int width = imageview.Width == 0 ? imageview.MeasuredWidth : -1;
			int height = imageview.Height == 0 ? imageview.MeasuredHeight : -1;	 

			requestbuilder.Override(width, height);

			RequestBuilderOperations(requestbuilder, operations);

			requestbuilder.Into(new ImageResourceTarget(imageview)
			{
				OnLoadFailedAction = drawable => oncomplete?.Invoke(false),
				OnResourceReadyAction = (resource, transition) => oncomplete?.Invoke(true),
			});
		}
		public async Task SetToViewBackground(Operations[] operations, View? view, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			if (view is null)
			{
				oncomplete?.Invoke(false);
				
				return;
			}

			using RequestBuilder? requestbuilder = await RequestBuilder(
				sources: sources,
				cancellationtoken: cancellationtoken,
				requestbuilderaction: requestmanager =>
				{
					view.Background = null;
					return null;
				});

			requestbuilder?.Override(view.Width, view.Height);

			RequestBuilderOperations(requestbuilder, operations);
		
			if (requestbuilder is null)
			{
				oncomplete?.Invoke(false);

				return;
			}

			requestbuilder.Into(new ViewBackgroundTarget(view)
			{
				OnLoadFailedAction = drawable => oncomplete?.Invoke(false),
				OnResourceReadyAction = (resource, transition) => oncomplete?.Invoke(true),
			});
		}

		public class XyzuGlideModule : AppGlideModule
		{
			internal XyzuGlideModule(Context context, GlideBuilder glidebuilder)
			{
				_Context = context;
				_GlideBuilder = glidebuilder;

				GlideAppModuleInjector.Inject(this);
			}

			private Context _Context;
			private GlideBuilder _GlideBuilder;
			private Glide? _Glide;
			
			public Glide Glide
			{
				get => _Glide ??= Glide.Get(_Context);
			}

			public IDiskCacheFactory? DiskCacheFactory { get; set; }
			public IMemoryCache? MemoryCache { get; set; }
			public RequestOptions? Options { get; set; }

			public void Init()
			{
				ApplyOptions(_Context, _GlideBuilder);

				Glide.Init(_Context, _GlideBuilder);
			}

			public override void ApplyOptions(Context context, GlideBuilder glidebuilder)
			{
				base.ApplyOptions(context, glidebuilder);

				DiskCacheFactory ??= new InternalCacheDiskCacheFactory(context);
				MemoryCache ??= new LruResourceCache(1024 * 1024 * 100); // 100MB
				Options ??= new RequestOptions()
					.Format(DecodeFormat.PreferRgb565)
					.FitCenter()
					.EncodeQuality(100)
					.SetDiskCacheStrategy(DiskCacheStrategy.Data)
					.SkipMemoryCache(false);

				glidebuilder.SetDefaultRequestOptions(Options);
				glidebuilder.SetDiskCache(DiskCacheFactory);
				glidebuilder.SetMemoryCache(MemoryCache);
			}
			public override void RegisterComponents(Context context, Glide glide, Registry registry)
			{
				base.RegisterComponents(context, glide, registry);

				glide.ClearMemory();
				Task.Run(() => glide.ClearDiskCache());
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (_Glide != null)
					{
						Task.Run(() =>
						{
							_Glide.ClearDiskCache();
							_Glide.ClearMemory();
						});

						_Glide.Dispose();
						_Glide = null;
					}
				}

				base.Dispose(disposing);
			}
		}
	}

	public static partial class ImagesExtensions
	{
		private static readonly Bitmap.Config BitmapConfig = Bitmap.Config.Argb8888!;

		public static Drawable? GetDrawable(this IImages images, Operations[] operations, params object?[] sources)
		{
			return (images as IImagesDroid)?.GetDrawable(operations, sources);
		}
		public static Bitmap? GetBitmap(this IImages images, Operations[] operations, BitmapFactory.Options? options, params object?[] sources)
		{
			return (images as IImagesDroid)?.GetBitmap(operations, options, sources);
		}
		public static Palette? GetPalette(this IImages images, params object?[] sources)
		{
			return (images as IImagesDroid)?.GetPalette(sources);
		}

		public static Task SetToImageView(this IImages images, Operations[] operations, ImageView? imageview, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return (images as IImagesDroid)?.SetToImageView(operations, imageview, oncomplete, cancellationtoken, sources) ?? Task.CompletedTask;
		}
		public static Task SetToViewBackground(this IImages images, Operations[] operations, View? view, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return (images as IImagesDroid)?.SetToViewBackground(operations, view, oncomplete, cancellationtoken, sources) ?? Task.CompletedTask;
		}

		public static Bitmap? MergeBitmaps(this IImages images, Bitmap? topleft, Bitmap? topright, Bitmap? bottomleft, Bitmap? bottomright)
		{
			int dimension = 0;

			if (topleft != null && Math.Min(topleft.Width, topleft.Height) is int topleftmin)
				dimension = dimension == 0
					? topleftmin
					: Math.Min(topleftmin, dimension);
			
			if (topright != null && Math.Min(topright.Width, topright.Height) is int toprightmin)
				dimension = dimension == 0
					? toprightmin
					: Math.Min(toprightmin, dimension);		  

			if (bottomleft != null && Math.Min(bottomleft.Width, bottomleft.Height) is int bottomleftmin)
				dimension = dimension == 0
					? bottomleftmin
					: Math.Min(bottomleftmin, dimension);
			
			if (bottomright != null && Math.Min(bottomright.Width, bottomright.Height) is int bottomrightmin)
				dimension = dimension == 0
					? bottomrightmin
					: Math.Min(bottomrightmin, dimension);

			Bitmap? bitmap = dimension == 0 || Bitmap.Config.Argb8888 is null
				? null 
				: Bitmap.CreateBitmap(dimension, dimension, Bitmap.Config.Argb8888);

			if (bitmap is null)
				return null;

			int scaleddimension = dimension / 2;

			int[]? pixels = null;
			int[]? colors = Enumerable.Range(0, scaleddimension * scaleddimension)
				.Select(_ => Color.Transparent.ToArgb())
				.ToArray();

			if (topleft != null && (topleft = Bitmap.CreateScaledBitmap(topleft, scaleddimension, scaleddimension, false)) != null)
				topleft.GetPixels(pixels = new int[dimension * dimension], 0, scaleddimension, 0, 0, scaleddimension, scaleddimension);
			bitmap.SetPixels(pixels ?? colors, 0, scaleddimension, 0, 0, scaleddimension, scaleddimension);
			pixels = null;

			if (topright != null && (topright = Bitmap.CreateScaledBitmap(topright, scaleddimension, scaleddimension, false)) != null)
				topright.GetPixels(pixels = new int[dimension * dimension], 0, scaleddimension, 0, 0, scaleddimension, scaleddimension);
			bitmap.SetPixels(pixels ?? colors, 0, scaleddimension, 0, scaleddimension, scaleddimension, scaleddimension);
			pixels = null;

			if (bottomleft != null && (bottomleft = Bitmap.CreateScaledBitmap(bottomleft, scaleddimension, scaleddimension, false)) != null)
				bottomleft.GetPixels(pixels = new int[dimension * dimension], 0, scaleddimension, 0, 0, scaleddimension, scaleddimension);
			bitmap.SetPixels(pixels ?? colors, 0, scaleddimension, scaleddimension, 0, scaleddimension, scaleddimension);
			pixels = null;

			if (bottomright != null && (bottomright = Bitmap.CreateScaledBitmap(bottomright, scaleddimension, scaleddimension, false)) != null)
				bottomright.GetPixels(pixels = new int[dimension * dimension], 0, scaleddimension, 0, 0, scaleddimension, scaleddimension);
			bitmap.SetPixels(pixels ?? colors, 0, scaleddimension, scaleddimension, scaleddimension, scaleddimension, scaleddimension);
			pixels = null;

			colors = null;

			return bitmap;

		}
	}
}