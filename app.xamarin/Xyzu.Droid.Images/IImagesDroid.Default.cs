using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

			public Action<IModel>? SetBuffer { get; set; }

			protected byte[]? Buffer(IModel model)
			{
				if (Image(model) is not IImage image) 
					return null;

				Buffers ??= new Dictionary<string, byte[]>();

				string? bufferkey = image.BufferKey;
				byte[]? buffer = image.Buffer ?? (Buffers.TryGetValue(bufferkey ?? string.Empty, out byte[]? _buffer) ? _buffer : null);

				if (buffer is not null || bufferkey is null || SetBuffer is null)
					return buffer;

				SetBuffer.Invoke(model);

				if (image.BufferKey is null)
					return null;

				Buffers.AddOrReplace(image.BufferKey, image.Buffer);

				return image.Buffer;
			}
			protected IImage? Image(IModel model)
			{
				return true switch
				{
					true when model is IAlbum album => album.Artwork,
					true when model is IArtist artist => artist.Image,
					true when model is ISong song => song.Artwork,

					_ => null,
				};
			}

			public virtual async Task<Bitmap?> GetBitmap(Operations[] operations, BitmapFactory.Options? options, CancellationToken cancellationtoken = default, params object?[] sources)
			{
				Bitmap? bitmap = null;

				IEnumerator sourceenumerator = sources.GetEnumerator();

				while (sourceenumerator.MoveNext() && bitmap is null)
					switch (true)
					{
						case true when
						sourceenumerator.Current is IModel model &&
						Buffer(model) is byte[] modelbuffer:
							bitmap ??= await BitmapFactory.DecodeByteArrayAsync(modelbuffer, 0, modelbuffer.Length, options);
							break;

						case true when sourceenumerator.Current is IImage image:
							switch (true)
							{
								case true when image.Buffer != null:
									bitmap ??= await BitmapFactory.DecodeByteArrayAsync(image.Buffer, 0, image.Buffer.Length, options);
									break;

								case true when image.Uri?.ToAndroidUri() is AndroidUri androiduri:
									bitmap ??=
										await BitmapFactory.DecodeFileAsync(androiduri.Path, options) ??
										await BitmapFactory.DecodeFileDescriptorAsync(
											opts: options,
											outPadding: null,
											fd: Context.ContentResolver?.OpenFileDescriptorSafe(androiduri, "r")?.FileDescriptor);
									break;

								default: break;
							}
							break;

						case true when sourceenumerator.Current is Bitmap sourcebitmap:
							bitmap ??= sourcebitmap;
							break;

						case true when sourceenumerator.Current is byte[] sourcebuffer:
							bitmap ??= await BitmapFactory.DecodeByteArrayAsync(sourcebuffer, 0, sourcebuffer.Length, options);
							break;

						case true when sourceenumerator.Current is Java.Lang.Integer sourceresourceid:
							bitmap ??= await BitmapFactory.DecodeResourceAsync(Context.Resources, sourceresourceid.IntValue(), options);
							break;

						case true when sourceenumerator.Current is Drawable:
							break;

						case true when (sourceenumerator.Current as AndroidUri ?? (sourceenumerator.Current as Uri)?.ToAndroidUri()) is AndroidUri sourceandroiduri:
							bitmap ??=
								await BitmapFactory.DecodeFileAsync(sourceandroiduri.Path, options) ??
								await BitmapFactory.DecodeFileDescriptorAsync(
									opts: options,
									outPadding: null,
									fd: Context.ContentResolver?.OpenFileDescriptorSafe(sourceandroiduri, "r")?.FileDescriptor);
							break;

						default: break;
					}

				return bitmap;
			}
			public virtual Task<Drawable?> GetDrawable(Operations[] operations, CancellationToken cancellationtoken = default, params object?[] sources)
			{
				return Task.FromResult<Drawable?>(null);
			}
			public virtual async Task<Palette?> GetPalette(CancellationToken cancellationtoken = default, params object?[] sources)
			{
				if (await GetBitmap(Array.Empty<Operations>(), null, cancellationtoken, sources) is Bitmap bitmap)
					return Palette.From(bitmap).Generate();

				return null;
			}

			public virtual async Task SetToImageView(Operations[] operations, ImageView? imageview, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
			{
				if (imageview is null)
					return;

				await Task.Run(() => imageview.SetImageBitmap(null));

				if (imageview.Width == 0 && imageview.Height == 0 && imageview.Parent is ViewGroup viewgroup)
				{
					int widthMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Width, MeasureSpecMode.AtMost);
					int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(viewgroup.Height, MeasureSpecMode.AtMost);

					imageview.Measure(widthMeasureSpec, heightMeasureSpec);
				}

				int width = imageview.Width == 0 ? imageview.MeasuredWidth : -1;
				int height = imageview.Height == 0 ? imageview.MeasuredHeight : -1;

				BitmapFactory.Options options = new()
				{
					OutWidth = width,
					OutHeight = height,
				};

				Bitmap? bitmap = await GetBitmap(operations, options, cancellationtoken, sources);

				await Task.Run(() => imageview.SetImageBitmap(bitmap));
			}
			public virtual async Task SetToViewBackground(Operations[] operations, View? view, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
			{
				if (view is null)
					return;

				Drawable? drawable = await GetDrawable(operations, cancellationtoken, sources);

				view.Background = drawable;
			}
		}
	}
}