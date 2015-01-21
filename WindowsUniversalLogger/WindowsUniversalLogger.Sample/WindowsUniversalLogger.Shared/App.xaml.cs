using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WindowsUniversalLogger.Interfaces;
using WindowsUniversalLogger.Interfaces.Channels;
using WindowsUniversalLogger.Logging;
using WindowsUniversalLogger.Logging.Channels;
using WindowsUniversalLogger.Logging.Sessions;

namespace WindowsUniversalLogger
{
    public sealed partial class App : Application
    {
        public const string FileLoggingChannelName = "MyChannel";

#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            this.UnhandledException += OnApplicationUnhandledException;
        }

        private void OnApplicationUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LoggingSession.Instance.LogToAllChannels(
                new LogEntry(
                    LogLevel.ERROR,
                    "Exception: {0}", e.Exception));

            e.Handled = true;
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ILoggingSession session = LoggingSession.Instance;
            ILoggingChannel channel = new FileLoggingChannel(App.FileLoggingChannelName, ApplicationData.Current.LocalFolder, "logs.txt");
            await channel.Init();
            session.AddLoggingChannel(channel);
            
            await LoggingSession.Instance.LogToAllChannels(
                new LogEntry(
                    LogLevel.INFO,
                    "App is initialized"));

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.CacheSize = 1;
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}