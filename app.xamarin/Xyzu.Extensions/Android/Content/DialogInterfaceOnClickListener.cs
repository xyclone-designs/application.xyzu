#nullable enable

using System;

namespace Android.Content
{
	public class DialogInterfaceOnClickListener : Java.Lang.Object, IDialogInterfaceOnClickListener
	{
		public DialogInterfaceOnClickListener() { }
		public DialogInterfaceOnClickListener(Action<IDialogInterface?, int> onclickaction)
		{
			OnClickAction = onclickaction;
		}

		public Action<IDialogInterface?, int>? OnClickAction { get; set; }

		public void OnClick(IDialogInterface? dialog, int which)
		{
			OnClickAction?.Invoke(dialog, which);
		}
	}
}