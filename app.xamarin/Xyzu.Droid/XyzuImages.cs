using Android.Graphics;
using Android.Graphics.Drawables;

using System;
using System.Threading.Tasks;

using Xyzu.Images;

namespace Xyzu
{
	public sealed partial class XyzuImages : IImagesDroid
	{
		private XyzuImages(IImagesDroid images)
		{
			Images = images;
		}

		private static XyzuImages? _Instance;

		public static XyzuImages Instance
		{
			get => _Instance ?? throw new Exception("Instane is null. Init AppImages before use");
		}
		public static bool Inited => _Instance != null;

		public static void Init(IImagesDroid instance, Action<XyzuImages>? oninit = null)
		{
			_Instance = new XyzuImages(instance);

			oninit?.Invoke(_Instance);
		}

		public IImagesDroid Images { get; }

		public Task Operate(IImagesDroid.Parameters parameters)
		{
			return Images.Operate(parameters);
		}
		public Task SetToImageView(IImagesDroid.Parameters parameters)
		{
			return Images.SetToImageView(parameters);
		}
		public Task SetToViewBackground(IImagesDroid.Parameters parameters)
		{
			return Images.SetToViewBackground(parameters);
		}
		public Task<Bitmap?> GetBitmap(IImagesDroid.Parameters parameters)
		{
			return Images.GetBitmap(parameters);
		}
		public Task<Drawable?> GetDrawable(IImagesDroid.Parameters parameters)
		{
			return Images.GetDrawable(parameters);
		}
	}
}