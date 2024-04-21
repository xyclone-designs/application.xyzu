
namespace System.Collections.Specialized
{
	public enum NotifyListChangedAction
	{
		Add,
		AddRange,
		Move,
		MoveRange,
		Refresh,
		Remove,
		RemoveRange,
		Replace,
		ReplaceRange,
		Reset,
		Reverse,
	}

	public static class NotifyActionExtensions
	{
		public static NotifyListChangedAction? ToListChangedAction(this NotifyCollectionChangedAction notifycollectionchangedaction)
		{
			return notifycollectionchangedaction switch
			{
				NotifyCollectionChangedAction.Add => NotifyListChangedAction.Add,
				NotifyCollectionChangedAction.Move => NotifyListChangedAction.Move,
				NotifyCollectionChangedAction.Remove => NotifyListChangedAction.Remove,
				NotifyCollectionChangedAction.Replace => NotifyListChangedAction.Replace,
				NotifyCollectionChangedAction.Reset => NotifyListChangedAction.Reset,

				_ => new NotifyListChangedAction?()
			};
		}		  
		public static NotifyCollectionChangedAction? ToCollectionChangedAction(this NotifyListChangedAction notifylistchangedaction)
		{
			return notifylistchangedaction switch
			{
				NotifyListChangedAction.Add => NotifyCollectionChangedAction.Add,
				NotifyListChangedAction.Move => NotifyCollectionChangedAction.Move,
				NotifyListChangedAction.Remove => NotifyCollectionChangedAction.Remove,
				NotifyListChangedAction.Replace => NotifyCollectionChangedAction.Replace,
				NotifyListChangedAction.Reset => NotifyCollectionChangedAction.Reset,

				_ => new NotifyCollectionChangedAction?()
			};
		}
	}
}
