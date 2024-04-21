#nullable enable

using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View;

using Google.Android.Material.Dialog;

using System;

using Xyzu.Droid;

namespace AndroidX.AppCompat.Widget
{
	[Register("androidx/appcompat/widget/AppCompatEditDate")]
	public class AppCompatEditDate : AppCompatEditText
	{
		public AppCompatEditDate(Context context) : base(context)
		{
			Init(null);
		}
		public AppCompatEditDate(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(attrs);
		}
		public AppCompatEditDate(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(attrs);
		}
		public AppCompatEditDate(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		protected void Init(IAttributeSet? attrs) 
		{
			TypedArray? styledattributes = Context?.ObtainStyledAttributes(attrs, Resource.Styleable.AppCompatEditDate);

			_DateFormat = styledattributes?.GetString(Resource.Styleable.AppCompatEditDate_dateFormat);
			_DateDefaultText = styledattributes?.GetString(Resource.Styleable.AppCompatEditDate_dateDefaultText);

			styledattributes?.Recycle();
		}
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			Click += OnViewClick;
		}
		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();

			Click -= OnViewClick;
		}

		private DateTime? _Date;
		private string? _DateFormat;
		private string? _DateDefaultText;

		public DateTime? Date
		{
			get => _Date;
			set
			{
				_Date = value;

				SetText(_Date?.ToString(_DateFormat ?? "dd MMMM yyyy") ?? _DateDefaultText, null);

				OnDateChanged?.Invoke(this, _Date);
				OnDateChangedAction?.Invoke(_Date);
			}
		}
		public string? DateFormat
		{
			get => _DateFormat;
			set
			{
				_DateFormat = value;

				if (_Date.HasValue)
					SetText(_Date.Value.ToString(_DateFormat ?? "dd MMMM yyyy"), null);
			}
		}
		public string? DateDefaultText
		{
			get => _DateDefaultText;
			set
			{
				_DateDefaultText = value;

				if (_Date is null)
					SetText(_DateDefaultText, null);
			}
		}

		public DatePicker? AlbumReleaseDatePicker { get; set; }
		public AlertDialog? AlbumReleaseDateDialog { get; set; }

		public event EventHandler<DateTime?>? OnDateChanged;
		public Action<DateTime?>? OnDateChangedAction { get; set; }

		public IDialogInterfaceOnClickListener? NegativeButtonListener { get; set; }
		public EventHandler<DialogClickEventArgs>? NegativeButtonEvent { get; set; }
		public IDialogInterfaceOnClickListener? PositiveButtonListener { get; set; }
		public EventHandler<DialogClickEventArgs>? PositiveButtonEvent { get; set; }

		public void SetDate(DateTime? date)
		{
			Date = date;
		}
		public void SetDate(DateTime? date, string? dateformat)
		{
			_DateFormat = dateformat;

			SetDate(date);
		}

		private void OnViewClick(object sender, EventArgs e)
		{
			AlbumReleaseDatePicker ??= new DatePicker(new ContextThemeWrapper(Context, Resource.Style.Xyzu_DatePicker));
			AlbumReleaseDatePicker.DateTime = Date ?? default;

			if (AlbumReleaseDateDialog is null && Context != null)
			{
				AlertDialog.Builder? alertdialogbuilder = new MaterialAlertDialogBuilder(Context, Resource.Style.Xyzu_Material_Dialog_Alert)
					.SetCancelable(true)?
					.SetView(AlbumReleaseDatePicker);

				if (NegativeButtonListener != null) alertdialogbuilder?.SetNegativeButton(Resource.String.cancel, NegativeButtonListener);
				else if (NegativeButtonEvent != null) alertdialogbuilder?.SetNegativeButton(Resource.String.cancel, NegativeButtonEvent);
				else alertdialogbuilder?.SetNegativeButton(Resource.String.cancel, (sender, args) => AlbumReleaseDateDialog?.Dismiss());		   

				if (PositiveButtonListener != null) alertdialogbuilder?.SetPositiveButton(Resource.String.select, PositiveButtonListener);
				else if (PositiveButtonEvent != null) alertdialogbuilder?.SetPositiveButton(Resource.String.select, PositiveButtonEvent);
				else alertdialogbuilder?.SetPositiveButton(Resource.String.select, (sender, args) =>
				{
					Date = AlbumReleaseDatePicker.DateTime;

					AlbumReleaseDateDialog?.Dismiss();
				});

				AlbumReleaseDateDialog = alertdialogbuilder?.Create();
			}

			AlbumReleaseDateDialog?.Dismiss();
			AlbumReleaseDateDialog?.Show();
		}
	}
}