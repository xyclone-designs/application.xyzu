using Android.Content;
using Android.Runtime;
using Android.Util;

using Xyzu.Droid;
using Xyzu.Library.Models;

namespace Xyzu.Widgets.RecyclerViews.LibraryItems
{
	[Register("xyzu/widgets/recyclerviews/libraryitems/AlbumsRecyclerView")]
	public class AlbumsRecyclerView : LibraryItemsRecyclerView<IAlbum>
	{
		public AlbumsRecyclerView(Context context) : this(context, null!) { }
		public AlbumsRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_LibraryItems) { }
		public AlbumsRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetAdapter(_LibraryItemsAdapter = new Adapter<IAlbum>(this));
		}

		private Adapter<IAlbum> _LibraryItemsAdapter;
		public override Adapter<IAlbum> LibraryItemsAdapter
		{
			set => SetAdapter(_LibraryItemsAdapter = value);
			get => GetAdapter() as Adapter<IAlbum> ?? _LibraryItemsAdapter;
		}
	}
}