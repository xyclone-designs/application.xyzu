#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Util;

using Oze.Music.MusicBarLib;

using System;
using System.IO;
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

		public override void LoadFrom(Stream stream, int duration)
		{
			for (int index = 0; index < 5; index++)
				try { base.LoadFrom(stream, duration); break; }
				catch (Exception) { }
		}
		public override void LoadFrom(string pathname, int duration)
		{
			for (int index = 0; index < 5; index++)
				try { base.LoadFrom(pathname, duration); break; }
				catch (Exception) { }
		}
		public override void SetProgress(int position)
		{
			try { base.SetProgress(position); }
			catch (Exception) { }
		}
	}
	public class ProgressBarScrollable : ScrollableMusicBar
	{
		public ProgressBarScrollable(Context context) : this(context, null) { }
		public ProgressBarScrollable(Context context, IAttributeSet? attrs) : this(context, attrs, Resource.Style.Xyzu_View_NowPlaying_ProgressBar_Scrollable) { }
		public ProgressBarScrollable(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void OnDraw(Canvas? canvas)
		{
			try { base.OnDraw(canvas); } catch (Exception) { }
		}

		public override void LoadFrom(Stream stream, int duration)
		{
			for (int index = 0; index < 5; index++)
				try { base.LoadFrom(stream, duration); break; }
				catch (Exception) { }
		}
		public override void LoadFrom(string pathname, int duration)
		{
			for (int index = 0; index < 5; index++)
				try { base.LoadFrom(pathname, duration); break; }
				catch (Exception) { }
		}
		public override void SetProgress(int position)
		{
			try { base.SetProgress(position); }
			catch (Exception) { }
		}
	}
}