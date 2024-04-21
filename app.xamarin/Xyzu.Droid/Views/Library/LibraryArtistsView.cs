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
using Xyzu.Images;
using Xyzu.Menus;
using Xyzu.Library.Models;
using Xyzu.Settings.UserInterface.Library;
using Xyzu.Widgets.RecyclerViews;
using Xyzu.Views.LibraryItem;

namespace Xyzu.Views.Library
{
	public class LibraryArtistsView : LibraryView, ILibraryArtists
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_artists;

			public const int Artists_RecyclerView = Resource.Id.xyzu_view_library_artists_artists_artistsrecyclerview;

		}

		public LibraryArtistsView(Context context) : base(context) { }
		public LibraryArtistsView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.Artist
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.Artist.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Artist.GoToArtist => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Artist.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.Artist.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Artist.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Artist.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Artist.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Artist.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			Artists.LibraryItemsLayoutManager.LibraryLayoutType = Settings.LayoutType;

			Artists.LibraryItemsAdapter.Images = Images;
			Artists.LibraryItemsAdapter.Library = Library;
			Artists.LibraryItemsAdapter.LibraryLayoutType = Settings.LayoutType;
			Artists.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			Artists.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				if (viewholder.ItemView is LibraryItemView libraryitemview)
					libraryitemview.ImagesOperations = IImages.DefaultOperations.CirculariseDownsample;

				viewholder.LibraryItem.SetArtist(Artists.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Artist(Context));
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
			};
			Artists.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, Artists.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						Navigatable?.NavigateArtist(Artists.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						Artists.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					Artists.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						Artists.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						Artists.LibraryItemsAdapter.SelectLibraryItemsFocused();
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

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(Artists.LibraryItemsAdapter);

			switch (Artists.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Artists = Artists.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = Artists.LibraryItemsAdapter.LibraryItems.Index(Artists.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Artists = Artists.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(IArtistsSettings.LayoutType):
					Artists.ReloadLayout(Settings.LayoutType);
					break;
				case nameof(IArtistsSettings.IsReversed):
					Artists.LibraryItemsAdapter.LibraryItems.Reverse();
					break;
				case nameof(IArtistsSettings.SortKey):
					Artists.LibraryItemsAdapter.SetLibraryItems(Artists.LibraryItemsAdapter.LibraryItems.Sort(Settings.SortKey, Settings.IsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryArtists(Settings)?
					.Apply();
		}

		private IArtistsSettings? _Settings;
		private ArtistsRecyclerView? _Artists;

		protected override string? QueueId
		{
			get => MenuOptionsUtils.QueueIdArtists(Settings);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(Artists.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => Artists.LibraryItemsAdapter;
		}

		public IArtistsSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryArtists();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IArtistsSettings.Defaults.ArtistsSettings;
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
		public ArtistsRecyclerView Artists
		{
			get => _Artists ??= FindViewById(Ids.Artists_RecyclerView) as ArtistsRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || (force is false && Artists.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			IAsyncEnumerable<IArtist> artists = Library.Artists.GetArtists(
				identifiers: null,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GenerateArtistRetriever(
					artists: Artists.LibraryItemsAdapter.LibraryItems,
					modelsortkey: Settings.SortKey,
					librarylayouttype: Settings.LayoutType))
				.Sort(Settings.SortKey, Settings.IsReversed);

			Artists.LibraryItemsAdapter.LibraryItems.Clear();

			await Artists.LibraryItemsAdapter.LibraryItems.AddRange(artists, Cancellationtoken);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && Artists.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				Artists.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.Artist.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistArtists(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Artist.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueArtists(MenuVariables);
					return true;

				case Menus.LibraryItem.Artist.Delete:
					MenuOptionsUtils.DeleteArtists(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Artist.EditInfo:
					MenuOptionsUtils.EditInfoArtist(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Artist.GoToArtist:
					DismissMenuOptions();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;

				case Menus.LibraryItem.Artist.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlayArtists(MenuVariables, null);
					return true;

				case Menus.LibraryItem.Artist.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.Artist.ViewInfo:
					MenuOptionsUtils.ViewInfoArtist(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			Artists.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}
	}
}