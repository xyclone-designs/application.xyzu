#nullable enable

using Android.Graphics.Drawables;
using Android.Views;

using Bumptech.Glide.Request.Transition;

using Java.Lang;

namespace Bumptech.Glide.Request.Target
{
	public class ViewBackgroundTarget : XyzuGlideViewTarget
	{
		public ViewBackgroundTarget(View view) : base(view) { }

		public override void OnResourceReady(Object resource, ITransition transition)
		{
			View.Background = resource as BitmapDrawable;

			base.OnResourceReady(resource, transition);
		}
	}
}