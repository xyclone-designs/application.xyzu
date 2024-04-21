#nullable enable

using Android.Views;

using System;

namespace Com.Sothree.Slidinguppanel
{
	public class PanelSlideListener : SlidingUpPanelLayout.SimplePanelSlideListener
	{
		public Action<View, float>? OnPanelSlideAction { get; set; }
		public Action<View, SlidingUpPanelLayout.PanelState, SlidingUpPanelLayout.PanelState>? OnPanelStateChangedAction { get; set; }

		public override void OnPanelSlide(View p0, float p1)
		{
			base.OnPanelSlide(p0, p1);

			OnPanelSlideAction?.Invoke(p0, p1);
		}
		public override void OnPanelStateChanged(View p0, SlidingUpPanelLayout.PanelState p1, SlidingUpPanelLayout.PanelState p2)
		{
			base.OnPanelStateChanged(p0, p1, p2);

			OnPanelStateChangedAction?.Invoke(p0, p1, p2);
		}
	}
}