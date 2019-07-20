using System;
using System.Collections.Generic;

namespace Infrastructure.NoxRx.Types
{
	public class RxMapSignal<TK, TV> : ISignalObservable<IDictionary<TK, TV>>
	{
		private readonly ISignalObservable<IDictionary<TK, TV>> watchable;
		private readonly IObservable<IDictionary<TK, TV>>       onSignal;

		public RxMapSignal(ISignalObservable<IDictionary<TK, TV>> watchable)
		{
			this.watchable = watchable;
			onSignal       = watchable.OnSignal();
		}

		public IDisposable Subscribe(IObserver<IDictionary<TK, TV>> observer) => onSignal.Subscribe(observer);

		public IObservable<IDictionary<TK, TV>> OnSignal() => onSignal;

		public RxMapSignalReadOnly<TK, TV> AsReadOnly() => new RxMapSignalReadOnly<TK, TV>(watchable);
	}
}