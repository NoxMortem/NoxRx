using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Infrastructure.NoxRx.Types
{
	public class Rn<T> : IObservable<IEnumerable<T>>, IDisposable
	{
		private readonly ReactiveProperty<IList<T>> reactiveProperty = new ReactiveProperty<IList<T>>(default);

		public void Set(T              value)  => reactiveProperty.SetValueAndForceNotify(new List<T> {value});
		public void Set(IEnumerable<T> values) => reactiveProperty.SetValueAndForceNotify(values.ToList());

		public IDisposable Subscribe(IObserver<IEnumerable<T>> observer) => reactiveProperty.Subscribe(observer);
		public void        Dispose()                                     => reactiveProperty?.Dispose();

		public RnWatch<T>     AsWatch() => new RnWatch<T>(this);
		public IEnumerable<T> Values    => reactiveProperty.Value;

		public List<T> ToList()  => reactiveProperty.Value.ToList();
		public T[]     ToArray() => reactiveProperty.Value.ToArray();
	}
}