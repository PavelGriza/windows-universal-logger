using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using WindowsUniversalLogger.Interfaces.Extensions;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace WindowsUniversalLogger.Interfaces.Channels
{
    public class FileLoggingChannelBase : IFileLoggingChannel
    {
        private SemaphoreSlim _fileLock = new SemaphoreSlim(1);

        protected FileLoggingChannelBase(string channelName)
        {
            if (string.IsNullOrWhiteSpace(channelName))
            {
                throw new ArgumentException("Variable must be initialized", "channelName");
            }

            this.Name = channelName;

            this.FileName = "logs.txt";
            this.LocalFolderPath = "Logs";
            this.RootFolder = ApplicationData.Current.LocalFolder;
            this.DetailLevel = LogLevel.INFO;
            this.IsEnabled = true;
        }


        public string FileName { get; set; }
        public string LocalFolderPath { get; private set; }
        public IStorageFolder RootFolder { get; private set; }
        public IStorageFolder LoggingFoler { get; private set; }
        public IStorageFile LoggingFile { get; private set; }
        public ulong MaxFileSize { get; set; }

        public async Task<bool> ChangeLoggingFolder(IStorageFolder rootFolder, string localFolderPath)
        {
            this.RootFolder = rootFolder;
            this.LocalFolderPath = localFolderPath;

            this.LoggingFoler = await rootFolder.GetOrCreateFolderAsync(localFolderPath);

            // todo fire event
            //throw new NotImplementedException();

            return true;
        }

        public event EventHandler<EventArgs> LoggingFolderChanged;

        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public LogLevel DetailLevel { get; set; }

        public async Task Init()
        {
            if (string.IsNullOrWhiteSpace(this.FileName))
            {
                throw new ArgumentException("Property must be initialized", "FileName");
            }

            this.LoggingFoler = await this.RootFolder.GetOrCreateFolderAsync(this.LocalFolderPath);
            this.LoggingFile = await this.LoggingFoler.CreateFileAsync(this.FileName, CreationCollisionOption.OpenIfExists);
        }

        public async Task<bool> Log(ILogEntry logEntry)
        {
            if (logEntry == null)
            {
                throw new ArgumentNullException("logEntry");
            }

            if (!this.IsEnabled)
            {
                return false;
            }

            var currentFileSize = await this.LoggingFile.GetFileSize();
            var availableSpace = await this.RootFolder.GetFreeSpace();
            
            // todo check is currentFileSize less then MaxFileSize

            var sb = new StringBuilder();
            sb.Append(logEntry.LogLevel)
                .Append('\t')
                .Append(logEntry.Time.ToString("O"))
                .Append('\t')
                .Append(logEntry.Message)
                .AppendLine();

            await _fileLock.WaitAsync();

            try
            {
                await FileIO.AppendTextAsync(this.LoggingFile, sb.ToString(), UnicodeEncoding.Utf8);
            }
            catch (Exception e)
            {
                // todo: handle
                return false;
            }
            finally
            {
                _fileLock.Release();
            }

            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}