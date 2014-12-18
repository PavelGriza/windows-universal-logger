using WindowsUniversalLogger.Interfaces;
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
    }
}
