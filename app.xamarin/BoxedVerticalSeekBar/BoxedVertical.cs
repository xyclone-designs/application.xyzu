
/**
 * Created by alpaslanbak on 29/09/2017.
 * Modified by Nick Panagopoulos @npanagop on 12/05/2018.
 * Modified & translated by Thando Ndlovu
 */

using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;

using System;

using BoxedVerticalSeekBar;

namespace Abak.Tr.Com.BoxedVerticalSeekBar
{
	[Register("abak/tr/com/boxedverticalseekbar/BoxedVertical")]
    public class BoxedVertical : View
    {
		private const string TAG = nameof(BoxedVertical);

		public BoxedVertical(Context context) : this(context, null) { }
		public BoxedVertical(Context context, IAttributeSet? attrs) : this(context, attrs, default) { }
        public BoxedVertical(Context context, IAttributeSet? attrs, int defStyle) : base(context, attrs, defStyle)
		{
			_ValueDefault = _ValueMax / 2;
			_TextSize = (int)(_TextSize * context.Resources?.DisplayMetrics?.Density ?? 1);
			// _ColorText = ContextCompat.GetColor(context, Resource.Color.color_text);
			// _ColorProgress = ContextCompat.getColor(context, Resource.Color.color_progress);
			// _ColorBackground = ContextCompat.getColor(context, Resource.Color.color_background);

			TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.BoxedVertical, 0, 0);

			Enabled = a.GetBoolean(Resource.Styleable.BoxedVertical_enabled, Enabled);

			_ColorText = a.GetColor(Resource.Styleable.BoxedVertical_textColor, _ColorText);
			_ColorProgress = a.GetColor(Resource.Styleable.BoxedVertical_progressColor, _ColorProgress);
			_ColorBackground = a.GetColor(Resource.Styleable.BoxedVertical_backgroundColor, _ColorBackground);
			_CornerRadius = a.GetDimensionPixelSize(Resource.Styleable.BoxedVertical_libCornerRadius, _CornerRadius);
			_Value = a.GetInt(Resource.Styleable.BoxedVertical_points, _Value);
			_ValueMax = a.GetInt(Resource.Styleable.BoxedVertical_max, _ValueMax);
			_ValueMin = a.GetInt(Resource.Styleable.BoxedVertical_min, _ValueMin);
			_ValueDefault = a.GetInt(Resource.Styleable.BoxedVertical_defaultValue, _ValueDefault);
			_ProgressStep = a.GetInt(Resource.Styleable.BoxedVertical_step, _ProgressStep);
			_TextSize = (int)a.GetDimension(Resource.Styleable.BoxedVertical_textSize, _TextSize);
			_TextEnabled = a.GetBoolean(Resource.Styleable.BoxedVertical_textEnabled, _TextEnabled);
			_TextBottomPadding = a.GetInt(Resource.Styleable.BoxedVertical_textBottomPadding, _TextBottomPadding);
			_TouchDisabled = a.GetBoolean(Resource.Styleable.BoxedVertical_touchDisabled, _TouchDisabled);

			if (a.GetBoolean(Resource.Styleable.BoxedVertical_imageEnabled, false))
			{
				ImageDefault = (a.GetDrawable(Resource.Styleable.BoxedVertical_defaultImage) as BitmapDrawable)?.Bitmap
					?? throw new InflateException("When images are enabled, defaultImage can not be null. Please assign a drawable in the layout XML file");
				ImageMin = (a.GetDrawable(Resource.Styleable.BoxedVertical_minImage) as BitmapDrawable)?.Bitmap
					?? throw new InflateException("When images are enabled, minImage can not be null. Please assign a drawable in the layout XML file");
				ImageMax = (a.GetDrawable(Resource.Styleable.BoxedVertical_maxImage) as BitmapDrawable)?.Bitmap
					?? throw new InflateException("When images are enabled, maxImage can not be null. Please assign a drawable in the layout XML file");
			}

			Value = ValueDefault;

			a.Recycle();

			// range check
			Value = (Value > ValueMax) ? ValueMax : Value;
			Value = (Value < ValueMin) ? ValueMin : Value;

			_PaintProgress = new Paint
			{
				AntiAlias = true,
				Color = _ColorProgress,
			};
			_PaintText = new Paint
			{
				AntiAlias = true,
				TextSize = _TextSize,
				Color = _ColorText,
			};

			_PaintText.SetStyle(Paint.Style.Fill);
			_PaintProgress.SetStyle(Paint.Style.Stroke);
		}

		private int _CornerRadius = 10;
		private Color _ColorBackground = Color.Orange;
		private Color _ColorProgress = Color.Blue;
		private Color _ColorText = Color.Red;
		private bool _FirstRun = true;
		private Paint _PaintProgress;
		private Paint _PaintText;
		private int _ProgressStep = 10;
		private int _ProgressSweep = 00;
		private int _SrcWidth;
		private int _SrcHeight;
		private int _CanvasWidth;
		private int _CanvasHeight;
		private int _Value = default;
		private int _ValueMin = 000;
		private int _ValueMax = 100;
		private int _ValueDefault = 50;
		private bool _TouchDisabled = true;
		private int _TextSize = 26;
		private bool _TextEnabled = true;
		private int _TextBottomPadding = 20;

		private bool ImageEnabled { get; set; }
		private Bitmap? ImageMin { get; set; }
		private Bitmap? ImageMax { get; set; }
		private Bitmap? ImageDefault { get; set; }
		public int CornerRadius
		{
			get => _CornerRadius;
			set 
			{
				_CornerRadius = value;

				Invalidate();
			}
		}
		public bool TextEnabled
		{
			get => _TextEnabled;
			set => _TextEnabled = value;
		}
		public int ProgressStep
        {
            get => _ProgressStep;
            set => _ProgressStep = value;
		}
		public int Value
		{
			get => _Value;
			set
			{
				_Value = value;
				_Value = (_Value > _ValueMax) ? _ValueMax : _Value;
				_Value = (_Value < _ValueMin) ? _ValueMin : _Value;

				//convert min-max range to progress
				_ProgressSweep = (_Value - ValueMin) * _SrcHeight / (ValueMax - ValueMin);
				//reverse value because progress is descending
				_ProgressSweep = _SrcHeight - _ProgressSweep;

				ListenerOnValuesChange?.OnPointsChanged(this, _Value);

				Invalidate();
			}
		}
		public int ValueMin
		{
			get => _ValueMin;
			set => _ValueMin = value;
		}
		public int ValueMax
		{
			get => _ValueMax;
			set => _ValueMax = value;
		}
		public int ValueDefault
		{
			get => _ValueDefault;
			set => _ValueDefault = value;
		}
		public IOnValuesChangeListener? ListenerOnValuesChange { get; set; }

		private double ConvertTouchEventPoint(float yPos)
		{
			float wReturn;

			if (yPos > (_SrcHeight * 2))
			{
				wReturn = _SrcHeight * 2;
				return wReturn;
			}
			else if (yPos < 0)
			{
				wReturn = 0;
			}
			else
			{
				wReturn = yPos;
			}

			return wReturn;
		}
		private void DrawText(Canvas canvas, Paint paint, string text)
		{
			Rect dRect = new();
			canvas.GetClipBounds(dRect);
			int cWidth = dRect.Width();
			paint.TextAlign = Paint.Align.Left;
			paint.GetTextBounds(text, 0, text.Length, dRect);
			float x = cWidth / 2f - dRect.Width() / 2f - dRect.Left;
			canvas.DrawText(text, x, canvas.Height - _TextBottomPadding, paint);
		}
		private void DrawIcon(Bitmap bitmap, Canvas canvas)
		{
			canvas.DrawBitmap( 
				src: null,
				paint: null,
				bitmap: GetResizedBitmap(bitmap, canvas.Width / 2, canvas.Width / 2), 
				dst: new RectF(
					bottom: canvas.Height,
					top: canvas.Height - bitmap.Height,
					right: (canvas.Width / 3) + bitmap.Width,
					left: (canvas.Width / 2) - (bitmap.Width / 2)));
		}
		private Bitmap GetResizedBitmap(Bitmap bm, int newHeight, int newWidth) 
		{
			//Thanks Piyush
			int width = bm.Width;
			int height = bm.Height;
			float scaleWidth = ((float)newWidth) / width;
			float scaleHeight = ((float)newHeight) / height;
			// create a matrix for the manipulation
			Matrix matrix = new ();
			matrix.PostScale(scaleWidth, scaleHeight);
			return Bitmap.CreateBitmap(bm, 0, 0, width, height, matrix, false);
		}
		private void UpdateProgress(int progress)
		{
			_ProgressSweep = progress;

			progress = (progress > _CanvasHeight) ? _CanvasHeight : progress;
			progress = (progress < 0) ? 0 : progress;

			//convert progress to min-max range
			Value = progress * (_ValueMax - _ValueMin) / _CanvasHeight + _ValueMin;
			//reverse value because progress is descending
			Value = _ValueMax + _ValueMin - Value;
			//if value is not max or min, apply step
			if (Value != _ValueMax && Value != _ValueMin)
				Value = Value - (Value % ProgressStep) + (_ValueMin % ProgressStep);

			ListenerOnValuesChange?.OnPointsChanged(this, Value);

			Invalidate();
		}
		private void UpdateOnTouch(MotionEvent @event)
		{
			if (_TouchDisabled) 
				return;

			Pressed = true;
			double mTouch = ConvertTouchEventPoint(@event.GetY());
			int progress = (int)Math.Round(mTouch);
			UpdateProgress(progress);
		}

		protected override void OnDraw(Canvas canvas) 
		{
			Path mPath = new ();

			_CanvasWidth = canvas.Width;
			_CanvasHeight = canvas.Height;

			_ProgressSweep = (_Value - ValueMin) * _CanvasHeight / (ValueMax - ValueMin);
			_ProgressSweep = _CanvasHeight - _ProgressSweep;

			if (Path.Direction.Cw is not null)
				mPath.AddRoundRect(new RectF(0, 0, _CanvasWidth, _CanvasHeight), _CornerRadius, _CornerRadius, Path.Direction.Cw);

			Paint paint = new()
			{
				Alpha = 255,
				AntiAlias = true,
				Color = _ColorBackground,
			};

			canvas.ClipPath(mPath);
			canvas.Translate(0, 0);
			canvas.DrawRect(0, 0, _CanvasWidth, _CanvasHeight, paint);
			canvas.DrawLine(_CanvasWidth / 2, (_CanvasHeight / 2) - 1, _CanvasWidth / 2, _ProgressSweep == 0 ? _ProgressSweep + 1 : _ProgressSweep, _PaintProgress);

			if (ImageEnabled && ImageDefault != null && ImageMin != null && ImageMax != null)
			{
				//If image is enabled, text will not be shown
				if (Value == _ValueMax)
				{
					DrawIcon(ImageMax, canvas);
				}
				else if (Value == _ValueMin)
				{
					DrawIcon(ImageMin, canvas);
				}
				else
				{
					DrawIcon(ImageDefault, canvas);
				}
			}
			else
			{
				//If image is disabled and text is enabled show text
				if (_TextEnabled)
					DrawText(canvas, _PaintText, Value.ToString());
			}

			if (_FirstRun)
			{
				_FirstRun = false;
				Value = Value;
			}
		}
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) 
		{
			if (_SrcWidth <= 0) _SrcWidth = LayoutParameters?.Width ?? 0;
			if (_SrcWidth <= 0) _SrcWidth = MeasuredWidth;
			if (_SrcWidth <= 0) _SrcWidth = GetDefaultSize(SuggestedMinimumWidth, widthMeasureSpec);

			if (_SrcHeight <= 0) _SrcHeight = LayoutParameters?.Height ?? 0;
			if (_SrcHeight <= 0) _SrcHeight = MeasuredHeight;
			if (_SrcHeight <= 0) _SrcHeight = GetDefaultSize(SuggestedMinimumHeight, heightMeasureSpec);

			if (_PaintProgress is not null)
				_PaintProgress.StrokeWidth = _SrcWidth;

			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
		}
		public override bool OnTouchEvent(MotionEvent? @event)
		{
			if (Enabled is false)
				return false;

			Parent?.RequestDisallowInterceptTouchEvent(true);

			switch (@event?.Action)
			{
				case MotionEventActions.Down:
					ListenerOnValuesChange?.OnStartTrackingTouch(this); 
					UpdateOnTouch(@event);
					break;
				case MotionEventActions.Move:
					UpdateOnTouch(@event);
					break;
				case MotionEventActions.Up:
					ListenerOnValuesChange?.OnStopTrackingTouch(this);
					Pressed = false;
					Parent?.RequestDisallowInterceptTouchEvent(false);
					break;
				case MotionEventActions.Cancel:
					ListenerOnValuesChange?.OnStopTrackingTouch(this);
					Pressed = false;
					Parent?.RequestDisallowInterceptTouchEvent(false);
					break;
				default: break;
			}

			return true;
		}

		public interface IOnValuesChangeListener
		{
			void OnPointsChanged(BoxedVertical boxedPoints, int points);
			void OnStartTrackingTouch(BoxedVertical boxedPoints);
			void OnStopTrackingTouch(BoxedVertical boxedPoints);
		}
	}
}
