using Android.Content;
using Android.Runtime;
using Android.Util;

using Xyzu.Droid;
using Xyzu.Library.Models;

namespace Xyzu.Widgets.RecyclerViews.LibraryItems
{
	[Register("xyzu/widgets/recyclerviews/libraryitems/SongsRecyclerView")]
	public class SongsRecyclerView : LibraryItemsRecyclerView<ISong>
	{
		public SongsRecyclerView(Context context) : this(context, null!) 
		{ }
		public SongsRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_LibraryItems)
		{ }
		public SongsRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetAdapter(_LibraryItemsAdapter = new Adapter<ISong>(this));
		}

		private Adapter<ISong> _LibraryItemsAdapter;
		public override Adapter<ISong> LibraryItemsAdapter
		{
			set => SetAdapter(_LibraryItemsAdapter = value);
			get => GetAdapter() as Adapter<ISong> ?? _LibraryItemsAdapter;
		}
	}
}