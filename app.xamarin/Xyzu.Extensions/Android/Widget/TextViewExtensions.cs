#nullable enable

using Android.Graphics.Drawables;

namespace Android.Widget
{
	public static class TextViewExtensions
	{
		public static void SetDrawableBottom(this TextView textview, Drawable? drawable, bool resetall = false)
		{
			Drawable[] drawables = textview.GetCompoundDrawablesRelative();

			Drawable? start = resetall ? null : drawables[0];
			Drawable? top = resetall ? null : drawables[1];
			Drawable? end = resetall ? null : drawables[2];

			textview.SetCompoundDrawablesRelativeWithIntrinsicBounds(start, top, end, drawable);
		}
		public static void SetDrawableEnd(this TextView textview, Drawable? drawable, bool resetall = false)
		{
			Drawable[] drawables = textview.GetCompoundDrawablesRelative();

			Drawable? start = resetall ? null : drawables[0];
			Drawable? top = resetall ? null : drawables[1];
			Drawable? bottom = resetall ? null : drawables[3];

			textview.SetCompoundDrawablesRelativeWithIntrinsicBounds(start, top, drawable, bottom);
		}
		public static void SetDrawableLeft(this TextView textview, Drawable? drawable, bool resetall = false)
		{
			Drawable[] drawables = textview.GetCompoundDrawables();

			Drawable? top = resetall ? null : drawables[1];
			Drawable? right = resetall ? null : drawables[2];
			Drawable? bottom = resetall ? null : drawables[3];

			textview.SetCompoundDrawablesWithIntrinsicBounds(drawable, top, right, bottom);
		}
		public static void SetDrawableRight(this TextView textview, Drawable? drawable, bool resetall = false)
		{
			Drawable[] drawables = textview.GetCompoundDrawables();

			Drawable? left = resetall ? null : drawables[0];
			Drawable? top = resetall ? null : drawables[1];
			Drawable? bottom = resetall ? null : drawables[3];

			textview.SetCompoundDrawablesWithIntrinsicBounds(left, top, drawable, bottom);
		}
		public static void SetDrawableStart(this TextView textview, Drawable? drawable, bool resetall = false)
		{
			Drawable[] drawables = textview.GetCompoundDrawablesRelative();

			Drawable? top = resetall ? null : drawables[1];
			Drawable? end = resetall ? null : drawables[2];
			Drawable? bottom = resetall ? null : drawables[3];

			textview.SetCompoundDrawablesRelativeWithIntrinsicBounds(drawable, top, end, bottom);
		}
		public static void SetDrawableTop(this TextView textview, Drawable? drawable, bool resetall = false)
		{
			Drawable[] drawables = textview.GetCompoundDrawablesRelative();

			Drawable? start = resetall ? null : drawables[0];
			Drawable? end = resetall ? null : drawables[2];
			Drawable? bottom = resetall ? null : drawables[3];

			textview.SetCompoundDrawablesRelativeWithIntrinsicBounds(start, drawable, end, bottom);
		}
	}
}