using System;

namespace AndroidX.Palette.Graphics
{
	public class PaletteAsyncListener : Java.Lang.Object, Palette.IPaletteAsyncListener
	{
		public Action<Palette?>? ActionOnGenerated { get; set; }

		public void OnGenerated(Palette? palette)
		{
			ActionOnGenerated?.Invoke(palette);
		}
	}
}