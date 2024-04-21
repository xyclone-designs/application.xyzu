#nullable enable

using Android.App;
using Android.Content;
using Android.Views;
using AndroidX.AppCompat.Widget;

using Google.Android.Material.BottomSheet;

using System;
using System.Collections.Generic;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library.Models;
using Xyzu.Menus;
using Xyzu.Views.LibraryItem;
using Xyzu.Views.Option;
using Xyzu.Widgets.RecyclerViews;

using AndroidXAlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Xyzu.Views.Library
{
	public partial class LibraryView : PopupMenu.IOnMenuItemClickListener, IDialogInterfaceOnDismissListener, IDialogInterfaceOnShowListener
	{
		private MenuOptionsUtils.VariableContainer? _MenuVariables;
		private BottomSheetDialog? _MenuOptionsDialog;
		private OptionsMenuView? _MenuOptionsView;

		protected const int MenuOptionEnabledWhenNone = 0;
		protected const int MenuOptionEnabledWhenSingle = 1;
		protected const int MenuOptionEnabledWhenMultiple = 2;

		protected static int[] MenuOptionEnabledArray_None = new int[] { MenuOptionEnabledWhenNone };
		protected static int[] MenuOptionEnabledArray_Single = new int[] { MenuOptionEnabledWhenSingle };
		protected static int[] MenuOptionEnabledArray_Multiple = new int[] { MenuOptionEnabledWhenMultiple };
		protected static int[] MenuOptionEnabledArray_SingleMultiple = new int[] { MenuOptionEnabledWhenSingle, MenuOptionEnabledWhenMultiple };
		protected static int[] MenuOptionEnabledArray_All = new int[] { MenuOptionEnabledWhenNone, MenuOptionEnabledWhenSingle, MenuOptionEnabledWhenMultiple };

		protected BottomSheetDialog MenuOptionsDialog
		{
			set => _MenuOptionsDialog = value;
			get	=> _MenuOptionsDialog ??= XyzuUtils.Dialogs.BottomSheet(Context!, _menuoptionsdialog =>
			{
				_menuoptionsdialog.SetCancelable(true);
				_menuoptionsdialog.SetCanceledOnTouchOutside(false);
				_menuoptionsdialog.SetContentView(MenuOptionsView);
				_menuoptionsdialog.SetDim(false);
				_menuoptionsdialog.SetOnShowListener(this);
				_menuoptionsdialog.SetOnDismissListener(this);
			});
		}
		protected OptionsMenuView MenuOptionsView
		{
			set => _MenuOptionsView = value;
			get => _MenuOptionsView ??= new OptionsMenuView(Context!)
			{
				Background = Context?.Resources?.GetDrawable(Resource.Drawable.shape_cornered_top, Context.Theme),

				MenuOptions = MenuOptionsViewOptions?.Keys ?? Enumerable.Empty<MenuOptions>(),
				OnAllClicked = OnMenuOptionsAllClick,
				OnCancelClicked = OnMenuOptionsCancelClick,
				OnMenuOptionClicked = OnMenuOptionClick,
			};
		}
		protected MenuOptionsUtils.VariableContainer MenuVariables
		{
			set => _MenuVariables = value;
			get => _MenuVariables ??= new MenuOptionsUtils.VariableContainer
			{
				Activity = LibraryFragment?.Activity,
				AnchorView = this,
				AnchorViewGroup = this,
				Context = Context,
				LibraryNavigatable = Navigatable,
				SnackbarOnCreate = OnSnackbarCreated,
				SnackbarParent = this,
			};
		}
		protected PopupMenuDismissListener? PopupmenuDismissListener
		{
			get; set;
		}

		protected virtual IDictionary<MenuOptions, int[]?>? MenuOptionsViewOptions { get; }

		protected virtual void ProcessMenuOptions(LibraryItemsRecyclerView.Adapter? adapter)
		{
			if (adapter is null)
			{
				_MenuOptionsDialog?.Dismiss();

				return;
			}

			if (_MenuOptionsView is null)
				MenuOptionsView.MenuOptionsRecyclerView.SimpleAdapter.ViewHolderOnBind = (viewholder, position) =>
				{
					MenuOptionsView.MenuOptionsViewHolderOnBind(viewholder, position);

					if (MenuOptionsViewOptions is null)
						return;

					OptionsMenuView.ViewHolder optionsmenuviewholder = (OptionsMenuView.ViewHolder)viewholder;

					viewholder.ItemView.Enabled = adapter?.SelectedPositions.Count() switch
					{
						0 => MenuOptionsViewOptions[optionsmenuviewholder.MenuOption]?.Contains(MenuOptionEnabledWhenNone),
						1 => MenuOptionsViewOptions[optionsmenuviewholder.MenuOption]?.Contains(MenuOptionEnabledWhenSingle),

						_ => MenuOptionsViewOptions[optionsmenuviewholder.MenuOption]?.Contains(MenuOptionEnabledWhenMultiple),

					} ?? true;
				};

			switch (adapter.SelectedPositions.Count())
			{
				case 0:
				case 1:
				case 2:
					MenuOptionsView.MenuOptionsRecyclerView.SimpleAdapter.NotifyDataSetChanged();
					break;

				default: break;
			}

			MenuOptionsView.All.Checked = adapter.ItemCount == adapter.SelectedPositions.Count();
			MenuOptionsView.Text.Text = adapter.SelectedPositions.Count() switch
			{
				0 => string.Format("0/{0} {1}", adapter.ItemCount, Context?.Resources?.GetString(Resource.String.selected) ?? string.Empty),

				1 when
				adapter is LibraryItemsRecyclerView.Adapter<IAlbum> albumadapter &&
				albumadapter.SelectedLibraryItems.FirstOrDefault() is IAlbum selectedalbum &&
				LibraryItemView.Defaults.Album(Context) is IAlbum defaultalbum
					=> string.Format("{0} - {1}", selectedalbum.Title ?? defaultalbum.Title ?? string.Empty, selectedalbum.Artist ?? defaultalbum.Artist ?? string.Empty),

				1 when
				adapter is LibraryItemsRecyclerView.Adapter<IArtist> artistadapter &&
				artistadapter.SelectedLibraryItems.FirstOrDefault() is IArtist selectedartist &&
				LibraryItemView.Defaults.Artist(Context) is IArtist defaultartist
					=> selectedartist.Name ?? defaultartist.Name ?? string.Empty,

				1 when
				adapter is LibraryItemsRecyclerView.Adapter<IGenre> genreadapter &&
				genreadapter.SelectedLibraryItems.FirstOrDefault() is IGenre selectedgenre &&
				LibraryItemView.Defaults.Genre(Context) is IGenre defaultgenre
					=> selectedgenre.Name ?? defaultgenre.Name ?? string.Empty,

				1 when
				adapter is LibraryItemsRecyclerView.Adapter<IPlaylist> playlistadapter &&
				playlistadapter.SelectedLibraryItems.FirstOrDefault() is IPlaylist selectedplaylist &&
				LibraryItemView.Defaults.Playlist(Context) is IPlaylist defaultplaylist
					=> selectedplaylist.Name ?? defaultplaylist.Name ?? string.Empty,

				1 when
				adapter is LibraryItemsRecyclerView.Adapter<ISong> songadapter &&
				songadapter.SelectedLibraryItems.FirstOrDefault() is ISong selectedsong &&
				LibraryItemView.Defaults.Song(Context) is ISong defaultsong
					=> string.Format("{0} - {1}", selectedsong.Title ?? defaultsong.Title ?? string.Empty, selectedsong.Artist ?? defaultsong.Artist ?? string.Empty),

				_ => string.Format("{0}/{1} {2}", adapter.SelectedPositions.Count(), adapter.ItemCount, Context?.Resources?.GetString(Resource.String.selected) ?? string.Empty)
			};
		}
		protected virtual void ProcessMenuVariables(MenuOptionsUtils.VariableContainer menuvariables, MenuOptions? menuoption)
		{
			menuvariables.AnchorView ??= this;
			menuvariables.Activity ??= LibraryFragment?.Activity;
			menuvariables.Context ??= Context;
			menuvariables.AnchorViewGroup ??= this;
			menuvariables.LibraryNavigatable ??= Navigatable;

			menuvariables.DialogInterfaceListenerCancel = dialoginterface =>
			{
				dialoginterface?.Dismiss();
			};							   
			menuvariables.DialogInterfaceListenerClose = dialoginterface =>
			{
				dialoginterface?.Dismiss();
			};
			menuvariables.DialogInterfaceListenerEdit = dialoginterface =>
			{
				dialoginterface?.Dismiss();
				OnMenuOptionClick(MenuOptions.EditInfo);
			};
			menuvariables.DialogInterfaceListenerLyrics = dialoginterface =>
			{
				dialoginterface?.Dismiss();
				menuvariables.DialogInterfaceListenerEdit = dialoginterface =>
				{
					dialoginterface?.Dismiss();
					MenuOptionsUtils.EditInfoSongLyrics(menuvariables)?
						.Show();
				};
				MenuOptionsUtils.ViewInfoSongLyrics(menuvariables)?
					.Show();
			};
		}

		protected virtual AndroidXAlertDialog CreateOptionsMenuAlertDialog(Action<OptionsMenuView, AndroidXAlertDialog>? buildaction)
		{
			OptionsMenuView menuoptionsview = new OptionsMenuView(Context!)
			{
				WithHeaderButtons = false,
			};
			AndroidXAlertDialog alertdialog = XyzuUtils.Dialogs.Alert(Context!, alertdialogbuilderaction: null);

			alertdialog.SetCancelable(true);
			alertdialog.SetCanceledOnTouchOutside(true);
			alertdialog.SetView(menuoptionsview);
			alertdialog.SetDim(false);
			alertdialog.SetOnShowListener(this);
			alertdialog.SetOnDismissListener(this);

			buildaction?.Invoke(menuoptionsview, alertdialog);

			return alertdialog;
		}
		protected virtual IDialogInterfaceOnDismissListener? DefaultDialogInterfaceListenerOnDismiss(LibraryItemsRecyclerView.Adapter? adapter)
		{
			return new DialogInterfaceOnDismissListener(dialoginterface =>
			{
				if (adapter?.FocusedViewHolder != null)
					adapter.FocusedViewHolder.ItemView.Selected = false;
			});
		}

		public virtual void DismissMenuOptions()
		{
			RemoveInsets(nameof(MenuOptionsView));

			MenuOptionsDialog.Dismiss();
		}

		public virtual void OnShow(IDialogInterface? dialog)
		{
			MenuOptionsDialog?.SetAllowTouchOutside(true, motionevent =>
			{
				return LibraryFragment?.Activity?.DispatchTouchEvent(motionevent) ?? false;
			});

			AddInsets(nameof(MenuOptionsView), null, null, null, MenuOptionsView?.Height);
		}
		public virtual void OnDismiss(IDialogInterface? dialog)
		{
			RemoveInsets(nameof(MenuOptionsView));

			_MenuOptionsDialog = null;
			_MenuOptionsView = null;
		}

		public virtual bool OnMenuItemClick(IMenuItem? item)
		{
			if ((MenuOptions?)item?.ItemId is MenuOptions menuoption)
				return OnMenuOptionClick(menuoption);

			return false;
		}
		public virtual bool OnMenuOptionClick(MenuOptions menuoption)
		{
			ProcessMenuVariables(MenuVariables, menuoption);

			return false;
		}
		public virtual void OnMenuOptionsAllClick(object sender, EventArgs args)
		{ }
		public virtual void OnMenuOptionsCancelClick(object sender, EventArgs args)
		{
			MenuOptionsDialog?.Dismiss();
		}
	}
}