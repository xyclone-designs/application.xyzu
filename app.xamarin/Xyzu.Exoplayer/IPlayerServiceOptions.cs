#nullable enable

using Android.App;
using Android.Graphics;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xyzu.Library.Models;

namespace Xyzu.Player
{
	public interface IPlayerServiceOptions
	{
		INotificationOptions? Notification { get; set; }

		public interface INotificationOptions
		{
			public class IconKeys
			{
				public const string Destroy = "IconKeys.Destroy"; 
				public const string Next = "IconKeys.Next"; 
				public const string Pause = "IconKeys.Pause"; 
				public const string Play = "IconKeys.Play"; 
				public const string Previous = "IconKeys.Previous"; 
				public const string Stop = "IconKeys.Stop"; 
				public const string SmallIcon = "IconKeys.SmallIcon"; 
			}

			int? Id { get; set; }
			int? ChannelName { get; set; }
			int? ChannelDescription { get; set; }
			string? DefaultArtist { get; set; }
			string? DefaultAlbum { get; set; }
			string? DefaultId { get; set; }
			string? DefaultTitle { get; set; }
			PendingIntent? ContentIntent { get; set; }
			IDictionary<string, int>? Icons { get; set; }
			Func<BitmapFactory.Options?, ISong?, Bitmap?>? OnBitmap { get; set; }

			public class Default : INotificationOptions
			{
				public int? Id { get; set; }
				public int? ChannelName { get; set; }
				public int? ChannelDescription { get; set; }
				public string? DefaultArtist { get; set; }
				public string? DefaultAlbum { get; set; }
				public string? DefaultId { get; set; }
				public string? DefaultTitle { get; set; }
				public PendingIntent? ContentIntent { get; set; }
				public IDictionary<string, int>? Icons { get; set; }
				public Func<BitmapFactory.Options?, ISong?, Bitmap?>? OnBitmap { get; set; }
			}
		}

		public class Default : IPlayerServiceOptions
		{
			public INotificationOptions? Notification { get; set; }
		}
	}
}