using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.Views;
using AndroidX.Palette.Graphics;

using Bumptech.Glide.Load.Engine;
using Bumptech.Glide;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Images;
using Xyzu.Images.Enums;

namespace Bumptech.Glide
{
	public static class RequestBuilderExtensions
	{
		public static RequestBuilder RunOperations(this RequestBuilder requestbuilder, Operations[] operations, Action<RequestBuilder, Operations> operate)
		{
			requestbuilder.SetDiskCacheStrategy(DiskCacheStrategy.All);

			foreach (Operations operation in operations)
				operate(requestbuilder, operation);

			return requestbuilder;
		}
	}
}