# NoxLog
Simple logger for Unity with support for Console Pro and Zenject features.

It's main goal is to provide convenient logging in unity.

# Features
* Supports [Editor Console Pro Documentation](http://flyingworm.com/) filters
* Supports [Zenject](https://github.com/modesttree/Zenject) bindings incl. LogLevel override per class
* Provides instance and singleton (static logger) based logging
* Classic loglevel based
* Provides optional extensions for this.*

## Usage
```
this.logger.Trace($"Option 1: {name}");
// Option 2: Use the copy of the default logger and set the context manually
logger.SetContext(this); 			// Adds #this.GetType().Name# as filter for Editor Console Pro
logger.SetLogLevel(LogLevel.Debug);	// Overwrite the LogLevel manually if you do not use Container.BindLogger<T>(LogLevel) on DI level
logger.Trace($"Option 2: {name}");
// Option 3: Use the static Logger with a) using Logger = Infrastructure.NoxLog.Logger; or b) specify the full namespace Infrastructure.NoxLog.Logger
Logger.StaticLogger.SetContext("Example"); 				// Optionally redefine the context
Logger.StaticLogger.SetLogLevel(LogLevel.Trace); 		// Optionally redefine the loglevel
Logger.LogTrace($"Option 3.1: Without context {name}");
Logger.LogTrace(this, $"Option 3.2: With context {name}");
// Option 4: use the extension method for the static logger. Requires LOGGER_EXTENSIONS
this.LogTrace("Inject 5");
"Example".LogTrace("Inject 6"); // If this is a string, it will directly be picked up as a filter for Editor Console Pro, e.g. #Example#
```

## LogLevels
```
public enum LogLevel
{
	Trace,
	Debug,
	Info,
	Warning,
	Error,
	None
}
```

# Installation
* Import the noxlog-<version>.unitypackage
* Move the folder NoxLog/* folder to any assembly linked to any other you intend to use logging 
  * e.g. Plugins/* if you do not use assembly definitions

# Dependencies
```
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

#if ZENJECT
using Zenject;
#endif
```

# (Optional) 3rd Party Support

## Editor Console Pro
Please see the [Editor Console Pro Documentation](http://flyingworm.com/) for all the features this improved console provides.

This simple logger utilizes [Editor Console Pro](https://assetstore.unity.com/packages/tools/utilities/editor-console-pro-11889) filters.
Each class name ("context") is used as a temporary filter.

### Recommended Stack Trace Ignores
Editor Console Pro supports ignoring stack trace ignores. Adding [Zenject](https://github.com/modesttree/Zenject), [UniRx](https://github.com/neuecc/UniRx) and NoxLog to the ignores,
 will in most cases help you see your own code instead of wrapper functions. Be aware that this makes it more difficult to find bugs in those 3rd party libraries.

![editor-console-pro-recommended-ignores.PNG](https://raw.githubusercontent.com/NoxMortem/NoxLog/master/docs/images/editor-console-pro-recommended-ignores.PNG)

## Zenject
This simple logger is shipped with an example Installer for [Zenject](https://github.com/modesttree/Zenject).
I highly recommend Zenject in general, and even this small logger shows some nice things you can easily eachieve with a good DI Container.
With the Zenject Installer you can request both the `defaultLogger`, the `Logger.Factory` and create your own Factory or request a `Logger`
and override the `LogLevel` via a new specific Binding.

Even if you do not use `Zenject` or a different DI container, you still can use this logger or it's singleton variant
```
// The context can be any object and the type name will be used as filter, s.t. all logs from `class C` will end up in the Editor Console Pro Filter #C#
// or it can be a string used as filter.
// public Logger(LogLevel logLevel = LogLevel.Trace)
// public Logger(object context = null, LogLevel logLevel = LogLevel.Trace)
this.logger = new Logger(this, LogLevel.Debug);
this.logger = new Logger(LogLevel.Debug);
this.logger = new Logger(this);
this.logger = new Logger();
```

### Examples: Zenject Installation & Configuration
Please see the [Zenject#Installers](https://github.com/modesttree/Zenject#Installers) documentation about how to use custom installers in the first place.
```
// Install the default bindings
Infrastructure.NoxLog.Installer.Install(Container);
// Bind the generic logger used for all classes with it's default log level
// Requires LOGGER_EXTENSIONS`
Container.BindLogger(LogLevel.Trace);
// Overwrite the LogLevel for a specific class T
Container.BindLogger<T>(LogLevel.Debug);
```

### Zenject Usage
```
// Examples
[Inject, UsedImplicitly]
public void Inject(Logger.Factory logging, Logger logger)
{
	// Option 1: Inject Logger.Factory to create a logger with this context and a specific LogLevel
	this.logger = logging.Create(this, LogLevel.Trace);
	// Option 2: Inject a copy of the default logger
	this.logger = logger;
}
```

# Best Practices
* Do *not* use the static Logger.LogTrace(msg) or this.LogTrace(msg) or "Example".LogTraec(msg).
  These are convenience functions to provide quick fully featured logging during tracer bullet style logging used commonly for debugging
* **Do** use constructor injection for the Logger.
  * If you do not want to use any DI framework, inject the logger manually. This allows to inject a different logger during testing, e.g. one which does not log.
  * If you do not want to inject the logger at all, at least create your logger per base class and inherit it.
* The usage of Editor Console Pro or any other console replacement for unity which allows temporary filters is **highly** recommended. It should be quite easy to change how the filter is appended in Logger.cs if you use a different console.

# Supported Scripting Defines
* `ENABLE_LOGGER`
  * Add this to enable logging, remove this to disable the call to all logger methods with 0% performance impact (`[Conditiona("ENABLE_LOGGER")]`).
* `ZENJECT`
  * Add this to enable optional Zenject support. This 
* `LOGGER_EXTENSIONS`
  * Add this to enable this.LogTrace using the static logger on any object
  * If used together with `ZENJECT` also adds Container.BindLogger<T>(LogLevel) which will set the LogLevel for a Logger injected into class T.
    This is very handy to override the log level for a specific class. e.g. `Container.BindLogger<LayerView>(LogLevel.Trace);` will set `LogLevel.Trace` for a `Logger` injected into `class LayerView`.

# Disclaimer
This project is intented for personal use only at the moment and is provided AS IS. Use it at your own risk.
If others find it helpful - great, if not - there are likely other similar and [more ambitious projects](https://stackify.com/nlog-vs-log4net-vs-serilog/) out there.