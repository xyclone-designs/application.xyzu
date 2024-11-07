using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;

using Oze.Music.MusicBarLib;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Images;
using Xyzu.Library;
using Xyzu.Library.Models;
using Xyzu.Player;
using Xyzu.Player.Enums;
using Xyzu.Settings.UserInterface;

namespace Xyzu.Views.NowPlaying
{
	public partial class NowPlayingView : INowPlayingDroid, MusicBar.IOnMusicBarProgressChangeListener
	{
		INowPlayingSettings INowPlaying.Settings 
		{ 
			get => throw new NotImplementedException(); 
			set => throw new NotImplementedException(); 
		}

		protected int PositionTimerDue = 0;
		protected int PositionTimerPeriodStop = -1;
		protected int PositionTimerPeriodStart = 1_000;

		protected IPlayer? _Player;
		protected INowPlayingSettingsDroid? _Settings;
		protected Timer? _PositionTimer;
		protected ISong? _SongPrevious;
		protected ISong? _SongCurrent;
		protected ISong? _SongNext;

		protected Timer PositionTimer
		{
			set => _PositionTimer = value;
			get => _PositionTimer ??= new Timer(state => SetPosition(null, null), null, PositionTimerDue, PositionTimerPeriodStop);
		}

		public IPlayer? Player
		{
			get => _Player;
			set
			{
				if (_Player != null)
				{
					_Player.OnPlayerError -= PlayerOnPlayerError;
					_Player.OnPlayerOperation -= PlayerOnPlayerOperation;
					_Player.OnPropertyChanged -= PlayerPropertyChanged;
					_Player.Queue.PropertyChanged -= PlayerQueuePropertyChanged;
				}

				_Player = value;

				if (_Player != null)
				{
					_Player.OnPlayerError += PlayerOnPlayerError;
					_Player.OnPlayerOperation += PlayerOnPlayerOperation;
					_Player.OnPropertyChanged += PlayerPropertyChanged;
					_Player.Queue.PropertyChanged += PlayerQueuePropertyChanged;
				}

				OnPropertyChanged();
			}
		}
		public INowPlayingSettingsDroid Settings
		{
			get
			{
				if (_Settings is null)
				{
					_Settings = SharedPreferences?.GetUserInterfaceNowPlayingDroid();

					if (_Settings != null)
						_Settings.PropertyChanged += SettingsPropertyChanged;
				}

				return _Settings ?? INowPlayingSettingsDroid.Defaults.NowPlayingSettingsDroid;
			}
			set
			{
				if (_Settings != null)
					_Settings.PropertyChanged -= SettingsPropertyChanged;

				_Settings = value;

				if (_Settings != null)
					_Settings.PropertyChanged += SettingsPropertyChanged;

				OnPropertyChanged();
			}
		}
		public ILibrary? Library { get; set; }
		public IImages? Images { get; set; }
		public IGestureDetector? GestureDetector { get; set; }
		public ISharedPreferences? SharedPreferences { get; set; }
		public FragmentActivity? FragmentActivity { get; set; }

		protected virtual void SettingsPropertyChanged(object? sender, PropertyChangedEventArgs args) 
		{ }
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null) 
		{
			switch (propertyname)
			{
				case nameof(Player):
					PlayerPropertyChanged(this, new PropertyChangedEventArgs(nameof(IPlayer.State)));
					PlayerQueuePropertyChanged(this, new PropertyChangedEventArgs(nameof(IQueue.CurrentIndex)));
					break;

				default: break;
			}
		}

		public void ViewRefresh()
		{
			if (Player?.Queue.CurrentIndex is not int queueindex || Artwork.SimpleLayoutManager.GetPosition(ArtworkSnapHelper) is not int viewindex)
			{
				SetBlur(null);
				SetPosition(null);
				SetText(null, null, null);

				return;
			}

			switch (true)
			{
				case true when queueindex == viewindex - 1 || queueindex == viewindex + 1:
					Artwork.SmoothScrollToPosition(queueindex);
					break;

				default:
					Artwork.ScrollToPosition(queueindex);
					Artwork.SimpleAdapter.NotifyItemChanged(queueindex);
					break;
			}

			ArtworkPalette = _ArtworkPalette;

			SetBlur(Player.Queue.CurrentSong);
			SetPosition(Player.Queue.CurrentSong);
			SetText(Player.Queue.CurrentSong, null, null);
			
			if ((Player.Queue.CurrentSong?.Malformed ?? false) && Context is not null)
				PlayerOnPlayerError(this, new IPlayer.PlayerErrorsEventArgs(PlayerErrors.Load));
		}
		public void PlayerPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(IPlayer.State):
					switch (Player?.State)
					{
						case (PlayerStates.Loading):
							PositionTimer?.Change(PositionTimerDue, PositionTimerPeriodStop);
							break;
						
						case (PlayerStates.NoSong):
						case (PlayerStates.Paused):
							PositionTimer?.Change(PositionTimerDue, PositionTimerPeriodStop);
							Buttons_Player_PlayPause.SetImageResource(Resource.Drawable.icon_player_play);
							break;
						
						case (PlayerStates.Playing):
							PositionTimer?.Change(PositionTimerDue, PositionTimerPeriodStart);
							Buttons_Player_PlayPause.SetImageResource(Resource.Drawable.icon_player_pause);
							break;

						default:
							PositionTimer?.Change(PositionTimerDue, PositionTimerPeriodStart);
							Buttons_Player_PlayPause.SetImageResource(Resource.Drawable.icon_player_play);
							break;

						case (PlayerStates.Stopped):
						case (PlayerStates.Uninitialised):

							_SongNext = null;
							_SongCurrent = null;
							_SongPrevious = null;

							PositionTimer?.Change(PositionTimerDue, PositionTimerPeriodStop);
							Buttons_Player_PlayPause.SetImageResource(Resource.Drawable.icon_player_play);

							ViewRefresh();
							break;
					}
					break;

				default: break;
			}
		}
		public void PlayerOnPlayerError(object? sender, IPlayer.PlayerErrorsEventArgs args)
		{
			if (Context is null)
				return;

			string? filepath = args.QueueIndex.HasValue
				? Player?.Queue.ElementAtOrDefault(args.QueueIndex.Value)?.Uri?.ToString().Split('/').Last()
				: null;

			XyzuUtils.Dialogs
				.SnackBar(Context, Artwork, snackbar =>
				{
					if (filepath is null)
						snackbar.SetText(args.PlayerError.AsResoureIdDescription());
					else snackbar.SetText(string.Format(
						"{0} '.../{1}'",
						args.PlayerError.AsStringDescription(Context), filepath));

				}).Show();
		}
		public void PlayerOnPlayerOperation(object? sender, IPlayer.PlayerOperationsEventArgs args)
		{ }
		public void PlayerQueuePropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(IQueue.CurrentIndex):
					ViewRefresh();
					break;

				default: break;
			}
		}

		public async void SetBlur(ISong? song)
		{
			void OnComplete(bool completed)
			{
				if (completed is false)
				{
					BackgroundBlur.Background = null;
					BackgroundBlur.SetBackgroundResource(Resource.Drawable.xyzu_view_nowplaying_background);
				}

				if (ValueAnimator.OfFloat(BackgroundBlur.Alpha, 1) is ValueAnimator valueanimator)
				{
					valueanimator.SetDuration(300);
					valueanimator.AddUpdateListener(new AnimatorUpdateListener
					{
						OnAnimationUpdateAction = valueanimator =>
						{
							if (valueanimator?.AnimatedValue is Java.Lang.Float value)
								BackgroundBlur.Alpha = (float)value;
						},
					});

					valueanimator.Start();
				}
				else BackgroundBlur.Alpha = 1;
			}

			if (song is null)
				OnComplete(false);
			else if (Images is not null)
				await Images.SetToViewBackground(new IImagesDroid.Parameters
				{
					View = BackgroundBlur,
					OnComplete = OnComplete,
					Operations = IImages.DefaultOperations.Blur,
					Sources = song != Player?.Queue.CurrentSong
						? new object?[] { song }
						: new object?[] { Player.Queue.CurrentSong },
				});
		}

		public void SetText(ISong? song, string? detailone, string? detailtwo)
		{
			detailone ??= song is null
				? Context?.Resources?.GetString(Resource.String.player_default_detail_one)
				: song.Title ?? Context?.Resources?.GetString(Resource.String.library_unknown_title);

			detailtwo ??= song is null
				? Context?.Resources?.GetString(Resource.String.player_default_detail_two)
				: string.Format(
					"{0} - {1}",
				   song.Artist ?? Context?.Resources?.GetString(Resource.String.library_unknown_artist),
				   song.Album ?? Context?.Resources?.GetString(Resource.String.library_unknown_album));

			Detail_One.SetText(detailone, null);
			Detail_Two.SetText(detailtwo, null);
		}
		public void SetPosition(ISong? song)
		{
			if (song?.Filepath is null || song.Malformed || song.Duration is null)
				Position.Hide();
			else
			{
				Position.Show();
				Task.Run(() => Position.LoadFrom(song.Filepath, (int)song.Duration.Value.TotalMilliseconds));
			}
			
			SetPosition(TimeSpan.Zero, song?.Duration ?? TimeSpan.Zero, true);
		}
		public void SetPosition(TimeSpan? current, TimeSpan? duration, bool withprogress = true)
		{
			current ??= TimeSpan.FromMilliseconds(Player?.Position ?? 0);
			duration ??= (Player?.Queue.CurrentSong) is ISong song 
				? song.Malformed  
					? TimeSpan.FromSeconds(3) 
					: song.Duration ?? TimeSpan.Zero
				: TimeSpan.Zero;

			if (current < TimeSpan.Zero) current = TimeSpan.Zero;
			if (duration < TimeSpan.Zero) duration = TimeSpan.Zero;

			string text = string.Format(
				"{0} | {1}",
				current.Value.ToMicrowaveFormat(false, duration.Value.TotalHours >= 1),
				duration.Value.ToMicrowaveFormat());

			FragmentActivity?.RunOnUiThread(() => PositionText?.SetText(text, null));

			if (withprogress)
				Position?.SetProgress((int)current.Value.TotalMilliseconds);
		}

		private void ConfigureMenuOptionButton(View? view)
		{
			if (view?.Background is not StateListDrawable statelistdrawable || (
				ArtworkPalette?.DominantSwatch?.Rgb ?? Context?.Resources?.GetColor(Resource.Color.ColorPrimary, Context.Theme)) is not int color)
				return;

			int[] colors = new int[] { color, color, color, };
			int state_active = statelistdrawable.FindStateDrawableIndex(new int[] { Android.Resource.Attribute.StateActive });
			int state_pressed = statelistdrawable.FindStateDrawableIndex(new int[] { Android.Resource.Attribute.StatePressed });

			if (state_active != -1 && statelistdrawable.GetStateDrawable(state_active) is GradientDrawable state_active_gradientdrawable)
				state_active_gradientdrawable.SetColors(colors);

			if (state_pressed != -1 && statelistdrawable.GetStateDrawable(state_pressed) is RippleDrawable state_active_rippledrawable)
			{
				state_active_rippledrawable.SetColor(new Android.Content.Res.ColorStateList(new int[][] { Array.Empty<int>() }, new int[] { color }));

				if (state_active_rippledrawable.FindDrawableByLayerId(Android.Resource.Id.Mask) is GradientDrawable state_active_max_gradientdrawable)
					state_active_max_gradientdrawable.SetColors(colors);
			}
		}
		private void ConfigureNowPlayingButton(View? view)
		{
			if (view?.Background is not StateListDrawable statelistdrawable || (
				ArtworkPalette?.DominantSwatch?.Rgb ?? Context?.Resources?.GetColor(Resource.Color.ColorPrimary, Context.Theme)) is not int color)
				return;

			int[] colors = new int[] { color, Color.Transparent.ToArgb(), Color.Transparent.ToArgb() };
			int state_active = statelistdrawable.FindStateDrawableIndex(new int[] { Android.Resource.Attribute.StateActive });
			int state_pressed = statelistdrawable.FindStateDrawableIndex(new int[] { Android.Resource.Attribute.StatePressed });

			if (state_active != -1 && statelistdrawable.GetStateDrawable(state_active) is GradientDrawable state_active_gradientdrawable)
				state_active_gradientdrawable.SetColors(colors);

			if (state_pressed != -1 && statelistdrawable.GetStateDrawable(state_pressed) is RippleDrawable state_active_rippledrawable)
			{
				state_active_rippledrawable.SetColor(new Android.Content.Res.ColorStateList(new int[][] { Array.Empty<int>() }, new int[] { color }));

				if (state_active_rippledrawable.FindDrawableByLayerId(Android.Resource.Id.Mask) is GradientDrawable state_active_max_gradientdrawable)
					state_active_max_gradientdrawable.SetColors(colors);
			}
		}

		void MusicBar.IOnMusicBarProgressChangeListener.OnProgressChanged(MusicBar musicBarView, int progress, bool fromUser)
		{
			if (fromUser is false)
				return;

			SetPosition(TimeSpan.FromMilliseconds(musicBarView.Position), null, false);
		}
		void MusicBar.IOnMusicBarProgressChangeListener.OnStartTrackingTouch(MusicBar musicBarView)
		{ }
		void MusicBar.IOnMusicBarProgressChangeListener.OnStopTrackingTouch(MusicBar musicBarView)
		{
			InvokeOnViewOperation(this, new ViewOperationEventArgs(ViewOperations.Seek)
			{
				SeekValue = musicBarView.Position
			});
		}

		class ArtworkOnScrollListener : RecyclerView.OnScrollListener
		{
			public ArtworkOnScrollListener(NowPlayingView nowplaying)
			{
				NowPlaying = nowplaying;
			}

			NowPlayingView NowPlaying { get; set; }

			public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
			{
				base.OnScrolled(recyclerView, dx, dy);

				NowPlaying.ArtworkViewCurrent ??= NowPlaying.Artwork.FindViewHolderForAdapterPosition(NowPlaying.Player?.Queue.CurrentIndex)?.ItemView;

				if (NowPlaying.ArtworkViewCurrent is null)
					return;

				if (NowPlaying.ArtworkSnapHelper.CalculateDistanceToFinalSnap(NowPlaying.Artwork.SimpleLayoutManager, NowPlaying.ArtworkViewCurrent) is int[] distances)
				{
					if (distances[0] < 0) distances[0] *= -1;
					NowPlaying.BackgroundBlur.Alpha = 1 - (float)distances[0] / (float)NowPlaying.Artwork.Width;
				}
			}
			public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
			{
				base.OnScrollStateChanged(recyclerView, newState);

				switch (newState)
				{
					case RecyclerView.ScrollStateIdle when
					NowPlaying.Player?.Queue.CurrentIndex != null &&
					NowPlaying.Artwork.SimpleLayoutManager.GetPosition(NowPlaying.ArtworkSnapHelper) is int position:
						switch (true)
						{
							case true when position == NowPlaying.Player.Queue.CurrentIndex.Value - 1:
								NowPlaying.ArtworkViewCurrent = null;
								NowPlaying.Artwork.SimpleAdapter.NotifyItemChanged(position - 1);
								NowPlaying.InvokeOnViewOperation(this, new ViewOperationEventArgs(ViewOperations.PressPrevious));
								break;

							case true when position == NowPlaying.Player.Queue.CurrentIndex.Value:
								NowPlaying.Artwork.SimpleAdapter.NotifyItemChanged(position);
								break;

							case true when position == NowPlaying.Player.Queue.CurrentIndex.Value + 1:
								NowPlaying.ArtworkViewCurrent = null;
								NowPlaying.Artwork.SimpleAdapter.NotifyItemChanged(position + 1);
								NowPlaying.InvokeOnViewOperation(this, new ViewOperationEventArgs(ViewOperations.PressNext));
								break;

							default:
								NowPlaying.ArtworkViewCurrent = null;
								NowPlaying.Player.Skip(position);
								break;
						}
						break;

					default: break;
				}
			}
		}
	}
}