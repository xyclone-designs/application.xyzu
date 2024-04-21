#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;

using JaredRummler.Android.ColorPicker;

using System;

using Xyzu.Droid;

namespace Xyzu.Preference
{
	[Register("xyzu/preference/ColorPickerPreference")]
	public class ColorPickerPreference : DialogPreference 
	{
		public ColorPickerPreference(Context context) : base(context)
		{
			Init(context, null);
		}
		public ColorPickerPreference(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}
		public ColorPickerPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Init(context, attrs);
		}
		public ColorPickerPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Init(context, attrs);
		}
		public ColorPickerPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		private ColorPanelView? _ColorPanel;
		private ColorPickerView? _ColorPicker;

		ColorPanelView ColorPanel
		{
			set => _ColorPanel = value;
			get => _ColorPanel ??= new ColorPanelView(Context)
			{
				Color = CurrentColor.ToArgb(),
				BorderColor = CurrentColor.ToArgb(),
				Shape = IColorShape.Circle,
				LayoutParameters = new ViewGroup.MarginLayoutParams(48, 48)
				{
					MarginEnd = 16,
					TopMargin = 16,
					MarginStart = 16,
					BottomMargin = 16,
				},
			};
		}
		ColorPickerView ColorPicker
		{
			set => _ColorPicker = value;
			get
			{
				if (_ColorPicker is null)
				{
					_ColorPicker = new ColorPickerView(Context) { Color = CurrentColor.ToArgb() };
					_ColorPicker.SetAlphaSliderVisible(true);
				}

				return _ColorPicker;
			}
		}
		public Color CurrentColor { get; set; }

		protected override void OnClick()
		{
			// base.OnClick();

			if (PreferenceUtils.StyledAlertDialog(Context, this) is AlertDialog.Builder alertdialogbuilder)
			{
				_ColorPicker = null;

				alertdialogbuilder.SetView(ColorPicker);
				alertdialogbuilder.SetPositiveButton(Resource.String.save, (sender, args) =>
				{
					CurrentColor = new Color(ColorPanel.Color = ColorPanel.BorderColor = ColorPicker.Color);

					CurrentAlertDialog?.Dismiss();

					CallChangeListener(CurrentColor.ToArgb());
				});

				DialogOnBuild?.Invoke(alertdialogbuilder);

				(CurrentAlertDialog = alertdialogbuilder.Create())
					.Show();
			}
		}
		public override void OnBindViewHolder(AndroidX.Preference.PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			if (holder.FindViewById(Resource.Id.xyzu_preference_preference_additionalitem_contentframelayout) is ContentFrameLayout contentframelayout)
			{
				_ColorPanel = null;

				contentframelayout.RemoveAllViews();
				contentframelayout.AddView(ColorPanel);
			}
		}
	}
}