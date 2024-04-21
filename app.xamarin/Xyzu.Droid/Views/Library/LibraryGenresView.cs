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
	public class LibraryGenresView : LibraryView, ILibraryGenres
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_library_genres;

			public const int Genres_RecyclerView = Resource.Id.xyzu_view_library_genres_genres_genresrecyclerview;
		}

		public LibraryGenresView(Context context) : base(context) { }
		public LibraryGenresView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		protected override IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions
		{
			get => Menus.LibraryItem.Genre
				.AsEnumerable()
				.ToDictionary<MenuOptions, MenuOptions, int[]?>(
					keySelector: menuoption => menuoption,
					elementSelector: menuoption => menuoption switch
					{
						Menus.LibraryItem.Genre.EditInfo => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Genre.GoToGenre => MenuOptionEnabledArray_Single,
						Menus.LibraryItem.Genre.ViewInfo => MenuOptionEnabledArray_Single,

						Menus.LibraryItem.Genre.AddToPlaylist => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Genre.AddToQueue => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Genre.Delete => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Genre.Play => MenuOptionEnabledArray_SingleMultiple,
						Menus.LibraryItem.Genre.Share => MenuOptionEnabledArray_SingleMultiple,

						_ => MenuOptionEnabledArray_All,
					});
		}

		protected override void Configure()
		{
			base.Configure();

			Genres.LibraryItemsLayoutManager.LibraryLayoutType = Settings.LayoutType;

			Genres.LibraryItemsAdapter.Images = Images;
			Genres.LibraryItemsAdapter.Library = Library;
			Genres.LibraryItemsAdapter.LibraryLayoutType = Settings.LayoutType;
			Genres.LibraryItemsAdapter.PropertyChangedAction = PropertyChangedLibraryItemsAdapter;
			Genres.LibraryItemsAdapter.ViewHolderOnBind = (viewholder, position) =>
			{
				viewholder.LibraryItem.SetGenre(Genres.LibraryItemsAdapter.LibraryItems[position], LibraryItemView.Defaults.Genre(Context));
				viewholder.LibraryItem.SetPlaying(Player, QueueId);
			};
			Genres.LibraryItemsAdapter.ViewHolderOnEvent = viewholdereventargs =>
			{
				switch (viewholdereventargs.Gesture, Genres.LibraryItemsAdapter.RecyclerViewAdapterState)
				{
					case (Gestures.Click, RecyclerViewAdapterStates.Normal):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Normal):
						Navigatable?.NavigateGenre(Genres.LibraryItemsAdapter.FocusedLibraryItem);
						break;

					case (Gestures.Click, RecyclerViewAdapterStates.Select):
					case (Gestures.SingleTapConfirmed, RecyclerViewAdapterStates.Select):
						Genres.LibraryItemsAdapter.SelectLibraryItemsFocused();
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Select) when
					Genres.LibraryItemsAdapter.FocusedViewHolder?.LayoutPosition is int position:
						Genres.LibraryItemsAdapter.SelectLibraryItems(position);
						break;

					case (Gestures.LongPress, RecyclerViewAdapterStates.Normal):
						Genres.LibraryItemsAdapter.SelectLibraryItemsFocused();
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

			menuvariables.DialogInterfaceListenerOnDismiss ??= DefaultDialogInterfaceListenerOnDismiss(Genres.LibraryItemsAdapter);

			switch (Genres.LibraryItemsAdapter.RecyclerViewAdapterState)
			{
				case RecyclerViewAdapterStates.Select:
					MenuVariables.Index = 0;
					MenuVariables.QueueId = null;
					MenuVariables.Genres = Genres.LibraryItemsAdapter.SelectedLibraryItems;
					break;

				default:
					MenuVariables.Index = Genres.LibraryItemsAdapter.LibraryItems.Index(Genres.LibraryItemsAdapter.FocusedLibraryItem);
					MenuVariables.QueueId = QueueId;
					MenuVariables.Genres = Genres.LibraryItemsAdapter.LibraryItems;
					break;
			}
		}
		protected override void PropertyChangedSettings(object sender, PropertyChangedEventArgs args)
		{
			base.PropertyChangedSettings(sender, args);

			switch (args.PropertyName)
			{
				case nameof(IGenresSettings.LayoutType):
					Genres.ReloadLayout(Settings.LayoutType);
					break;
				case nameof(IGenresSettings.IsReversed):
					Genres.LibraryItemsAdapter.LibraryItems.Reverse();
					break;
				case nameof(IGenresSettings.SortKey):
					Genres.LibraryItemsAdapter.SetLibraryItems(Genres.LibraryItemsAdapter.LibraryItems.Sort(Settings.SortKey, Settings.IsReversed));
					break;

				default: break;
			}

			if (Settings != null)
				SharedPreferences?
					.Edit()?
					.PutUserInterfaceLibraryGenres(Settings)?
					.Apply();
		}

		private IGenresSettings? _Settings;
		private GenresRecyclerView? _Genres;

		protected override string? QueueId
		{
			get => MenuOptionsUtils.QueueIdGenres(Settings);
		}
		protected override LibraryItemInsetView? InsetBottomView
		{
			get => (LibraryItemInsetView)(Genres.LibraryItemsAdapter.Footer ??= new LibraryItemInsetView(Context!));
		}
		protected override LibraryItemsRecyclerView.Adapter? LibraryAdpter
		{
			get => Genres.LibraryItemsAdapter;
		}

		public IGenresSettings Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceLibraryGenres();

					if (_Settings != null)
						_Settings.PropertyChanged += PropertyChangedSettings;
				}

				return _Settings ?? IGenresSettings.Defaults.GenresSettings;
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
		public GenresRecyclerView Genres
		{
			get => _Genres ??= FindViewById(Ids.Genres_RecyclerView) as GenresRecyclerView ?? throw new InflateException();
		}

		public override async Task OnRefresh(bool force = false)
		{
			if (Library is null || (force is false && Genres.LibraryItemsAdapter.LibraryItems.Any()))
			{
				Refreshing = false;

				return;
			}

			Refreshing = true;

			IAsyncEnumerable<IGenre> genres = Library.Genres.GetGenres(
				identifiers: null,
				cancellationToken: Cancellationtoken,
				retriever: LibraryItemView.Retrievers.GenerateGenreRetriever(
					genres: Genres.LibraryItemsAdapter.LibraryItems,
					modelsortkey: Settings.SortKey,
					librarylayouttype: Settings.LayoutType))
				.Sort(Settings.SortKey, Settings.IsReversed);

			Genres.LibraryItemsAdapter.LibraryItems.Clear();

			await Genres.LibraryItemsAdapter.LibraryItems.AddRange(genres, Cancellationtoken);

			Refreshing = false;
		}
		public override void OnDismiss(IDialogInterface? dialog)
		{
			if (dialog == MenuOptionsDialog && Genres.LibraryItemsAdapter.RecyclerViewAdapterState == RecyclerViewAdapterStates.Select)
				Genres.LibraryItemsAdapter.RecyclerViewAdapterState = RecyclerViewAdapterStates.Normal;

			base.OnDismiss(dialog);
		}
		public override bool OnMenuOptionClick(MenuOptions menuoption)
		{
			base.OnMenuOptionClick(menuoption);

			PopupmenuDismissListener?.SetMenuOptionClicked(true);

			switch (menuoption)
			{
				case Menus.LibraryItem.Genre.AddToPlaylist:
					MenuOptionsUtils.AddToPlaylistGenres(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Genre.AddToQueue:
					DismissMenuOptions();
					MenuOptionsUtils.AddToQueueGenres(MenuVariables);
					return true;

				case Menus.LibraryItem.Genre.Delete:
					MenuOptionsUtils.DeleteGenres(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Genre.EditInfo:
					MenuOptionsUtils.EditInfoGenre(MenuVariables)?
						.Show();
					return true;

				case Menus.LibraryItem.Genre.GoToGenre:
					DismissMenuOptions();
					MenuOptionsUtils.GoToGenre(MenuVariables);
					return true;

				case Menus.LibraryItem.Genre.Play:
					DismissMenuOptions();
					MenuOptionsUtils.PlayGenres(MenuVariables, null);
					return true;

				case Menus.LibraryItem.Genre.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.LibraryItem.Genre.ViewInfo:
					MenuOptionsUtils.ViewInfoGenre(MenuVariables)?
						.Show();
					return true;

				default: return false;
			}
		}
		public override void OnMenuOptionsAllClick(object sender, EventArgs args)
		{
			base.OnMenuOptionsAllClick(sender, args);

			Genres.LibraryItemsAdapter.SelectLibraryItemsAll(false);
		}
	}
}