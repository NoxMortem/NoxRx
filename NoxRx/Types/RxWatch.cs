using System;

namespace Infrastructure.NoxRx.Types
{
	public class RxWatch<T> : IObservable<T>
	{
		private readonly Rx<T> rx;

		public RxWatch(Rx<T> rx) => this.rx = rx;

		public IDisposable Subscribe(IObserver<T> observer) => rx.Subscribe(observer);
	}
}