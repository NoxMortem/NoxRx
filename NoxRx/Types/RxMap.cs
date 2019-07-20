using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxMap<TK, TV> : IDictionary<TK, TV>, ISignalSource, ISignalObservable<IDictionary<TK, TV>>, IDisposable
	{
		private readonly  IDictionary<TK, TV> dict;
		internal readonly Subject<int>        OnCountChangedSubject = new Subject<int>();
		internal readonly Subject<(TK, TV)>   OnAddSubject          = new Subject<(TK, TV)>();
		internal readonly Subject<(int, TV)>  OnInsertAtSubject     = new Subject<(int, TV)>();
		internal readonly Subject<TV>         OnRemoveSubject       = new Subject<TV>();
		internal readonly Subject<int>        OnRemoveAtSubject     = new Subject<int>();
		internal readonly Subject<Unit>       OnClearSubject        = new Subject<Unit>();

		internal readonly Subject<IDictionary<TK, TV>> OnSignalSubject = new Subject<IDictionary<TK, TV>>();
		public RxMap() => dict = new Dictionary<TK, TV>();

		public RxMap(IDictionary<TK, TV> dict) => this.dict = dict;

		public RxMap(IEnumerable<ValueTuple<TK, TV>> enumerable) =>
			dict = enumerable.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

		public IObservable<IDictionary<TK, TV>> OnSignal() => OnSignalSubject.AsObservable();

		public IDisposable Subscribe(IObserver<IDictionary<TK, TV>> observer) => throw new NotImplementedException();

		public void Signal() => OnSignalSubject.OnNext(new ReadOnlyDictionary<TK, TV>(dict));

		public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => throw new NotImplementedException();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(KeyValuePair<TK, TV> kvp)
		{
			dict.Add(kvp.Key, kvp.Value);
			OnAddSubject.OnNext((kvp.Key, kvp.Value));
			OnCountChangedSubject.OnNext(dict.Count);
		}

		public void Clear()
		{
			if (dict.Count == 0)
				return;
			dict.Clear();
			OnClearSubject.OnNext(Unit.Default);
			OnCountChangedSubject.OnNext(dict.Count);
		}

		public bool Contains(KeyValuePair<TK, TV> item) => dict.Contains(item);

		public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex) => dict.CopyTo(array, arrayIndex);

		public bool Remove(KeyValuePair<TK, TV> kvp)
		{
			if (dict.Remove(kvp))
			{
				OnRemoveSubject.OnNext(kvp.Value);
				OnCountChangedSubject.OnNext(dict.Count);
				return true;
			}

			return false;
		}

		public int  Count      => dict.Count;
		public bool IsReadOnly => false;

		public void Add(TK key, TV value)
		{
			dict.Add(key, value);
			OnAddSubject.OnNext((key, value));
			OnCountChangedSubject.OnNext(dict.Count);
		}

		public bool ContainsKey(TK key) => dict.ContainsKey(key);

		public bool Remove(TK key)
		{
			if (dict.TryGetValue(key, out var old))
			{
				dict.Remove(key);
				OnRemoveSubject.OnNext(old);
				OnCountChangedSubject.OnNext(dict.Count);
				return false;
			}

			return false;
		}

		public bool TryGetValue(TK key, out TV value) => dict.TryGetValue(key, out value);

		public TV this[TK key]
		{
			get => dict[key];
			set
			{
				if (dict.TryGetValue(key, out var old))
				{
					dict[key] = value;
					OnRemoveSubject.OnNext(old);
					OnAddSubject.OnNext((key, value));
					OnCountChangedSubject.OnNext(dict.Count);
				}
				else
				{
					dict[key] = value;
					OnAddSubject.OnNext((key, value));
					OnCountChangedSubject.OnNext(dict.Count);
				}
			}
		}

		public ICollection<TK> Keys   => dict.Keys;
		public ICollection<TV> Values => dict.Values;

		public void Dispose()
		{
			OnCountChangedSubject?.Dispose();
			OnAddSubject?.Dispose();
			OnInsertAtSubject?.Dispose();
			OnRemoveSubject?.Dispose();
			OnRemoveAtSubject?.Dispose();
			OnClearSubject?.Dispose();
		}

		public RxMapWatch<TK, TV>          AsWatch()          => new RxMapWatch<TK, TV>(this);
		public RxMapWatchReadOnly<TK, TV>  AsWatchReadOnly()  => new RxMapWatchReadOnly<TK, TV>(this);
		public RxMapSignal<TK, TV>         AsSignal()         => new RxMapSignal<TK, TV>(this);
		public RxMapSignalReadOnly<TK, TV> AsSignalReadOnly() => new RxMapSignalReadOnly<TK, TV>(this);

		IObservable<IDictionary<TK, TV>> ISignalObservable<IDictionary<TK, TV>>.OnSignal() =>
			throw new NotImplementedException();
	}
}