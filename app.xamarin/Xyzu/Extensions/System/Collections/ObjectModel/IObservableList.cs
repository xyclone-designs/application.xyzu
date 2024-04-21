using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.ObjectModel
{
	public interface IObservableList<T> : INotifyListChanged, INotifyPropertyChanged, ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>
	{
		void AddRange(IEnumerable<T>? enumerable);
		void AddRange(IAsyncEnumerable<T>? asyncenumerable);
		Task AddRange(IAsyncEnumerable<T>? asyncenumerable, CancellationToken cancellationtoken = default);		   

		void Insert(int index, IEnumerable<T>? enumerable);
		void Insert(int index, IAsyncEnumerable<T>? asyncenumerable);
		Task Insert(int index, IAsyncEnumerable<T>? asyncenumerable, CancellationToken cancellationtoken = default);
		
		void Refresh(T? item);
		void Refresh(IEnumerable<T>? enumerable);
		Task Refresh(IAsyncEnumerable<T>? asyncenumerable, CancellationToken cancellationtoken = default);

		void Reverse();
	}
}