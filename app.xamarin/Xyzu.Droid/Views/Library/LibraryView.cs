#nullable enable

using Android.Content;
using Android.Util;
using AndroidX.SwipeRefreshLayout.Widget;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Fragments.Library;
using Xyzu.Library.Models;
using Xyzu.Widgets.RecyclerViews;

using IXyzuImages = Xyzu.Images.IImages;
using IXyzuPlayer = Xyzu.Player.IPlayer;
using IXyzuPlayerQueue = Xyzu.Player.IQueue;
using IXyzuLibrary = Xyzu.Library.ILibrary;

namespace Xyzu.Views.Library
{
	public partial class LibraryView : SwipeRefreshLayout, ILibrary
	{
		public LibraryView(Context context) : base(context)
		{
			Init(context, null);
		}
		public LibraryView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}

		private IXyzuImages? _Images;
		private IXyzuPlayer? _Player;
		private IXyzuLibrary? _Library;
		private ILibrary.INavigatable? _Navigatable;
		private ISharedPreferences? _SharedPreferences;
		private LibraryFragment? _LibraryFragment;

		public IXyzuImages? Images 
		{
			get => _Images;
			set
			{
				_Images = value;

				PropertyChanged();
			}
		}
		public IXyzuPlayer? Player 
		{ 
			get => _Player;
			set
			{
				if (_Player != null)
					_Player.Queue.PropertyChanged -= PropertyChangedPlayerQueue;

				_Player = value;

				if (_Player != null)
					_Player.Queue.PropertyChanged += PropertyChangedPlayerQueue;

				PropertyChanged();
			}
		}
		public IXyzuLibrary? Library 
		{ 
			get => _Library;
			set
			{
				_Library = value;

				PropertyChanged();
			}
		}
		public ILibrary.INavigatable? Navigatable 
		{ 
			get => _Navigatable;
			set
			{
				_Navigatable = value;

				PropertyChanged();
			}
		}
		public ISharedPreferences? SharedPreferences 
		{ 
			get => _SharedPreferences;
			set
			{
				_SharedPreferences = value;

				PropertyChanged();
			}
		}					
		public LibraryFragment? LibraryFragment 
		{ 
			get => _LibraryFragment;
			set
			{
				_LibraryFragment = value;

				PropertyChanged();
			}
		}
		public CancellationToken Cancellationtoken { get; set; }

		protected virtual string? QueueId { get; }
		protected virtual LibraryItemsRecyclerView.Adapter? LibraryAdpter { get; }

		protected virtual void Configure() { }
		protected virtual void Init(Context context, IAttributeSet? attrs)
		{
			SetColorSchemeResources(
				colorResIds: new int[]
				{
					Resource.Color.ColorPrimarySurface,
					Resource.Color.ColorPrimary,
					Resource.Color.ColorPrimaryDark,
					Resource.Color.ColorPrimaryLight,
					Resource.Color.ColorPrimaryDark,
					Resource.Color.ColorPrimary,
					Resource.Color.ColorPrimarySurface,
				});
		}
		protected virtual void PropertyChanged([CallerMemberName] string? propertyname = null) { }
		protected virtual void PropertyChangedLibraryItemsAdapter(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(LibraryItemsRecyclerView.Adapter.SelectedPositions):
					ProcessMenuOptions((LibraryItemsRecyclerView.Adapter)sender);
					break;

				default: break;
			}
		}
		protected virtual void PropertyChangedSettings(object sender, PropertyChangedEventArgs args) { }
		protected virtual void PropertyChangedPlayerQueue(object sender, PropertyChangedEventArgs args) 
		{
			switch (args.PropertyName)
			{
				case nameof(IXyzuPlayerQueue.CurrentIndex) when
				LibraryAdpter != null &&
				Player?.Queue.Id != null && QueueId != null && QueueId == Player.Queue.Id:

					if (Player.Queue.PreviousIndex.HasValue) 
						LibraryAdpter.NotifyItemChanged(Player.Queue.PreviousIndex.Value);
																										   
					if (Player.Queue.CurrentIndex.HasValue) 
						LibraryAdpter.NotifyItemChanged(Player.Queue.CurrentIndex.Value);
					break;

				default: break;
			}
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			Configure();

			Refresh += OnRefresh;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			Refresh += OnRefresh;
		}

		public virtual async Task OnRefresh(bool force = false)
		{
			Refreshing = true;

			await Task.CompletedTask;

			Refreshing = false;
		}
		public virtual async void OnRefresh(object sender, EventArgs args)
		{
			await OnRefresh(true);
		}
	}
}