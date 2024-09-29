#nullable enable

using System;							   

using AndroidUri = Android.Net.Uri;

namespace Android.Media
{
	public class OnScanCompletedListener : Java.Lang.Object, MediaScannerConnection.IOnScanCompletedListener
	{
		public Action<string?, AndroidUri?>? OnScanCompletedAction { get; set; }

		public void OnScanCompleted(string? path, AndroidUri? uri)
		{
			OnScanCompletedAction?.Invoke(path, uri);
		}
	}
}