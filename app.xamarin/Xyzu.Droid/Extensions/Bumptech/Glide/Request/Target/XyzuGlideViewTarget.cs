#nullable enable

using Android.Graphics.Drawables;
using Android.Views;

using Bumptech.Glide.Request.Transition;

using System;

using JavaObject = Java.Lang.Object;

namespace Bumptech.Glide.Request.Target
{
	public class XyzuGlideViewTarget : CustomViewTarget
	{
		public XyzuGlideViewTarget(View view) : base(view) { }

		public new View View { get => (View)base.View; }

		public Action<Drawable>? OnLoadFailedAction { get; set; }          
		public Action<JavaObject, ITransition>? OnResourceReadyAction { get; set; }          

		public override void OnLoadFailed(Drawable p0)
		{
			OnLoadFailedAction?.Invoke(p0);
		}          
		public override void OnResourceReady(JavaObject resource, ITransition transition) 
		{
			OnResourceReadyAction?.Invoke(resource, transition);
		} 
		protected override void OnResourceCleared(Drawable p0) 
		{ }
	}
}