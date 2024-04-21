#nullable enable

using System;

namespace Android.Content
{
	public class OnReceiveEventArgs : EventArgs
	{
		public OnReceiveEventArgs(Context? context, Intent? intent)
		{
			Context = context;
			Intent = intent;
		}

		public Context? Context { get; }
		public Intent? Intent { get; }
	}
}