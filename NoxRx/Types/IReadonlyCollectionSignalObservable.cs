using System;
using System.Collections.Generic;

namespace Infrastructure.NoxRx.Types
{
	// IReadonlyList<T>
	public interface IReadonlyCollectionSignalObservable<out T, out TRc> : IObservable<TRc>
	where TRc : IReadOnlyCollection<T>
	{
		IObservable<TRc> OnSignal();
	}

	// IReadonlyDictionary<TK,TV>
	public interface IReadonlyCollectionSignalObservable<TK, TV, out TRc> : IObservable<TRc>
	where TRc : IReadOnlyCollection<KeyValuePair<TK, TV>>
	{
		IObservable<TRc> OnSignal();
	}
}