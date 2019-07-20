using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxList<T> : IList<T>, ISignalSource, ISignalObservable<IList<T>>, IDisposable
	{
		private readonly  List<T>           list;
		internal readonly Subject<int>      OnCountChangedSubject = new Subject<int>();
		internal readonly Subject<T>        OnAddSubject          = new Subject<T>();
		internal readonly Subject<(int, T)> OnInsertAtSubject     = new Subject<(int, T)>();
		internal readonly Subject<T>        OnRemoveSubject       = new Subject<T>();
		internal readonly Subject<int>      OnRemoveAtSubject     = new Subject<int>();
		internal readonly Subject<Unit>     OnClearSubject        = new Subject<Unit>();

		internal readonly Subject<IList<T>> OnSignalSubject = new Subject<IList<T>>();

		public IDisposable           Subscribe(IObserver<IList<T>> observer) => OnSignalSubject.Subscribe(observer);
		public IObservable<IList<T>> OnSignal()                              => OnSignalSubject.AsObservable();

		public RxList() => list = new List<T>();

		public RxList(IEnumerable<T> enumerable) => list = enumerable.ToList();

		public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) list).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(T item)
		{
			list.Add(item);
			OnAddSubject.OnNext(item);
			OnCountChangedSubject.OnNext(list.Count);
		}

		public void Clear()
		{
			var nonEmpty = list.Count > 0;
			list.Clear();
			OnClearSubject.OnNext(Unit.Default);
			if (nonEmpty)
				OnCountChangedSubject.OnNext(list.Count);
		}

		public bool Contains(T item) => list.Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

		public bool Remove(T item)
		{
			var result = list.Remove(item);
			if (result)
			{
				OnRemoveSubject.OnNext(item);
				OnCountChangedSubject.OnNext(list.Count);
			}

			return result;
		}

		public int  Count           => list.Count;
		public bool IsReadOnly      => false;
		public int  IndexOf(T item) => list.IndexOf(item);

		public void Insert(int index, T item)
		{
			list.Insert(index, item);
			OnInsertAtSubject.OnNext((index, item));
			OnCountChangedSubject.OnNext(list.Count);
		}

		public void RemoveAt(int index)
		{
			list.RemoveAt(index);
			OnRemoveAtSubject.OnNext(index);
			OnCountChangedSubject.OnNext(list.Count);
		}

		public T this[int index]
		{
			get => list[index];
			set
			{
				var old = list[index];
				list[index] = value;
				OnRemoveSubject.OnNext(old);
				OnAddSubject.OnNext(value);
			}
		}

		public void Signal() => OnSignalSubject.OnNext(list.AsReadOnly());

		public void Dispose()
		{
			OnCountChangedSubject?.Dispose();
			OnAddSubject?.Dispose();
			OnInsertAtSubject?.Dispose();
			OnRemoveSubject?.Dispose();
			OnRemoveAtSubject?.Dispose();
			OnClearSubject?.Dispose();
		}

		public RxListWatch<T>          AsWatch()          => new RxListWatch<T>(this);
		public RxListWatchReadOnly<T>  AsWatchReadOnly()  => new RxListWatchReadOnly<T>(this);
		public RxListSignal<T>         AsSignal()         => new RxListSignal<T>(this);
		public RxListSignalReadOnly<T> AsSignalReadOnly() => new RxListSignalReadOnly<T>(this);
	}
}