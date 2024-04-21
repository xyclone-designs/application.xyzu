#nullable enable

using Android.Content;

using System;

namespace Xyzu.Player
{
	public interface IPlayerServiceBinder : IDisposable
	{
		bool PreviouslyBound { get; set; }

		IPlayerService PlayerService { get; }
		IServiceConnection? ServiceConnection { get; set; }
	}
}