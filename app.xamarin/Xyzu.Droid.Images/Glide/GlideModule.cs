using Android.Content;

using Bumptech.Glide;
using Bumptech.Glide.Load;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Load.Engine.Cache;
using Bumptech.Glide.Module;
using Bumptech.Glide.Request;

using Kdd.Glide.AppModuleInjector;

using System.Threading.Tasks;

using Xamarin.Essentials;

using BumptechGlide = Bumptech.Glide.Glide;

namespace Xyzu.Images.Glide
{
	public class GlideModule : AppGlideModule
	{
		public GlideModule(Context context, GlideBuilder glidebuilder)
		{
			Context = context;

			GlideAppModuleInjector.Inject(this);
			ApplyOptions(Context, glidebuilder);
			BumptechGlide.Init(Context, glidebuilder);

			Glide = BumptechGlide.Get(Context);
		}

		public BumptechGlide Glide { get; }
		public Context Context { get; set; }
		public IDiskCacheFactory? DiskCacheFactory { get; set; }
		public IMemoryCache? MemoryCache { get; set; }
		public RequestOptions? Options { get; set; }

		public override void ApplyOptions(Context context, GlideBuilder glidebuilder)
		{
			base.ApplyOptions(context, glidebuilder);

			DiskCacheFactory ??= new InternalCacheDiskCacheFactory(context);
			MemoryCache ??= new LruResourceCache(1024 * 1024 * 100); // 100MB
			Options ??= new RequestOptions()
				.Format(DecodeFormat.PreferRgb565)
				.FitCenter()
				.EncodeQuality(100)
				.SetDiskCacheStrategy(DiskCacheStrategy.Data)
				.SkipMemoryCache(false);

			glidebuilder.SetDefaultRequestOptions(Options);
			glidebuilder.SetDiskCache(DiskCacheFactory);
			glidebuilder.SetMemoryCache(MemoryCache);
		}
		public override void RegisterComponents(Context context, BumptechGlide glide, Registry registry)
		{
			base.RegisterComponents(context, glide, registry);

			glide.ClearDiskCache();
			MainThread.BeginInvokeOnMainThread(() => glide.ClearMemory());
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				Task.Run(() =>
				{
					Glide.ClearDiskCache();
					MainThread.BeginInvokeOnMainThread(() => Glide.ClearMemory());
				});

			base.Dispose(disposing);
		}
	}
}