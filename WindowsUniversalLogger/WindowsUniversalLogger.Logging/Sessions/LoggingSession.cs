using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsUniversalLogger.Interfaces;
using WindowsUniversalLogger.Interfaces.Channels;
using WindowsUniversalLogger.Interfaces.Extensions;

namespace WindowsUniversalLogger.Logging.Sessions
{
    public class LoggingSession : ILoggingSession
    {
        #region Singleton

        private static Lazy<LoggingSession> _lazyInstance = new Lazy<LoggingSession>(() => new LoggingSession());

        public static LoggingSession Instance
        {
            get { return _lazyInstance.Value; }
        }

        private LoggingSession()
        {

        }

        #endregion

        public Dictionary<string, ILoggingChannel> LoggingChannels { get; private set; }

        public bool AddLoggingChannel(ILoggingChannel channel, string uniqueChannelName = null)
        {
            if (this.LoggingChannels == null)
            {
                this.LoggingChannels = new Dictionary<string, ILoggingChannel>();
            }

            string channelKey = string.IsNullOrWhiteSpace(uniqueChannelName) ? channel.Name : uniqueChannelName;

            if (this.LoggingChannels.ContainsKey(channelKey))
            {
                return false;
            }

            this.LoggingChannels.Add(channelKey, channel);

            return true;
        }
        
        public bool RemoveLoggingChannel(string channelName)
        {
            if (this.LoggingChannels == null || !this.LoggingChannels.ContainsKey(channelName))
            {
                return false;
            }

            this.LoggingChannels.Remove(channelName);

            return true;
        }

        public bool RemoveLoggingChannel(ILoggingChannel channel)
        {
            return this.RemoveLoggingChannel(channel.Name);
        }

        public async Task<bool> LogToAllChannels(ILogEntry logEntry)
        {
            if (this.LoggingChannels == null || !this.LoggingChannels.Any())
            {
                return false;
            }

            bool logResult = true;

            foreach (var channel in this.LoggingChannels.Values)
            {
                if (!await channel.Log(logEntry))
                {
                    logResult = false;
                }
            }

            return logResult;
        }

        public async Task<bool> LogTo(string channelName, ILogEntry logEntry)
        {
            if (string.IsNullOrWhiteSpace(channelName))
            {
                throw new ArgumentException("Value cannot be null or empty", "channelName");
            }

            if (this.LoggingChannels == null || !this.LoggingChannels.ContainsKey(channelName))
            {
                return false;
            }

            return await this.LoggingChannels[channelName].Log(logEntry);
        }

        public async Task<bool> LogTo<T>(ILogEntry logEntry) where T : class, ILoggingChannel
        {
            if (this.LoggingChannels == null || !this.LoggingChannels.Values.Any(c => c is T))
            {
                return false;
            }

            bool logResult = true;

            foreach (var channel in this.LoggingChannels.Values.Where(c => c is T))
            {
                if (!await channel.Log(logEntry))
                {
                    logResult = false;
                }
            }

            return logResult;
        }

        public void Dispose()
        {
            if (this.LoggingChannels == null || !this.LoggingChannels.Any())
            {
                return;
            }

            foreach (var channel in LoggingChannels.Values)
            {
                channel.SafeDispose();
            }
        }
    }
}
