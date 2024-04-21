#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library;
using Xyzu.Library.Models;
using Xyzu.Player;
using Xyzu.Settings.Enums;
using Xyzu.Views.LibraryItem;

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