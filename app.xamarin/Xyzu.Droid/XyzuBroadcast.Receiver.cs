using Android.App;
using Android.Content;
using System;

namespace Xyzu
{
	public sealed partial class XyzuBroadcast
	{
		[BroadcastReceiver(
			Description = null,
			DirectBootAware = true,
			Enabled = true,
			Exported = true,
			Icon = null,
			Label = "Xyzu Intent Broadcast Receiver",
			Name = "co.za.xyclonedesigns.xyzu.XyzuIntentBroadcastReceiver",
			Permission = null,
			Process = null,
			RoundIcon = null)]
		[IntentFilter(new string[]
		{
		 	Intents.Actions.MediaChecking,
		 	Intents.Actions.MediaMounted,
		 	Intents.Actions.MediaScannerFinished,
		 	Intents.Actions.MediaScannerStarted,
		 	Intents.Actions.VolumeChanged,
		})]
		public class Receiver : BroadcastReceiver
		{
			public Action<Context?, Intent?>? OnReceiveAction { get; set; }

			public event EventHandler<OnReceiveEventArgs>? Receive;

			public override void OnReceive(Context? context, Intent? intent)
			{
				OnReceiveAction?.Invoke(context, intent);
				Receive?.Invoke(this, new OnReceiveEventArgs(context, intent));
			}
		}

		public static class Intents
		{
			public static IntentFilter IntentFilter
			{
				get
				{
					IntentFilter intentfilter = new ();

					intentfilter.AddAction(Actions.MediaChecking);
					intentfilter.AddAction(Actions.MediaMounted);
					intentfilter.AddAction(Actions.MediaScannerFinished);
					intentfilter.AddAction(Actions.MediaScannerStarted);
					intentfilter.AddAction(Actions.VolumeChanged);

					intentfilter.AddDataScheme(DataSchemes.File);

					return intentfilter;
				}
			}

			public static class Actions
			{
				public const string MediaChecking = Intent.ActionMediaChecking;
				public const string MediaMounted = Intent.ActionMediaMounted;
				public const string MediaScannerFinished = Intent.ActionMediaScannerFinished;
				public const string MediaScannerStarted = Intent.ActionMediaScannerStarted;
				public const string VolumeChanged = "android.media.VOLUME_CHANGED_ACTION";
			}
			public static class DataSchemes
			{
				public const string File = "file";
			}
		}
	}
}