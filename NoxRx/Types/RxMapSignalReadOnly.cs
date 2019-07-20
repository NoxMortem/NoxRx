using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxMapSignalReadOnly<TK, TV> : ISignalObservable<IReadOnlyDictionary<TK, TV>>
	{
		private readonly IObservable<IReadOnlyDictionary<TK, TV>> onSignal;

		public RxMapSignalReadOnly(ISignalObservable<IDictionary<TK, TV>> watchable) =>
			onSignal = watchable.OnSignal().Select(d => new ReadOnlyDictionary<TK, TV>(d));

		public IDisposable Subscribe(IObserver<IReadOnlyDictionary<TK, TV>> observer) => onSignal.Subscribe(observer);

		public IObservable<IReadOnlyDictionary<TK, TV>> OnSignal() => onSignal;
	}
}