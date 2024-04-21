#nullable enable

using Android.Content;
using Android.Animation;
using Android.Views;
using AndroidX.CardView.Widget;

using Com.Sothree.Slidinguppanel;

using Xyzu.Droid;
using Xyzu.Views.Library;
using Xyzu.Views.NowPlaying;

namespace Xyzu.Activities
{
	public partial class LibraryActivity 
	{
		private NowPlayingView? _ViewNowPlaying;
		private bool _ConfigureSlidingUpPanelHeight = false;
		private bool _ConfigureNowPlayingArtworkDetailsSpace = false;
		private SlidingUpPanelLayout.PanelState? _ConfigureFloatingActionButtonState;
		private string _ConfigureSlidingUpPanelHeightKey => string.Format("LibraryActivity.SlidingUpPanelLayout.PanelHeight.{0}", NewConfiguration?.Orientation.ToString() ?? string.Empty);
		private string _ConfigureNowPlayingArtworkDetailsSpaceKey => string.Format("LibraryActivity.NowPlayingView.ArtworkDetails_Space.{0}", NewConfiguration?.Orientation.ToString() ?? string.Empty);

		protected NowPlayingView? ViewNowPlaying
		{
			get => _ViewNowPlaying;
			set
			{
				_ViewNowPlaying = value;

				if (_ViewNowPlaying != null)
				{
					_ViewNowPlaying.FragmentActivity = this;
					_ViewNowPlaying.Images = XyzuImages.Instance;
					_ViewNowPlaying.Library = XyzuLibrary.Instance;
					_ViewNowPlaying.MenuVariables = new Menus.MenuOptionsUtils.VariableContainer
					{
						Activity = this,
						LibraryNavigatable = this,
					};
					_ViewNowPlaying.Player = XyzuPlayer.Instance.Player;
					_ViewNowPlaying.SharedPreferences = XyzuSettings.Instance.SharedPreferences;
				}
			}
		}
		protected CardView? ViewNowPlayingContainer { get; set; }
		protected float? ViewNowPlayingContainerRadius { get; set; }
		protected SlidingUpPanelLayout? SlidingUpPanel { get; set; }
		protected SlidingUpPanelLayout.IPanelSlideListener? SlidingUpPanelSlideListener { get; set; }

		private void ConfigurePanelInsets(LibraryView? libraryview)
		{
			if (SlidingUpPanel?.GetPanelState() is SlidingUpPanelLayout.PanelState panelstate)
				switch (true)
				{
					case true when
					panelstate == SlidingUpPanelLayout.PanelState.Hidden:
						libraryview?.RemoveInsets("SlidingUpPanelLayoutPanel");
						break;

					case true when
					panelstate == SlidingUpPanelLayout.PanelState.Collapsed:
						libraryview?.AddInsets("SlidingUpPanelLayoutPanel", null, null, null, SlidingUpPanel.PanelHeight);
						break;

					default: break;
				}
		}

		private bool ConfigureSlidingUpPanelHeight(bool force = false)
		{
			bool wasinitial = false;

			if (_ConfigureSlidingUpPanelHeight && force is false)
				return wasinitial;

			if (SlidingUpPanel is null || ViewNowPlaying is null)
				return wasinitial;

			int slidinguppanelpanelheight = XyzuSettings.Instance.GetInt(_ConfigureSlidingUpPanelHeightKey, -1);

			if (slidinguppanelpanelheight != -1)
			{
				SlidingUpPanel.PanelHeight = slidinguppanelpanelheight;
				Floatingactionbutton.SetMarginBottom(slidinguppanelpanelheight + Resources?.GetDimensionPixelSize(Resource.Dimension.dp16) ?? 0);
			}
			else if (ViewNowPlaying.Height != 0 && ViewNowPlaying.CurrentState == NowPlayingView.Ids.MotionScene.ConstraintSets.Collapsed)
			{
				wasinitial = true;

				SlidingUpPanel.PanelHeight = slidinguppanelpanelheight = ViewNowPlaying.Height;

				XyzuSettings.Instance.SharedPreferences?
					.Edit()?
					.PutInt(_ConfigureSlidingUpPanelHeightKey, slidinguppanelpanelheight)?
					.Commit();
			}
			else return wasinitial;

			_ConfigureSlidingUpPanelHeight = true;

			return wasinitial;
		}
		private bool ConfigureNowPlayingArtworkDetailsSpace(bool force = false)
		{
			bool wasinitial = false;

			if (_ConfigureNowPlayingArtworkDetailsSpace && force is false)
				return wasinitial;

			if (SlidingUpPanel is null || ViewNowPlaying is null)
				return wasinitial;

			int artworkdetailsspaceheight = XyzuSettings.Instance.GetInt(_ConfigureNowPlayingArtworkDetailsSpaceKey, -1);

			if (artworkdetailsspaceheight != -1)
			{
				ViewNowPlaying.ConstraintSetExpanded?.ConstrainHeight(NowPlayingView.Ids.ArtworkDetails_Space, artworkdetailsspaceheight);
				ViewNowPlaying.ConstraintSetExpanded?.ApplyTo(ViewNowPlaying);

				ViewNowPlaying.ConstraintSetCollapsed?.ConstrainHeight(NowPlayingView.Ids.ArtworkDetails_Space, 0);
				ViewNowPlaying.ConstraintSetCollapsed?.ApplyTo(ViewNowPlaying);
			}
			else if (ViewNowPlaying.Height != 0 && ViewNowPlaying.CurrentState == NowPlayingView.Ids.MotionScene.ConstraintSets.Expanded)
			{
				wasinitial = true;

				artworkdetailsspaceheight = SlidingUpPanel.Bottom - ViewNowPlaying.NavigationbarInset.Bottom;

				if (ViewNowPlaying.FindViewById(NowPlayingView.Ids.ArtworkDetails_Space)?.Height is int height && ValueAnimator.OfInt(height, artworkdetailsspaceheight) is ValueAnimator valueanimator)
				{
					valueanimator.SetDuration(300);
					valueanimator.AddUpdateListener(new AnimatorUpdateListener
					{
						OnAnimationUpdateAction = animator =>
						{
							if (animator?.AnimatedValue is Java.Lang.Integer integer)
							{
								ViewNowPlaying.ConstraintSetExpanded?.ConstrainHeight(NowPlayingView.Ids.ArtworkDetails_Space, artworkdetailsspaceheight);
								ViewNowPlaying.ConstraintSetExpanded?.ApplyTo(ViewNowPlaying);

								ViewNowPlaying.ConstraintSetCollapsed?.ConstrainHeight(NowPlayingView.Ids.ArtworkDetails_Space, 0);
								ViewNowPlaying.ConstraintSetCollapsed?.ApplyTo(ViewNowPlaying);
							}
						}
					});

					valueanimator.Start();
				}
				else
				{
					ViewNowPlaying.ConstraintSetExpanded?.ConstrainHeight(NowPlayingView.Ids.ArtworkDetails_Space, artworkdetailsspaceheight);
					ViewNowPlaying.ConstraintSetExpanded?.ApplyTo(ViewNowPlaying);		  

					ViewNowPlaying.ConstraintSetCollapsed?.ConstrainHeight(NowPlayingView.Ids.ArtworkDetails_Space, 0);
					ViewNowPlaying.ConstraintSetCollapsed?.ApplyTo(ViewNowPlaying);
				}

				XyzuSettings.Instance.SharedPreferences?
					.Edit()?
					.PutInt(_ConfigureNowPlayingArtworkDetailsSpaceKey, artworkdetailsspaceheight)?
					.Commit();
			}
			else return wasinitial;

			_ConfigureNowPlayingArtworkDetailsSpace = true;

			return wasinitial;
		}

		protected virtual void OnPanelSlideAction(View view, float percentage)
		{
			if (ViewNowPlayingContainer != null)
				ViewNowPlayingContainer.Radius = (ViewNowPlayingContainerRadius ??= ViewNowPlayingContainer.Radius) * (1 - percentage);

			if (ViewNowPlaying != null)
			{
				switch (ViewNowPlaying.TransitionCurrent?.Id)
				{
					case NowPlayingView.Ids.MotionScene.Transitions.Collapse:
						ViewNowPlaying.SetInterpolatedProgress(1 - percentage);
						break;

					default:
						ViewNowPlaying.SetInterpolatedProgress(percentage);
						break;
				}
			}
		}
		protected virtual void OnPanelStateChangedAction(View view, SlidingUpPanelLayout.PanelState state1, SlidingUpPanelLayout.PanelState state2)
		{
			if (ViewNowPlaying is null)
				return;

			switch (true)
			{
				// Collapsed => Dragging
				case true when
				state1 == SlidingUpPanelLayout.PanelState.Collapsed &&
				state2 == SlidingUpPanelLayout.PanelState.Dragging:
					ViewNowPlaying.SetTransition(NowPlayingView.Ids.MotionScene.Transitions.Expand);
					break;

				// Dragging => Expanded
				case true when
				state1 == SlidingUpPanelLayout.PanelState.Dragging &&
				state2 == SlidingUpPanelLayout.PanelState.Expanded:
					ViewNowPlaying.SetTransition(NowPlayingView.Ids.MotionScene.Transitions.Expand);
					ViewNowPlaying.SetInterpolatedProgress(1);
					break;

				// Expanded => Dragging 
				case true when
				state1 == SlidingUpPanelLayout.PanelState.Expanded &&
				state2 == SlidingUpPanelLayout.PanelState.Dragging:
					ViewNowPlaying.SetTransition(NowPlayingView.Ids.MotionScene.Transitions.Collapse);
					break;

				// Dragging => Collapsed
				case true when
				state1 == SlidingUpPanelLayout.PanelState.Dragging &&
				state2 == SlidingUpPanelLayout.PanelState.Collapsed:
					ViewNowPlaying.SetTransition(NowPlayingView.Ids.MotionScene.Transitions.Collapse);
					ViewNowPlaying.SetInterpolatedProgress(1);
					break;

				default: break;
			}

			if (SlidingUpPanel?.GetPanelState() is SlidingUpPanelLayout.PanelState panelstate)
				switch (true)
				{
					case true when
					panelstate == SlidingUpPanelLayout.PanelState.Hidden:
						FragmentActions(libraryfragment => ConfigurePanelInsets(libraryfragment.LibraryView));
						if (_ConfigureFloatingActionButtonState != SlidingUpPanelLayout.PanelState.Hidden)
						{
							Floatingactionbutton.SetMarginBottom(Resources?.GetNavigationBarHeight() ?? 0 + Resources?.GetDimensionPixelSize(Resource.Dimension.dp16) ?? 0);
							_ConfigureFloatingActionButtonState = SlidingUpPanelLayout.PanelState.Hidden;
						}
						break;

					case true when
					panelstate == SlidingUpPanelLayout.PanelState.Collapsed:
						FragmentActions(libraryfragment => ConfigurePanelInsets(libraryfragment.LibraryView));
						if (_ConfigureFloatingActionButtonState != SlidingUpPanelLayout.PanelState.Collapsed)
						{
							Floatingactionbutton.SetMarginBottom(SlidingUpPanel.PanelHeight + Resources?.GetDimensionPixelSize(Resource.Dimension.dp16) ?? 0);
							_ConfigureFloatingActionButtonState = SlidingUpPanelLayout.PanelState.Collapsed;
						}
						break;

					default: break;
				}
		}

		protected void InitSlidingPanelLayout(View? slidinguppanel, View? viewnowplaying, View? viewnowplayingcontainer)
		{
			InitSlidingPanelLayout(slidinguppanel as SlidingUpPanelLayout, viewnowplaying as NowPlayingView, viewnowplayingcontainer as CardView);
		}	 
		protected void InitSlidingPanelLayout(SlidingUpPanelLayout? slidinguppanel, NowPlayingView? viewnowplaying, CardView? viewnowplayingcontainer)
		{
			RootView = SlidingUpPanel = slidinguppanel;
			ViewNowPlaying = viewnowplaying;
			ViewNowPlayingContainer = viewnowplayingcontainer;
			ViewNowPlayingContainerRadius = viewnowplayingcontainer?.Radius;

			ConfigureSlidingUpPanelHeight();
			ConfigureNowPlayingArtworkDetailsSpace();

			SlidingUpPanel?.AddPanelSlideListener(SlidingUpPanelSlideListener ??= new PanelSlideListener
			{
				OnPanelSlideAction = OnPanelSlideAction,
				OnPanelStateChangedAction = OnPanelStateChangedAction
			});

			if (ViewNowPlaying != null)
			{
				ViewNowPlaying.ViewTreeObserver?.AddOnGlobalLayoutListener(new ViewTreeObserverOnGlobalLayoutListener
				{
					OnGlobalLayoutAction = () =>
					{
						ConfigureSlidingUpPanelHeight();
						ConfigureNowPlayingArtworkDetailsSpace();
					}
				});
				ViewNowPlaying.OnViewOperationAction = args =>
				{
					switch (args.ViewOperation)
					{
						case NowPlayingView.ViewOperations.PressView:
							if (SlidingUpPanel != null && SlidingUpPanel.GetPanelState() != SlidingUpPanelLayout.PanelState.Expanded)
								SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
							break;

						default: break;
					}
				};
			}
		}
	}
}