using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xyzu.Library.Models;
using Xyzu.Player.Enums;

namespace Xyzu.Player
{
	public partial interface IQueue : IObservableList<IQueueItem>, IDisposable
	{
		string? Id { get; set; }
		int? CurrentIndex { get; set; }
		int? PreviousIndex { get; }

		IQueueItem? Previous { get; }
		IQueueItem? Current { get; }
		IQueueItem? Next { get; }  

		ISong? PreviousSong { get; }
		ISong? CurrentSong { get; }
		ISong? NextSong { get; }

		Func<IQueueItem?, ISong?>? OnRetreiveSong { get; set; }

		void Refresh(int? index, params IQueueItem[] songs);
		void Refresh(int? index, IEnumerable<IQueueItem> songs);

		void Add(int position, params IQueueItem[] songs);
		void Add(QueuePositions position, params IQueueItem[] songs);
		void Add(int position, IEnumerable<IQueueItem> songs);
		void Add(QueuePositions position, IEnumerable<IQueueItem> songs);

		void Move(string fromid, int to);
		void Move(QueuePositions from, int to);
		void Move(int from, QueuePositions to);
		void Move(string fromid, QueuePositions to);
		void Move(QueuePositions from, QueuePositions to);

		void Remove();
		void Remove(IEnumerable<string> ids);
		void Remove(IEnumerable<int> positions);
		void Remove(IEnumerable<IQueueItem> queueSongs);
		void Remove(IEnumerable<QueuePositions> positions);
	}
}
