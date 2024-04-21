#nullable enable

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;

using System;

using Xyzu.Droid;

namespace Xyzu.Views.Setting
{
	public class AudioBand : ConstraintLayout
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_view_setting_audioband;

			public const int Title_AppCompatTextView = Resource.Id.xyzu_view_setting_audioband_title_appcompattextview;
			public const int Max_AppCompatTextView = Resource.Id.xyzu_view_setting_audioband_max_appcompattextview;
			public const int ValueFrame_ContentFrameLayout = Resource.Id.xyzu_view_setting_audioband_value_frame_contentframelayout;
			public const int Value_AppCompatSeekBar = Resource.Id.xyzu_view_setting_audioband_value_appcompatseekbar;
			public const int Min_AppCompatTextView = Resource.Id.xyzu_view_setting_audioband_min_appcompattextview;
			public const int Footer_AppCompatTextView = Resource.Id.xyzu_view_setting_audioband_footer_appcompattextview;

		}

		public AudioBand(Context context) : this(context, null!) { }
		public AudioBand(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_View_Setting_AudioBand) { }
		public AudioBand(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}

		protected void Init(Context context, IAttributeSet? attrs)
		{
			Inflate(context, Ids.Layout, this);

			_Title = FindViewById(Ids.Title_AppCompatTextView) as AppCompatTextView;
			_Max = FindViewById(Ids.Max_AppCompatTextView) as AppCompatTextView;
			_ValueFrame = FindViewById(Ids.ValueFrame_ContentFrameLayout) as ContentFrameLayout;
			_Value = FindViewById(Ids.Value_AppCompatSeekBar) as AppCompatSeekBar;
			_Min = FindViewById(Ids.Min_AppCompatTextView) as AppCompatTextView;
			_Footer = FindViewById(Ids.Footer_AppCompatTextView) as AppCompatTextView;
		}

		private AppCompatTextView? _Title;
		private AppCompatTextView? _Max;
		private ContentFrameLayout? _ValueFrame;
		private AppCompatSeekBar? _Value;
		private AppCompatTextView? _Min;
		private AppCompatTextView? _Footer;

		public AppCompatTextView Title
		{
			get => _Title ?? throw new InflateException();
		}
		public AppCompatTextView Max
		{
			get => _Max ?? throw new InflateException();
		}
		public ContentFrameLayout ValueFrame
		{
			get => _ValueFrame ?? throw new InflateException();
		}											
		public AppCompatSeekBar Value
		{
			get => _Value ?? throw new InflateException();
		}											 
		public AppCompatTextView Min
		{
			get => _Min ?? throw new InflateException();
		}
		public AppCompatTextView Footer
		{
			get => _Footer ?? throw new InflateException();
		}

		public Action<object, SeekBar.ProgressChangedEventArgs>? ValueProgressChanged { get; set; }

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			Value.ProgressChanged += Value_ProgressChanged;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			Value.ProgressChanged -= Value_ProgressChanged;
		}

		private void Value_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs args)
		{
			ValueProgressChanged?.Invoke(sender, args);
		}
	}
}