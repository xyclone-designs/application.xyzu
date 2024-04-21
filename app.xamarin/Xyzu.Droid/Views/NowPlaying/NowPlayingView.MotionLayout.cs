#nullable enable

using Android.Content;
using Android.Graphics;
using AndroidX.ConstraintLayout.Motion.Widget;
using AndroidX.ConstraintLayout.Widget;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.NowPlaying
{
	public partial class NowPlayingView : MotionLayout.ITransitionListener 
	{
		public static partial class Ids
		{
			public static class MotionScene
			{
				public const int LayoutDescription = Resource.Xml.motionscene_nowplayingview;

				public static class ConstraintSets
				{
					public const int Collapsed = Layout_Collapsed;
					public const int Expanded = Layout_Expanded;
					public const int Collapsed_OutLeft = Resource.Id.motionscene_nowplayingview_constraintset_collapsed_outleft;
					public const int Collapsed_OutRight = Resource.Id.motionscene_nowplayingview_constraintset_collapsed_outright;
				}
				public static class Transitions
				{
					public const int Expand = Resource.Id.motionscene_nowplayingview_transition_expand;
					public const int Collapse = Resource.Id.motionscene_nowplayingview_transition_collapse;
					public const int Collapsed_OutRight = Resource.Id.motionscene_nowplayingview_transition_collapsed_outright;
					public const int Collapsed_OutLeft = Resource.Id.motionscene_nowplayingview_transition_collapsed_outleft;
				}
			}
		}

		public ConstraintSet? _ConstraintSetCollapsed;
		public ConstraintSet? ConstraintSetCollapsed
		{
			get => GetConstraintSet(Ids.MotionScene.ConstraintSets.Collapsed);
		}
		public ConstraintSet? ConstraintSetExpanded
		{
			get => GetConstraintSet(Ids.MotionScene.ConstraintSets.Expanded);
		}
		public ConstraintSet? ConstraintSetCollapsed_OutLeft
		{
			get => GetConstraintSet(Ids.MotionScene.ConstraintSets.Collapsed_OutLeft);
		}
		public ConstraintSet? ConstraintSetCollapsed_OutRight
		{
			get => GetConstraintSet(Ids.MotionScene.ConstraintSets.Collapsed_OutRight);
		}

		public MotionScene.Transition? TransitionExpand
		{
			get => GetTransition(Ids.MotionScene.Transitions.Expand);
		}
		public MotionScene.Transition? TransitionCollapse

		{
			get => GetTransition(Ids.MotionScene.Transitions.Collapse);
		}
		public MotionScene.Transition? TransitionCollapsed_OutRight
		{
			get => GetTransition(Ids.MotionScene.Transitions.Collapsed_OutRight);
		}
		public MotionScene.Transition? TransitionCollapsed_OutLeft
		{
			get => GetTransition(Ids.MotionScene.Transitions.Collapsed_OutLeft);
		}					

		public MotionScene.Transition? TransitionCurrent { get; protected set; }

		public void InitConstraintSets(Context context)
		{
			// TODO InitConstraintSets on MotionScene and Layout XML (XML Attributes don't work)

			if (context.Resources?.GetStatusBarHeight() is int statusbarheight)
			{
				ConstraintSetExpanded?.ConstrainHeight(Ids.StatusbarInset, statusbarheight);
				ConstraintSetCollapsed?.ConstrainHeight(Ids.StatusbarInset, statusbarheight);
			}
			if (context.Resources?.GetNavigationBarHeight() is int navigationbarheight)
			{
				ConstraintSetExpanded?.ConstrainHeight(Ids.NavigationbarInset, navigationbarheight);
				ConstraintSetCollapsed?.ConstrainHeight(Ids.NavigationbarInset, navigationbarheight);
			}

			ConstraintSetExpanded?.ApplyTo(this);
			ConstraintSetCollapsed?.ApplyTo(this);
		}

		public override void SetTransition(int transitionId) 
		{
			SetTransition(GetTransition(transitionId));

		}
		public override void SetTransition(int beginId, int endId) 
		{
			if (true switch
			{
				true when TransitionCollapse?.StartConstraintSetId == beginId && TransitionCollapse.EndConstraintSetId == beginId
					=> TransitionCollapse,

				true when TransitionCollapsed_OutRight?.StartConstraintSetId == beginId && TransitionCollapsed_OutRight.EndConstraintSetId == beginId
					=> TransitionCollapsed_OutRight,

				true when TransitionCollapsed_OutLeft?.StartConstraintSetId == beginId && TransitionCollapsed_OutLeft.EndConstraintSetId == beginId
					=> TransitionCollapsed_OutLeft,

				true when TransitionExpand?.StartConstraintSetId == beginId && TransitionExpand.EndConstraintSetId == beginId
					=> TransitionExpand,

				_ => null,
			
			} is MotionScene.Transition transition)
				SetTransition(transition);

			else base.SetTransition(beginId, endId);
		}

		protected override void SetTransition(MotionScene.Transition? transition)
		{
			base.SetTransition(TransitionCurrent = transition);
		}

		public Action<MotionLayout?, int, int, float>? OnTransitionChangeAction { get; set; }
		public Action<MotionLayout?, int>? OnTransitionCompletedAction { get; set; }
		public Action<MotionLayout?, int, int>? OnTransitionStartedAction { get; set; }
		public Action<MotionLayout?, int, bool, float>? OnTransitionTriggerAction { get; set; }

		public void OnTransitionChange(MotionLayout? motionLayout, int startId, int endId, float progress) 
		{
			OnTransitionChangeAction?.Invoke(motionLayout, startId, endId, progress);
		}
		public void OnTransitionCompleted(MotionLayout? motionLayout, int currentId)
		{
			switch (TransitionCurrent?.Id)
			{
				case Ids.MotionScene.Transitions.Expand:
					if (Player?.Queue.CurrentIndex is int currentindex)
						Artwork.SimpleAdapter.NotifyItemChanged(currentindex);
					break;

				case Ids.MotionScene.Transitions.Collapsed_OutLeft:
					if (Progress != 1 || Player is null)
						break;

					if (Player.Queue.Next is null)
						TransitionToStart();
					else
					{
						Player.Next();

						this.SetAndTransitionToStart(Ids.MotionScene.Transitions.Collapsed_OutRight);
					}

					break;

				case Ids.MotionScene.Transitions.Collapsed_OutRight:

					if (Progress != 1 || Player is null)
						break;

					if (Player.Queue.Previous is null)
						TransitionToStart();
					else
					{
						Player.Previous();

						this.SetAndTransitionToStart(Ids.MotionScene.Transitions.Collapsed_OutLeft);
					}

					break;

				default: break;
			}

			OnTransitionCompletedAction?.Invoke(motionLayout, currentId);
		}
		public void OnTransitionStarted(MotionLayout? motionLayout, int startId, int endId)
		{
			OnTransitionStartedAction?.Invoke(motionLayout, startId, endId);
		}
		public void OnTransitionTrigger(MotionLayout? motionLayout, int triggerId, bool positive, float progress)
		{
			OnTransitionTriggerAction?.Invoke(motionLayout, triggerId, positive, progress);
		}
	}
}