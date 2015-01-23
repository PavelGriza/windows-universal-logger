using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using WindowsUniversalLogger.Extensions;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace WindowsUniversalLogger.Interfaces.Channels
{
    public class FileLoggingChannelBase : IFileLoggingChannel
    {
        private SemaphoreSlim _fileLock = new SemaphoreSlim(1);
        private string _fileName;

        protected FileLoggingChannelBase(string channelName, IStorageFolder folder, string fileName)
        {
            if (string.IsNullOrWhiteSpace(channelName))
            {
                throw new ArgumentException("Variable must be initialized", "channelName");
            }

            this.Name = channelName;

            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            this.Folder = folder;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Variable must be initialized", "fileName");
            }

            _fileName = fileName;

            this.IsEnabled = true;
            this.DetailLevel = LogLevel.INFO;
            this.MaxFileSize = 51200;
        }

        public IStorageFolder Folder { get; private set; }
        public IStorageFile LogFile { get; private set; }
        public ulong MaxFileSize { get; set; }

        public async Task<bool> ChangeLoggingFolder(IStorageFolder folder)
        {
            return await this.ChangeLoggingFolder(folder, _fileName);
        }

        public async Task<bool> ChangeLoggingFolder(IStorageFolder folder, string fileName)
        {
            this.Folder = folder;
            _fileName = fileName;
            await Init();

            return true;
        }

        public event EventHandler<EventArgs> LoggingFolderChanged;

        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public LogLevel DetailLevel { get; set; }

        public async Task Init()
        {
            this.LogFile = await Folder.CreateFileAsync(_fileName, CreationCollisionOption.OpenIfExists);
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
                if (!await this.LogFile.Exists())
                {
                    await Init();
                }

                var currentFileSize = await this.LogFile.GetFileSize();

                if (currentFileSize > this.MaxFileSize)
                {
                    await FileIO.WriteTextAsync(this.LogFile, sb.ToString(), UnicodeEncoding.Utf8);
                }
                else
                {
                    await FileIO.AppendTextAsync(this.LogFile, sb.ToString(), UnicodeEncoding.Utf8);
                }
            }
            catch (Exception e)
            {
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

        }
    }
}