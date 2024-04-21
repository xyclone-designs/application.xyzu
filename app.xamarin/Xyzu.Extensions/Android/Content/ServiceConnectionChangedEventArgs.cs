#nullable enable

using Android.OS;

using System;

namespace Android.Content
{
	public class ServiceConnectionChangedEventArgs : EventArgs
	{
		public enum Events
		{
			Connected,	
			Disconnected,	
		}

		public ServiceConnectionChangedEventArgs(Events @event)
		{
			Event = @event;
		}

		public Events Event { get; }
		public ComponentName? Name { get; set; }
		public IBinder? Binder { get; set; }
	}
}