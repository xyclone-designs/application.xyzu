using Android.App;
using Android.Content;
using Android.Widget;

using Xyzu.Droid;
using Xyzu.Activities;
using Xyzu.Player.Enums;

using PlayerIntents = Xyzu.Player.Intents;

namespace Xyzu.Widgets.HomeScreen
{
	public abstract class NowPlayingWidget : HomeScreenWidget
	{
		protected override RemoteViews OnRemoteViews(Context context)
		{
			RemoteViews remoteviews = base.OnRemoteViews(context);

			remoteviews.SetImageViewResource(
				viewId: Resource.Id.xyzu_widget_nowplaying_playpause_imagebutton,
				srcId: XyzuPlayer.Inited && XyzuPlayer.Instance.Player.State == PlayerStates.Playing
					? Resource.Drawable.icon_player_pause
					: Resource.Drawable.icon_player_play);

			remoteviews.SetOnClickPendingIntent(Resource.Id.xyzu_widget_nowplaying_repeat_imagebutton, PendingIntent.GetService(
				requestCode: 0,
				context: context,
				flags: PendingIntentFlags.UpdateCurrent,
				intent: new Intent(context, XyzuPlayer.ServiceType).SetAction(PlayerIntents.Actions.Repeat)));
			remoteviews.SetOnClickPendingIntent(Resource.Id.xyzu_widget_nowplaying_previous_imagebutton, PendingIntent.GetService(
				requestCode: 0,
				context: context,
				flags: PendingIntentFlags.UpdateCurrent,
				intent: new Intent(context, XyzuPlayer.ServiceType).SetAction(PlayerIntents.Actions.Previous)));
			remoteviews.SetOnClickPendingIntent(Resource.Id.xyzu_widget_nowplaying_playpause_imagebutton, PendingIntent.GetService(
				requestCode: 0,
				context: context,
				flags: PendingIntentFlags.UpdateCurrent,
				intent: new Intent(context, XyzuPlayer.ServiceType).SetAction(PlayerIntents.Actions.PlayPause)));
			remoteviews.SetOnClickPendingIntent(Resource.Id.xyzu_widget_nowplaying_next_imagebutton, PendingIntent.GetService(
				requestCode: 0,
				context: context,
				flags: PendingIntentFlags.UpdateCurrent,
				intent: new Intent(context, XyzuPlayer.ServiceType).SetAction(PlayerIntents.Actions.Next)));
			remoteviews.SetOnClickPendingIntent(Resource.Id.xyzu_widget_nowplaying_shuffle_imagebutton, PendingIntent.GetService(
				requestCode: 0,
				context: context,
				flags: PendingIntentFlags.UpdateCurrent,
				intent: new Intent(context, XyzuPlayer.ServiceType).SetAction(PlayerIntents.Actions.Shuffle)));
			remoteviews.SetOnClickPendingIntent(
				Resource.Id.xyzu_widget_nowplaying_container_relativelayout,
				PendingIntent.GetActivity(
					requestCode: 0,
					context: context,
					flags: PendingIntentFlags.UpdateCurrent,
					intent: XyzuPlayer.Inited is false 
						? new Intent(context, typeof(SplashScreenActivity))
							.PutExtra(LibraryActivity.IntentKeys.IsFromWidget, true)
						: XyzuSettings.Utils
							.MainActivityIntent(context, null)
							.PutExtra(LibraryActivity.IntentKeys.IsFromWidget, true)));

			return remoteviews;
		}
	}
}