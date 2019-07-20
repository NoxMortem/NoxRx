using System;
using UniRx;
using UniRx.Async;

namespace Infrastructure.NoxRx
{
	public class CompositeBindable<T>
	{
		private readonly IObservable<T>      observable;
		private readonly CompositeDisposable compositeDisposable;

		public CompositeBindable(IObservable<T> observable, CompositeDisposable compositeDisposable)
		{
			this.observable          = observable.Share();
			this.compositeDisposable = compositeDisposable;
		}

		public void It()
		{
			if (compositeDisposable.IsDisposed)
				throw new InvalidOperationException("The CompositeDisposable may not be disposed yet");
			observable.Subscribe().AddTo(compositeDisposable);
		}

		public void To(Action<T>        action)        => Call(action);
		public void To(Func<T, UniTask> func, T param) => Call(func, param);
		public void To(Subject<T>       s) => Call(s.OnNext);

		public void Call(Action<T> action)
		{
			if (compositeDisposable.IsDisposed)
				throw new InvalidOperationException("The CompositeDisposable may not be disposed yet");
			observable.Subscribe(action.Invoke).AddTo(compositeDisposable);
		}

		public void To(Action action) => Call(action);

		public void Call(Func<T, UniTask> func, T param)
		{
			if (compositeDisposable.IsDisposed)
				throw new InvalidOperationException("The CompositeDisposable may not be disposed yet");
			observable.Subscribe(_ => func.Invoke(param)).AddTo(compositeDisposable);
		}

		public void Call(Action action)
		{
			if (compositeDisposable.IsDisposed)
				throw new InvalidOperationException("The CompositeDisposable may not be disposed yet");
			observable.Subscribe(_ => action.Invoke()).AddTo(compositeDisposable);
		}

		public CompositeBindable<T> OnComplete(Action action)
		{
			if (compositeDisposable.IsDisposed)
				throw new InvalidOperationException("The CompositeDisposable may not be disposed yet");
			observable.DoOnCompleted(action.Invoke).Subscribe().AddTo(compositeDisposable);
			return this;
		}
	}
}