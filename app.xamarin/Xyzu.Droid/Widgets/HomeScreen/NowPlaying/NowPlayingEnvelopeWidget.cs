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
		Label = "@string/xyzu_widget_nowplaying_envelope_label",
		Description = "@string/xyzu_widget_nowplaying_envelope_description")]
	[IntentFilter(new string[] { AppWidgetManager.ActionAppwidgetUpdate })]
	[MetaData(AppWidgetManager.MetaDataAppwidgetProvider, Resource = "@xml/provider_appwidget_nowplaying_envelope")]
	public class NowPlayingEnvelopeWidget : NowPlayingWidget
	{
		public override int LayoutRes { get => Resource.Layout.xyzu_widget_nowplaying_envelope; }

		protected override RemoteViews OnRemoteViews(Context context)
		{
			RemoteViews remoteviews = base.OnRemoteViews(context);

			Bitmap? blur = null, artwork = null;
			string? detailone = null, detailtwo = null, detailthree = null, detailfour = null;

			switch (true)
			{
				case true when XyzuPlayer.Inited is false:
				case true when XyzuPlayer.Instance.Player.Queue.CurrentSong is null:
					detailone = context.GetString(Resource.String.player_default_detail_one_alt);
					detailtwo = context.GetString(Resource.String.player_default_detail_two_alt);
					detailthree = context.GetString(Resource.String.player_default_detail_three_alt);
					detailfour = context.GetString(Resource.String.player_default_detail_four_alt);
					break;

				default:
					detailone =
						XyzuPlayer.Instance.Player.Queue.CurrentSong.Title ??
						context.GetString(Resource.String.player_unknown_title);
					detailtwo =
						XyzuPlayer.Instance.Player.Queue.CurrentSong.Artist ??
						context.GetString(Resource.String.player_unknown_artist);
					detailthree =
						XyzuPlayer.Instance.Player.Queue.CurrentSong.Album ??
						context.GetString(Resource.String.player_unknown_album);
					detailfour = string.Format(
						"{0}/{1}",
						(XyzuPlayer.Instance.Player.Queue.CurrentIndex ?? -1) + 1,
						XyzuPlayer.Instance.Player.Queue.Count);

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

			remoteviews.SetTextViewText(Resource.Id.xyzu_widget_nowplaying_detail_one_textview, detailone);
			remoteviews.SetTextViewText(Resource.Id.xyzu_widget_nowplaying_detail_two_textview, detailtwo);
			remoteviews.SetTextViewText(Resource.Id.xyzu_widget_nowplaying_detail_three_textview, detailthree);
			remoteviews.SetTextViewText(Resource.Id.xyzu_widget_nowplaying_detail_four_textview, detailfour);
			remoteviews.SetImageViewBitmap(Resource.Id.xyzu_widget_nowplaying_blur_imageview, blur);
			remoteviews.SetImageViewBitmap(Resource.Id.xyzu_widget_nowplaying_artwork_imageview, artwork);

			return remoteviews;
		}
	}
}