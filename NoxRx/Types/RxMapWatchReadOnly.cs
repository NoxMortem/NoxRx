using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxMapWatchReadOnly<TK, TV>
	{
		public readonly IObservable<int>       Count;
		public readonly IObservable<(TK, TV)>  Add;
		public readonly IObservable<(int, TV)> InsertAt;
		public readonly IObservable<TV>        Remove;
		public readonly IObservable<int>       RemoveAt;
		public readonly IObservable<Unit>      Clear;

		public readonly IObservable<IReadOnlyDictionary<TK, TV>> Signal;

		public RxMapWatchReadOnly(RxMap<TK, TV> rxMap)
		{
			Count    = rxMap.OnCountChangedSubject.AsObservable();
			Add      = rxMap.OnAddSubject.AsObservable();
			InsertAt = rxMap.OnInsertAtSubject.AsObservable();
			Remove   = rxMap.OnRemoveSubject.AsObservable();
			RemoveAt = rxMap.OnRemoveAtSubject.AsObservable();
			Clear    = rxMap.OnClearSubject.AsObservable();
			Signal   = rxMap.OnSignalSubject.AsObservable().Select(d => new ReadOnlyDictionary<TK, TV>(d));
		}
	}
}