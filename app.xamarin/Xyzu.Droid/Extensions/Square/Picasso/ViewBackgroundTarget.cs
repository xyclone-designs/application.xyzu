#nullable enable

using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

using Java.Lang;

namespace Square.Picasso
{
	public class ViewBackgroundTarget : XyzuPicassoViewTarget 
	{
		public ViewBackgroundTarget(View view)
		{
			View = view;
		}

		public View View { get; set; }

		public override void OnBitmapFailed(Exception errorDrawable, Drawable p1)
		{
			View.Background = null;

			base.OnBitmapFailed(errorDrawable, p1);
		}
		public override void OnBitmapLoaded(Bitmap bitmap, Picasso.LoadedFrom loadedFrom)
		{
			View.Background = new BitmapDrawable(View.Context?.Resources, bitmap);

			base.OnBitmapLoaded(bitmap, loadedFrom);
		}
		public override void OnPrepareLoad(Drawable placeHolderDrawable)
		{
			View.Background = placeHolderDrawable;

			base.OnPrepareLoad(placeHolderDrawable);
		}
	}
}