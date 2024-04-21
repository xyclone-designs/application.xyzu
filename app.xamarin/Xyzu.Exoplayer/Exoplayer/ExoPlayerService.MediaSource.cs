#nullable enable

using Com.Google.Android.Exoplayer2.Extractor;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Util;

using System;
using System.Linq;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService
	{
		public const long SilenceMediaSourceDurationMS = 1000;

		private ConcatenatingMediaSource? _ConcatenatingMediaSource;
		private DefaultDataSourceFactory? _DataSourceFactory;
		private DefaultExtractorsFactory? _ExtractorsFactory;
		private ProgressiveMediaSource.Factory? _ProgressiveMediaSourceFactory;

		public ConcatenatingMediaSource ConcatenatingMediaSource 
		{
			get => _ConcatenatingMediaSource ??= new ConcatenatingMediaSource(
				isAtomic: false, 
				useLazyPreparation: true,
				mediaSources: Array.Empty<IMediaSource>(),
				shuffleOrder: new ShuffleOrderUnshuffledShuffleOrder(0));
		}							 
		public DefaultDataSourceFactory DataSourceFactory 
		{
			get => _DataSourceFactory ??= new DefaultDataSourceFactory(
				context: this,
				userAgent: Util.GetUserAgent(this, PackageName ?? string.Empty));
		}							   
		public DefaultExtractorsFactory ExtractorsFactory 
		{
			get => _ExtractorsFactory ??= new DefaultExtractorsFactory()
				.SetConstantBitrateSeekingEnabled(true);
		}
		public ProgressiveMediaSource.Factory ProgressiveMediaSourceFactory 
		{
			get => _ProgressiveMediaSourceFactory ??= new ProgressiveMediaSource.Factory(
				dataSourceFactory: DataSourceFactory,
				extractorsFactory: ExtractorsFactory);
		}
		public SilenceMediaSource SilenceMediaSource
		{
			get => new SilenceMediaSource(SilenceMediaSourceDurationMS); 
		}
	}
}