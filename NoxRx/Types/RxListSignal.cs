using System;
using System.Collections.Generic;

namespace Infrastructure.NoxRx.Types
{
	public class RxListSignal<T> : ISignalObservable<IList<T>>
	{
		private readonly ISignalObservable<IList<T>> watchable;
		private readonly IObservable<IList<T>>       onSignal;

		public RxListSignal(ISignalObservable<IList<T>> watchable)
		{
			this.watchable = watchable;
			onSignal       = watchable.OnSignal();
		}

		public IObservable<IList<T>> OnSignal() => onSignal;

		public IDisposable Subscribe(IObserver<IList<T>> observer) => onSignal.Subscribe(observer);

		public RxListSignalReadOnly<T> AsReadOnly() => new RxListSignalReadOnly<T>(watchable);
	}
}