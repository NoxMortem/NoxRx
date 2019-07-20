using System;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class Rx<T> : ISignalSource, ISignalObservable<T>, IDisposable
	{
		private readonly ReactiveProperty<T> reactiveProperty = new ReactiveProperty<T>(default);
		private readonly Subject<T>          onSignalSubject  = new Subject<T>();

		public void Set(T value) => reactiveProperty.SetValueAndForceNotify(value);
		public T    Value        => reactiveProperty.Value;

		public IDisposable Subscribe(IObserver<T> observer) => reactiveProperty.Subscribe(observer);

		public                          RxWatch<T> AsWatch() => new RxWatch<T>(this);
		public static implicit operator T(Rx<T> rx)          => rx.reactiveProperty.Value;

		public void Signal() => onSignalSubject.OnNext(reactiveProperty.Value);

		public IObservable<T> OnSignal() => onSignalSubject.AsObservable();

		public void Dispose() => reactiveProperty?.Dispose();
	}
}