#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;

using AndroidX.Preference;

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

			CurrentAlertDialog = XyzuUtils.Dialogs.Alert(
				context: Context,
				style: Resource.Style.Xyzu_Preference_AlertDialog,
				action: (dialogbuilder, dialog) =>
				{
					if (dialogbuilder is null)
						return;

					_ColorPicker = null;

					dialogbuilder.ProcessDialog(this);
					dialogbuilder.SetView(ColorPicker);
					dialogbuilder.SetPositiveButton(Resource.String.save, (sender, args) =>
					{
						CurrentColor = new Color(ColorPanel.Color = ColorPanel.BorderColor = ColorPicker.Color);

						CurrentAlertDialog?.Dismiss();

						CallChangeListener(CurrentColor.ToArgb());
					});

					DialogOnBuild?.Invoke(dialogbuilder);
				});

			CurrentAlertDialog.Show();
		}
		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			if (View?.ViewAdditionalItem != null)
			{
				_ColorPanel = null;

				View.ViewAdditionalItem.RemoveAllViews();
				View.ViewAdditionalItem.AddView(ColorPanel);
				View.ViewAdditionalItem.Visibility = ViewStates.Visible;
			}
		}
	}
}