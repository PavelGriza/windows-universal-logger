using System;
using System.IO;
using Windows.Storage;
using WindowsUniversalLogger.Interfaces;
using WindowsUniversalLogger.Logging;
using WindowsUniversalLogger.Logging.Channels;
using WindowsUniversalLogger.Logging.Sessions;
using Xunit;

namespace UnitTests
{
    public class SessionTests
    {
        [Fact]
        public void SessionInitTest()
        {
            ILoggingSession loggingSession = LoggingSession.Instance;

            loggingSession.Dispose();
        }

        [Theory]
        [InlineData(@"C:\Users\Public\Documents")]
        public async void ChannelInitTest(string path)
        {
            var fileLoggingChannel = new FileLoggingChannel("FileLoggingChannel",
                await StorageFolder.GetFolderFromPathAsync(path),
                Path.Combine("UnitTestLogs", "UnitTest.log"));

            //await fileLoggingChannel.Init();


        }

        //[Theory]
        [InlineData(@"C:\Users\Public\Documents")]
        public async void FileSessionTest(string path)
        {
            ILoggingSession loggingSession = LoggingSession.Instance;

            //var fileLoggingChannel = new FileLoggingChannel("FileLoggingChannel",
            //    await StorageFolder.GetFolderFromPathAsync(path),
            //    Path.Combine("UnitTestLogs", "UnitTest.log"));
            
            
            //await fileLoggingChannel.Init();
            
            //loggingSession.AddLoggingChannel(fileLoggingChannel);

            //for (int i = 0; i < 50; i++)
            //{
            //    await loggingSession.LogTo<FileLoggingChannel>(new LogEntry(LogLevel.INFO, "Test message"));
            //}

            loggingSession.Dispose();
        }
    }
}
