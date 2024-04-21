#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Menus;
using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Widgets.RecyclerViews;
using Xyzu.Views.LibraryItem;

namespace Xyzu.Views.Library
{
	public class LibraryAlbumsView : LibraryView, ILibraryAlbums
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_albums;

			public const int Albums_RecyclerView = Resource.Id.xyzu_view_library_albums_albums_albumsrecyclerview;
		}

		public LibraryAlbumsView(Context context) : base(context) { }
		public LibraryAlbumsView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.Album
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.Album.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Album.GoToAlbum => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Album.GoToArtist => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Album.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.Album.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Album.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Album.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Album.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Album.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			Albums.LibraryItemsLayoutManager.LibraryLayoutType = Settings.LayoutType;

			Albums.LibraryItemsAdapter.Images = Images;
			Albums.LibraryItemsAdapter.Library = Library;
			Albums.LibraryItemsAdapter.LibraryLayoutType = Settings.LayoutType;
			Albums.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			Albums.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				viewholder.LibraryItem.SetAlbum(Albums.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Album(Context));
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
			};
			Albums.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, Albums.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						Navigatable?.NavigateAlbum(Albums.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						Albums.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					Albums.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						Albums.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						Albums.LibraryItemsAdapter.SelectLibraryItemsFocused();
						MenuOptionsDialog.Show();
						break;

					default: break;
				}
			};
		}
		protected override void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			base.Init(context, attrs);
		}
		protected override void ProcessMenuVariables(MenuOptionsUtils.VariableContainer menuvariables, MenuOptions? menuoption)
		{
			base.ProcessMenuVariables(menuvariables, menuoption);

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(Albums.LibraryItemsAdapter);

			switch (Albums.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Albums = Albums.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = Albums.LibraryItemsAdapter.LibraryItems.Index(Albums.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Albums = Albums.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(IAlbumsSettings.LayoutType):
					Albums.ReloadLayout(Settings.LayoutType);
					break;
				case nameof(IAlbumsSettings.IsReversed):
					Albums.LibraryItemsAdapter.LibraryItems.Reverse();
					break;
				case nameof(IAlbumsSettings.SortKey):
					Albums.LibraryItemsAdapter.SetLibraryItems(Albums.LibraryItemsAdapter.LibraryItems.Sort(Settings.SortKey, Settings.IsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryAlbums(Settings)?
					.Apply();
		}

		private IAlbumsSettings? _Settings;
		private AlbumsRecyclerView? _Albums;

		protected override string? QueueId
		{
			get => MenuOptionsUtils.QueueIdAlbums(Settings);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(Albums.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => Albums.LibraryItemsAdapter;
		}

		public IAlbumsSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryAlbums();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IAlbumsSettings.Defaults.AlbumsSettings;
			}
			set
			{
				if (_Settings != null)
					_Settings.PropertyChanged -= PropertyChangedSettings;

				_Settings = value;

				if (_Settings != null)
					_Settings.PropertyChanged += PropertyChangedSettings;
			}
		}
		public AlbumsRecyclerView Albums
		{
			get => _Albums ??= FindViewById(Ids.Albums_RecyclerView) as AlbumsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || (force is false && Albums.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;
				
				return;
			}

			Refreshing = true;

			IAsyncEnumerable<IAlbum> albums = Library.Albums.GetAlbums(
				identifiers: null,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GenerateAlbumRetriever(
					albums: Albums.LibraryItemsAdapter.LibraryItems,
					modelsortkey: Settings.SortKey,
					librarylayouttype: Settings.LayoutType))
				.Sort(Settings.SortKey, Settings.IsReversed);

			Albums.LibraryItemsAdapter.LibraryItems.Clear();

			await Albums.LibraryItemsAdapter.LibraryItems.AddRange(albums, Cancellationtoken);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && Albums.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				Albums.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.Album.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistAlbums(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Album.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueAlbums(MenuVariables);
					return true;

				case Menus.LibraryItem.Album.Delete:
					MenuOptionsUtils.DeleteAlbums(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Album.EditInfo:
					MenuOptionsUtils.EditInfoAlbum(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Album.GoToAlbum:
					DismissMenuOptions();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;

				case Menus.LibraryItem.Album.GoToArtist:
					DismissMenuOptions();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;

				case Menus.LibraryItem.Album.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlayAlbums(MenuVariables, null);
					return true;

				case Menus.LibraryItem.Album.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.Album.ViewInfo:
					MenuOptionsUtils.ViewInfoAlbum(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			Albums.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}
	}
}