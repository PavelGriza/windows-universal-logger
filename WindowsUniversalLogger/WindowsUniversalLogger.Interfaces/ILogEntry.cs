using System;

namespace WindowsUniversalLogger.Interfaces
{
    public interface ILogEntry
    {
        LogLevel LogLevel { get; set; }
        DateTime Time { get; set; }
        string Message { get; set; }
    }
}