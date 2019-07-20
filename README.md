# NoxRx
Small plugin for base classes reacting to IObservables using [UniRx](https://github.com/neuecc/UniRx).


# Features
* Provides two base classes for UniRx: Reactor and Presenter which provide functionality which was removed from UniRx some versions ago.
* Short fluent API
* Supports Unity.TextMeshPro
* Supports Unity UI Elements (TextField, Slider, Toggle, ...)

## Usage
These base classes can be used as architecture cornerstones in a reactive application:
* All Controllers and Systems inherit from Reactor and while providing reactive interfaces themself, which then are bound by their clients
* All Views inherit from Presenter which allows to react to Controllers

```
public class MyClass : Reactor		// Non-MonoBehaviour
public class MyClass : Presenter 	// MonoBehaviour

// When IObservable emits the MethodGroup will be called
var subject = new Subject(); 		// Some IObservable
Bind(subject).To(MethodGroup);		// Short API

// When IObservable<Unit> emits SomeMethod(unit) will be called
var subject = new Subject<Unit>(); 			// Some IObservable<Unit>
Bind(subject).To(unit => SomeMethod(unit));	// SomeMethod(Unit unit);
```

## Rx{List|Map}

These classes provide implementations for IList<T> and IDictionar<TK,TV> and provide operation granularity observables (Add, Remove, ...).
Use them as a replacement for List<T> and Dictionary<TK,TV> whenever you require to subscribe to operations on those.
```
private readonly RxList<int> values = new RxList<int>();
private readonly RxMap<int,int> values = new RxMap<int,int>();
```

## Rx{List|Map}{Watch|Signal}[ReadOnly]

These classes allow to expose a different observable (e.g. a read only view on a mutable list) as the one used internally.
```
private readonly RxList<int> values = new RxList<int>();
public readonly  RxListSignalReadOnly<int> ValuesChanged => values.AsSignal().AsReadOnly();
```

* A watch exposes fine granularity on operation level observables and all underlying methods will propagate elements.
* A signal exposes the more general observer pattern approach and allows ot do many changes and signal only once. A signal will never fire on it's own and needs to be triggered manually via `Signal()`.

# Installation
* Import the noxrx-version.unitypackage
* Move the folder NoxRx/* folder to any assembly linked to any other you intend to use it from or create an asmdef and reference it
  * e.g. Plugins/* if you do not use assembly definitions

# Dependencies
* [UniRx](https://github.com/neuecc/UniRx)
* Unity.TextMeshPro

# Disclaimer
This project is intented for personal use only at the moment and is provided AS IS. Use it at your own risk.
If others find it helpful - great, if not, just as good.