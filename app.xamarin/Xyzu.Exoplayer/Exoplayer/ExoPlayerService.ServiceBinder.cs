#nullable enable

using Android.Content;
using Android.OS;

namespace Xyzu.Player.Exoplayer
{
	public partial class ExoPlayerService
	{
		public class ServiceBinder : Binder, IPlayerServiceBinder
		{
			public ServiceBinder(ExoPlayerService exoplayerservice)
			{
				_ExoPlayerService = exoplayerservice;
			}

			private readonly ExoPlayerService _ExoPlayerService;

			public bool PreviouslyBound { get; set; }

			public IPlayerService PlayerService => _ExoPlayerService;
			public IServiceConnection? ServiceConnection { get; set; }
		}
	}
}