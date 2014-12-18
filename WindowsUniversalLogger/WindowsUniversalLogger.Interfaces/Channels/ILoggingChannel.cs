using System;
using System.Threading.Tasks;

namespace WindowsUniversalLogger.Interfaces.Channels
{
    public interface ILoggingChannel : IDisposable
    {
        string Name { get; set; }
        bool IsEnabled { get; set; }
        LogLevel DetailLevel { get; set; }
        Task Init();
        Task<bool> Log(ILogEntry logEntry);
    }
}   