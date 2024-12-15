using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Images.Enums;
using Xyzu.Library.Models;

using AndroidUri = Android.Net.Uri;

namespace Xyzu.Images
{
	public partial interface IImagesDroid : IImages
	{
		public new class Default : IImages.Default, IImagesDroid
		{
			public Default(Context context)
			{
				Context = context;
			}

			public Context Context { get; set; }
			public IDictionary<string, byte[]>? Buffers { get; set; }

			public Func<IModel, Task>? SetBuffer { get; set; }

			protected async Task<byte[]?> Buffer(IModel model)
			{
				if ((
					(model as IAlbum)?.Artwork ??
					(model as IArtist)?.Image ??
					(model as ISong)?.Artwork) is not IImage image) return null;

				Buffers ??= new Dictionary<string, byte[]>();

				string? bufferkey = image.BufferKey;
				byte[]? buffer = image.Buffer ?? (Buffers.TryGetValue(bufferkey ?? string.Empty, out byte[]? _buffer) ? _buffer : null);

				if (buffer is not null || bufferkey is null || SetBuffer is null)
					return buffer;

				await SetBuffer.Invoke(model);

				if (image.BufferKey is null)
					return null;

				Buffers.AddOrReplace(image.BufferKey, image.Buffer);

				return image.Buffer;
			}

			public async virtual Task Operate(Parameters parameters)
			{
				await Task.CompletedTask;
			}
			public async virtual Task SetToImageView(Parameters parameters)
			{
				if (parameters.ImageView is null)
					return;

				parameters.ImageView.SetImageBitmap(null);

				if (parameters.ImageView.Width == 0 && parameters.ImageView.Height == 0 && parameters.ImageView.Parent is ViewGroup viewgroup)
				{
					int widthMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Width, MeasureSpecMode.AtMost);
					int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Height, MeasureSpecMode.AtMost);

					parameters.ImageView.Measure(widthMeasureSpec, heightMeasureSpec);
				}

				int width = parameters.ImageView.Width == 0 ? parameters.ImageView.MeasuredWidth : -1;
				int height = parameters.ImageView.Height == 0 ? parameters.ImageView.MeasuredHeight : -1;

				BitmapFactory.Options options = new()
				{
					OutWidth = width,
					OutHeight = height,
				};

				if (parameters.Operations.Contains(Operations.Merge) is false && await GetBitmap(parameters) is Bitmap a)
					parameters.ImageView.SetImageBitmap(a);
				else if (Bitmap.Config.Rgb565 is null) 
					return;
				else
				{
					List<Bitmap> bitmaps = new();

					for (int index = 0; bitmaps.Count != 4 && index < parameters.Sources.Length; index++)
						if ((parameters.Sources[index] as IEnumerable)?.GetEnumerator() is IEnumerator enumerator)
						{
							while (bitmaps.Count != 4 && enumerator.MoveNext())
								if (await GetBitmap(enumerator.Current, parameters.BitmapOptions) is Bitmap c)
									bitmaps.Add(c);
						}
						else if (parameters.Sources[index] is IAsyncEnumerable<object> asyncenumerable && await asyncenumerable
							.SelectAwait(async _ => await GetBitmap(_, parameters.BitmapOptions))
							.OfType<Bitmap>()
							.Take(4 - bitmaps.Count)
							.ToListAsync()
							.AsTask() is List<Bitmap> _bitmapsasync) bitmaps.AddRange(_bitmapsasync);

						else if (parameters.Sources[index] is object obj && await GetBitmap(obj, parameters.BitmapOptions) is Bitmap c)
							bitmaps.Add(c);

					int dimension = 0;

					Bitmap? topleft = bitmaps.ElementAtOrDefault(0);
					Bitmap? topright = bitmaps.ElementAtOrDefault(1);
					Bitmap? bottomleft = bitmaps.ElementAtOrDefault(2);
					Bitmap? bottomright = bitmaps.ElementAtOrDefault(3);

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

					if (dimension is 0)
						return;

					Bitmap bitmap = Bitmap.CreateBitmap(dimension, dimension, Bitmap.Config.Rgb565);

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

					parameters.ImageView.SetImageBitmap(bitmap);
				}
			}
			public async virtual Task SetToViewBackground(Parameters parameters)
			{
				if (parameters.View is null)
					return;

				parameters.View.Background = await GetDrawable(parameters);
			}
			public async virtual Task<Bitmap?> GetBitmap(Parameters parameters)
			{
				await Task.CompletedTask;

				Bitmap? bitmap = null;

				IEnumerator sourceenumerator = parameters.Sources.GetEnumerator();

				while (sourceenumerator.MoveNext() && bitmap is null)
					bitmap = await GetBitmap(sourceenumerator.Current, parameters.BitmapOptions);

				return bitmap;
			}
			public async virtual Task<Drawable?> GetDrawable(Parameters parameters)
			{
				return await Task.FromResult<Drawable?>(null);
			}

			public async virtual Task<Bitmap?> GetBitmap(object source, BitmapFactory.Options? options)
			{
				await Task.CompletedTask;

				Bitmap? bitmap = null;

				switch (true)
				{
					case true when
						source is IModel model &&
						await Buffer(model) is byte[] modelbuffer:
						bitmap ??= await BitmapFactory.DecodeByteArrayAsync(modelbuffer, 0, modelbuffer.Length, options);
						break;

					case true when source is IImage image:
						switch (true)
						{
							case true when image.Buffer != null:
								bitmap ??= await BitmapFactory.DecodeByteArrayAsync(image.Buffer, 0, image.Buffer.Length, options);
								break;

							case true when image.Uri?.ToAndroidUri() is AndroidUri androiduri:
								bitmap ??=
									await BitmapFactory.DecodeFileAsync(androiduri.Path, options) ??
									await BitmapFactory.DecodeFileDescriptorAsync(
										outPadding: null,
										opts: options,
										fd: Context.ContentResolver?.OpenFileDescriptorSafe(androiduri, "r")?.FileDescriptor);
								break;

							default: break;
						}
						break;

					case true when source is Bitmap sourcebitmap:
						bitmap ??= sourcebitmap;
						break;

					case true when source is byte[] sourcebuffer:
						bitmap ??= await BitmapFactory.DecodeByteArrayAsync(sourcebuffer, 0, sourcebuffer.Length, options);
						break;

					case true when source is int sourceresourceid:
						bitmap ??= await BitmapFactory.DecodeResourceAsync(Context.Resources, sourceresourceid, options);
						break;
					case true when source is Java.Lang.Integer sourceresourceidjava:
						bitmap ??= await BitmapFactory.DecodeResourceAsync(Context.Resources, sourceresourceidjava.IntValue(), options);
						break;

					case true when source is Drawable:
						break;

					case true when (source as AndroidUri ?? (source as Uri)?.ToAndroidUri()) is AndroidUri sourceandroiduri:
						bitmap ??=
							await BitmapFactory.DecodeFileAsync(sourceandroiduri.Path, options) ??
							await BitmapFactory.DecodeFileDescriptorAsync(
								outPadding: null,
								opts: options,
								fd: Context.ContentResolver?.OpenFileDescriptorSafe(sourceandroiduri, "r")?.FileDescriptor);
						break;

					default: break;
				}

				return bitmap;
			}
		}
	}
}