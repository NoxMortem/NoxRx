using System;
using TMPro;
using UniRx;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Infrastructure.NoxRx
{
	public abstract class Reactor : IDisposable
	{
		public readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();

		protected Bindable<T> BindNew<T>(IObservable<T> observable) => new Bindable<T>(observable);

		protected CompositeBindable<T> Bind<T>(IObservable<T> observable, CompositeDisposable disposable = null) =>
			disposable == null
				? new CompositeBindable<T>(observable, CompositeDisposable)
				: new CompositeBindable<T>(observable, disposable);

		protected CompositeBindable<int> Bind(TMP_Dropdown dropdown, CompositeDisposable disposable = null) =>
			Bind(dropdown.onValueChanged.AsObservable(), disposable);

		protected CompositeBindable<float> Bind(Slider slider, CompositeDisposable disposable = null) =>
			Bind(slider.OnValueChangedAsObservable(), disposable);

		protected CompositeBindable<bool> Bind(Toggle toggle, CompositeDisposable disposable = null) =>
			Bind(toggle.OnValueChangedAsObservable(), disposable);

		protected CompositeBindable<Unit> Bind(Button button, CompositeDisposable disposable = null) =>
			Bind(button.OnClickAsObservable(), disposable);

		public void Dispose() => CompositeDisposable?.Dispose();
	}
}