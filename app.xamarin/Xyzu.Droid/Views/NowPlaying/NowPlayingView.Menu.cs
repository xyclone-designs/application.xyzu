using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

using System;
using System.Linq;

using Xyzu.Droid;
using Xyzu.Library.Models;
using Xyzu.Menus;
using Xyzu.Player.Enums;
using Xyzu.Views.Option;

using AndroidXAlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Xyzu.Views.NowPlaying
{
	public partial class NowPlayingView 
	{
		protected OptionsMenuView? _MenuOptionsView;
		protected OptionsMenuView? _MenuPlayerSettingsView;

		public OptionsMenuView? MenuOptionsView 
		{
			get
			{
				if (_MenuOptionsView != null || Context is null)
					return _MenuOptionsView;

				_MenuOptionsView = new OptionsMenuView(Context)
				{
					WithHeaderButtons = false,
					MenuOptions = Menus.NowPlaying.AsEnumerable(),
					OnMenuOptionClicked = OnMenuOptionClick
				};

				if (ArtworkPalette?.DominantSwatch != null)
					_MenuOptionsView.Text.SetTextColor(new Color(ArtworkPalette.DominantSwatch.Rgb));

				_MenuOptionsView.Text.Text = MenuOptions.Options.AsStringTitle(Context);
				_MenuOptionsView.MenuOptionsRecyclerView.SimpleAdapter.ViewHolderOnBind = (recyclerviewviewholderdefault, position) =>
				{
					_MenuOptionsView.MenuOptionsViewHolderOnBind(recyclerviewviewholderdefault, position);

					ConfigureMenuOptionButton(recyclerviewviewholderdefault.ItemView);
				};

				return _MenuOptionsView;
			}
		}
		public OptionsMenuView? MenuPlayerSettingsView
		{
			get
			{
				if (_MenuPlayerSettingsView != null || Context is null)
					return _MenuPlayerSettingsView;

				_MenuPlayerSettingsView = new OptionsMenuView(Context)
				{
					WithHeaderButtons = false,
					MenuOptions = new MenuOptions[] { (MenuOptions)0, (MenuOptions)1, },
					OnMenuOptionClicked = menuoption =>
					{
						switch ((int)menuoption)
						{
							case 0 when Player != null:
								Player.Repeat = Player.Repeat.Next();
								_MenuPlayerSettingsView?.MenuOptionsRecyclerView.SimpleAdapter.NotifyItemChanged(0);
								return true;

							case 1 when Player != null:
								Player.Shuffle = Player.Shuffle.Next();
								_MenuPlayerSettingsView?.MenuOptionsRecyclerView.SimpleAdapter.NotifyItemChanged(1);
								return true;						  

							default: throw new ArgumentException("Invalid MenuOption");
						}
					}
				};

				if (ArtworkPalette?.DominantSwatch != null)
					_MenuPlayerSettingsView.Text.SetTextColor(new Color(ArtworkPalette.DominantSwatch.Rgb));

				_MenuPlayerSettingsView.Text.SetText(Resource.String.settings_userinterface_nowplaying_title);
				_MenuPlayerSettingsView.MenuOptionsRecyclerView.SimpleLayoutManager.SpanCount = 1;
				_MenuPlayerSettingsView.MenuOptionsRecyclerView.SimpleAdapter.ViewHolderOnBind = (recyclerviewviewholderdefault, position) =>
				{
					OptionsMenuView.ViewHolder viewholder = (OptionsMenuView.ViewHolder)recyclerviewviewholderdefault;

					viewholder.MenuOption = _MenuPlayerSettingsView.MenuOptions.ElementAt(position);

					(int text, Drawable? drawable) = (position switch
					{
						0 => ((Player?.Repeat ?? default).AsResoureIdTitle(), (Player?.Repeat ?? default).AsDrawable(Context)),
						1 => ((Player?.Shuffle ?? default).AsResoureIdTitle(), (Player?.Shuffle ?? default).AsDrawable(Context)),

						_ => throw new ArgumentException("Invalid MenuOption")
					});

					viewholder.ItemView.SetText(text, null);
					viewholder.ItemView.SetCompoundDrawablesRelativeWithIntrinsicBounds(null, drawable, null, null);

					ConfigureMenuOptionButton(viewholder.ItemView);
				};

				return _MenuPlayerSettingsView;
			}
		}
		public AndroidXAlertDialog? MenuDialog 
		{ 
			get; set;
		}
		public MenuOptionsUtils.VariableContainer? MenuVariables
		{
			get; set;
		}

		protected void ShowDialog(View view, Action<AndroidXAlertDialog>? builddialogaction = null)
		{
			if (Context is null)
				return;

			MenuDialog = XyzuUtils.Dialogs.Alert(Context, (alertdialogbuilder, alertdialog) => 
			{
				if (alertdialog == null)
					return;

				alertdialog.Window?.SetGravity(GravityFlags.Bottom | GravityFlags.End);
				alertdialog.SetView(view);
				alertdialog.SetCancelable(true);
				alertdialog.SetCanceledOnTouchOutside(true);
			});
			
			builddialogaction?.Invoke(MenuDialog);

			MenuDialog.Show();
		}

		public virtual bool OnMenuItemClick(IMenuItem item)
		{
			return false;
		}
		public virtual bool OnMenuOptionClick(MenuOptions menuoption)
		{
			MenuVariables ??= new MenuOptionsUtils.VariableContainer();
			MenuVariables.AnchorView ??= this;
			MenuVariables.AnchorViewGroup ??= this;
			MenuVariables.Context ??= Context;
			MenuVariables.Songs = Enumerable.Empty<ISong?>()
				.Append(Player?.Queue.CurrentSong)
				.OfType<ISong>();

			switch (menuoption)
			{
				case Menus.NowPlaying.Delete:
					MenuOptionsUtils.DeleteSongs(MenuVariables)?
						.Show();
					return true;

				case Menus.NowPlaying.EditInfo:
					MenuOptionsUtils.EditInfoSong(MenuVariables)?
						.Show();
					return true;

				case Menus.NowPlaying.GoToAlbum:
					MenuDialog?.Dismiss();
					MenuOptionsUtils.GoToAlbum(MenuVariables);
					return true;

				case Menus.NowPlaying.GoToArtist:
					MenuDialog?.Dismiss();
					MenuOptionsUtils.GoToArtist(MenuVariables);
					return true;

				case Menus.NowPlaying.GoToGenre:
					MenuDialog?.Dismiss();
					MenuOptionsUtils.GoToGenre(MenuVariables);
					return true;					

				case Menus.NowPlaying.GoToQueue:
					MenuDialog?.Dismiss();
					MenuOptionsUtils.GoToQueue(MenuVariables);
					return true;

				case Menus.NowPlaying.Settings when Context != null:
					MenuDialog?.Dismiss();
					Intent settingsintent = new Intent(Context, typeof(Activities.SettingsActivity))
						.PutExtra(
							name: Activities.SettingsActivity.Intents.ExtraKeys.FragmentName,
							value: Fragments.Settings.UserInterface.NowPlayingPreferenceFragment.FragmentName);

					Context.StartActivity(settingsintent);
					return true;

				case Menus.NowPlaying.Share:
					MenuOptionsUtils.Share(MenuVariables);
					return true;

				case Menus.NowPlaying.ViewInfo:
					MenuOptionsUtils.ViewInfoSong(MenuVariables)?
						.Show();
					return true;

				default: break;
			}

			return false;
		}
	}
}