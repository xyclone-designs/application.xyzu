#nullable enable

using Android.Graphics.Drawables;
using Android.Widget;

using Bumptech.Glide.Request.Transition;

using Java.Lang;

namespace Bumptech.Glide.Request.Target
{
	public class ImageResourceTarget : XyzuGlideViewTarget
	{
		public ImageResourceTarget(ImageView imageview) : base(imageview) { }

		public new ImageView View { get => (ImageView)base.View; }

		public override void OnResourceReady(Object resource, ITransition transition)
		{
			View.SetImageDrawable(resource as BitmapDrawable);

			base.OnResourceReady(resource, transition);
		}
	}
}