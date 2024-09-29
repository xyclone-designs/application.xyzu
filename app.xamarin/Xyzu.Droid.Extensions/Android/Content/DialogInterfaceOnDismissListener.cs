#nullable enable

using System;

namespace Android.Content
{
	public class DialogInterfaceOnDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
	{
		public DialogInterfaceOnDismissListener() { }
		public DialogInterfaceOnDismissListener(Action<IDialogInterface?> ondismissaction)
		{
			OnDismissAction = ondismissaction;
		}

		public Action<IDialogInterface?>? OnDismissAction { get; set; }

		public void OnDismiss(IDialogInterface? dialog)
		{
			OnDismissAction?.Invoke(dialog);
		}
	}
}