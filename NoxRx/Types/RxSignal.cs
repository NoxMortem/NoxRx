using System;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class RxSignal : IObservable<Unit>, ISignalSource, IDisposable
	{
		private readonly Subject<Unit> subject = new Subject<Unit>();

		public void        Signal()                            => subject.OnNext(Unit.Default);
		public IDisposable Subscribe(IObserver<Unit> observer) => subject.Subscribe(observer);

		public void Dispose() => subject?.Dispose();
	}

	public class RxSignal<T> : IObservable<T>
	{
		private readonly Rx<T> rx;

		public RxSignal(Rx<T> rx) => this.rx = rx;

		public IDisposable Subscribe(IObserver<T> observer) => rx.OnSignal().Subscribe(observer);
	}
}