#nullable enable

using System;

using JavaObject = Java.Lang.Object;

namespace Android.OS
{
		public class CancellationSignalOnCancelListener : JavaObject, CancellationSignal.IOnCancelListener
		{
			public Action? OnCancelAction { get; set; }

			public void OnCancel()
			{
				OnCancelAction?.Invoke();
			}
		}
}