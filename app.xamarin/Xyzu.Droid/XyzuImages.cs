using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.Views;
using AndroidX.Palette.Graphics;

using System;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Images.Enums;

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

		public Task<Bitmap?> GetBitmap(Operations[] operations, BitmapFactory.Options? options, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return Images.GetBitmap(operations, options, cancellationtoken, sources);
		}
		public Task<Drawable?> GetDrawable(Operations[] operations, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return Images.GetDrawable(operations, cancellationtoken, sources);
		}
		public Task<Palette?> GetPalette(CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return Images.GetPalette(cancellationtoken, sources);
		}
		public Task SetToImageView(Operations[] operations, ImageView? imageview, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return Images.SetToImageView(operations, imageview, oncomplete, cancellationtoken, sources);
		}
		public Task SetToViewBackground(Operations[] operations, View? view, Action<bool>? oncomplete, CancellationToken cancellationtoken = default, params object?[] sources)
		{
			return Images.SetToViewBackground(operations, view, oncomplete, cancellationtoken, sources);
		}
	}
}