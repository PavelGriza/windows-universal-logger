WindowsUniversalLogger
===

The simple and light-weight logging tool for <a href="http://msdn.microsoft.com/en-us/library/windows/apps/dn609832.aspx" target="_blank">Windows Universal Applications</a>. It inherits some principals which are presented in <a href="http://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.foundation.diagnostics.aspx" target="_blank">Windows.Foundation.Diagnostics</a> namespace. In development we used <a href="https://github.com/Injac/ExGrip.WinRT.Logging" target="_blank">ExGrip.WinRT.Logging</a> solution because of interesting ideas.

---

Dependencies
---
- <a href="http://xunit.github.io/" target="_blank">xUnit</a> - unit test framework

How it works
---
First of all you need to have <b>ILoggingSession</b> instance. You may implement your own session object or use <b>WindowsUniversalLogger.Logging.Sessions.LoggingSession</b> as a singleton:
```c#
ILoggingSession session = LoggingSession.Instance;
```
Then you need to instantiate one or several <b>ILoggingChannel</b> objects:
```c#
ILoggingChannel channel = new FileLoggingChannel("MyChannel");
await channel.Init();
session.AddLoggingChannel(channel);
```
For writing logging message you need to create ILoggingEntry instance:
```c#
await LoggingSession.Instance.LogToAllChannels(
	new LogEntry(
		LogLevel.INFO,
		"App is initialized"));
```

How to logging unhandled exception in Windows Universal Application
---
Coming soon...

License
---
WindowsUniversalLogger is licensed under <a href="http://www.apache.org/licenses/LICENSE-2.0" target="_blank" >License Apache 2.0 License</a>. Refer to license file for more information.
