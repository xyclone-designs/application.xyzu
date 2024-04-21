#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Menus;
using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Widgets.RecyclerViews;
using Xyzu.Views.LibraryItem;

using ILibraryIdentifiers = Xyzu.Library.ILibrary.IIdentifiers;
using IXyzuPlayer = Xyzu.Player.IPlayer;

namespace Xyzu.Views.Library
{
	public class LibraryQueueView : LibraryView, ILibraryQueue
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_queue;

			public const int QueueSongs_RecyclerView = Resource.Id.xyzu_view_library_queue_queuesongs_songsrecyclerview;
		}

		public LibraryQueueView(Context context) : base(context) { }
		public LibraryQueueView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.QueueSong
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.QueueSong.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.QueueSong.GoToAlbum => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.QueueSong.GoToArtist => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.QueueSong.GoToGenre => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.QueueSong.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.QueueSong.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.QueueSong.MoveDown => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.QueueSong.MoveToBottom => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.QueueSong.MoveToTop => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.QueueSong.MoveUp => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.QueueSong.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.QueueSong.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.QueueSong.Remove => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.QueueSong.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			QueueSongs.LibraryItemsLayoutManager.LibraryLayoutType = Settings.LayoutType;

			QueueSongs.LibraryItemsAdapter.Images = Images;
			QueueSongs.LibraryItemsAdapter.Library = Library;
			QueueSongs.LibraryItemsAdapter.LibraryLayoutType = Settings.LayoutType;
			QueueSongs.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			QueueSongs.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				viewholder.LibraryItem.SetSong(QueueSongs.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Song(Context));
				viewholder.LibraryItem.SetPlaying(Player, null);
			};
			QueueSongs.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, QueueSongs.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						OnMenuOptionClick(MenuOptions.Play);
						Navigatable?.NavigateSong(QueueSongs.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						QueueSongs.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					QueueSongs.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						QueueSongs.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						QueueSongs.LibraryItemsAdapter.SelectLibraryItemsFocused();
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

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(QueueSongs.LibraryItemsAdapter);

			switch (QueueSongs.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Songs = QueueSongs.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = QueueSongs.LibraryItemsAdapter.LibraryItems.Index(QueueSongs.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.Songs = QueueSongs.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(IQueueSettings.LayoutType):
					QueueSongs.ReloadLayout(Settings.LayoutType);
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryQueue(Settings)?
					.Apply();
		}

		private IQueueSettings? _Settings;
		private SongsRecyclerView? _QueueSongs;

		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(QueueSongs.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => QueueSongs.LibraryItemsAdapter;
		}

		public new IXyzuPlayer? Player
		{
			get => base.Player;
			set
			{
				if (base.Player != null)
					base.Player.Queue.ListChanged -= Queue_ListChanged;

				base.Player = value;

				if (base.Player != null)
					base.Player.Queue.ListChanged += Queue_ListChanged;
			}
		}

		public ILibraryIdentifiers? Identifiers { get; set; }
		public IQueueSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryQueue();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IQueueSettings.Defaults.QueueSettings;
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
		public SongsRecyclerView QueueSongs
		{
			get => _QueueSongs ??= FindViewById(Ids.QueueSongs_RecyclerView) as SongsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Player is null || Library is null || (force is false && QueueSongs.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			// TODO: For some reason doing this metho the same as everywhere else caseus multiple refreshes

			QueueSongs.LibraryItemsAdapter.LibraryItems.Clear();

			IList<ISong> songs = await Task.Run(() => Library.Songs.GetSongs(
				retriever: LibraryItemView.Retrievers.GenerateSongRetriever(
					modelsortkey: null,
					librarylayouttype: Settings.LayoutType,
					songs: QueueSongs.LibraryItemsAdapter.LibraryItems),
				identifiers: new ILibraryIdentifiers.Default
				{
					SongIds = Player.Queue.Select(queueitem => queueitem.PrimaryId)

				}).ToList(), Cancellationtoken);

			QueueSongs.LibraryItemsAdapter.LibraryItems.AddRange(songs);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && QueueSongs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				QueueSongs.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.QueueSong.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.QueueSong.Delete:
					MenuOptionsUtils.DeleteSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.QueueSong.EditInfo:
					MenuOptionsUtils.EditInfoSong(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.QueueSong.GoToAlbum:
					DismissMenuOptions();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.GoToArtist:
					DismissMenuOptions();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.GoToGenre:
					DismissMenuOptions();
					MenuOptionsUtils.GoToGenre(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.MoveDown:
					MenuOptionsUtils.Play(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.MoveToBottom:
					MenuOptionsUtils.MoveToBottom(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.MoveToTop:
					MenuOptionsUtils.MoveToTop(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.MoveUp:
					MenuOptionsUtils.MoveUp(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.Play:
					DismissMenuOptions();
					if (Player?.Queue.Index(queueitem =>
					{
						return string.Equals(queueitem.PrimaryId, QueueSongs.LibraryItemsAdapter.FocusedLibraryItem?.Id, StringComparison.OrdinalIgnoreCase);

					}) is int index) Player.Skip(index);
					return true;

				case Menus.LibraryItem.QueueSong.Remove:
					MenuOptionsUtils.Remove(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.QueueSong.ViewInfo:
					MenuOptionsUtils.ViewInfoSong(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			QueueSongs.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}

		private async void Queue_ListChanged(object sender, NotifyListChangedEventArgs args)
		{
			await OnRefresh(true);
		}
	}
}