using System;
using System.Collections.Generic;

namespace Infrastructure.NoxRx.Types
{
	public class RnWatch<T> : IObservable<IEnumerable<T>>
	{
		private readonly Rn<T> rn;

		public RnWatch(Rn<T> rn) => this.rn = rn;

		public IDisposable Subscribe(IObserver<IEnumerable<T>> observer) => rn.Subscribe(observer);
	}
}