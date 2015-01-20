using System;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace WindowsUniversalLogger.Interfaces.Channels
{
    public interface IFileLoggingChannel : ILoggingChannel
    {
        IStorageFolder Folder { get; }
        IStorageFile LogFile { get; }
        ulong MaxFileSize { get; set; }

        Task<bool> ChangeLoggingFolder(IStorageFolder folder);

        event EventHandler<EventArgs> LoggingFolderChanged;
    }
}
