using Android.Content;
using AndroidX.Media3.Common.Util;
using AndroidX.Media3.DataSource;
using AndroidX.Media3.ExoPlayer.Source;
using AndroidX.Media3.Extractor;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService
	{
		public const long SilenceMediaSourceDurationMS = 1_000;
	
		private BasicDataSourceFactory? _DataSourceFactory;
		private DefaultExtractorsFactory? _ExtractorsFactory;
		private ProgressiveMediaSource.Factory? _ProgressiveMediaSourceFactory;
			 
		public BasicDataSourceFactory DataSourceFactory 
		{
			get => _DataSourceFactory ??= new BasicDataSourceFactory(
				context: this,
				userAgent: Util.GetUserAgent(this, PackageName ?? string.Empty));
		}							   
		public DefaultExtractorsFactory ExtractorsFactory 
		{
			get => _ExtractorsFactory ??= 
				new DefaultExtractorsFactory()
					.SetConstantBitrateSeekingEnabled(true) ?? 
				new DefaultExtractorsFactory();
		}
		public ProgressiveMediaSource.Factory ProgressiveMediaSourceFactory 
		{
			get => _ProgressiveMediaSourceFactory ??= new ProgressiveMediaSource.Factory(
				dataSourceFactory: DataSourceFactory,
				extractorsFactory: ExtractorsFactory);
		}
		public static SilenceMediaSource SilenceMediaSource
		{
			get => new (SilenceMediaSourceDurationMS); 
		}

		public class BasicDataSourceFactory : Java.Lang.Object, IDataSourceFactory
		{
			public BasicDataSourceFactory(Context context, string? userAgent)
			{
				Context = context;
				UserAgent = userAgent;
			}

			public Context Context { get; set; } 
			public string? UserAgent { get; set; } 
			

			public IDataSource? CreateDataSource()
			{
				return new DefaultDataSource(Context, UserAgent, true);
			}
		}
	}
}