#nullable enable

using Android.Support.V4.Media.Session;

using Com.Google.Android.Exoplayer2.Ext.Mediasession;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService
	{
		public const string MediaSessionTag = "XyzuMediaSession";

		private MediaSessionCompat? _MediaSession;
		private MediaSessionConnector? _MediaSessionConnector;
		
		public MediaSessionCompat MediaSession
		{
			get => _MediaSession ??= new MediaSessionCompat(this, MediaSessionTag)
			{
				Active = true,
			};
		}
		public MediaSessionConnector MediaSessionConnector
		{
			get
			{
				if (_MediaSessionConnector is null)
				{
					_MediaSessionConnector = new MediaSessionConnector(MediaSession);

					_MediaSessionConnector.SetPlayer(Exoplayer);
				}

				return _MediaSessionConnector;
			}
		}
	}
}