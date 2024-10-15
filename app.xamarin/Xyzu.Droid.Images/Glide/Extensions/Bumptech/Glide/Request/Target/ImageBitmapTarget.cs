using Android.Graphics;
using Android.Widget;

using Bumptech.Glide.Request.Transition;

using Java.Lang;

namespace Bumptech.Glide.Request.Target
{
	public class ImageBitmapTarget : GlideViewTarget
	{
		public ImageBitmapTarget(ImageView imageview) : base(imageview) { }

		public new ImageView View { get => (ImageView)base.View; }

		public override void OnResourceReady(Object resource, ITransition transition)
		{
			View.SetImageBitmap(resource as Bitmap);

			base.OnResourceReady(resource, transition);
		}
	}
}