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
		Label = "@string/xyzu_widget_nowplaying_box_label",
		Description = "@string/xyzu_widget_nowplaying_box_description")]
	[IntentFilter(new string[] { AppWidgetManager.ActionAppwidgetUpdate })]
	[MetaData(AppWidgetManager.MetaDataAppwidgetProvider, Resource = "@xml/provider_appwidget_nowplaying_box")]
	public class NowPlayingBoxWidget : NowPlayingWidget
	{
		public override int LayoutRes { get => Resource.Layout.xyzu_widget_nowplaying_box; }

		protected override RemoteViews OnRemoteViews(Context context)
		{
			RemoteViews remoteviews = base.OnRemoteViews(context);

			Bitmap? artwork = null;
			string? detailone = null, detailtwo = null;

			switch (true)
			{
				case true when XyzuPlayer.Inited is false:
				case true when XyzuPlayer.Instance.Player.Queue.CurrentSong is null:
					detailone = context.GetString(Resource.String.player_default_detail_one_alt);
					detailtwo = context.GetString(Resource.String.player_default_detail_two_alt);
					break;

				default:
					detailone =
						XyzuPlayer.Instance.Player.Queue.CurrentSong.Title ??
						context.GetString(Resource.String.player_unknown_title);
					detailtwo = string.Format(
						"{0} - {1}",
						XyzuPlayer.Instance.Player.Queue.CurrentSong.Artist ?? context.GetString(Resource.String.player_unknown_artist),
						XyzuPlayer.Instance.Player.Queue.CurrentSong.Album ?? context.GetString(Resource.String.player_unknown_album));
					artwork = Task.Run(async () => await XyzuImages.Instance.GetBitmap(
						parameters: new IImagesDroid.Parameters(XyzuPlayer.Instance.Player.Queue.CurrentSong)
						{
							CancellationToken = default,
							Operations = IImages.DefaultOperations.Downsample,

						})).GetAwaiter().GetResult();

					break;
			}

			remoteviews.SetTextViewText(Resource.Id.xyzu_widget_nowplaying_detail_one_textview, detailone);
			remoteviews.SetTextViewText(Resource.Id.xyzu_widget_nowplaying_detail_two_textview, detailtwo);
			remoteviews.SetImageViewBitmap(Resource.Id.xyzu_widget_nowplaying_artwork_imageview, artwork);

			return remoteviews;
		}
	}
}