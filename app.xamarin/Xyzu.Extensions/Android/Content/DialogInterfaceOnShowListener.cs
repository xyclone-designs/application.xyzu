#nullable enable

using System;

namespace Android.Content
{
	public class DialogInterfaceOnShowListener : Java.Lang.Object, IDialogInterfaceOnShowListener
	{
		public DialogInterfaceOnShowListener() { }
		public DialogInterfaceOnShowListener(Action<IDialogInterface?> onshowaction)
		{
			OnShowAction = onshowaction;
		}

		public Action<IDialogInterface?>? OnShowAction { get; set; }

		public void OnShow(IDialogInterface? dialog)
		{
			OnShowAction?.Invoke(dialog);
		}
	}
}