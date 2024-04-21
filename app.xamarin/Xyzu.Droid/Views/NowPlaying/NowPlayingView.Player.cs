#nullable enable

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
		protected int PositionTimerPeriodStart = 500;

		protected IPlayer? _Player;
		protected INowPlayingSettingsDroid? _Settings;
		protected Timer? _PositionTimer;
		protected ISong? _SongPrevious;
		protected ISong? _SongCurrent;
		protected ISong? _SongNext;

		protected Bitmap? _SongPreviousBitmap;
		protected Drawable? _SongPreviousBlurDrawable;
		protected Bitmap? _SongCurrentBitmap;
		protected Drawable? _SongCurrentBlurDrawable;
		protected Bitmap? _SongNextBitmap; 
		protected Drawable? _SongNextBlurDrawable;

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
					_Player.OnPropertyChanged -= PlayerPropertyChanged;
					_Player.OnPlayerOperation -= PlayerOnPlayerOperation;
					_Player.Queue.PropertyChanged -= PlayerQueuePropertyChanged;
				}

				_Player = value;

				if (_Player != null)
				{
					_Player.OnPropertyChanged += PlayerPropertyChanged;
					_Player.OnPlayerOperation += PlayerOnPlayerOperation;
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

		public ISong? SongPrevious
		{
			get => _SongPrevious;
			set
			{ 
				_SongPrevious = value;

				OnPropertyChanged();
			}
		}
		public ISong? SongCurrent
		{
			get => _SongCurrent;
			set
			{ 
				_SongCurrent = value;

				OnPropertyChanged();
			}
		}
		public ISong? SongNext
		{
			get => _SongNext;
			set
			{
				_SongNext = value;

				OnPropertyChanged();
			}
		}
		public IEnumerable<ISong> Songs
		{
			get
			{
				IEnumerable<ISong> songs = Enumerable.Empty<ISong>();

				if (SongCurrent != null)
					songs = songs.Append(SongCurrent);

				return songs;
			}
		}

		protected virtual void SettingsPropertyChanged(object sender, PropertyChangedEventArgs args) 
		{ }
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyname = null)
		{
			switch (propertyname)
			{
				case nameof(Player):
					PlayerPropertyChanged(this, new PropertyChangedEventArgs(nameof(IPlayer.State)));
					PlayerQueuePropertyChanged(this, new PropertyChangedEventArgs(nameof(IQueue.CurrentIndex)));
					break;

				case nameof(SongPrevious):
					if (SongPrevious is null)
					{
						_SongPreviousBitmap = null;
						_SongPreviousBlurDrawable = null;
					}
					else
					{
						_SongPreviousBitmap ??= Images?.GetBitmap(IImagesDroid.DefaultOperations.Rounded, null, SongPrevious);
						_SongPreviousBlurDrawable ??= Images?.GetBitmap(IImagesDroid.DefaultOperations.BlurDownsample, null, _SongPreviousBitmap, SongPrevious) is Bitmap _songpreviousblurdrawable
							? new BitmapDrawable(Context?.Resources, _songpreviousblurdrawable)
							: null;
					}
					break;			

				case nameof(SongCurrent):
					if (SongCurrent is null)
					{
						_SongCurrentBitmap = null;
						_SongCurrentBlurDrawable = null;
					}
					else
					{
						_SongCurrentBitmap ??= Images?.GetBitmap(IImagesDroid.DefaultOperations.Rounded, null, SongCurrent);
						_SongCurrentBlurDrawable ??= Images?.GetBitmap(IImagesDroid.DefaultOperations.BlurDownsample, null, _SongCurrentBitmap, SongCurrent) is Bitmap _songcurrentblurdrawable
							? new BitmapDrawable(Context?.Resources, _songcurrentblurdrawable)
							: null;
					}
					break;			

				case nameof(SongNext):
					if (SongNext is null)
					{
						_SongNextBitmap = null;
						_SongNextBlurDrawable = null;
					}
					else
					{
						_SongNextBitmap ??= Images?.GetBitmap(IImagesDroid.DefaultOperations.Rounded, null, SongNext);
						_SongNextBlurDrawable ??= Images?.GetBitmap(IImagesDroid.DefaultOperations.BlurDownsample, null, _SongNextBitmap, SongNext) is Bitmap _songnextblurdrawable
							? new BitmapDrawable(Context?.Resources, _songnextblurdrawable)
							: null;
					}
					break;

				default: break;
			}
		}

		public void ViewRefresh()
		{
			int? queueindex = Player?.Queue.CurrentIndex;
			int? viewindex = Artwork.SimpleLayoutManager.GetPosition(ArtworkSnapHelper);

			switch (true)
			{
				case true when queueindex is null || viewindex is null:
					break;

				case true when queueindex == viewindex - 1 || queueindex == viewindex + 1:
					Artwork.SmoothScrollToPosition(queueindex.Value);
					break;

				default:
					Artwork.ScrollToPosition(queueindex.Value);
					Artwork.SimpleAdapter.NotifyItemChanged(queueindex.Value);
					break;
			}

			SetPalette(SongCurrent);
			SetText(SongCurrent, null, null);
			SetBlur(SongCurrent);
			SetPosition(SongCurrent);
		}
		public void ViewReset()
		{
			SongCurrent = null;
			SongPrevious = null;
			SongNext = null;

			Player = null;

			ViewRefresh();
		}

		public void PlayerPropertyChanged(object sender, PropertyChangedEventArgs args)
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
							_SongNextBitmap = null;
							_SongNextBlurDrawable = null;

							_SongCurrent = null;
							_SongCurrentBitmap = null;
							_SongCurrentBlurDrawable = null;

							_SongPrevious = null;
							_SongPreviousBitmap = null;
							_SongPreviousBlurDrawable = null;

							PositionTimer?.Change(PositionTimerDue, PositionTimerPeriodStop);
							Buttons_Player_PlayPause.SetImageResource(Resource.Drawable.icon_player_play);

							ViewReset();
							break;
					}
					break;

				default: break;
			}
		}
		public void PlayerOnPlayerOperation(object sender, IPlayer.PlayerOperationsEventArgs args)
		{
			switch (args.PlayerOperation)
			{
				case PlayerOperations.Previous:
					_SongNext = _SongCurrent;
					_SongNextBitmap = _SongCurrentBitmap;
					_SongNextBlurDrawable = _SongCurrentBlurDrawable;

					_SongCurrent = _SongPrevious;
					_SongCurrentBitmap = _SongPreviousBitmap;
					_SongCurrentBlurDrawable = _SongPreviousBlurDrawable;

					_SongPrevious = Library?.Songs.PopulateSong(Player?.Queue.PreviousSong);
					_SongPreviousBitmap = null;
					_SongPreviousBlurDrawable = null;	
					break;
						  
				case PlayerOperations.Next:
					_SongPrevious = _SongCurrent;
					_SongPreviousBitmap = _SongCurrentBitmap;
					_SongPreviousBlurDrawable = _SongCurrentBlurDrawable;

					_SongCurrent = _SongNext;
					_SongCurrentBitmap = _SongNextBitmap;
					_SongCurrentBlurDrawable = _SongNextBlurDrawable;

					_SongNext = Library?.Songs.PopulateSong(Player?.Queue.NextSong);
					_SongNextBitmap = null;
					_SongNextBlurDrawable = null;						 
					break;

				default: break;
			}
		}
		public void PlayerQueuePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(IQueue.CurrentIndex):

					if (Player?.Queue.PreviousSong is null || (SongPrevious?.Id is string songpreviousid && songpreviousid != Player.Queue.PreviousSong.Id))
					{
						_SongPreviousBitmap = null;
						_SongPreviousBlurDrawable = null;
						_SongPrevious = null;
					}
																																						  
					if (Player?.Queue.CurrentSong is null || (SongCurrent?.Id is string songcurrentid && songcurrentid != Player.Queue.CurrentSong.Id))
					{
						_SongCurrentBitmap = null;
						_SongCurrentBlurDrawable = null;
						_SongCurrent = null;
					}
																																						  
					if (Player?.Queue.NextSong is null || (SongNext?.Id is string nextsongid && nextsongid != Player.Queue.NextSong.Id))
					{
						_SongNextBitmap = null;
						_SongNextBlurDrawable = null;
						_SongNext = null;
					}

					SongPrevious ??= Library?.Songs.PopulateSong(Player?.Queue.PreviousSong);
					SongCurrent ??= Library?.Songs.PopulateSong(Player?.Queue.CurrentSong);
					SongNext ??= Library?.Songs.PopulateSong(Player?.Queue.NextSong);

					ViewRefresh();
					break;

				default: break;
			}
		}

		public void SetBlur(ISong? song)
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
			else
				Images?.SetToViewBackground(
					view: BackgroundBlur,
					oncomplete: OnComplete,
					operations: IImages.DefaultOperations.Blur,
					sources: song != SongCurrent
						? new object?[] { song }
						: new object?[] { _SongCurrentBitmap, SongCurrent });
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
		public void SetPalette(ISong? song)
		{
			ArtworkPalette = Images?.GetPalette(song);

			if ((ArtworkPalette?.DominantSwatch?.Rgb ?? Context?.Resources?.GetColor(Resource.Color.ColorPrimary, Context.Theme)) is int color)
				Position.SetLoadedBarColor(color);

			ConfigureNowPlayingButton(Buttons_Player_Previous);
			ConfigureNowPlayingButton(Buttons_Player_PlayPause);
			ConfigureNowPlayingButton(Buttons_Player_Next);

			ConfigureNowPlayingButton(Buttons_Menu_Queue);
			ConfigureNowPlayingButton(Buttons_Menu_AudioEffects);
			ConfigureNowPlayingButton(Buttons_Menu_PlayerSettings);
			ConfigureNowPlayingButton(Buttons_Menu_Options);
		}
		public void SetPosition(ISong? song)
		{
			if (song?.Filepath is null || song?.Duration is null || song.Duration.Value == TimeSpan.Zero)
				Position.Hide();
			else
			{
				Task.Run(() => Position.LoadFrom(song.Filepath, (int)song.Duration.Value.TotalMilliseconds));
				Position.Show();
			}
			
			SetPosition(TimeSpan.Zero, song?.Duration, true);
		}
		public void SetPosition(TimeSpan? current, TimeSpan? duration, bool withprogress = true)
		{
			current ??= TimeSpan.FromMilliseconds(Player?.Position ?? 0);
			duration ??= SongCurrent?.Duration ?? Player?.Queue.CurrentSong?.Duration ?? TimeSpan.Zero;

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
			StateListDrawable? statelistdrawable = view?.Background as StateListDrawable;
			int? color = ArtworkPalette?.DominantSwatch?.Rgb ?? Context?.Resources?.GetColor(Resource.Color.ColorPrimary, Context.Theme);

			if (statelistdrawable is null || color is null)
				return;

			int state_active = statelistdrawable.FindStateDrawableIndex(new int[] { Android.Resource.Attribute.StateActive });
			int state_pressed = statelistdrawable.FindStateDrawableIndex(new int[] { Android.Resource.Attribute.StatePressed });
			int[] colors = new int[]
			{
				color.Value,
				color.Value,
				color.Value,
			};

			if (state_active != -1 && statelistdrawable.GetStateDrawable(state_active) is GradientDrawable state_active_gradientdrawable)
				state_active_gradientdrawable.SetColors(colors);

			if (state_pressed != -1 && statelistdrawable.GetStateDrawable(state_pressed) is RippleDrawable state_active_rippledrawable)
			{
				state_active_rippledrawable.SetColor(new Android.Content.Res.ColorStateList(new int[][] { new int[] { } }, new int[] { color.Value }));

				if (state_active_rippledrawable.FindDrawableByLayerId(Android.Resource.Id.Mask) is GradientDrawable state_active_max_gradientdrawable)
					state_active_max_gradientdrawable.SetColors(colors);
			}
		}
		private void ConfigureNowPlayingButton(View? view)
		{
			StateListDrawable? statelistdrawable = view?.Background as StateListDrawable;
			int? color = ArtworkPalette?.DominantSwatch?.Rgb ?? Context?.Resources?.GetColor(Resource.Color.ColorPrimary, Context.Theme);

			if (statelistdrawable is null || color is null)
				return;

			int state_active = statelistdrawable.FindStateDrawableIndex(new int[] { Android.Resource.Attribute.StateActive });
			int state_pressed = statelistdrawable.FindStateDrawableIndex(new int[] { Android.Resource.Attribute.StatePressed });
			int[] colors = new int[]
			{
				color.Value,
				Color.Transparent.ToArgb(),
				Color.Transparent.ToArgb()
			};

			if (state_active != -1 && statelistdrawable.GetStateDrawable(state_active) is GradientDrawable state_active_gradientdrawable)
				state_active_gradientdrawable.SetColors(colors);

			if (state_pressed != -1 && statelistdrawable.GetStateDrawable(state_pressed) is RippleDrawable state_active_rippledrawable)
			{
				state_active_rippledrawable.SetColor(new Android.Content.Res.ColorStateList(new int[][] { new int[] { } }, new int[] { color.Value }));

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