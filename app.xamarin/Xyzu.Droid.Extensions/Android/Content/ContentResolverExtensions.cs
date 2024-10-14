using Android.OS;

using System;

using AndroidUri = Android.Net.Uri;
using SystemUri = System.Uri;

namespace Android.Content
{
	public static class ContentResolverExtensions
	{
		public static ParcelFileDescriptor? OpenFileDescriptorSafe(this ContentResolver contentresolver, SystemUri? uri, string? mode = null)
		{
			return contentresolver.OpenFileDescriptorSafe(uri?.ToAndroidUri(), mode);
		}
		public static ParcelFileDescriptor? OpenFileDescriptorSafe(this ContentResolver contentresolver, AndroidUri? uri, string? mode = null)
		{
			ParcelFileDescriptor? parcelfiledescriptor = null;

			if (uri is null)
				return null;
			try
			{
				parcelfiledescriptor = contentresolver.OpenFileDescriptor(uri, mode ?? "r");
			}
			catch (Exception) { }

			return parcelfiledescriptor;
		}
	}
}