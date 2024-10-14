using Android.Content;
using Android.Runtime;
using Android.Util;

using Xyzu.Droid;
using Xyzu.Library.Models;

namespace Xyzu.Widgets.RecyclerViews.LibraryItems
{
	[Register("xyzu/widgets/recyclerviews/libraryitems/ArtistsRecyclerView")]
	public class ArtistsRecyclerView : LibraryItemsRecyclerView<IArtist>
	{
		public ArtistsRecyclerView(Context context) : this(context, null!) { }
		public ArtistsRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_LibraryItems) { }
		public ArtistsRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetAdapter(_LibraryItemsAdapter = new Adapter<IArtist>(this));
		}

		private Adapter<IArtist> _LibraryItemsAdapter;
		public override Adapter<IArtist> LibraryItemsAdapter
		{
			set => SetAdapter(_LibraryItemsAdapter = value);
			get => GetAdapter() as Adapter<IArtist> ?? _LibraryItemsAdapter;
		}
	}
}