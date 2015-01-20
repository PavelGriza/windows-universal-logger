using Windows.Storage;
using Windows.UI.Xaml.Controls;
using WindowsUniversalLogger.Interfaces.Channels;

namespace WindowsUniversalLogger.Logging.Channels
{
    public class FileLoggingChannel : FileLoggingChannelBase
    {
        public FileLoggingChannel(string channelName, IStorageFolder folder, string fileName)
            : base (channelName, folder, fileName)
        {
        }
    }
}