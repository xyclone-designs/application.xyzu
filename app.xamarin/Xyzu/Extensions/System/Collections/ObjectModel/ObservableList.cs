using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.ObjectModel
{
	public class ObservableList<T> : ObservableCollection<T>, IObservableList<T>
	{
		~ObservableList()
		{
			CollectionChanged -= OnCollectionChanged;
		}

		public ObservableList() : base() 
		{
			CollectionChanged += OnCollectionChanged;
		}
		public ObservableList(IEnumerable<T> collection) : base(collection) 
		{
			CollectionChanged += OnCollectionChanged;
		}
		public ObservableList(List<T> list) : base(list)
		{
			CollectionChanged += OnCollectionChanged;
		}

		public event NotifyListChangedEventHandler? ListChanged;

		protected IEnumerable<NotifyListChangedEventHandler> ListChangedInvacationList()
		{
			if (ListChanged?.GetInvocationList().OfType<NotifyListChangedEventHandler>() is IEnumerable<NotifyListChangedEventHandler> notifylistchangedeventhandlers)
				foreach (NotifyListChangedEventHandler notifylistchangedeventhandler in notifylistchangedeventhandlers)
					yield return notifylistchangedeventhandler;
		}

		public void AddRange(IEnumerable<T>? enumerable)
		{
			AddRangeEventless(enumerable);

			if (enumerable is not null)
				RaiseListChangedEvent(NotifyListChangedEventArgs.FromAddRange(enumerable.ToList()));
		}
		public void AddRange(IAsyncEnumerable<T>? asyncenumerable)
		{
			int count = -Items.Count;

			AddRangeEventless(asyncenumerable);

			count += Items.Count;

			if (asyncenumerable is not null)
				RaiseListChangedEvent(NotifyListChangedEventArgs.FromAddRange(Items.TakeLast(count).ToList()));
		}
		public async Task AddRange(IAsyncEnumerable<T>? asyncenumerable, CancellationToken cancellationtoken = default)
		{
			int count = -Items.Count;

			await AddRangeEventless(asyncenumerable, cancellationtoken);

			count += Items.Count;

			if (asyncenumerable is not null)
				RaiseListChangedEvent(NotifyListChangedEventArgs.FromAddRange(Items.TakeLast(count).ToList()));
		}

		public new void Clear()
		{
			Items.Clear();

			RaiseListChangedEvent(NotifyListChangedEventArgs.FromReset());
		}

		public void Insert(int index, IEnumerable<T>? enumerable)
		{
			if (enumerable is null || enumerable.Any() is false)
				return;

			int position = index - 1;

			foreach (T t in enumerable)
				Items.Insert(position++, t);

			RaiseListChangedEvent(NotifyListChangedEventArgs.FromInsert(index, Items.Skip(index).Take(position - index).ToList()));
		}
		public async void Insert(int index, IAsyncEnumerable<T>? asyncenumerable)
		{
			if (asyncenumerable is null || await asyncenumerable.AnyAsync() is false)
				return;

			int position = index - 1;

			await foreach (T t in asyncenumerable)
				Items.Insert(position++, t);

			RaiseListChangedEvent(NotifyListChangedEventArgs.FromInsert(index, Items.Skip(index).Take(position - index).ToList()));
		}
		public async Task Insert(int index, IAsyncEnumerable<T>? asyncenumerable, CancellationToken cancellationtoken = default)
		{
			if (asyncenumerable is null || await asyncenumerable.AnyAsync(cancellationtoken) is false)
				return;

			int position = index - 1;

			await foreach (T t in asyncenumerable.WithCancellation(cancellationtoken))
				Items.Insert(position++, t);

			RaiseListChangedEvent(NotifyListChangedEventArgs.FromInsert(index, Items.Skip(index).Take(position - index).ToList()));
		}

		public void Refresh(T? item)
		{
			Items.Clear();

			if (item is not null)
				Items.Add(item);

			RaiseListChangedEvent(NotifyListChangedEventArgs.FromRefresh());
		}
		public void Refresh(IEnumerable<T>? enumerable)
		{
			Items.Clear();

			if (enumerable is not null)
				foreach (T t in enumerable)
					Items.Add(t);

			RaiseListChangedEvent(NotifyListChangedEventArgs.FromRefresh());
		}
		public async Task Refresh(IAsyncEnumerable<T>? asyncenumerable, CancellationToken cancellationtoken = default)
		{
			Items.Clear();

			if (asyncenumerable is not null)
				await foreach (T t in asyncenumerable.WithCancellation(cancellationtoken))
					Items.Add(t);

			RaiseListChangedEvent(NotifyListChangedEventArgs.FromRefresh());
		}

		public void Reverse()
		{							   
			// todo
			IList<T> reversed = Items.Reverse().ToList();

			Items.Clear();

			foreach (T t in reversed)
				Items.Add(t);

			RaiseListChangedEvent(NotifyListChangedEventArgs.FromReverse());
		}

		protected virtual void RaiseListChangedEvent(NotifyListChangedEventArgs args)
		{
			ListChanged?.Invoke(this, args);
		}
		protected virtual void RaiseListChangedEvent(NotifyCollectionChangedEventArgs args)
		{
			ListChanged?.Invoke(this, NotifyListChangedEventArgs.FromArgs(args));
		}
		protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			ListChanged?.Invoke(sender, NotifyListChangedEventArgs.FromArgs(args));
		}

		protected void AddRangeEventless(IEnumerable<T>? enumerable)
		{
			if (enumerable is not null)
				foreach (T t in enumerable)
					Items.Add(t);
		}
		protected async void AddRangeEventless(IAsyncEnumerable<T>? asyncenumerable)
		{
			if (asyncenumerable is not null)
				await foreach (T t in asyncenumerable)
					Items.Add(t);
		}
		protected async Task AddRangeEventless(IAsyncEnumerable<T>? asyncenumerable, CancellationToken cancellationtoken = default)
		{
			if (asyncenumerable is not null)
				await foreach (T t in asyncenumerable.WithCancellation(cancellationtoken))
					Items.Add(t);
		}
	}
}
