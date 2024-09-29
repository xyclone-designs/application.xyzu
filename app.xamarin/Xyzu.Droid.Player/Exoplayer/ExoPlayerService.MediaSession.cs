#nullable enable

using System;

using MediaSession = AndroidX.Media3.Session.MediaSession;
using MediaSessionCallback = AndroidX.Media3.Session.MediaSessionCallback;
using MediaSessionCompat = Android.Support.V4.Media.Session.MediaSessionCompat;
using MediaButtonReceiver = AndroidX.Media3.Session.MediaButtonReceiver;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService
	{
		public const string MediaSessionTag = "XyzuMediaSession";

		private MediaSession? _MediaSession;
		private MediaSessionCompat? _MediaSessionCompat;
		private MediaSessionCallback? _MediaSessionCallback;
		private MediaButtonReceiver? _MediaButtonReceiver;
		
		public MediaSession MediaSession
		{
			get => _MediaSession ??= 
				new MediaSession.Builder(this, Exoplayer)
					.SetPeriodicPositionUpdateEnabledBuilder(true)?
					.SetCallback(MediaSessionCallback)?
					.Build() ?? 
				throw new Exception("ExoPlayerService.MediaSession");
		}
		public MediaSessionCompat MediaSessionCompat
		{
			get => _MediaSessionCompat ??= 
				new MediaSessionCompat(this, MediaSessionTag) { Active = true } ??
				throw new Exception("ExoPlayerService.MediaSessionCompat");
		}
		public MediaSessionCallback MediaSessionCallback
		{
			get => _MediaSessionCallback ??= new MediaSessionCallback { };
		}
		public MediaButtonReceiver MediaButtonReceiver
		{
			get => _MediaButtonReceiver ??= new MediaButtonReceiver { };
		}

		public override MediaSession? OnGetSession(MediaSession.ControllerInfo? p0)
		{
			return MediaSession;
		}
	}
}