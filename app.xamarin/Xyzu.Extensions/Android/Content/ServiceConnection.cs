#nullable enable

using Android.OS;

using System;

namespace Android.Content
{
	public class ServiceConnection : Java.Lang.Object, IServiceConnection
	{
		public Action<ComponentName?, IBinder?>? OnServiceConnectedAction { get; set; }
		public Action<ComponentName?>? OnServiceDisconnectedAction { get; set; }

		public void OnServiceConnected(ComponentName? name, IBinder? service)
		{
			OnServiceConnectedAction?.Invoke(name, service);
		}
		public void OnServiceDisconnected(ComponentName? name)
		{
			OnServiceDisconnectedAction?.Invoke(name);
		}
	}
}