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