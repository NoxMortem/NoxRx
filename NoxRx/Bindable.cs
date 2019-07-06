using System;
using UniRx;
using UniRx.Async;

namespace Infrastructure.NoxRx
{
	public class Bindable<T>
	{
		private readonly IObservable<T> observable;

		public Bindable(IObservable<T> observable) => this.observable = observable.Share();		
		public IDisposable It()                               => observable.Subscribe();
		public IDisposable To(Action<T>        action)        => Call(action);
		public IDisposable To(Func<T, UniTask> func, T param) => Call(func, param);
		public IDisposable To(Subject<T>       s)      => Call(s.OnNext);
		public IDisposable To(Action           action) => Call(action);

		public IDisposable Call(Action<T>        action)        => observable.Subscribe(action.Invoke);
		public IDisposable Call(Func<T, UniTask> func, T param) => observable.Subscribe(_ => func.Invoke(param));
		public IDisposable Call(Action           action) => observable.Subscribe(_ => action.Invoke());

		public Bindable<T> OnComplete(Action action)
		{
			observable.DoOnCompleted(action.Invoke).Subscribe();
			return this;
		}
	}
}