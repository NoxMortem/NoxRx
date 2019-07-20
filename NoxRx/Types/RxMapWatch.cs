using System;
using System.Collections.Generic;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxMapWatch<TK, TV>
	{
		private readonly RxMap<TK, TV>          rxMap;
		public readonly  IObservable<int>       Count;
		public readonly  IObservable<(TK, TV)>  Add;
		public readonly  IObservable<(int, TV)> InsertAt;
		public readonly  IObservable<TV>        Remove;
		public readonly  IObservable<int>       RemoveAt;
		public readonly  IObservable<Unit>      Clear;

		public readonly IObservable<IDictionary<TK, TV>> Signal;

		public RxMapWatch(RxMap<TK, TV> rxMap)
		{
			this.rxMap = rxMap;
			Count      = rxMap.OnCountChangedSubject.AsObservable();
			Add        = rxMap.OnAddSubject.AsObservable();
			InsertAt   = rxMap.OnInsertAtSubject.AsObservable();
			Remove     = rxMap.OnRemoveSubject.AsObservable();
			RemoveAt   = rxMap.OnRemoveAtSubject.AsObservable();
			Clear      = rxMap.OnClearSubject.AsObservable();
			Signal     = rxMap.OnSignalSubject.AsObservable();
		}

		public RxMapWatchReadOnly<TK, TV> AsReadOnly() => new RxMapWatchReadOnly<TK, TV>(rxMap);
	}
}