#nullable enable

using Android.App;

using System;

namespace Com.Google.Android.Exoplayer2.UI
{
	public class NotificationListener : Java.Lang.Object, PlayerNotificationManager.INotificationListener
	{
		public Action<int, bool>? ActionOnNotificationCancelled { get; set; }
		public Action<int, Notification, bool>? ActionOnNotificationPosted { get; set; }
		public Action<int, Notification>? ActionOnNotificationStarted { get; set; }

		public void OnNotificationCancelled(int notificationId, bool dismissedByUser)
		{
			ActionOnNotificationCancelled?.Invoke(notificationId, dismissedByUser);
		}
		public void OnNotificationPosted(int notificationId, Notification notification, bool ongoing)
		{
			ActionOnNotificationPosted?.Invoke(notificationId, notification, ongoing);
		}
		public void OnNotificationStarted(int notificationId, Notification notification)
		{
			ActionOnNotificationStarted?.Invoke(notificationId, notification);
		}
	}
}