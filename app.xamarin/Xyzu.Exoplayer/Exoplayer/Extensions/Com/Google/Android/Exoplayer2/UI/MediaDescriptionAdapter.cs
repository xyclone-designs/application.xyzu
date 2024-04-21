#nullable enable

using Android.App;
using Android.Graphics;

using System;

namespace Com.Google.Android.Exoplayer2.UI
{
	public class MediaDescriptionAdapter : Java.Lang.Object, PlayerNotificationManager.IMediaDescriptionAdapter
	{
		public MediaDescriptionAdapter
			(PendingIntent defaultcontentintent, string defaultcontenttextformatted, string defaultcontenttitleformatted, string defaultsubtextformatted, Bitmap defaultlargeicon) : this
			(defaultcontentintent, new Java.Lang.String(defaultcontenttextformatted), new Java.Lang.String(defaultcontenttitleformatted), new Java.Lang.String(defaultsubtextformatted), defaultlargeicon) { }  
		public MediaDescriptionAdapter
			(PendingIntent defaultcontentintent, Java.Lang.ICharSequence defaultcontenttextformatted, Java.Lang.ICharSequence defaultcontenttitleformatted, Java.Lang.ICharSequence defaultsubtextformatted, Bitmap defaultlargeicon)
		{
			_DefaultContentIntent = defaultcontentintent;
			_DefaultContentTextFormatted = defaultcontenttextformatted;
			_DefaultContentTitleFormatted = defaultcontenttitleformatted;
			_DefaultSubTextFormatted = defaultsubtextformatted;
			_DefaultLargeIcon = defaultlargeicon;
		}

		private readonly PendingIntent _DefaultContentIntent;
		private readonly Java.Lang.ICharSequence _DefaultContentTextFormatted;
		private readonly Java.Lang.ICharSequence _DefaultContentTitleFormatted;
		private readonly Java.Lang.ICharSequence _DefaultSubTextFormatted;
		private readonly Bitmap _DefaultLargeIcon;

		public Func<IPlayer, PendingIntent?>? ActionCreateCurrentContentIntent { get; set; }
		public Func<IPlayer, string?>? ActionGetCurrentContentText { get; set; }
		public Func<IPlayer, Java.Lang.ICharSequence?>? ActionGetCurrentContentTextFormatted { get; set; }
		public Func<IPlayer, string?>? ActionGetCurrentContentTitle { get; set; }
		public Func<IPlayer, Java.Lang.ICharSequence?>? ActionGetCurrentContentTitleFormatted { get; set; }		  
		public Func<IPlayer, string?>? ActionGetCurrentSubText { get; set; }
		public Func<IPlayer, Java.Lang.ICharSequence?>? ActionGetCurrentSubTextFormatted { get; set; }
		public Func<IPlayer, PlayerNotificationManager.BitmapCallback, Bitmap?>? ActionGetCurrentLargeIcon { get; set; }

		public PendingIntent CreateCurrentContentIntent(IPlayer player)
		{
			return ActionCreateCurrentContentIntent?.Invoke(player) ?? _DefaultContentIntent;
		}
		public Java.Lang.ICharSequence GetCurrentContentTextFormatted(IPlayer player)
		{
			return
				ActionGetCurrentContentTextFormatted?.Invoke(player) ??
				(
					ActionGetCurrentContentText?.Invoke(player) is string currentcontenttext
						? new Java.Lang.String(currentcontenttext)
						: _DefaultContentTextFormatted
				);
		}
		public Java.Lang.ICharSequence GetCurrentContentTitleFormatted(IPlayer player)
		{
			return 
				ActionGetCurrentContentTitleFormatted?.Invoke(player) ??
				(
					ActionGetCurrentContentTitle?.Invoke(player) is string currentcontenttitle
						? new Java.Lang.String(currentcontenttitle)
						: _DefaultContentTitleFormatted
				);
		}
		public Java.Lang.ICharSequence GetCurrentSubTextFormatted(IPlayer player)
		{
			return
				ActionGetCurrentSubTextFormatted?.Invoke(player) ??
				(
					ActionGetCurrentSubText?.Invoke(player) is string currentsubtext
						? new Java.Lang.String(currentsubtext)
						: _DefaultSubTextFormatted
				);
		}
		public Bitmap GetCurrentLargeIcon(IPlayer player, PlayerNotificationManager.BitmapCallback bitmapcallback)
		{
			return ActionGetCurrentLargeIcon?.Invoke(player, bitmapcallback) ?? _DefaultLargeIcon;
		}
	}
}