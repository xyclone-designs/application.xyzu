#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Util;

using Oze.Music.MusicBarLib;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.NowPlaying
{
	public class ProgressBarFixed : FixedMusicBar
	{
		public ProgressBarFixed(Context context) : this(context, null) { }
		public ProgressBarFixed(Context context, IAttributeSet? attrs) : this(context, attrs, Resource.Style.Xyzu_View_NowPlaying_ProgressBar_Fixed) { }
		public ProgressBarFixed(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void OnDraw(Canvas? canvas)
		{
			try { base.OnDraw(canvas); } catch (Exception) { }
		}
	}
	public class ProgressBarScrollable : ScrollableMusicBar
	{
		public ProgressBarScrollable(Context context) : this(context, null) { }
		public ProgressBarScrollable(Context context, IAttributeSet? attrs) : this(context, attrs, Resource.Style.Xyzu_View_NowPlaying_ProgressBar_Scrollable) { }
		public ProgressBarScrollable(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
	}
}