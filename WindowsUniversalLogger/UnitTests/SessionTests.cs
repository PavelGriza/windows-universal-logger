using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.System.Threading;
using WindowsUniversalLogger.Interfaces;
using WindowsUniversalLogger.Interfaces.Channels;
using WindowsUniversalLogger.Logging;
using WindowsUniversalLogger.Logging.Channels;
using WindowsUniversalLogger.Logging.Sessions;
using Xunit;

namespace UnitTests
{
    public class SessionTests
    {
        [Theory]
        [InlineData(@"C:\Users\Public\Documents")]
        public async void FileSessionTest(string path)
        {
            string logFileName = "Session.log";

            ILoggingSession loggingSession = LoggingSession.Instance;

            var channel = new FileLoggingChannel("TestChannel", await StorageFolder.GetFolderFromPathAsync(path),logFileName)
            {
                DetailLevel = LogLevel.INFO,
                IsEnabled = true,
                MaxFileSize = 102400 //1000 KB
            };
            await channel.Init();

            loggingSession.AddLoggingChannel(channel);

            for (int i = 0; i < 500; i++)
            {
                await loggingSession.LogTo<IFileLoggingChannel>(new LogEntry(LogLevel.INFO, "Test message"));
            }

            loggingSession.Dispose();
        }
    }
}
