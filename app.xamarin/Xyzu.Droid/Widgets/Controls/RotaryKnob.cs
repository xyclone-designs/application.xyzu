using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;

using SeekArc.Droid;

using System;

using XyzuResource = Xyzu.Droid.Resource;

namespace Xyzu.Widgets.Controls
{
	[Register("xyzu/widgets/controls/RotaryKnob")]
	public class RotaryKnob : ConstraintLayout
	{
		public RotaryKnob(Context context) : this(context, null) { }
		public RotaryKnob(Context context, IAttributeSet? attrs) : this(context, attrs, XyzuResource.Style.Xyzu_Widget_Controls_RotaryKnob) { }
		public RotaryKnob(Context context, IAttributeSet? attrs, int defStyleAttr) : this(context, attrs, defStyleAttr, XyzuResource.Style.Xyzu_Widget_Controls_RotaryKnob) { }
		public RotaryKnob(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Inflate(context, XyzuResource.Layout.xyzu_widget_control_rotaryknob, this);

			Title = FindViewById(XyzuResource.Id.xyzu_widget_control_rotaryknob_title_appcompattextview) as AppCompatTextView ??
				throw new InflateException("xyzu_widget_control_rotaryknob_title_appcompattextview");
			TitleBold = FindViewById(XyzuResource.Id.xyzu_widget_control_rotaryknob_titlebold_appcompattextview) as AppCompatTextView ??
				throw new InflateException("xyzu_widget_control_rotaryknob_titlebold_appcompattextview");
			Changer = FindViewById(XyzuResource.Id.xyzu_widget_control_rotaryknob_changer_seekarcview) as SeekArcView ??
				throw new InflateException("xyzu_widget_control_rotaryknob_changer_seekarcview");
			ValueStart = FindViewById(XyzuResource.Id.xyzu_widget_control_rotaryknob_valuestart_appcompattextview) as AppCompatTextView ??
				throw new InflateException("xyzu_widget_control_rotaryknob_valuestart_appcompattextview");
			Value = FindViewById(XyzuResource.Id.xyzu_widget_control_rotaryknob_value_appcompattextview) as AppCompatTextView ??
				throw new InflateException("xyzu_widget_control_rotaryknob_value_appcompattextview");
			ValueEnd = FindViewById(XyzuResource.Id.xyzu_widget_control_rotaryknob_valueend_appcompattextview) as AppCompatTextView ??
				throw new InflateException("xyzu_widget_control_rotaryknob_valueend_appcompattextview");

			TypedArray styledattributes = context.ObtainStyledAttributes(attrs, XyzuResource.Styleable.RotaryKnob);

			_Title = styledattributes.GetString(XyzuResource.Styleable.RotaryKnob_title);
			_TitleBold = styledattributes.GetString(XyzuResource.Styleable.RotaryKnob_titleBold);
			_TitleTooltip = styledattributes.GetString(XyzuResource.Styleable.RotaryKnob_titleTooltip);

			ValueStart.SetText(styledattributes.GetString(XyzuResource.Styleable.RotaryKnob_valueStart), null);
			ValueEnd.SetText(styledattributes.GetString(XyzuResource.Styleable.RotaryKnob_valueEnd), null);

			if (styledattributes.HasValue(XyzuResource.Styleable.RotaryKnob_size))
			{
				Changer.LayoutParameters ??= new LayoutParams(context, attrs);
				Changer.LayoutParameters.Width = 
				Changer.LayoutParameters.Height = 
				_Size = styledattributes.GetDimensionPixelSize(XyzuResource.Styleable.RotaryKnob_size, XyzuResource.Dimension.dp96);
			}

			Changer.AngleStart = styledattributes.GetInteger(XyzuResource.Styleable.RotaryKnob_seekArcAngleStart, Changer.AngleStart);
			Changer.AngleSweep = styledattributes.GetInteger(XyzuResource.Styleable.RotaryKnob_seekArcAngleSweep, Changer.AngleSweep);
			Changer.ArcWidth = styledattributes.GetDimensionPixelSize(XyzuResource.Styleable.RotaryKnob_seekArcArcWidth, Changer.ArcWidth);
			Changer.Clockwise = styledattributes.GetBoolean(XyzuResource.Styleable.RotaryKnob_seekArcClockwise, Changer.Clockwise);
			Changer.Progress = styledattributes.GetInteger(XyzuResource.Styleable.RotaryKnob_seekArcProgress, Changer.Progress);
			Changer.ProgressStart = styledattributes.GetInteger(XyzuResource.Styleable.RotaryKnob_seekArcProgressStart, Changer.ProgressStart);
			Changer.ProgressWidth = styledattributes.GetDimensionPixelSize(XyzuResource.Styleable.RotaryKnob_seekArcProgressWidth, Changer.ProgressWidth);
			Changer.Rotation = styledattributes.GetFloat(XyzuResource.Styleable.RotaryKnob_seekArcRotation, Changer.Rotation);
			Changer.Thumb = styledattributes.GetBoolean(XyzuResource.Styleable.RotaryKnob_seekArcWithThumb, false)
				? context.Resources?.GetDrawable(XyzuResource.Drawable.xyzu_widget_control_rotaryknob_seekarc_thumb, context.Theme) ?? Changer.Thumb
				: null;

			styledattributes.Recycle();

			SetValue(0.ToString());
			SetTitle(_Title, _TitleBold, _TitleTooltip);
		}

		protected int _Size;
		protected string? _Title;
		protected string? _TitleBold;
		protected string? _TitleTooltip;
		protected string? _ValueStart;
		protected string? _Value;
		protected string? _ValueEnd;

		public AppCompatTextView Title { get; }
		public AppCompatTextView TitleBold { get; }
		public SeekArcView Changer { get; }
		public AppCompatTextView ValueStart { get; }
		public AppCompatTextView Value { get; }
		public AppCompatTextView ValueEnd { get; }

		protected int _Min;
		protected int _Max;
		protected int? _Range;
		protected int _Progress;
		protected int _ProgressStart;

		public int Min 
		{
			get => _Min;
			set
			{
				_Min = value;
				_Range = null;
			}
		}
		public int Max 
		{
			get => _Max;
			set
			{
				_Max = value;
				_Range = null;
			}
		}
		public int Range 
		{
			get => _Range ??= true switch
			{
				true when Min < 0 && Max < 0 => (+Max) - (+Min),
				true when Min < 0 && Max >= 0 => (+Max) + (-Min),
				//true when Min > 0 && Max < 0 => 
				true when Min >= 0 && Max >= 0 => (+Max) - (+Min),
				_ => 0,
			};
		}
		public int Progress 
		{
			get => _Progress;
			set
			{
				_Progress = value;

				if (_Range is null)
				{
					Changer.Min = 0;
					Changer.Max = Range;
				}

				Changer.Progress = true switch
				{
					true when Min < 0 && _Progress < 0 => (-Min) - (-_Progress),
					true when Min < 0 && _Progress >= 0 => (-Min) + (+_Progress),
					//true when Min > 0 && _ProgressStart < 0 => 
					true when Min >= 0 && _Progress >= 0 => (+_Progress) - (+Min),
					_ => 0,
				};
			}
		}
		public int ProgressStart 
		{
			get => _ProgressStart;
			set
			{
				_ProgressStart = value;

				Changer.ProgressStart = true switch
				{
					true when Min < 0 && _ProgressStart < 0 => (-Min) - (-_ProgressStart),
					true when Min < 0 && _ProgressStart >= 0 => (-Min) + (+_ProgressStart),
					//true when Min > 0 && _ProgressStart < 0 => 
					true when Min >= 0 && _ProgressStart >= 0 => (+_ProgressStart) - (+Min),
					_ => 0,
				};
			}
		}

		public Func<int, int>? OnProgressSet { get; set; }
		public Action<SeekArcProgressChangedEventArgs>? OnProgress { get; set; }
		public Func<SeekArcProgressChangedEventArgs, string>? OnProgressValue { get; set; }

		public void SetProgress(int progress)
		{
			Progress = OnProgressSet?.Invoke(progress) ?? progress;

			ChangerProgressChanged(this, new SeekArcProgressChangedEventArgs(Changer, Progress, false));
		}
		public void SetTitle(string? title, string? titlebold, string? titletooltip = null)
		{
			Title.Text = _Title = title;
			TitleBold.Text = _TitleBold = titlebold;
			TitleBold.TooltipText = Title.TooltipText = _TitleTooltip = titletooltip;
			Title.Visibility = title is null ? ViewStates.Gone : ViewStates.Visible;
			TitleBold.Visibility = titlebold is null ? ViewStates.Gone : ViewStates.Visible;
		}
		public void SetValue(string? value)
		{
			Value.Text = _Value = value;
			Value.Visibility = value is null ? ViewStates.Gone : ViewStates.Visible;
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			Changer.ProgressChanged += ChangerProgressChanged;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			Changer.ProgressChanged -= ChangerProgressChanged;
		}

		private void ChangerProgressChanged(object? sender, SeekArcProgressChangedEventArgs args)
		{
			_Progress = args.Progress = Min + args.Progress;

			OnProgress?.Invoke(args);
			string? value = OnProgressValue?.Invoke(args);
			
			SetValue(value);
		}
	}
}