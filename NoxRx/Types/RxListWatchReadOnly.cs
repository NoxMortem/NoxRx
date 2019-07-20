using System;
using System.Collections.Generic;
using Infrastructure.Extensions.CollectionExtensions;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxListWatchReadOnly<T>
	{
		public readonly IObservable<int>      Count;
		public readonly IObservable<T>        Add;
		public readonly IObservable<(int, T)> Insert;
		public readonly IObservable<T>        Remove;
		public readonly IObservable<int>      RemoveAt;
		public readonly IObservable<Unit>     Clear;

		public readonly IObservable<IReadOnlyList<T>> Signal;

		public RxListWatchReadOnly(RxList<T> rxList)
		{
			Count    = rxList.OnCountChangedSubject.AsObservable();
			Add      = rxList.OnAddSubject.AsObservable();
			Insert   = rxList.OnInsertAtSubject.AsObservable();
			Remove   = rxList.OnRemoveSubject.AsObservable();
			RemoveAt = rxList.OnRemoveAtSubject.AsObservable();
			Clear    = rxList.OnClearSubject.AsObservable();
			Signal   = rxList.OnSignalSubject.AsObservable().Select(l => l.AsReadOnly());
		}
	}
}