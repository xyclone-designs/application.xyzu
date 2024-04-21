#nullable enable

using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.Palette.Graphics;

using System;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images.Enums;

namespace Xyzu.Images
{
	public interface IImagesDroid : IImages
	{
		Bitmap? GetBitmap(Operations[] operations, BitmapFactory.Options? options, params object?[] sources);
		Drawable? GetDrawable(Operations[] operations, params object?[] sources);				  		 
		Palette? GetPalette(params object?[] sources);

		Task SetToImageView(Operations[] operations, ImageView? imageview, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources);
		Task SetToViewBackground(Operations[] operations, View? view, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources);
	}
}