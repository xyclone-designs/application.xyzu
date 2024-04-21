#nullable enable

using Android.Content;
using Android.Graphics;

using System;
using System.Linq;

namespace AndroidX.Palette.Graphics
{
	public static class PaletteExtensions
	{
		public static Color? GetColorForBackground(this Palette palette, Context context, int resId)
		{
			if (context.Resources?.GetColor(resId, context.Theme) is Color backgroundcolor)
				return palette.GetColorForBackground(backgroundcolor);

			return new Color?();
		}
		public static Color? GetColorForBackground(this Palette palette, Color backgroundcolor)
		{
			double[] uicolors = Enumerable.Empty<double>()
				.Append(backgroundcolor.R / 255)
				.Append(backgroundcolor.G / 255)
				.Append(backgroundcolor.B / 255)
				.Select(color =>
				{
					return color <= 0.03928
						? color / 12.92
						: Math.Pow((color + 0.055) / 1.055, 2.4);

				}).ToArray();

			double lightness = (0.2126 * uicolors[0]) + (0.7152 * uicolors[1]) + (0.0722 * uicolors[2]);
			int? rgb = (lightness > 0.179)
				? (palette.DarkVibrantSwatch?.Rgb ?? palette.DarkMutedSwatch?.Rgb ?? palette.DominantSwatch?.Rgb)
				: (palette.LightVibrantSwatch?.Rgb ?? palette.LightMutedSwatch?.Rgb ?? palette.DominantSwatch?.Rgb);

			return rgb.HasValue
				? new Color(rgb.Value)
				: new Color?();
		}
	}
}