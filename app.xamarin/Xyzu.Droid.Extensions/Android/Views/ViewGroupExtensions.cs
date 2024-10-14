using System;
using System.Collections.Generic;

namespace Android.Views
{
	public static class ViewGroupExtensions
	{
		public const int ChildrenLevelCountAll = -1;

		public static IEnumerable<View> GetAllChildren(this ViewGroup viewgroup, int levels = ChildrenLevelCountAll)
		{
			if (levels < ChildrenLevelCountAll) 
				throw new ArgumentException(string.Format("levels arg '{0}' is not >= 0 or -1 (ViewGroupExtensions.ChildreLevelCountAll)", levels));


			int childindex = 0;
			int? nextlevel = levels == ChildrenLevelCountAll ? new int?() : levels;

			while (true)
			{
				if (nextlevel.HasValue is true && nextlevel.Value == ChildrenLevelCountAll)
					break;

				View? child = childindex < viewgroup.ChildCount ? viewgroup.GetChildAt(childindex) : null;

				if (child is null)
					break;

				yield return child;

				if (child is ViewGroup childviewgroup && (nextlevel.HasValue is false || nextlevel.Value != ChildrenLevelCountAll + 1))
				{
					IEnumerable<View> childviewgroupchildren = nextlevel.HasValue is false
						? childviewgroup.GetAllChildren()
						: childviewgroup.GetAllChildren(nextlevel.Value - 1);

					foreach (View viewgroupview in childviewgroupchildren)
						yield return viewgroupview;
				}
					
				childindex++;
			}
		}
	}
}