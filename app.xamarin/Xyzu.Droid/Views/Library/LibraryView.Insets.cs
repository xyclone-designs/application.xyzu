#nullable enable

using Xyzu.Views.LibraryItem;

namespace Xyzu.Views.Library
{
	public partial class LibraryView 
	{
		protected string InsetKeyTop(string key) => string.Format("{0}.TOP", key);
		protected string InsetKeyEnd(string key) => string.Format("{0}.END", key);
		protected string InsetKeyStart(string key) => string.Format("{0}.START", key);
		protected string InsetKeyBottom(string key) => string.Format("{0}.BOTTOM", key);

		protected virtual LibraryItemInsetView? InsetTopView { get; }
		protected virtual LibraryItemInsetView? InsetEndView { get; }
		protected virtual LibraryItemInsetView? InsetStartView { get; }
		protected virtual LibraryItemInsetView? InsetBottomView { get; }

		public virtual void AddInsets(string key, int? top, int? start, int? end, int? bottom) 
		{
			if (top.HasValue) 
				InsetTopView?.AddInset(InsetKeyTop(key), top.Value);

			if (start.HasValue) 
				InsetStartView?.AddInset(InsetKeyStart(key), start.Value);	

			if (end.HasValue) 
				InsetEndView?.AddInset(InsetKeyEnd(key), end.Value);

			if (bottom.HasValue) 
				InsetBottomView?.AddInset(InsetKeyBottom(key), bottom.Value);
		}
		public virtual void RemoveInsets(string key, bool top = true, bool start = true, bool end = true, bool bottom = true)
		{
			if (top)
				InsetTopView?.RemoveInset(InsetKeyTop(key));

			if (start)
				InsetStartView?.RemoveInset(InsetKeyStart(key));

			if (end)
				InsetEndView?.RemoveInset(InsetKeyEnd(key));

			if (bottom)
				InsetBottomView?.RemoveInset(InsetKeyBottom(key));
		}
	}
}