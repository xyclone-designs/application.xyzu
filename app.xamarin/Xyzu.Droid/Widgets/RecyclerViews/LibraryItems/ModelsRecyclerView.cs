using Android.Content;
using Android.Runtime;
using Android.Util;

using Xyzu.Droid;
using Xyzu.Library.Models;

namespace Xyzu.Widgets.RecyclerViews.LibraryItems
{
	[Register("xyzu/widgets/recyclerviews/libraryitems/ModelsRecyclerView")]
	public class ModelsRecyclerView : LibraryItemsRecyclerView<IModel>
	{
		public ModelsRecyclerView(Context context) : this(context, null!) { }
		public ModelsRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_RecyclerView_LibraryItems) { }
		public ModelsRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetAdapter(_LibraryItemsAdapter = new Adapter<IModel>(this));
		}

		private Adapter<IModel> _LibraryItemsAdapter;
		public override Adapter<IModel> LibraryItemsAdapter
		{
			set => SetAdapter(_LibraryItemsAdapter = value);
			get => GetAdapter() as Adapter<IModel> ?? _LibraryItemsAdapter;
		}
	}
}