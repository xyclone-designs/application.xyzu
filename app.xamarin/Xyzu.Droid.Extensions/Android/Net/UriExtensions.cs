using System;

using AndroidUri = Android.Net.Uri;
using JavaUri = Java.Net.URI;
using SystemUri = System.Uri;

namespace Android.Net
{
	public static class UriExtensions
	{
		public static SystemUri? ToSystemUri(this AndroidUri androiduri)
		{
			string? androiduristring = androiduri.ToString();
			UriKind androidurikind = (androiduri.IsRelative, androiduri.IsAbsolute) switch
			{
				(true, false) => UriKind.Relative,
				(false, true) => UriKind.Absolute,

				(_, _) => UriKind.RelativeOrAbsolute
			};

			if (androiduristring is null || SystemUri.TryCreate(androiduristring, androidurikind, out SystemUri? systemuri) is false)
				return null;

			return systemuri;
		}
		public static JavaUri? ToJavaUri(this AndroidUri androiduri)
		{
			string? systemuriuristring = androiduri
				.ToString()?
				.Replace(" ", "%20");

			if (systemuriuristring is null)
				return null;

			return JavaUri.Create(systemuriuristring);
		}
	}
}