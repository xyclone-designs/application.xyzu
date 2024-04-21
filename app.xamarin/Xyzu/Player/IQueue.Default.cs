using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using Xyzu.Library.Models;
using Xyzu.Player.Enums;

namespace Xyzu.Player
{
	public partial interface IQueue 
	{
		public class Default : ObservableList<IQueueItem>, IQueue
		{
			public Default() : base() { }
			public Default(IEnumerable<IQueueItem> collection) : base(collection) { }
			public Default(List<IQueueItem> list) : base(list) { }

			private string? _Id;
			private int? _PreviousIndex;
			private int? _CurrentIndex;
			private ISong? _PreviousSong;
			private ISong? _CurrentSong;
			private ISong? _NextSong;

			public string? Id
			{
				get => _Id;
				set
				{
					_Id = value;

					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Id)));
				}
			}
			public int? CurrentIndex
			{
				get => _CurrentIndex;
				set
				{
					_PreviousIndex = _CurrentIndex;
					_CurrentIndex = value;

					_PreviousSong = null;
					_CurrentSong = null;
					_NextSong = null;

					OnPropertyChanged(new PropertyChangedEventArgs(nameof(CurrentIndex)));
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(PreviousIndex)));
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Current)));
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Previous)));
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Next)));			   
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(CurrentSong)));
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(PreviousSong)));
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(NextSong)));
				}
			}
			public int? PreviousIndex
			{
				get => _PreviousIndex;
			}

			public IQueueItem? Previous
			{
				get
				{
					if (CurrentIndex.HasValue)
						return Items.ElementAtOrDefault(CurrentIndex.Value - 1);

					return null;
				}
			}
			public IQueueItem? Current
			{
				get
				{
					if (CurrentIndex.HasValue)
						return Items.ElementAtOrDefault(CurrentIndex.Value);

					return null;
				}
			}
			public IQueueItem? Next
			{
				get
				{
					if (CurrentIndex.HasValue)
						return Items.ElementAtOrDefault(CurrentIndex.Value + 1);

					return null;
				}
			}

			public ISong? PreviousSong
			{
				get => _PreviousSong ??= OnRetreiveSong?.Invoke(Previous);
			}
			public ISong? CurrentSong
			{
				get => _CurrentSong ??= OnRetreiveSong?.Invoke(Current);
			}
			public ISong? NextSong
			{
				get => _NextSong ??= OnRetreiveSong?.Invoke(Next); 
			}

			public Func<IQueueItem?, ISong?>? OnRetreiveSong { get; set; }


			protected new event PropertyChangedEventHandler? PropertyChanged;
			protected IEnumerable<PropertyChangedEventHandler> PropertyChangedInvacationList()
			{
				if (PropertyChanged?.GetInvocationList().OfType<PropertyChangedEventHandler>() is IEnumerable<PropertyChangedEventHandler> propertychangedeventhandlers)
					foreach (PropertyChangedEventHandler propertychangedeventhandler in propertychangedeventhandlers)
						yield return propertychangedeventhandler;
			}

			public void Refresh(int? index, params IQueueItem[] songs)
			{
				Refresh(index, songs as IEnumerable<IQueueItem>);
			}
			public void Refresh(int? index, IEnumerable<IQueueItem> songs)
			{
				Refresh(songs);

				CurrentIndex = index;
			}

			public void Add(int position, params IQueueItem[] songs)
			{
				Add(position, songs as IEnumerable<IQueueItem>);
			}
			public void Add(QueuePositions position, params IQueueItem[] songs)
			{
				Add(position, songs as IEnumerable<IQueueItem>);
			}
			public void Add(int position, IEnumerable<IQueueItem> songs)
			{
				if (position >= Items.Count - 1)
					AddRange(songs);
				else
				{
					int index = position;
					foreach (IQueueItem song in songs)
					{
						Insert(index, song);
						index += 1;
					}


					RaiseListChangedEvent(NotifyListChangedEventArgs.FromInsert(index, songs.ToList()));
				}
			}
			public void Add(QueuePositions queuePosition, IEnumerable<IQueueItem> songs)
			{
				switch (queuePosition)
				{
					case QueuePositions.Now:
						Add(CurrentIndex ?? 0, songs);
						break;

					case QueuePositions.First:
						Add(0, songs);
						break;
					case QueuePositions.Last:
						Add(Count - 1, songs);
						break;
					case QueuePositions.UpOne:
					case QueuePositions.Next:
						Add(CurrentIndex ?? 1, songs);
						break;
					case QueuePositions.DownOne:
					case QueuePositions.Previous:
						Add(CurrentIndex ?? 0, songs);
						break;
					case QueuePositions.DownOneSubQueue:
						IQueueItem? downonesubqueuesong = Items
							.SkipWhile(x => string.Equals(x.PrimaryId, Current?.PrimaryId, StringComparison.OrdinalIgnoreCase) is false)
							.SkipWhile(x => x.SecondaryId == Current?.SecondaryId)
							.FirstOrDefault();

						if (downonesubqueuesong != null)
							Add(downonesubqueuesong);
						break;
					case QueuePositions.UpOneSubQueue:
						IQueueItem? uponesubqueuesong = Items
							.SkipWhile(x => string.Equals(x.PrimaryId, Current?.PrimaryId, StringComparison.OrdinalIgnoreCase) is false)
							.Reverse()
							.SkipWhile(x => x.SecondaryId == Current?.SecondaryId)
							.FirstOrDefault();

						if (uponesubqueuesong != null)
							Add(uponesubqueuesong);
						break;
					default:
						foreach (IQueueItem song in songs)
							Add(song);
						break;
				}
			}

			public void Move(string fromid, int to)
			{
				int? fromposition = PositionFrom(fromid);
				int? toposition = PositionFrom(to);

				if (fromposition.HasValue && toposition.HasValue)
					Move(fromposition.Value, toposition.Value);
			}
			public void Move(QueuePositions from, int to)
			{
				int? fromposition = PositionFrom(from);
				int? toposition = PositionFrom(to);

				if (fromposition.HasValue && toposition.HasValue)
					Move(fromposition.Value, toposition.Value);
			}
			public void Move(int from, QueuePositions to)
			{
				int? fromposition = PositionFrom(from);
				int? toposition = PositionFrom(to);

				if (fromposition.HasValue && toposition.HasValue)
					Move(fromposition.Value, toposition.Value);
			}
			public void Move(string fromid, QueuePositions to)
			{
				int? fromposition = PositionFrom(fromid);
				int? toposition = PositionFrom(to);

				if (fromposition.HasValue && toposition.HasValue)
					Move(fromposition.Value, toposition.Value);
			}
			public void Move(QueuePositions from, QueuePositions to)
			{
				int? fromposition = PositionFrom(from);
				int? toposition = PositionFrom(to);

				if (fromposition.HasValue && toposition.HasValue)
					Move(fromposition.Value, toposition.Value);
			}

			public void Remove()
			{
				Clear();
			}
			public void Remove(IEnumerable<string> ids)
			{
				IEnumerable<int> positions = Items
					.Select((queuesong, index) =>
					{
						return ids.Contains(queuesong.PrimaryId) ? index : new int?();

					}).OfType<int>().Reverse();

				foreach (int position in positions)
					RemoveAt(position);
			}
			public void Remove(IEnumerable<int> positions)
			{
				foreach (int position in positions.OrderByDescending(x => x))
					if (position >= 0 && Count > position)
						RemoveAt(position);
			}
			public void Remove(IEnumerable<IQueueItem> queueSongs)
			{
				foreach (IQueueItem queueSong in queueSongs)
					Remove(queueSong);
			}
			public void Remove(IEnumerable<QueuePositions> queuePositions)
			{
				foreach (QueuePositions position in queuePositions)
					switch (position)
					{
						case QueuePositions.First:
							if (Items.FirstOrDefault() is IQueueItem firstqueuesong)
								Remove(firstqueuesong);
							break;
						case QueuePositions.Last:
							if (Items.LastOrDefault() is IQueueItem lastqueuesong)
								Remove(lastqueuesong);
							break;
						case QueuePositions.UpOne:
						case QueuePositions.Previous:
							if (Previous != null)
								Remove(Previous);
							break;
						case QueuePositions.DownOne:
						case QueuePositions.Next:
							if (Next != null)
								Remove(Next);
							break;
						case QueuePositions.DownOneSubQueue:
							IQueueItem? downonesubqueuesong = Items
								.SkipWhile(x => string.Equals(x.PrimaryId, Current?.PrimaryId, StringComparison.OrdinalIgnoreCase) is false)
								.SkipWhile(x => x.SecondaryId == Current?.SecondaryId)
								.FirstOrDefault();

							if (downonesubqueuesong != null)
								Remove(downonesubqueuesong);
							break;
						case QueuePositions.UpOneSubQueue:
							IQueueItem? uponesubqueuesong = Items
								.SkipWhile(x => string.Equals(x.PrimaryId, Current?.PrimaryId, StringComparison.OrdinalIgnoreCase) is false)
								.Reverse()
								.SkipWhile(x => x.SecondaryId == Current?.SecondaryId)
								.FirstOrDefault();

							if (uponesubqueuesong != null)
								Remove(uponesubqueuesong);
							break;
						case QueuePositions.Now:
						default: break;
					}
			}

			public void Dispose()
			{
				Clear();
			}

			protected int? PositionFrom(int position)
			{
				bool valid = position >= 0 && Items.Count > position;

				return valid ? position : new int?();
			}
			protected int? PositionFrom(string id)
			{
				int? position = Items
					.Select((queuesong, index) =>
					{
						return string.Equals(id, queuesong.PrimaryId, StringComparison.OrdinalIgnoreCase)
							? index
							: new int?();

					}).FirstOrDefault(index => index.HasValue);

				return position;
			}
			protected int? PositionFrom(QueuePositions queueposition)
			{
				if (CurrentIndex - 1 is not int currentqueuesongindex)
					return null;

				switch (queueposition)
				{
					case QueuePositions.First: return 0;
					case QueuePositions.Now: return currentqueuesongindex;
					case QueuePositions.Last: return Items.Count - 1;

					case QueuePositions.UpOne:
					case QueuePositions.Previous: return currentqueuesongindex - 1;

					case QueuePositions.DownOne:
					case QueuePositions.Next: return currentqueuesongindex + 1;

					case QueuePositions.UpOneSubQueue:
					case QueuePositions.DownOneSubQueue:

						IEnumerable<(int index, string? queueid)> subqueues = Items.Select(((queuesong, index) => (index, queuesong.SecondaryId)));

						if (queueposition is QueuePositions.UpOneSubQueue)
							subqueues = subqueues.Reverse();

						(int index, string? queueid) = subqueues
							.SkipWhile(subqueue => subqueue.index != currentqueuesongindex)
							.SkipWhile(subqueue =>
							{
								if (subqueue.queueid is null && Current?.SecondaryId is null)
									return true;

								return string.Equals(subqueue.queueid, Current?.SecondaryId, StringComparison.OrdinalIgnoreCase);

							}).FirstOrDefault();

						return index;

					default: return null;
				}
			}
		}
	}
}
