#nullable enable

using System;

namespace Android.Views
{
	public class OnClickListener : Java.Lang.Object, View.IOnClickListener
	{
		public OnClickListener() { }
		public OnClickListener(Action<View?> onmenuitemclickaction)
		{
			OnClickAction = onmenuitemclickaction;
		}

		public Action<View?>? OnClickAction { get; set; }

		public void OnClick(View? view)
		{
			OnClickAction?.Invoke(view);
		}
	}
}