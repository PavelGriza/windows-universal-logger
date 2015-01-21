using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsUniversalLogger.Interfaces.Channels;
using WindowsUniversalLogger.Logging.Sessions;

namespace WindowsUniversalLogger
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        
        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null)
            {
                this.DataContext.ToString(); // throw NullReferenceException
            }
        }

        private async void OnLogButtonClick(object sender, RoutedEventArgs e)
        {
            var channel =
                LoggingSession.Instance.LoggingChannels[App.FileLoggingChannelName] as IFileLoggingChannel;

            this.LogTextBox.Text = await FileIO.ReadTextAsync(channel.LogFile);
        }

        private async void OnClearLogButtonClick(object sender, RoutedEventArgs e)
        {
            var channel =
                LoggingSession.Instance.LoggingChannels[App.FileLoggingChannelName] as IFileLoggingChannel;

            await channel.LogFile.DeleteAsync();
            this.LogTextBox.Text = string.Empty;
        }
    }
}
