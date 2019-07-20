using System;

namespace Infrastructure.NoxRx.Types
{
	public interface ISignalObservable<out T> : IObservable<T>
	{
		IObservable<T> OnSignal();
	}
}