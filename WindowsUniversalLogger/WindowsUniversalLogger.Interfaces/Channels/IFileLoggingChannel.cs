using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace WindowsUniversalLogger.Interfaces.Channels
{
    public interface IFileLoggingChannel : ILoggingChannel
    {
        string FileName { get; set; }
        string LocalFolderPath { get; }
        IStorageFolder RootFolder { get; }
        IStorageFolder LoggingFoler { get; }
        IStorageFile LoggingFile { get; }
        ulong MaxFileSize { get; set; }

        Task<bool> ChangeLoggingFolder(IStorageFolder rootFolder, string localFolderPath);

        event EventHandler<EventArgs> LoggingFolderChanged;
    }
}
