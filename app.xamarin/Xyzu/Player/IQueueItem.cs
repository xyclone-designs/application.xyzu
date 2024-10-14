using System;

using Xyzu.Library.Models;

namespace Xyzu.Player
{
	public interface IQueueItem
	{
		Uri? Uri { get; set; }
		string PrimaryId { get; set; }
		string? SecondaryId { get; set; }
		bool ShouldSkip { get; set; }
		
		public class Default : IQueueItem
		{
			public Default(string primaryid) 
			{
				PrimaryId = primaryid;
			}

			public Uri? Uri { get; set; }
			public string PrimaryId { get; set; }
			public string? SecondaryId { get; set; }
			public bool ShouldSkip { get; set; }
		}

		public static IQueueItem FromSong(ISong song, Action<IQueueItem>? oncreate = null)
		{
			IQueueItem queueitem = new Default(song.Id)
			{
				Uri = song.Uri,
				ShouldSkip = song.IsCorrupt
			};

			oncreate?.Invoke(queueitem);

			return queueitem;
		}
	}
}
