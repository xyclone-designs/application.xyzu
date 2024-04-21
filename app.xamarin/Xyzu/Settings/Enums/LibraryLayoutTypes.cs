using System.Collections.Generic;

namespace Xyzu.Settings.Enums
{
	public enum LibraryLayoutTypes
	{
		GridSmall,
		ListSmall,
		GridMedium,
		ListMedium,
		GridLarge,
		ListLarge,
	}

	public static class LibraryLayoutTypesExtensions
	{
		public static IEnumerable<LibraryLayoutTypes> NeatlyOrdered(this IEnumerable<LibraryLayoutTypes> librarylayouttypes)
		{
			int gridsmallcount = 0;
			int gridmediumcount = 0;
			int gridlargecount = 0;		
			int listsmallcount = 0;
			int listmediumcount = 0;
			int listlargecount = 0;

			foreach (LibraryLayoutTypes librarylayouttype in librarylayouttypes)
				switch (librarylayouttype)
				{
					case LibraryLayoutTypes.GridSmall:
						gridsmallcount++;
						break;						 
					case LibraryLayoutTypes.GridMedium:
						gridmediumcount++;
						break;		   			 
					case LibraryLayoutTypes.GridLarge:
						gridlargecount++;
						break;		  
					case LibraryLayoutTypes.ListSmall:
						listsmallcount++;
						break;						 
					case LibraryLayoutTypes.ListMedium:
						listmediumcount++;
						break;		   			 
					case LibraryLayoutTypes.ListLarge:
						listlargecount++;
						break;

					default: break;
				}

			while (true)
				if (gridsmallcount > 0)
				{
					gridsmallcount--;
					yield return LibraryLayoutTypes.GridSmall;
				}
				else if (gridmediumcount > 0)
				{
					gridmediumcount--;
					yield return LibraryLayoutTypes.GridMedium;
				}
				else if (gridlargecount > 0)
				{
					gridlargecount--;
					yield return LibraryLayoutTypes.GridLarge;
				}
				else if (listsmallcount > 0)
				{
					listsmallcount--;
					yield return LibraryLayoutTypes.ListSmall;
				}
				else if (listmediumcount > 0)
				{
					listmediumcount--;
					yield return LibraryLayoutTypes.ListMedium;
				}
				else if (listlargecount > 0)
				{
					listlargecount--;
					yield return LibraryLayoutTypes.ListLarge;
				}
				else break;
		}
	}
}
