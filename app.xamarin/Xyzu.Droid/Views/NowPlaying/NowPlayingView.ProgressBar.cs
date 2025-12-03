using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;


//using Oze.Music.MusicBarLib;

using System;
using System.IO;
using Xyzu.Droid;

namespace Xyzu.Views.NowPlaying
{
	public class Bar : Android.Views.View
	{
		public Bar(Context context) : base(context) { }
		public Bar(Context context, IAttributeSet? attrs) : base(context, attrs) { }
		public Bar(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		public void Hide() { }
		public void Show() { }

		public virtual void LoadFrom(Stream stream, int duration) { }
		public virtual void LoadFrom(string pathname, int duration) { }
		public virtual void SetProgress(int position) { }
		public virtual void SetBarWidth(int barwidth) { }
		public virtual void SetSpaceBetweenBar(int spacebetweenbar) { }
		public virtual void SetProgressChangeListener(View view) { }
		public virtual void SetLoadedBarColor(int loadedbarcolor) { }
	}
	public class ProgressBarFixed : Bar// FixedMusicBar
	{
		public ProgressBarFixed(Context context) : this(context, null) { }
		public ProgressBarFixed(Context context, IAttributeSet? attrs) : this(context, attrs, Resource.Style.Xyzu_View_NowPlaying_ProgressBar_Fixed) { }
		public ProgressBarFixed(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void OnDraw(Canvas canvas)
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
	public class ProgressBarScrollable : Bar// ScrollableMusicBar
	{
		public ProgressBarScrollable(Context context) : this(context, null) { }
		public ProgressBarScrollable(Context context, IAttributeSet? attrs) : this(context, attrs, Resource.Style.Xyzu_View_NowPlaying_ProgressBar_Scrollable) { }
		public ProgressBarScrollable(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void OnDraw(Canvas canvas)
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