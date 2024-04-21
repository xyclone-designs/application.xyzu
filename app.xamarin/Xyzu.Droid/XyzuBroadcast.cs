#nullable enable

using Android.Content;

using System;
using System.Collections.Generic;

using Exception = System.Exception;

namespace Xyzu
{
	public sealed partial class XyzuBroadcast
	{
		private XyzuBroadcast(Context context) 
		{
			Context = context;
			ReceiveActions = new Dictionary<string, Action<object, OnReceiveEventArgs>>();
			BroadcastReceiver = new Receiver
			{
				OnReceiveAction = (context, intent) =>
				{
					foreach (Action<object, OnReceiveEventArgs> receiveaction in ReceiveActions.Values)
						receiveaction.Invoke(this, new OnReceiveEventArgs(context, intent));
				}
			};

			Context.RegisterReceiver(BroadcastReceiver, Intents.IntentFilter);
		}																 
		
		private static XyzuBroadcast? _Instance;

		public static XyzuBroadcast Instance
		{
			get => _Instance ?? throw new Exception("Init before use");
		}
		public static void Init(Context context, Action<XyzuBroadcast>? oninit = null)
		{
			_Instance = new XyzuBroadcast(context) { };

			oninit?.Invoke(_Instance);
		}

		private Context Context { get; set; }
		private Receiver BroadcastReceiver { get; set; }

		public IDictionary<string, Action<object, OnReceiveEventArgs>> ReceiveActions { get; set; }
	}
}