using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Rx.UI
{
	public abstract class Presenter : MonoBehaviour
	{
		protected PresenterBindable<T> Bind<T>(IObservable<T> observable) => new PresenterBindable<T>(observable, this);

		protected PresenterBindable<int> Bind(TMP_Dropdown dropdown) =>
			Bind(dropdown.onValueChanged.AsObservable());

		protected PresenterBindable<float> Bind(Slider slider) => Bind(slider.OnValueChangedAsObservable());
		protected PresenterBindable<bool>  Bind(Toggle toggle) => Bind(toggle.onValueChanged.AsObservable());
		protected PresenterBindable<Unit>  Bind(Button button) => Bind(button.OnClickAsObservable());
	}
}