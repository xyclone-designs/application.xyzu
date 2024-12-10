using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.Widget;

using System.Threading.Tasks;

using Xyzu.Droid;
using Xyzu.Images;

namespace Xyzu.Widgets.HomeScreen.NowPlaying
{
	[Service(Exported = true)]
	[BroadcastReceiver(
		Exported = true,
		Label = "@string/xyzu_widget_nowplaying_shelf_label",
		Description = "@string/xyzu_widget_nowplaying_shelf_description" )]
	[IntentFilter(new string[] { AppWidgetManager.ActionAppwidgetUpdate })]
	[MetaData(AppWidgetManager.MetaDataAppwidgetProvider, Resource = "@xml/provider_appwidget_nowplaying_shelf")]
	public class NowPlayingShelfWidget : NowPlayingWidget
	{
		public override int LayoutRes { get => Resource.Layout.xyzu_widget_nowplaying_shelf; }

		protected override RemoteViews OnRemoteViews(Context context)
		{
			RemoteViews remoteviews = base.OnRemoteViews(context);

			string? detailonetwo = null;
			Bitmap? blur = null, artwork = null;

			switch (true)
			{
				case true when XyzuPlayer.Inited is false:
				case true when XyzuPlayer.Instance.Player.Queue.CurrentSong is null:
					detailonetwo = context.GetString(Resource.String.player_default_detail_onetwo_alt);
					break;

				default:
					detailonetwo = string.Format(
						"{0} - {1}",
						XyzuPlayer.Instance.Player.Queue.CurrentSong.Title ?? context.GetString(Resource.String.player_unknown_title),
						XyzuPlayer.Instance.Player.Queue.CurrentSong.Artist ?? context.GetString(Resource.String.player_unknown_artist));

					artwork = Task.Run(async () => await XyzuImages.Instance.GetBitmap(
						parameters: new IImagesDroid.Parameters(XyzuPlayer.Instance.Player.Queue.CurrentSong)
						{
							CancellationToken = default,
							Operations = IImages.DefaultOperations.Downsample,

						})).GetAwaiter().GetResult();
					blur = Task.Run(async () => await XyzuImages.Instance.GetBitmap(
						parameters: new IImagesDroid.Parameters(XyzuPlayer.Instance.Player.Queue.CurrentSong)
						{
							CancellationToken = default,
							Operations = IImages.DefaultOperations.BlurDownsample,

						})).GetAwaiter().GetResult();
					break;
			}

			remoteviews.SetTextViewText(Resource.Id.xyzu_widget_nowplaying_detail_onetwo_textview, detailonetwo);
			remoteviews.SetImageViewBitmap(Resource.Id.xyzu_widget_nowplaying_blur_imageview, blur);
			remoteviews.SetImageViewBitmap(Resource.Id.xyzu_widget_nowplaying_artwork_imageview, artwork);

			return remoteviews;
		}
	}
}