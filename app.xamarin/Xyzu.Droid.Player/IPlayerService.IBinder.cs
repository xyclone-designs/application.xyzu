#nullable enable

using Android.Content;

using System;

namespace Xyzu.Player
{
	public partial interface IPlayerService
	{
		public interface IBinder : IDisposable
		{
			bool PreviouslyBound { get; set; }

			IPlayerService PlayerService { get; }
			IServiceConnection? ServiceConnection { get; set; }
		}
	}
}