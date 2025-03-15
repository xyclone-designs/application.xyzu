using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;

using System;

namespace SeekArc.Droid
{
    /**
     * 
     * 
     * This is a class that functions much like a SeekBar but
     * follows a circle path instead of a straight line.
     * 
     * @author Neil Davies
     * 
     * edited by Thando Ndlovu
     * 
     */
    public class SeekArcView : View
    {
        private const string TAG = "SeekArc";
        private static readonly int InvalidProgressValue = -1;
        private const int MAngleOffset = -90;

        private int ArcRadius;
		private RectF ArcRect = new();
		private float ProgressSweep;
		private int TranslateX;
        private int TranslateY;
        private int ThumbXPos;
        private int ThumbYPos;
        private double? TouchAngle;
        private float TouchIgnoreRadius;

		public SeekArcView(Context context) : this(context, null, 0) { }
		public SeekArcView(Context context, IAttributeSet? attrs) : this(context, attrs, Resource.Attribute.seekArcStyle) { }
		public SeekArcView(Context context, IAttributeSet? attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Log.Debug(TAG, "Initialising SeekArc");

			if (context.Resources?.DisplayMetrics?.Density is float density)
				ProgressWidth = (int)(ProgressWidth * density);

			TypedArray typedarray = context.ObtainStyledAttributes(attrs, Resource.Styleable.SeekArcView, defStyle, 0);

			_AngleStart = typedarray.GetInt(Resource.Styleable.SeekArcView_startAngle, _AngleStart);
			_AngleSweep = typedarray.GetInt(Resource.Styleable.SeekArcView_sweepAngle, _AngleSweep);
			_ArcWidth = (int)typedarray.GetDimension(Resource.Styleable.SeekArcView_arcWidth, _ArcWidth);
			_ArcRotation = typedarray.GetInt(Resource.Styleable.SeekArcView_rotation, _ArcRotation);
			_Clockwise = typedarray.GetBoolean(Resource.Styleable.SeekArcView_clockwise, _Clockwise);
			_HasRoundedEdges = typedarray.GetBoolean(Resource.Styleable.SeekArcView_roundEdges, _HasRoundedEdges);
			_IsTouchInsideEnabled = typedarray.GetBoolean(Resource.Styleable.SeekArcView_touchInside, _IsTouchInsideEnabled);
			_Min = typedarray.GetInt(Resource.Styleable.SeekArcView_min, _Min);
			_Max = typedarray.GetInt(Resource.Styleable.SeekArcView_max, _Max);
			_Progress = typedarray.GetInteger(Resource.Styleable.SeekArcView_progress, _Progress);
			_ProgressStart = typedarray.GetInteger(Resource.Styleable.SeekArcView_progressStart, _Min);
			_ProgressWidth = (int)typedarray.GetDimension(Resource.Styleable.SeekArcView_progressWidth, _ProgressWidth);

			Thumb = typedarray.GetDrawable(Resource.Styleable.SeekArcView_thumb) ?? context.Resources?.GetDrawable(Resource.Drawable.seek_arc_control_selector, context.Theme);
			ArcPaint.Color = typedarray.GetColor(
				index: Resource.Styleable.SeekArcView_arcColor,
				defValue: context.Resources?.GetColor(Resource.Color.progress_gray, context.Theme) ?? Color.Transparent);
			ProgressPaint.Color = typedarray.GetColor(
				index: Resource.Styleable.SeekArcView_progressColor,
				defValue: context.Resources?.GetColor(Android.Resource.Color.HoloBlueLight, context.Theme) ?? Color.Transparent);

			_Progress = (_Progress > _Max) ? _Max : _Progress;
			_Progress = (_Progress < _Min) ? _Min : _Progress;

			_AngleSweep = (_AngleSweep > 360) ? 360 : _AngleSweep;
			_AngleSweep = (_AngleSweep < 0) ? 0 : _AngleSweep;

			_AngleStart = (_AngleStart > 360) ? 0 : _AngleStart;
			_AngleStart = (_AngleStart < 0) ? 0 : _AngleStart;

			HasRoundedEdges = _HasRoundedEdges;

			typedarray.Recycle();
		}

		public event EventHandler<SeekArcProgressChangedEventArgs>? ProgressChanged;
        public event EventHandler<SeekArcTrackingTouchEventArgs>? StartTrackingTouch;
        public event EventHandler<SeekArcTrackingTouchEventArgs>? StopTrackingTouch;

		private int _AngleStart = 000;
		private int _AngleSweep = 360;
		private int _ArcRotation = 0;
		private int _ArcWidth = 2;
		private bool _Clockwise = true;
		private bool _HasRoundedEdges;
		private bool _IsTouchInsideEnabled = true;
		private int _Min = 000;
		private int _Max = 100;
		private int? _MinMaxRange = null;
		private int _Progress = 000;
		private Paint? _ArcPaint;
		private Paint? _ProgressPaint;
		private int _ProgressWidth = 4;
		private int? _ProgressStart = null;
		private Drawable? _Thumb;

		public int AngleStart
		{
			get => _AngleStart;
			set
			{
				_AngleStart = value;

				UpdateThumbPosition();
			}
		}
		public int AngleSweep
		{
			get => _AngleSweep;
			set
			{
				_AngleSweep = value;

				UpdateThumbPosition();
			}
		}
		public Paint ArcPaint
		{
			set => _ArcPaint = value;
			get
			{
				if (_ArcPaint is null)
				{
					_ArcPaint = new Paint
					{
						AntiAlias = true,
						StrokeWidth = _ProgressWidth,
						StrokeCap = HasRoundedEdges ? Paint.Cap.Round : Paint.Cap.Square,
					};

					_ArcPaint.SetStyle(Paint.Style.Stroke);
				}

				return _ArcPaint;
			}
		}
		public int ArcRotation
		{
			get => _ArcRotation;
			set
			{
				_ArcRotation = value;

				UpdateThumbPosition();
			}
		}
		public int ArcWidth
		{
			get => _ArcWidth;
			set
			{
				_ArcWidth = value;
				ArcPaint.StrokeWidth = value;
			}
		}
		public bool Clockwise
		{
			get => _Clockwise; 
			set => _Clockwise = value;
		}
		public bool HasRoundedEdges
		{
			get => _HasRoundedEdges; 
			set
			{
				_HasRoundedEdges = value;
				ArcPaint.StrokeCap = ProgressPaint.StrokeCap = _HasRoundedEdges
					? Paint.Cap.Round
					: Paint.Cap.Square;
			}
		}
		public bool IsTouchInsideEnabled
		{
			get => _IsTouchInsideEnabled; 
			set
			{
				_IsTouchInsideEnabled = value;
				TouchIgnoreRadius = _IsTouchInsideEnabled
					? ArcRadius / 4
					: ArcRadius - Math.Min(Thumb?.IntrinsicWidth / 2 ?? 0, Thumb?.IntrinsicHeight / 2 ?? 0);
			}
		}
		public int Min
		{
			get => _Min; 
			set
			{
				if (value < 0) throw new ArgumentException("Min cannot be less than 0");
                _Min = value;
				_MinMaxRange = null;
			}
		}
		public int Max
		{
			get => _Max; 
			set
			{
                _Max = value;
                _MinMaxRange = null;
			}
		}
		public int MinMaxRange
		{
			get => _MinMaxRange ??= (_Max - _Min);
		}
		public int Progress
		{
			get => _Progress;
			set => UpdateProgress(value, false);
		}
		public Paint ProgressPaint
		{			
			set => _ProgressPaint = value;
			get			
			{
				if (_ProgressPaint is null)
				{
					_ProgressPaint = new Paint
					{
						AntiAlias = true,
						StrokeWidth = _ProgressWidth,
						StrokeCap = HasRoundedEdges ? Paint.Cap.Round : Paint.Cap.Square,
					};

					_ProgressPaint.SetStyle(Paint.Style.Stroke);
				}

				return _ProgressPaint;
			}
		}
		public int ProgressStart
		{
			get => _ProgressStart ?? Min;
			set => _ProgressStart = value;
		}
		public int ProgressWidth
		{
			get => _ProgressWidth; 
			set
			{
				_ProgressWidth = value;
				ProgressPaint.StrokeWidth = value;
			}
		}
		public Drawable? Thumb
		{
			get => _Thumb;
			set
			{
				_Thumb = value;

				if (_Thumb is not null)
				{
					int thumbHalfWidth = _Thumb.IntrinsicWidth / 2, thumbHalfheight = _Thumb.IntrinsicHeight / 2;

					_Thumb.SetBounds(-thumbHalfWidth, -thumbHalfheight, thumbHalfWidth, thumbHalfheight);
				}
			}
		}

		public override bool OnTouchEvent(MotionEvent? motionEvent)
		{
			switch (motionEvent?.Action)
			{
				case MotionEventActions.Down:
					StartTrackingTouch?.Invoke(this, new SeekArcTrackingTouchEventArgs(this));
					UpdateOnTouch(motionEvent);
					break;
				case MotionEventActions.Move:
					UpdateOnTouch(motionEvent);
					break;
				case MotionEventActions.Up:
					StopTrackingTouch?.Invoke(this, new SeekArcTrackingTouchEventArgs(this));
					Pressed = false;
					break;
				case MotionEventActions.Cancel:
					StopTrackingTouch?.Invoke(this, new SeekArcTrackingTouchEventArgs(this)); 
					Pressed = false;
					break;
			}

			return true;
		}

		protected override void DrawableStateChanged()
		{
			base.DrawableStateChanged();
			
			if (Thumb is not null && Thumb.IsStateful && GetDrawableState() is int[] state)
				Thumb.SetState(state);

			Invalidate();
		}
		protected override void OnDraw(Canvas canvas)
		{
			if (!_Clockwise)
				canvas.Scale(-1, 1, ArcRect.CenterX(), ArcRect.CenterY());

			float sweeparcstart = AngleStart + MAngleOffset + ArcRotation, progressarcstart, progressarcsweep;

			if (ProgressStart == Min)
				(progressarcstart, progressarcsweep) = (sweeparcstart, ProgressSweep);
			else
			{
				float progressstart = ProgressStart + Min;
				float arcstart = progressstart / MinMaxRange * AngleSweep;

				progressarcstart = arcstart + sweeparcstart;
				progressarcsweep = TouchAngle is null ? 0 : (float)TouchAngle.Value - progressarcstart + sweeparcstart;
			}

			canvas.DrawArc(ArcRect, sweeparcstart, AngleSweep, false, ArcPaint);
			canvas.DrawArc(ArcRect, progressarcstart, progressarcsweep, false, ProgressPaint);

			if (ProgressStart == Progress || (int)progressarcsweep == 0)
			{
				if (ThumbXPos == 0 && ThumbYPos == 0)
					canvas.DrawPoint(ThumbXPos, ThumbYPos, ProgressPaint);
				else 
					canvas.DrawArc(ArcRect, progressarcstart, 1, false, ProgressPaint);
			}

			if (Thumb is not null)
			{
				canvas.Translate(TranslateX - ThumbXPos, TranslateY - ThumbYPos);
				Thumb.Draw(canvas);
			}
		}
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var height = GetDefaultSize(SuggestedMinimumHeight, heightMeasureSpec);
            var width = GetDefaultSize(SuggestedMinimumWidth, widthMeasureSpec);
            int arcDiameter = Math.Min(width, height) - PaddingLeft;
            float top = height / 2 - (arcDiameter / 2);
            float left = width / 2 - (arcDiameter / 2);

			ArcRadius = arcDiameter / 2;
			ArcRect.Set(left, top, left + arcDiameter, top + arcDiameter);
			TranslateX = (int)(width * 0.5f);
			TranslateY = (int)(height * 0.5f);

			UpdateThumbPosition();

			IsTouchInsideEnabled = _IsTouchInsideEnabled;

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

		private int GetProgressForAngle(double angle)
		{
			float valueperdegree = (float)MinMaxRange / AngleSweep;
			int touchProgress = (int)Math.Round(valueperdegree * angle);

			touchProgress = (touchProgress < 0) ? InvalidProgressValue : touchProgress;
			touchProgress = (touchProgress > MinMaxRange) ? InvalidProgressValue : touchProgress;

			return touchProgress;
		}
		private double GetTouchDegrees(float xPos, float yPos)
        {
            float x = xPos - TranslateX, y = yPos - TranslateY;
            double arcangle = ConvertToDegrees(Math.Atan2(y, _Clockwise ? x : -x) + (Math.PI / 2) - ConvertToRadians(_ArcRotation));

			arcangle = arcangle < 0 ? 360 + arcangle : arcangle;
			arcangle -= _AngleStart;

            return arcangle;
        }
		private bool ShouldIgnoreTouch(float xPos, float yPos)
		{
			float x = xPos - TranslateX, y = yPos - TranslateY;
			float touchRadius = (float)Math.Sqrt((x * x) + (y * y));
			
			return touchRadius < TouchIgnoreRadius;
		}
		private void UpdateOnTouch(MotionEvent motionEvent)
		{
			if (ShouldIgnoreTouch(motionEvent.GetX(), motionEvent.GetY()))
				return;

			Pressed = true;
			TouchAngle = GetTouchDegrees(motionEvent.GetX(), motionEvent.GetY());
			
			UpdateProgress(GetProgressForAngle(TouchAngle.Value), true);
		}
		private void UpdateProgress(int progress, bool fromUser)
        {
            if (progress == InvalidProgressValue)
                return;

			_Progress = progress;
			_Progress = _Progress > Max ? Max : _Progress;
            _Progress = _Progress < Min ? Min : _Progress;

            ProgressSweep = (float)_Progress / MinMaxRange * _AngleSweep;

            UpdateThumbPosition();

            Invalidate();

			ProgressChanged?.Invoke(this, new SeekArcProgressChangedEventArgs(this, _Progress, fromUser));
		}
		private void UpdateThumbPosition()
		{
			int angle = ProgressStart == Min
				? (int)ProgressSweep + AngleStart + ArcRotation + 90
				: (ProgressStart + Min) / MinMaxRange * AngleSweep;

			ThumbXPos = (int)(ArcRadius * Math.Cos(ConvertToRadians(angle)));
			ThumbYPos = (int)(ArcRadius * Math.Sin(ConvertToRadians(angle)));
		}

		public static double ConvertToDegrees(double val)
		{
			return val * (180.0 / Math.PI);
		}
		public static double ConvertToRadians(double angle)
		{
			return (Math.PI / 180) * angle;
		}
	}
}
