#nullable enable

using System;

using JavaObject = Java.Lang.Object;

namespace AndroidX.Activity.Result
{
	public class ActivityResultCallback : JavaObject, IActivityResultCallback
	{
		public Action<JavaObject?>? OnActivityResultAction { get; set; }

		public void OnActivityResult(JavaObject? result)
		{
			OnActivityResultAction?.Invoke(result);
		}
	}
}