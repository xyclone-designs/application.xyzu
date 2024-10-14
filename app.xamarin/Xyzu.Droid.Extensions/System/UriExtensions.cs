using AndroidUri = Android.Net.Uri;
using JavaUri = Java.Net.URI;
using SystemUri = System.Uri;

namespace System
{
	public static class UriExtensions
	{
		public static AndroidUri? ToAndroidUri(this SystemUri systemuri)
		{
			string systemuriuristring = systemuri.ToString();

			return AndroidUri.Parse(systemuriuristring);
		}	 
		public static JavaUri? ToJavaUri(this SystemUri systemuri)
		{
			string systemuriuristring = systemuri
				.ToString()
				.Replace(" ", "%20");

			return JavaUri.Create(systemuriuristring);
		}
	}
}