using System;
using System.Collections.Generic;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxListWatch<T>
	{
		private readonly RxList<T>             rxList;
		public readonly  IObservable<int>      Count;
		public readonly  IObservable<T>        Add;
		public readonly  IObservable<(int, T)> Insert;
		public readonly  IObservable<T>        Remove;
		public readonly  IObservable<int>      RemoveAt;
		public readonly  IObservable<Unit>     Clear;

		public readonly IObservable<IList<T>> Signal;

		public RxListWatch(RxList<T> rxList)
		{
			this.rxList = rxList;
			Count       = rxList.OnCountChangedSubject.AsObservable();
			Add         = rxList.OnAddSubject.AsObservable();
			Insert      = rxList.OnInsertAtSubject.AsObservable();
			Remove      = rxList.OnRemoveSubject.AsObservable();
			RemoveAt    = rxList.OnRemoveAtSubject.AsObservable();
			Clear       = rxList.OnClearSubject.AsObservable();
			Signal      = rxList.OnSignalSubject.AsObservable();
		}

		public RxListWatchReadOnly<T> AsReadOnly() => new RxListWatchReadOnly<T>(rxList);
	}
}