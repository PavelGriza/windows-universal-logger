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
ILoggingChannel channel = new FileLoggingChannel(
    "UniqueChannelName",
    ApplicationData.Current.LocalFolder, 
    "logs.txt");
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
For logging unhandled exceptions you need to open App.xaml.cs and add envent handler for UnhandledException event.
Before using in Shared library you need to add reference to WindowsUniversalLogger.Logging library (or install from <a href="https://www.nuget.org/packages/WindowsUniversalLogger/1.0.0" target="_blank">nuget package</a>) into both of your Win and WP projects 
Here it's a simple code snippet:
```c#
/// ...
using WindowsUniversalLogger.Interfaces;
using WindowsUniversalLogger.Interfaces.Channels;
using WindowsUniversalLogger.Logging;
using WindowsUniversalLogger.Logging.Channels;
using WindowsUniversalLogger.Logging.Sessions;

public sealed partial class App : Application
{
	public App()
	{
		this.InitializeComponent();
		this.UnhandledException += OnApplicationUnhandledException;

		// ...
	}
	
	protected async override void OnLaunched(LaunchActivatedEventArgs e)
	{
		ILoggingSession session = LoggingSession.Instance;
		ILoggingChannel channel = new FileLoggingChannel(
			"UniqueChannelName", 
			ApplicationData.Current.LocalFolder, 
			"logs.txt");
		await channel.Init();
		session.AddLoggingChannel(channel);

		await LoggingSession.Instance.LogToAllChannels(
			new LogEntry(
				LogLevel.INFO,
				"App is initialized"));
		
		// ...
	}

	private void OnApplicationUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		LoggingSession.Instance.LogToAllChannels(
			new LogEntry(
				LogLevel.ERROR,
				"Exception: {0}", e.Exception));

		e.Handled = true;
	}

	// ...
}
```

How to run unit tests
---
> Read how to <a href="http://xunit.github.io/docs/getting-started.html#run-tests" target="_blank">run tests with the xUnit.net console runner</a>

Install from NuGet
---
> Watch at <a href="https://www.nuget.org/packages/WindowsUniversalLogger/1.0.0" target="_blank">Windows Universal App Logger</a>

License
---
WindowsUniversalLogger is licensed under <a href="http://www.apache.org/licenses/LICENSE-2.0" target="_blank" >License Apache 2.0 License</a>. Refer to license file for more information.
