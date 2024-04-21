#nullable enable

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Widget;
using AndroidX.Preference;

using System;

using Xyzu.Droid;

using AndroidResource = Android.Resource;
using AndroidView = Android.Views.View;
using AndroidViewGroup = Android.Views.ViewGroup;
using static Android.Views.ViewGroupExtensions;

namespace JaredRummler.Android.ColorPicker
{
	[Register("com.jaredrummler/android/colorpicker/StyledColorPreferenceCompat")]
	public class StyledColorPreferenceCompat : ColorPreferenceCompat
	{
		public StyledColorPreferenceCompat(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public StyledColorPreferenceCompat(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
		public StyledColorPreferenceCompat(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override void OnBindViewHolder(PreferenceViewHolder holder)
		{
			base.OnBindViewHolder(holder);

			if (holder.ItemView is AndroidViewGroup viewgroup)
				foreach (AndroidView view in viewgroup.GetAllChildren())
					switch (view.Id)
					{
						case AndroidResource.Id.Title when
						view is TextView titletext:
							titletext.SetTextAppearance(Resource.Style.Xyzu_TextAppearance);
							titletext.Ellipsize = TextUtils.TruncateAt.Middle;
							titletext.SetMaxLines(1);
							titletext.SetSingleLine();
							titletext.SetTypeface(titletext.Typeface, TypefaceStyle.Bold);
							if (Context.Resources?.GetDimension(Resource.Dimension.abc_text_size_menu_material) is float titletextsize)
								titletext.SetTextSize(ComplexUnitType.Px, titletextsize);
							break;

						case AndroidResource.Id.Summary when
						view is TextView summarytext:
							summarytext.SetTextAppearance(Resource.Style.Xyzu_TextAppearance);
							summarytext.Ellipsize = TextUtils.TruncateAt.Middle;
							summarytext.SetMaxLines(4);
							break;

						default: break;
					}
		}
	}
}