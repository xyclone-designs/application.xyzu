using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images.Enums;
using Xyzu.Library.Models;

namespace Xyzu.Images
{
	public partial interface IImagesDroid : IImages
	{
		Task<Bitmap?> GetBitmap(Operations[] operations, BitmapFactory.Options? options, CancellationToken cancellationtoken = default, params object?[] sources);
		Task<Drawable?> GetDrawable(Operations[] operations, CancellationToken cancellationtoken = default, params object?[] sources);				  		 
		Task<Palette?> GetPalette(CancellationToken cancellationtoken = default, params object?[] sources);

		Task SetToImageView(Operations[] operations, ImageView? imageview, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources);
		Task SetToViewBackground(Operations[] operations, View? view, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources);
	}

	public static class IImagesDroidExtensions
	{
		public static Task<Drawable?> GetDrawable(this IImages images, Operations[] operations, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return (images as IImagesDroid)?.GetDrawable(operations, cancellationtoken, sources) ?? Task.FromResult<Drawable?>(null);
		}
		public static Task<Bitmap?> GetBitmap(this IImages images, Operations[] operations, BitmapFactory.Options? options, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return (images as IImagesDroid)?.GetBitmap(operations, options, cancellationtoken, sources) ?? Task.FromResult<Bitmap?>(null);
		}
		public static Task<Palette?> GetPalette(this IImages images, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return (images as IImagesDroid)?.GetPalette(cancellationtoken, sources) ?? Task.FromResult<Palette?>(null);
		}

		public static Task SetToImageView(this IImages images, Operations[] operations, ImageView? imageview, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return (images as IImagesDroid)?.SetToImageView(operations, imageview, oncomplete, cancellationtoken, sources) ?? Task.CompletedTask;
		}
		public static Task SetToViewBackground(this IImages images, Operations[] operations, View? view, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return (images as IImagesDroid)?.SetToViewBackground(operations, view, oncomplete, cancellationtoken, sources) ?? Task.CompletedTask;
		}

		public static Bitmap? MergeBitmaps(this IImages _, Bitmap? topleft, Bitmap? topright, Bitmap? bottomleft, Bitmap? bottomright, BitmapFactory.Options? options)
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