#nullable enable

using Android.Content;

using System;

namespace Android.Views
{
	public interface IGestureDetector : GestureDetector.IOnGestureListener, GestureDetector.IOnContextClickListener, GestureDetector.IOnDoubleTapListener, View.IOnClickListener, View.IOnTouchListener
	{
		event EventHandler<GestureEventArgs>? OnGesture;

		View? View { get; set; }
		Action<GestureEventArgs>? OnGestureAction { get; set; }
		
		public class Default : GestureDetector.SimpleOnGestureListener, IGestureDetector
		{
			public Default(Context? context) : base()
			{
				_GestureDetector = new GestureDetector(context, this);
			}

			protected GestureDetector _GestureDetector;

			public View? View { get; set; }
			public event EventHandler<GestureEventArgs>? OnGesture;
			public Action<GestureEventArgs>? OnGestureAction { get; set; }

			public void OnClick(View? view)
			{
				if (OnGesture is null && OnGestureAction is null)
					return;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.Click,
					View = view ?? View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);
			}
			public bool OnTouch(View? view, MotionEvent? motionevent)
			{
				View = view;

				bool result = _GestureDetector.OnTouchEvent(motionevent);

				if (result is false)
					switch (motionevent?.Action)
					{
						case MotionEventActions.Up:
							if (OnGesture is null && OnGestureAction is null)
								return true;

							GestureEventArgs args = new GestureEventArgs
							{
								Gesture = Gestures.Up,
								MotionEvents = new MotionEvent?[] { motionevent },
								View = View,
							};

							OnGesture?.Invoke(this, args);
							OnGestureAction?.Invoke(args);
							
							result = true;
							break;
					}
			
				return result;
			}
			public override bool OnContextClick(MotionEvent? e)
			{
				if (OnGesture is null && OnGestureAction is null)
					return true;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.ContextClick,
					MotionEvents = new MotionEvent?[] { e },
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);

				return true;
			}
			public override bool OnDoubleTap(MotionEvent? e)
			{
				if (OnGesture is null && OnGestureAction is null)
					return true;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.DoubleTap,
					MotionEvents = new MotionEvent?[] { e },
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);

				return true;
			}
			public override bool OnDoubleTapEvent(MotionEvent? e)
			{
				if (OnGesture is null && OnGestureAction is null)
					return true;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.DoubleTapEvent,
					MotionEvents = new MotionEvent?[] { e },
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);

				return true;
			}
			public override bool OnDown(MotionEvent? e)
			{
				if (OnGesture is null && OnGestureAction is null)
					return true;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.Down,
					MotionEvents = new MotionEvent?[] { e },
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);

				return true;
			}
			public override bool OnFling(MotionEvent? e1, MotionEvent? e2, float velocityX, float velocityY)
			{
				if (OnGesture is null && OnGestureAction is null)
					return true;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.Fling,
					MotionEvents = new MotionEvent?[] { e1, e2 },
					VelocityX = velocityX,
					VelocityY = velocityY,
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);

				return true;
			}
			public override void OnLongPress(MotionEvent? e)
			{
				if (OnGesture is null && OnGestureAction is null)
					return;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.LongPress,
					MotionEvents = new MotionEvent?[] { e },
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);
			}
			public override bool OnScroll(MotionEvent? e1, MotionEvent? e2, float distanceX, float distanceY)
			{
				if (OnGesture is null && OnGestureAction is null)
					return true;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.Scroll,
					MotionEvents = new MotionEvent?[] { e1, e2 },
					DistanceX = distanceX,
					DistanceY = distanceY,
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);

				return true;
			}
			public override void OnShowPress(MotionEvent? e)
			{
				if (OnGesture is null && OnGestureAction is null)
					return;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.ShowPress,
					MotionEvents = new MotionEvent?[] { e },
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);
			}
			public override bool OnSingleTapConfirmed(MotionEvent? e)
			{
				if (OnGesture is null && OnGestureAction is null)
					return true;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.SingleTapConfirmed,
					MotionEvents = new MotionEvent?[] { e },
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);

				return true;
			}
			public override bool OnSingleTapUp(MotionEvent? e)
			{
				if (OnGesture is null && OnGestureAction is null)
					return true;

				GestureEventArgs args = new GestureEventArgs
				{
					Gesture = Gestures.SingleTapUp,
					MotionEvents = new MotionEvent?[] { e },
					View = View,
				};

				OnGesture?.Invoke(this, args);
				OnGestureAction?.Invoke(args);

				return true;
			}
		}
	}
}