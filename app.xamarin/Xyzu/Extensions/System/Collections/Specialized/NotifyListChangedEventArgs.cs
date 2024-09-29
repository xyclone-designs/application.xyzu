
namespace System.Collections.Specialized
{
	public class NotifyListChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action) : base(action) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, IList? changedItems) : base(action, changedItems) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, object changedItem) : base(action, changedItem) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems) : base(action, newItems, oldItems) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, IList? changedItems, int startingIndex) : base(action, changedItems, startingIndex) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index) : base(action, changedItem, index) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem) : base(action, newItem, oldItem) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, IList? changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex) { }
		public NotifyListChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index) : base(action, newItem, oldItem, index) { }

		public new NotifyListChangedAction Action { get; private set; }
		public NotifyCollectionChangedAction BaseAction
		{
			get => base.Action;
		}

		public int RangeCount { get; private set; }

		public static NotifyListChangedEventArgs FromArgs(NotifyCollectionChangedEventArgs args)
		{
			return args.Action switch
			{
				NotifyCollectionChangedAction.Add => new NotifyListChangedEventArgs(args.Action, args.NewItems, args.NewStartingIndex),
				NotifyCollectionChangedAction.Move => new NotifyListChangedEventArgs(args.Action, args.OldItems, args.NewStartingIndex, args.OldStartingIndex),
				NotifyCollectionChangedAction.Remove => new NotifyListChangedEventArgs(args.Action, args.OldItems, args.OldStartingIndex),
				NotifyCollectionChangedAction.Replace => new NotifyListChangedEventArgs(args.Action, args.NewItems ?? new ArrayList(), args.OldItems ?? new ArrayList(), args.OldStartingIndex),
				NotifyCollectionChangedAction.Reset => new NotifyListChangedEventArgs(args.Action),

				_ => throw new ArgumentException(string.Format("Invalid NotifyCollectionChangedEventArgs.Action '{0}'", args.Action))               
			};
		}
		public static NotifyListChangedEventArgs FromAddRange(IList newitems)
		{
			return new NotifyListChangedEventArgs(NotifyCollectionChangedAction.Add, newitems)
			{
				Action = NotifyListChangedAction.AddRange,
				RangeCount = newitems.Count,
			};
		}
		public static NotifyListChangedEventArgs FromInsert(int index, IList newitems)
		{
			return new NotifyListChangedEventArgs(NotifyCollectionChangedAction.Add, newitems, index)
			{
				Action = NotifyListChangedAction.AddRange,
				RangeCount = newitems.Count,
			};
		}
		public static NotifyListChangedEventArgs FromMoveRange()
		{
			return new NotifyListChangedEventArgs(NotifyCollectionChangedAction.Move)
			{
				Action = NotifyListChangedAction.MoveRange
			};
		}
		public static NotifyListChangedEventArgs FromRefresh()
		{
			return new NotifyListChangedEventArgs(NotifyCollectionChangedAction.Reset)
			{
				Action = NotifyListChangedAction.Refresh
			};
		}
		public static NotifyListChangedEventArgs FromRemoveRange()
		{
			return new NotifyListChangedEventArgs(NotifyCollectionChangedAction.Remove)
			{
				Action = NotifyListChangedAction.RemoveRange
			};
		}
		public static NotifyListChangedEventArgs FromReplaceRange()
		{
			return new NotifyListChangedEventArgs(NotifyCollectionChangedAction.Replace)
			{
				Action = NotifyListChangedAction.ReplaceRange
			};
		}
		public static NotifyListChangedEventArgs FromReset()
		{
			return new NotifyListChangedEventArgs(NotifyCollectionChangedAction.Reset)
			{
				Action = NotifyListChangedAction.Reset
			};
		}							   
		public static NotifyListChangedEventArgs FromReverse()
		{
			return new NotifyListChangedEventArgs(NotifyCollectionChangedAction.Reset)
			{
				Action = NotifyListChangedAction.Reverse
			};
		}
	}
}
