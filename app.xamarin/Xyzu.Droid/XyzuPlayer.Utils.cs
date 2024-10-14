#nullable enable

using Android.App;

using System;

using Xyzu.Library.Models;
using Xyzu.Player;

namespace Xyzu
{
	public sealed partial class XyzuPlayer 
	{
		public static IQueueItem QueueItemFrom(ISong song, string? secondaryId = null)
		{
			return IQueueItem.FromSong(song, queueitem =>
			{
				queueitem.SecondaryId = secondaryId;

				if (song.IsCorrupt)
					queueitem.Uri = new Uri("file:///android_asset/sounds/silence.mp3");
			});
		}
	}
}