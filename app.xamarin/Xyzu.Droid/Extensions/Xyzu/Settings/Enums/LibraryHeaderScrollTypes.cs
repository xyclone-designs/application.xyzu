#nullable enable

using Android.Content;

using System;

using Xyzu.Droid;

namespace Xyzu.Settings.Enums
{
	public enum LibraryHeaderScrollTypes
	{
		PinToTop,
		PinToTopScroll,
		Scroll,
	}

	public static class LibraryHeaderScrollTypesExtensions
	{  
		public static int AsResoureIdTitle(this LibraryHeaderScrollTypes headerscrolllayout)
		{
			return headerscrolllayout switch
			{
				LibraryHeaderScrollTypes.PinToTop => Resource.String.enums_libraryheaderscrolltypes_pintotop_title,
				LibraryHeaderScrollTypes.PinToTopScroll => Resource.String.enums_libraryheaderscrolltypes_pintotopscroll_title,
				LibraryHeaderScrollTypes.Scroll => Resource.String.enums_libraryheaderscrolltypes_scroll_title,

				_ => throw new ArgumentException(string.Format("Invalid Placeholder '{0}'", headerscrolllayout))
			};
		}	
		public static int AsResoureIdDescription(this LibraryHeaderScrollTypes headerscrolllayout)
		{
			return headerscrolllayout switch
			{
				LibraryHeaderScrollTypes.PinToTop => Resource.String.enums_libraryheaderscrolltypes_pintotop_description,
				LibraryHeaderScrollTypes.PinToTopScroll => Resource.String.enums_libraryheaderscrolltypes_pintotopscroll_description,
				LibraryHeaderScrollTypes.Scroll => Resource.String.enums_libraryheaderscrolltypes_scroll_description,

				_ => throw new ArgumentException(string.Format("Invalid Placeholder '{0}'", headerscrolllayout))
			};
		}
		public static string? AsStringTitle(this LibraryHeaderScrollTypes headerscrolllayouts, Context? context)
		{
			if (headerscrolllayouts.AsResoureIdTitle() is int resourcetitle)
				return context?.Resources?.GetString(resourcetitle);

			return null;
		}
		public static string? AsStringDescription(this LibraryHeaderScrollTypes headerscrolllayouts, Context? context)
		{
			if (headerscrolllayouts.AsResoureIdDescription() is int resourcedescription)
				return context?.Resources?.GetString(resourcedescription);

			return null;
		}
	}
}