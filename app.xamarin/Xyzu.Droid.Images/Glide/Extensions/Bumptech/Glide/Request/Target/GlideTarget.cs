using Android.Graphics.Drawables;

using Bumptech.Glide.Request.Transition;

using System;

using JavaObject = Java.Lang.Object;

namespace Bumptech.Glide.Request.Target
{
	public class GlideTarget : CustomTarget
	{
		public GlideTarget() : base() {  }

		public Action<Drawable?>? OnLoadClearedAction { get; set; }          
		public Action<Drawable?>? OnLoadFailedAction { get; set; }          
		public Action<JavaObject?, ITransition?>? OnResourceReadyAction { get; set; }

		public override void OnLoadCleared(Drawable? p0)
		{
			OnLoadClearedAction?.Invoke(p0);
		}
		public override void OnLoadFailed(Drawable? p0)
		{
			OnLoadFailedAction?.Invoke(p0);
		}          
		public override void OnResourceReady(JavaObject? resource, ITransition? transition) 
		{
			OnResourceReadyAction?.Invoke(resource, transition);
		} 
	}
}