using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Extensions.CollectionExtensions;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxListSignalReadOnly<T> : ISignalObservable<IReadOnlyList<T>>
	{
		private readonly IObservable<IReadOnlyList<T>> onSignal;

		public RxListSignalReadOnly(ISignalObservable<IList<T>> watchable) =>
			onSignal = watchable.OnSignal().Select(l => new ReadOnlyCollection<T>(l));

		public IDisposable Subscribe(IObserver<IReadOnlyList<T>> observer) => onSignal.Subscribe(observer);

		public IObservable<IReadOnlyList<T>> OnSignal() => onSignal;
	}
}