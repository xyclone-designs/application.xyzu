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