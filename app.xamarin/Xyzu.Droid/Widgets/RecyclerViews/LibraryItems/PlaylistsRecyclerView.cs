using Android.Content;
using Android.Runtime;
using Android.Util;

using Xyzu.Droid;
using Xyzu.Library.Models;

namespace Xyzu.Widgets.RecyclerViews.LibraryItems
{
	[Register("xyzu/widgets/recyclerviews/libraryitems/PlaylistsRecyclerView")]
	public class PlaylistsRecyclerView : LibraryItemsRecyclerView<IPlaylist>
	{
		public PlaylistsRecyclerView(Context context) : this(context, null!) { }
		public PlaylistsRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_LibraryItems) { }
		public PlaylistsRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetAdapter(_LibraryItemsAdapter = new Adapter<IPlaylist>(this));
		}

		private Adapter<IPlaylist> _LibraryItemsAdapter;
		public override Adapter<IPlaylist> LibraryItemsAdapter
		{
			set => SetAdapter(_LibraryItemsAdapter = value);
			get => GetAdapter() as Adapter<IPlaylist> ?? _LibraryItemsAdapter;
		}
	}
}