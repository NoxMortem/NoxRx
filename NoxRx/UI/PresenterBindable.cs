using System;
using UniRx;

namespace Infrastructure.NoxRx.UI
{
	public class PresenterBindable<T>
	{
		private readonly IObservable<T> observable;
		private readonly Presenter      presenter;

		public PresenterBindable(IObservable<T> observable, Presenter presenter)
		{
			this.observable = observable.Share();
			this.presenter  = presenter;
		}

		public void Silent() => observable.Subscribe().AddTo(presenter);

		public void Call(Action<T> action) => observable.Subscribe(action.Invoke).AddTo(presenter);
		public void To(Action<T>   action) => Call(action);

		public void Call(Action action) => observable.Subscribe(_ => action.Invoke()).AddTo(presenter);
		public void To(Action   action) => Call(action);

		public PresenterBindable<T> OnComplete(Action action)
		{
			observable.DoOnCompleted(action.Invoke).Subscribe().AddTo(presenter);
			return this;
		}
	}
}