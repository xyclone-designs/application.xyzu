using Abak.Tr.Com.BoxedVerticalSeekBar;

using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;

using System;

using Xyzu.Droid;

namespace Xyzu.Widgets.Controls
{
	[Register("xyzu/widgets/controls/AudioBand")]
	public class AudioBand : ConstraintLayout, BoxedVertical.IOnValuesChangeListener
	{
		public static class Ids
		{
			public const int Layout = Resource.Layout.xyzu_widget_control_audioband;

			public const int Title_AppCompatTextView = Resource.Id.xyzu_widget_control_audioband_title_appcompattextview;
			public const int Max_AppCompatTextView = Resource.Id.xyzu_widget_control_audioband_max_appcompattextview;
			public const int ValueContainer_LinearLayoutCompat = Resource.Id.xyzu_widget_control_audioband_valuecontainer_linearlayoutcompat;
			public const int Value_BoxedVertical = Resource.Id.xyzu_widget_control_audioband_value_boxedvertical;
			public const int Min_AppCompatTextView = Resource.Id.xyzu_widget_control_audioband_min_appcompattextview;
			public const int Footer_AppCompatTextView = Resource.Id.xyzu_widget_control_audioband_footer_appcompattextview;
		}

		public AudioBand(Context context) : this(context, null!) { }
		public AudioBand(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Style.Xyzu_Widget_Controls_AudioBand) { }
		public AudioBand(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Inflate(context, Ids.Layout, this);

			_Title = FindViewById(Ids.Title_AppCompatTextView) as AppCompatTextView;
			_Max = FindViewById(Ids.Max_AppCompatTextView) as AppCompatTextView;
			_ValueContainer = FindViewById(Ids.ValueContainer_LinearLayoutCompat) as LinearLayoutCompat;
			_Value = FindViewById(Ids.Value_BoxedVertical) as BoxedVertical;
			_Min = FindViewById(Ids.Min_AppCompatTextView) as AppCompatTextView;
			_Footer = FindViewById(Ids.Footer_AppCompatTextView) as AppCompatTextView;

			TypedArray styledattributes = context.ObtainStyledAttributes(attrs, Resource.Styleable.AudioBand);

			ValueContainer.LayoutChange += ValueContainerLayoutChange;
			Value.ListenerOnValuesChange = this;
			Value.Value = styledattributes.GetInteger(Resource.Styleable.AudioBand_value, 0);
			Footer.SetText(styledattributes.GetString(Resource.Styleable.AudioBand_footer), null);
			Max.SetText(styledattributes.GetString(Resource.Styleable.AudioBand_max), null);
			Min.SetText(styledattributes.GetString(Resource.Styleable.AudioBand_min), null);
			Title.SetText(styledattributes.GetString(Resource.Styleable.AudioBand_title), null);
			ValueToFooter = styledattributes.GetBoolean(Resource.Styleable.AudioBand_valueToFooter, true);
			ValueToTitle = styledattributes.GetBoolean(Resource.Styleable.AudioBand_valueToTitle, false);
			ValueToFooter = true;
			styledattributes.Recycle();

			TextChanged(Footer, new TextChangedEventArgs(Footer.Text, 0, 0, 0));
			TextChanged(Min, new TextChangedEventArgs(Min.Text, 0, 0, 0));
			TextChanged(Max, new TextChangedEventArgs(Max.Text, 0, 0, 0));
			TextChanged(Title, new TextChangedEventArgs(Title.Text, 0, 0, 0));
		}

		public bool _ValueToFooter;
		public bool _ValueToTitle;
		private AppCompatTextView? _Title;
		private AppCompatTextView? _Max;
		private LinearLayoutCompat? _ValueContainer;
		private BoxedVertical? _Value;
		private AppCompatTextView? _Min;
		private AppCompatTextView? _Footer;

		public bool ValueToFooter
		{
			get => _ValueToFooter;
			set => _ValueToFooter = value;
		}
		public bool ValueToTitle
		{
			get => _ValueToTitle;
			set => _ValueToTitle = value;
		}
		public AppCompatTextView Title
		{
			get => _Title ?? throw new InflateException();
		}
		public AppCompatTextView Max
		{
			get => _Max ?? throw new InflateException();
		}							
		public LinearLayoutCompat ValueContainer
		{
			get => _ValueContainer ?? throw new InflateException();
		}							
		public BoxedVertical Value
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

		public Action<object?, int>? OnProgress { get; set; }
		public Func<int, string>? OnProgressValue { get; set; }

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			Title.TextChanged += TextChanged;
			Min.TextChanged += TextChanged;
			Max.TextChanged += TextChanged;
			Footer.TextChanged += TextChanged;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			Title.TextChanged -= TextChanged;
			Min.TextChanged -= TextChanged;
			Max.TextChanged -= TextChanged;
			Footer.TextChanged -= TextChanged;
		}

		private void TextChanged(object? sender, TextChangedEventArgs args)
		{
			if (sender is AppCompatTextView appcompattextview)
				appcompattextview.Visibility = appcompattextview.Text is null ? ViewStates.Gone : ViewStates.Visible;
		}
		private void ValueContainerLayoutChange(object? sender, LayoutChangeEventArgs args)
		{			
			static bool AllZero(int top, int left, int right, int bottom)
			{
				return
					top == 0 &&
					left == 0 &&
					right == 0 &&
					bottom == 0;
			}

			if (AllZero(args.OldTop, args.OldLeft, args.OldRight, args.OldBottom) && AllZero(args.Top, args.Left, args.Right, args.Bottom) is false)
			{
				Value.LayoutParameters ??= new LayoutParams(ValueContainer.Width, ValueContainer.Height);
				Value.LayoutParameters.Width = ValueContainer.Width;
				Value.LayoutParameters.Height = ValueContainer.Height;
				ValueContainer.LayoutChange -= ValueContainerLayoutChange;
			}
		}

		public void SetValue(int value)
		{
			Value.Value = value;
			OnProgress?.Invoke(this, value);

			string _value = OnProgressValue?.Invoke(value) ?? value.ToString();

			if (ValueToFooter is true) Footer.SetText(_value, null);
			if (ValueToTitle is true) Title.SetText(_value, null);
		}

		public void OnPointsChanged(BoxedVertical boxedPoints, int points) 
		{
			OnProgress?.Invoke(boxedPoints, points);

			string _value = OnProgressValue?.Invoke(points) ?? points.ToString();

			if (ValueToFooter is true) Footer.SetText(_value, null);
			if (ValueToTitle is true) Title.SetText(_value, null);
		}
		public void OnStartTrackingTouch(BoxedVertical boxedPoints) { }
		public void OnStopTrackingTouch(BoxedVertical boxedPoints) { }

		public class OnProgressEventArgs : EventArgs
		{
			public bool FromUser { get; set; }
			public int Progress { get; set; }
		}
	}
}