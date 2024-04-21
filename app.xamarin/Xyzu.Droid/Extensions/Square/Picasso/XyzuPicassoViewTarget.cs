#nullable enable

using Android.Graphics;
using Android.Graphics.Drawables;

using System;

using JavaException = Java.Lang.Exception;
using JavaObject = Java.Lang.Object;

namespace Square.Picasso
{
	public class XyzuPicassoViewTarget : JavaObject, ITarget
	{
		public Action<JavaException, Drawable>? OnBitmapFailedAction { get; set; }
		public Action<Bitmap, Picasso.LoadedFrom>? OnBitmapLoadedAction { get; set; }
		public Action<Drawable>? OnPrepareLoadAction { get; set; }

		public virtual void OnBitmapFailed(JavaException errorDrawable, Drawable p1)
		{
			OnBitmapFailedAction?.Invoke(errorDrawable, p1);
		}
		public virtual void OnBitmapLoaded(Bitmap bitmap, Picasso.LoadedFrom loadedFrom)
		{
			OnBitmapLoadedAction?.Invoke(bitmap, loadedFrom);
		}
		public virtual void OnPrepareLoad(Drawable placeHolderDrawable)
		{
			OnPrepareLoadAction?.Invoke(placeHolderDrawable);
		}
	}
}