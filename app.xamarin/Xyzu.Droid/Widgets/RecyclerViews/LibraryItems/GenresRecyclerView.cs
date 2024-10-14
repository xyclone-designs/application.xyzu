using Android.Content;
using Android.Runtime;
using Android.Util;

using Xyzu.Droid;
using Xyzu.Library.Models;

namespace Xyzu.Widgets.RecyclerViews.LibraryItems
{
	[Register("xyzu/widgets/recyclerviews/libraryitems/GenresRecyclerView")]
	public class GenresRecyclerView : LibraryItemsRecyclerView<IGenre>
	{
		public GenresRecyclerView(Context context) : this(context, null!) { }
		public GenresRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_LibraryItems) { }
		public GenresRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetAdapter(_LibraryItemsAdapter = new Adapter<IGenre>(this));
		}

		private Adapter<IGenre> _LibraryItemsAdapter;
		public override Adapter<IGenre> LibraryItemsAdapter
		{
			set => SetAdapter(_LibraryItemsAdapter = value);
			get => GetAdapter() as Adapter<IGenre> ?? _LibraryItemsAdapter;
		}
	}
}