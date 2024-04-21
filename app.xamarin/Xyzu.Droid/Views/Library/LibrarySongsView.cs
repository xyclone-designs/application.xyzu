#nullable enable

using Android.Content;
using Android.Runtime;
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
	public class LibrarySongsView : LibraryView, ILibrarySongs
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_songs;

			public const int Songs_RecyclerView = Resource.Id.xyzu_view_library_songs_songs_songsrecyclerview;
		}

		public LibrarySongsView(Context context) : base(context) { }
		public LibrarySongsView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.Song
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.Song.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Song.GoToAlbum => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Song.GoToArtist => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Song.GoToGenre => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Song.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.Song.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Song.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Song.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Song.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Song.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			Songs.LibraryItemsLayoutManager.LibraryLayoutType = Settings.LayoutType;

			Songs.LibraryItemsAdapter.Images = Images;
			Songs.LibraryItemsAdapter.Library = Library;
			Songs.LibraryItemsAdapter.LibraryLayoutType = Settings.LayoutType;
			Songs.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			Songs.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				viewholder.LibraryItem.SetSong(Songs.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Song(Context));
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
			};
			Songs.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, Songs.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
						OnMenuOptionClick(MenuOptions.Play);
						Navigatable?.NavigateSong(Songs.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
						Songs.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					Songs.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						Songs.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						Songs.LibraryItemsAdapter.SelectLibraryItemsFocused();
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

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(Songs.LibraryItemsAdapter);

			switch (Songs.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Songs = Songs.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = Songs.LibraryItemsAdapter.LibraryItems.Index(Songs.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Songs = Songs.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(ISongsSettings.LayoutType):
					Songs.ReloadLayout(Settings.LayoutType);
					break;
				case nameof(ISongsSettings.IsReversed):
					Songs.LibraryItemsAdapter.LibraryItems.Reverse();
					break;
				case nameof(ISongsSettings.SortKey):
					Songs.LibraryItemsAdapter.SetLibraryItems(Songs.LibraryItemsAdapter.LibraryItems.Sort(Settings.SortKey, Settings.IsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibrarySongs(Settings)?
					.Apply();
		}

		private ISongsSettings? _Settings;
		private SongsRecyclerView? _Songs;

		protected override string? QueueId 
		{
			get => MenuOptionsUtils.QueueIdSongs(Settings);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(Songs.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => Songs.LibraryItemsAdapter;
		}

		public ISongsSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibrarySongs();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? ISongsSettings.Defaults.SongsSettings;
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
		public SongsRecyclerView Songs
		{
			get => _Songs ??= FindViewById(Ids.Songs_RecyclerView) as SongsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || (force is false && Songs.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			IAsyncEnumerable<ISong> songs = Library.Songs
				.GetSongs(
					identifiers: null,
					cancellationToken: Cancellationtoken,
					retriever: LibraryItemView.Retrievers.GenerateSongRetriever(
						songs: Songs.LibraryItemsAdapter.LibraryItems,
						modelsortkey: Settings.SortKey,
						librarylayouttype: Settings.LayoutType))
				.Sort(Settings.SortKey, Settings.IsReversed);

			Songs.LibraryItemsAdapter.LibraryItems.Clear();
			
			await Songs.LibraryItemsAdapter.LibraryItems.AddRange(songs, Cancellationtoken);
			
			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && Songs.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				Songs.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.Song.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Song.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueSongs(MenuVariables);
					return true;

				case Menus.LibraryItem.Song.Delete:
					MenuOptionsUtils.DeleteSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Song.EditInfo:
					MenuOptionsUtils.EditInfoSong(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Song.GoToAlbum:
					DismissMenuOptions();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;

				case Menus.LibraryItem.Song.GoToArtist:
					DismissMenuOptions();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;

				case Menus.LibraryItem.Song.GoToGenre:
					DismissMenuOptions();
					MenuOptionsUtils.GoToGenre(MenuVariables);
					return true;

				case Menus.LibraryItem.Song.Play:	  
					DismissMenuOptions();
					MenuOptionsUtils.PlaySongs(MenuVariables, null);
					return true;

				case Menus.LibraryItem.Song.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.Song.ViewInfo:
					MenuOptionsUtils.ViewInfoSong(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			Songs.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}
	}
}