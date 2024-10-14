using Android.App;
using Android.Content;
using AndroidX.Core.App;

namespace Xyzu.Library
{
	public partial interface IScanner
	{
		public class ServiceNotification
		{
			public ServiceNotification(int id, string channelid, string channelname)
			{
				Id = id;
				ChannelId = channelid;
				ChannelName = channelname;
				ContentTextsPreparing = new IContentTexts.Default();
				ContentTextsReading = new IContentTexts.Default();
				ContentTextsRemoving = new IContentTexts.Default();
				ContentTextsSaving = new IContentTexts.Default();
				ContentTextsScanning = new IContentTexts.Default();
			}

			public int Id { get; set; }
			public string ChannelId { get; set; }
			public string ChannelName { get; set; }
			public bool Built { get; set; }

			NotificationManager? _Manager;
			NotificationChannel? _Channel;
			NotificationCompat.Builder? _CompatBuilder;
			NotificationCompat.Action? _CompatActionCancel;
			NotificationCompat.BigTextStyle? _CompatBigTextStyle;

			public NotificationChannel Channel
			{
				set => _Channel = value;
				get
				{
					if (_Channel is null && Manager is not null && (_Channel = Manager.GetNotificationChannel(ChannelId)) is null)
						Manager.CreateNotificationChannel(_Channel = new NotificationChannel(ChannelId, ChannelName, NotificationImportance.Max));

					return _Channel ??= new NotificationChannel(ChannelId, ChannelName, NotificationImportance.Max);
				}
			}
			public NotificationManager? Manager
			{
				set => _Manager = value;
				get => _Manager ??= Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
			}

			public IContentTexts ContentTextsPreparing { get; set; }
			public IContentTexts ContentTextsReading { get; set; }
			public IContentTexts ContentTextsRemoving { get; set; }
			public IContentTexts ContentTextsSaving { get; set; }
			public IContentTexts ContentTextsScanning { get; set; }

			public NotificationCompat.Builder CompatBuilder
			{
				set => _CompatBuilder = value;
				get => _CompatBuilder ??= new NotificationCompat.Builder(Application.Context, ChannelId)
					.SetChannelId(Channel.Id ?? ChannelId)
					.SetPriority(NotificationCompat.PriorityDefault)
					.SetSilent(true);
			}
			public NotificationCompat.Action CompatActionCancel
			{
				set => _CompatActionCancel = value;
				get => _CompatActionCancel ??= new NotificationCompat.Action(
					icon: null,
					title: null as string,
					intent: PendingIntent.GetService(Application.Context, 0, new Intent(IntentActions.Destroy), PendingIntentFlags.CancelCurrent));
			}
			public NotificationCompat.BigTextStyle CompatBigTextStyle
			{
				set => _CompatBigTextStyle = value;
				get => _CompatBigTextStyle ??= new NotificationCompat.BigTextStyle();
			}

			public void Update(string? contenttext, string? subtext)
			{
				if (Manager is null)
					return;

				CompatBigTextStyle = CompatBigTextStyle
					.SetBigContentTitle(contenttext ?? string.Empty)
					.BigText(subtext ?? string.Empty);

				CompatBuilder = CompatBuilder
					.SetContentTitle(contenttext ?? string.Empty)
					.SetStyle(CompatBigTextStyle);

				Manager.Notify(Id, CompatBuilder.Build());
			}

			public Notification Build()
			{
				Built = true;

				return CompatBuilder
					.AddAction(CompatActionCancel)
					.Build();
			}
		}
	}
}