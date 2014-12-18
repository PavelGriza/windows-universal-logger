using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsUniversalLogger.Interfaces.Channels;

namespace WindowsUniversalLogger.Interfaces
{
    public interface ILoggingSession : IDisposable
    {
        Dictionary<string, ILoggingChannel> LoggingChannels { get; }

        /// <summary>
        /// Adds new channel to a session collection
        /// </summary>
        /// <param name="channel">instance of a new channel</param>
        /// <param name="uniqueChannelName">Unique key for getting channel from <see cref="LoggingChannels"/>. If null then use <see cref="ILoggingChannel.Name"/></param>
        /// <returns>A value of <see lanqword="true"/> if channel is added successfully</returns>
        bool AddLoggingChannel(ILoggingChannel channel, string uniqueChannelName = null);

        /// <summary>
        /// Removes specific channel from <see cref="LoggingChannels"/>
        /// </summary>
        /// <param name="channel">Instance of channel</param>
        /// <returns>A value of <see lanqword="true"/> if channel is removed successfully</returns>
        bool RemoveLoggingChannel(ILoggingChannel channel);

        /// <summary>
        /// Removes specific channel from collection by unique key
        /// </summary>
        /// <param name="channelName">Unique key for getting channel from <see cref="LoggingChannels"/></param>
        /// <returns>A value of <see lanqword="true"/> if channel is removed successfully</returns>
        bool RemoveLoggingChannel(string channelName);

        /// <summary>
        /// Writes log enty to all channels from <see cref="LoggingChannels"/>
        /// </summary>
        /// <param name="logEntry">Log entry</param>
        /// <returns>A value of <see langword="true"/> if each channel wrote log entry successfully</returns>
        Task<bool> LogToAllChannels(ILogEntry logEntry);

        /// <summary>
        /// Writes log entry to specific channel by unique key
        /// </summary>
        /// <param name="channelName">Unique key for getting channel from <see cref="LoggingChannels"/></param>
        /// <param name="logEntry">Log entry</param>
        /// <returns>A value of <see langword="true"/> if specific channel wrote log entry successfully</returns>
        Task<bool> LogTo(string channelName, ILogEntry logEntry);

        /// <summary>
        /// Writes log entry to specific channels by channel type
        /// </summary>
        /// <typeparam name="T">The channel type</typeparam>
        /// <param name="logEntry">Log entry</param>
        /// <returns>A value of <see langword="true"/> if each channel wrote log entry successfully</returns>
        Task<bool> LogTo<T>(ILogEntry logEntry) where T : class, ILoggingChannel;
    }
}
