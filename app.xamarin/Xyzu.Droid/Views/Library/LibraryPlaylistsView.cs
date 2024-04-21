#nullable enable

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
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
using Xyzu.Views.LibraryItem;
using Xyzu.Views.Option;
using Xyzu.Widgets.RecyclerViews;

namespace Xyzu.Views.Library
{
	public class LibraryPlaylistsView : LibraryView, ILibraryPlaylists
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_playlists;

			public const int Playlists_RecyclerView = Resource.Id.xyzu_view_library_playlists_playlists_playlistsrecyclerview;
		}

		public LibraryPlaylistsView(Context context) : base(context) { }
		public LibraryPlaylistsView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.Playlist
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.Playlist.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Playlist.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.Playlist.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Playlist.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Playlist.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Playlist.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			Playlists.LibraryItemsLayoutManager.LibraryLayoutType = Settings.LayoutType;

			Playlists.LibraryItemsAdapter.Images = Images;
			Playlists.LibraryItemsAdapter.Library = Library;
			Playlists.LibraryItemsAdapter.LibraryLayoutType = Settings.LayoutType;
			Playlists.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			Playlists.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				viewholder.LibraryItem.SetPlaylist(Playlists.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Playlist(Context));
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
			};
			Playlists.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, Playlists.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						Navigatable?.NavigatePlaylist(Playlists.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						Playlists.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					Playlists.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						Playlists.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						Playlists.LibraryItemsAdapter.SelectLibraryItemsFocused();
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

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(Playlists.LibraryItemsAdapter);

			switch (Playlists.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Playlists = Playlists.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = Playlists.LibraryItemsAdapter.LibraryItems.Index(Playlists.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Playlists = Playlists.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(IPlaylistsSettings.LayoutType):
					Playlists.ReloadLayout(Settings.LayoutType);
					break;
				case nameof(IPlaylistsSettings.IsReversed):
					Playlists.LibraryItemsAdapter.LibraryItems.Reverse();
					break;
				case nameof(IPlaylistsSettings.SortKey):
					Playlists.LibraryItemsAdapter.SetLibraryItems(Playlists.LibraryItemsAdapter.LibraryItems.Sort(Settings.SortKey, Settings.IsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryPlaylists(Settings)?
					.Apply();
		}

		private IPlaylistsSettings? _Settings;
		private PlaylistsRecyclerView? _Playlists;

		protected override string? QueueId
		{
			get => MenuOptionsUtils.QueueIdPlaylists(Settings);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(Playlists.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => Playlists.LibraryItemsAdapter;
		}

		protected bool CreatePlaylistResult { get; set; }
		protected AlertDialog? CreatePlaylistAlertDialog { get; set; }
		protected OptionsCreateAndViewView? CreatePlaylistView { get; set; }

		public IPlaylistsSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryPlaylists();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IPlaylistsSettings.Defaults.PlaylistsSettings;
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
		public PlaylistsRecyclerView Playlists
		{
			get => _Playlists ??= FindViewById(Ids.Playlists_RecyclerView) as PlaylistsRecyclerView ?? throw new InflateException();
		}

		public async void DisplayCreateDialog()
		{
			if (Library is null || Context is null)
				return;

			await OnRefresh();

			CreatePlaylistAlertDialog = XyzuUtils.Dialogs.Alert(Context, alertdialogbuilder =>
			{
				if (alertdialogbuilder is null)
					return;

				IEnumerable<string> playlistnames = Playlists.LibraryItemsAdapter.LibraryItems
					.Select(playlist => playlist.Name)
					.OfType<string>();

				alertdialogbuilder.SetTitle(Resource.String.create_playlist);
				alertdialogbuilder.SetOnDismissListener(new DialogInterfaceOnDismissListener
				{
					OnDismissAction = async dialoginterface =>
					{
						if (CreatePlaylistResult)
							await OnRefresh(true);

						CreatePlaylistResult = false;
						CreatePlaylistView = null;
						CreatePlaylistAlertDialog = null;
					}
				});
				alertdialogbuilder.SetNegativeButton(Resource.String.cancel, new DialogInterfaceOnClickListener((dialoginterface, which) => dialoginterface?.Dismiss()));
				alertdialogbuilder.SetView(CreatePlaylistView = new OptionsCreateAndViewView(Context!)
				{
					WithView = false,
					OnCreateTextChanged = MenuOptionsUtils.PlaylistOnCreateTextChanged(playlistnames),
					OnCreate = (createplaylistview, playlistname) =>
					{
						bool createresult = CreatePlaylistResult = MenuOptionsUtils.PlaylistOnCreate(createplaylistview, MenuVariables)
							.Invoke(playlistname);

						if (createresult)
						{
							CreatePlaylistAlertDialog?.Dismiss();
							XyzuUtils.Dialogs.SnackBar(Context, this, OnSnackbarCreated)
								.SetText(string.Format("{0} {1}", playlistname, Context.Resources?.GetString(Resource.String.created) ?? string.Empty))
								.Show();
						}

						return createresult;
					},
				});
			});

			CreatePlaylistAlertDialog.Show();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || (force is false && Playlists.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			IAsyncEnumerable<IPlaylist> playlists = Library.Playlists.GetPlaylists(
				identifiers: null,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GeneratePlaylistRetriever(
					playlists: Playlists.LibraryItemsAdapter.LibraryItems,
					modelsortkey: Settings.SortKey,
					librarylayouttype: Settings.LayoutType))
				.Sort(Settings.SortKey, Settings.IsReversed);

			Playlists.LibraryItemsAdapter.LibraryItems.Clear();

			await Playlists.LibraryItemsAdapter.LibraryItems.AddRange(playlists, Cancellationtoken);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && Playlists.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				Playlists.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.Playlist.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueuePlaylists(MenuVariables);
					return true;

				case Menus.LibraryItem.Playlist.Delete:
					MenuOptionsUtils.DeletePlaylists(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Playlist.EditInfo:
					MenuOptionsUtils.EditInfoPlaylist(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Playlist.GoToPlaylist:
					DismissMenuOptions();
					MenuOptionsUtils.GoToPlaylist(MenuVariables);
					return true;

				case Menus.LibraryItem.Playlist.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlayPlaylists(MenuVariables, null);
					return true;

				case Menus.LibraryItem.Playlist.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.Playlist.ViewInfo:
					MenuOptionsUtils.ViewInfoPlaylist(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			Playlists.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}
	}
}