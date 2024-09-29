#nullable enable

using Android.Content.Res;

using System;

namespace Android.Content
{
	public static class ResourcesExtensions
	{
		public static int? GetNavigationBarHeight(this Resources resources)
		{
			int? navigationbardividerheight = null;

			try
			{
				int? navigationbarheightidid = resources.GetIdentifier(						
					name: ResourceNames.NavigationBarHeight, 
					defType: ResourceDefTypes.Dimen, 
					defPackage: ResourceDefPackages.Android);

				if (navigationbarheightidid.HasValue && navigationbarheightidid.Value > 0)
					navigationbardividerheight = resources.GetDimensionPixelSize(navigationbarheightidid.Value);
			}
			catch (Exception) { }

			return navigationbardividerheight;
		}	   
		public static int? GetStatusBarHeight(this Resources resources)
		{
			int? statusbarheight = null;

			try
			{
				int? statusbarheightid = resources.GetIdentifier(						
					name: ResourceNames.StatusBarHeight, 
					defType: ResourceDefTypes.Dimen, 
					defPackage: ResourceDefPackages.Android);

				if (statusbarheightid.HasValue && statusbarheightid.Value > 0)
					statusbarheight = resources.GetDimensionPixelSize(statusbarheightid.Value);
			}
			catch (Exception) { }

			return statusbarheight;
		}
	}
}